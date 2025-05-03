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

namespace MMR.Randomizer.Utils
{
    public class SequenceUtils
    {
        // These are scenes the play may never visit, if they do, then they are visited very briefly and very little music is heard
        public static readonly List<int> lowUseMusicSlots = new()
        {
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
        };

        public static int MAX_BGM_BUDGET = 0x6000; // Vanilla: 0x3800
        public static int MAX_COMBAT_BUDGET = 0x6000; // unk
        public static int MAX_TYPE2_MUSIC_BUDGET = 0x6000; // Vanilla: 0x4100

        public static int New_AudioBankTable = 0; // For mmfilelist
        public static int NewInstrumentSetAddress; // For BGM shuffle, used to store AudioBankTable address
        public static int CurrentFreeBank = 0x29;
        public const  int REQUIRES_NEW_BANK = 0x28; // 0x28 used to be the only free bank, used to indicate custom banks

        public static MD5 md5lib; // Used for zip

        public static void ResetBudget()
        {
            MAX_BGM_BUDGET = 0x6000;
            MAX_COMBAT_BUDGET = 0x6000;
            MAX_TYPE2_MUSIC_BUDGET = 0x6000;
        }

        public static bool TryParseCategory(object input, out int value)
        {
            // Ensures that categories return their proper int value if they are a string
            
            value = 0;

            // Handle ints
            if (input is int intValue)
            {
                value = intValue;
                return true;
            }

            // Handle strings
            if (input is string trimmed)
            {
                trimmed = trimmed.Trim();

                if (trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(trimmed.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int parsed))
                    {
                        value = parsed;
                        return true;
                    }
                }

                if (int.TryParse(trimmed, out int intVal))
                {
                    value = intVal;
                    return true;
                }

                // Try enum values
                if (Enum.TryParse<MusicGroups.Category>(trimmed, true, out var category))
                {
                    value = (int)category;
                    return true;
                }

                // Try dictionary
                if (MusicGroups.CategoryDisplayNames.TryGetValue(trimmed, out category))
                {
                    value = (int)category;
                    return true;
                }
            }

            return false;
        }

        public static void ReadSequenceInfo()
        {
            md5lib = MD5.Create();

            RomData.SequenceList = new List<SequenceInfo>();
            RomData.TargetSequences = new List<SequenceInfo>();

            // If the user has a SEQS.yml file, use it instead of the one in resources
            string seqsContent;

            if (File.Exists(Path.Combine(Values.MusicDirectory, "SEQS.yml")))
            {
                Debug.WriteLine("We found a user SEQS.yml file that we can use");
                seqsContent = File.ReadAllText(Path.Combine(Values.MusicDirectory, "SEQS.yml"));
            }
            else
            {
                seqsContent = Properties.Resources.SEQS;
            }

            var sequenceEntries = YamlSerializer.Deserialize<Dictionary<string, SEQSYaml>>(seqsContent);

            // If the music directory doesn't exist, create it because it's required still
            if (!Directory.Exists(Values.MusicDirectory))
            {
                Directory.CreateDirectory(Values.MusicDirectory);
            }

            // Search through every directory in the music folder
            IEnumerable<string> directories = new[] { Values.MusicDirectory }.Concat(Directory.EnumerateDirectories(Values.MusicDirectory, "*", SearchOption.AllDirectories));

            foreach (string directory in directories)
            {
                try
                {
                    foreach (var entry in sequenceEntries)
                    {
                        string seqName = entry.Key;
                        var data = entry.Value;

                        var seqCategories = new List<int>();
                        foreach (var part in data.MusicGroups)
                        {
                            if (TryParseCategory(part, out int c) && !seqCategories.Contains(c))
                            {
                                seqCategories.Add(c);
                            }
                            else
                            {
#if DEBUG
                                throw new Exception($"Invalid category in YAML for '{seqName}': '{part}'");
#else
                                continue;
#endif
                            }
                        }

                        int seqInstrument = data.InstrumentSet;
                        int seqId = data.SequenceId;

                        SequenceInfo targetSequence = new()
                        {
                            Name = seqName,
                            DisplayName = data.DisplayName ?? seqName,
                            Categories = seqCategories,
                            Instrument = seqInstrument,
                        };

                        SequenceInfo sourceSequence = new()
                        {
                            Name = seqName,
                            DisplayName = data.DisplayName ?? seqName,
                            Categories = seqCategories,
                            Instrument = seqInstrument,
                        };

                        if (sourceSequence.Name.StartsWith("mm-"))
                        {
                            targetSequence.Replaces = data.SequenceId;
                            sourceSequence.SeqId = data.SequenceId;

                            if (data.NoRecycle)
                            {
                                sourceSequence.Name = "drop";
                            }

                            if (RomData.TargetSequences.Find(u => u.Name == seqName) == null)
                            {
                                RomData.TargetSequences.Add(targetSequence);
                            }
                        }
                        else
                        {
                            if (File.Exists(Path.Combine(directory, seqName)))
                            {
                                sourceSequence.Directory = directory;
                            }
                            else
                            {
                                continue;
                            }
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
                        DisplayName = "MMR - Song of Time",
                        Categories = new List<int> { (int)MusicGroups.Category.ItemFanfares },
                        Instrument = 0x03,
                        Replaces = INTRO_CUTSCENE_2,
                    });

                    ScanForMMRS(directory); // Scan for .mmrs files in the music directory
                }
                catch (UnauthorizedAccessException)
                {
                    throw new Exception($"GetDirectories: Cannot access the following directory: {directory}");
                }
            }

            // Secondary check for old music files returned some, so write it out!
            // This is contained within its own file because it could be hundreds of lines long
            if (MusicConversionUtils.OLD_MUSIC_FILES.Any())
            {
                File.WriteAllLines(Path.Combine(Values.MusicDirectory, "unsupported_music_files.txt"), MusicConversionUtils.OLD_MUSIC_FILES);
            }
        }

        private static int RoundTo16(int value)
        {
            return (value + 0xF) & ~0xF;
        }

        public static int GetSequenceSize(SequenceInfo seq)
        {
            // The sequence should be loaded into memory if it was in an MMRS file
            if (seq.SequenceBinaryList != null && seq.SequenceBinaryList.Count > 0)
            {
                return RoundTo16(seq.SequenceBinaryList[0].SequenceBinary.Length);
            }
            else if (seq.Name.StartsWith("mm-")) // Look up vanilla sequences from AudioSeq index table
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
                byte[] data;
                if (File.Exists(seq.Filename))
                {
                    using (var reader = new BinaryReader(File.OpenRead(seq.Filename)))
                    {
                        data = new byte[(int)reader.BaseStream.Length];
                        return RoundTo16(data.Length);
                    }
                }
            }

            throw new Exception("GetSequenceSize: Sequence File is missing");
        }

        private static bool ReadMMRSInstrumentBank(SequenceInfo song, SequenceBinaryData combo, ZipArchiveEntry bankFile, ZipArchiveEntry bankmetaFile)
        {
            // Instrument bank files are a binary file with the ".zbank" extension, they're paired with a binary metadata file with the ".bankmeta" extension
            // Returns true/false if the music file uses a custom instrument bank

            if (bankFile != null && bankmetaFile != null)
            {
                // The Bankmeta file that music files use is 8 bytes long
                if (bankmetaFile.Length != 8)
                    throw new Exception($"Error: Bankmeta file is too short for file: '{song.Name}'. Expected 8 bytes, but got {bankmetaFile.Length} bytes instead.");

                byte[] bankmetaData = new byte[8];

                using var bankmetaReader = bankmetaFile.Open();
                bankmetaReader.Read(bankmetaData, 0, 8);

                int minLen = 0x08 + (bankmetaData[4] * 0x04) + (bankmetaData[5] * 0x04);

                // The bank should have at least as many bytes as there are drum and instrument pointers
                if (bankFile.Length < minLen)
                    throw new Exception($"Error: Zbank file is too short for file: '{song.Name}'. Expected at least {minLen} bytes, but got {bankFile.Length} bytes instead.");

                byte[] bankData = new byte[bankFile.Length];

                using var bankStream = bankFile.Open();
                bankStream.Read(bankData, 0, bankData.Length);

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

            return false; // The music file does not use a custom bank
        }

        private static void ReadMMRSFormmask(SequenceBinaryData combo, ZipArchiveEntry formmaskFile)
        {
            // Read the Formmask (".formmask") file, it's a single JSON/YAML list that determines which
            // sequence channels should be turned on and off for each of Link's states

            if (formmaskFile != null)
            {
                using var reader = new StreamReader(formmaskFile.Open(), Encoding.Default);
                string formMaskData = reader.ReadToEnd();
                try
                {
                    // playState is a boolean bitfield, in the file it's "play with these states", but in the code it's "mute these states"
                    // so it needs to be reversed
                    var playState = YamlSerializer.Deserialize<SequencePlayState[]>(formMaskData); // all JSON is valid YAML

                    // Backwards compatibility for version 1.15 and lower sequence files
                    if (!playState.Any(s => s.HasFlag(SequencePlayState.FierceDeity) && !s.HasFlag(SequencePlayState.Human)))
                    {
                        for (var i = 0; i < playState.Length; i++)
                        {
                            if (playState[i].HasFlag(SequencePlayState.Human))
                            {
                                playState[i] |= SequencePlayState.FierceDeity;
                            }
                        }
                    }

                    // Ensure unused cumulative states won't cause the channel to be muted when in those states
                    foreach (var cumulativeState in Enum.GetValues<SequencePlayState>().Where(s => s > SequencePlayState.All))
                    {
                        if (!playState.Any(s => s.HasFlag(cumulativeState)))
                        {
                            playState[0x10] |= cumulativeState;
                        }
                    }

                    combo.FormMask = ConvertUtils.U16ArrayToBytes(playState.Cast<ushort>().ToArray());
                }
                catch (Exception e)
                {
                    throw new Exception($"Error: Music file's Formmask file is invalid: {e.Message}", e);
                }
            }
        }

        private static void ReadMMRSSequence(SequenceInfo song, MMRSArchiveContents mmrs, string instrumentSet)
        {
            int claimedBankCount = 0;
            ZipArchiveEntry sequenceFile = mmrs.SequenceFile ?? throw new FileNotFoundException($"ReadMMRSSequence: Sequence file is missing.");

            // The sequence file shouldn't be empty
            if (sequenceFile.Length == 0)
                throw new Exception($"Error: Sequence file for '{song.Name}' contains no data.");

            byte[] rawSeqData = new byte[sequenceFile.Length];

            using var stream = sequenceFile.Open();
            stream.Read(rawSeqData, 0, rawSeqData.Length);

            SequenceBinaryData sequence = new() { SequenceBinary = rawSeqData };

            // If the value is "custom" or "-", then the music file uses a custom bank
            if (instrumentSet == "custom" || instrumentSet == "-")
            {
                song.Instrument = REQUIRES_NEW_BANK;
            }
            else
            {
                try
                {
                    song.Instrument = Convert.ToInt32(instrumentSet, 16);
                }
                catch (FormatException e)
                {
                    song.Instrument = REQUIRES_NEW_BANK;
                }
            }

            var customBankIncluded = ReadMMRSInstrumentBank(song, sequence, mmrs.BankFile, mmrs.BankmetaFile);

            // Before AudioBankTable expansion, MMR used to overwrite the original instrument bank
            // However, this causes more problems now that expansion is available, so if an old instrument bank exists, treat it as custom
            if (song.Instrument > 0x28 || customBankIncluded)
            {
                song.Instrument = REQUIRES_NEW_BANK;
                foreach (var seq in song.SequenceBinaryList)
                {
                    seq.InstrumentSet.BankSlot = song.Instrument;
                }
            }
            if (song.Instrument == REQUIRES_NEW_BANK && !customBankIncluded)
            {
#if DEBUG
                throw new Exception($"Error: File with no bank has a bad instrument set: {instrumentSet}");
#else
                continue;
#endif
            }

            if (customBankIncluded)
            {
                claimedBankCount++;
            }

            ReadMMRSFormmask(sequence, mmrs.FormmaskFile);

            song.SequenceBinaryList.Add(sequence);
        }

        private static MMRSMetadata ReadMMRSMetaYaml(string songname, ZipArchiveEntry metaFile)
        {
            // Reads and collects the music files metadata from the .meta YAML file

            if (metaFile == null)
                throw new Exception($"Error: No metadata file available for song: '{songname}'");

            // Valid values
            var validTypes = new HashSet<string> { "bgm", "fanfare" };
            var validGames = new HashSet<string> { "oot", "mm" }; // Game is mainly for OOTMM, but could be useful if OOTRS support is added

            var validSoundTypes = new HashSet<string> { "INST", "DRUM", "SFX" };
            var validKeyRegions = new HashSet<string> { "LOW", "PRIM", "HIGH" };

            MMRSMetadataYAML yamlData;

            using (var reader = new StreamReader(metaFile.Open(), Encoding.Default))
            {
                string yamlText = reader.ReadToEnd();
                yamlData = YamlSerializer.Deserialize<MMRSMetadataYAML>(yamlText);
            }

            if (yamlData == null || yamlData.Metadata == null)
                throw new Exception($"Error: Invalid or empty YAML metadata for song: '{songname}'");

            string songType = validTypes.Contains(yamlData.Metadata.SongType?.ToLower()) ? yamlData.Metadata.SongType.ToLower() : "bgm";

            //Handle the categories
            var categories = new List<int> // Default to all BGM
            {
                (int)MusicGroups.Category.Fields,
                (int)MusicGroups.Category.Towns,
                (int)MusicGroups.Category.Dungeons,
                (int)MusicGroups.Category.Indoors,
                (int)MusicGroups.Category.Minigames,
                (int)MusicGroups.Category.ActionThemes,
                (int)MusicGroups.Category.CalmThemes,
                (int)MusicGroups.Category.Fights,
            };
            if (yamlData.Metadata.MusicGroups != null && yamlData.Metadata.MusicGroups.Any())
            {
                categories.Clear();
                MusicGroups.Type? firstType = null;

                foreach (var category in yamlData.Metadata.MusicGroups)
                {
                    if (TryParseCategory(category, out int cat))
                    {
                        // BGM and Fanfare categories can't be mixed, so get the type
                        var currentType = MusicGroups.GetCategoryType(cat);

                        // Ensure at least the first type matches the given song type, otherwise throw an error
                        if (firstType == null && !string.Equals(songType, MusicGroups.TypeCheck[currentType], StringComparison.OrdinalIgnoreCase))
                            throw new Exception($"Error: Category '{category}' does not match given song type '{songType}' for song: {songname}");

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
                        throw new Exception($"Error: Bad category '{category}' in song: '{songname}'.");
#else
                        continue;
#endif
                    }
                }
            }

            // Handle META commands
            var commands = new List<Dictionary<string, object>>();
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
                        throw new InvalidOperationException($"Error: Audio sample '{entry.Key}': If type is null, index and key region must also be null.");
                    }
                    else
                    {
                        if (!validTypes.Contains(type) && type != null)
                            throw new InvalidOperationException($"Sample '{entry.Key}': Invalid instrument type '{type}'.");

                        if (validTypes.Contains(type) && listIndex == null)
                            throw new InvalidOperationException($"Sample '{entry.Key}': Index must not be null when type is '{type}'.");

                        if (type != null && tempAddr != null)
                            throw new InvalidOperationException($"Sample '{entry.Key}': temp addr must be null in the new format.");

                        if (type == "INST")
                        {
                            if (string.IsNullOrEmpty(keyRegion) || !validKeyRegions.Contains(keyRegion))
                                throw new InvalidOperationException($"Error: Audio sample '{entry.Key}': key region must be one of LOW, NORM, HIGH for INST.");
                        }
                        else // DRUM or SFX
                        {
                            if (!string.IsNullOrEmpty(keyRegion))
                                throw new InvalidOperationException($"Error: Audio sample '{entry.Key}': key region must be null or empty for {type}.");
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

            return new MMRSMetadata
            {
                CosmeticName = yamlData.Metadata.DisplayName,
                InstrumentSet = yamlData.Metadata.InstrumentSet,
                SongType = songType,
                Categories = categories,
                Commands = commands
            };
        }

        public static void ScanForMMRS(string directory)
        {
            // Check the directory for MMRS files the user added
            // MMRS is a zip file with a custom file extension ".mmrs" and can contain the following:
            //   - Sequence file (.seq; required)
            //   - Metadata file (.meta; required)
            //   - Instrument bank file (.zbank)
            //   - Instrument bank metadata file (.bankmeta)
            //   - Custom audio sample file (.zsound)
            //   - Formmask array file (.formmask)
            //
            // Only one file for each file type is allowed except custom audio sample files
            // an instrument bank may contain multiple sounds, so multiple may be required

            // Check for old standalone sequence files and add them to the old file list
            foreach (string filePath in Directory.GetFiles(directory, "*.zseq"))
            {
                MusicConversionUtils.OLD_MUSIC_FILES.Add(Path.GetFileName(filePath));
            }

            foreach (string filePath in Directory.GetFiles(directory, "*.mmrs"))
            {
                try
                {
                    using (ZipArchive zip = ZipFile.OpenRead(filePath))
                    {
                        var mmrs = new MMRSArchiveContents();

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
                            { ".seq",      CreateSetter(() => mmrs.SequenceFile,     e => mmrs.SequenceFile = e, "sequence") },
                            { ".meta",     CreateSetter(() => mmrs.MetaFile,         e => mmrs.MetaFile = e,     "meta") },
                            { ".zbank",    CreateSetter(() => mmrs.BankFile,         e => mmrs.BankFile = e,     "zbank") },
                            { ".bankmeta", CreateSetter(() => mmrs.BankmetaFile,     e => mmrs.BankmetaFile = e, "bankmeta") },
                            { ".formmask", CreateSetter(() => mmrs.FormmaskFile,     e => mmrs.FormmaskFile = e, "formmask") },
                            { ".zsound",   entry => mmrs.AudioSamples.Add(entry) },
                        };

                        foreach (var entry in zip.Entries)
                        {
                            if (entry.FullName.Contains('/'))
                                continue;

                            // If the file is using the old format, it will have a categories file
                            if (entry.Name.Equals("categories.txt"))
                            {
                                mmrs.CategoriesFile = entry;
                                continue;
                            }

                            string ext = Path.GetExtension(entry.Name).ToLowerInvariant();
                            if (handlers.TryGetValue(ext, out var handler))
                            {
                                handler(entry);
                            }
                        }

                        // Verify all required files are present
                        if (mmrs.SequenceFile == null || mmrs.MetaFile == null)
                        {
                            // If the file is an old file, it will have categories and no meta file
                            if (mmrs.CategoriesFile != null)
                            {
                                MusicConversionUtils.OLD_MUSIC_FILES.Add(Path.GetFileName(filePath));
                            }

                            continue;
                        }

                        bool hasBankFile = mmrs.BankFile != null;
                        bool hasBankmetaFile = mmrs.BankmetaFile != null;

                        if (hasBankFile != hasBankmetaFile)
                            continue;

                        SequenceInfo currentSong = new()
                        {
                            Name = Path.GetFileNameWithoutExtension(filePath)
                        };

                        var metadata = ReadMMRSMetaYaml(currentSong.Name, mmrs.MetaFile);

                        currentSong.DisplayName = metadata.CosmeticName;
                        currentSong.Categories = metadata.Categories;

                        // Handle custom audio samples
                        var samplesList = new List<SequenceSoundSampleBinaryData>();
                        foreach (var command in metadata.Commands)
                        {
                            var zsoundName = command.TryGetValue("file", out var nameVal) ? nameVal as string : null;
                            var zsoundFile = mmrs.AudioSamples.FirstOrDefault(entry => entry.Name.Contains(zsoundName));

                            if (zsoundFile != null)
                            {
                                byte[] sampleData = new byte[zsoundFile.Length];
                                zsoundFile.Open().Read(sampleData, 0, sampleData.Length);

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
                                        InstrumentType = zsoundType,
                                        ListIndex = zsoundIndex ?? -1,
                                        KeyRegion = zsoundKeyRegion,
                                    }
                                );
                            }
                        }

                        currentSong.InstrumentSamples = samplesList;
                        currentSong.SequenceBinaryList = new List<SequenceBinaryData>();

                        ReadMMRSSequence(currentSong, mmrs, metadata.InstrumentSet);

                        if (currentSong != null && currentSong.SequenceBinaryList != null)
                        {
                            RomData.SequenceList.Add(currentSong);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error attempting to read archive: " + filePath + " -- " + e);
                }
            }
        }

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

        public static void ConvertSequenceSlotToPointer(int seqSlotIndex, int substituteSlotIndex)
        {
            // Converts the sequence slot to a pointer, then marks the slot so a new sequence isn't
            // placed into the pointer's slot. This will free a song slot, but it won't be completely
            // empty if a player encounters it.

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


        // Passed to RomData.SequenceList in Builder.cs::WriteAudioSeq
        public static void RebuildAudioSeq(List<SequenceInfo> sequenceList, int? sequenceMaskFileIndex, int? sequenceNamesFileIndex)
        {
            // Spoiler log output DEBUG
            StringBuilder log = new StringBuilder();
            void WriteOutput(string str)
            {
                Debug.WriteLine(str); // Keep DEBUG output
                log.AppendLine(str);
            }

            var oldSeq = new List<MMSequence>();
            int f = RomUtils.GetFileIndexForWriting(Addresses.SeqTable);
            int basea = RomData.MMFileList[f].Addr;

            for (int i = 0; i < 128; i++)
            {
                var entry = new MMSequence();

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

            var newSeq = new List<MMSequence>();
            int addr = 0;
            byte[] newAudioSeq = new byte[0];
            for (int i = 0; i < 128; i++)
            {
                var newentry = new MMSequence();
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
                    WriteOutput($"Error: Slot {i:X} has multiple songs pointing at it!");
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
                    else if (sequenceList[j].SequenceBinaryList != null && sequenceList[j].SequenceBinaryList.Count > 0)
                    {
                        if (sequenceList[j].SequenceBinaryList.Count > 1)
                        {
                            WriteOutput("Warning: writing song with multiple sequence/bank combos, selecting first available");
                        }
                        newentry.Data = sequenceList[j].SequenceBinaryList[0].SequenceBinary;
                        WriteOutput($"Slot {i:X2} := {sequenceList[j].Name} *");

                    }
                    else // Not an MM sequence, load and add file
                    {
                        byte[] data;
                        if (File.Exists(sequenceList[j].Filename))
                        {
                            using (var reader = new BinaryReader(File.OpenRead(sequenceList[j].Filename)))
                            {
                                data = new byte[(int)reader.BaseStream.Length];
                                reader.Read(data, 0, data.Length);
                            }
                        }
                        else if (sequenceList[j].Name == nameof(Properties.Resources.mmr_f_sot))
                        {
                            data = Properties.Resources.mmr_f_sot;
                        }
                        else
                        {
                            throw new Exception($"Music not found as file or built-in resource: '{sequenceList[j].Filename}'");
                        }

                        // This might check if the sequence type is correct for MM
                        // DB ripped sequences from SF64/SM64/MK64 without modifying them
                        if (data[1] != 0x20)
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
                    newentry.Data = newentry.Data.Concat(new byte[padding]).ToArray();
                }

                newSeq.Add(newentry);
                // TODO is there not a better way to write this?
                if (newentry.Data != null)
                {
                    newAudioSeq = newAudioSeq.Concat(newentry.Data).ToArray();
                }

                addr += newentry.Size;
            }

            // discovered when MM-only music was fixed, if the audioseq is left in it's old spot
            // audio quality is garbage, sounds like static
            //if (addr > (RomData.MMFileList[4].End - RomData.MMFileList[4].Addr))
            //else
            //RomData.MMFileList[4].Data = NewAudioSeq;

            int index = RomUtils.AppendFile(newAudioSeq);
            ResourceUtils.ApplyHack(Resources.mods.reloc_audio);
            RelocateSeq(index);
            RomData.MMFileList[4].Data = new byte[0];
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
                        formMask = sequenceList[j].SequenceBinaryList?.FirstOrDefault()?.FormMask;
                    }

                    if (sequenceNamesFileIndex.HasValue)
                    {
                        name = sequenceList[j].DisplayName;
                    }
                }

                if (sequenceMaskFileIndex.HasValue)
                {
                    formMask ??= Enumerable.Repeat<byte>(0xFF, 0x20).ToArray();

                    Array.Resize(ref formMask, MusicConfig.SEQUENCE_DATA_SIZE);
                    ReadWriteUtils.Arr_Insert(formMask, 0, MusicConfig.SEQUENCE_DATA_SIZE, RomData.MMFileList[sequenceMaskFileIndex.Value].Data, i * MusicConfig.SEQUENCE_DATA_SIZE);
                }

                if (sequenceNamesFileIndex.HasValue)
                {
                    name ??= "";
                    if (name.Length > MusicConfig.SEQUENCE_NAME_MAX_SIZE - 1)
                    {
                        name = name.Substring(0, MusicConfig.SEQUENCE_NAME_MAX_SIZE - 4) + "...";
                    }
                    name += "\0";
                    var nameBytes = Encoding.ASCII.GetBytes(name);
                    Array.Resize(ref nameBytes, MusicConfig.SEQUENCE_NAME_MAX_SIZE);
                    ReadWriteUtils.Arr_Insert(nameBytes, 0, MusicConfig.SEQUENCE_NAME_MAX_SIZE, RomData.MMFileList[sequenceNamesFileIndex.Value].Data, i * MusicConfig.SEQUENCE_NAME_MAX_SIZE);
                }
            }

            //// DEBUG spoiler log output
            //String dir = Path.GetDirectoryName(_settings.OutputROMFilename);
            //String path = $"{Path.GetFileNameWithoutExtension(_settings.OutputROMFilename)}";
            //// spoiler log should already be written by the time we reach this far
            //if (File.Exists(Path.Combine(dir, path + "_SpoilerLog.txt")))
            //    path += "_SpoilerLog.txt";
            //else // TODO add HTML log compatibility
            //    path += "_SongLog.txt";

            //using (StreamWriter sw = new StreamWriter(Path.Combine(dir, path), append: true))
            //{
            //    sw.WriteLine(""); // spacer
            //    sw.Write(log);
            //}
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
            ReadWriteUtils.WriteToROM(0x00C2739C, new byte[] { 0x3C, 0x08, 0x80, 0x0A, 0x8D, 0x05, (byte)(offset >> 8), (byte)(offset & 0xFF) });
        }

        public static void MoveAudioBankTable()
        {
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
            //                      nops a metadata copy function, and sets a fixed size for the audiobank pointer index
            ResourceUtils.ApplyHack(Resources.mods.instrumentset_patch);

            // moveaudiostatebytes: Sets where read and writes for sequence and instrumentset states go.
            //                      In this hack, they're moved from 0x80205008 to end of old instrumentset table in code and
            //                      given more space. if these don't get moved, new banks at 0x30 and up will overflow into
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

        public static void ResetFreeBankIndex()
        {
            CurrentFreeBank = 0x29;
        }

        public static bool TestIfAvailableBanks(SequenceInfo testSeq, Random rng)
        {
            // Test is the testSeq can be used with available instrument set slots

            // Check if the instrument set already exists for this sequence
            if (testSeq.SequenceBinaryList != null && testSeq.SequenceBinaryList.Count > 0 && testSeq.SequenceBinaryList.Any(u => u.InstrumentSet != null))
            {
                // Randomize the instrument sets last second, this way early banks don't get ravaged based on order
                if (testSeq.SequenceBinaryList.Count > 1)
                {
                    testSeq.SequenceBinaryList = testSeq.SequenceBinaryList.OrderBy(x => rng.Next()).ToList();
                }

                var testBanks = testSeq.CheckAvailableBanks();
                if (testBanks == true)
                {
                    testSeq.ClearUnavailableBanks(); // Remove any already claimed bank sequences
                }
                else // All custom banks have been claimed
                {
                    if (CurrentFreeBank > 0x0080)
                    {
                        return false; // Can't overwrite any more entries
                    }

                    testSeq.SequenceBinaryList[0].InstrumentSet.BankSlot = CurrentFreeBank;
                }
            }
            return true; // Sequences with instrument banks, or without needing instrument banks, available
        }

        public static void TryBackupSongPlacement(SequenceInfo targetSlot, StringBuilder log, List<SequenceInfo> unassignedSequences, OutputSettings settings)
        {
            // Loosen the restrictions on song placement if there are no compatible songs left

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
                        SequenceBinaryList = replacementSong.SequenceBinaryList,
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
            throw new Exception($"Cannot randomize music on this seed with available music: \nSlot Name:[{targetSlot.Name}] PreviousSlot: [{targetSlot.Replaces:X}]");
        }

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

            using (var writer = new StreamWriter(Path.Combine(dir, path), append: true))
            {
                writer.WriteLine(""); // spacer between spoiler log and song log
                writer.Write(log);
            }
        }

        private static (int sequenceBankIndex, int bankListIndex) FindMatchingInstrumentSetDuplicate(SequenceInfo replacementSequence)
        {
            for (int b = 0; b < replacementSequence.SequenceBinaryList.Count; b++)
            {
                var bank = replacementSequence.SequenceBinaryList[b].SequenceBinary;
                if (bank != null)
                {
                    var searchResult = RomData.InstrumentSetList.FindIndex(match => match.BankBinary == bank);
                    if (searchResult != -1)
                    {
                        return (b, searchResult);
                    }
                }
            }

            return (-1, -1);
        }

        public static void AssignSequenceSlot(SequenceInfo slotSequence, SequenceInfo replacementSequence, List<SequenceInfo> remainingSequences, string debugString, StringBuilder log)
        {
            // If the song has a custom instrument set: lock the sequence, update the instrument set value, and write debug output
            if (replacementSequence.SequenceBinaryList != null && replacementSequence.SequenceBinaryList[0] != null && replacementSequence.SequenceBinaryList[0].InstrumentSet != null)
            {
                (int sequenceBankIndex, int bankListIndex) duplicateBankSearch = FindMatchingInstrumentSetDuplicate(replacementSequence);
                if (duplicateBankSearch.sequenceBankIndex != -1)
                {
                    RomData.InstrumentSetList[duplicateBankSearch.bankListIndex].Modified += 1;
                    replacementSequence.Instrument = duplicateBankSearch.bankListIndex;
                    log.AppendLine(" -- v -- Instrument set number " + replacementSequence.Instrument.ToString("X2") + " is being reused -- v --");
                    replacementSequence.SequenceBinaryList = new List<SequenceBinaryData> {
                        replacementSequence.SequenceBinaryList[duplicateBankSearch.sequenceBankIndex]
                    };
                }
                else // No duplicate instrument bank found, add a new one
                {
                    replacementSequence.Instrument = CurrentFreeBank++; // Update the instrument bank that will be used
                    replacementSequence.SequenceBinaryList[0].InstrumentSet.BankSlot = replacementSequence.Instrument;
                    RomData.InstrumentSetList[replacementSequence.Instrument] = replacementSequence.SequenceBinaryList[0].InstrumentSet;
                    RomData.InstrumentSetList[replacementSequence.Instrument].InstrumentSamples = replacementSequence.InstrumentSamples;
                    log.AppendLine(" -- v -- Instrument set number " + replacementSequence.Instrument.ToString("X2") + " has been claimed -- v --");
                    replacementSequence.SequenceBinaryList = new List<SequenceBinaryData> { replacementSequence.SequenceBinaryList[0] }; // Reduce to one for later
                }
            }

            replacementSequence.Replaces = slotSequence.Replaces; // Determines what song will be placed in slot_seq later
            // -40 and +10 pad the text to align in the same middle area for visual clarity
            log.AppendLine($"{slotSequence.Name,-40} {debugString,+10} -> " + replacementSequence.Name);
            remainingSequences.Remove(replacementSequence);
        }

        public static void CheckSongTest(List<SequenceInfo> sequences, StringBuilder log)
        {
            // For creators: Songtest is a debug token in the song filename. It specifies
            // to the rando that the music pool should be flooded with the song for testing.

            SequenceInfo songtestSequence = RomData.SequenceList.Find(u => u.Name.Contains("songtest") == true);
            if (songtestSequence == null)
            {
                return;
            }

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
            else if (!(songtestSequence.Categories.Contains((int)MusicGroups.Category.ItemFanfares) || songtestSequence.Categories.Contains((int)MusicGroups.Category.EventFanfares)
                        || songtestSequence.Categories.Contains((int)MusicGroups.Category.ClearFanfares) || songtestSequence.Categories.Contains((int)MusicGroups.Category.Cutscenes)))
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
                ConvertSequenceSlotToPointer(songslot.Replaces, FILE_SELECT); // Point replacement to "File Select"
            }
            RomData.TargetSequences.Remove(fileselectSlot);

            // Additionally, because songs that use custom banks replace the original bank by design,
            // the replacement bank should be a super set of the original and old songs should still work.
            // However, sometimes the old instruments in the new bank are broken and need to be tested.
            // To do so, the lottery will become a new song slot with a vanilla song using the songtest
            // sequence's instrument bank.

            if (songtestSequence.SequenceBinaryList == null)
            {
                return; // The song doesn't have a custom instrument bank, no need to continue
            }

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
            sharedBankSequences.RemoveAll(u => u.SequenceBinaryList != null);

            var newRandom = new Random();
            sharedBankSequences = sharedBankSequences.OrderBy(x => newRandom.Next()).ToList(); // Random shuffle

            ConvertRoomForSongTest(sceneFID: 1334, 1335, actorIDOffset: 0x98, 0x7, sharedBankSequences); // Lottery
            ConvertRoomForSongTest(sceneFID: 1158, 1159, actorIDOffset: 0x88, 0x7, sharedBankSequences); // Honey & Darling
            ConvertRoomForSongTest(sceneFID: 1188, 1189, actorIDOffset: 0x88, 0x7, sharedBankSequences); // Treasure Chest Game Shop
            ConvertRoomForSongTest(sceneFID: 1502, 1503, actorIDOffset: 0xC4, 0x7, sharedBankSequences); // Bomb Shop
        }

        public static void CheckSongForce(List<SequenceInfo> sequences, StringBuilder log, Random rng)
        {
            // Songforce is priority token in the song filename. It places it at the top of the previously randomized sequence list
            
            List<SequenceInfo> forcedSequences = RomData.SequenceList.FindAll(u => u.Name.Contains("songforce") == true).OrderBy(x => rng.Next()).ToList();
            if (forcedSequences != null && forcedSequences.Count > 0)
            {
                foreach (SequenceInfo seq in forcedSequences)
                {
                    log.AppendLine("Forcing song (" + seq.Name + ") to top of the song pool");
                    sequences.Remove(seq);
                    sequences.Insert(0, seq);
                }
            }
        }

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
                var songNotStarved = TestIfAvailableBanks(testSeq, rng);
                if (songNotStarved == false)
                {
                    continue; // The song is unacceptable
                }

                var maxSize = targetSlot.Replaces == SMALL_ENEMY_BATTLE ? MAX_COMBAT_BUDGET : MAX_BGM_BUDGET;
                var seqSize = GetSequenceSize(testSeq);
                if (seqSize > maxSize)
                {
                    continue; // The song is too big
                }

                // Check if the target and the possible match share a category
                if (testSeq.Categories.Intersect(targetSlot.Categories).Any())
                {
                    AssignSequenceSlot(targetSlot, testSeq, unassignedSequences, "", log);
                    return true;
                }

                // Deathbasket wanted there to be a small chance of getting out of category music, but
                // did not want to mix BGM and fanfares — or vice versa.
                //
                // (testSeq.Categories.Count > targetSlot.Categories.Count) DBs code, maybe thought to be safer?
                else if (unassignedSequences.Count > 30
                    && testSeq.Categories.Count > targetSlot.Categories.Count
                    && cosmeticSettings.MusicLuckRollChance > 0 && (decimal)(rng.NextDouble() * 100.0) < cosmeticSettings.MusicLuckRollChance
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

        public static void CheckBGMCombatMusicBudget(CosmeticSettings cosmeticSettings, List<SequenceInfo> unassignedSequences, Random rng, StringBuilder log)
        {
            // For any given scene, BGM and Small Enemy Battle music share the same buffer, loading to the other side. If their sum
            // is greater than the size of the buffer, they clip into one another when one loads — this kills one of them, usually BGM.

            var combatSequences = RomData.SequenceList.FindAll(u => u.Categories.Contains((int)MusicGroups.Category.ActionThemes));
            var BGMSlots = RomData.TargetSequences.FindAll(u => u.Categories.Contains((int)MusicGroups.Category.Fields)
                                                             || u.Categories.Contains((int)MusicGroups.Category.Dungeons)
                                                             || u.SeqId == DEKU_PALACE); // 0x12: Deku Palace (has enemies)

            var usedBGMSequences = new List<SequenceInfo>();
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

            var usedCombatSequence = RomData.SequenceList.Find(u => u.Replaces == SMALL_ENEMY_BATTLE && u.Name != "mm-combat");
            if (usedCombatSequence == null) // Songtest removes the sequence and points it at "File Select" for testing
            {
                combatVsBGMCoinToss = true; // "COMBAT" manually selected because of combat songtest
                usedCombatSequence = RomData.SequenceList.Find(u => u.Replaces == FILE_SELECT && u.Name != "mm-fileselect");
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
                {
                    return;
                }
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
                        bool status = SearchForValidSongReplacement(cosmeticSettings, unassignedSequences, bgmSlot, rng, log);
                        if (status == false)
                        {
                            throw new Exception("Music Budget Error: this seed cannot find acceptable music for this combat slot\n" +
                                "Try another!");
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
                    var combatSlot = RomData.TargetSequences.Find(u => u.Name == "mm-combat");
                    usedCombatSequence.Replaces = -1; // Cancel using this song
                    bool status = SearchForValidSongReplacement(cosmeticSettings, unassignedSequences, combatSlot, rng, log);
                    if (status == false)
                    {
                        throw new Exception("Music Budget Error: this seed cannot find acceptable music for this combat slot\n" +
                            "Try another!");
                    }
                }

            }
        }

        public static void ReadInstrumentSetList()
        {
            // Go through the audiobank index and get details about every bank, then use those details
            // to generate a list from the vanilla game that can be modified as needed.
            
            RomData.InstrumentSetList = new List<InstrumentSetInfo>();

            // The index list can go up to 0x80 with current extended instrument bank table file
            for (int audiobankIndex = 0; audiobankIndex <= 0x80; ++audiobankIndex)
            {
                // Each instrument bank has one 16 byte sentence of data, first word is address, second is length, last 2 words metadata
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
            {
                return;
            }

            int soundbankAddr = RomData.MMFileList[5].Cmp_Addr; // In vanilla it's 0x97F70, but it can be shifted because MMR changes AudioSeq's location
            int audiobankInstSetAddr = RomData.MMFileList[3].Cmp_Addr; // Point to a specific instrument set, starting with 0 and updating per loop
            foreach (var instrumentset in RomData.InstrumentSetList)
            {
                if (instrumentset.InstrumentSamples != null && instrumentset.InstrumentSamples.Any())
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
                        AudiobankUtils.Audiobank instrumentBank = new(instrumentset.BankMetaData, instrumentset.BankBinary);

                        if (sample.Marker == 0)
                        {
                            // Get the offset to the sample using the given type, index, and region
                            uint sampleBankAddress = 0;
                            sampleBankAddress = sample.InstrumentType switch
                            {
                                "INST" => sample.KeyRegion switch
                                {
                                    "LOW"  => instrumentBank.Instruments[sample.ListIndex].LowSampleAddress,
                                    "PRIM" => instrumentBank.Instruments[sample.ListIndex].PrimSampleAddress,
                                    "HIGH" => instrumentBank.Instruments[sample.ListIndex].HighSampleAddress,
                                    _      => throw new Exception(),// invalid key region
                                },

                                // Drums and SFX don't have key regions
                                "DRUM" => instrumentBank.Drums[sample.ListIndex].SampleAddress,
                                "SFX"  => instrumentBank.Effects[sample.ListIndex].SampleAddress,
                                _      => throw new Exception(),// invalid type
                            };

                            // Replace the sample struct's address with the correct address
                            // The first 4 bytes are a bitfield, so add 4 to the index
                            ROM[audiobankInstSetAddr + sampleBankAddress + 4] = newAddressBytes[0];
                            ROM[audiobankInstSetAddr + sampleBankAddress + 5] = newAddressBytes[1];
                            ROM[audiobankInstSetAddr + sampleBankAddress + 6] = newAddressBytes[2];
                            ROM[audiobankInstSetAddr + sampleBankAddress + 7] = newAddressBytes[3];
                        }
                        else // Fallback to sample marker matching
                        {
                            uint sampleBankAddress = 0;

                            // Instead of searching byte by byte, collect all the samples, match the address, then get the offset to the matched address
                            // With this, there should be no accidental overwrites of data in the instrument bank 
                            foreach (var s in instrumentBank.GetBankSamples())
                            {
                                switch (s)
                                {
                                    case AudiobankUtils.Sample<AudiobankUtils.Instrument> instrumentSample:
                                        var instParent = instrumentSample.Parent;

                                        if (instParent.LowSample != null && instParent.LowSample.Address == sample.Marker)
                                            sampleBankAddress = instrumentBank.Instruments[instParent.InstrumentId].LowSampleAddress;

                                        else if (instParent.PrimSample != null && instParent.PrimSample.Address == sample.Marker)
                                            sampleBankAddress = instrumentBank.Instruments[instParent.InstrumentId].PrimSampleAddress;

                                        else if (instParent.HighSample != null && instParent.HighSample.Address == sample.Marker)
                                            sampleBankAddress = instrumentBank.Instruments[instParent.InstrumentId].HighSampleAddress;

                                        break;

                                    case AudiobankUtils.Sample<AudiobankUtils.Drum> drumSample:
                                        var drumParent = drumSample.Parent;

                                        if (drumParent.Sample != null && drumParent.Sample.Address == sample.Marker)
                                            sampleBankAddress = instrumentBank.Drums[drumParent.DrumId].SampleAddress;

                                        break;

                                    case AudiobankUtils.Sample<AudiobankUtils.Effect> effectSample:
                                        var effectParent = effectSample.Parent;

                                        if (effectParent.Sample != null && effectParent.Sample.Address == sample.Marker)
                                            sampleBankAddress = instrumentBank.Effects[effectParent.EffectId].SampleAddress;

                                        break;

                                    default:
                                        break;
                                }

                                if (sampleBankAddress != 0)
                                    break;
                            }

                            // Replace the sample struct's address with the correct address
                            // The first 4 bytes are a bitfield, so add 4 to the index
                            ROM[audiobankInstSetAddr + sampleBankAddress + 4] = newAddressBytes[0];
                            ROM[audiobankInstSetAddr + sampleBankAddress + 5] = newAddressBytes[1];
                            ROM[audiobankInstSetAddr + sampleBankAddress + 6] = newAddressBytes[2];
                            ROM[audiobankInstSetAddr + sampleBankAddress + 7] = newAddressBytes[3];
                        }
                    }
                }

                audiobankInstSetAddr += instrumentset.BankBinary.Length;
            }
        }


        public static void WriteNewSoundSamples(List<InstrumentSetInfo> InstrumentSetList)
        {
            // Write all the custom audio samples in a single file at the end.
            // In the event there are no more MMFile DMA indices, these files don't need to be a hard file in the filesystem,
            // they can be placed anywhere after the soundbank starting address on ROM. The instrument sample lookup doesn't use
            // the file system, but adding to the file system is useful for shifting in BuildROM().

            // Issue: it's unknown where the samples will be written to at this point, because BuildROM() will shift the files.
            // For now, write the audio samples after the audiobank/soundbank/samples are written, then update the audiobank
            // pointers.
            // Save extra soundsamples FID, and the samples with their data, for later (UpdateBankInstrumentPointers())

            int fid = RomData.SamplesFileID = RomUtils.AppendFile(new byte[0x0]);
            RomData.ListOfSamples = new List<SequenceSoundSampleBinaryData>();

            // For each custom instrument set that needs a custom audio sample
            foreach (InstrumentSetInfo instrumentSet in InstrumentSetList)
            {
                if (instrumentSet.InstrumentSamples != null && instrumentSet.InstrumentSamples.Any())
                {
                    foreach (SequenceSoundSampleBinaryData sample in instrumentSet.InstrumentSamples)
                    {
                        // Test if sample was already added by another song
                        var previouslyWrittenSample = RomData.ListOfSamples.Find(u => sample.Hash == u.Hash);
                        if (previouslyWrittenSample == null) // If sample not already written
                        {
                            sample.Addr = (uint)RomData.MMFileList[fid].Data.Length; // Get the ROM addr of the new file, the file will start at the end of the last file
                            RomData.MMFileList[fid].Data = RomData.MMFileList[fid].Data.Concat(sample.BinaryData).ToArray(); // Concat the sample to sample collection file

                            int paddingRemainder = RomData.MMFileList[fid].Data.Length % 0x10; // Samples may not need to be 16 byte aligned, but just in case
                            if (paddingRemainder > 0)
                            {
                                RomData.MMFileList[fid].Data = RomData.MMFileList[fid].Data.Concat(new byte[paddingRemainder]).ToArray();
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

        public static void RebuildAudioBank(List<InstrumentSetInfo> InstrumentSetList)
        {
            // Get the index for the old instrument bank, it will be put in the same spot while
            // letting it expand into the AudioSeq's spot that has been moved to the end
            int fid = RomUtils.GetFileIndexForWriting(NewInstrumentSetAddress);

            // The DMA table doesn't point directly to the indextable on the ROM, its part of a larger yaz0 file, an offset is required to get the address in the file
            int audiobankIndexOffset = NewInstrumentSetAddress - RomData.MMFileList[RomUtils.GetFileIndexForWriting(NewInstrumentSetAddress)].Addr;

            int audiobankBankOffset = 0;
            byte[] audiobankData = new byte[0];

            // For each instrument bank, concat onto the new object, then update the table to match the new instrument sets.
            // CurrentFreeBank is used so not all unused dummy instrument banks get written to ROM.
            for (int audiobankIndex = 0; audiobankIndex <= CurrentFreeBank; ++audiobankIndex)
            {
                var currentBank = InstrumentSetList[audiobankIndex];
                audiobankData = audiobankData.Concat(currentBank.BankBinary).ToArray();

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
                    audiobankData = audiobankData.Concat(new byte[paddingRemainder + 0x10]).ToArray(); // Padding with an extra line is cheap enough to try
                    audiobankBankOffset += paddingRemainder;
                }
            }

            // Write the new instrument bank back to file
            var audiobankFile = RomData.MMFileList[RomUtils.GetFileIndexForWriting(Addresses.Audiobank)];
            audiobankFile.Data = audiobankData;
            audiobankFile.End = audiobankFile.Addr + audiobankFile.Data.Length;
        }

        private class MMRSArchiveContents
        {
            // Intermediary class to store files contained in the music file archive
            public ZipArchiveEntry MetaFile { get; set; }
            public ZipArchiveEntry SequenceFile { get; set; }
            public ZipArchiveEntry BankFile { get; set; }
            public ZipArchiveEntry BankmetaFile { get; set; }
            public ZipArchiveEntry FormmaskFile { get; set; }
            public ZipArchiveEntry CategoriesFile { get; set; }
            public List<ZipArchiveEntry> AudioSamples { get; set; } = new();
        }

        private class MMRSMetadata
        {
            // Intermediary class to store metadata information from the META file
            public string CosmeticName { get; set; }
            public string InstrumentSet { get; set; }
            public string SongType { get; set; }
            public List<int> Categories { get; set; } = new();
            public List<Dictionary<string, object>> Commands { get; set; } = new();
        }

        public class SEQSYaml
        {
            // Store SEQS data as YAML instead of plaintext

            [YamlMember(Alias = "display name")]
            public string DisplayName { get; set; }

            [YamlMember(Alias = "music groups")]
            public List<object> MusicGroups { get; set; }

            [YamlMember(Alias = "instrument set")]
            public int InstrumentSet { get; set; }

            [YamlMember(Alias = "sequence id")]
            public int SequenceId { get; set; }

            [YamlMember(Alias = "no recycle")]
            public bool NoRecycle { get; set; } = false;
        }
    }
}
