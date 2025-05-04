using MMR.Randomizer.Constants;
using MMR.Randomizer.Models.Rom;
using System;
using System.Diagnostics;
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

        private static readonly Dictionary<int, (string Name, string DisplayName, string Type)> SEQUENCE_ID_MAP = new()
        {
            // Sequence ID for matching, then tuple of data
            { 0x02, ("mm-terminafield", "Termina Field", "bgm") },
            { 0x13, ("mm-snowheadmountains", "Snowhead", "bgm") },
            { 0x10, ("mm-greatbaycoast", "Great Bay Coast", "bgm") },
            { 0x11, ("mm-ikanacanyon", "Ikana Canyon", "bgm") },
            { 0x0C, ("mm-southernswamp", "Southern Swamp", "bgm") },
            { 0x15, ("mm-clocktown1", "Clock Town (Day 1)", "bgm") },
            { 0x16, ("mm-clocktown2", "Clock Town (Day 2)", "bgm") },
            { 0x17, ("mm-clocktown3", "Clock Town (Day 3)", "bgm") },
            { 0x30, ("mm-goronshrine", "Goron Shrine", "bgm") },
            { 0x2F, ("mm-romaniranch", "Romani Ranch", "bgm") },
            { 0x36, ("mm-zorahall", "Zora Hall", "bgm") },
            { 0x12, ("mm-dekupalace", "Deku Palace", "bgm") },
            { 0x3B, ("mm-caves", "Secret Grotto", "bgm") },
            { 0x65, ("mm-snowheadtemple", "Snowhead Temple", "bgm") },
            { 0x66, ("mm-greatbaytemple", "Great Bay Temple", "bgm") },
            { 0x14, ("mm-piratesfortress", "Pirates' Fortress", "bgm") },
            { 0x6F, ("mm-ikanacastle", "Ancient Castle of Ikana", "bgm") },
            { 0x06, ("mm-stonetower", "Stone Tower Temple", "bgm") },
            { 0x07, ("mm-invertedstonetower", "Inverted Stone Tower Temple", "bgm") },
            { 0x1C, ("mm-woodfalltemple", "Woodfall Temple", "bgm") },
            { 0x05, ("mm-clocktower", "Clock Tower Interior", "bgm") },
            { 0x2E, ("mm-guruguru", "Guru-Guru's Theme", "bgm") },
            { 0x3C, ("mm-milkbar", "Milk Bar", "bgm") },
            { 0x1F, ("mm-house", "House", "bgm") },
            { 0x44, ("mm-shop", "Item Shop", "bgm") },
            { 0x46, ("mm-shootinggallery", "Minigame Shop", "bgm") },
            { 0x2C, ("mm-laboratory", "Curiosity Shop", "bgm") },
            { 0x3A, ("mm-observatory", "Astral Observatory", "bgm") },
            { 0x27, ("mm-musicbox", "Music-Box House", "bgm") },
            { 0x26, ("mm-goronrace", "Goron Race", "bgm") },
            { 0x25, ("mm-minigame", "Minigame", "bgm") },
            { 0x72, ("mm-wagonride", "Cremia's Theme", "bgm") },
            { 0x0E, ("mm-boatcruise", "Old Koume's Boat Cruise", "bgm") },
            { 0x40, ("mm-horserace", "Horse Race", "bgm") },
            { 0x31, ("mm-meeting", "Mayor Dotour's Office", "bgm") },
            { 0x0D, ("mm-aliens", "Aliens' Theme", "bgm") },
            { 0x50, ("mm-swordschool", "Swordsman's School", "bgm") },
            { 0x0F, ("mm-sharpscurse", "Sharp's Curse", "bgm") },
            { 0x03, ("mm-chase", "Pursuit Theme", "bgm") },
            { 0x04, ("mm-skullkid", "Majora's Theme", "bgm") },
            { 0x7B, ("mm-maskreveal", "The Moon Enraged", "bgm") },
            { 0x28, ("mm-fairyfountain", "Great Fairy's Fountain", "bgm") },
            { 0x18, ("mm-fileselect", "File Select", "bgm") },
            { 0x73, ("mm-keaton", "Keaton's Theme", "bgm") },
            { 0x45, ("mm-kaepora", "Kaepora Gaebora's Theme", "bgm") },
            { 0x43, ("mm-witches", "Koume & Kotake's Theme", "bgm") },
            { 0x42, ("mm-gormanbros", "Gorman Bros.' Theme", "bgm") },
            { 0x3E, ("mm-mysterywoods", "Woods of Mystery", "bgm") },
            { 0x29, ("mm-zelda", "Zelda's Theme", "bgm") },
            { 0x7D, ("mm-reunion", "Reunion Theme", "bgm") },
            { 0x0B, ("mm-healed", "Song of Healing Theme", "bgm") },
            { 0x2D, ("mm-giants", "Giants' Theme", "bgm") },
            { 0x38, ("mm-miniboss", "Miniboss Battle", "bgm") },
            { 0x1B, ("mm-boss", "Boss Battle", "bgm") },
            { 0x6B, ("mm-mask", "Majora's Mask", "bgm") },
            { 0x6A, ("mm-incarnation", "Majora's Incarnation", "bgm") },
            { 0x69, ("mm-wrath", "Majora's Wrath", "bgm") },
            { 0x08, ("mm-f-chasefail", "Event Failure 1", "fanfare") },
            { 0x09, ("mm-f-fail", "Event Failure 2", "fanfare") },
            { 0x19, ("mm-f-clearshort", "Event Success", "fanfare") },
            { 0x20, ("mm-f-gameover", "Game Over", "fanfare") },
            { 0x21, ("mm-f-bossdown", "Boss Defeated", "fanfare") },
            { 0x22, ("mm-f-gotitem", "Item Get", "fanfare") },
            { 0x24, ("mm-f-heart", "Heart Container Get", "fanfare") },
            { 0x37, ("mm-f-mask", "Mask Get", "fanfare") },
            { 0x39, ("mm-f-smallitem", "Heart Piece Get", "fanfare") },
            { 0x3D, ("mm-f-meet", "The Truth Revealed", "fanfare") },
            { 0x3F, ("mm-f-goronwin", "Goron Race Win", "fanfare") },
            { 0x41, ("mm-f-horsewin", "Horse Race Win", "fanfare") },
            { 0x52, ("mm-f-song", "Song Get", "fanfare") },
            { 0x55, ("mm-f-soar", "Song of Soaring", "fanfare") },
            { 0x77, ("mm-f-dungeonopen", "Temple Appears", "fanfare") },
            { 0x78, ("mm-f-dungeonclearshort", "Temple Clear (Short)", "fanfare") },
            { 0x79, ("mm-f-dungeonclearlong", "Temple Clear (Long)", "fanfare") },
            { 0x7E, ("mm-f-moonclear", "The Moon Destroyed", "fanfare") },
            { 0x7C, ("mm-f-giantsleave", "The Giants Farewell", "fanfare") },
            { 0x71, ("mm-kamaros-mask-item-dance", "Kamaro's Theme", "bgm") },
            { 0x70, ("mm-c-giantscs", "The Giants Appear", "bgm") },
            { 0x76, ("mm-c-titlescreen", "Title Screen", "bgm") },
            { 0x1A, ("mm-combat", "Enemy Battle", "bgm") },
            { 0x6C, ("mm-japas-basspractice", "Japas' Room", "bgm") },
            { 0x6D, ("mm-tijo-drumpractice", "Tijo's Room", "bgm") },
            { 0x6E, ("mm-evan-pianopractice", "Evan's Room", "bgm") },
            { 0x57, ("mm-finalhours", "Final Hours", "bgm") },
            { 0x2B, ("mm-opening-a-chest", "Opening Chest", "fanfare") },
            { 0x2A, ("mm-kamaros-dance-rosa-sisters", "Rosa Sisters' Theme", "bgm") },
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

                if (!SEQUENCE_ID_MAP.TryGetValue(seqId, out var def))
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
