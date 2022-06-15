using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using MMR.DiscordBot.Data.Entities;
using MMR.DiscordBot.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MMR.DiscordBot.Services
{
    public class AsyncGenerationService
    {
        private static string[] Scopes = { SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive };

        private readonly DriveService _driveService;
        private readonly SheetsService _sheetsService;
        private readonly MMRService _mmrService;
        private readonly AsyncSheetRepository _asyncSheetRepository;

        public AsyncGenerationService(MMRService mmrService, AsyncSheetRepository asyncSheetRepository)
        {
            _mmrService = mmrService;
            _asyncSheetRepository = asyncSheetRepository;

            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MMR.DiscordBot",
            });

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "MMR.DiscordBot",
            });
        }

        public async Task<(string sheetLink, DateTime? oldSheetDate)> GenerateAsyncSheet(Func<string, Task> reportProgress)
        {
            DateTime? oldSheetDate = null;
            var asyncSheet = await _asyncSheetRepository.GetLatest();
            var oldSheetFileId = asyncSheet.SheetId;

            await reportProgress("Locking old sheet.");

            // lock permissions on old sheet (anyone with link can View instead of can Edit)
            var updatePermissionRequest = _driveService.Permissions.Update(new Google.Apis.Drive.v3.Data.Permission()
            {
                Role = "reader",
            }, oldSheetFileId, "anyoneWithLink");
            await updatePermissionRequest.ExecuteAsync();

            await reportProgress("Making old spoiler logs public.");

            // enable sharing on old spoiler logs
            var oldFolderId = asyncSheet.FolderId;
            var listRequest = _driveService.Files.List();
            listRequest.Q = $"'{oldFolderId}' in parents and mimeType = 'text/plain'";
            listRequest.OrderBy = "name_natural";
            listRequest.Fields = "files(id,createdTime)";
            var oldSpoilerLogs = await listRequest.ExecuteAsync();
            foreach (var oldSpoilerLog in oldSpoilerLogs.Files)
            {
                oldSheetDate = oldSpoilerLog.CreatedTime;
                await _driveService.Permissions.Create(new Google.Apis.Drive.v3.Data.Permission()
                {
                    Type = "anyone",
                    Role = "reader",
                    AllowFileDiscovery = false,
                }, oldSpoilerLog.Id).ExecuteAsync();
            }

            await reportProgress("Publishing old spoiler logs in the spreadsheet.");

            // add links to old spoiler logs to old sheet
            var updateOldSheetRequest = _sheetsService.Spreadsheets.Values.Update(new Google.Apis.Sheets.v4.Data.ValueRange()
            {
                MajorDimension = "COLUMNS",
                Values = new List<IList<object>>
                {
                    oldSpoilerLogs.Files.Select(f => $"https://drive.google.com/open?id={f.Id}").Cast<object>().ToList()
                }
            }, oldSheetFileId, "SpoilerLogs");
            updateOldSheetRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            await updateOldSheetRequest.ExecuteAsync();

            // generate 10 seeds
            var seeds = (await _mmrService.GetSeed(10)).ToList();
            var driveFolderName = DateTime.UtcNow.ToString("yyyy-MM");
            var filenames = new List<(string patchPath, string hashIconPath, string spoilerLogPath)>();
            for (var i = 0; i < seeds.Count; i++)
            {
                await reportProgress($"Generating new seeds: {i + 1}/{seeds.Count}.");

                var seed = seeds[i];
                var filenameWithoutExtension = $"{driveFolderName}_{i.ToString("00")}";
                filenames.Add(await _mmrService.GenerateSeed(filenameWithoutExtension, null, seed));
            }

            await reportProgress("Creating new Drive folder.");

            // upload all outputted files to drive
            var createFolderRequest = _driveService.Files.Create(new Google.Apis.Drive.v3.Data.File()
            {
                Name = driveFolderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { "1xhY7A0FPHnt3Xx2rmB6S_P00Rl2o0QcS" }
            });
            createFolderRequest.Fields = "id";
            var driveFolder = await createFolderRequest.ExecuteAsync();
            var patchFileIds = new List<string>();
            var hashIconIds = new List<string>();
            var progress = 1;
            foreach ((var patchPath, var hashIconPath, var spoilerLogPath) in filenames)
            {
                await reportProgress($"Uploading files to Drive: {progress}/{filenames.Count}");

                // todo check upload statuses
                using (var stream = File.OpenRead(patchPath))
                {
                    var createRequest = _driveService.Files.Create(new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = Path.GetFileName(patchPath),
                        Parents = new List<string> { driveFolder.Id }
                    }, stream, "application/x-gzip");
                    createRequest.Fields = "id";
                    await createRequest.UploadAsync();
                    await _driveService.Permissions.Create(new Google.Apis.Drive.v3.Data.Permission()
                    {
                        Type = "anyone",
                        Role = "reader",
                        AllowFileDiscovery = false,
                    }, createRequest.ResponseBody.Id).ExecuteAsync();

                    patchFileIds.Add(createRequest.ResponseBody.Id);
                }
                using (var stream = File.OpenRead(hashIconPath))
                {
                    var createRequest = _driveService.Files.Create(new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = Path.GetFileName(hashIconPath),
                        Parents = new List<string> { driveFolder.Id }
                    }, stream, "image/png");
                    createRequest.Fields = "id";
                    await createRequest.UploadAsync();
                    await _driveService.Permissions.Create(new Google.Apis.Drive.v3.Data.Permission()
                    {
                        Type = "anyone",
                        Role = "reader",
                        AllowFileDiscovery = false,
                    }, createRequest.ResponseBody.Id).ExecuteAsync();

                    hashIconIds.Add(createRequest.ResponseBody.Id);
                }
                using (var stream = File.OpenRead(spoilerLogPath))
                {
                    var createRequest = _driveService.Files.Create(new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = Path.GetFileName(spoilerLogPath),
                        Parents = new List<string> { driveFolder.Id }
                    }, stream, "text/plain");
                    createRequest.Fields = "id";
                    await createRequest.UploadAsync();
                }
                progress++;
            }

            await reportProgress("Creating new sheet.");

            // copy template spreadsheet
            var copiedFile = new Google.Apis.Drive.v3.Data.File();
            copiedFile.Name = $"MMR - {DateTime.UtcNow.ToString("MMMM yyyy")} Asynchronous Races Leaderboard";
            var createNewSheetRequest = _driveService.Files.Copy(copiedFile, "1CyIdGK7P2iEsTWSBWhBwZFH1enKuCe-gIytQg7SFZFI");
            createNewSheetRequest.Fields = "id";
            var newSheet = await createNewSheetRequest.ExecuteAsync();

            await reportProgress("Updating new sheet details.");

            // add date, settings, patches, iconhashes to new sheet
            var updateNewSheetRequest = _sheetsService.Spreadsheets.Values.BatchUpdate(new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest()
            {
                Data = new List<Google.Apis.Sheets.v4.Data.ValueRange>
                {
                    new Google.Apis.Sheets.v4.Data.ValueRange()
                    {
                        Range = "Date",
                        Values = new List<IList<object>>
                        {
                            new List<object> { DateTime.UtcNow.ToString("yyyy/MM/dd") }
                        }
                    },
                    new Google.Apis.Sheets.v4.Data.ValueRange()
                    {
                        Range = "IconHashes",
                        MajorDimension = "COLUMNS",
                        Values = new List<IList<object>>
                        {
                            hashIconIds.Select(s => $"=IMAGE(\"https://drive.google.com/uc?export=view&id={s}\", 2)").Cast<object>().ToList()
                        }
                    },
                    new Google.Apis.Sheets.v4.Data.ValueRange()
                    {
                        Range = "Patches",
                        MajorDimension = "COLUMNS",
                        Values = new List<IList<object>>
                        {
                            patchFileIds.Select(s => $"https://drive.google.com/uc?export=download&id={s}").Cast<object>().ToList()
                        }
                    },
                    // todo settings
                    //new Google.Apis.Sheets.v4.Data.ValueRange()
                    //{
                    //    Range = "Settings",
                    //    Values = new List<IList<object>>
                    //    {
                    //        new List<object> { DateTime.UtcNow.ToString("yyyy/MM/dd") }
                    //    }
                    //}
                },
                ValueInputOption = "USER_ENTERED",
            }, newSheet.Id);
            await updateNewSheetRequest.ExecuteAsync();

            await reportProgress("Making new sheet public.");

            // enable sharing on new sheet (anyone with link can Edit)
            await _driveService.Permissions.Create(new Google.Apis.Drive.v3.Data.Permission()
            {
                Type = "anyone",
                Role = "writer",
                AllowFileDiscovery = false,
            }, newSheet.Id).ExecuteAsync();

            await _asyncSheetRepository.Save(new AsyncSheetEntity
            {
                SheetId = newSheet.Id,
                DateCreated = DateTime.UtcNow,
                FolderId = driveFolder.Id,
            });

            return ($"https://docs.google.com/spreadsheets/d/{newSheet.Id}/edit?usp=sharing", oldSheetDate);
        }
    }
}
