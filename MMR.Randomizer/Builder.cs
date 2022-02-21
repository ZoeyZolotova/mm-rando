using MMR.Common.Extensions;
using MMR.Randomizer.Asm;
using MMR.Randomizer.Attributes;
using MMR.Randomizer.Constants;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Colors;
using MMR.Randomizer.Models.Rom;
using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Models.SoundEffects;
using MMR.Randomizer.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Point = SixLabors.Primitives.Point;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Color = System.Drawing.Color;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.Formats.Png;
using System.Security.Cryptography;

namespace MMR.Randomizer
{
    public class Builder
    {
        private RandomizedResult _randomized;
        private CosmeticSettings _cosmeticSettings;
        private MessageTable _messageTable;
        private ExtendedObjects _extendedObjects;
        private List<MessageEntry> _extraMessages;
        private Dictionary<int, ItemGraphic> _graphicOverrides;

        public Builder(RandomizedResult randomized, CosmeticSettings cosmeticSettings)
        {
            _randomized = randomized;
            _cosmeticSettings = cosmeticSettings;
            _messageTable = null;
            _extendedObjects = null;
            _extraMessages = new List<MessageEntry>();
            _graphicOverrides = new Dictionary<int, ItemGraphic>();
        }

        #region Sequences, sounds and BGM

        // this function decides which songs get shuffled, choosing song -> slot
        //  the audioseq file gets rearanged/built in SequenceUtils::RebuildAudioSeq
        private void BGMShuffle(Random random, OutputSettings settings)
        {
            // spoiler log output
            StringBuilder log = new StringBuilder();
            void WriteOutput(string str)
            {
                Debug.WriteLine(str);
                log.AppendLine(str);
            }
            

            // we randomize both slots and songs because if we're low on variety, and we don't sort slots
            //   then all the variety can be dried up for the later slots
            // the biggest example is MM-only, many songs are action/boss but the boss slots are later
            //  as a result boss music is often used up early placed into early action slots
            // if we don't randomize remaining, then we only get upper alphabetical, same every seed
            List<SequenceInfo> unassigned = RomData.SequenceList.FindAll(u => u.Replaces == -1);
            unassigned = unassigned.OrderBy(x => random.Next()).ToList();                           // random ordered songs
            RomData.TargetSequences = RomData.TargetSequences.OrderBy(x => random.Next()).ToList(); // random ordered slots
            WriteOutput(" Randomizing " + RomData.TargetSequences.Count + " song slots, with " + unassigned.Count + " available songs:");

            SequenceUtils.ResetBudget();

            // songtest filename token allows music makers and users to force a song into a MMR seed for recording/testing
            SequenceUtils.CheckSongTest(unassigned, log);

            SequenceUtils.CheckSongForce(unassigned, log, random);

            foreach (var targetSlot in RomData.TargetSequences)
            {
                // scan all songs for a replacement that fits in this slot
                bool foundValidReplacement = SequenceUtils.SearchForValidSongReplacement(unassigned, targetSlot, random, log);

                if (foundValidReplacement == false) // no available songs fit in this slot category
                {
                    WriteOutput($"No song fits in [{targetSlot.Name}] slot, with categories: " + String.Join(", ", targetSlot.Type.Select(x => "0x" + x.ToString("X2"))));
                    // loosen song restrictions and re-attempt
                    SequenceUtils.TryBackupSongPlacement(targetSlot, log, unassigned, settings);
                }
            }

            SequenceUtils.CheckBGMCombatMusicBudget(unassigned, _cosmeticSettings.DisableCombatMusic, random, log);

            RomData.SequenceList.RemoveAll(u => u.Replaces == -1); // this still gets used in SequenceUtils.cs::RebuildAudioSeq

            if (_cosmeticSettings.Music == Music.Random && settings.GenerateSpoilerLog)
            {
                SequenceUtils.WriteSongLog(log, settings);
            }

            // write bigger music buffer
            ReadWriteUtils.WriteCodeUInt32(0x801DB9B4, 0x6000);
            ReadWriteUtils.WriteCodeUInt32(0x801DB9B8, 0x6000);
        }

        private void WriteAudioSeq(Random random, OutputSettings _settings)
        {
            if (_cosmeticSettings.Music == Music.None)
            {
                return;
            }

            RomData.PointerizedSequences = new List<SequenceInfo>();
            SequenceUtils.ReadSequenceInfo();
            SequenceUtils.ReadInstrumentSetList();
            if (_cosmeticSettings.Music == Music.Random)
            {
                SequenceUtils.PointerizeSequenceSlots();
                BGMShuffle(random, _settings);
                ResourceUtils.ApplyHack(Resources.mods.remove_morning_music);
            }

            ResourceUtils.ApplyHack(Resources.mods.fix_music);
            ResourceUtils.ApplyHack(Resources.mods.inst24_swap_guitar);
            SequenceUtils.RebuildAudioSeq(RomData.SequenceList, _cosmeticSettings.AsmOptions.MusicConfig.SequenceMaskFileIndex);
            SequenceUtils.WriteNewSoundSamples(RomData.InstrumentSetList);
            SequenceUtils.RebuildAudioBank(RomData.InstrumentSetList);
        }

        private void WriteMuteMusic()
        {
            if (_cosmeticSettings.Music == Music.None)
            {
                /// mute all music by setting their master volume to zero
                // Traverse the audioseq index table to get the locations of all sequences
                // the audioseq index table is not its own file, its buried within the code file, we need the offset to the table
                var codeFile = RomData.MMFileList[RomUtils.GetFileIndexForWriting(Addresses.SeqTable)];
                var audioseqIndexTable = codeFile.Data;
                int audioseqIndexTableAddr = Addresses.SeqTable - codeFile.Addr;
                var audioseq = RomData.MMFileList[RomUtils.GetFileIndexForWriting(Addresses.AudioSequence)].Data;
                // for each sequence, search for the master volume byte and change to zero
                for (int seq = 2; seq < 128; seq += 1)
                {
                    if (seq == 0x54) // It was requested that the bar band minigame not be silenced
                    {
                        continue;
                    }

                    int seqLocation = (int)ReadWriteUtils.Arr_ReadU32(audioseqIndexTable, audioseqIndexTableAddr + (seq * 16));
                    for (int b = 3; b < 128; b++) // search for master volume byte
                    {
                        if (audioseq[seqLocation + b] == 0xDB) // master volume byte
                        { 
                            audioseq[seqLocation + b + 1] = 0x0; // set value to zero
                            continue;
                        }
                    }
                }
            }
        }

        private void WriteEnemyCombatMusicMute()
        {
            if (_cosmeticSettings.DisableCombatMusic == CombatMusic.Normal)
            {
                return;
            }

            ReadWriteUtils.WriteToROM(0xCA7F00 + 0x16818, 0x1000);
        }
        #endregion

        private void WritePlayerModel()
        {
            if (_randomized.Settings.Character == Character.LinkMM)
            {
                return;
            }

            if (_randomized.Settings.Character == Character.AdultLink)
            {
                PlayerModelUtils.ApplyAdultLinkPatches();
                return;
            }

            int characterIndex = (int)_randomized.Settings.Character;

            ResourceUtils.ApplyIndexedHack(characterIndex-1, Resources.mods.fix_link_1, Resources.mods.fix_link_2, Resources.mods.fix_link_3);
            ObjUtils.InsertIndexedObj(characterIndex - 1, 0x11, Resources.models.link_1, Resources.models.link_2, Resources.models.link_3);

            if (_randomized.Settings.Character == Character.Kafei)
            {
                ObjUtils.InsertObj(Resources.models.kafei, 0x1C);
                ResourceUtils.ApplyHack(Resources.mods.fix_kafei);

                ObjUtils.InsertObj(Resources.models.link_mask, 0x1FF);
                ResourceUtils.ApplyHack(Resources.mods.update_kafei_mask_icon);

                ObjUtils.InsertObj(Resources.models.gi_link_mask, 0x258);
            }
        }

        private void WriteTunicColor()
        {
            if (_cosmeticSettings.UseTunicColors[TransformationForm.Human])
            {
                Color t = _cosmeticSettings.TunicColors[TransformationForm.Human];
                byte[] color = { t.R, t.G, t.B };

                var playerModel = DeterminePlayerModel();
                var characterIndex = (int)playerModel;
                if (playerModel == Character.Kafei)
                {
                    var objectData = ObjUtils.GetObjectData(0x11);
                    TunicUtils.UpdateKafeiTunic(ref objectData, t);
                    ObjUtils.InsertObj(objectData, 0x11);
                }
                else
                {
                    var locations = ResourceUtils.GetIndexedAddresses(characterIndex, Resources.addresses.tunic_0, Resources.addresses.tunic_1, Resources.addresses.tunic_2, Resources.addresses.tunic_3);
                    var objectData = ObjUtils.GetObjectData(0x11);
                    for (int j = 0; j < locations.Count; j++)
                    {
                        ReadWriteUtils.WriteFileAddr(locations[j], color, objectData);
                    }
                    ObjUtils.InsertObj(objectData, 0x11);
                };
            }

            var tunicForms = new List<TransformationForm>
            {
                TransformationForm.Deku,
                TransformationForm.Goron,
                TransformationForm.Zora,
                TransformationForm.Zora,
                TransformationForm.FierceDeity,
            };

            var otherTunics = ResourceUtils.GetAddresses(Resources.addresses.tunic_forms);

            for (var i = 0; i < tunicForms.Count; i++)
            {
                if (_cosmeticSettings.UseTunicColors[tunicForms[i]])
                {
                    var t = _cosmeticSettings.TunicColors[tunicForms[i]];
                    TunicUtils.UpdateFormTunics(i, otherTunics, t);
                }
            }
        }

        private void WriteInstruments(Random random)
        {
            var codeFileAddress = 0xB3C000;

            var milkBarActions = new List<Action>();
            var audioSeqFileAddress = 0x46AF0;
            var milkBarSequenceOffset = 0x3AC90;
            var formOffsets = new Dictionary<TransformationForm, int>
            {
                { TransformationForm.Human, 0x85 },
                { TransformationForm.Deku, 0xE6 },
                { TransformationForm.Zora, 0x158 },
                { TransformationForm.Goron, 0x16A },
            };
            var stringOffset = 0x178;
            milkBarActions.Add(() =>
            {
                // Change instrument for sequence 0x54 (milk bar performance) to 0x00
                ReadWriteUtils.WriteToROM(codeFileAddress + 0x13BB0A, 0x00);
                ReadWriteUtils.WriteToROM(audioSeqFileAddress + milkBarSequenceOffset + stringOffset, Instrument.FemaleVoice.Id());
            });
            var shouldPerformMilkBarActions = false;

            var playbackInstrumentsOffset = 0x12A8DC; // data for playback instruments
            var freePlayInstrumentsOffset = 0x12A8E4; // data for free play instruments
            var freePlayInstrumentsArrayAddress = 0x51CBE;
            var previouslyUsedInstruments = new List<Instrument>();
            foreach (var form in Enum.GetValues<TransformationForm>().Where(form => form != TransformationForm.FierceDeity).OrderBy(f => _cosmeticSettings.Instruments[f] == Instrument.Random))
            {
                var index = form.Id();

                if (form == TransformationForm.Human)
                {
                    // human and FD use the same instrument indices
                    index = 0;
                }

                var instrument = _cosmeticSettings.Instruments[form];

                if (instrument == form.DefaultInstrument())
                {
                    milkBarActions.Add(() => ReadWriteUtils.WriteToROM(audioSeqFileAddress + milkBarSequenceOffset + formOffsets[form], instrument.Id()));
                    previouslyUsedInstruments.Add(instrument);
                    continue;
                }

                shouldPerformMilkBarActions = true;

                if (instrument == Instrument.Random)
                {
                    instrument = Enum.GetValues(typeof(Instrument)).Cast<Instrument>()
                        .Where(u => ! previouslyUsedInstruments.Contains(u)).Skip(1).ToList()
                        .Random(random);
                }

                milkBarActions.Add(() => ReadWriteUtils.WriteToROM(audioSeqFileAddress + milkBarSequenceOffset + formOffsets[form], instrument.Id()));

                previouslyUsedInstruments.Add(instrument);
                var freePlayInstrumentIndex = ReadWriteUtils.Read(codeFileAddress + freePlayInstrumentsOffset + index) - 1;
                ReadWriteUtils.WriteToROM(freePlayInstrumentsArrayAddress + freePlayInstrumentIndex, instrument.Id());

                ReadWriteUtils.WriteToROM(codeFileAddress + playbackInstrumentsOffset + index, instrument.Id());
                Debug.WriteLine($" form: {form} was assigned instrument: {instrument}");
            }

            if (shouldPerformMilkBarActions)
            {
                foreach (var action in milkBarActions)
                {
                    action();
                }
            }
        }

        private void WriteMiscellaneousChanges()
        {
            if (_cosmeticSettings.EnableHoldZTargeting)
            {
                ResourceUtils.ApplyHack(Resources.mods.ztargetinghold);
            }

            if (_cosmeticSettings.EnableNightBGM)
            {
                SceneUtils.ReenableNightBGM();
            }

            if (!_cosmeticSettings.KeepPictoboxAntialiasing)
            {
                ResourceUtils.ApplyHack(Resources.mods.instant_pictobox);
            }

            WriteCrashDebuggerShow();
        }

        /// <summary>
        /// Write text for pictograph prompt.
        /// </summary>
        /// <param name="table"><see cref="MessageTable"/> to update.</param>
        private void WritePictographPromptText(MessageTable table)
        {
            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0xF8)
                .Message(it =>
                {
                    it.Text("Keep this ").StartRedText().PictureSubject().StartWhiteText().Text("?").NewLine()
                    .StartGreenText().Text(" ").NewLine()
                    .TwoChoices().Text("Yes").NewLine().Text("No")
                    .EndFinalTextBox()
                    .StartWhiteText();
                })
                .Build()
            );
        }

        /// <summary>
        /// Write text for swamp archery double reward message.
        /// </summary>
        /// <param name="table"><see cref="MessageTable"/> to update.</param>
        private void WriteArcheryDoubleRewardText(MessageTable table)
        {
            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0x23E)
                .Header(it =>
                {
                    it.Standard().Y(0).Icon(0xFE);
                })
                .Message(it =>
                {
                    it.Text("Y'played so well, y've'rned").NewLine()
                    .Text("yuhself the ").Red("grand prize").Text("!")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
        }

        /// <summary>
        /// Write text for bank post-reward when giving another reward afterwards.
        /// </summary>
        /// <param name="table"><see cref="MessageTable"/> to update.</param>
        private void WriteBankPostRewardText(MessageTable table)
        {
            // Copy of message 0x47A without EndFinalTextBox (0xBF).
            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0x1B67)
                .Header(it =>
                {
                    it.Standard2().Y(0).Icon(0xFE);
                })
                .Message(it =>
                {
                    it
                    .Text("See! Doesn't it hold more than").NewLine()
                    .Text("your old one? Fill it up and bring").NewLine()
                    .Text("it all in to deposit!").DisableTextSkip2();
                })
                .Build()
            );

            // Copy of message 0x47B without EndFinalTextBox (0xBF).
            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0x1B77)
                .Header(it =>
                {
                    it.Standard2().Y(0).Icon(0xFE);
                })
                .Message(it =>
                {
                    it.Text("That's what they call interest!").DisableTextSkip2();
                })
                .Build()
            );
        }

        /// <summary>
        /// Write text for pictograph prompt.
        /// </summary>
        /// <param name="table"><see cref="MessageTable"/> to update.</param>
        private void WriteBankPromptText(MessageTable table)
        {
            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0x450)
                .Message(it =>
                {
                    it.Text("How much? How much? ").NewLine()
                    .Text("\xCC").NewLine()
                    .Text("Set the amount with \xBB \xB4 \xB5").NewLine()
                    .Text("and press \xB0 to decide.")
                    .EndFinalTextBox();
                })
                .Build()
            );

            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0x46E)
                .Message(it =>
                {
                    it.Text("How much do you want?").NewLine()
                    .Text("\xCC").NewLine()
                    .Text("Set the amount with \xBB \xB4 \xB5").NewLine()
                    .Text("and press \xB0 to decide.")
                    .EndFinalTextBox();
                })
                .Build()
            );
        }

        /// <summary>
        /// Write text for Royal Wallet get-item message.
        /// </summary>
        /// <param name="table">Table to update.</param>
        private void WriteRoyalWalletText(MessageTable table)
        {
            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0xB)
                .Header(it =>
                {
                    // Using icon from Giant Wallet message.
                    it.FaintBlue().Y(1).Icon(0x9);
                })
                .Message(it =>
                {
                    // Note: Messages for Adult Wallet and Giant Wallet use 0xC2 (TwoChoices) before ending text box?
                    it.Text("You got a ").Red("Royal Wallet").Text("!").NewLine()
                    .Text("It can hold up to ").Red("999 Rupees").Text(".")
                    .EndFinalTextBox();
                })
                .Build()
            );
        }

        private void WriteMiscText()
        {
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3108)
                .Message(it =>
                {
                    it.Text("Say...Did you come to have some").NewLine()
                    .Text("items fashioned?")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3130)
                .Message(it =>
                {
                    it.Text("Gabora, fetch our customer some").NewLine()
                    .Text("coffee, quick-like.").EndTextBox()
                    .Text("Now then, let me take a look at").NewLine()
                    .Text("our offers.").EndTextBox()
                    .Text("Hmmm...")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3131)
                .Message(it =>
                {
                    it.Text("All right... For today's ").Red("hot deal").Text(",").NewLine()
                    .Text("it will cost you ").Pink("100 Rupees").Text(". It'll").NewLine()
                    .Text("be ready at ").Red("sunrise").Text(".")
                    .EndTextBox()
                    .Text("So how about it? Wanna grab a").NewLine()
                    .Text("hot item for ").Pink("100 Rupees").Text("?").NewLine()
                    .StartGreenText()
                    .TwoChoices()
                    .Text("I'll do it").NewLine()
                    .Text("No thanks")
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3133)
                .Message(it =>
                {
                    it.Text("This is a secret, but I'll let you in").NewLine()
                    .Text("on it... The strongest sword out").NewLine()
                    .Text("there was forged using ").Red(() => {
                        it.Text("gold").NewLine().Text("dust");
                    })
                    .Text(".... I made it! Me!")
                    .EndConversation()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3134)
                .Message(it =>
                {
                    it.Text("Wanna grab a deal? ").NewLine()
                    .StartGreenText().Text(" ").NewLine()
                    .TwoChoices()
                    .Text("Yes").NewLine()
                    .Text("No thanks")
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3140)
                .Message(it =>
                {
                    it.Text("Hey! It's gonna be ready ").Red(() => {
                        it.Text("tomorrow").NewLine()
                        .Text("morning");
                    })
                    .Text(". We'll take good care of").NewLine()
                    .Text("it, so don't worry.")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3141)
                .Message(it =>
                {
                    it.Text("Hey! For today's special product").NewLine()
                    .Text("we'll need to get hold of some ").NewLine()
                    .Red("gold dust").Text(".")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3142)
                .Message(it =>
                {
                    it.Text("Why, if it isn't ").Red("gold dust").Text("! And it's").NewLine()
                    .Text("even top quality!!!")
                    .EndTextBox()
                    .Text("Why, even if I use it to craft").NewLine()
                    .Text("a nifty item, there'll still be some").NewLine()
                    .Text("left...")
                    .EndTextBox()
                    .Text("All right! Just for you, I'll do this").NewLine()
                    .Text("for free. But don't tell anyone!")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3147)
                .Message(it =>
                {
                    it.Text("To make our item for you today,").NewLine()
                    .Text("I'll need ").Red("gold dust").Text(", which just so").NewLine()
                    .Text("happens to be first prize at the").NewLine()
                    .Text("Goron racetrack.")
                    .EndTextBox()
                    .Text("If I can just get some gold dust...").NewLine()
                    .Text("and this is just between us...I can").NewLine()
                    .Text("make you the ").Red(() => {
                        it.Text("hottest item").NewLine()
                        .Text("in the lands");
                    })
                    .Text("... Really!!")
                    .EndConversation()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3150)
                .Message(it =>
                {
                    it.Text("Huh? ").PauseText(0x10).Text("Look, I'm working on").NewLine()
                    .Text("making this item for you. I'm").NewLine()
                    .Text("busy, so don't bother me.")
                    .EndConversation()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3153)
                .Message(it =>
                {
                    it.Text("Behold! My skills in craftsmanship").NewLine()
                    .Text("are truly unrivalled!")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(3155)
                .Message(it =>
                {
                    it.Text("Ah! My finest work!").NewLine()
                    .Text("The look in your eye, I can").NewLine()
                    .Text("tell you really wanted this!!")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );
        }

        private Character DeterminePlayerModel()
        {
            var data = ObjUtils.GetObjectData(0x11);
            var hash = MD5.Create().ComputeHash(data);

            if (hash.SequenceEqual(PlayerModelHash.LinkMM))
            {
                return Character.LinkMM;
            }
            else if (hash.SequenceEqual(PlayerModelHash.LinkOOT))
            {
                return Character.LinkOOT;
            }
            else if (hash.SequenceEqual(PlayerModelHash.AdultLink))
            {
                return Character.AdultLink;
            }
            else if (hash.SequenceEqual(PlayerModelHash.Kafei))
            {
                return Character.Kafei;
            }
            else
            {
                throw new Exception("Unable to determine player's model.");
            }
        }

        private void SetTatlColour(Random random)
        {
            if (_cosmeticSettings.TatlColorSchema == TatlColorSchema.Random)
            {
                for (int i = 0; i < 10; i++)
                {
                    byte[] c = new byte[4];
                    random.NextBytes(c);

                    if ((i % 2) == 0)
                    {
                        c[0] = 0xFF;
                    }
                    else
                    {
                        c[0] = 0;
                    }

                    Values.TatlColours[4, i] = BitConverter.ToUInt32(c, 0);
                }
            }
        }

        private void WriteTatlColour(Random random)
        {
            if (_cosmeticSettings.TatlColorSchema != TatlColorSchema.Rainbow)
            {
                SetTatlColour(random);
                var selectedColorSchemaIndex = (int)_cosmeticSettings.TatlColorSchema;
                byte[] c = new byte[8];
                List<int[]> locs = ResourceUtils.GetAddresses(Resources.addresses.tatl_colour);
                for (int i = 0; i < locs.Count; i++)
                {
                    ReadWriteUtils.Arr_WriteU32(c, 0, Values.TatlColours[selectedColorSchemaIndex, i << 1]);
                    ReadWriteUtils.Arr_WriteU32(c, 4, Values.TatlColours[selectedColorSchemaIndex, (i << 1) + 1]);
                    ReadWriteUtils.WriteROMAddr(locs[i], c);
                }
            }
            else
            {
                ResourceUtils.ApplyHack(Resources.mods.rainbow_tatl);
            }
        }

        private void WriteNutsAndSticks()
        {
            /// adds deku sticks and deku nuts as additional drops to the drop tables, very useful in enemizer
            /// when an actor drops an item, they roll a 16 side die, sometimes hardcode overrides in special cases (fairy)
            ///   all of the slots replaced here with sticks and nuts were empty in vanilla
            /// image guide from mzxrules of the drop tables in vanilla
            /// https://pbs.twimg.com/media/Dct7fa6X4AEeYpv?format=jpg&name=large 

            if (_randomized.Settings.NutandStickDrops == NutAndStickDrops.Default)
            {
                return;
            }

            const byte DEKUNUT   = 0x0C;
            const byte DEKUSTICK = 0x0D;
            int  bushCount = (int)_randomized.Settings.NutandStickDrops;
            byte nutCount = _randomized.Settings.NutandStickDrops == NutAndStickDrops.Light ? (byte) 0x1 : (byte)bushCount;
            byte stickCount = (byte)Math.Max((int)_randomized.Settings.NutandStickDrops - 2, 1);
            int  droptableFileID = RomUtils.GetFileIndexForWriting(0xC444B8);
            RomUtils.CheckCompressed(droptableFileID);

            void AddDropToDropTable(byte dropType, int replacementSlot = 0xC444B8, byte amount = 0)
            {                
                // each replacementSlot is a single 1/16 slot for random item drop
                int offset = replacementSlot - RomData.MMFileList[droptableFileID].Addr;
                RomData.MMFileList[droptableFileID].Data[offset] = dropType;
                // how many items are dropped is the table that follows, aligns exactly with 0x110
                RomData.MMFileList[droptableFileID].Data[offset + 0x110] = amount;
            }

            // termina field bushes 1/16 drop table entry
            AddDropToDropTable(DEKUNUT, 0xC444B7, nutCount);
            AddDropToDropTable(DEKUSTICK, 0xC444BF, stickCount);
            if (bushCount >= 2) // medium and higher
            {
                // another slot in the TF grass drop table
                AddDropToDropTable(DEKUNUT, 0xC444B8, nutCount);
                AddDropToDropTable(DEKUSTICK, 0xC444C0, stickCount);
            }
            if (bushCount >= 3) // extra and higher
            {
                // another slot in the TF grass drop table
                AddDropToDropTable(DEKUNUT, 0xC444BC, nutCount);
                AddDropToDropTable(DEKUSTICK, 0xC444C1, stickCount);
                // if extra and higher, add some to non termina field droplists
                AddDropToDropTable(DEKUNUT, 0xC444CB, nutCount);   // stalchild and south swamp
                AddDropToDropTable(DEKUSTICK, 0xC444CC, stickCount);
                AddDropToDropTable(DEKUNUT, 0xC44574, nutCount);   // lens of truth grass
                AddDropToDropTable(DEKUSTICK, 0xC44575, stickCount);
            }
            if (bushCount >= 4) // mayhem
            {
                // nuts and sticks in weird drop tables too for mayhem
                AddDropToDropTable(DEKUNUT, 0xC444F8, nutCount);   // graveyard rocks
                AddDropToDropTable(DEKUSTICK, 0xC444F9, stickCount);
                AddDropToDropTable(DEKUNUT, 0xC444D6, nutCount);   // snow trees and snow rocks
                AddDropToDropTable(DEKUSTICK, 0xC444D7, stickCount);
                AddDropToDropTable(DEKUNUT, 0xC445BA, nutCount);   // field mice
                AddDropToDropTable(DEKUSTICK, 0xC445BB, stickCount);
            }
        }

        private void WriteQuickText()
        {
            if (_randomized.Settings.QuickTextEnabled)
            {
                ResourceUtils.ApplyHack(Resources.mods.quick_text);
            }
        }

        private void WriteCutscenes()
        {
            foreach (var shortenCutsceneGroup in _randomized.Settings.ShortenCutsceneSettings
                .GetType()
                .GetProperties()
                .Select(p => p.GetValue(_randomized.Settings.ShortenCutsceneSettings)).Cast<Enum>())
            {
                foreach (var value in Enum.GetValues(shortenCutsceneGroup.GetType()).Cast<Enum>())
                {
                    if (Convert.ToInt32(value) == 0)
                    {
                        continue;
                    }
                    if (shortenCutsceneGroup.HasFlag(value))
                    {
                        Debug.WriteLine($"Applying Shortened Cutscene: {value}");
                        var hackContentAttributes = value.GetAttributes<HackContentAttribute>();
                        foreach (var hackContent in hackContentAttributes.Select(h => h.HackContent))
                        {
                            ResourceUtils.ApplyHack(hackContent);
                        }
                    }
                }
            }

            if (_randomized.Settings.ShortenCutsceneSettings.General.HasFlag(ShortenCutsceneGeneral.AutomaticCredits))
            {
                for (ushort i = 0x1F5F; i <= 0x1F75; i++)
                {
                    var message = _messageTable.GetMessage(i);
                    if (!message.Message.Contains('\x1C'))
                    {
                        if (message.Message.Contains('\x13'))
                        {
                            var messages = message.Message.Split("\u0011\u0013\u0012");
                            ushort? nextMessageId = null;
                            for (var j = messages.Length - 1; j >= 0; j--)
                            {
                                var newMessage = messages[j];
                                var lines = newMessage.Count(c => c == '\x11') + 1;
                                newMessage = newMessage.Replace("\u00BF", "") + "\u001C\u0000" + (char)(lines * 0x20) + "\u00BF";
                                var newMessageId = (ushort) ((_extraMessages.Max(me => (ushort?)me.Id) ?? 0x9001) + 1);
                                var newHeader = message.Header.ToArray();
                                if (nextMessageId.HasValue)
                                {
                                    ReadWriteUtils.Arr_WriteU16(newHeader, 3, nextMessageId.Value);
                                }
                                if (j > 0)
                                {
                                    if (message.Message.StartsWith('\x05'))
                                    {
                                        newMessage = '\x05' + newMessage;
                                    }

                                    _extraMessages.Add(new MessageEntry
                                    {
                                        Id = newMessageId,
                                        Header = newHeader,
                                        Message = newMessage,
                                    });
                                }
                                else
                                {
                                    message.Message = newMessage;
                                    message.Header = newHeader;
                                }
                                nextMessageId = newMessageId;
                            }
                        }
                        else
                        {
                            var lines = message.Message.Count(c => c == '\x11') + 1;
                            message.Message = message.Message.Replace("\u00BF", "\u001C\u0000" + (char)(lines * 0x20) + "\u00BF");
                        }

                        message.Message = message.Message.Replace("\u0015", "");
                    }
                }
            }
        }

        private void WriteDungeons()
        {
            if (_randomized.Settings.LogicMode == LogicMode.Vanilla || !_randomized.Settings.RandomizeDungeonEntrances)
            {
                return;
            }

            SceneUtils.ReadSceneTable();
            SceneUtils.GetMaps();

            var entrances = new List<Item>
            {
                Item.AreaWoodFallTempleAccess,
                Item.AreaWoodFallTempleClear,
                Item.AreaSnowheadTempleAccess,
                Item.AreaSnowheadTempleClear,
                Item.AreaGreatBayTempleAccess,
                Item.AreaGreatBayTempleClear,
                Item.AreaInvertedStoneTowerTempleAccess,
                Item.AreaStoneTowerClear,
            };

            foreach (var entrance in entrances)
            {
                var newSpawns = entrance.DungeonEntrances();
                var exits = _randomized.ItemList[entrance].NewLocation.Value.DungeonEntrances();

                var mainExit = exits[0];
                var mainSpawn = newSpawns[0];

                EntranceSwapUtils.WriteNewEntrance(mainExit, mainSpawn);

                if (exits.Count > 1 && newSpawns.Count > 1)
                {
                    var pairExit = newSpawns[1];
                    var pairSpawn = exits[1];

                    EntranceSwapUtils.WriteNewEntrance(pairExit, pairSpawn);
                }
            }

            var clears = new List<Item>
            {
                Item.AreaWoodFallTempleClear,
                Item.AreaSnowheadTempleClear,
                Item.AreaStoneTowerClear,
                Item.AreaGreatBayTempleClear,
            };

            byte[] li = new byte[] { 0x24, 0x02, 0x00, 0x00 };
            byte[] addiuAtR0 = new byte[] { 0x24, 0x01, 0x00, 0x00 };
            var dCheckAddr = ResourceUtils.GetAddresses(Resources.addresses.d_check);
            var dcFlagloadAddr = ResourceUtils.GetAddresses(Resources.addresses.dc_flagload);
            var dcFlagmaskAddr = ResourceUtils.GetAddresses(Resources.addresses.dc_flagmask);
            var dGiantsCsAddr = ResourceUtils.GetAddresses(Resources.addresses.d_giants_cs);
            for (var i = 0; i < clears.Count; i++)
            {
                var clear = clears[i];
                var exit = _randomized.ItemList.SingleOrDefault(io => io.NewLocation == clear).Item;
                var newIndex = clears.IndexOf(exit);
                if (newIndex < 0)
                {
                    throw new Exception("Error randomizing dungoens.");
                }

                // Alter the Boss Warp to set the correct clear flag and next entrance.
                li[3] = (byte)newIndex;
                ReadWriteUtils.WriteROMAddr(dCheckAddr[i], li);

                // Alter the Giants Cutscene to set the correct exit value.
                addiuAtR0[3] = Values.DCSceneIds[i];
                ReadWriteUtils.WriteROMAddr(dGiantsCsAddr[newIndex], addiuAtR0);

                // Alter which address is checked when determining if an area is cleared.
                ReadWriteUtils.WriteROMAddr(dcFlagloadAddr[i], new byte[] { (byte)((Values.DCFlags[newIndex] & 0xFF00) >> 8), (byte)(Values.DCFlags[newIndex] & 0xFF) });

                // Alter the bit mask to use when determining if an area is cleared.
                ReadWriteUtils.WriteROMAddr(dcFlagmaskAddr[i], new byte[] {
                    (byte)((Values.DCFlagMasks[newIndex] & 0xFF00) >> 8),
                    (byte)(Values.DCFlagMasks[newIndex] & 0xFF) });
            }
        }

        private void WriteSpeedUps()
        {
            if (_randomized.Settings.SpeedupBeavers)
            {
                ResourceUtils.ApplyHack(Resources.mods.speedup_beavers);
                _messageTable.UpdateMessages(new MessageEntryBuilder()
                    .Id(0x10D6)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x291A)
                        .CompileTimeWrap((wrapped) =>
                        {
                            wrapped.Text("There's a total of ")
                            .Red("25 rings")
                            .Text(". You must swim through them in the right order for it to count. Swim through the ring that's ")
                            .Red("flashing")
                            .Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("My big brother will show you the way, so follow him and don't get separated!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                _messageTable.UpdateMessages(new MessageEntryBuilder()
                    .Id(0x10FA)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x2919).Text("This time, the limit is ").Red("1:50").Text(".")
                        .EndTextBox()
                        .Text("Don't fall behind!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                _messageTable.UpdateMessages(new MessageEntryBuilder()
                    .Id(0x1107)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x2919)
                        .CompileTimeWrap((wrapped) =>
                        {
                            wrapped.Text("This time around, you have to beat ").Red("1:40").Text(".");
                        })
                        .EndTextBox()
                        .Text("Don't fall behind!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            if (_randomized.Settings.SpeedupDampe)
            {
                ResourceUtils.ApplyHack(Resources.mods.speedup_dampe);
            }

            if (_randomized.Settings.SpeedupLabFish)
            {
                ResourceUtils.ApplyHack(Resources.mods.speedup_labfish);
            }

            if (_randomized.Settings.SpeedupDogRace)
            {
                ResourceUtils.ApplyHack(Resources.mods.speedup_dograce);
            }

            if (_randomized.Settings.SpeedupBank)
            {
                ResourceUtils.ApplyHack(Resources.mods.speedup_bank);
                _messageTable.UpdateMessages(new MessageEntryBuilder()
                    .Id(0x45C)
                    .Message(it =>
                    {
                        it.QuickText(() =>
                        {
                            it.Text("What's this? You've already saved").NewLine()
                            .Text("up ").Red("500 Rupees").Text("!?!");
                        })
                        .EndTextBox()
                        .Text("Well, little guy, here's your special").NewLine()
                        .Text("gift. Take it!")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                _messageTable.UpdateMessages(new MessageEntryBuilder()
                    .Id(0x45D)
                    .Message(it =>
                    {
                        it.QuickText(() =>
                        {
                            it.Text("What's this? You've already saved").NewLine()
                            .Text("up ").Red("1000 Rupees").Text("?!");
                        })
                        .EndTextBox()
                        .Text("Well, little guy, I can't take any").NewLine()
                        .Text("more deposits. Sorry, but this is").NewLine()
                        .Text("all I can give you.")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }
        }

        private void WriteGimmicks()
        {
            int damageMultiplier = (int)_randomized.Settings.DamageMode;
            if (damageMultiplier > 0)
            {
                ResourceUtils.ApplyIndexedHack(damageMultiplier-1, Resources.mods.dm_1, Resources.mods.dm_2, Resources.mods.dm_3, Resources.mods.dm_4);
            }

            int damageEffect = (int)_randomized.Settings.DamageEffect;
            if (damageEffect > 0)
            {
                ResourceUtils.ApplyIndexedHack(damageEffect - 1, Resources.mods.de_1, Resources.mods.de_2, Resources.mods.de_3, Resources.mods.de_4, Resources.mods.de_5);
            }

            int gravityType = (int)_randomized.Settings.MovementMode;
            if (gravityType > 0)
            {
                ResourceUtils.ApplyIndexedHack(gravityType - 1, Resources.mods.movement_1, Resources.mods.movement_2, Resources.mods.movement_3, Resources.mods.movement_4);
            }

            int floorType = (int)_randomized.Settings.FloorType;
            if (floorType > 0)
            {
                ResourceUtils.ApplyIndexedHack(floorType - 1, Resources.mods.floor_1, Resources.mods.floor_2, Resources.mods.floor_3, Resources.mods.floor_4);
            }

            if (_randomized.Settings.ClockSpeed != ClockSpeed.Default)
            {
                WriteClockSpeed(_randomized.Settings.ClockSpeed);
            }

            if (_randomized.Settings.HideClock)
            {
                WriteHideClock();
            }

            if (_randomized.Settings.BlastMaskCooldown != BlastMaskCooldown.Default)
            {
                WriteBlastMaskCooldown();
            }

            if (_randomized.Settings.EnableSunsSong)
            {
                WriteSunsSong();
            }

            if (_randomized.Settings.AllowFierceDeityAnywhere)
            {
                ResourceUtils.ApplyHack(Resources.mods.fierce_deity_anywhere);
            }

            if (_randomized.Settings.ByoAmmo)
            {
                ResourceUtils.ApplyHack(Resources.mods.byo_ammo);
            }

            if (_randomized.Settings.DeathMoonCrash)
            {
                ResourceUtils.ApplyHack(Resources.mods.death_moon_crash);
            }

            if (_randomized.Settings.HookshotAnySurface)
            {
                ResourceUtils.ApplyHack(Resources.mods.hookshot_any_surface);
            }

            if (_randomized.Settings.ClimbMostSurfaces)
            {
                ResourceUtils.ApplyHack(Resources.mods.climb_most_surfaces);
            }
        }

        private void WriteSunsSong()
        {
            _messageTable.UpdateMessages(new MessageEntryBuilder()
                .Id(0x1B7D)
                .Header(it =>
                {
                    it.OcarinaStaff();
                })
                .Message(it =>
                {
                    it.Text("You played the ").Yellow("Sun's Song").Text("!")
                    .EndFinalTextBox();
                })
                .Build()
            );

            ResourceUtils.ApplyHack(Resources.mods.enable_sunssong);
        }

        private void WriteBlastMaskCooldown()
        {
            ushort value;
            switch (_randomized.Settings.BlastMaskCooldown)
            {
                default:
                case BlastMaskCooldown.Default:
                    value = 0x136; // 310 frames 
                    break;
                case BlastMaskCooldown.Instant:
                    value = 0x1; // 1 frame
                    break;
                case BlastMaskCooldown.VeryShort:
                    value = 0x20; // 32 frames
                    break;
                case BlastMaskCooldown.Short:
                    value = 0x80; // 128 frames
                    break;
                case BlastMaskCooldown.Long:
                    value = 0x200; // 512 frames
                    break;
                case BlastMaskCooldown.VeryLong:
                    value = 0x400; // 1024 frames
                    break;
            }

            var codeFileAddress = 0x00CA7F00;
            var offset = 0x002766;
            ReadWriteUtils.WriteToROM(codeFileAddress + offset, value);
        }

        private void WriteHideClock()
        {
            var codeFileAddress = 0xB3C000;
            var offset = 0x73B7C; // branch for UI is time hasn't changed
            ReadWriteUtils.WriteToROM(codeFileAddress + offset, 0x10); // change to always branch
        }

        /// <summary>
        /// Overwrite the clockspeed (see Settings.ClockSpeed for details)
        /// </summary>
        /// <param name="clockSpeed"></param>
        private void WriteClockSpeed(ClockSpeed clockSpeed)
        {
            byte speed;
            short invertedModifier;
            switch (clockSpeed)
            {
                default:
                case ClockSpeed.Default:
                    speed = 3;
                    invertedModifier = -2;
                    break;
                case ClockSpeed.VerySlow:
                    speed = 1;
                    invertedModifier = 0;
                    break;
                case ClockSpeed.Slow:
                    speed = 2;
                    invertedModifier = -1;
                    break;
                case ClockSpeed.Fast:
                    speed = 6;
                    invertedModifier = -4;
                    break;
                case ClockSpeed.VeryFast:
                    speed = 9;
                    invertedModifier = -6;
                    break;
                case ClockSpeed.SuperFast:
                    speed = 18;
                    invertedModifier = -12;
                    break;
            }

            ResourceUtils.ApplyHack(Resources.mods.fix_clock_speed);

            var codeFileAddress = 0xB3C000;
            var hackAddressOffset = 0x8A674;
            var modificationOffset = 0x1B;
            ReadWriteUtils.WriteToROM(codeFileAddress + hackAddressOffset + modificationOffset, speed);

            var invertedModifierOffsets = new List<int>
            {
                0xB1B8E,
                0x7405E
            };
            foreach (var offset in invertedModifierOffsets)
            {
                ReadWriteUtils.WriteToROM(codeFileAddress + offset, (ushort)invertedModifier);
            }
        }

        /// <summary>
        /// Update the gossip stone actor to not check mask of truth
        /// </summary>
        private void WriteFreeHints()
        {
            int address = 0x00E0A810 + 0x378;
            byte val = 0x00;
            ReadWriteUtils.WriteToROM(address, val);
        }

        private void WriteSoundEffects(Random random)
        {
            if (!_cosmeticSettings.RandomizeSounds)
            {
                return;
            }

            var shuffledSoundEffects = new Dictionary<SoundEffect, SoundEffect>();

            var replacableSounds = SoundEffects.Replacable();
            foreach (var sound in replacableSounds)
            {
                var soundPool = SoundEffects.FilterByTags(sound.ReplacableByTags());

                if (soundPool.Count > 0)
                {
                    shuffledSoundEffects[sound] = soundPool.Random(random);
                }
            }

            shuffledSoundEffects.Remove(SoundEffect.LowHealthBeep); // handled in the WriteLowHealthSound function

            foreach (var sounds in shuffledSoundEffects)
            {
                var oldSound = sounds.Key;
                var newSound = sounds.Value;

                if (oldSound.IsReplacableInMessage())
                {
                    oldSound.ReplaceInMessageWith(newSound, _messageTable);
                }
                else
                {
                    oldSound.ReplaceWith(newSound);
                }
                Debug.WriteLine($"Writing SFX {newSound} --> {oldSound}");
            }
        }

        private void WriteLowHealthSound(Random random)
        {
            if (_cosmeticSettings.LowHealthSFX == LowHealthSFX.Default)
            {
                return;
            }
            
            if (_cosmeticSettings.LowHealthSFX == LowHealthSFX.Disabled)
            {
                var replacableAttribute = SoundEffect.LowHealthBeep.GetAttribute<ReplacableAttribute>();
                var addresses = replacableAttribute.Addresses;
                foreach (var address in addresses)
                {
                    ReadWriteUtils.WriteToROM(address, (ushort)0);
                }
            }
            else if ((int) _cosmeticSettings.LowHealthSFX > (int) LowHealthSFX.Random)
            {
                SoundEffect.LowHealthBeep.ReplaceWith( (SoundEffect) _cosmeticSettings.LowHealthSFX);
            }
            else if(_cosmeticSettings.LowHealthSFX == LowHealthSFX.Random)
            {
                var soundPool = SoundEffects.FilterByTags(SoundEffect.LowHealthBeep.ReplacableByTags());
                if (soundPool.Count > 0)
                {
                    SoundEffect.LowHealthBeep.ReplaceWith(soundPool.Random(random));
                }
            }
        }

        private void WriteEnemies()
        {
            if (_randomized.Settings.RandomizeEnemies)
            {
                Enemies.ShuffleEnemies(new Random(_randomized.Seed));
            }
        }

        private void PutOrCombine(Dictionary<int, byte> dictionary, int key, byte value, bool add = false)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = 0;
            }
            dictionary[key] = add ? (byte)(dictionary[key] + value) : (byte)(dictionary[key] | value);
        }

        private void WriteFreeItems(params Item[] items)
        {
            Dictionary<int, byte> startingItems = new Dictionary<int, byte>();
            if (_randomized.Settings.EnableSunsSong)
            {
                PutOrCombine(startingItems, 0xC5CE71, 0x02);
            }

            var itemList = items.Where(item => item != Item.RecoveryHeart).ToList();

            if (_randomized.Settings.CustomStartingItemList != null)
            {
                itemList.AddRange(_randomized.Settings.CustomStartingItemList);
            }

            itemList = itemList.Distinct().ToList();

            itemList.Add(Item.StartingHeartContainer1);
            while (itemList.Count(item => item.Name() == "Piece of Heart") >= 4)
            {
                itemList.Add(Item.StartingHeartContainer1);
                for (var i = 0; i < 4; i++)
                {
                    var heartPiece = itemList.First(item => item.Name() == "Piece of Heart");
                    itemList.Remove(heartPiece);
                }
            }

            if (_randomized.Settings.ProgressiveUpgrades)
            {
                itemList = itemList
                    .GroupBy(item => ItemUtils.ForbiddenStartTogether.FirstOrDefault(fst => fst.Contains(item)))
                    .SelectMany(g => g.Key == null || g.Key.Contains(Item.StartingShield) ? g.ToList() : g.Key.Skip(g.Count()-1).Take(1))
                    .ToList();
            }

            itemList = itemList
                .GroupBy(item => ItemUtils.ForbiddenStartTogether.FirstOrDefault(fst => fst.Contains(item)))
                .SelectMany(g => g.Key == null ? g.ToList() : g.OrderByDescending(item => g.Key.IndexOf(item)).Take(1))
                .ToList();

            _randomized.Settings.AsmOptions.MMRConfig.ExtraStartingMaps = TingleMap.None;
            _randomized.Settings.AsmOptions.MMRConfig.ExtraStartingItemIds.Clear();
            foreach (var item in itemList)
            {
                var startingTingleMap = item.GetAttribute<StartingTingleMapAttribute>();
                if (startingTingleMap != null)
                {
                    _randomized.Settings.AsmOptions.MMRConfig.ExtraStartingMaps |= startingTingleMap.TingleMap;
                    continue;
                }
                if (item.HasAttribute<StartingItemIdAttribute>())
                {
                    foreach (var startingItemIdAttribute in item.GetAttributes<StartingItemIdAttribute>())
                    {
                        _randomized.Settings.AsmOptions.MMRConfig.ExtraStartingItemIds.Add(startingItemIdAttribute.ItemId);
                    }
                    continue;
                }
                var startingItemValues = item.GetAttributes<StartingItemAttribute>();
                if (!startingItemValues.Any() && _randomized.Settings.StartingItemMode != StartingItemMode.None)
                {
                    throw new Exception($@"Invalid starting item ""{item}""");
                }
                foreach (var startingItem in startingItemValues)
                {
                    PutOrCombine(startingItems, startingItem.Address, startingItem.Value, startingItem.IsAdditional);
                }
            }

            foreach (var kvp in startingItems)
            {
                ReadWriteUtils.WriteToROM(kvp.Key, kvp.Value);
            }

            if (itemList.Count(item => item.Name() == "Heart Container") == 1)
            {
                ReadWriteUtils.WriteToROM(0x00B97E8F, 0x0C); // reduce low health beep threshold
            }
        }

        private ushort GetLocationIdOfItem(Item item)
        {
            var itemObject = _randomized.ItemList[item];
            return itemObject.Item == item ? itemObject.NewLocation.Value.GetItemIndex().Value : (ushort)0;
        }

        private void WriteMiscHacks()
        {
            var hacks = new List<byte[]>();

            if (_randomized.Settings.SmallKeyMode.HasFlag(SmallKeyMode.DoorsOpen))
            {
                hacks.AddRange(SmallKeyMode.DoorsOpen.GetAttributes<HackContentAttribute>().Select(hc => hc.HackContent));
            }

            if (_randomized.Settings.BossKeyMode.HasFlag(BossKeyMode.DoorsOpen))
            {
                hacks.AddRange(BossKeyMode.DoorsOpen.GetAttributes<HackContentAttribute>().Select(hc => hc.HackContent));
            }

            if (_randomized.Settings.BossKeyMode.HasFlag(BossKeyMode.KeepThroughTime))
            {
                hacks.AddRange(BossKeyMode.KeepThroughTime.GetAttributes<HackContentAttribute>().Select(hc => hc.HackContent));
            }

            ushort requiredStrayFairies = 15;
            if (_randomized.Settings.StrayFairyMode.HasFlag(StrayFairyMode.ChestsOnly))
            {
                requiredStrayFairies = 0;
                hacks.AddRange(StrayFairyMode.ChestsOnly.GetAttributes<HackContentAttribute>().Select(hc => hc.HackContent));
            }

            requiredStrayFairies += 0xA; // Needed for the value to be correct.
            ReadWriteUtils.WriteToROM(0x00EA3366, requiredStrayFairies);

            if (_randomized.Settings.LenientGoronSpikes)
            {
                hacks.Add(Resources.mods.lenient_goron_spikes);
            }

            if (_randomized.Settings.TargetHealthBar)
            {
                hacks.Add(Resources.mods.enemy_max_health);
            }

            if (_randomized.Settings.LogicMode != LogicMode.Vanilla)
            {
                if (!_randomized.Settings.CustomStartingItemList.Contains(Item.ItemOcarina) || !_randomized.Settings.CustomStartingItemList.Contains(Item.SongTime)
                    || _randomized.Settings.CustomItemList.Contains(Item.ItemOcarina) || _randomized.Settings.CustomItemList.Contains(Item.SongTime))
                {
                    hacks.Add(Resources.mods.fix_ocarina_checks);
                    hacks.Add(Resources.mods.fix_song_of_time);
                }
            }

            foreach (var hack in hacks)
            {
                ResourceUtils.ApplyHack(hack);
            }
        }

        private void WriteItems()
        {
            var freeItems = new List<Item>();
            if (_randomized.Settings.LogicMode == LogicMode.Vanilla)
            {
                freeItems.Add(Item.FairyMagic);
                freeItems.Add(Item.MaskDeku);
                freeItems.Add(Item.ItemOcarina);
                freeItems.Add(Item.SongTime);
                freeItems.Add(Item.SongHealing);
                freeItems.Add(Item.StartingSword);
                freeItems.Add(Item.StartingShield);
                freeItems.Add(Item.StartingHeartContainer1);
                freeItems.Add(Item.StartingHeartContainer2);

                if (_randomized.Settings.ShortenCutsceneSettings.General.HasFlag(ShortenCutsceneGeneral.EverythingElse))
                {
                    //giants cs were removed
                    freeItems.Add(Item.SongOath);
                }

                WriteFreeItems(freeItems.ToArray());

                return;
            }

            //write free item (start item default = Deku Mask)
            freeItems.Add(_randomized.ItemList.Find(u => u.NewLocation == Item.MaskDeku).Item);
            freeItems.Add(_randomized.ItemList.Find(u => u.NewLocation == Item.SongHealing).Item);
            freeItems.Add(_randomized.ItemList.Find(u => u.NewLocation == Item.StartingSword).Item);
            freeItems.Add(_randomized.ItemList.Find(u => u.NewLocation == Item.StartingShield).Item);
            freeItems.Add(_randomized.ItemList.Find(u => u.NewLocation == Item.StartingHeartContainer1).Item);
            freeItems.Add(_randomized.ItemList.Find(u => u.NewLocation == Item.StartingHeartContainer2).Item);
            WriteFreeItems(freeItems.ToArray());

            //write everything else
            ItemSwapUtils.ReplaceGetItemTable();
            ItemSwapUtils.InitItems();

            // Write extended object indexes to Get-Item list entries.
            WriteExtendedObjects();

            if (_randomized.Settings.FixEponaSword)
            {
                ResourceUtils.ApplyHack(Resources.mods.fix_epona);
            }
            if (_randomized.Settings.PreventDowngrades)
            {
                ResourceUtils.ApplyHack(Resources.mods.fix_downgrades);
            }
            if (_randomized.Settings.CustomItemList.Any(item => item.ItemCategory() == ItemCategory.Milk))
            {
                ResourceUtils.ApplyHack(Resources.mods.fix_cow_bottle_check);
            }

            ResourceUtils.ApplyHack(Resources.mods.update_trade_scrubs);

            var newMessages = new List<MessageEntry>();
            _randomized.Settings.AsmOptions.MMRConfig.RupeeRepeatableLocations.Clear();
            _randomized.Settings.AsmOptions.MMRConfig.ItemsToReturnIds.Clear();
            foreach (var item in _randomized.ItemList)
            {
                // Unused item
                if (item.NewLocation == null)
                {
                    continue;
                }

                if (item.Item.DungeonEntrances() != null)
                {
                    continue;
                }

                if (ItemUtils.IsBottleCatchContent(item.Item))
                {
                    ItemSwapUtils.WriteNewBottle(item.NewLocation.Value, item.Item);
                }
                else
                {
                    ChestTypeAttribute.ChestType? overrideChestType = null;
                    if ((item.Item.Name().Contains("Bombchu") || item.Item.Name().Contains("Shield")) && _randomized.Logic.Any(il => il.RequiredItemIds?.Contains(item.ID) == true || il.ConditionalItemIds?.Any(c => c.Contains(item.ID)) == true))
                    {
                        overrideChestType = ChestTypeAttribute.ChestType.LargeGold;
                    }
                    ItemSwapUtils.WriteNewItem(item, newMessages, _randomized.Settings, item.Mimic?.ChestType ?? overrideChestType, _messageTable, _extendedObjects);
                }
            }

            _randomized.Settings.AsmOptions.MMRConfig.LocationBottleRedPotion = GetLocationIdOfItem(Item.ItemBottleWitch);
            _randomized.Settings.AsmOptions.MMRConfig.LocationBottleGoldDust = GetLocationIdOfItem(Item.ItemBottleGoronRace);
            _randomized.Settings.AsmOptions.MMRConfig.LocationBottleMilk = GetLocationIdOfItem(Item.ItemBottleAliens);
            _randomized.Settings.AsmOptions.MMRConfig.LocationBottleChateau = GetLocationIdOfItem(Item.ItemBottleMadameAroma);

            _randomized.Settings.AsmOptions.MMRConfig.LocationSwordKokiri = GetLocationIdOfItem(Item.StartingSword);
            _randomized.Settings.AsmOptions.MMRConfig.LocationSwordRazor = GetLocationIdOfItem(Item.UpgradeRazorSword);
            _randomized.Settings.AsmOptions.MMRConfig.LocationSwordGilded = GetLocationIdOfItem(Item.UpgradeGildedSword);

            _randomized.Settings.AsmOptions.MMRConfig.LocationMagicSmall = GetLocationIdOfItem(Item.FairyMagic);
            _randomized.Settings.AsmOptions.MMRConfig.LocationMagicLarge = GetLocationIdOfItem(Item.FairyDoubleMagic);

            _randomized.Settings.AsmOptions.MMRConfig.LocationWalletAdult = GetLocationIdOfItem(Item.UpgradeAdultWallet);
            _randomized.Settings.AsmOptions.MMRConfig.LocationWalletGiant = GetLocationIdOfItem(Item.UpgradeGiantWallet);
            _randomized.Settings.AsmOptions.MMRConfig.LocationWalletRoyal = GetLocationIdOfItem(Item.UpgradeRoyalWallet);

            _randomized.Settings.AsmOptions.MMRConfig.LocationBombBagSmall = GetLocationIdOfItem(Item.ItemBombBag);
            _randomized.Settings.AsmOptions.MMRConfig.LocationBombBagBig = GetLocationIdOfItem(Item.UpgradeBigBombBag);
            _randomized.Settings.AsmOptions.MMRConfig.LocationBombBagBiggest = GetLocationIdOfItem(Item.UpgradeBiggestBombBag);

            _randomized.Settings.AsmOptions.MMRConfig.LocationQuiverSmall = GetLocationIdOfItem(Item.ItemBow);
            _randomized.Settings.AsmOptions.MMRConfig.LocationQuiverLarge = GetLocationIdOfItem(Item.UpgradeBigQuiver);
            _randomized.Settings.AsmOptions.MMRConfig.LocationQuiverLargest = GetLocationIdOfItem(Item.UpgradeBiggestQuiver);

            if (_randomized.Settings.UpdateShopAppearance)
            {
                // update tingle shops
                foreach (var messageShopText in Enum.GetValues<MessageShopText>())
                {
                    var messageShop = messageShopText.GetAttribute<MessageShopAttribute>();
                    var item1 = _randomized.ItemList.First(io => io.NewLocation == messageShop.Items[0]);
                    var item2 = _randomized.ItemList.First(io => io.NewLocation == messageShop.Items[1]);
                    var messageId = (ushort)messageShopText;
                    var messageHeader = _messageTable.GetMessage(messageId).Header;
                    var cost1 = ReadWriteUtils.Arr_ReadU16(messageHeader, 5);
                    var cost2 = ReadWriteUtils.Arr_ReadU16(messageHeader, 7);

                    newMessages.Add(new MessageEntryBuilder()
                        .Id(messageId)
                        .Message(it =>
                        {
                            switch (messageShop.MessageShopStyle)
                            {
                                case MessageShopStyle.Tingle:
                                    it.StartGreenText()
                                    .ThreeChoices()
                                    .RuntimeItemName(item1.DisplayName(), item1.NewLocation.Value).Text(": ").Red($"{cost1} Rupees").NewLine()
                                    .RuntimeItemName(item2.DisplayName(), item2.NewLocation.Value).Text(": ").Red($"{cost2} Rupees").NewLine()
                                    .Text("No Thanks")
                                    .EndFinalTextBox();
                                    break;
                                case MessageShopStyle.MilkBar:
                                    it.Text("What'll it be?")
                                    .EndTextBox()
                                    .StartGreenText()
                                    .ThreeChoices()
                                    .RuntimeItemName(item1.DisplayName(), item1.NewLocation.Value).Text(": ").Pink($"{cost1} Rupees").NewLine()
                                    .RuntimeItemName(item2.DisplayName(), item2.NewLocation.Value).Text(": ").Pink($"{cost2} Rupees").NewLine()
                                    .Text("Nothing")
                                    .EndFinalTextBox();
                                    break;
                            }
                        })
                        .Build()
                    );
                }

                // update business scrub
                var businessScrubItem = _randomized.ItemList.First(io => io.NewLocation == Item.HeartPieceTerminaBusinessScrub);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1631)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3AD2)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Please! I'll sell you ")
                            .RuntimeArticle(businessScrubItem.DisplayItem, businessScrubItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(businessScrubItem.DisplayName(), businessScrubItem.NewLocation.Value);
                            })
                            .Text(" if you just keep this place a secret...")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1632)
                    .Message(it =>
                    {
                        it.Pink("150 Rupees").Text(" for").RuntimePronounOrAmount(businessScrubItem.DisplayItem, businessScrubItem.NewLocation.Value).Text("!").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("I'll buy ").RuntimePronoun(businessScrubItem.DisplayItem, businessScrubItem.NewLocation.Value).NewLine()
                        .Text("No thanks")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1634)
                    .Message(it =>
                    {
                        it.Text("What about for ").Pink("100 Rupees").Text("?").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("I'll buy ").RuntimePronoun(businessScrubItem.DisplayItem, businessScrubItem.NewLocation.Value).NewLine()
                        .Text("No thanks")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // update biggest bomb bag purchase
                var biggestBombBagItem = _randomized.ItemList.First(io => io.NewLocation == Item.UpgradeBiggestBombBag);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x15F5)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("I sell ")
                            .RuntimeArticle(biggestBombBagItem.DisplayItem, biggestBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(biggestBombBagItem.AlternateName(), biggestBombBagItem.NewLocation.Value);
                            })
                            .Text(", but I'm focusing my marketing efforts on ").Red("Gorons").Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("What I'd really like to do is go back home and do business where I'm surrounded by trees and grass.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x15FF)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x398C)
                        .Text("Right now, I've got a ").Red("special").NewLine()
                        .Text("offer just for you.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1600)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3881)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'll give you ")
                            .RuntimeArticle(biggestBombBagItem.DisplayItem, biggestBombBagItem.NewLocation.Value, "my ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(biggestBombBagItem.DisplayName(), biggestBombBagItem.NewLocation.Value);
                            })
                            .Text(", regularly priced at ")
                            .Pink("1000 Rupees")
                            .Text("...")
                            ;
                        })
                        .EndTextBox()
                        .Text("In return, you'll give me just").NewLine()
                        .Pink("200 Rupees").Text("!")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1606)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3881)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'll give you ")
                            .RuntimeArticle(biggestBombBagItem.DisplayItem, biggestBombBagItem.NewLocation.Value, "my ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(biggestBombBagItem.DisplayName(), biggestBombBagItem.NewLocation.Value);
                            })
                            .Text(", regularly priced at ")
                            .Pink("1000 Rupees")
                            .Text(", for just ")
                            .Pink("200 Rupees")
                            .Text("!")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // update swamp scrub purchase
                var magicBeanItem = _randomized.ItemList.First(io => io.NewLocation == Item.ShopItemBusinessScrubMagicBean);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x15E1)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x39A7)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'm selling ")
                            .RuntimeArticle(magicBeanItem.DisplayItem, magicBeanItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(magicBeanItem.DisplayName(), magicBeanItem.NewLocation.Value);
                            })
                            .Text(" to Deku Scrubs, but I'll really like to leave my hometown.")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("I'm hoping to find some success in a livelier place!")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x15E9)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3AD2)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Do you know what ")
                            .RuntimeArticle(magicBeanItem.DisplayItem, magicBeanItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(magicBeanItem.AlternateName(), magicBeanItem.NewLocation.Value);
                            })
                            .Text(" ")
                            .RuntimeVerb(magicBeanItem.DisplayItem, magicBeanItem.NewLocation.Value)
                            .Text(", sir?")
                            ;
                        })
                        .NewLine()
                        .Text("I'll sell you").RuntimePronounOrAmount(magicBeanItem.DisplayItem, magicBeanItem.NewLocation.Value).Text(" for ").Pink("10 Rupees").Text(".")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x15F3)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3AD2)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Do you know what ")
                            .RuntimeArticle(magicBeanItem.DisplayItem, magicBeanItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(magicBeanItem.AlternateName(), magicBeanItem.NewLocation.Value);
                            })
                            .Text(" ")
                            .RuntimeVerb(magicBeanItem.DisplayItem, magicBeanItem.NewLocation.Value)
                            .Text("?")
                            ;
                        })
                        .NewLine()
                        .Text("I'll sell you").RuntimePronounOrAmount(magicBeanItem.DisplayItem, magicBeanItem.NewLocation.Value).Text(" for ").Pink("10 Rupees").Text(".")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // update ocean scrub purchase
                var greenPotionItem = _randomized.ItemList.First(io => io.NewLocation == Item.ShopItemBusinessScrubGreenPotion);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1608)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x39A7)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'm selling ")
                            .RuntimeArticle(greenPotionItem.DisplayItem, greenPotionItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(greenPotionItem.AlternateName(), greenPotionItem.NewLocation.Value);
                            })
                            .Text(", but I'm focusing my marketing efforts on Zoras.")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("Actually, I'd like to do business someplace where it's cooler and the air is clean.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1612)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x398C)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'll sell you ")
                            .RuntimeArticle(greenPotionItem.DisplayItem, greenPotionItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(greenPotionItem.DisplayName(), greenPotionItem.NewLocation.Value);
                            })
                            .Text(" for ")
                            .Pink("40 Rupees")
                            .Text("!")
                            ;
                        })
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                var coldifyRegex = new Regex("([A-Z])");
                var coldItemName = coldifyRegex.Replace(greenPotionItem.DisplayItem.Name(), "$1-$1");
                // TODO coldify replacement item name
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1617)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x398C)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'll s-sell you ")
                            .RuntimeArticle(greenPotionItem.DisplayItem, greenPotionItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(coldItemName, greenPotionItem.NewLocation.Value);
                            })
                            .Text(" for ")
                            .Pink("40 Rupees")
                            .Text(".")
                            ;
                        })
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1618)
                    .Message(it =>
                    {
                        it.Text("D-Do we have a deal?").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("Yes").NewLine()
                        .Text("No")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // update canyon scrub purchase
                var bluePotionItem = _randomized.ItemList.First(io => io.NewLocation == Item.ShopItemBusinessScrubBluePotion);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x161C)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x39A7)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'm here to sell ")
                            .RuntimeArticle(bluePotionItem.DisplayItem, bluePotionItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bluePotionItem.AlternateName(), bluePotionItem.NewLocation.Value);
                            })
                            .Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("Actually, I want to do business in the sea breeze while listening to the sound of the waves.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1626)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3AD2)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Don't you need ")
                            .RuntimeArticle(bluePotionItem.DisplayItem, bluePotionItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bluePotionItem.AlternateName(), bluePotionItem.NewLocation.Value);
                            })
                            .Text("? I'll sell you")
                            .RuntimePronounOrAmount(bluePotionItem.DisplayItem, bluePotionItem.NewLocation.Value)
                            .Text(" for ")
                            .Pink("100 Rupees")
                            .Text(".")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x162D)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x398C)
                        .RuntimeWrap(() =>
                        {
                            it.Text("I'll sell you ")
                            .RuntimeArticle(bluePotionItem.DisplayItem, bluePotionItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.Text(bluePotionItem.DisplayName());
                            })
                            .Text(" for ")
                            .Pink("100 Rupees")
                            .Text(".")
                            ;
                        })
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x15EA)
                    .Message(it =>
                    {
                        it.Text("Do we have a deal?").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("Yes").NewLine()
                        .Text("No")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // update gorman bros milk purchase
                var gormanBrosMilkItem = _randomized.ItemList.First(io => io.NewLocation == Item.ShopItemGormanBrosMilk);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x3463)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Won'tcha buy ")
                            .RuntimeArticle(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(gormanBrosMilkItem.AlternateName(), gormanBrosMilkItem.NewLocation.Value);
                            })
                            .Text("?")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x3466)
                    .Message(it =>
                    {
                        it.Pink("50 Rupees").Text(" will do ya for").RuntimePronounOrAmount(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value).Text(".").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("I'll buy ").RuntimePronoun(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value).NewLine()
                        .Text("No thanks")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x346B)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Buyin' ")
                            .RuntimeArticle(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(gormanBrosMilkItem.AlternateName(), gormanBrosMilkItem.NewLocation.Value);
                            })
                            .Text("?")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x348F)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Seems like we're the only ones who have ")
                            .RuntimeArticle(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(gormanBrosMilkItem.AlternateName(), gormanBrosMilkItem.NewLocation.Value);
                            })
                            .Text(". Hyuh, hyuh. If you like, I'll sell you")
                            .RuntimePronounOrAmount(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value)
                            .Text(".")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x3490)
                    .Message(it =>
                    {
                        it.Pink("50 Rupees").Text(" will do you for").RuntimePronounOrAmount(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value).Text("!").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("I'll buy ").RuntimePronoun(gormanBrosMilkItem.DisplayItem, gormanBrosMilkItem.NewLocation.Value).NewLine()
                        .Text("No thanks")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // update lottery message
                var lotteryItem = _randomized.ItemList.First(io => io.NewLocation == Item.MundaneItemLotteryPurpleRupee);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2B5C)
                    .Message(it =>
                    {
                        it.CompileTimeWrap((wrapped) =>
                        {
                            wrapped.Text("Would you like the chance to buy your dreams for ").Pink("10 Rupees").Text("?");
                        })
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Pick any three numbers, and if those are picked, you'll win ")
                            .RuntimeArticle(lotteryItem.DisplayItem, lotteryItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(lotteryItem.DisplayName(), lotteryItem.NewLocation.Value);
                            })
                            .Text(". It's only for the ")
                            .Red("first")
                            .Text(" person!")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2B66)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x4853)
                        .Text("Congratulations!")
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("You win a prize of ")
                            .RuntimeArticle(lotteryItem.DisplayItem, lotteryItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(lotteryItem.DisplayName(), lotteryItem.NewLocation.Value);
                            })
                            .Text("!")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // Update Witch Blue Potion message
                var witchBluePotionItem = _randomized.ItemList.First(io => io.NewLocation == Item.ShopItemWitchBluePotion);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x880)
                    .Message(it =>
                    {
                        it.Red(() =>
                        {
                            it.RuntimeItemName(witchBluePotionItem.DisplayName(), witchBluePotionItem.NewLocation.Value).Text(": 60 Rupees").NewLine();
                        })
                        .Text("Actually, I can't get the").NewLine()
                        .Text("ingredients for this, so I'm sold").NewLine()
                        .Text("out. Sorry.")
                        .DisableTextBoxClose()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x881)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("What's that? You want ")
                            .RuntimeArticle(witchBluePotionItem.DisplayItem, witchBluePotionItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(witchBluePotionItem.DisplayName(), witchBluePotionItem.NewLocation.Value);
                            })
                            .Text(", do you?")
                            ;
                        })
                        .EndTextBox()
                        .Text("Well, you gave me a mushroom, so").NewLine()
                        .Text("I'll give you").RuntimePronounOrAmount(witchBluePotionItem.DisplayItem, witchBluePotionItem.NewLocation.Value).Text(" for free.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                // Update Keg Challenge
                var kegChallengeItem = _randomized.ItemList.First(io => io.NewLocation == Item.ItemPowderKeg);
                if (kegChallengeItem.Item != Item.ItemPowderKeg)
                {
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC80)
                        .Message(it =>
                        {
                            it.PlaySoundEffect(0x38BB)
                            .RuntimeWrap(() =>
                            {
                                it.Text("I'm the Goron who sells ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(kegChallengeItem.AlternateName(), kegChallengeItem.NewLocation.Value);
                                })
                                .Text(", the most famous product of the Gorons.")
                                ;
                            })
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC81)
                        .Message(it =>
                        {
                            it.PlaySoundEffect(0x38FC)
                            .RuntimeWrap(() =>
                            {
                                it.Text("Want ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(kegChallengeItem.DisplayName(), kegChallengeItem.NewLocation.Value);
                                })
                                .Text("? Be careful, ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(kegChallengeItem.AlternateName(), kegChallengeItem.NewLocation.Value);
                                })
                                .Text(" ")
                                .RuntimeVerb(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Text(" very dangerous...")
                                ;
                            })
                            .EndTextBox()
                            .CompileTimeWrap("Until I have tested you to see if you are responsible, I can't sell to you.")
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC83)
                        .Message(it =>
                        {
                            it.PlaySoundEffect(0x38FC)
                            .Text("If you can ").Red("destroy").Text(" the boulder").NewLine()
                            .Text("that blocks the entrance to the").NewLine()
                            .Red("Goron Racetrack").Text(" near here...")
                            .EndTextBox()
                            .Text("using the ").Red("Powder Keg ").Text("I'm about").NewLine()
                            .Text("to give you, then I'll sell to you.")
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC86)
                        .Message(it =>
                        {
                            it.PlaySoundEffect(0x3A04)
                            .RuntimeWrap(() =>
                            {
                                it.Text("It looks like you managed to succeed! Knowing your skills, I feel fine letting you handle ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .RuntimeItemName(kegChallengeItem.AlternateName(), kegChallengeItem.NewLocation.Value)
                                .Text(" on your own.")
                                ;
                            })
                            .EndTextBox()
                            .Text("It was bad of me to put you").NewLine()
                            .Text("through such a dangerous test. I").NewLine()
                            .Text("want you to take this as my").NewLine()
                            .Text("apology.")
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC88)
                        .Message(it =>
                        {
                            it.PlaySoundEffect(0x38FC)
                            .Text("You failed?").NewLine()
                            .Text("In that case, I can't sell").NewLine()
                            .Text("to you.")
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC8C)
                        .Message(it =>
                        {
                            it.RuntimeWrap(() =>
                            {
                                it.Text("Will you a buy ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(kegChallengeItem.DisplayName(), kegChallengeItem.NewLocation.Value);
                                })
                                .Text(" for ").Pink("100 Rupees").Text("?")
                                ;
                            })
                            .NewLine()
                            .StartGreenText()
                            .TwoChoices()
                            .Text("I'll buy ").RuntimePronoun(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value).NewLine()
                            .Text("No thanks")
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC8E)
                        .Message(it =>
                        {
                            it.RuntimeWrap(() =>
                            {
                                it.Text("I'm the Goron who sells ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(kegChallengeItem.AlternateName(), kegChallengeItem.NewLocation.Value);
                                })
                                .Text(", the most famous product of the Gorons.")
                                ;
                            })
                            .EndTextBox()
                            .RuntimeWrap(() =>
                            {
                                it.Text("But the rules say I can't sell ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(kegChallengeItem.AlternateName(), kegChallengeItem.NewLocation.Value);
                                })
                                .Text(" to anyone who isn't a").Red(" Goron").Text(". ").PlaySoundEffect(0x391C).Text("Sorry.")
                                ;
                            })
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0xC8F)
                        .Message(it =>
                        {
                            it.RuntimeWrap(() =>
                            {
                                it.Text("The rules say I can't sell ")
                                .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(kegChallengeItem.AlternateName(), kegChallengeItem.NewLocation.Value);
                                })
                                .Text(" to anyone who isn't a").Red(" Goron").Text(". ").PlaySoundEffect(0x391C).Text("Sorry.")
                                ;
                            })
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                }
            }

            // Update messages to match updated world models.
            if (_randomized.Settings.UpdateWorldModels)
            {
                // Update Moon's Tear messages.
                var moonsTearItem = _randomized.ItemList.First(io => io.NewLocation == Item.TradeItemMoonTear);
                if (moonsTearItem.Item != Item.TradeItemMoonTear)
                {
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0x5E3)
                        .Message(it =>
                        {
                            it.Text("That is one of the lunar objects").NewLine()
                            .Text("that has been blazing from the").NewLine()
                            .Text("surface of the moon lately.")
                            .EndTextBox()
                            .CompileTimeWrap((wrapped) =>
                            {
                                wrapped.Text("They fall from what looks to be the moon's eye, I call ")
                                .Text(MessageUtils.GetPronoun(moonsTearItem.DisplayItem))
                                .Text(" ")
                                .Text(MessageUtils.GetArticle(moonsTearItem.DisplayItem))
                                .Red(moonsTearItem.DisplayName())
                                .Text(".")
                                ;
                            })
                            .EndTextBox()
                            .Text("They are rare, valued by many").NewLine()
                            .Text("in town.")
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0x5ED)
                        .Message(it =>
                        {
                            it.Text($"That ill-mannered troublemaker").NewLine()
                            .Text("from the other day said he'd").NewLine()
                            .Text("break my instruments...")
                            .EndTextBox()
                            .CompileTimeWrap((wrapped) =>
                            {
                                wrapped.Text("He said he's steal my ")
                                .Red(moonsTearItem.DisplayName())
                                .Text("... There was no stopping him.")
                                ;
                            })
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0x5F2)
                        .Message(it =>
                        {
                            it.Text($"Well, did you find that").NewLine()
                            .Red("troublemaker").Text("? And that loud").NewLine()
                            .Text("noise...What was that?")
                            .EndTextBox()
                            .CompileTimeWrap((wrapped) =>
                            {
                                wrapped.Text("Perhaps another ")
                                .Red(moonsTearItem.DisplayName())
                                .Text(" has falled nearby...Go through that door and take a look outside.")
                                ;
                            })
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                }

                // Update Seahorse messages.
                var seahorseItem = _randomized.ItemList.First(io => io.NewLocation == Item.MundaneItemSeahorse);
                if (seahorseItem.Item != Item.MundaneItemSeahorse)
                {
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0x106F)
                        .Message(it =>
                        {
                            it.PlaySoundEffect(0x694C)
                            .Text("Are you interested in that?")
                            .EndTextBox()
                            .RuntimeWrap(() =>
                            {
                                it.Text("It's rare, isn't it? It's called ")
                                .RuntimeArticle(seahorseItem.DisplayItem, seahorseItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(seahorseItem.DisplayName(), seahorseItem.NewLocation.Value);
                                })
                                .Text(".")
                                ;
                            })
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                    newMessages.Add(new MessageEntryBuilder()
                        .Id(0x1074)
                        .Message(it =>
                        {
                            it.RuntimeWrap(() =>
                            {
                                it.Text("If you want that ")
                                .Red(() =>
                                {
                                    it.RuntimeItemName(seahorseItem.DisplayName(), seahorseItem.NewLocation.Value);
                                })
                                .Text(", bring me a ")
                                .Red("pictograph")
                                .Text(" of a ")
                                .Red("female pirate")
                                .Text(".")
                                ;
                            })
                            .DisableTextSkip2()
                            .EndFinalTextBox();
                        })
                        .Build()
                    );
                }
            }

            // Remove "...Suddenly, memories of Princess Zelda come rushing back to you..."
            if (_randomized.ItemList[Item.ItemOcarina].NewLocation != Item.ItemOcarina || _randomized.ItemList[Item.SongTime].NewLocation != Item.SongTime)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x4C)
                    .Message(it =>
                    {
                        it.QuickText(() =>
                        {
                            it.Text("You got the ").Red("Ocarina of Time").NewLine()
                            .Text("back!");
                        })
                        .NewLine()
                        .PauseText(10)
                        .Red("Princess Zelda").Text(" gave you this").NewLine()
                        .Text("precious instrument.")
                        .EndTextBox()
                        .Text("Set it to ").Yellow("\u00B2").Text(" and use ").Yellow("\u00B0 ").Text("and the").NewLine()
                        .Text("four ").Yellow("\u00B2").Text(" Buttons to play it. Press").NewLine()
                        .Text("\u00B1 to stop.")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            // Update Zora Jar message.
            var zoraJarItem = _randomized.ItemList.First(io => io.NewLocation == Item.CollectableZoraCapeJarGame1);
            if (zoraJarItem.Item != Item.CollectableZoraCapeJarGame1)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x126F)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Well, here's ")
                            .RuntimeArticle(zoraJarItem.DisplayItem, zoraJarItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(zoraJarItem.DisplayName(), zoraJarItem.NewLocation.Value);
                            })
                            .Text(".")
                            ;
                        })
                        .EndTextBox()
                        .Text("Except...").NewLine()
                        .Text("Jar replacement costs ").Pink("10 Rupees").Text(",").NewLine()
                        .Text("so I'll have to deduct that.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1270)
                    .Message(it =>
                    {
                        // TODO need to update this if ice traps become non-repeatable.
                        if (zoraJarItem.Item == Item.IceTrap)
                        {
                            it.QuickText(() =>
                            {
                                it.Text("You are a ").DarkBlue("FOOL").Text("!");
                            })
                            .EndConversation()
                            .EndFinalTextBox();
                        }
                        else
                        {
                            it.RuntimeWrap(() =>
                            {
                                it.Text("You get ")
                                .RuntimeArticle(zoraJarItem.DisplayItem, zoraJarItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(zoraJarItem.DisplayName(), zoraJarItem.NewLocation.Value);
                                })
                                .Text("!")
                                ;
                            })
                            .EndConversation()
                            .EndFinalTextBox();
                        }
                    })
                    .Build()
                );
            }

            // Update Dampe rupee message.
            var dampeRupeeItem = _randomized.ItemList.First(io => io.NewLocation == Item.CollectableIkanaGraveyardDay2Bats1);
            if (dampeRupeeItem.Item != Item.CollectableIkanaGraveyardDay2Bats1)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1413)
                    .Message(it =>
                    {
                        it.Text("Was it you who chased those bats").NewLine()
                        .Text("away?")
                        .EndTextBox()
                        .Text("That's a big help... ").NewLine()
                        .RuntimeWrap(() =>
                        {
                            it.Text("It isn't much, but here's ")
                            .RuntimeArticle(dampeRupeeItem.DisplayItem, dampeRupeeItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(dampeRupeeItem.DisplayName(), dampeRupeeItem.NewLocation.Value);
                            })
                            .Text(" for your trouble. Take it!")
                            ;
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }
            var messageAttribute = Item.CollectableIkanaGraveyardDay2Bats1.GetAttribute<ExclusiveItemMessageAttribute>();
            var entry = new MessageEntry(
                messageAttribute.Id,
                messageAttribute.Message);
            _extraMessages.Add(entry);

            // replace "Razor Sword is now blunt" message with get-item message for Kokiri Sword.
            newMessages.Add(new MessageEntryBuilder()
                .Id(0xF9)
                .Header(it =>
                {
                    it.Standard2();
                })
                .Message(it =>
                {
                    it.Text("You got the ").Red("Kokiri Sword").Text("!").NewLine()
                    .Text("This is a hidden treasure of").NewLine()
                    .Text("the Kokiri, but you can borrow it").NewLine()
                    .Text("for a while.")
                    .EndFinalTextBox();
                })
                .Build()
            );

            // replace Magic Power message
            newMessages.Add(new MessageEntryBuilder()
                .Id(0xC8)
                .Message(it =>
                {
                    it.QuickText(() =>
                    {
                        it.Text("You've been granted ")
                        .Green("Magic Power")
                        .Text("!")
                        ;
                    })
                    .NewLine()
                    .Text("Replenish it with ")
                    .Red("Magic Jars")
                    .NewLine()
                    .Text("and ")
                    .Red("Potions")
                    .Text(".")
                    .EndFinalTextBox()
                    ;
                })
                .Build()
            );

            if (_randomized.Settings.CustomItemList.Any(item => item.ItemCategory() == ItemCategory.SkulltulaTokens) || _randomized.ItemList.Any(io => io.ID >= 433 && io.IsRandomized))
            {
                ResourceUtils.ApplyHack(Resources.mods.fix_piece_of_heart_message);
            }

            if (_randomized.Settings.CustomItemList.Any(item => item.ItemCategory() == ItemCategory.SkulltulaTokens))
            {
                ResourceUtils.ApplyHack(Resources.mods.fix_skulltula_tokens);

                MessageEntry oceanSkulltulaEntry = new MessageEntryBuilder()
                    .Id(0x51)
                    .Header(it => { it.FaintBlue().Icon(0x52); })
                    .Message(it =>
                    {
                        it.QuickText(() =>
                        {
                            it.Text("You got an ")
                            .LightBlue(() =>
                            {
                                it.Text("Ocean Gold Skulltula")
                                .NewLine()
                                .Text("Spirit");
                            })
                            .Text("!");
                        })
                        .PauseText(0x0010)
                        .Text(" This is your ")
                        .Red(() => { it.SkulltulaCount(); })
                        .Text(" one!")
                        .EndFinalTextBox();
                    })
                    .Build();
                newMessages.Add(oceanSkulltulaEntry);

                MessageEntry swampSkulltulaEntry = new MessageEntryBuilder()
                    .Id(0x52)
                    .Header(it => { it.FaintBlue().Icon(0x52); })
                    .Message(it =>
                    {
                        it.QuickText(() =>
                        {
                            it.Text("You got a ")
                            .Pink(() =>
                            {
                                it.Text("Swamp Gold Skulltula")
                                .NewLine()
                                .Text("Spirit");
                            })
                            .Text("!");
                        })
                        .PauseText(0x0010)
                        .Text(" This is your ")
                        .Red(() => { it.SkulltulaCount(); })
                        .Text(" one!")
                        .EndFinalTextBox();
                    })
                    .Build();

                newMessages.Add(swampSkulltulaEntry);
            }

            if (_randomized.Settings.CustomItemList.Any(item => item.ItemCategory() == ItemCategory.StrayFairies))
            {
                ResourceUtils.ApplyHack(Resources.mods.fix_fairies);
            }

            var dungeonItemMessageIds = new byte[] {
                0x3C, 0x3D, 0x3E, 0x3F, 0x74,
                0x40, 0x4D, 0x4E, 0x53, 0x75,
                0x54, 0x61, 0x64, 0x6E, 0x76,
                0x70, 0x71, 0x72, 0x73, 0x77,
            };

            var dungeonNames = new (char color, string dungeonName)[]
            {
                (TextCommands.ColorPink, "Woodfall Temple"),
                (TextCommands.ColorGreen, "Snowhead Temple"),
                (TextCommands.ColorLightBlue, "Great Bay Temple"),
                (TextCommands.ColorYellow, "Stone Tower Temple"),
            };

            var dungeonItemNames = new (string article, string itemName)[]
            {
                ("a ", "Small Key"),
                ("the ", "Boss Key"),
                ("the ", "Dungeon Map"),
                ("the ", "Compass"),
                ("a ", "Stray Fairy"),
            };

            var dungeonItemIcons = new byte[]
            {
                0x3C, 0x3D, 0x3E, 0x3F, 0xFE
            };

            for (var i = 0; i < dungeonItemMessageIds.Length; i++)
            {
                var itemTypeIndex = i % 5;
                var dungeonIndex = i / 5;
                var messageId = dungeonItemMessageIds[i];
                var icon = dungeonItemIcons[itemTypeIndex];
                var dungeonName = dungeonNames[dungeonIndex];

                newMessages.Add(new MessageEntryBuilder()
                    .Id(messageId)
                    .Header(it =>
                    {
                        it.FaintBlue().Icon(icon);
                    })
                    .Message(it =>
                    {
                        it.QuickText(() =>
                        {
                            it.Text("You found ")
                            .Text(dungeonItemNames[itemTypeIndex].article)
                            .Red(dungeonItemNames[itemTypeIndex].itemName)
                            .Text(" for")
                            .NewLine()
                            .TextColor(dungeonName.color, dungeonName.dungeonName)
                            .Text("!")
                            ;
                        });
                        if (itemTypeIndex == 4)
                        {
                            it.PauseText(0x10).NewLine()
                            .Text("This is your ").Red(() => { it.StrayFairyCount(); }).Text(" one!")
                            ;
                        }
                        it.EndFinalTextBox();
                    })
                    .Build()
                );
            }

            // TODO if costs randomized
            var messageCostRegex = new Regex("\\b[0-9]{1,3} Rupees?");
            for (var i = 0; i < MessageCost.MessageCosts.Length; i++)
            {
                var messageCost = MessageCost.MessageCosts[i];
                var cost = _randomized.MessageCosts[i];
                if (!cost.HasValue)
                {
                    continue;
                }
                foreach (var (messageId, costIndex) in messageCost.MessageIds)
                {
                    var oldMessage = _messageTable.GetMessage(messageId);
                    var newMessage = newMessages.FirstOrDefault(me => me.Id == messageId);
                    if (newMessage == null)
                    {
                        newMessage = new MessageEntry
                        {
                            Id = messageId,
                            Header = oldMessage.Header.ToArray(),
                            Message = oldMessage.Message,
                        };
                        newMessages.Add(newMessage);
                    }
                    if (newMessage.Header == null)
                    {
                        newMessage.Header = oldMessage.Header.ToArray();
                    }
                    var oldCost = ReadWriteUtils.Arr_ReadS16(newMessage.Header, 5 + (costIndex * 2));
                    if (oldCost >= 0)
                    {
                        ReadWriteUtils.Arr_WriteU16(newMessage.Header, 5 + (costIndex * 2), cost.Value);
                    }
                    var replacementIndex = 0;
                    newMessage.Message = messageCostRegex.Replace(newMessage.Message, match =>
                    {
                        return replacementIndex++ == costIndex ? $"{cost} Rupee{(cost != 1 && messageId != 1143 ? "s" : "")}" : match.Value;
                    });
                    if (messageId == 1143)
                    {
                        _randomized.Settings.AsmOptions.MiscConfig.Shorts.BankWithdrawFee = cost.Value;
                    }
                }
                foreach (var address in messageCost.PriceAddresses)
                {
                    ReadWriteUtils.WriteToROM(address, cost.Value);
                }
                foreach (var address in messageCost.SubtractPriceAddresses)
                {
                    var subtractCost = (ushort)(0 - cost);
                    ReadWriteUtils.WriteToROM(address, subtractCost);
                }
            }

            _messageTable.UpdateMessages(newMessages);

            ResourceUtils.ApplyHack(Resources.mods.fix_shop_curiosity_bigbombbag);
        }

        private void WriteGossipQuotes()
        {
            if (_randomized.Settings.LogicMode == LogicMode.Vanilla)
            {
                return;
            }

            if (_randomized.Settings.FreeHints)
            {
                WriteFreeHints();
            }

            if (_randomized.Settings.GossipHintStyle != GossipHintStyle.Default)
            {
                _messageTable.UpdateMessages(_randomized.GossipQuotes);
            }
        }

        private void WriteTitleScreen()
        {
            var titleScreen = Resources.mods.title_screen;

            int rot = _randomized.TitleLogoColor;
            Color l;
            float h;
            for (int i = 0; i < 144 * 64; i++)
            {
                int p = (i * 4) + 8;
                l = Color.FromArgb(titleScreen[p + 3], titleScreen[p], titleScreen[p + 1], titleScreen[p + 2]);
                h = l.GetHue();
                h += rot;
                h %= 360f;
                l = ColorUtils.FromAHSB(l.A, h, l.GetSaturation(), l.GetBrightness());
                titleScreen[p] = l.R;
                titleScreen[p + 1] = l.G;
                titleScreen[p + 2] = l.B;
                titleScreen[p + 3] = l.A;
            }
            l = Color.FromArgb(titleScreen[0x1FE72], titleScreen[0x1FE73], titleScreen[0x1FE76]);
            h = l.GetHue();
            h += rot;
            h %= 360f;
            l = ColorUtils.FromAHSB(255, h, l.GetSaturation(), l.GetBrightness());
            titleScreen[0x1FE72] = l.R;
            titleScreen[0x1FE73] = l.G;
            titleScreen[0x1FE76] = l.B;

            ResourceUtils.ApplyHack(titleScreen);
        }


        private void WriteFileSelect()
        {
            ResourceUtils.ApplyHack(Resources.mods.file_select);
            byte[] SkyboxDefault = new byte[] { 0x91, 0x78, 0x9B, 0x28, 0x00, 0x28 };
            List<int[]> Addrs = ResourceUtils.GetAddresses(Resources.addresses.skybox_init);
            for (int i = 0; i < 2; i++)
            {
                Color c = Color.FromArgb(SkyboxDefault[i * 3], SkyboxDefault[i * 3 + 1], SkyboxDefault[i * 3 + 2]);
                float h = c.GetHue();
                h += _randomized.FileSelectSkybox;
                h %= 360f;
                c = ColorUtils.FromAHSB(c.A, h, c.GetSaturation(), c.GetBrightness());
                SkyboxDefault[i * 3] = c.R;
                SkyboxDefault[i * 3 + 1] = c.G;
                SkyboxDefault[i * 3 + 2] = c.B;
            }

            for (int i = 0; i < 3; i++)
            {
                ReadWriteUtils.WriteROMAddr(Addrs[i], new byte[] { SkyboxDefault[i * 2], SkyboxDefault[i * 2 + 1] });
            }

            byte[] FSDefault = new byte[] { 0x64, 0x96, 0xFF, 0x96, 0xFF, 0xFF, 0x64, 0xFF, 0xFF };
            Addrs = ResourceUtils.GetAddresses(Resources.addresses.fs_colour);
            for (int i = 0; i < 3; i++)
            {
                Color c = Color.FromArgb(FSDefault[i * 3], FSDefault[i * 3 + 1], FSDefault[i * 3 + 2]);
                float h = c.GetHue();
                h += _randomized.FileSelectColor;
                h %= 360f;
                c = ColorUtils.FromAHSB(c.A, h, c.GetSaturation(), c.GetBrightness());
                FSDefault[i * 3] = c.R;
                FSDefault[i * 3 + 1] = c.G;
                FSDefault[i * 3 + 2] = c.B;
            }
            for (int i = 0; i < 9; i++)
            {
                if (i < 6)
                {
                    ReadWriteUtils.WriteROMAddr(Addrs[i], new byte[] { 0x00, FSDefault[i] });
                }
                else
                {
                    ReadWriteUtils.WriteROMAddr(Addrs[i], new byte[] { FSDefault[i] });
                }
            }
        }

        private void WriteCrashDebuggerShow()
        {
            /// in vanilla, if you hit the crash screen you have to know the secret button combo to see debug info
            /// here we bypass this to allow players to see the debug info and post it for us to help fix

            // we need to set 0x80082C1C to 0x1000000C which bypasses
            //   the conditional branches detecting if you haven't hit the right buttons, leaving function
            var bootFile = RomData.MMFileList[1].Data;
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x2BBC, 0x1000000C);

            void SwapBytes(int start, int end, byte searchByte, byte replaceByte)
            {
                for (int i = start; i < end; ++i)
                {
                    if (bootFile[i] == searchByte)
                    {
                        bootFile[i]  = replaceByte;
                    }
                }
            }

            // the H after each hex value for registers (H for base 16 'hex' ?) is hard for me to read
            // starts RAM 80098648, on rom it starts on 0x19640, within boot file its 0x185E0
            SwapBytes(0x185E0, 0x18720, (byte) 'H', (byte) ' '); // general registers
            SwapBytes(0x18760, 0x188D0, (byte) 'H', (byte) ' '); // floating point registers

            // convert lower case hex values to upper case (eg 8000abcd to 8000ABCD)
            // they used c printf string substitution, so just change %x to %X (8 byte wide though, "%08x")
            SwapBytes(0x181E0, 0x18230, (byte) 'x', (byte) 'X'); // dma section
            SwapBytes(0x18470, 0x18B04, (byte) 'x', (byte) 'X'); // the rest of hex values for the whole debug crasher

            // show V-PC for overlays to help find vram address in actors, useful
            // yes I know it shows this page _later_, but a lot of users will only wait for the first one,
            // no reason to have stupid question marks instead
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x31D8, 0x00000000); // replace branch with nop
        }

        private void WriteStartupStrings()
        {
            if (_randomized.Settings.LogicMode == LogicMode.Vanilla)
            {
                //ResourceUtils.ApplyHack(ModsDir + "postman-testing");
                return;
            }
            RomUtils.SetStrings(Resources.mods.logo_text, $"v{Randomizer.AssemblyVersion}", string.Empty);
        }

        public void OutputHashIcons(IEnumerable<byte> iconFileIndices, string filename)
        {
            var iconFiles = RomUtils.GetFilesFromArchive(19);
            var numberOfHashIcons = iconFileIndices.Count();
            var margin = 8;
            using (var image = new Image<Argb32>(32 * numberOfHashIcons + margin * (numberOfHashIcons - 1), 32))
            {
                var i = 0;
                foreach (var iconFileIndex in iconFileIndices)
                {
                    using (var icon = Image.LoadPixelData<Rgba32>(iconFiles[iconFileIndex], 32, 32))
                    {
                        image.Mutate(o => o.DrawImage(icon, new Point(i * 32 + i * margin, 0), 1f));
                    }
                    i++;
                }
                image.Save(filename, new PngEncoder());
            }
        }

        private void WriteAsmPatch(AsmContext asm)
        {
            // Load the symbols and use them to apply the patch data
            var options = _randomized.Settings.AsmOptions;

            // Finalize Misc configuration.
            options.MiscConfig.FinalizeSettings(_randomized.Settings);

            asm.ApplyPatch(options);

            if (_extendedObjects != null)
            {
                // Add extended objects file and write addresses to table in ROM
                var extended = _extendedObjects;
                var fileIndex = RomUtils.AppendFile(extended.Bundle.GetFull());
                var file = RomData.MMFileList[fileIndex];
                var baseAddr = (uint)file.Addr;
                asm.Symbols.WriteExtendedObjects(extended.GetAddresses(baseAddr));
            }

            // Add extra messages to message table.
            if (_randomized.Settings.QuickTextEnabled)
            {
                var regex = new Regex("(?<!(?:\x1B|\x1C|\x1D|\x1E).?)(?:\x1F..|\x17|\x18)", RegexOptions.Singleline);
                foreach (var entry in _extraMessages)
                {
                    entry.Message = regex.Replace(entry.Message, "");
                }
            }
            asm.ExtraMessages.AddMessage(_extraMessages.ToArray());
            asm.WriteExtMessageTable();

            // Add item graphics to table and write to ROM.
            asm.MimicTable.Update(_graphicOverrides);
            asm.WriteMimicItemTable();
        }

        private void WriteAsmConfig(AsmContext asm, byte[] hash)
        {
            UpdateHudColorOverrides(hash);
            _cosmeticSettings.AsmOptions.FinalizeSettings(_cosmeticSettings);

            // Apply Asm configuration (after hash has been calculated)
            var options = _cosmeticSettings.AsmOptions;
            options.Hash = hash;
            asm.ApplyPostConfiguration(options, false);
        }

        private void WriteAsmConfigPostPatch(AsmContext asm, byte[] hash)
        {
            UpdateHudColorOverrides(hash);
            _cosmeticSettings.AsmOptions.FinalizeSettings(_cosmeticSettings);

            // Apply current configuration on top of existing Asm patch file
            var options = _cosmeticSettings.AsmOptions;
            options.Hash = hash;
            asm.ApplyPostConfiguration(options, true);
        }

        /// <summary>
        /// Update the HUD colors override options.
        /// </summary>
        /// <param name="hash">Hash which is used with <see cref="Random"/></param>
        private void UpdateHudColorOverrides(byte[] hash)
        {
            var config = _cosmeticSettings.AsmOptions.HudColorsConfig;
            var random = new Random(BitConverter.ToInt32(hash, 0));

            // Update override for heart colors
            if (_cosmeticSettings.HeartsSelection != null)
                config.HeartsOverride = ColorSelectionManager.Hearts.GetItems().FirstOrDefault(csi => csi.Name == _cosmeticSettings.HeartsSelection)?.GetColors(random);
            else
                config.HeartsOverride = null;

            // Update override for magic meter colors
            if (_cosmeticSettings.MagicSelection != null)
                config.MagicOverride = ColorSelectionManager.MagicMeter.GetItems().FirstOrDefault(csi => csi.Name == _cosmeticSettings.HeartsSelection)?.GetColors(random);
            else
                config.MagicOverride = null;

            // Get random values for hue shift.
            if (_cosmeticSettings.ShiftHueMiscUI)
            {
                config.HueShift = new Tuple<float, float, float>((float)(random.NextDouble() * 360.0), (float)(random.NextDouble() * 360.0), (float)(random.NextDouble() * 360.0));
            }
        }

        /// <summary>
        /// Build <see cref="ExtendedObjects"/> and write object indexes to Get-Item list entries.
        /// </summary>
        private void WriteExtendedObjects()
        {
            var addFairies = _randomized.Settings.CustomItemList.Any(item => item.ItemCategory() == ItemCategory.StrayFairies);
            var addSkulltulas = _randomized.Settings.CustomItemList.Any(item => item.ItemCategory() == ItemCategory.SkulltulaTokens);
            var extended = _extendedObjects = ExtendedObjects.Create(addFairies, addSkulltulas);

            foreach (var e in RomData.GetItemList.Values)
            {
                // Update gi-table for Skulltula Tokens.
                if (e.ItemGained == 0x6E && e.Object == 0x125 && extended.Indexes.Skulltula != null)
                {
                    var index = e.Message == 0x51 ? 1 : 0;
                    e.Object = (short)(extended.Indexes.Skulltula.Value + index);
                }

                // Update gi-table for Stray Fairies.
                if (e.ItemGained == 0xA8 && e.Object == 0x13A && extended.Indexes.Fairies != null)
                {
                    var index = e.Type >> 4;
                    e.Object = (short)(extended.Indexes.Fairies.Value + index);
                }

                // Update gi-table for Double Defense.
                if (e.ItemGained == 0xA7 && e.Object == 0x96 && extended.Indexes.DoubleDefense != null)
                {
                    e.Object = extended.Indexes.DoubleDefense.Value;
                }

                // Update gi-table for Notes.
                if (((e.ItemGained >= 0x66 && e.ItemGained <= 0x6C) || e.ItemGained == 0x62) && e.Object == 0x8F && extended.Indexes.MusicNotes != null)
                {
                    e.Object = extended.Indexes.MusicNotes.Value;
                }

                // Update gi-table for Magic Power
                if (e.ItemGained == 0xA5 && e.Object == 0xA4 && extended.Indexes.MagicPower != null)
                {
                    e.Object = extended.Indexes.MagicPower.Value;
                }

                // Update gi-table for Extra Rupees
                if (e.ItemGained == 0xB1 && e.Object == 0x13F && extended.Indexes.Rupees != null)
                {
                    e.Object = extended.Indexes.Rupees.Value;
                }
            }
        }

        /// <summary>
        /// Write data related to ice traps to ROM.
        /// </summary>
        public void WriteIceTraps()
        {
            // Add mimic graphic to graphic overrides table.
            foreach (var item in _randomized.IceTraps)
            {
                var newLocation = item.NewLocation.Value;
                if (newLocation.IsVisible())
                {
                    var giIndex = item.NewLocation.Value.GetItemIndex().Value;
                    var graphic = item.Mimic.ResolveGraphic();
                    _graphicOverrides.Add(giIndex, graphic);
                }
            }

            // Add "You are a FOOL!" message to extra messages table.
            var entry = new MessageEntry(
                Item.IceTrap.ExclusiveItemEntry().Message,
                Item.IceTrap.ExclusiveItemMessage());
            _extraMessages.Add(entry);
        }

        public void MakeROM(OutputSettings outputSettings, IProgressReporter progressReporter)
        {
            using (BinaryReader OldROM = new BinaryReader(File.OpenRead(outputSettings.InputROMFilename)))
            {
                RomUtils.ReadFileTable(OldROM);
                _messageTable = MessageTable.ReadDefault();
            }

            var originalMMFileList = RomData.MMFileList.Select(file => file.Clone()).ToList();

            byte[] hash;
            AsmContext asm;
            if (!string.IsNullOrWhiteSpace(outputSettings.InputPatchFilename))
            {
                progressReporter.ReportProgress(50, "Applying patch...");
                hash = Patch.Patcher.ApplyPatch(outputSettings.InputPatchFilename);

                // Parse Symbols data from the ROM (specific MMFile)
                asm = AsmContext.LoadFromROM();

                // Apply Asm configuration post-patch
                WriteAsmConfigPostPatch(asm, hash);
            }
            else
            {
                progressReporter.ReportProgress(55, "Writing player model...");
                WritePlayerModel();

                if (_randomized.Settings.LogicMode != LogicMode.Vanilla)
                {
                    progressReporter.ReportProgress(60, "Applying hacks...");
                    ResourceUtils.ApplyHack(Resources.mods.title_screen);
                    WriteTitleScreen();
                    ResourceUtils.ApplyHack(Resources.mods.misc_changes);
                    WriteMiscText();
                    ResourceUtils.ApplyHack(Resources.mods.cm_cs);
                    ResourceUtils.ApplyHack(Resources.mods.fix_song_of_healing);
                    WriteFileSelect();
                }
                ResourceUtils.ApplyHack(Resources.mods.init_file);
                ResourceUtils.ApplyHack(Resources.mods.fix_deku_drowning);
                ResourceUtils.ApplyHack(Resources.mods.fix_collectable_flags);
                ResourceUtils.ApplyHack(Resources.mods.fix_great_bay_clear_mikau);

                // TODO: Move this to a helper function?
                if (_randomized.Settings.EnablePictoboxSubject)
                {
                    WritePictographPromptText(_messageTable);

                    // NOP call to update pictobox flags after message prompt.
                    ReadWriteUtils.WriteCodeNOP(0x801127D0);
                }

                if (_randomized.Settings.ShortenCutsceneSettings.General.HasFlag(ShortenCutsceneGeneral.FasterBankText))
                {
                    WriteBankPromptText(_messageTable);
                }

                WriteArcheryDoubleRewardText(_messageTable);
                WriteBankPostRewardText(_messageTable);
                WriteRoyalWalletText(_messageTable);

                progressReporter.ReportProgress(61, "Writing quick text...");
                WriteQuickText();

                progressReporter.ReportProgress(62, "Writing cutscenes...");
                WriteCutscenes();

                progressReporter.ReportProgress(63, "Writing dungeons...");
                WriteDungeons();

                progressReporter.ReportProgress(64, "Writing gimmicks...");
                WriteGimmicks();

                progressReporter.ReportProgress(65, "Writing speedups...");
                WriteSpeedUps();

                progressReporter.ReportProgress(66, "Writing enemies...");
                WriteEnemies();

                progressReporter.ReportProgress(67, "Writing items...");
                WriteItems();
                WriteMiscHacks();

                progressReporter.ReportProgress(68, "Writing messages...");
                WriteGossipQuotes();

                MessageTable.WriteDefault(_messageTable, _randomized.Settings.QuickTextEnabled);

                progressReporter.ReportProgress(69, "Writing startup...");
                WriteStartupStrings();

                // Overwrite existing items with ice traps.
                if (_randomized.Settings.LogicMode != LogicMode.Vanilla && _randomized.Settings.IceTraps != IceTraps.None)
                {
                    progressReporter.ReportProgress(70, "Writing ice traps...");
                    WriteIceTraps();
                }

                // Load Asm data from internal resource files and apply
                asm = AsmContext.LoadInternal();
                progressReporter.ReportProgress(71, "Writing ASM patch...");
                WriteAsmPatch(asm);

                WriteNutsAndSticks();
                
                progressReporter.ReportProgress(72, outputSettings.GeneratePatch ? "Generating patch..." : "Computing hash...");
                hash = outputSettings.GeneratePatch switch
                {
                    // Write patch file to path and return hash.
                    true => Patch.Patcher.CreatePatch(Path.ChangeExtension(outputSettings.OutputROMFilename, "mmr"), originalMMFileList),
                    // Only return hash.
                    false => Patch.Patcher.CreatePatch(originalMMFileList),
                };

                // Write subset of Asm config post-patch
                WriteAsmConfig(asm, hash);

                if (_randomized.Settings.DrawHash || outputSettings.GeneratePatch)
                {
                    var iconStripIcons = asm.Symbols.ReadHashIconsTable();
                    OutputHashIcons(ImageUtils.GetIconIndices(hash).Select(index => iconStripIcons[index]), Path.ChangeExtension(outputSettings.OutputROMFilename, "png"));
                }
            }
            WriteMiscellaneousChanges();

            progressReporter.ReportProgress(72, "Writing cosmetics...");
            WriteTatlColour(new Random(BitConverter.ToInt32(hash, 0)));
            WriteTunicColor();
            WriteInstruments(new Random(BitConverter.ToInt32(hash, 0)));

            progressReporter.ReportProgress(73, "Writing music...");
            WriteAudioSeq(new Random(BitConverter.ToInt32(hash, 0)), outputSettings);
            WriteMuteMusic();
            WriteEnemyCombatMusicMute();

            progressReporter.ReportProgress(74, "Writing sound effects...");
            WriteSoundEffects(new Random(BitConverter.ToInt32(hash, 0)));
            WriteLowHealthSound(new Random(BitConverter.ToInt32(hash, 0)));

            if (outputSettings.GenerateROM || outputSettings.OutputVC)
            {
                progressReporter.ReportProgress(75, "Building ROM...");

                byte[] ROM = RomUtils.BuildROM();

                if (outputSettings.GenerateROM)
                {
                    if (ROM.Length > 0x4000000) // over 64mb
                    {
                        throw new ROMOverflowException("64 MB", "hardware (Everdrive)");
                    }
                    progressReporter.ReportProgress(85, "Writing ROM...");
                    RomUtils.WriteROM(outputSettings.OutputROMFilename, ROM);
                }

                if (outputSettings.OutputVC)
                {
                    if (ROM.Length > 0x2000000) // over 32mb
                    {
                        throw new ROMOverflowException("32 MB", "WiiVC");
                    }
                    progressReporter.ReportProgress(90, "Writing VC...");
                    var fileName = Path.ChangeExtension(outputSettings.OutputROMFilename, "wad");
                    if (!Path.IsPathRooted(fileName))
                    {
                        fileName = Path.Combine(Values.MainDirectory, fileName);
                    }
                    VCInjectionUtils.BuildVC(ROM, _cosmeticSettings.AsmOptions.DPadConfig, Values.VCDirectory, fileName);
                }
            }
            progressReporter.ReportProgress(100, "Done!");

        }

    }

}
