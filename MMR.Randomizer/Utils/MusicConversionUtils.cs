using MMR.Randomizer.Constants;
using MMR.Randomizer.Models.Rom;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using YamlDotNet.Serialization;
using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using YamlDotNet.Serialization.EventEmitters;

namespace MMR.Randomizer.Utils
{
    public class MusicConversionUtils
    {
        // Process outline:
        // Step 1: Check for old music files
        // Step 2: Create backup of music folder
        // Step 3: Convert music files

        public static List<string> OLD_MUSIC_FILES = new();

        public class FlowStyleListEmitter : ChainedEventEmitter
        {
            public FlowStyleListEmitter(IEventEmitter nextEmitter) : base(nextEmitter) { }

            public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
            {
                eventInfo.Style = SequenceStyle.Flow;
                base.Emit(eventInfo, emitter);
            }
        }

        // fanfare categories to ensure correct song type
        private static readonly string[] FANFARE_CATEGORIES =
        {   //groups
            "ItemFanfares",
            "EventFanfares",
            "ClearFanfares",

            // individual
            "EventFail1",
            "EventFail2",
            "EventSuccess",
            "GameOver",
            "BossDefeated",
            "ItemGet",
            "HeartContainerGet",
            "MaskGet",
            "HeartPieceGet",
            "TruthRevealed",
            "GoronRaceWin",
            "HorseRaceWin",
            "SongGet",
            "SoaringTheme",
            "TempleAppears",
            "TempleClearShort",
            "TempleClearLong",
            "GiantsLeave",
            "MoonDestroyed",
        };

        public static void BackupMusicFolder(string folder)
        {
            // backs up the music folder into a zip file with the .old extension
            // all the files are copied, but one can never be too careful

            string folderName = Path.GetFileName(folder);
            string tempZipFolder = Path.Combine(Path.GetTempPath(), $"mmr_music_folder_{Guid.NewGuid()}");
            string finalBackupPath = Path.Combine(folder, $"music.old");

            if (File.Exists(finalBackupPath))
                File.Delete(finalBackupPath);

            ZipFile.CreateFromDirectory(folder, tempZipFolder, CompressionLevel.Optimal, includeBaseDirectory: false);
            
            File.Move(tempZipFolder, finalBackupPath);

            if (Directory.Exists(tempZipFolder))
                Directory.Delete(tempZipFolder);
        }

        public static void ConvertMusicFiles()
        {
            // backup the music folder, convert files, delete the original directory, rename converted
            // directory structure is maintained, and all non-music files are copied as well

            var convFolder = Path.Combine(Path.GetDirectoryName(Values.MusicDirectory), "converted");
            if (Directory.Exists(Values.MusicDirectory)) // This isn't needed, but keeping it just in case
            {
                try
                {
                    ProcessFiles(Values.MusicDirectory, convFolder);
                }
                catch
                {
                    if (Directory.Exists(convFolder))
                        Directory.Delete(convFolder, true);
                }

                Directory.Delete(Values.MusicDirectory, true);
                Directory.Move(convFolder, Values.MusicDirectory);
            }
        }

        public class StandaloneSequence
        {
            public string Filename { get; set; }
            public string InstrumentSet { get; set; }
            public string[] Categories { get; set; }
            public string TempFolder { get; set; }

            public StandaloneSequence(string filename)
            {
                TempFolder = Path.Combine(Path.GetTempPath(), $"zseq_conversion_{Guid.NewGuid()}");
                Directory.CreateDirectory(TempFolder);

                ParseFilename(filename);
            }

            public void ParseFilename(string filename)
            {
                string basename = Path.GetFileName(filename);
                string nameNoExt = basename.Replace(".zseq", "");

                string[] parts = nameNoExt.Split('_');

                if (parts.Length != 3)
                    throw new Exception("Invalid filename format.");

                Filename = parts[0];
                InstrumentSet = parts[1];
                Categories = parts[2].Split('-');
            }

            public void Copy(string filepath)
            {
                // copies the sequence into its temp directory
                
                //if (File.Exists(Filename + ".zip"))
                //    File.Delete(Filename + ".zip");

                //if (File.Exists(Filename + ".mmrs"))
                //    File.Delete(Filename + ".mmrs");

                string tempSeqFilePath = Path.Combine(TempFolder, Filename + ".seq");

                using (var src = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                using (var dst = new FileStream(tempSeqFilePath, FileMode.Create, FileAccess.Write))
                {
                    src.CopyTo(dst);
                }

                if (File.Exists(filepath))
                    File.Delete(filepath);
            }

            public void Pack(string filename, string destinationDir)
            {
                // packs the sequence into a new archive
                
                string archivePath = Path.Combine(destinationDir, filename);

                ZipFile.CreateFromDirectory(TempFolder, $"{archivePath}.zip", CompressionLevel.Optimal, false);

                string zipFilePath = $"{archivePath}.zip";
                string mmrsFilePath = $"{archivePath}.mmrs";

                if (File.Exists(zipFilePath))
                    File.Move(zipFilePath, mmrsFilePath);

                if (Directory.Exists(TempFolder))
                    Directory.Delete(TempFolder, true);
            }
        }

        public class MusicArchive
        {
            public List<(string Base, string Extension)> Sequences { get; set; } = new();
            public string Categories { get; set; }
            public Dictionary<string, (string ZBank, string BankMeta)> Banks { get; set; } = new();
            public Dictionary<string, string> FormMasks { get; set; } = new();
            public Dictionary<string, uint> ZSounds { get; set; } = new();
            public string TempFolder { get; set; }

            private static readonly string[] SEQ_EXTS = new[] { ".seq", ".zseq", ".aseq" };

            public MusicArchive(bool skipTempCreate = false)
            {
                if (!skipTempCreate)
                {
                    TempFolder = Path.Combine(Path.GetTempPath(), $"mmrs_conversion_{Guid.NewGuid()}");
                    Directory.CreateDirectory(TempFolder);
                }
            }

            public void Unpack(string filename, string filePath)
            {
                // unpacks the music file into its temp directory
                
                if (Directory.Exists(TempFolder))
                    Directory.Delete(TempFolder, recursive: true);

                //if (File.Exists(filename + ".zip"))
                //    File.Delete(filename + ".zip");

                //if (File.Exists(filename + ".mmrs"))
                //    File.Delete(filename + ".mmrs");

                ZipFile.ExtractToDirectory(filePath, TempFolder);

                foreach (var f in Directory.GetFiles(TempFolder))
                {
                    var fileName = Path.GetFileName(f);
                    var ext = Path.GetExtension(fileName).ToLowerInvariant();

                    if (SEQ_EXTS.Contains(ext))
                    {
                        var baseName = Path.GetFileNameWithoutExtension(f);
                        Sequences.Add((baseName, ext));
                        continue;
                    }

                    if (ext == ".zbank")
                    {
                        var baseName = Path.GetFileNameWithoutExtension(fileName);
                        var zbankPath = fileName;
                        var bankMetaPath = $"{baseName}.bankmeta";

                        if (File.Exists(Path.Combine(TempFolder, bankMetaPath)))
                        {
                            Banks[baseName] = (zbankPath, bankMetaPath);
                        }
                        else
                        {
                            throw new FileNotFoundException($"Missing bankmeta for {zbankPath}!");
                        }
                        continue;
                    }

                    if (ext == ".bankmeta")
                        continue;

                    if (fileName == "categories.txt")
                    {
                        Categories = fileName;
                        continue;
                    }

                    if (ext == ".formmask")
                    {
                        var baseName = Path.GetFileNameWithoutExtension(fileName);
                        FormMasks[baseName] = fileName;
                        continue;
                    }

                    if (ext == ".zsound")
                    {
                        var split = fileName.Split(new[] { ".zsound" }, StringSplitOptions.None)[0];
                        var parts = split.Split('_');

                        if (parts.Length != 2)
                            throw new Exception($"ERROR: An exception occurred while processing a zsound file: {fileName} — wrong format!");

                        var name = parts[0];
                        var tempAddr = Convert.ToUInt32(parts[1], 16);

                        var oldPath = Path.Combine(TempFolder, fileName);
                        var newPath = Path.Combine(TempFolder, $"{name}.zsound");

                        File.Move(oldPath, newPath);
                        ZSounds[name] = tempAddr;
                    }
                }

                if (!Sequences.Any())
                    throw new FileNotFoundException("No sequence file found!");
                if (Categories == null)
                    throw new FileNotFoundException("No categories.txt file found!");
            }

            public void Pack(string filename, string destinationDir)
            {
                // packs the music file into a new archive
                
                string archivePath = Path.Combine(destinationDir, filename);

                ZipFile.CreateFromDirectory(TempFolder, $"{archivePath}.zip", CompressionLevel.Optimal, false);

                string zipFilePath = $"{archivePath}.zip";
                string mmrsFilePath = $"{archivePath}.mmrs";

                if (File.Exists(zipFilePath))
                    File.Move(zipFilePath, mmrsFilePath);

                if (Directory.Exists(TempFolder))
                    Directory.Delete(TempFolder, true);
            }
        }

        public static void ProcessFiles(string baseFolder, string convFolder)
        {
            // creates conversion folder, then copies and converts every file in the original music folder
            // into the new music folder
            // copy instead of edit in place for extra safety
            
            Directory.CreateDirectory(convFolder);

            var allFiles = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);
            var seqsTxtFile = allFiles.FirstOrDefault(f => Path.GetFileName(f).Equals("SEQS.txt", StringComparison.OrdinalIgnoreCase));

            foreach (var inputFile in allFiles)
            {
                string extension = Path.GetExtension(inputFile).ToLower();
                string filename = Path.GetFileName(inputFile);
                string relativePath = Path.GetRelativePath(baseFolder, inputFile);
                string destinationFile = Path.Combine(convFolder, relativePath);
                string destinationDir = Path.GetDirectoryName(destinationFile);

                if (!Directory.Exists(destinationDir))
                    Directory.CreateDirectory(destinationDir);

                // Don't copy the SEQS file because it gets converted too
                if (filename.Equals("SEQS.txt", StringComparison.OrdinalIgnoreCase))
                    continue;

                File.Copy(inputFile, destinationFile, overwrite: true);

                switch (extension)
                {
                    case ".zseq":
                        ConvertStandalone(destinationFile, destinationDir);
                        break;

                    case ".mmrs":
                        ConvertArchive(destinationFile, destinationDir);
                        break;

                    default:
                        break;
                }
            }

            if (seqsTxtFile != null)
            {
                string seqsYamlFile = Path.Combine(convFolder, "SEQS.yml");
                ConvertSEQSToYAML(seqsTxtFile, seqsYamlFile);
            }
        }

        public static void CheckForOldFiles(string baseFolder)
        {
            if (OLD_MUSIC_FILES.Any())
                OLD_MUSIC_FILES.Clear(); // Clear out the list if it's populated

            var allFiles = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);
            bool seqsTxtFound = false;

            foreach (var inputFile in allFiles)
            {
                string extension = Path.GetExtension(inputFile).ToLower();

                switch (extension)
                {
                    case ".zseq":
                        OLD_MUSIC_FILES.Add(inputFile);
                        break;

                    case ".mmrs":
                        using (ZipArchive zip = ZipFile.OpenRead(inputFile))
                        {
                            bool metaExists = false;

                            foreach (var entry in zip.Entries)
                            {
                                if (entry.FullName.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                                {
                                    metaExists = true;
                                    break;
                                }
                            }

                            if (!metaExists)
                                OLD_MUSIC_FILES.Add(inputFile); // Add to the list if a .meta file doesn't exist
                        }
                        break;

                    default:
                        break;
                }

                if (!seqsTxtFound && Path.GetFileName(inputFile).Equals("SEQS.txt", StringComparison.OrdinalIgnoreCase))
                {
                    OLD_MUSIC_FILES.Add(inputFile);
                    seqsTxtFound = true;
                }
            }
        }

        public static void WriteMetadata(string folder, string baseName, string cosmeticName, string metaBank, string songType, List<object> categories, Dictionary<string, uint> zsounds = null)
        {
            // Prepare the YAML object
            var yaml = new MMRSMetadataYAML
            {
                Game = "mm",
                Metadata = new MMRSMetadataYAML.Meta
                {
                    DisplayName = cosmeticName,
                    InstrumentSet = metaBank,
                    SongType = songType,
                    MusicGroups = categories,
                    AudioSamples = null
                }
            };

            // Optional audio sample info from zsounds
            if (zsounds != null && zsounds.Any())
            {
                yaml.Metadata.AudioSamples = new Dictionary<string, MMRSMetadataYAML.Sample>();

                var index = 0;
                foreach (var kvp in zsounds)
                {
                    string filename = $"{kvp.Key}.zsound"; // don't know if the extension is needed, but just in case...
                    uint tempAddr = kvp.Value;

                    // Since old data doesn't use the new format, the type, index, and keyregion can be left null
                    yaml.Metadata.AudioSamples[$"{filename}"] = new MMRSMetadataYAML.Sample
                    {
                        // Type = "~",
                        // Index = -1,
                        // KeyRegion = "~",
                        TempAddress = tempAddr
                    };

                    index++;
                }
            }

            // Serialize to YAML
            var serializer = new SerializerBuilder()
                .WithEventEmitter(next => new FlowStyleListEmitter(next))
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
                .Build();

            string yamlOutput = serializer.Serialize(yaml);

            File.WriteAllText(Path.Combine(folder, $"{baseName}.meta"), yamlOutput);
        }

        public static void ConvertStandalone(string destinationFile, string destinationDir)
        {
            // converts a zseq into the new mmrs file
            
            string cosmeticName = "";
            string metaBank = "";
            string songType = "";
            string[] categories;

            string filename = Path.GetFileNameWithoutExtension(destinationFile);
            string filepath = Path.GetFullPath(destinationFile);

            var standaloneSeq = new StandaloneSequence(filename);

            try
            {
                standaloneSeq.Copy(filepath);

                cosmeticName = Regex.Replace(standaloneSeq.Filename, @"(^|\W)(songforce|songtest)(?=\W|$)", " ", RegexOptions.IgnoreCase);
                cosmeticName = Regex.Replace(cosmeticName, @"\s+", " ").Trim();
                cosmeticName = string.IsNullOrWhiteSpace(cosmeticName) ? "???" : cosmeticName;

                metaBank = standaloneSeq.InstrumentSet;
                var rawCategories = standaloneSeq.Categories;
                List<object> cleanedCategories = new();
                foreach (var category in rawCategories)
                {
                    string cleaned = category.Trim();

                    if (cleaned.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                        cleaned = cleaned.Substring(2);

                    int value = Convert.ToInt32(cleaned, 16);

                    if (Enum.IsDefined(typeof(MusicGroups.Category), value))
                        cleanedCategories.Add(Enum.GetName(typeof(MusicGroups.Category), value));
                }

                categories = cleanedCategories.OfType<string>().ToArray();

                bool[] ffOrBgm = new bool[categories.Length];
                for (int i = 0; i < categories.Length; i++)
                    ffOrBgm[i] = FANFARE_CATEGORIES.Contains(categories[i].ToUpper());

                if (Array.TrueForAll(ffOrBgm, x => x))
                    songType = "fanfare";
                else if (Array.Exists(ffOrBgm, x => !x) && Array.Exists(ffOrBgm, x => x))
                    throw new Exception($"ERROR: Mixed categories for categories.txt in .mmrs file: {filename}.mmrs!");
                else
                    songType = "bgm";

                WriteMetadata(standaloneSeq.TempFolder, standaloneSeq.Filename, cosmeticName, metaBank, songType, cleanedCategories);

                standaloneSeq.Pack(standaloneSeq.Filename, destinationDir);
            }
            catch
            {
                return;
            }
            finally
            {
                if (Directory.Exists(standaloneSeq.TempFolder))
                    Directory.Delete(standaloneSeq.TempFolder, true);
            }
        }

        public static void ConvertArchive(string destinationFile, string destinationDir)
        {
            // converts an old mmrs file into a new mmrs file
            
            // if the file is new, skip it~
            using (ZipArchive zip = ZipFile.OpenRead(destinationFile))
            {
                foreach (var entry in zip.Entries)
                {
                    // only new files should have a .meta
                    if (Path.GetExtension(entry.FullName).Equals(".meta", StringComparison.OrdinalIgnoreCase))
                        return;
                }
            }

            string cosmeticName = "";
            string metaBank = "";
            string songType = "";
            List<object> categories = new();
            Dictionary<string, uint> zsounds = new();

            string filename = Path.GetFileNameWithoutExtension(destinationFile);
            string filepath = Path.GetFullPath(destinationFile);

            var archive = new MusicArchive();
            string originalTemp = archive.TempFolder;

            try
            {
                archive.Unpack(filename, filepath);
                File.Delete(filepath);

                cosmeticName = Regex.Replace(filename, @"(^|\W)(songforce|songtest)(?=\W|$)", " ", RegexOptions.IgnoreCase);
                cosmeticName = Regex.Replace(cosmeticName, @"\s+", " ").Trim();
                cosmeticName = string.IsNullOrWhiteSpace(cosmeticName) ? "???" : cosmeticName;

                using (var reader = new StreamReader(Path.Combine(originalTemp, archive.Categories)))
                {
                    string raw = reader.ReadLine();
                    string[] categoriesList;

                    if (raw.Contains("-"))
                        categoriesList = raw.Split('-');
                    else
                        categoriesList = raw.Split(',');

                    foreach (var category in categoriesList)
                    {
                        var cleanedCategory = category.Trim();
                        if (cleanedCategory.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            cleanedCategory = cleanedCategory.Substring(2);

                        // Convert to a string value
                        var value = Convert.ToInt32(cleanedCategory, 16);

                        if (Enum.IsDefined(typeof(MusicGroups.Category), value))
                            categories.Add(Enum.GetName(typeof(MusicGroups.Category), value));
                    }

                    bool[] ffOrBgm = new bool[categories.Count];
                    for (int i = 0; i < categories.Count; i++)
                        ffOrBgm[i] = FANFARE_CATEGORIES.Contains(categories[i]);

                    if (Array.TrueForAll(ffOrBgm, x => x))
                        songType = "fanfare";
                    else if (Array.Exists(ffOrBgm, x => !x) && Array.Exists(ffOrBgm, x => x))
                        throw new Exception($"ERROR: Mixed categories for categories.txt in .mmrs file: {filename}.mmrs!");
                    else
                        songType = "bgm";
                }

                foreach (var (baseName, ext) in archive.Sequences)
                {
                    string songFolder = Path.Combine(originalTemp, $"{baseName}");
                    Directory.CreateDirectory(songFolder);

                    metaBank = baseName;

                    string originalSeq = Path.Combine(originalTemp, $"{baseName}{ext}");
                    string newSeqPath = Path.Combine(songFolder, $"{baseName}.seq");
                    File.Copy(originalSeq, newSeqPath, true);

                    if (archive.Banks.ContainsKey(baseName))
                    {
                        var zbank = archive.Banks[baseName].Item1;
                        var bankmeta = archive.Banks[baseName].Item2;

                        File.Copy(Path.Combine(originalTemp, zbank), Path.Combine(songFolder, zbank), true);
                        File.Copy(Path.Combine(originalTemp, bankmeta), Path.Combine(songFolder, bankmeta), true);

                        metaBank = "custom";

                        foreach (var item in Directory.GetFiles(originalTemp, "*.zsound"))
                            File.Copy(item, Path.Combine(songFolder, Path.GetFileName(item)), true);

                        foreach (var z in archive.ZSounds)
                            zsounds[z.Key] = z.Value;
                    }

                    if (archive.FormMasks.ContainsKey(baseName))
                    {
                        string formmask = archive.FormMasks[baseName];
                        File.Copy(Path.Combine(originalTemp, formmask), Path.Combine(songFolder, formmask), true);
                    }

                    // Copy extra non-processed files
                    foreach (var item in Directory.GetFiles(originalTemp))
                    {
                        if (item.EndsWith(".seq") || item.EndsWith(".zseq") || item.EndsWith(".aseq") ||
                            item.EndsWith(".zbank") || item.EndsWith(".bankmeta") || item.EndsWith(".zsound") ||
                            item.EndsWith(".formmask") || Path.GetFileName(item).Equals("categories.txt", StringComparison.OrdinalIgnoreCase))
                            continue;

                        File.Copy(item, Path.Combine(songFolder, Path.GetFileName(item)), true);
                    }

                    WriteMetadata(songFolder, baseName, cosmeticName, metaBank, songType, categories, zsounds);

                    var tempArchive = new MusicArchive(skipTempCreate: true)
                    {
                        TempFolder = songFolder
                    };

                    if (archive.Sequences.Count > 1)
                        tempArchive.Pack($"{filename}_{baseName}", destinationDir);
                    else
                        tempArchive.Pack($"{filename}", destinationDir);

                    if (Directory.Exists(songFolder))
                        Directory.Delete(songFolder, true);
                }
            }
            catch
            {
                return;
            }
            finally
            {
                if (Directory.Exists(originalTemp))
                    Directory.Delete(originalTemp, true);
            }
        }

        public static void ConvertSEQSToYAML(string seqsTxtFile, string seqsYamlFile)
        {
            var lines = File.ReadAllLines(seqsTxtFile).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            var output = new Dictionary<string, SequenceUtils.SEQSYaml>();

            for (int i = 0; i < lines.Count;)
            {
                string seqName = lines[i++].Trim();
                string musicGroups = lines[i++].Trim();
                string instrumentSetStr = lines[i++].Trim();
                string seqIdStr = lines[i++].Trim();
                var noRecycle = i < lines.Count && lines[i].Trim().Equals("no-recycle") ? lines[i++].Trim() : null;

                // Convert instrument set to int
                if (instrumentSetStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    instrumentSetStr = instrumentSetStr.Substring(2);

                int instrumentSet = int.Parse(instrumentSetStr, NumberStyles.HexNumber);

                // Convert sequence id to int
                if (seqIdStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    seqIdStr = seqIdStr.Substring(2);

                int seqId = int.Parse(seqIdStr, NumberStyles.HexNumber);

                if (!AudioSequenceIds.SEQUENCE_ID_MAP.TryGetValue(seqId, out var def))
                {
                    continue;
                }

                var musicGroupList = musicGroups.Split(new[] { ',', '-' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s =>
                    {
                        bool parsed = int.TryParse(s.Trim(), NumberStyles.HexNumber, null, out int val);
                        return parsed ? val : -1;  // Return the value if parsed, otherwise -1
                    })
                    .Where(val =>
                    {
                        return Enum.IsDefined(typeof(MusicGroups.Category), val);
                    })
                    .Select(val =>
                    {
                        return Enum.GetName(typeof(MusicGroups.Category), val);
                    })
                    .Where(name => !string.IsNullOrEmpty(name))  // Filter out null or empty names
                    .ToList<object>();

                output[def.Name] = new SequenceUtils.SEQSYaml
                {
                    DisplayName = def.DisplayName,
                    MusicGroups = musicGroupList,
                    InstrumentSet = instrumentSet,
                    SequenceId = seqId,
                    SongType = def.Type,
                    NoRecycle = noRecycle != null,
                };
            }

            var serializer = new SerializerBuilder()
                             .WithEventEmitter(next => new FlowStyleListEmitter(next))
                             .Build();

            File.WriteAllText(seqsYamlFile, serializer.Serialize(output));
        }
    }
}
