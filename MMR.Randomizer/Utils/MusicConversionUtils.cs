using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using MMR.Randomizer.Constants;
using MMR.Randomizer.Models.Rom;
using MMR.Common.Utils;

namespace MMR.Randomizer.Utils
{
    /// <summary>
    /// Stores classes and functions that convert old format Majora's Mask Randomizer music files into metadata YAML '.mmrs' formatted music files.
    /// </summary>
    public class MusicConversionUtils
    {
        // Process outline:
        // Step 1: Check for old music files
        // Step 2: Create backup of music folder
        // Step 3: Convert music files

        /// <summary>
        /// Stores a list of old format music files.
        /// </summary>
        public static List<string> OLD_MUSIC_FILES = [];

        /// <summary>
        /// Stores a list of named music groups for fanfare.
        /// </summary>
        private static readonly string[] FANFARE_CATEGORIES =
        [   //groups
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
        ];

        /// <summary>
        /// Backs up the current state of the music folder into a 'music.old' zip archive.
        /// </summary>
        public static void BackupMusicFolder(string folder)
        {
            // backs up the music folder into a zip file with the .old extension
            // all the files are copied, but one can never be too careful

            string tempZipFolder = Path.Combine(Path.GetTempPath(), $"mmr_music_folder_{Guid.NewGuid()}");
            string finalBackupPath = Path.Combine(folder, $"music.old");

            try
            {
                if (File.Exists(finalBackupPath))
                    File.Delete(finalBackupPath);

                ZipFile.CreateFromDirectory(folder, tempZipFolder, CompressionLevel.Optimal, includeBaseDirectory: false);
            
                File.Move(tempZipFolder, finalBackupPath);
            }
            catch (Exception e)
            {
                throw new Exception($"BackupMusicFolder Error: Could not back up the music folder: {e.Message}");
            }
            finally
            {
                if (Directory.Exists(tempZipFolder))
                    Directory.Delete(tempZipFolder);
            }
        }

        /// <summary>
        /// Runs through the music folder to check for old format music files.
        /// </summary>
        public static void CheckForOldFiles(string baseFolder)
        {
            if (OLD_MUSIC_FILES.Count > 0)
                OLD_MUSIC_FILES.Clear(); // Clear out the list if it's populated

            // These are the only files that need to be checked
            var zseqFiles = Directory.GetFiles(baseFolder, "*.zseq", SearchOption.AllDirectories);
            var mmrsFiles = Directory.GetFiles(baseFolder, "*.mmrs", SearchOption.AllDirectories);
            var seqsFile = Directory.GetFiles(baseFolder, "SEQS.txt", SearchOption.AllDirectories).FirstOrDefault();

            var cachedHashes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Cached files should have already beed converted,
            // so there's no need to check unless their hash changed
            if (File.Exists(MusicCacheUtils.CachePath))
            {
                var cache = MusicCacheUtils.Load();
                cachedHashes = cache.FileHashes;
            }

            bool seqsTxtFound = seqsFile != null;

            foreach (var f in zseqFiles)
            {
                OLD_MUSIC_FILES.Add(f);
            }

            foreach (var f in mmrsFiles)
            {
                if (cachedHashes.TryGetValue(f, out var cachedHash))
                {
                    var currentHash = MusicCacheUtils.GetFileHash(f);
                    if (string.Equals(currentHash, cachedHash, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                using ZipArchive zip = ZipFile.OpenRead(f);
                bool hasCategoriesTxt = zip.Entries.Any(e => e.FullName.Equals("categories.txt", StringComparison.OrdinalIgnoreCase));

                if (hasCategoriesTxt)
                    OLD_MUSIC_FILES.Add(f); // Add to the list if a .metadata file doesn't exist
            }

            if (seqsTxtFound && seqsFile != null)
            {
                OLD_MUSIC_FILES.Add(seqsFile);
            }
        }

        /// <summary>
        /// Creates the music conversion folder, then begins the conversion process.
        /// </summary>
        public static void ConvertMusicFiles()
        {
            // backup the music folder, convert files, delete the original directory, rename converted
            // directory structure is maintained, and all non-music files are copied as well

            var convFolder = Path.Combine(Path.GetDirectoryName(Values.MusicDirectory), "converted");
            try
            {
                ProcessFiles(Values.MusicDirectory, convFolder);

                Directory.Delete(Values.MusicDirectory, true);
                Directory.Move(convFolder, Values.MusicDirectory);
            }
            catch (Exception e)
            {
#if DEBUG
                throw new Exception($"ConvertMusicFiles Error: {e.Message}");
#endif
            }
            finally
            {
                if (Directory.Exists(convFolder))
                    Directory.Delete(convFolder, true);
            }
        }

        /// <summary>
        /// Represents a standalone sequence ('.zseq') file used in previous versions of Majora's Mask Randomizer.
        /// </summary>
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

                (Filename, InstrumentSet, Categories) = ParseFilename(filename);
            }

            /// <summary>
            /// Parses and extracts metadata from the standalone sequence file's filename.
            /// </summary>
            public static (string filename, string instrumentSet, string[] categories) ParseFilename(string filename)
            {
                string[] parts = Path.GetFileNameWithoutExtension(filename).Split('_');

                if (parts.Length != 3)
                    throw new Exception("StandaloneSeqeunce Error: Invalid filename format.");

                return (parts[0], parts[1], parts[2].Split('-'));
            }

            /// <summary>
            /// Copies the sequence file into a temp folder and changes its file extension to '.seq'.
            /// </summary>
            public void Copy(string filepath)
            {
                // copies the sequence into its temp directory

                string tempSeqFilePath = Path.Combine(TempFolder, Filename + ".seq");

                using (var src = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                using (var dst = new FileStream(tempSeqFilePath, FileMode.Create, FileAccess.Write))
                src.CopyTo(dst);

                if (File.Exists(filepath))
                    File.Delete(filepath);
            }
        }

        /// <summary>
        /// Represents an old format '.mmrs' file used by Majora's Mask Randomizer.
        /// </summary>
        public class MusicArchive
        {
            public List<(string Base, string Extension)> Sequences { get; set; } = [];
            public string Categories { get; set; }
            public Dictionary<string, (string ZBank, string BankMeta)> Banks { get; set; } = [];
            public Dictionary<string, string> Formmasks { get; set; } = [];
            public Dictionary<string, uint> ZSounds { get; set; } = [];
            public string TempFolder { get; set; }

            private static readonly string[] SEQ_EXTS = [".seq", ".zseq", ".aseq"];

            public MusicArchive(bool skipTempCreate = false)
            {
                if (!skipTempCreate)
                {
                    TempFolder = Path.Combine(Path.GetTempPath(), $"mmrs_conversion_{Guid.NewGuid()}");
                    Directory.CreateDirectory(TempFolder);
                }
            }

            /// <summary>
            /// Unpacks the music file into a temp folder.
            /// </summary>
            public void Unpack(string filePath)
            {
                if (Directory.Exists(TempFolder))
                    Directory.Delete(TempFolder, recursive: true);

                ZipFile.ExtractToDirectory(filePath, TempFolder);

                int sampleCounter = 1;
                foreach (var f in Directory.GetFiles(TempFolder))
                {
                    var filename = Path.GetFileName(f);
                    var baseName = Path.GetFileNameWithoutExtension(filename);
                    var ext = Path.GetExtension(filename).ToLowerInvariant();

                    switch (ext)
                    {
                        case var _ when SEQ_EXTS.Contains(ext):
                            Sequences.Add((baseName, ext));
                            break;

                        case ".zbank":
                            var bankmetaPath = $"{baseName}.bankmeta"; // The bankmeta should have the same name as the zbank and sequence it's tied to
                            if (!File.Exists(Path.Combine(TempFolder, bankmetaPath)))
                                throw new FileNotFoundException($"Missing bankmeta for {filePath}!");
                            Banks[baseName] = (filename, bankmetaPath);
                            break;

                        case ".bankmeta":
                            break;

                        case ".formmask":
                            Formmasks[baseName] = filename;
                            break;

                        case ".zsound":
                            ProcessZSound(filename, ref sampleCounter);
                            break;

                        case ".txt" when filename.Equals("categories.txt", StringComparison.OrdinalIgnoreCase):
                            Categories = filename;
                            break;

                        default:
                            break;
                    }
                }

                if (Sequences.Count == 0)
                    throw new FileNotFoundException("MusicArchive Error: No sequence file found!");
                if (Categories == null)
                    throw new FileNotFoundException("MusicArchive Error: No categories.txt file found!");
            }

            private void ProcessZSound(string filename, ref int sampleCounter)
            {
                string baseName = filename.Split(".zsound")[0];
                string[] parts = baseName.Split("_");

                string sampleName = string.Empty;
                uint tempAddress = 0xFFFFFFFF;

                // The standard is "filename_address.zsound", but apparently some people just have the temp address
                if (parts.Length == 2 && uint.TryParse(parts[1], NumberStyles.HexNumber, null, out tempAddress))
                {
                    sampleName = parts[0];
                }
                else if (parts.Length == 1 && uint.TryParse(parts[0], NumberStyles.HexNumber, null, out tempAddress))
                {
                    sampleName = $"Sample{sampleCounter++}"; // Give the sample a default name and increment for the next default name
                }
                else
                {
                    // There's more than 2 parts, so the address could theoretically be anywhere, throw an exception
                    throw new Exception($"ProcessZSound Error: An exception occurred while processing a zsound file: {filename} — wrong format!");
                }

                string oldPath = Path.Combine(TempFolder, filename);
                string newPath = Path.Combine(TempFolder, $"{sampleName}.zsound");

                // If the filename already exists, just add 1 to suffix
                int suffix = 1;
                while (File.Exists(newPath))
                {
                    newPath = Path.Combine(TempFolder, $"{sampleName}{suffix}.zsound");
                    suffix++;
                }

                try
                {
                    File.Move(oldPath, newPath);
                }
                catch
                {
                    return; // This should never happen, but just in case
                }

                ZSounds[Path.GetFileNameWithoutExtension(newPath)] = tempAddress;
            }
        }

        /// <summary>
        /// Begins the process of converting old format music files into the metadata YAML '.mmrs' file format.
        /// </summary>
        private static void ProcessFiles(string baseFolder, string convFolder)
        {
            // Creates the conversion folder
            // Gathers a list of all files in the directory
            // Builds a hashset for old files that need to be processed and converted
            // Attempts to convert files, if conversion fails copy the original file into the new directory
            // If the file isn't a file that needs conversion, copy it into the new folder
            
            Directory.CreateDirectory(convFolder);

            var allFiles = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);
            var oldFiles = new HashSet<string>(OLD_MUSIC_FILES, StringComparer.OrdinalIgnoreCase);

            Parallel.ForEach(allFiles, (inputFile) =>
            {
                try
                {
                    string extension = Path.GetExtension(inputFile).ToLower();
                    string filename = Path.GetFileName(inputFile);
                    string relativePath = Path.GetRelativePath(baseFolder, inputFile);
                    string destinationFile = Path.Combine(convFolder, relativePath);
                    string destinationDir = Path.GetDirectoryName(destinationFile);

                    Directory.CreateDirectory(destinationDir);

                    switch (extension)
                    {
                        case ".zseq":
                            if (!ConvertStandalone(inputFile, destinationDir))
                                File.Copy(inputFile, destinationFile, true);
                            break;

                        case ".mmrs":
                            var originalPath = Path.Combine(baseFolder, relativePath);
                            if (oldFiles.Contains(originalPath) && !ConvertArchive(inputFile, destinationDir))
                                File.Copy(inputFile, destinationFile, true);
                            break;

                        case ".txt" when filename.Equals("SEQS.txt", StringComparison.OrdinalIgnoreCase):
                            ConvertSEQSToYAML(inputFile, Path.Combine(destinationDir, "SEQS.yaml"));
                            break;

                        default:
                            File.Copy(inputFile, destinationFile);
                            break;
                    }
                }
                catch (Exception e)
                {
#if DEBUG
                    throw new Exception($"ProcessFiles Error: {e.Message}");
#endif
                }
            });
        }

        /// <summary>
        /// Converts a standalone sequence ('.zseq') file to the new metadata YAML '.mmrs' file format.
        /// </summary>
        private static bool ConvertStandalone(string inputFile, string destinationDir)
        {
            string filename = Path.GetFileNameWithoutExtension(inputFile);
            string filepath = Path.GetFullPath(inputFile);

            StandaloneSequence standaloneSeq = null;

            try
            {
                standaloneSeq = new StandaloneSequence(filename);

                standaloneSeq.Copy(filepath);

                string cosmeticName = CleanCosmeticName(standaloneSeq.Filename);
                string metaBank = standaloneSeq.InstrumentSet;

                List<object> categories = ParseCategories(standaloneSeq.Categories);
                string songType = GetSongType(categories, filename);

                WriteMetadata(standaloneSeq.TempFolder, standaloneSeq.Filename, cosmeticName, metaBank, songType, categories);

                Pack(standaloneSeq.Filename, standaloneSeq.TempFolder, destinationDir);

                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new Exception($"ConvertStandalone Error: {e.Message}");
#else
                return false;
#endif
            }
            finally
            {
                if (standaloneSeq?.TempFolder is string standaloneTemp && Directory.Exists(standaloneTemp))
                    Directory.Delete(standaloneTemp, true);
            }
        }

        /// <summary>
        /// Converts an old format '.mmrs' file to the new metadata YAML '.mmrs' file format.
        /// </summary>
        private static bool ConvertArchive(string inputFile, string destinationDir)
        {
            string filename = Path.GetFileNameWithoutExtension(inputFile);
            string filepath = Path.GetFullPath(inputFile);

            var archive = new MusicArchive();
            string originalTemp = archive.TempFolder;

            try
            {
                archive.Unpack(filepath);

                string cosmeticName = CleanCosmeticName(filename);

                (var categories, string songType) = ParseCategoriesAndSongType(Path.Combine(originalTemp, archive.Categories), filename);

                ProcessArchiveSequences(archive, destinationDir, filename, cosmeticName, categories, songType, originalTemp);

                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new Exception($"ConvertArchive Error: {e.Message}");
#else
                return false;
#endif
            }
            finally
            {
                if (Directory.Exists(originalTemp))
                    Directory.Delete(originalTemp, true);
            }
        }

        private static void ProcessArchiveSequences(MusicArchive archive, string destinationDir, string filename, string cosmeticName, List<object> categories, string songType, string originalTemp)
        {
            //Dictionary<string, uint> zsounds = [];
            Dictionary<string, Dictionary<string, object>> zsounds = [];

            foreach (var (baseName, ext) in archive.Sequences)
            {
                string songFolder = Path.Combine(originalTemp, $"{baseName}");
                Directory.CreateDirectory(songFolder);

                string metaBank = baseName;

                string originalSeq = Path.Combine(originalTemp, $"{baseName}{ext}");
                string newSeqPath = Path.Combine(songFolder, $"{baseName}.seq");
                File.Copy(originalSeq, newSeqPath, true);

                if (archive.Banks.TryGetValue(baseName, out (string ZBank, string BankMeta) bv))
                {
                    var zbank = bv.ZBank;
                    var bankmeta = bv.BankMeta;

                    File.Copy(Path.Combine(originalTemp, zbank), Path.Combine(songFolder, zbank), true);
                    File.Copy(Path.Combine(originalTemp, bankmeta), Path.Combine(songFolder, bankmeta), true);

                    metaBank = "custom";

                    foreach (var item in Directory.GetFiles(originalTemp, "*.zsound"))
                    {
                        File.Copy(item, Path.Combine(songFolder, Path.GetFileName(item)), true);
                    }

                    // Create an audiobank object so samples can use the new link type when converted :)
                    var bankmetaPath = Path.Combine(originalTemp, bankmeta);
                    byte[] bankmetaData = File.ReadAllBytes(bankmetaPath);

                    var zbankPath = Path.Combine(originalTemp, zbank);
                    byte[] bankData = File.ReadAllBytes(zbankPath);

                    var audiobank = new AudiobankUtils.Audiobank(bankmetaData, bankData, null, null);

                    foreach (var z in archive.ZSounds)
                    {
                        foreach (var s in audiobank.GetBankSamples())
                        {
                            if (z.Value == s.Address)
                            {
                                var entry = new Dictionary<string, object>
                                {
                                    ["instrument type"] = s.ParentString,
                                    ["list index"] = s.ParentId
                                };

                                // Only instruments have a key region
                                if (s.ParentString == "INST")
                                {
                                    entry["key region"] = s.KeyRegion;
                                }

                                zsounds[z.Key] = entry;
                                break;
                            }
                        }

                        //zsounds[z.Key] = z.Value;
                    }

                    // Hope GC removes it from memory
                    audiobank = null;
                }

                List<string> formmaskList = [];

                if (archive.Formmasks.TryGetValue(baseName, out string fv))
                {
                    string formmaskPath = Path.Combine(originalTemp, fv);
                    string formmaskContent = File.ReadAllText(formmaskPath);
                    formmaskList = YamlSerializer.Deserialize<List<string>>(formmaskContent);
                }

                CopyUnprocessedFiles(originalTemp, songFolder);

                WriteMetadata(songFolder, baseName, cosmeticName, metaBank, songType, categories, zsounds, formmaskList);

                var tempArchive = new MusicArchive(skipTempCreate: true)
                {
                    TempFolder = songFolder
                };

                if (archive.Sequences.Count > 1)
                {
                    Pack($"{filename}_{baseName}", tempArchive.TempFolder, destinationDir);
                }
                else
                {
                    Pack($"{filename}", tempArchive.TempFolder, destinationDir);
                }

                if (Directory.Exists(songFolder))
                    Directory.Delete(songFolder, true);
            }
        }

        /// <summary>
        /// Converts a SEQS plaintext file to a SEQS YAML file.
        /// </summary>
        private static void ConvertSEQSToYAML(string seqsTxtFile, string seqsYamlFile)
        {
            List<string> lines = [.. File.ReadAllLines(seqsTxtFile).Where(l => !string.IsNullOrWhiteSpace(l))];
            Dictionary<string, SequenceUtils.SEQSYaml> output = [];

            for (int i = 0; i < lines.Count;)
            {
                string seqName = lines[i++].Trim();
                string musicGroups = lines[i++].Trim();
                string instrumentSetStr = lines[i++].Trim();
                string seqIdStr = lines[i++].Trim();
                var noRecycle = i < lines.Count && lines[i].Trim().Equals("no-recycle") ? lines[i++].Trim() : null;

                // Convert instrument set to int
                if (instrumentSetStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    instrumentSetStr = instrumentSetStr[2..];
                }

                int instrumentSet = int.Parse(instrumentSetStr, NumberStyles.HexNumber);

                // Convert sequence id to int
                if (seqIdStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    seqIdStr = seqIdStr[2..];
                }

                int seqId = int.Parse(seqIdStr, NumberStyles.HexNumber);

                if (!AudioSequenceIds.SEQUENCE_ID_MAP.TryGetValue(seqId, out var def))
                    continue;

                var musicGroupList = musicGroups.Split([',', '-'], StringSplitOptions.RemoveEmptyEntries)
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

            File.WriteAllText(seqsYamlFile, YamlSerializer.FlowListSerialize(output));
        }

        /// <summary>
        /// Writes the YAML metadata file for the new metadata YAML '.mmrs' file format.
        /// </summary>
        private static void WriteMetadata(string folder, string baseName, string cosmeticName, string metaBank, string songType, List<object> categories, Dictionary<string, Dictionary<string, object>> zsounds = null, List<string> formmask = null)
        //private static void WriteMetadata(string folder, string baseName, string cosmeticName, string metaBank, string songType, List<object> categories, Dictionary<string, uint> zsounds = null, List<string> formmask = null)
        {
            // Prepare the YAML object
            var yaml = new MusicMetadataYaml
            {
                Game = "mm",
                Metadata = new MusicMetadataYaml.Meta
                {
                    DisplayName = cosmeticName,
                    InstrumentSet = metaBank,
                    SongType = songType,
                    MusicGroups = categories,
                    AudioSamples = null
                }
            };

            if (zsounds != null && zsounds.Count != 0)
            {
                yaml.Metadata.AudioSamples = [];

                foreach (var kvp in zsounds)
                {
                    string filename = $"{kvp.Key}.zsound";

                    var sampleData = kvp.Value;

                    yaml.Metadata.AudioSamples[filename] = new MusicMetadataYaml.Sample
                    {
                        Type = sampleData.TryGetValue("instrument type", out object it) ? it?.ToString() : null,
                        Index = sampleData.TryGetValue("list index", out object li) ? Convert.ToInt32(li) : -1,
                        KeyRegion = sampleData.TryGetValue("key region", out object kr) ? kr.ToString() : null,
                    };
                }
            }


            // Optional audio sample info from zsounds
            //if (zsounds != null && zsounds.Count != 0)
            //{
            //    yaml.Metadata.AudioSamples = [];

            //    var index = 0;
            //    foreach (var kvp in zsounds)
            //    {
            //        string filename = $"{kvp.Key}.zsound"; // don't know if the extension is needed, but just in case...
            //        uint tempAddr = kvp.Value;

            //        // Since old data doesn't use the new format, the type, index, and keyregion can be left null
            //        yaml.Metadata.AudioSamples[$"{filename}"] = new MusicMetadataYaml.Sample
            //        {
            //            // Type = "~",
            //            // Index = -1,
            //            // KeyRegion = "~",
            //            TempAddress = tempAddr
            //        };

            //        index++;
            //    }
            //}

            // Serialize to YAML
            string yamlPath = Path.Combine(folder, $"{baseName}.metadata");
            string yamlOutput = YamlSerializer.FlowListSerialize(yaml);
            File.WriteAllText(yamlPath, yamlOutput);

            if (formmask != null && formmask.Count > 0)
            {
                using (var writer = new StreamWriter(yamlPath, append: true))
                {
                    writer.WriteLine("formmask: [");
                    for (int i = 0; i < formmask.Count; i++)
                    {
                        string value = formmask[i];
                        string comment = i < 16 ? $"Channel {i}" : $"Cumulative States";

                        writer.Write($"  \"{value}\"");
                        if (i != formmask.Count - 1)
                            writer.Write(",");
                        writer.WriteLine($" # {comment}");
                    }
                    writer.WriteLine("]");
                }
            }
        }

        private static string CleanCosmeticName(string name)
        {
            name = Regex.Replace(name, @"(^|\W)(songforce|songtest)(?=\W|$)", " ", RegexOptions.IgnoreCase);
            name = Regex.Replace(name, @"\s+", " ").Trim();
            return string.IsNullOrWhiteSpace(name) ? "???" : name;
        }

        private static List<object> ParseCategories(IEnumerable<string> rawCategories)
        {
            List<object> categories = [];

            foreach (var c in rawCategories)
            {
                var cleanedCategory = c.Trim();

                if (cleanedCategory.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    cleanedCategory = cleanedCategory[2..];
                }

                // Convert to a string value
                if (int.TryParse(cleanedCategory, NumberStyles.HexNumber, null, out int value)
                    && Enum.IsDefined(typeof(MusicGroups.Category), value))
                {
                    categories.Add(Enum.GetName(typeof(MusicGroups.Category), value));
                }
            }

            return categories;
        }

        private static string GetSongType(IEnumerable<object> categories, string filename)
        {
            var flags = categories.Select(c => FANFARE_CATEGORIES.Contains(c)).ToArray();

            if (flags.All(f => f))
                return "fanfare";

            if (flags.Any(f => f) && flags.Any(f => !f))
                throw new Exception($"GetSongType Error: Mismatched categories for file: '{filename}'");

            return "bgm";
        }

        private static (List<object> Categories, string SongType) ParseCategoriesAndSongType(string categoryFile, string filename)
        {
            string raw = File.ReadLines(categoryFile).FirstOrDefault()?.Trim() ?? "";
            string[] list = raw.Contains('-') ? raw.Split('-') : raw.Split(',');

            var categories = ParseCategories(list);
            var songType = GetSongType(categories, filename);

            return (categories, songType);
        }

        private static void CopyUnprocessedFiles(string sourceDir, string destinationDir)
        {
            string[] skipExts = [".seq", ".zseq", ".aseq", ".zbank", ".bankmeta", ".zsound", ".formmask"];
            string skipFile = "categories.txt";

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string name = Path.GetFileName(file);
                string ext = Path.GetExtension(file);

                if (skipExts.Contains(ext, StringComparer.OrdinalIgnoreCase) || name.Equals(skipFile, StringComparison.OrdinalIgnoreCase))
                    continue;

                File.Copy(file, Path.Combine(destinationDir, name), true);
            }

        }

        /// <summary>
        /// Packs the music file into an '.mmrs' archive.
        /// </summary>
        public static void Pack(string filename, string tempFolder, string destinationDir)
        {
            string archivePath = Path.Combine(destinationDir, filename);

            ZipFile.CreateFromDirectory(tempFolder, $"{archivePath}.zip", CompressionLevel.Fastest, false);

            string zipFilePath = $"{archivePath}.zip";
            string mmrsFilePath = $"{archivePath}.mmrs";

            if (File.Exists(zipFilePath))
                File.Move(zipFilePath, mmrsFilePath);
        }
    }
}
