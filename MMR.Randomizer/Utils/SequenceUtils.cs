using MMR.Randomizer.Constants;
using static MMR.Randomizer.Constants.AudioSequenceIds;
using MMR.Randomizer.Models.Rom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Models;
using MMR.Common.Utils;
using MMR.Randomizer.Asm;
using YamlDotNet.Serialization;
using MMR.Common.Extensions;

namespace MMR.Randomizer.Utils
{
    public class SequenceUtils
    {
        // These are scenes the play may never visit, if they do, then they are visited very briefly and very little music is heard
        public static readonly List<int> lowUseMusicSlots =
        [
            MAJORAS_THEME,        // 0x04: Majora's Theme
            CLOCK_TOWER_INTERIOR, // 0x05: Clock Tower Interior
            BOAT_CRUISE,          // 0x0E: Old Koume's Boat Cruise
            SHARPS_CURSE,         // 0x0F: Sharp's Curse
            MUSIC_BOX_HOUSE,      // 0x27: Music-Box House
            ZELDAS_THEME,         // 0x29: Zelda's Theme
            GIANTS_THEME,         // 0x2D: Giants' Theme
            GURU_GURUS_THEME,     // 0x2E: Guru-Guru's Theme
            MAYORS_OFFICE,        // 0x31: Mayor Dotour's Office
            GORMAN_BROS_THEME,    // 0x42: Gorman Bros.' Theme
            OWLS_THEME,           // 0x45: Kaepora Gaebora's Theme
            SWORDSMANS_SCHOOL,    // 0x50: Swordsman's School
            GIANTS_APPEAR,        // 0x70: The Giants Appear
            CREMIAS_THEME,        // 0x72: Cremia's Theme
            KEATONS_THEME,        // 0x73: Keaton's Theme
            MOON_ENRAGED,         // 0x7B: The Moon Enraged
            GIANTS_LEAVE,         // 0x7C: The Giants Leave
            REUNION_THEME,        // 0x7D: Reunion Theme
        ];

        public static int MAX_BGM_BUDGET = 0x6000; // Vanilla: 0x3800
        public static int MAX_COMBAT_BUDGET = 0x6000; // unk
        public static int MAX_TYPE2_MUSIC_BUDGET = 0x6000; // Vanilla: 0x4100

        public static int New_AudioBankTable = 0; // For mmfilelist
        public static int NewInstrumentSetAddress; // For BGM shuffle, used to store AudioBankTable address
        public static int CurrentFreeBank = 0x29;
        public const  int REQUIRES_NEW_BANK = 0x28; // 0x28 used to be the only free bank, used to indicate custom banks

        public static MD5 md5lib; // Used for zip

        // Majora's Mask Audio Binary
        /// <summary>
        /// Holds bytearrays for the audiobank, audiobank index, audiotable, and audiotable index audio binary files for Majora's Mask.
        /// </summary>
        public static AudiobankUtils.Audiobin MM_AUDIOBIN = null;

        // Ocarina of Time Audio Binary
        public static string OOT_AUDIOBIN_PATH = Path.Combine(Values.MusicDirectory, "OOT.audiobin");
        /// <summary>
        /// Holds bytearrays for the audiobank, audiobank index, audiotable, and audiotable index audio binary files for Ocarina of Time.
        /// </summary>
        public static AudiobankUtils.Audiobin OOT_AUDIOBIN = null;

        /// <summary>
        /// Resets MAX_BGM_BUDGET, MAX_COMBAT_BUDGET, and MAX_TYPE2_MUSIC_BUDGET to 0x6000.
        /// </summary>
        public static void ResetBudget()
        {
            MAX_BGM_BUDGET = 0x6000;
            MAX_COMBAT_BUDGET = 0x6000;
            MAX_TYPE2_MUSIC_BUDGET = 0x6000;
        }

        /// <summary>
        /// Resets CurrentFreeBank to 0x29.
        /// </summary>
        public static void ResetFreeBankIndex()
        {
            CurrentFreeBank = 0x29;
        }

        public static bool IsMMRSFile(string filepath)
        {
            return Path.GetExtension(filepath) == ".mmrs";
        }

        public static bool IsOOTRSFile(string filepath)
        {
            return Path.GetExtension(filepath) == ".ootrs";
        }

        #region Audiobin Utilities
        /// <summary>
        /// Stores the Majora's Mask audio binary files (Audiobank, Audiobank Index, Audiotable, Audiotable Index) into an AudiobankUtils.Audiobin class.
        /// <para>
        /// IMPORTANT: Must be called before the Audiobank Index is moved.
        /// </para>
        /// </summary>
        public static void LoadMMAudiobin()
        {
            if (MM_AUDIOBIN != null)
                return; // Audiobin was already loaded into memory

            byte[] mmAudiobankFile = RomData.MMFileList[3].Data;
            byte[] mmAudiobankIndex = new byte[Addresses.AUDIOBANK_INDEX_SIZE];
            byte[] mmAudiotableFile = RomData.MMFileList[5].Data;
            byte[] mmAudiotableIndex = new byte[Addresses.AUDIOTABLE_INDEX_SIZE];

            byte[] mmCodeFile = RomData.MMFileList[31].Data;

            Array.Copy(mmCodeFile, Addresses.AUDIOBANK_INDEX_ADDR - Addresses.CODE_ADDR, mmAudiobankIndex, 0, Addresses.AUDIOBANK_INDEX_SIZE);
            Array.Copy(mmCodeFile, Addresses.AUDIOTABLE_INDEX_ADDR - Addresses.CODE_ADDR, mmAudiotableIndex, 0, Addresses.AUDIOTABLE_INDEX_SIZE);

            MM_AUDIOBIN = new AudiobankUtils.Audiobin(mmAudiobankFile, mmAudiobankIndex, mmAudiotableFile, mmAudiotableIndex);
        }

        /// <summary>
        /// Stores the Ocarina of Time audio binary files (Audiobank, Audiobank Index, Audiotable, Audiotable Index) into an AudiobankUtils.Audiobin class.
        /// </summary>
        public static void LoadOOTAudiobin()
        {
            if (OOT_AUDIOBIN != null)
                return; // Audiobin was already loaded into memory

            byte[] ootAudiobank = null;
            byte[] ootAudiobankIndex = null;
            byte[] ootAudiotable = null;
            byte[] ootAudiotableIndex = null;

            Dictionary<string, Action<byte[]>> binHandler = new()
            {
                { "Audiobank",        data => ootAudiobank       = data },
                { "Audiobank_index",  data => ootAudiobankIndex  = data },
                { "Audiotable",       data => ootAudiotable      = data },
                { "Audiotable_index", data => ootAudiotableIndex = data }
            };

            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(OOT_AUDIOBIN_PATH))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (binHandler.TryGetValue(entry.Name, out var action))
                        {
                            byte[] data = new byte[entry.Length];
                            using var stream = entry.Open();
                            stream.ReadExactly(data);
                            action(data);
                        }
                    }
                }

                OOT_AUDIOBIN = new AudiobankUtils.Audiobin(ootAudiobank, ootAudiobankIndex, ootAudiotable, ootAudiotableIndex);
            }
            catch (FileNotFoundException)
            {
                throw new Exception($"LoadOOTAudiobin Error: ScanForCustomMusicFiles found an OOTR music file, but could not find the Ocarina of Time audio binary ('OOT.audiobin') in the root directory of the music folder.");
            }
        }
        #endregion

        #region Music File Processing
        /// <summary>
        /// Reads and loads sequence metadata from the SEQS YAML and music cache.
        /// Updates the the target and source sequences, and begins processing custom music files.
        /// </summary>
        public static void ReadSequenceInfo()
        {
            // If the music directory doesn't exist, create it because it's required still
            if (!Directory.Exists(Values.MusicDirectory))
                Directory.CreateDirectory(Values.MusicDirectory);

            md5lib = MD5.Create();

            // Initialize the list of sequences and targets
            //RomData.SequenceList = [];
            RomData.TargetSequences = [];

            // Load the music cache, if it doesn't exist it returns an empty MusicCache object
            MusicCacheUtils.MusicCache cache = MusicCacheUtils.Load();
            Dictionary<string, string> updatedHashes = new(StringComparer.OrdinalIgnoreCase);

            HashSet<string> validFiles = cache.FileHashes
                .AsParallel()
                .Where(kvp => File.Exists(kvp.Key) && MusicCacheUtils.GetFileHash(kvp.Key) == kvp.Value)
                .Select(kvp => kvp.Key)
                .ToHashSet();

            // Reload the cached sequence list and remove vanilla sequences
            RomData.SequenceList = [.. cache.SequenceList.Where(f => f.Filepath != null && validFiles.Contains(f.Filepath))];

            // If the user has a SEQS.yml file, use that one instead of the one in resources
            string seqsContent;
            string seqsYamlPath = Directory.GetFiles(Values.MusicDirectory)
                                 .FirstOrDefault(f =>
                                 {
                                     string seqsYaml = Path.GetFileName(f);
                                     return string.Equals(seqsYaml, "SEQS.yml", StringComparison.OrdinalIgnoreCase) ||
                                            string.Equals(seqsYaml, "SEQS.yaml", StringComparison.OrdinalIgnoreCase);
                                 });

            if (seqsYamlPath != null)
            {
                Debug.WriteLine("SEQS: Found a SEQS file in the music folder to use");
                seqsContent = File.ReadAllText(seqsYamlPath);
            }
            else // There was no SEQS.yml/yaml, use the one in resources
            {
                seqsContent = Properties.Resources.SEQS;
            }

            var sequenceEntries = YamlSerializer.Deserialize<Dictionary<string, SEQSYaml>>(seqsContent);

            // Loop through each entry in the SEQS file
            // Entries are a YAML dictionary, for example:
            //
            // mm-terminafield:
            //   display name:   Termina Field
            //   music groups:   [Fields, TerminaField]
            //   instrument set: 0x03
            //   sequence id:    0x02
            //   song type:      bgm
            //   no recycle:     false
            //
            foreach (var entry in sequenceEntries)
            {
                string seqName = entry.Key;
                var seqData = entry.Value;

                // Check for missing song type field, if it is missing set to bgm
                // Then also assign a default music group based on that type
                var seqType = string.IsNullOrEmpty(seqData.SongType) ? "bgm" : seqData.SongType.ToLower();
                var defaultMusicGroup = seqType switch
                {
                    "bgm" => MusicGroups.DEFAULT_BGM_CATEGORIES,
                    "fanfares" => MusicGroups.DEFAULT_FANFARE_CATEGORIES,
                    _ => MusicGroups.DEFAULT_BGM_CATEGORIES,
                };

                // If there's no music groups, or the entry is null set to the default
                // Otherwise add each category
                List<int> seqCategories = [];
                if (seqData.MusicGroups == null || seqData.MusicGroups.Count == 0)
                {
                    seqCategories.AddRange(defaultMusicGroup);
                }
                else
                {
                    foreach (var part in seqData.MusicGroups)
                    {
                        if (TryParseCategory(part, out int c) && !seqCategories.Contains(c))
                        {
                            seqCategories.Add(c);
                        }
                        else
                        {
#if DEBUG
                            throw new Exception($"SEQS Error: Invalid category in SEQS file for '{seqName}': '{part}'");
#else
                            continue;
#endif
                        }
                    }
                }

                int seqInstrument = seqData.InstrumentSet;
                int seqId = seqData.SequenceId;

                SequenceInfo targetSequence = new()
                {
                    Name = seqName,
                    DisplayName = seqData.DisplayName ?? seqName,
                    Categories = seqCategories,
                    Instrument = seqInstrument,
                };

                SequenceInfo sourceSequence = new()
                {
                    Name = seqName,
                    DisplayName = seqData.DisplayName ?? seqName,
                    Categories = seqCategories,
                    Instrument = seqInstrument,
                };

                // Each entry should have a sequence ID that's available in the SEQUENCE_ID_MAP,
                // so try to match the entry's sequence ID with one in the sequence map between 0x02 and 0x7F
                if (SEQUENCE_ID_MAP.ContainsKey(seqId) && seqId >= 0x02 && seqId <= 0x7F)
                {
                    // If the randomizer relies on searching for sequence names
                    //targetSequence.Name = SEQUENCE_ID_MAP[seqId].Name;
                    //sourceSequence.Name = SEQUENCE_ID_MAP[seqId].Name;

                    targetSequence.Replaces = seqId;
                    sourceSequence.SeqId = seqId;

                    if (seqData.NoRecycle)
                    {
                        sourceSequence.Name = "drop";
                    }

                    //if (RomData.TargetSequences.Find(u => u.Name == SEQUENCE_ID_MAP[seqId].Name) != null)
                    if (RomData.TargetSequences.Find(u => u.Replaces == seqId) != null)
                        continue;

                    RomData.TargetSequences.Add(targetSequence);
                }

                if (sourceSequence.SeqId != FILE_SELECT && sourceSequence.Name != "drop")
                {
                    RomData.SequenceList.Add(sourceSequence);
                }
            }

            // MMR shortens the Song of Time cutscene and uses a custom sequence
            // It uses an unused slot because Song of Time doesn't have its own slot
            RomData.SequenceList.Add(new SequenceInfo
            {
                Name = nameof(Properties.Resources.mmr_f_sot),
                DisplayName = "Song of Time (MMR)",
                Categories = [(int)MusicGroups.Category.ItemFanfares],
                Instrument = 0x03,
                Replaces = INTRO_CUTSCENE_2,
            });

            // Search through every directory in the music folder
            IEnumerable<string> directories = new[] { Values.MusicDirectory }.Concat(Directory.EnumerateDirectories(Values.MusicDirectory, "*", SearchOption.AllDirectories));

            // Scan for custom music files in the music directory
            foreach (string directory in directories)
            {
                try
                {
                    ScanForCustomMusicFiles(directory, cache.FileHashes, updatedHashes);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new Exception($"Directory Error: Cannot access the following directory in the music folder: '{directory}'");
                }
            }
            
            // Secondary check for old music files returned some, so write the list of old files for users
            // This is contained within its own file because it could be hundreds of lines long
            if (MusicConversionUtils.OLD_MUSIC_FILES.Count > 0)
                File.WriteAllLines(Path.Combine(Values.MusicDirectory, "unsupported_music_files.txt"), MusicConversionUtils.OLD_MUSIC_FILES);

            // Update the music cache
            MusicCacheUtils.Save(
                new MusicCacheUtils.MusicCache
                {
                    FileHashes = updatedHashes,
                    SequenceList = RomData.SequenceList
                }
            );
        }

        /// <summary>
        /// Attempts to convert named music groups into integer values using the MusicGroups.cs file.
        /// </summary>
        private static bool TryParseCategory(object input, out int value)
        {
            value = 0;

            // Handle ints, if it's an int just return the value
            switch (input)
            {
                case int intValue:
                    value = intValue;
                    return true;

                case string strValue:
                    string trimmed = strValue.Trim();

                    // Handle "0x" prefixed hex strings
                    if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                        && int.TryParse(trimmed[2..], System.Globalization.NumberStyles.HexNumber, null, out int hexCategory))
                    {
                        value = hexCategory;
                        return true;
                    }

                    // If there's no hex prefix, it's still probably in hex anyway (and is expected to be)
                    if (int.TryParse(trimmed, System.Globalization.NumberStyles.HexNumber, null, out int intCategory))
                    {
                        value = intCategory;
                        return true;
                    }

                    // Try to match the string with a named MusicGroup (e.g. TerminaField = 0x102)
                    if (Enum.TryParse<MusicGroups.Category>(trimmed, true, out var enumCategory))
                    {
                        value = (int)enumCategory;
                        return true;
                    }

                    // If the named MusicGroup has spaces, try matching it with the dictionary mappings
                    if (MusicGroups.CategoryDisplayNames.TryGetValue(trimmed, out var mappedCategory))
                    {
                        value = (int)mappedCategory;
                        return true;
                    }

                    break;
            }

            return false;
        }

        /// <summary>
        /// Loops through the music folder for custom music files ('.mmrs' or '.ootrs') the user has added. Once a file is processed, it updates its music cache hash.
        /// </summary>
        public static void ScanForCustomMusicFiles(string directory, Dictionary<string, string> cachedhHashes, Dictionary<string, string> updatedHashes)
        {
            // MMRS and OOTRS are zip files with a custom file extension ".mmrs" and ".ootrs" respectively
            // They can contain the following music-related files:
            //   - Sequence file (.seq; required)
            //   - Metadata file (.metadata; required)
            //   - Instrument bank file (.zbank)
            //   - Instrument bank metadata file (.bankmeta)
            //   - Custom audio sample file (.zsound)
            //   - Formmask array file (.formmask; may be present in .metadata file)
            //
            // Only one file for each file type is allowed except custom audio sample files
            // an instrument bank may contain multiple sounds, so multiple may be required

            foreach (string filePath in Directory.GetFiles(directory))
            {
                var extension = Path.GetExtension(filePath);
                string hash = MusicCacheUtils.GetFileHash(filePath);

                // Only process files if they don't exist in the music cache or their hash has changed
                if (!cachedhHashes.TryGetValue(filePath, out string cachedHash) || cachedHash != hash)
                {
                    switch (extension)
                    {
                        case ".mmrs":
                        case ".ootrs":
                            if (extension == ".ootrs")
                                LoadOOTAudiobin(); // We need the OOT audiobin loaded if the file is OOTRS

                            ProcessCustomMusicFile(filePath);
                            break;

                        // Standalone sequences are a legacy format, they should have been converted, but double check
                        case ".zseq":
                            MusicConversionUtils.OLD_MUSIC_FILES.Add(Path.GetFileName(filePath));
                            break;

                        default:
                            break;
                    }
                }

                // Make sure the hash gets added so the cache is properly updated later
                switch (extension)
                {
                    default:
                        break;

                    case ".mmrs":
                    case ".ootrs":
                        updatedHashes[filePath] = hash;
                        break;
                }
            }
        }

        /// <summary>
        /// Processes a custom music file ('.mmrs' or '.ootrs'), storing required data and metadata to insert it into Majora's Mask.
        /// </summary>
        public static void ProcessCustomMusicFile(string filePath)
        {
            try
            {
                using (ZipArchive zip = ZipFile.OpenRead(filePath))
                {
                    MusicArchiveContents musicArchive = new();

                    // Setter factory
                    Action<ZipArchiveEntry> CreateSetter(Func<ZipArchiveEntry> getter, Action<ZipArchiveEntry> setter, string fileType)
                    {
                        return entry =>
                        {
                            if (getter() != null)
                                return;

                            setter(entry);
                        };
                    }

                    var handlers = new Dictionary<string, Action<ZipArchiveEntry>>(StringComparer.OrdinalIgnoreCase)
                    {
                        // Only allow a single file type for each file, except zsounds which may require multiple
                        { ".seq",      CreateSetter(() => musicArchive.SequenceFile,     e => musicArchive.SequenceFile = e, "sequence") },
                        { ".metadata", CreateSetter(() => musicArchive.MetaFile,         e => musicArchive.MetaFile = e,     "metadata") },
                        { ".zbank",    CreateSetter(() => musicArchive.BankFile,         e => musicArchive.BankFile = e,     "zbank") },
                        { ".bankmeta", CreateSetter(() => musicArchive.BankmetaFile,     e => musicArchive.BankmetaFile = e, "bankmeta") },
                        //{ ".formmask", CreateSetter(() => musicArchive.FormmaskFile,     e => musicArchive.FormmaskFile = e, "formmask") },
                        { ".zsound",   entry => musicArchive.AudioSamples.Add(entry) },
                    };

                    foreach (var entry in zip.Entries)
                    {
                        if (entry.FullName.Contains('/'))
                            continue;

                        // If the file is using the old format, it will have a categories file
                        if (entry.Name.Equals("categories.txt"))
                        {
                            musicArchive.CategoriesFile = entry;
                            continue;
                        }

                        string ext = Path.GetExtension(entry.Name).ToLowerInvariant();
                        if (handlers.TryGetValue(ext, out var handler))
                        {
                            handler(entry);
                        }
                    }

                    // Verify all required files are present
                    if (musicArchive.SequenceFile == null || musicArchive.MetaFile == null)
                    {
                        // If the file is an old file, it will have categories and no metadata file
                        if (musicArchive.CategoriesFile != null)
                        {
                            MusicConversionUtils.OLD_MUSIC_FILES.Add(Path.GetFileName(filePath));
                        }

                        return;
                    }

                    // Check to make sure there's a bank file for a bankmeta, and vice versa
                    bool hasBankFile = musicArchive.BankFile != null;
                    bool hasBankmetaFile = musicArchive.BankmetaFile != null;

                    if (hasBankFile != hasBankmetaFile)
                        return;

                    SequenceInfo currentSong = new()
                    {
                        Name = Path.GetFileNameWithoutExtension(filePath),
                        Filepath = filePath // Store the filepath for the music cache
                    };

                    var metadata = ReadMusicMetadataYaml(currentSong.Name, musicArchive.MetaFile);

                    // If game is OOT, but the OOT audiobin wasn't loaded already, load the OOT audiobin
                    if (metadata.Game == "oot" && !IsMMRSFile(currentSong.Filepath) ||
                        metadata.Game != "oot" && IsOOTRSFile(currentSong.Filepath))
                        LoadOOTAudiobin();

                    currentSong.Game = metadata.Game;
                    currentSong.DisplayName = metadata.CosmeticName;
                    currentSong.Categories = metadata.Categories;

                    // Handle custom audio samples
                    List<SequenceSoundSampleBinaryData> samplesList = [];
                    foreach (var command in metadata.Commands)
                    {
                        var zsoundName = command.TryGetValue("file", out var nameVal) ? nameVal as string : null;
                        var zsoundFile = musicArchive.AudioSamples.FirstOrDefault(entry => entry.Name.Contains(zsoundName));

                        if (zsoundFile != null)
                        {
                            byte[] sampleData = new byte[zsoundFile.Length];
                            using var zsoundStream = zsoundFile.Open();
                            zsoundStream.ReadExactly(sampleData);

                            var zsoundType = command.TryGetValue("type", out var typeVal) ? typeVal as string : null;
                            var zsoundIndex = command.TryGetValue("index", out var indexVal) ? indexVal as int? : null;
                            var zsoundKeyRegion = command.TryGetValue("key region", out var regionVal) ? regionVal as string : null;
                            var zsoundTempAddr = command.TryGetValue("temp addr", out var markerVal) ? markerVal as uint? : null;

                            samplesList.Add(
                                new SequenceSoundSampleBinaryData()
                                {
                                    BinaryData = sampleData,
                                    Addr = zsoundTempAddr ?? 0,
                                    Marker = zsoundTempAddr ?? 0,
                                    Hash = BitConverter.ToInt64(md5lib.ComputeHash(sampleData), 0),

                                    // Store the new type if available
                                    ParentFile = currentSong.Name,
                                    InstrumentType = zsoundType,
                                    ListIndex = zsoundIndex ?? -1,
                                    KeyRegion = zsoundKeyRegion,
                                }
                            );
                        }
                    }

                    currentSong.InstrumentSamples = samplesList;
                    currentSong.SequenceBinary = new SequenceBinaryData();

                    ReadMusicSequence(currentSong, musicArchive, metadata);

                    if (currentSong != null && currentSong.SequenceBinary != null)
                        RomData.SequenceList.Add(currentSong);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ProcessCustomMusicFile Error: An exception occured when attempting to read archive ('{Path.GetFileNameWithoutExtension(filePath)}'): {e}");
            }
        }

        /// <summary>
        /// Reads and stores the data from a music file's '.metadata' metadata YAML file.
        /// </summary>
        private static MusicMetadata ReadMusicMetadataYaml(string songname, ZipArchiveEntry metaFile)
        {
            if (metaFile == null)
                throw new Exception($"ReadMusicMetaYaml Error: No metadata file available for song: '{songname}'");

            // Valid values for the game and song type fields
            var validGames = new HashSet<string> { "oot", "mm" };
            var validTypes = new HashSet<string> { "bgm", "fanfare" };

            // Valid custom audio sample types and key regions
            var validSoundTypes = new HashSet<string> { "INST", "DRUM", "SFX" };
            var validKeyRegions = new HashSet<string> { "LOW", "PRIM", "HIGH" };

            MusicMetadataYaml yamlData;

            using (var reader = new StreamReader(metaFile.Open(), Encoding.Default))
            {
                string yamlText = reader.ReadToEnd();
                yamlData = YamlSerializer.Deserialize<MusicMetadataYaml>(yamlText);
            }

            if (yamlData == null || yamlData.Metadata == null)
                throw new Exception($"ReadMusicMetadataYaml Error: Invalid or empty YAML metadata for song: '{songname}'");

            string songType = validTypes.Contains(yamlData.Metadata.SongType?.ToLower()) ? yamlData.Metadata.SongType.ToLower() : "bgm";
            string songGame = validGames.Contains(yamlData.Game?.ToLower()) ? yamlData.Game.ToLower() : "mm"; // Default to MM if no game

            //Handle the categories
            List<int> categories = [.. MusicGroups.DEFAULT_BGM_CATEGORIES];
            if (yamlData.Metadata.MusicGroups != null && yamlData.Metadata.MusicGroups.Count > 0)
            {
                categories.Clear(); // Clear the defaults
                MusicGroups.Type? firstType = null;

                foreach (var category in yamlData.Metadata.MusicGroups)
                {
                    if (TryParseCategory(category, out int cat))
                    {
                        // BGM and Fanfare categories can't be mixed, so get the type
                        var currentType = MusicGroups.GetCategoryType(cat);

                        // Ensure at least the first type matches the given song type, otherwise throw an error
                        if (firstType == null && !string.Equals(songType, MusicGroups.TypeCheck[currentType], StringComparison.OrdinalIgnoreCase))
                            throw new Exception($"ReadMusicMetadataYaml Error: Category ('{category}') does not match given song type ('{songType}') for song: {songname}");

                        // After the first category, if any categories are mismatched then drop them entirely
                        // Might be good to throw an error or log the file... but this is fine for now
                        if (firstType != null && firstType != currentType)
                            continue;

                        firstType ??= currentType;

                        if (!categories.Contains(cat))
                            categories.Add(cat);
                    }
                    else
                    {
#if DEBUG
                        throw new Exception($"TryParseCategory Error: Bad category ('{category}') in song: '{songname}'.");
#else
                        continue;
#endif
                    }
                }
            }

            // Handle extra metadata
            List<Dictionary<string, object>> commands = [];
            if (yamlData.Metadata.AudioSamples != null)
            {
                foreach (var entry in yamlData.Metadata.AudioSamples)
                {
                    var sample = entry.Value;

                    string type = sample.Type?.Trim().ToUpperInvariant();
                    string keyRegion = sample.KeyRegion?.Trim().ToUpperInvariant();
                    int? listIndex = sample.Index == -1 ? null : sample.Index;
                    uint? tempAddr = sample.TempAddress;

                    if (type == null && listIndex != null && sample.KeyRegion != null)
                    {
                        throw new InvalidOperationException($"ReadMusicMetadataYaml Error: If type is null, index and ke region must also be null for audio sample ('{entry.Key}') in song: '{songname}'");
                    }
                    else
                    {
                        if (!validTypes.Contains(type) && type != null)
                            throw new InvalidOperationException($"ReadMusicMetadataYaml Error: Invalid instrument type ('{type}') for audio sample ('{entry.Key}'): '{songname}'");

                        if (validTypes.Contains(type) && listIndex == null)
                            throw new InvalidOperationException($"ReadMusicMetadataYaml Error: Index must not be null with given type ('{type}') for audio sample ('{entry.Key}') in song: '{songname}'");

                        if (type != null && tempAddr != null)
                            throw new InvalidOperationException($"ReadMusicMetadataYaml Error: Temp address must be null with new format for audio sample ('{entry.Key}') in song: '{songname}'");

                        if (type == "INST")
                        {
                            if (string.IsNullOrEmpty(keyRegion) || !validKeyRegions.Contains(keyRegion))
                                throw new InvalidOperationException($"ReadMusicMetadataYaml Error: Key region must be LOW, PRIM, or HIGH with given type ('{type}') for audio sample ('{entry.Key}') in song: '{songname}'");
                        }
                        else // DRUM or SFX
                        {
                            if (!string.IsNullOrEmpty(keyRegion))
                                throw new InvalidOperationException($"ReadMusicMetadataYaml Error: Key region must not be null or empty with given type ('{type}') for audio sample ('{entry.Key}') in song: '{songname}'");
                        }
                    }

                    // type, index, and key region are the new sample linking format used by OOTR and now MMR
                    // The new format links samples by parsing the audiobank to get the corresponding sample struct offsets,
                    // then it writes new address at the given sample struct offset (+ 4 bytes due to the bitfield).
                    //
                    // Originally, OOTR and MMR both required temp addresses and searched the bank byte by byte
                    // to find any matching sequences. There is fallback to a similar method, but now it only matches
                    // via parsed samples, ensuring only matching sample addresses are modified and nothing else is.
                    //
                    var zsound = new Dictionary<string, object>
                    {
                        { "type", sample.Type }, // Instrument type: INST, DRUM, SFX
                        { "index", listIndex }, // Index in the related structure list
                        { "key region", type == "INST" ? keyRegion : null }, // For INST: LOW, NORM, HIGH; for DRUM and SFX: leave empty
                        { "file", entry.Key },
                        { "temp addr", tempAddr }, // This is unused in the new format
                    };

                    commands.Add(zsound);
                }
            }

            return new MusicMetadata
            {
                Game = songGame,
                CosmeticName = yamlData.Metadata.DisplayName,
                InstrumentSet = yamlData.Metadata.InstrumentSet,
                SongType = songType,
                Categories = categories,
                Commands = commands,
                Formmask = yamlData.Formmask
            };
        }
        #endregion

        #region Sequence File Processing
        /// <summary>
        /// Reads sequence data and creates a SequenceBinaryData for the sequence file.
        /// </summary>
        private static void ReadMusicSequence(SequenceInfo song, MusicArchiveContents musicArchive, MusicMetadata metadata)
        {
            int claimedBankCount = 0;
            ZipArchiveEntry sequenceFile = musicArchive.SequenceFile ?? throw new FileNotFoundException($"ReadMusicSequence Error: Sequence file is missing");

            // The sequence file shouldn't be empty
            if (sequenceFile.Length == 0)
                throw new Exception($"ReadMusicSequence Error: Sequence file contains no data for song: '{song.Name}'");

            byte[] rawSeqData = new byte[sequenceFile.Length];
            using var stream = sequenceFile.Open();
            stream.ReadExactly(rawSeqData);

            SequenceBinaryData sequence = new() { SequenceData = rawSeqData };

            // If the value is "custom", then the music file uses a custom bank
            if (metadata.InstrumentSet == "custom")
            {
                song.Instrument = REQUIRES_NEW_BANK;
            }
            else
            {
                try
                {
                    song.Instrument = Convert.ToInt32(metadata.InstrumentSet, 16);
                }
                catch (FormatException)
                {
                    song.Instrument = REQUIRES_NEW_BANK;
                }
            }

            var customBankIncluded = ReadMusicInstrumentBank(song, sequence, musicArchive.BankFile, musicArchive.BankmetaFile);

            // Before AudioBankTable expansion, MMR used to overwrite the original instrument bank
            // However, this causes more problems now that expansion is available, so if an old instrument bank exists, treat it as custom
            if (song.Instrument > 0x28 || customBankIncluded)
                song.Instrument = REQUIRES_NEW_BANK;

            if (song.Instrument == REQUIRES_NEW_BANK && !customBankIncluded)
#if DEBUG
                throw new Exception($"ReadMusicSequence Error: Bad instrument set ('{metadata.InstrumentSet}') for song: '{song.Name}'");
#else
                return;
#endif

            if (customBankIncluded)
                claimedBankCount++;

            //ReadMusicFormmask(sequence, musicArchive.FormmaskFile, metadata.Formmask);
            ReadMusicFormmask(sequence, metadata.Formmask);

            song.SequenceBinary = sequence;
        }
        #endregion

        #region Instrument Bank Processing
        /// <summary>
        /// Reads and processes a music file's binary instrument bank data and stores it into the sequence's SequenceBinaryData.
        /// If the file is from OOT, then extra handling occurs to process OOT sample data.
        /// </summary>
        /// <returns>True or False whether the music file uses a custom bank.</returns>
        private static bool ReadMusicInstrumentBank(SequenceInfo song, SequenceBinaryData combo, ZipArchiveEntry bankFile, ZipArchiveEntry bankmetaFile)
        {
            // Instrument bank files are a binary file with the ".zbank" extension, they're paired with a binary metadata file with the ".bankmeta" extension
            // Returns true/false if the music file uses a custom instrument bank
            //
            // If the file being processed is an OOTRS file, then extra handling occurs for bank and sample data

            if (bankFile != null && bankmetaFile != null)
            {
                // The Bankmeta file that music files use is 8 bytes long
                if (bankmetaFile.Length != 8)
                    throw new Exception($"ReadMusicInstrumentBank Error: Bankmeta file is too short for file: '{song.Name}' - expected '8' bytes, but got '{bankmetaFile.Length}' bytes instead");

                byte[] bankmetaData = new byte[8];
                using var bankmetaReader = bankmetaFile.Open();
                bankmetaReader.ReadExactly(bankmetaData);

                int minLen = 0x08 + (bankmetaData[4] * 0x04) + (bankmetaData[5] * 0x04);

                // The bank should have at least as many bytes as there are drum and instrument pointers
                if (bankFile.Length < minLen)
                    throw new Exception($"ReadMusicInstrumentBank Error: Bank file is too short for file: '{song.Name}' - expected at least '{minLen}' bytes, but got '{bankFile.Length}' bytes instead");

                byte[] bankData = new byte[bankFile.Length];
                using var bankStream = bankFile.Open();
                bankStream.ReadExactly(bankData);

                // Modify OOT samples, checking if MM contains their data to update their sample addresses
                // or if the sample data needs to be used as a custom audio sample
                if (song.Game == "oot" && !IsMMRSFile(song.Filepath) ||
                    song.Game != "oot" && IsOOTRSFile(song.Filepath))
                    bankData = ProcessOOTSampleData(song, bankData, bankmetaData);

                // The audiotable should be 1, and the audiobin should correct all sample data to use AT0/AT1 (AT0 has 0 length)
                // The bankmeta stored in the audiobank class is 8 bytes, not 16
                bankmetaData[0x02] = 1;

                combo.InstrumentSet = new InstrumentSetInfo()
                {
                    BankBinary = bankData,
                    BankSlot = song.Instrument,
                    BankMetaData = bankmetaData,
                    Modified = 1,
                    Hash = BitConverter.ToInt64(md5lib.ComputeHash(bankData), 0),
                };

                return true; // The music file uses a custom bank
            }
            else if (song.Game == "oot" && !IsMMRSFile(song.Filepath) && song.Instrument < 0x25 ||
                     song.Game != "oot" && IsOOTRSFile(song.Filepath) && song.Instrument < 0x25) // Vanilla OOTR files require a custom bank
            {
                // Extract the bank's bankmeta from the audiobank index
                // The first line is the number of banks, so skip that and start on line 2
                int offset = 0x10 + (song.Instrument * 0x10);
                byte[] bankmeta = new byte[0x10];
                Array.Copy(OOT_AUDIOBIN.AudiobankIndex, offset, bankmeta, 0, 0x10);

                // Create the vanilla OOT bank object which stores the binary data of the bank
                var vanillaOOTBank = new AudiobankUtils.Audiobank(bankmeta, OOT_AUDIOBIN.AudiobankTable, OOT_AUDIOBIN.Audiotable, OOT_AUDIOBIN.AudiotableIndex);

                // The audiotable should be 1, and the audiobin should correct all sample data to use AT1
                // The bankmeta stored in the audiobank class is 8 bytes, not 16
                vanillaOOTBank.Bankmeta[0x02] = 1;

                // Modify OOT samples, checking if MM contains their data to update their sample addresses
                // or if the sample data needs to be used as a custom audio sample
                vanillaOOTBank.BankData = ProcessOOTSampleData(song, vanillaOOTBank.BankData, vanillaOOTBank.Bankmeta);

                combo.InstrumentSet = new InstrumentSetInfo()
                {
                    BankBinary = vanillaOOTBank.BankData,
                    BankSlot = REQUIRES_NEW_BANK,
                    BankMetaData = vanillaOOTBank.Bankmeta,
                    Modified = 1,
                    Hash = BitConverter.ToInt64(md5lib.ComputeHash(vanillaOOTBank.BankData), 0),
                };

                return true; // The music file uses a custom bank
            }

            return false; // The music file does not use a custom bank
        }

        /// <summary>
        /// Processes sample data in Ocarina of Time bank files, fixing sample addresses that match Majora's Mask sample data
        /// If the sample data doesn't match, then it creates a custom audio sample instead
        /// </summary>
        /// <returns>
        /// Updated bank data bytearray with Majora's Mask sample addresses
        /// </returns>
        private static byte[] ProcessOOTSampleData(SequenceInfo song, byte[] bankData, byte[] bankmetaData)
        {
            var newBank = new AudiobankUtils.Audiobank(bankmetaData, bankData, OOT_AUDIOBIN.Audiotable, OOT_AUDIOBIN.AudiotableIndex);
            var samples = newBank.GetBankSamples(); // Create a list of samples in the instrument bank

            foreach (var s in samples)
            {
                // Search the MM audio binary for data that matches the current sample's data
                // If the data matches, instead of creating a sound to inject, update the address
                // to the MM sample's address
                var matched = MM_AUDIOBIN.FindSampleInBanks(s.Data);
                if (matched.Address != null)
                {
                    // Addresses in vanilla shouldn't be above 0x7FFFFFFF, but keep uint just in case
                    byte[] matchedBytes = BitConverter.GetBytes((uint)matched.Address);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(matchedBytes);

                    // Update the address to the matched sample's address
                    // The sample struct starts with a 32-bit bitfield, so the sample address is 4 bytes later
                    for (int i = 0; i < 4; i++)
                    {
                        newBank.BankData[s.BankOffset + i + 4] = matchedBytes[i];
                    }
                }
                else // There was no MM match, so it needs to be injected
                {
                    // Ensure the hash of the current sample doesn't match the hash of previously added samples
                    long sampleHash = BitConverter.ToInt64(md5lib.ComputeHash(s.Data), 0);
                    if (!song.InstrumentSamples.Any(e => e.Hash == sampleHash))
                    {
                        // Create a new sample to inject
                        song.InstrumentSamples.Add(
                            new SequenceSoundSampleBinaryData()
                            {
                                BinaryData = s.Data,
                                Addr = s.Address ?? 0,
                                Marker = 0,//s.Address ?? 0, // 0 for new format
                                Hash = sampleHash,

                                // Use new format
                                ParentFile = song.Name,
                                InstrumentType = s.ParentString,//null,
                                ListIndex = s.ParentId,//-1,
                                KeyRegion = s.KeyRegion,//null,
                            }
                        );
                    }
                }
            }

            return newBank.BankData;
        }
        #endregion

        #region Formmask Data Processing
        /// <summary>
        /// Reads the formmask data from a '.formmask' file or the music files '.metadata' metadata file and creates a bitfield array that reflects the formmask conditions.
        /// </summary>
        //private static void ReadMusicFormmask(SequenceBinaryData combo, ZipArchiveEntry formmaskFile, SequencePlayState[] formmaskMetaArray = null)
        private static void ReadMusicFormmask(SequenceBinaryData combo, SequencePlayState[] formmaskMetaArray = null)
        {
            // The formmask file is a single JSON/YAML list that determines which sequence channels
            // should be turned on and off for each of Link's forms and states

            static void ProcessFormmaskData(SequencePlayState[] states, SequenceBinaryData combo)
            {
                // Backwards compatibility for version 1.15 and lower sequence files
                if (!states.Any(s => s.HasFlag(SequencePlayState.FierceDeity) && !s.HasFlag(SequencePlayState.Human)))
                {
                    for (var i = 0; i < states.Length; i++)
                    {
                        if (states[i].HasFlag(SequencePlayState.Human))
                        {
                            states[i] |= SequencePlayState.FierceDeity;
                        }
                    }
                }

                // Ensure unused cumulative states won't cause the channel to be muted when in those states
                foreach (var cumulativeState in Enum.GetValues<SequencePlayState>().Where(s => s > SequencePlayState.All))
                {
                    if (!states.Any(s => s.HasFlag(cumulativeState)))
                    {
                        states[0x10] |= cumulativeState;
                    }
                }

                combo.Formmask = ConvertUtils.U16ArrayToBytes([.. states.Cast<ushort>()]);
            }

            if (formmaskMetaArray != null)
            {
                ProcessFormmaskData(formmaskMetaArray, combo);
            }

            //if (formmaskFile != null && formmaskMetaArray == null)
            //{
            //    try
            //    {
            //        using var reader = new StreamReader(formmaskFile.Open(), Encoding.Default);
            //        string formMaskData = reader.ReadToEnd();

            //        // playState is a boolean bitfield, in the file it's "play with these states",
            //        // but in the code it's "mute these states" so it needs to be reversed
            //        var playState = YamlSerializer.Deserialize<SequencePlayState[]>(formMaskData);

            //        ProcessFormmaskData(playState, combo);
            //    }
            //    catch (Exception e)
            //    {
            //        throw new Exception($"ReadMusicFormmask Error: Music file's Formmask file is invalid: {e.Message}", e);
            //    }
            //}
            //else if (formmaskFile == null && formmaskMetaArray != null)
            //{
            //    ProcessFormmaskData(formmaskMetaArray, combo);
            //}
        }
        #endregion

        /// <summary>
        /// Gets the size of a binary sequence file and ensures its length is 16-byte aligned.
        /// </summary>
        public static int GetSequenceSize(SequenceInfo seq)
        {
            // The sequence should be loaded into memory if it was in an MMRS file
            if (seq.SequenceBinary != null)
            {
                return RoundTo16(seq.SequenceBinary.SequenceData.Length);
            }
            else if (SEQUENCE_ID_MAP.ContainsKey(seq.SeqId)) // If seq is vanilla, then SeqId is set and Replaces is -1; lookup from AudioSeq index table
            {
                // The code file ahould already be decompressed
                int codeFID = RomUtils.GetFileIndexForWriting(Addresses.SeqTable);
                var codeFile = RomData.MMFileList[codeFID];
                int audioseqIndexTableOffset = Addresses.SeqTable - codeFile.Addr;

                int entryaddr = audioseqIndexTableOffset + (seq.SeqId * 16); // Table entries are 16 bytes wide
                int size = (int)ReadWriteUtils.Arr_ReadU32(codeFile.Data, entryaddr + 4);
                return RoundTo16(size);
            }
            else // The sequence is not loaded into memory, so search for the sequence file
            {
                //byte[] data;
                if (File.Exists(seq.Filename))
                {
                    long length = new FileInfo(seq.Filename).Length;
                    return RoundTo16((int)length);
                    //using var reader = new BinaryReader(File.OpenRead(seq.Filename));
                    //data = new byte[(int)reader.BaseStream.Length];
                    //return RoundTo16(data.Length);
                }
            }

            throw new Exception("GetSequenceSize Error: Sequence File is missing");
        }

        /// <summary>
        /// Rounds data to the nearest 16-byte boundary.
        /// </summary>
        private static int RoundTo16(int value)
        {
            return (value + 0xF) & ~0xF;
        }

        #region Song Slot Pointerization
        /// <summary>
        /// Converts sequence slots to pointers so that if there's not enough music available, every song slot will be filled.
        /// </summary>
        public static void PointerizeSequenceSlots()
        {
            // If music availability is low, then some slots are converted to pointers
            // In vanilla, the Great Fairy's Fountain uses the same song as File Select,
            // with the former being a pointer to the latter. Because of that, there are
            // 78 slots and only 77 songs, which is not enough.
            //
            // Some music groups may also be exhausted, leaving slots unfilled with
            // the remaining music.
            //
            // So, slots the player may rarely hear or will never hear become pointers.
            // This will fill the remaining slots, that way if the player does encounter
            // the scene using the slot, it will still play music.
            //
            ConvertSequenceSlotToPointer(ZELDAS_THEME, SONG_OF_HEALING_THEME); // Point "Zelda's Theme" to "Song of Healing Theme"

            // With shortened cutscenes, slots that go unheard are converted to pointers.
            // If using a patch, "_randomized" is not set, so lookup a shortened cutscene byte instead
            //
            // =========================================================
            // File: 0x02CBF000, Address: 0x02CBFD48, Offset: 0x00000D48
            // Name: Z2_KONPEKI_ENT::Great Bay(Cutscene) -Scene File
            // =========================================================
            // Replaces:
            //   .dw 0x00010294  94
            // .orga 0x02CBFD48     ->
            //   .dw 0x00010000        00
            //
            // This checks "if not 94", because 94 is vanilla and 00 is the replacement.
            // It's possible this value could change one day, but vanilla is static.
            // If the file's data is null, nothing was changed in the file and it is vanilla.
            //
            bool shortenedCutscenes = RomData.MMFileList[1472].Data[0xD48 + 3] != 0x94;

            if (shortenedCutscenes)
            {
                // These cutscenes are never encountered with "Shorten cutscenes" enabled, so convert them to pointers
                ConvertSequenceSlotToPointer(CREMIAS_THEME, OWLS_THEME);            // Point "Cremia's Theme" to "Kaepora Gaebora's Theme"
                ConvertSequenceSlotToPointer(GIANTS_THEME, ASTRAL_OBSERVATORY);     // Point "The Giants' Theme" to "Astral Observatory"
                ConvertSequenceSlotToPointer(GIANTS_APPEAR, SONG_OF_HEALING_THEME); // Point "The Giants Appear" to "Song of Healing Theme"
                ConvertSequenceSlotToPointer(MOON_ENRAGED, ALIENS_THEME);           // Point "The Moon Enraged" to "Aliens' Theme"
                ConvertSequenceSlotToPointer(REUNION_THEME, CLOCK_TOWER_INTERIOR);  // Point "Reunion Theme" to "Clock Tower Interior"
            }

            // If the Ocarina is not randomized, then convert "Majora's Theme" to a pointer, it goes unused.
            // Randomized Ocarina applies the patch: "fix_ocarina_checks", use a change applied by the fix to detect.
            //
            // ============================================================================
            // File: 0x00B3C000, Address: 0x00BC66A0, Offset: 0x0008A6A0, Patch: 0x000000A8
            // Name: code
            // ============================================================================
            // Replaces:
            //  .dh 0x000014F9  F9
            // .org 0x80130160     ->
            //  .dh 0x00001000        00
            //
            bool ocarinaNotRandomized = RomData.MMFileList[31].Data[0x8A6A0 + 1] == 0xF9;

            if (ocarinaNotRandomized)
            {
                ConvertSequenceSlotToPointer(MAJORAS_THEME, SMALL_ENEMY_BATTLE); // Point "Majora's Theme" to "Small Enemy Battle"
            }

            // If the replacement pool is small (MM only or low variety), convert more sequences to pointers.
            if (RomData.TargetSequences.Count + 30 > RomData.SequenceList.Count)
            {
                ConvertSequenceSlotToPointer(TITLE_DEMO, CLOCK_TOWN_1);          // Point "Title Demo" to "Clock Town (Day 1)"
                ConvertSequenceSlotToPointer(EVENT_FAIL_1, EVENT_FAIL_2);        // Point "Event Failure 1" to "Event Failure 2"
                ConvertSequenceSlotToPointer(EVENT_SUCCESS, TEMPLE_CLEAR_SHORT); // Point "Event Success" to "Temple Clear (Short)"
            }

            // create some pointerized slots that are otherwise ignored, beacuse this pool gets re-used later for new song slots
            RomData.PointerizedSequences.Add(new SequenceInfo() { Name = "mm-introcutscene1", SeqId = INTRO_CUTSCENE_1, PreviousSlot = INTRO_CUTSCENE_1, Replaces = TITLE_DEMO });
        }

        /// <summary>
        /// Converts the input sequence slot index to be a pointer to the input substitute slot, then marks
        /// the slot so a new sequence isn't placed into the pointer's slot. This will free a song slot,
        /// but it won't be completely devoid of music if a player encounters it.
        /// </summary>
        public static void ConvertSequenceSlotToPointer(int seqSlotIndex, int substituteSlotIndex)
        {
            var targetSeq = RomData.TargetSequences.Find(u => u.Replaces == seqSlotIndex);
            var substituteSeq = RomData.TargetSequences.Find(u => u.Replaces == substituteSlotIndex);
            if (targetSeq != null && substituteSeq != null)
            {
                targetSeq.PreviousSlot = targetSeq.Replaces; // Needed during AudioSeq rebuild
                targetSeq.Replaces = substituteSeq.Replaces; // Point target to substitute
                RomData.PointerizedSequences.Add(targetSeq); // Save the sequence for AudioSeq rebuild
                RomData.TargetSequences.Remove(targetSeq);   // Ensure another sequence isn't placed in the slot
            }
            else
            {
                //throw new IndexOutOfRangeException("Could not convert slot to pointer:" + SeqSlotIndex.ToString("X2"));
                Debug.WriteLine("Cannot pointerize a songslot that does not exist: " + seqSlotIndex.ToString("X") + " and " + substituteSlotIndex.ToString("X"));
            }
        }
        #endregion

        /// <summary>
        /// Passed to RomData.SequenceList in Builder.cs::WriteAudioSeq
        /// </summary>
        public static void RebuildAudioSeq(List<SequenceInfo> sequenceList, int? sequenceMaskFileIndex, int? sequenceNamesFileIndex)
        {
            // Spoiler log output DEBUG
            StringBuilder log = new();
            void WriteOutput(string str)
            {
                Debug.WriteLine(str); // Keep DEBUG output
                log.AppendLine(str);
            }

            List<MMSequence> oldSeq = [];
            int f = RomUtils.GetFileIndexForWriting(Addresses.SeqTable);
            int basea = RomData.MMFileList[f].Addr;

            for (int i = 0; i < 128; i++)
            {
                MMSequence entry = new();

                int entryaddr = Addresses.SeqTable + (i * 16);
                entry.Addr = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, entryaddr - basea);
                var size = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, (entryaddr - basea) + 4);
                if (size > 0)
                {
                    entry.Data = new byte[size];
                    Array.Copy(RomData.MMFileList[4].Data, entry.Addr, entry.Data, 0, entry.Size);
                }
                else
                {
                    int j = sequenceList.FindIndex(u => u.Replaces == i);
                    if (j != -1)
                    {
                        if ((entry.Addr > 0) && (entry.Addr < 128))
                        {
                            if (sequenceList[j].Replaces != POINTER_0x18) // 0x28: Great Fairy's Fountain
                            {
                                sequenceList[j].Replaces = entry.Addr;
                            }
                            else
                            {
                                entry.Data = oldSeq[FILE_SELECT].Data;
                            }
                        }
                    }
                }

                oldSeq.Add(entry);
            }

            List<MMSequence> newSeq = [];
            int addr = 0;
            //byte[] newAudioSeq = [];
            List<byte> newAudioSeq = [];
            for (int i = 0; i < 128; i++)
            {
                MMSequence newentry = new();
                if (oldSeq[i].Size == 0)
                {
                    newentry.Addr = oldSeq[i].Addr;
                }
                else
                {
                    newentry.Addr = addr;
                }

                if (sequenceList.FindAll(u => u.Replaces == i).Count > 1)
                {
                    WriteOutput($"RebuildAudioSeq Error: Multiple songs pointing to song slot: '{i:X}'");
                }

                int p = RomData.PointerizedSequences.FindIndex(u => u.PreviousSlot == i);
                int j = sequenceList.FindIndex(u => u.Replaces == i);
                if (p != -1)
                {
                    // Found song to convert to a pointer
                    newentry.Addr = RomData.PointerizedSequences[p].Replaces;
                }
                else if (j != -1)
                {
                    // Replace old song with new song
                    if (sequenceList[j].SeqId != -1)
                    {
                        newentry.Data = oldSeq[sequenceList[j].SeqId].Data;
                        WriteOutput($"Slot {i:X2} := {sequenceList[j].Name}");
                    }
                    else if (sequenceList[j].SequenceBinary != null)
                    {
                        newentry.Data = sequenceList[j].SequenceBinary.SequenceData;
                        WriteOutput($"Slot {i:X2} := {sequenceList[j].Name} *");
                    }
                    else // Not an MM sequence, load and add file
                    {
                        byte[] data;
                        if (File.Exists(sequenceList[j].Filename))
                        {
                            using var reader = new BinaryReader(File.OpenRead(sequenceList[j].Filename));
                            data = new byte[reader.BaseStream.Length];
                            reader.ReadExact(data);

                            //using var reader = File.OpenRead(sequenceList[j].Filename);
                            //data = new byte[reader.Length];
                            //reader.ReadExactly(data, 0, (int)data.Length);

                            //using var reader = new BinaryReader(File.OpenRead(sequenceList[j].Filename));
                            //data = new byte[(int)reader.BaseStream.Length];
                            //reader.Read(data, 0, data.Length);
                        }
                        else if (sequenceList[j].Name == nameof(Properties.Resources.mmr_f_sot))
                        {
                            data = Properties.Resources.mmr_f_sot;
                        }
                        else
                        {
                            throw new Exception($"RebuildAudioSeq Error: Music not found as file or built-in resource: '{sequenceList[j].Filename}'");
                        }

                        // This might check if the sequence type is correct for MM
                        // DB ripped sequences from SF64/SM64/MK64 without modifying them
                        //
                        // 2025-05-08: Pretty sure this actually does nothing, don't know how mute flags work,
                        //             but making the second byte of sequence data 0x20 without checking if the
                        //             first byte is the proper instruction is risky and can completely ruin a
                        //             sequence if the first byte isn't 0xD3 and the first byte is a single byte
                        //             sequence instruction. It's a very, very, very low chance to happen, but you
                        //             can never discount the possibility that it will happen.
                        if (data[0] == 0xD3 && data[1] != 0x20)
                        {
                            data[1] = 0x20;
                        }

                        newentry.Data = data;
                        WriteOutput($"Slot {i:X2} := {sequenceList[j].Name}");
                    }
                }
                else // not found, song wasn't touched by rando, just transfer over
                {
                    newentry.Data = oldSeq[i].Data;
                }

                // DMA fails if the sequence isn't 16 byte aligned
                // Music will fails to play and will crash on actual hardware
                var padding = 0x10 - newentry.Size % 0x10;
                if (padding != 0x10)
                {
                    newentry.Data = [.. newentry.Data, .. new byte[padding]];
                }

                newSeq.Add(newentry);
                if (newentry.Data != null)
                {
                    //newAudioSeq = [.. newAudioSeq, .. newentry.Data];
                    newAudioSeq.AddRange(newentry.Data);
                }

                addr += newentry.Size;
            }

            // discovered when MM-only music was fixed, if the audioseq is left in it's old spot
            // audio quality is garbage, sounds like static
            //if (addr > (RomData.MMFileList[4].End - RomData.MMFileList[4].Addr))
            //else
            //RomData.MMFileList[4].Data = NewAudioSeq;

            int index = RomUtils.AppendFile([.. newAudioSeq]);
            ResourceUtils.ApplyHack(Resources.mods.reloc_audio);
            RelocateSeq(index);
            RomData.MMFileList[4].Data = [];
            RomData.MMFileList[4].Addr = RomData.MMFileList[4].End;
            RomData.MMFileList[4].Cmp_Addr = -1;
            RomData.MMFileList[4].Cmp_End = -1;

            // Update the sequence index pointer table
            f = RomUtils.GetFileIndexForWriting(Addresses.SeqTable);
            for (int i = 0; i < 128; i++)
            {
                ReadWriteUtils.Arr_WriteU32(RomData.MMFileList[f].Data, (Addresses.SeqTable + (i * 16)) - basea, (uint)newSeq[i].Addr);
                ReadWriteUtils.Arr_WriteU32(RomData.MMFileList[f].Data, 4 + (Addresses.SeqTable + (i * 16)) - basea, (uint)newSeq[i].Size);
            }

            // Update the instrument sets for each sequence file
            // This is not the instrument bank file, it's a complementary value for each sequence
            // e.g. Sequence 0x02 uses instrument set 0x03, but it is replaced with sequence 0xAE which needs instrument set 0x3E
            f = RomUtils.GetFileIndexForWriting(Addresses.InstSetMap);
            basea = RomData.MMFileList[f].Addr;
            for (int i = 0; i < 128; i++)
            {
                // huh? paddr? pointer? padding?
                int paddr = (Addresses.InstSetMap - basea) + (i * 2) + 2;

                int j = -1;
                if (newSeq[i].Size == 0) // Pointer, the instrument set needs to be copied to the destination
                {
                    j = sequenceList.FindIndex(u => u.Replaces == newSeq[i].Addr);
                }
                else
                {
                    j = sequenceList.FindIndex(u => u.Replaces == i);
                }

                byte[] formMask = null;
                string name = null;

                if (j != -1)
                {
                    RomData.MMFileList[f].Data[paddr] = (byte)sequenceList[j].Instrument;

                    if (sequenceMaskFileIndex.HasValue)
                    {
                        formMask = sequenceList[j].SequenceBinary?.Formmask;
                    }

                    if (sequenceNamesFileIndex.HasValue)
                    {
                        name = sequenceList[j].DisplayName;
                    }
                }

                if (sequenceMaskFileIndex.HasValue)
                {
                    formMask ??= [.. Enumerable.Repeat<byte>(0xFF, 0x20)];

                    Array.Resize(ref formMask, MusicConfig.SEQUENCE_DATA_SIZE);
                    ReadWriteUtils.Arr_Insert(formMask, 0, MusicConfig.SEQUENCE_DATA_SIZE, RomData.MMFileList[sequenceMaskFileIndex.Value].Data, i * MusicConfig.SEQUENCE_DATA_SIZE);
                }

                if (sequenceNamesFileIndex.HasValue)
                {
                    name ??= "";
                    if (name.Length > MusicConfig.SEQUENCE_NAME_MAX_SIZE - 1)
                    {
                        name = name[..(MusicConfig.SEQUENCE_NAME_MAX_SIZE - 4)] + "...";
                    }

                    name += "\0";
                    var nameBytes = Encoding.ASCII.GetBytes(name);
                    Array.Resize(ref nameBytes, MusicConfig.SEQUENCE_NAME_MAX_SIZE);
                    ReadWriteUtils.Arr_Insert(nameBytes, 0, MusicConfig.SEQUENCE_NAME_MAX_SIZE, RomData.MMFileList[sequenceNamesFileIndex.Value].Data, i * MusicConfig.SEQUENCE_NAME_MAX_SIZE);
                }
            }
        }

        /// <summary>
        /// Patch instructions to use new sequence data file.
        /// </summary>
        /// <param name="f">File index</param>
        /// <remarks>
        /// In memory: 0x80190E5C
        /// Replaces:
        ///   lui     a1, 0x0004
        ///   addiu   a1, a1, 0x6AF0
        /// With:
        ///   lui     t0, 0x800A
        ///   lw      a1, offset (t0)
        /// Note: File table in memory starts at 0x8009F8B0.
        /// </remarks>
        private static void RelocateSeq(int f)
        {
            var fileTable = 0xF8B0;
            var offset = (fileTable + (f * 0x10) + 8) & 0xFFFF;
            ReadWriteUtils.WriteToROM(0x00C2739C, [0x3C, 0x08, 0x80, 0x0A, 0x8D, 0x05, (byte)(offset >> 8), (byte)(offset & 0xFF)]);
        }

        /// <summary>
        /// Moves the audiobank index to unused space on the ROM, increases the audiobank index size,
        /// applies the instrumentset_patch and moveaudiostatebytes patches, and inserts dummy data to overwrite later.
        /// </summary>
        public static void MoveAudioBankTable()
        {
            // Store a copy of the audiobin before it gets moved if it wasn't already created
            if (MM_AUDIOBIN == null)
                LoadMMAudiobin();

            // Grab original AudioBankTable out of code, plus extra for modifying
            var table = ReadWriteUtils.ReadBytes(0xB3C000 + 0x13B6C0, 0x820);
            // Move to unused fbdemo.c (0x80163DC0)
            ReadWriteUtils.WriteToROM(0xB3C000 + 0xBE300, table);

            ReadWriteUtils.WriteU16ToROM(0xB3C000 + 0xBE300, 0x0080); // Increase AudioBankTable amount
            ReadWriteUtils.WriteCodeUInt32(0x80190E18, 0x3C0A8016);
            ReadWriteUtils.WriteCodeUInt32(0x80190E28, 0x254A3DC0);
            NewInstrumentSetAddress = 0xB3C000 + 0xBE300 + 0x10;

            // Clear the old instrument bank
            var zero = new byte[0x2A0];
            ReadWriteUtils.WriteToROM(0xB3C000 + 0x13B6C0, zero);

            // instrumentset_patch: Modifies instrument bank metadata read and writes, instrument/drum/sfx pointer read and writes,
            //                      nops a metadata copy function, and sets a fixed size for the audiobank pointer index.
            ResourceUtils.ApplyHack(Resources.mods.instrumentset_patch);

            // moveaudiostatebytes: Sets where read and writes for sequence and instrumentset states go.
            //                      In this hack, they're moved from 0x80205008 to end of old instrumentset table in code and
            //                      given more space. If these don't get moved, new banks at 0x30 and up will overflow into
            //                      sequence states and can knock out sound.
            ResourceUtils.ApplyHack(Resources.mods.moveaudiostatebytes);


            // Insert dummy metadata (Kamaro's Theme instrument bank duplicates)
            int dummybankindexOffset = NewInstrumentSetAddress + 0x280;
            int totaldummybanks = 0x58;
            ulong dummybankmetadata0 = 0x00021880000000D0;
            ulong dummybankmetadata1 = 0x020101FF01000000;

            for (int dummybankIndex = 0; dummybankIndex <= totaldummybanks; ++dummybankIndex)
            {
                ReadWriteUtils.WriteU64ToROM(dummybankindexOffset, dummybankmetadata0);
                ReadWriteUtils.WriteU64ToROM(dummybankindexOffset + 0x08, dummybankmetadata1);
                dummybankindexOffset += 0x10;
            }
        }

        /// <summary>
        /// Checks if testSeq can be used with any available instrument set slots.
        /// </summary>
        public static bool TestIfAvailableBanks(SequenceInfo testSeq)
        {
            // Check if the instrument set already exists for this sequence
            if (testSeq.SequenceBinary != null && testSeq.SequenceBinary.InstrumentSet != null)
            {
                if (CurrentFreeBank > 0x0080)
                    return false; // Can't overwrite any more entries

                testSeq.SequenceBinary.InstrumentSet.BankSlot = CurrentFreeBank;
            }

            return true; // Sequences with instrument banks, or without needing instrument banks, available
        }

        /// <summary>
        /// Loosens the restrictions on song placement and tries to assign to any available slot
        /// if there are no compatible songs left in the unassigned sequence pool.
        /// If there are still no compatible replacements, it copies a compatible song from the already assigned sequences instead.
        /// </summary>
        public static void TryBackupSongPlacement(SequenceInfo targetSlot, StringBuilder log, List<SequenceInfo> unassignedSequences, OutputSettings settings)
        {
            // First attempt: Merge BGM and Fanfares into a single category then attempt to find a replacement.
            //                The first category of the type is the main type, the rest are secondary.
            SequenceInfo replacementSong = null;
            if (MusicGroups.IsBgmCategory(targetSlot.Categories[0])) // Check if the category is BGM or Cutscene
            {
                replacementSong = unassignedSequences.Find(u => u.Categories[0] <= (int)MusicGroups.Category.Fights || u.Categories[0] == (int)MusicGroups.Category.Cutscenes);
            }
            else //if (targetSlot.Type[0] <= 8) // The category is a Fanfare
            {
                replacementSong = unassignedSequences.Find(u => u.Categories[0] >= (int)MusicGroups.Category.ItemFanfares && u.Categories[0] < (int)MusicGroups.Category.ClearFanfares);
            }

            if (replacementSong != null)
            {
                log.AppendLine(" * generalized replacement with " + replacementSong.Name + " song, with categories: " + string.Join(", ", replacementSong.Categories.Select(x => "0x" + x.ToString("X2"))));
                AssignSequenceSlot(targetSlot, replacementSong, unassignedSequences, "APROX", log);
                return;
            }

            // Second attempt: Copy an already used song.
            replacementSong = RomData.SequenceList.Find(u => u.Categories.Intersect(targetSlot.Categories).Any());
            if (replacementSong != null)
            {
                RomData.SequenceList.Add
                (
                    new SequenceInfo
                    {
                        Name = replacementSong.Name,
                        Directory = replacementSong.Directory,
                        SeqId = replacementSong.SeqId,
                        Categories = replacementSong.Categories,
                        Instrument = replacementSong.Instrument,
                        SequenceBinary = replacementSong.SequenceBinary,
                        PreviousSlot = replacementSong.PreviousSlot,
                        Replaces = targetSlot.Replaces
                    }
                );

                log.AppendLine(" * double dipping with song " + replacementSong.Name + ", with categories: " + string.Join(", ", replacementSong.Categories.Select(x => "0x" + x.ToString("X2"))));
                log.AppendLine($"{targetSlot.Name,-40} {"COPY",+10} -> " + replacementSong.Name);

                return;
            }

            // should not make it this far, throw error
            log.AppendLine(" out of remaining songs:");

            foreach (SequenceInfo RemainingSong in unassignedSequences)
            {
                log.AppendLine(" * [" + RemainingSong.Name + "] with categories [" + string.Join(",", RemainingSong.Categories) + "]");
            }

            WriteSongLog(log, settings);

            throw new Exception($"TryBackupSongPlacement Error: Cannot randomize music for current seed with available music: \nSlot Name:[{targetSlot.Name}] PreviousSlot: [{targetSlot.Replaces:X}]");
        }

        /// <summary>
        /// Writes the song log; if there's a spoiler log available it's written at the end of the spoiler log.
        /// </summary>
        public static void WriteSongLog(StringBuilder log, OutputSettings settings)
        {
            string dir = Path.GetDirectoryName(settings.OutputROMFilename);
            string path = $"{Path.GetFileNameWithoutExtension(settings.OutputROMFilename)}";

            // The spoiler log should already be written at this point
            // If there's no text log, create a separate song log
            if (File.Exists(Path.Combine(dir, path + "_SpoilerLog.txt")))
            {
                path += "_SpoilerLog.txt";
            }
            else
            {
                path += "_SongLog.txt";
            }

            using var writer = new StreamWriter(Path.Combine(dir, path), append: true);
            writer.WriteLine(""); // spacer between spoiler log and song log
            writer.Write(log);
        }

        /// <summary>
        /// Runs through the list of instrument banks to find a matching instrument bank, then assigns a matching matching bank if one is found.
        /// This reduces the amount of instrument sets required to inject when building the ROM.
        /// </summary>
        private static (int sequenceBankIndex, int bankListIndex) FindMatchingInstrumentSetDuplicate(SequenceInfo replacementSequence)
        {
            var bank = replacementSequence.SequenceBinary.SequenceData;

            if (bank != null)
            {
                var searchResult = RomData.InstrumentSetList.FindIndex(match => match.BankBinary == bank);

                if (searchResult != -1)
                    return (0, searchResult);
            }

            return (-1, -1);
        }

        /// <summary>
        /// Assigns the replacement sequence to the target sequence slot, then writes the replacement to the spoiler/song log.
        /// </summary>
        public static void AssignSequenceSlot(SequenceInfo slotSequence, SequenceInfo replacementSequence, List<SequenceInfo> remainingSequences, string debugString, StringBuilder log)
        {
            // If the song has a custom instrument set: lock the sequence, update the instrument set value, and write debug output
            if (replacementSequence.SequenceBinary != null && replacementSequence.SequenceBinary != null && replacementSequence.SequenceBinary.InstrumentSet != null)
            {
                (int sequenceBankIndex, int bankListIndex) = FindMatchingInstrumentSetDuplicate(replacementSequence);
                if (sequenceBankIndex != -1)
                {
                    RomData.InstrumentSetList[bankListIndex].Modified += 1;
                    replacementSequence.Instrument = bankListIndex;

                    log.AppendLine($" -- v -- Instrument set number {replacementSequence.Instrument:X2} is being reused -- v --");
                }
                else // No duplicate instrument bank found, add a new one
                {
                    replacementSequence.Instrument = CurrentFreeBank++; // Update the instrument bank that will be used
                    replacementSequence.SequenceBinary.InstrumentSet.BankSlot = replacementSequence.Instrument;

                    RomData.InstrumentSetList[replacementSequence.Instrument] = replacementSequence.SequenceBinary.InstrumentSet;
                    RomData.InstrumentSetList[replacementSequence.Instrument].InstrumentSamples = replacementSequence.InstrumentSamples;

                    log.AppendLine($" -- v -- Instrument set number {replacementSequence.Instrument:X2} has been claimed -- v --");
                }
            }

            replacementSequence.Replaces = slotSequence.Replaces; // Determines what song will be placed in slot_seq later

            // -40 and +10 pad the text to align in the same middle area for visual clarity
            log.AppendLine($"{slotSequence.Name,-40} {debugString,+10} -> " + $"{replacementSequence.Name} {(replacementSequence.Game != null ? $"({replacementSequence.Game.ToUpper()})" : "")}");
            remainingSequences.Remove(replacementSequence);
        }

        #region Songtest and Songforce
        /// <summary>
        /// Checks if a music file uses the 'songtest' debug token, then places the music file into
        /// specific music slots for easier music testing.
        /// </summary>
        public static void CheckSongTest(List<SequenceInfo> sequences, StringBuilder log)
        {
            // For creators: Songtest is a debug token in the song filename. It specifies
            // to the rando that the music pool should be flooded with the song for testing.

            SequenceInfo songtestSequence = RomData.SequenceList.Find(u => u.Name.Contains("songtest") == true);
            if (songtestSequence == null)
                return;

            // Songtest always replaces the following: "File Select", "Title Demo", "Clock Town (Day 1)", and "Small Enemy Battle"
            SequenceInfo fileselectSlot = RomData.TargetSequences.Find(u => u.Replaces == FILE_SELECT); // Don't rely on the name in the SEQS.txt, rely on the sequence ID instead
            AssignSequenceSlot(fileselectSlot, songtestSequence, sequences, "SONGTEST", log); // File Select

            // Because song testing is the focus, adjust the budget now
            var songtestSize = GetSequenceSize(songtestSequence);
            if (songtestSequence.Categories.Contains((int)MusicGroups.Category.ActionThemes) || songtestSequence.Categories.Contains((int)MusicGroups.Category.SmallEnemy))
            {
                MAX_COMBAT_BUDGET = songtestSize;
                MAX_BGM_BUDGET = MAX_TYPE2_MUSIC_BUDGET - MAX_COMBAT_BUDGET;
            }
            // else if Not Fanfare or Cutscene
            // This doesn't account for individual Fanfare categories
            else if (!(songtestSequence.Categories.Contains((int)MusicGroups.Category.ItemFanfares)
                    || songtestSequence.Categories.Contains((int)MusicGroups.Category.EventFanfares)
                    || songtestSequence.Categories.Contains((int)MusicGroups.Category.ClearFanfares)
                    || songtestSequence.Categories.Contains((int)MusicGroups.Category.Cutscenes)))
            {
                MAX_BGM_BUDGET = songtestSize;
                MAX_COMBAT_BUDGET = MAX_TYPE2_MUSIC_BUDGET - MAX_BGM_BUDGET;
            }

            ConvertSequenceSlotToPointer(TITLE_DEMO, FILE_SELECT);   // Point "Title Demo" to "File Select"
            ConvertSequenceSlotToPointer(CLOCK_TOWN_1, FILE_SELECT); // Point "Clock Town (Day 1)" to "File Select"

            // Additionally, every song that shares a category with the song should be added
            var allMatchingSlots = RomData.TargetSequences.FindAll(u => u.Categories.Intersect(songtestSequence.Categories).Any());
            allMatchingSlots.Remove(fileselectSlot); // Don't re-pointerize it

            foreach (SequenceInfo songslot in allMatchingSlots)
            {
                // targetSlot will encounter a null value if combat is removed from RomData.TargetSequences
                // So combat can't be pointerized unless something changes with how song slots work....
                if (songslot.Replaces == SMALL_ENEMY_BATTLE)
                    continue;

                ConvertSequenceSlotToPointer(songslot.Replaces, FILE_SELECT); // Point replacement to "File Select"
            }

            RomData.TargetSequences.Remove(fileselectSlot);

            // Additionally, because songs that use custom banks replace the original bank by design,
            // the replacement bank should be a super set of the original and old songs should still work.
            // However, sometimes the old instruments in the new bank are broken and need to be tested.
            // To do so, the lottery will become a new song slot with a vanilla song using the songtest
            // sequence's instrument bank.

            if (songtestSequence.SequenceBinary == null)
                return; // The song doesn't have a custom instrument bank, no need to continue

            void ConvertRoomForSongTest(int sceneFID, int roomFID, int actorIDOffset, int musicOffset, List<SequenceInfo> replacementSequences)
            {
                if (replacementSequences.Count > 0)
                {
                    var validSequence = replacementSequences[0];                // Pull a sequence from the randomized list
                    var newSlot = RomData.PointerizedSequences[0].PreviousSlot; // Recycle the list of slots converted to pointers
                    RomData.PointerizedSequences.RemoveAt(0);
                    validSequence.Replaces = newSlot;                           // Update the sequence to use the chosen slot
                    replacementSequences.Remove(validSequence);
                    sequences.Remove(validSequence);

                    log.AppendLine($" -- ^ -- Instrument set number {validSequence.Instrument:X2} also used by {validSequence.Name}");

                    // Set the scene to use this new song as the background music
                    RomUtils.CheckCompressed(sceneFID);
                    var scene = RomData.MMFileList[sceneFID].Data;
                    scene[musicOffset] = (byte)newSlot;
                }

                RomUtils.CheckCompressed(roomFID); // Mute the previous music by killing the SFX actor that plays the filtered shop music
                var room = RomData.MMFileList[roomFID].Data;
                room[actorIDOffset] = 0xFF; // Kill the SFX actor by setting its room slot ID to -1
                room[actorIDOffset + 1] = 0xFF;
            }

            // Generate a list of sequence that use the vanilla version of the bank the songtest sequence replaces
            var sharedBankSequences = RomData.SequenceList.FindAll(u => u.Instrument == songtestSequence.Instrument);
            sharedBankSequences.Remove(songtestSequence);
            sharedBankSequences.Remove(fileselectSlot);   // File Select is already set, so the values are broken
            sharedBankSequences.RemoveAll(u => u.SequenceBinary != null);

            Random newRandom = new();
            sharedBankSequences = [.. sharedBankSequences.OrderBy(x => newRandom.Next())]; // Random shuffle

            ConvertRoomForSongTest(sceneFID: 1334, 1335, actorIDOffset: 0x98, 0x7, sharedBankSequences); // Lottery
            ConvertRoomForSongTest(sceneFID: 1158, 1159, actorIDOffset: 0x88, 0x7, sharedBankSequences); // Honey & Darling
            ConvertRoomForSongTest(sceneFID: 1188, 1189, actorIDOffset: 0x88, 0x7, sharedBankSequences); // Treasure Chest Game Shop
            ConvertRoomForSongTest(sceneFID: 1502, 1503, actorIDOffset: 0xC4, 0x7, sharedBankSequences); // Bomb Shop
        }

        /// <summary>
        /// Checks if a music file uses the 'songforce' priority token, then forces the music file to the top of the music pool.
        /// </summary>
        public static void CheckSongForce(List<SequenceInfo> sequences, StringBuilder log, Random rng)
        {
            List<SequenceInfo> forcedSequences = [.. RomData.SequenceList.FindAll(u => u.Name.Contains("songforce") == true).OrderBy(x => rng.Next())];
            if (forcedSequences != null && forcedSequences.Count > 0)
            {
                foreach (SequenceInfo seq in forcedSequences)
                {
                    log.AppendLine($"Forcing song ({seq.Name}) to top of the song pool");
                    sequences.Remove(seq);
                    sequences.Insert(0, seq);
                }
            }
        }
        #endregion

        /// <summary>
        /// Runs through the list of unassigned sequences and searches for a valid sequence slot it can replace, then assigns the sequence to the slot if it fits.
        /// </summary>
        public static bool SearchForValidSongReplacement(CosmeticSettings cosmeticSettings, List<SequenceInfo> unassignedSequences, SequenceInfo targetSlot, Random rng, StringBuilder log)
        {
            // This could be replaced with a findall(compatible types), but then the random category gacha is lost
            foreach (var testSeq in unassignedSequences.ToList())
            {
                // Increases the change of getting non-MM music, but only if there's lost of music remaining
                // Disabled until this can be modified in the UI, there's enough music now so it feels unnecessary
                //if (unassigned.Count > 77 && testSeq.Name.StartsWith("mm") && testSeq.Type[0] < 0x100 && (random.Next(100) < 40))
                //    continue;

                // Check if the current song still has available instrument banks or sequences, if not remove the song and continue
                if (!TestIfAvailableBanks(testSeq))
                    continue; // The song is unacceptable

                var maxSize = targetSlot.Replaces == SMALL_ENEMY_BATTLE ? MAX_COMBAT_BUDGET : MAX_BGM_BUDGET;
                if (GetSequenceSize(testSeq) > maxSize)
                    continue; // The song is too big

                // Check if the target and the possible match share a category
                if (testSeq.Categories.Intersect(targetSlot.Categories).Any())
                {
                    AssignSequenceSlot(targetSlot, testSeq, unassignedSequences, "", log);
                    return true;
                }

                // Deathbasket wanted there to be a small chance of getting out of category music, but
                // did not want to mix BGM and fanfares — or vice versa
                else if (unassignedSequences.Count > 30
                    && testSeq.Categories.Count > targetSlot.Categories.Count
                    && cosmeticSettings.MusicLuckRollChance > 0
                    && (decimal)(rng.NextDouble() * 100.0) < cosmeticSettings.MusicLuckRollChance
                    && targetSlot.Categories[0] <= (int)MusicGroups.Category.Cutscenes
                    && testSeq.Categories[0] <= (int)MusicGroups.Category.Cutscenes
                    && (testSeq.Categories[0] & (int)MusicGroups.Category.ItemFanfares) == (targetSlot.Categories[0] & (int)MusicGroups.Category.ItemFanfares)
                    && testSeq.Categories.Contains((int)MusicGroups.Category.ClearFanfares) == targetSlot.Categories.Contains((int)MusicGroups.Category.ClearFanfares)
                    && !testSeq.Categories.Contains((int)MusicGroups.Category.Cutscenes))
                {
                    AssignSequenceSlot(targetSlot, testSeq, unassignedSequences, "LUCK", log);
                    return true;
                }
            }

            return false; // Exhausted available songs
        }

        /// <summary>
        /// Compares the chosen BGM sequence's size against the chosen combat music's size, chooses which is the limiting factor, then sees if it can place combat music.
        /// </summary>
        public static void CheckBGMCombatMusicBudget(CosmeticSettings cosmeticSettings, List<SequenceInfo> unassignedSequences, Random rng, StringBuilder log)
        {
            // For any given scene, BGM and Small Enemy Battle music share the same buffer, loading to the other side. If their sum
            // is greater than the size of the buffer, they clip into one another when one loads — this kills one of them, usually BGM.

            var combatSequences = RomData.SequenceList.FindAll(u => u.Categories.Contains((int)MusicGroups.Category.ActionThemes));
            var BGMSlots = RomData.TargetSequences.FindAll(u => u.Categories.Contains((int)MusicGroups.Category.Fields)
                                                             || u.Categories.Contains((int)MusicGroups.Category.Dungeons)
                                                             || u.SeqId == DEKU_PALACE); // 0x12: Deku Palace (has enemies)

            List<SequenceInfo> usedBGMSequences = [];
            foreach (var slot in BGMSlots)
            {
                var searchResult = RomData.SequenceList.Find(u => u.Replaces == slot.Replaces);

                if (searchResult != null)
                {
                    usedBGMSequences.Add(searchResult);
                }
            }

            // BGM or combat is the limiting factor, the other has to be smaller than the chosen limiter
            bool combatVsBGMCoinToss = rng.Next(2) == 1;

            var usedCombatSequence = RomData.SequenceList.Find(u => u.Replaces == SMALL_ENEMY_BATTLE && u.SeqId != SMALL_ENEMY_BATTLE); // SequencesList has Replaces as -1, use SeqId (u.Name != "mm-combat")
            if (usedCombatSequence == null) // Songtest removes the sequence and points it at "File Select" for testing
            {
                combatVsBGMCoinToss = true; // "COMBAT" manually selected because of combat songtest
                usedCombatSequence = RomData.SequenceList.Find(u => u.Replaces == FILE_SELECT && u.SeqId != FILE_SELECT); // SequencesList has Replaces as -1, use SeqId (u.Name != "mm-fileselect")
            }
            else if (RomData.SequenceList.Find(u => u.Name.Contains("songtest")) != null)
            {
                combatVsBGMCoinToss = false; // "BGM" manually selected because of non-combat songtest
            }

            if (cosmeticSettings.DisableCombatMusic)
            {
                combatVsBGMCoinToss = false; // "BGM" manually selected because combat music is disabled
            }

            var combatSize = GetSequenceSize(usedCombatSequence);

            string coinResult = (combatVsBGMCoinToss ? ("COMBAT") : ("BGM"));
            log.AppendLine($" SECOND PASS: Scanning for oversized BGM or combat cointoss: ({coinResult})");

            if (combatVsBGMCoinToss) // Combat chosen
            {
                if (usedBGMSequences.Count <= 0)
                    return;

                // Get new BGM budget from combat sequence
                var newBGMBudget = MAX_BGM_BUDGET = MAX_TYPE2_MUSIC_BUDGET - combatSize;
                log.AppendLine($" new BGM budget: {MAX_BGM_BUDGET:X}, from combat size: {combatSize:X}");

                // Per BGM sequence
                foreach (var seq in usedBGMSequences)
                {
                    var bgmSeqSize = GetSequenceSize(seq);
                    if (bgmSeqSize > newBGMBudget)
                    {
                        var seqName = seq.Name;
                        log.AppendLine($"BGM sequence {seqName} was too big to match your combat music, replacing ... ");

                        var bgmSlot = RomData.TargetSequences.Find(u => u.Replaces == seq.Replaces);
                        seq.Replaces = -1; // Cancel using this song

                        if (!SearchForValidSongReplacement(cosmeticSettings, unassignedSequences, bgmSlot, rng, log))
                        {
                            throw new Exception("CheckBGMCombatMusicBudget Error: Current seed cannot find acceptable music for the combat slot\n" + "Try a different seed!");
                        }
                    }
                }

            }
            else // BGM chosen
            {
                // Get new combat budget by comparing to the largest BGM sequence
                var largestBGMSize = usedBGMSequences.Max(s => (int?)GetSequenceSize(s)) ?? 0;
                log.AppendLine($" new Combat budget: {MAX_COMBAT_BUDGET:X} from bgm size: {largestBGMSize:X}");

                // Per BGM sequence
                var newCombatBudget = MAX_COMBAT_BUDGET = MAX_TYPE2_MUSIC_BUDGET - largestBGMSize;
                if (combatSize > newCombatBudget)
                {
                    var seqName = usedCombatSequence.Name;
                    log.AppendLine($"Combat sequence {seqName} was too big to match your BGM music, replacing ... ");

                    var combatSlot = RomData.TargetSequences.Find(u => u.Replaces == SMALL_ENEMY_BATTLE); // TargetSequences has SeqId as -1, use Replaces (u.Name == "mm-combat")
                    usedCombatSequence.Replaces = -1; // Cancel using this song

                    if (!SearchForValidSongReplacement(cosmeticSettings, unassignedSequences, combatSlot, rng, log))
                    {
                        throw new Exception("CheckBGMCombatMusicBudget Error: Current seed cannot find acceptable music for the combat slot\n" + "Try a different seed!");
                    }
                }

            }
        }

        /// <summary>
        /// Runs through the audiobank index, loads its data into memory, then uses that data to
        /// generate a list of banks from the vanilla game that can be modified as needed.
        /// </summary>
        public static void ReadInstrumentSetList()
        {
            RomData.InstrumentSetList = [];

            // The index list can go up to 0x80 with current extended instrument bank table file
            for (int audiobankIndex = 0; audiobankIndex <= 0x80; ++audiobankIndex)
            {
                // Each instrument bank has 16 bytes of data, the first 4 bytes is the bank's address in the audiobank MMFile,
                // the second 4 bytes is the bank's size in the audiobank MMFile, and the last 8 bytes are metadata.
                //
                // [ item (number of bytes) ]
                // [ address (4), size (4), sample medium (1), sequence player (1), table id (1), font id (1), num insts (1), num drums (1), num sfx (2) ]
                int audiobankIndexAddr = NewInstrumentSetAddress + (audiobankIndex * 0x10);
                int audiobankBankOffset = (ReadWriteUtils.ReadU16(audiobankIndexAddr) << 16) + ReadWriteUtils.ReadU16(audiobankIndexAddr + 2);
                int bankLength = (ReadWriteUtils.ReadU16(audiobankIndexAddr + 4) << 16) + ReadWriteUtils.ReadU16(audiobankIndexAddr + 6);

                byte[] bankMetadata = new byte[8];
                for (int b = 0; b < 8; ++b)
                {
                    bankMetadata[b] = ReadWriteUtils.Read(audiobankIndexAddr + 8 + b);
                }

                byte[] bankData = new byte[bankLength];
                for (int b = 0; b < bankLength; ++b)
                {
                    bankData[b] = ReadWriteUtils.Read(Addresses.Audiobank + audiobankBankOffset + b);
                }

                var newInstrumentSet = new InstrumentSetInfo
                {
                    BankSlot = audiobankIndex,
                    BankMetaData = bankMetadata,
                    BankBinary = bankData
                };

                RomData.InstrumentSetList.Add(newInstrumentSet);
            }
        }

        /// <summary>
        /// Updates the custom audio sample pointers in each instrument bank to properly point at their data in the ROM.
        /// </summary>
        public static void UpdateBankInstrumentPointers(byte[] ROM)
        {
            // The instrument bank and new samples are already written to the ROM file, now the pointers need to be updated.
            // It's not currently possible to know where the audio samples are written until they're actually written to ROM.
            // This is because the pointer is an offset to the soundbank ROM location, and both can shift in BuildROM()
            //
            // Previously, the samples were updated by going through the bank binary byte by byte, but now the bank is parsed and
            // the sample offsets are obtained — this allows the samples to be linked by type, index, and key region (if INST).
            // If the type, index, and key region are null and the temp address is present, then the system fallsback to searching
            // for the sample. However, because the bank is parsed it can search only sample addresses. This gets rid of the risk
            // of overwriting other data (it was common that ADPCM prediction coefficients would be overwritten if the address was small).
            // If the temp address is matched to a sample address, the parent type is gotten and the offset to the sample is obtained
            // using the stored index in the parent's struct.
            //
            if (RomData.InstrumentSetList == null)
                return;

            int soundbankAddr = RomData.MMFileList[5].Cmp_Addr; // In vanilla it's 0x97F70, but it can be shifted because MMR changes AudioSeq's location
            int audiobankInstSetAddr = RomData.MMFileList[3].Cmp_Addr; // Point to a specific instrument set, starting with 0 and updating per loop
            foreach (var instrumentset in RomData.InstrumentSetList)
            {
                if (instrumentset.InstrumentSamples != null && instrumentset.InstrumentSamples.Count > 0)
                {
                    foreach (var sample in instrumentset.InstrumentSamples)
                    {
                        // Get the new sample address from the ROM
                        int newSampleAddress = RomData.MMFileList[RomData.SamplesFileID].Cmp_Addr
                                             + (int)RomData.ListOfSamples.Find(u => u.Hash == sample.Hash).Addr
                                             - soundbankAddr;

                        byte[] newAddressBytes = BitConverter.GetBytes(newSampleAddress);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(newAddressBytes); // Ensure the byte[] is big endian so indices aren't reversed

                        // Parse the audiobank binary to find the sample address offsets
                        AudiobankUtils.Audiobank instrumentBank = null;
                        try
                        {
                            instrumentBank = new(instrumentset.BankMetaData, instrumentset.BankBinary, null, null);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                        }
                        
                        uint sampleBankAddress = 0;
                        if (sample.InstrumentType != null && sample.ListIndex != -1 && sample.Marker == 0) // Key region can be null
                        {
                            // Get the offset to the sample using the given type, index, and region
                            sampleBankAddress = sample.InstrumentType switch
                            {
                                "INST" => sample.KeyRegion switch
                                {
                                    "LOW"  => instrumentBank.Instruments[sample.ListIndex].LowSampleAddress,
                                    "PRIM" => instrumentBank.Instruments[sample.ListIndex].PrimSampleAddress,
                                    "HIGH" => instrumentBank.Instruments[sample.ListIndex].HighSampleAddress,
                                    _      => throw new Exception($"UpdateBankInstrumentPointers Error: Invalid audio sample key region in metadata for song: '{sample.ParentFile}'")
                                },

                                // Drums and SFX don't have key regions
                                "DRUM" => instrumentBank.Drums[sample.ListIndex].SampleAddress,
                                "SFX"  => instrumentBank.Effects[sample.ListIndex].SampleAddress,
                                _      => throw new Exception($"UpdateBankInstrumentPointers Error: Invalid audio sample type in metadata for song: '{sample.ParentFile}'")
                            };
                        }
                        else // Fallback to sample marker matching
                        {
                            // Instead of searching byte by byte, collect all the samples, match the address, then get the offset to the matched address
                            // With this, there should be no accidental overwrites of data in the instrument bank 
                            foreach (var s in instrumentBank.GetBankSamples())
                            {
                                // The bank offset gets stored in the sample struct, there's no need to find the parent
                                // structure and link it that way
                                if (sample.Marker == s.Address)
                                    sampleBankAddress = s.BankOffset;

                                if (sampleBankAddress != 0)
                                    break;

                                // If this is the last sample and there is still no match, exit loop
                                if (s == instrumentBank.GetBankSamples().Last())
                                    break;
                            }
                        }

                        if (sampleBankAddress == 0)
                            throw new Exception($"UpdateBankInstrumentPointers Error: Could not match audio sample's address ('{sample.Marker:X}') to any sample addresses in the instrument bank for song: '{sample.ParentFile}'");

                        // Replace the sample struct's address with the correct address
                        // The first 4 bytes are a bitfield, so add 4 to the index
                        for (int i = 0; i < 4; i++)
                        {
                            ROM[audiobankInstSetAddr + sampleBankAddress + i + 4] = newAddressBytes[i];
                        } 
                    }
                }

                // Instrument banks are stored decompressed one right after the other in the audiobank MMFile
                // So the next audiobank to examine is the current bank's size offset from the current offset
                audiobankInstSetAddr += instrumentset.BankBinary.Length;
            }
        }

        /// <summary>
        /// Writes all custom sample files to an MMFile at the end of the ROM.
        /// </summary>
        public static void WriteNewSoundSamples(List<InstrumentSetInfo> InstrumentSetList)
        {
            // In the event there are no more MMFile DMA indices, these files don't need to be a hard file in the filesystem,
            // they can be placed anywhere after the soundbank starting address on ROM. The instrument sample lookup doesn't use
            // the file system, but adding to the file system is useful for shifting in BuildROM().

            // Issue: it's unknown where the samples will be written to at this point, because BuildROM() will shift the files.
            // For now, write the audio samples after the audiobank/soundbank/samples are written, then update the audiobank
            // pointers.
            // Save extra soundsamples FID, and the samples with their data, for later (UpdateBankInstrumentPointers())

            int fid = RomData.SamplesFileID = RomUtils.AppendFile([]);
            RomData.ListOfSamples = [];

            // For each custom instrument set that needs a custom audio sample
            foreach (InstrumentSetInfo instrumentSet in InstrumentSetList)
            {
                if (instrumentSet.InstrumentSamples != null && instrumentSet.InstrumentSamples.Count > 0)
                {
                    foreach (SequenceSoundSampleBinaryData sample in instrumentSet.InstrumentSamples)
                    {
                        // Test if sample was already added by another song
                        var previouslyWrittenSample = RomData.ListOfSamples.Find(u => sample.Hash == u.Hash);
                        if (previouslyWrittenSample == null) // If sample not already written
                        {
                            sample.Addr = (uint)RomData.MMFileList[fid].Data.Length; // Get the ROM addr of the new file, the file will start at the end of the last file
                            RomData.MMFileList[fid].Data = [.. RomData.MMFileList[fid].Data, .. sample.BinaryData]; // Concat the sample to sample collection file

                            int paddingRemainder = RomData.MMFileList[fid].Data.Length % 0x10; // Samples may not need to be 16 byte aligned, but just in case
                            if (paddingRemainder > 0)
                            {
                                RomData.MMFileList[fid].Data = [.. RomData.MMFileList[fid].Data, .. new byte[paddingRemainder]];
                            }

                            RomData.ListOfSamples.Add(sample);
                        }
                        else // Get the address of the previously used audio sample
                        {
                            sample.Addr = previouslyWrittenSample.Addr;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Rebuilds the audiobank MMFile and writes it to the ROM file.
        /// </summary>
        public static void RebuildAudioBank(List<InstrumentSetInfo> InstrumentSetList)
        {
            // Get the index for the old instrument bank, it will be put in the same spot while
            // letting it expand into the AudioSeq's spot that has been moved to the end
            int fid = RomUtils.GetFileIndexForWriting(NewInstrumentSetAddress);

            // The DMA table doesn't point directly to the indextable on the ROM, its part of a larger yaz0 file, an offset is required to get the address in the file
            int audiobankIndexOffset = NewInstrumentSetAddress - RomData.MMFileList[RomUtils.GetFileIndexForWriting(NewInstrumentSetAddress)].Addr;

            int audiobankBankOffset = 0;
            //byte[] audiobankData = [];
            List<byte> audiobankData = [];

            // For each instrument bank, concat onto the new object, then update the table to match the new instrument sets.
            // CurrentFreeBank is used so not all unused dummy instrument banks get written to ROM.
            for (int audiobankIndex = 0; audiobankIndex <= CurrentFreeBank; ++audiobankIndex)
            {
                var currentBank = InstrumentSetList[audiobankIndex];
                //audiobankData = [.. audiobankData, .. currentBank.BankBinary];
                audiobankData.AddRange(currentBank.BankBinary);

                // Update address of the instrument bank in the index table
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 0] = (byte)((audiobankBankOffset & 0xFF000000) >> 24);
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 1] = (byte)((audiobankBankOffset & 0xFF0000) >> 16);
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 2] = (byte)((audiobankBankOffset & 0xFF00) >> 8);
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 3] = (byte)(audiobankBankOffset & 0xFF);

                // Update length of the instrument bank in the table
                int currentBankLength = currentBank.BankBinary.Length;
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 4] = (byte)((currentBankLength & 0xFF000000) >> 24);
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 5] = (byte)((currentBankLength & 0xFF0000) >> 16);
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 6] = (byte)((currentBankLength & 0xFF00) >> 8);
                RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 7] = (byte)(currentBankLength & 0xFF);

                // Update the metadata of the instrument bank in the table
                for (int metadataIter = 0; metadataIter < 8; ++metadataIter)
                {
                    RomData.MMFileList[fid].Data[audiobankIndexOffset + (audiobankIndex * 16) + 8 + metadataIter] = currentBank.BankMetaData[metadataIter];
                }

                // Adjust the address for the next instrument bank to use
                audiobankBankOffset += currentBankLength;
                int paddingRemainder = currentBankLength % 0x10;
                if (paddingRemainder > 0) // In the event the user made an instrument bank that isn't 16 byte aligned
                {
                    //audiobankData = [.. audiobankData, .. new byte[paddingRemainder + 0x10]]; // Padding with an extra line is cheap enough to try
                    audiobankData.AddRange(new byte[paddingRemainder + 0x10]);
                    audiobankBankOffset += paddingRemainder;
                }
            }

            // Write the new instrument bank back to file
            var audiobankFile = RomData.MMFileList[RomUtils.GetFileIndexForWriting(Addresses.Audiobank)];
            audiobankFile.Data = [.. audiobankData];
            audiobankFile.End = audiobankFile.Addr + audiobankFile.Data.Length;
        }

        /// <summary>
        /// Represents the files stored within a .mmrs or .ootrs music file.
        /// </summary>
        private class MusicArchiveContents
        {
            public ZipArchiveEntry MetaFile { get; set; }
            public ZipArchiveEntry SequenceFile { get; set; }
            public ZipArchiveEntry BankFile { get; set; }
            public ZipArchiveEntry BankmetaFile { get; set; }
            public ZipArchiveEntry FormmaskFile { get; set; }
            public ZipArchiveEntry CategoriesFile { get; set; }
            public List<ZipArchiveEntry> AudioSamples { get; set; } = [];
        }

        /// <summary>
        /// Represents the data contained within a music file's '.metadata' metadata YAML file.
        /// </summary>
        private class MusicMetadata
        {
            public string Game {  get; set; }
            public string CosmeticName { get; set; }
            public string InstrumentSet { get; set; }
            public string SongType { get; set; }
            public List<int> Categories { get; set; } = [];
            public List<Dictionary<string, object>> Commands { get; set; } = [];
            public SequencePlayState[] Formmask { get; set; }
        }

        /// <summary>
        /// Represents the YAML dictionary data stored in the SEQS file.
        /// </summary>
        public class SEQSYaml
        {
            [YamlMember(Alias = "display name")]
            public string DisplayName { get; set; }

            [YamlMember(Alias = "music groups")]
            public List<object> MusicGroups { get; set; }

            [YamlMember(Alias = "instrument set")]
            public int InstrumentSet { get; set; }

            [YamlMember(Alias = "sequence id")]
            public int SequenceId { get; set; }

            [YamlMember(Alias = "song type")]
            public string SongType { get; set; }

            [YamlMember(Alias = "no recycle")]
            public bool NoRecycle { get; set; } = false;
        }
    }
}
