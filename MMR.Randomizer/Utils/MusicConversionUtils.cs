using MMR.Randomizer.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace MMR.Randomizer.Utils
{
    class MusicConversionUtil
    {
        // fanfare categories to ensure correct song type
        private static readonly string[] FANFARE_CATEGORIES =
        {   //groups
            "8", "9", "10",
            // individual
            "108", "109", "119", "120", "121", "122",
            "124", "137", "139", "13D", "13F", "141",
            "152", "155", "177", "178", "179", "17C",
            "17E",
        };

        public static void BackupMusicFolder(string folder)
        {
            /// backs up the music folder into a zip file with the .old extension
            /// all the files are copied, but one can never be too careful

            string folderName = Path.GetFileName(folder);
            string tempZipFolder = Path.Combine(Path.GetTempPath(), $"mmr_music_folder_{Guid.NewGuid()}");
            string finalBackupPath = Path.Combine(folder, $"music.old");

            ZipFile.CreateFromDirectory(folder, tempZipFolder, CompressionLevel.Optimal, includeBaseDirectory: false);

            if (File.Exists(finalBackupPath))
                File.Delete(finalBackupPath);
            
            File.Move(tempZipFolder, finalBackupPath);

            if (Directory.Exists(tempZipFolder))
                Directory.Delete(tempZipFolder);
        }

        public static void ConvertMusicFiles()
        {
            /// backup the music folder, convert files, delete the original directory, rename converted
            /// directory structure is maintained, and all non-music files are copied as well

            var convFolder = Path.Combine(Path.GetDirectoryName(Values.MusicDirectory), "converted");
            if (Directory.Exists(Values.MusicDirectory))
            {
                BackupMusicFolder(Values.MusicDirectory);
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
                {
                    throw new Exception("Invalid filename format.");
                }

                Filename = parts[0];
                InstrumentSet = parts[1];
                Categories = parts[2].Split('-');
            }

            public void Copy(string filepath)
            {
                /// copies the sequence into its temp directory
                
                if (File.Exists(Filename + ".zip"))
                    File.Delete(Filename + ".zip");

                if (File.Exists(Filename + ".mmrs"))
                    File.Delete(Filename + ".mmrs");

                string tempSeqFilePath = Path.Combine(TempFolder, Filename + ".seq");

                using (FileStream src = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                using (FileStream dst = new FileStream(tempSeqFilePath, FileMode.Create, FileAccess.Write))
                {
                    src.CopyTo(dst);
                }

                if (File.Exists(filepath))
                    File.Delete(filepath);
            }

            public void Pack(string filename, string destinationDir)
            {
                /// packs the sequence into a new archive
                
                string archivePath = Path.Combine(destinationDir, filename);

                ZipFile.CreateFromDirectory(TempFolder, $"{archivePath}.zip", CompressionLevel.Optimal, false);

                string zipFilePath = $"{archivePath}.zip";
                string mmrsFilePath = $"{archivePath}.mmrs";

                if (File.Exists(zipFilePath))
                {
                    File.Move(zipFilePath, mmrsFilePath);
                }

                if (Directory.Exists(TempFolder))
                {
                    Directory.Delete(TempFolder, true);
                }
            }
        }

        public class MusicArchive
        {
            public List<(string Base, string Extension)> Sequences { get; set; } = new();
            public string Categories { get; set; }
            public Dictionary<string, (string ZBank, string BankMeta)> Banks { get; set; } = new();
            public Dictionary<string, string> FormMasks { get; set; } = new();
            public Dictionary<string, string> ZSounds { get; set; } = new();
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
                /// unpacks the music file into its temp directory
                
                if (Directory.Exists(TempFolder))
                    Directory.Delete(TempFolder, recursive: true);

                if (File.Exists(filename + ".zip"))
                    File.Delete(filename + ".zip");

                if (File.Exists(filename + ".mmrs"))
                    File.Delete(filename + ".mmrs");

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
                        {
                            throw new Exception($"ERROR: An exception occurred while processing a zsound file: {fileName} — wrong format!");
                        }

                        var name = parts[0];
                        var tempAddr = parts[1];

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
                /// packs the music file into a new archive
                
                string archivePath = Path.Combine(destinationDir, filename);

                ZipFile.CreateFromDirectory(TempFolder, $"{archivePath}.zip", CompressionLevel.Optimal, false);

                string zipFilePath = $"{archivePath}.zip";
                string mmrsFilePath = $"{archivePath}.mmrs";

                if (File.Exists(zipFilePath))
                {
                    File.Move(zipFilePath, mmrsFilePath);
                }

                if (Directory.Exists(TempFolder))
                {
                    Directory.Delete(TempFolder, true);
                }
            }
        }

        public static void ProcessFiles(string baseFolder, string convFolder)
        {
            /// creates conversion folder, then copies and converts every file in the original music folder
            /// into the new music folder
    
            Directory.CreateDirectory(convFolder);

            var allFiles = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);

            foreach (var inputFile in allFiles)
            {
                string extension = Path.GetExtension(inputFile).ToLower();
                string relativePath = Path.GetRelativePath(baseFolder, inputFile);
                string destinationFile = Path.Combine(convFolder, relativePath);
                string destinationDir = Path.GetDirectoryName(destinationFile);

                if (!Directory.Exists(destinationDir))
                    Directory.CreateDirectory(destinationDir);

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
        }
        
        public static void WriteMetadata(string folder, string baseName, string cosmeticName, string metaBank, string songType, string categories, List<string> zsounds = null)
        {
            /// writes metadata file
            
            var metadata = new List<string>
            {
                cosmeticName,
                metaBank,
                songType,
                categories
            };

            if (zsounds != null && zsounds.Count > 0)
                metadata.AddRange(zsounds);

            File.WriteAllLines(Path.Combine(folder, $"{baseName}.meta"), metadata);
        }

        public static void ConvertStandalone(string destinationFile, string destinationDir)
        {
            /// converts a zseq into the new mmrs file
            
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

                cosmeticName = standaloneSeq.Filename.Replace("songforce", "").Replace("songtest", "").Trim(" _-".ToCharArray());
                metaBank = standaloneSeq.InstrumentSet;

                categories = standaloneSeq.Categories;

                bool[] ffOrBgm = new bool[categories.Length];
                for (int i = 0; i < categories.Length; i++)
                {
                    ffOrBgm[i] = FANFARE_CATEGORIES.Contains(categories[i].ToUpper());
                }

                if (Array.TrueForAll(ffOrBgm, x => x))
                    songType = "fanfare";
                else if (Array.Exists(ffOrBgm, x => !x) && Array.Exists(ffOrBgm, x => x))
                    throw new Exception($"ERROR: Mixed categories for categories.txt in .mmrs file: {filename}.mmrs!");
                else
                    songType = "bgm";

                string categoriesString = string.Join(",", categories);

                WriteMetadata(standaloneSeq.TempFolder, standaloneSeq.Filename, cosmeticName, metaBank, songType, categoriesString);

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
            /// converts an old mmrs file into a new mmrs file
            
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
            string categories = "";
            List<string> zsounds = new List<string>();

            string filename = Path.GetFileNameWithoutExtension(destinationFile);
            string filepath = Path.GetFullPath(destinationFile);

            var archive = new MusicArchive();
            string originalTemp = archive.TempFolder;

            try
            {
                archive.Unpack(filename, filepath);
                File.Delete(filepath);

                cosmeticName = filename.Replace("songforce", "").Replace("songtest", "").Trim(" _-".ToCharArray());

                using (var reader = new StreamReader(Path.Combine(originalTemp, archive.Categories)))
                {
                    categories = reader.ReadLine();
                    string[] categoriesList;

                    if (categories.Contains("-"))
                        categoriesList = categories.Split('-');
                    else
                        categoriesList = categories.Split(',');

                    var cleanedCategories = new List<string>();

                    foreach (var category in categoriesList)
                    {
                        var cleanedCategory = category.Trim();
                        if (cleanedCategory.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                            cleanedCategory = cleanedCategory.Substring(2);

                        cleanedCategories.Add(cleanedCategory);
                    }

                    bool[] ffOrBgm = new bool[cleanedCategories.Count];
                    for (int i = 0; i < cleanedCategories.Count; i++)
                    {
                        ffOrBgm[i] = FANFARE_CATEGORIES.Contains(cleanedCategories[i].ToUpper());
                    }

                    if (Array.TrueForAll(ffOrBgm, x => x))
                        songType = "fanfare";
                    else if (Array.Exists(ffOrBgm, x => !x) && Array.Exists(ffOrBgm, x => x))
                        throw new Exception($"ERROR: Mixed categories for categories.txt in .mmrs file: {filename}.mmrs!");
                    else
                        songType = "bgm";

                    categories = string.Join(",", cleanedCategories);
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

                        metaBank = "-";

                        foreach (var item in Directory.GetFiles(originalTemp, "*.zsound"))
                        {
                            File.Copy(item, Path.Combine(songFolder, Path.GetFileName(item)), true);
                        }

                        foreach (var z in archive.ZSounds)
                        {
                            string command = $"ZSOUND:{z.Key}.zsound:{z.Value}";
                            zsounds.Add(command);
                        }
                    }

                    if (archive.FormMasks.ContainsKey(baseName))
                    {
                        string formmask = archive.FormMasks[baseName];
                        File.Copy(Path.Combine(originalTemp, formmask), Path.Combine(songFolder, formmask), true);
                    }

                    // Copy extra non-processed files
                    foreach (var item in Directory.GetFiles(originalTemp))
                    {
                        if (item.EndsWith(".seq") || item.EndsWith(".zseq") || item.EndsWith(".aseq") || item.EndsWith(".zbank") || item.EndsWith(".bankmeta") || item.EndsWith(".zsound") || item.EndsWith(".formmask") || Path.GetFileName(item).Equals("categories.txt"))
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
    }
}
