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
using MMR.Common.Utils;

namespace MMR.Randomizer
{
    public class Builder
    {
        private RandomizedResult _randomized;
        private CosmeticSettings _cosmeticSettings;
        private ExtendedObjects _extendedObjects;
        private List<MessageEntry> _extraMessages;
        private Dictionary<int, ItemGraphic> _graphicOverrides;

        public Builder(RandomizedResult randomized, CosmeticSettings cosmeticSettings)
        {
            _randomized = randomized;
            _cosmeticSettings = cosmeticSettings;
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
            // if we don't randomize unassigned, then we only get upper alphabetical, same every seed
            List<SequenceInfo> unassigned = RomData.SequenceList.FindAll(u => u.Replaces == -1);
            unassigned = unassigned.OrderBy(x => random.Next()).ToList();                           // random ordered songs
            RomData.TargetSequences = RomData.TargetSequences.OrderBy(x => random.Next()).ToList(); // random ordered slots
            WriteOutput(" Randomizing " + RomData.TargetSequences.Count + " song slots, with " + unassigned.Count + " available songs:");

            SequenceUtils.ResetBudget();

            // songtest filename token allows music makers and users to force a song into a MMR seed for recording/testing
            SequenceUtils.CheckSongTest(unassigned, log);

            // music plando, user has selected they want an easier time specifying where and what songs are placed in specific spots
            var plandoPlacements = PlandoUtils.GetRandomizedSongPlacements(random, log);
            foreach ((var song, var slot) in plandoPlacements)
            {
                SequenceUtils.AssignSequenceSlot(slot, song, unassigned, "PLANDO", log);
                RomData.TargetSequences.Remove(slot);
            }

            // songforce filename token places music in a higher priority, check for songs with songforce here
            SequenceUtils.CheckSongForce(unassigned, log, random);

            foreach (var targetSlot in RomData.TargetSequences)
            {
                // scan all songs for a replacement that fits in this slot
                bool foundValidReplacement = SequenceUtils.SearchForValidSongReplacement(_cosmeticSettings, unassigned, targetSlot, random, log);

                if (foundValidReplacement == false) // no available songs fit in this slot category
                {
                    WriteOutput($"No song fits in [{targetSlot.Name}] slot, with categories: " + string.Join(", ", targetSlot.Categories.Select(x => "0x" + x.ToString("X2"))));
                    // loosen song restrictions and re-attempt
                    SequenceUtils.TryBackupSongPlacement(targetSlot, log, unassigned, settings);
                }
            }

            SequenceUtils.CheckBGMCombatMusicBudget(_cosmeticSettings, unassigned, random, log);

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
            SequenceUtils.ResetFreeBankIndex();
            if (_cosmeticSettings.Music == Music.Random)
            {
                SequenceUtils.PointerizeSequenceSlots();
                BGMShuffle(random, _settings);
                ResourceUtils.ApplyHack(Resources.mods.remove_morning_music);
            }

            ResourceUtils.ApplyHack(Resources.mods.fix_music);
            SequenceUtils.RebuildAudioSeq(RomData.SequenceList,
                _cosmeticSettings.AsmOptions.MusicConfig.SequenceMaskFileIndex,
                _cosmeticSettings.AsmOptions.MusicConfig.SequenceNamesFileIndex);
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
            if (!_cosmeticSettings.DisableCombatMusic)
            {
                return;
            }

            ReadWriteUtils.WriteToROM(0xCA7F00 + 0x16818, 0x1000);
        }

        private void WriteRemoveMinorMusic()
        {
            if (_cosmeticSettings.RemoveMinorMusic)
            {
                ResourceUtils.ApplyHack(Resources.mods.remove_minor_music);
            }
        }

        private void WriteDisableFanfares()
        {
            if (_cosmeticSettings.DisableFanfares)
            {
                ResourceUtils.ApplyHack(Resources.mods.remove_fanfares);

                // if Skulltulas do not reset with song of time (therefore not randomized)
                if (ReadWriteUtils.ReadU32(0xBDA9DC) != 0)
                {
                    ReadWriteUtils.WriteU32ToROM(0xDFF8B8, 0x0C067C32); // JAL    PlaySfx
                    ReadWriteUtils.WriteU32ToROM(0xDFF8BC, 0x24044824); // ADDIU  A0, R0, 0x4824 // NA_SE_SY_GET_ITEM

                    ReadWriteUtils.WriteU32ToROM(0xDFF8D4, 0x0C067C32); // JAL    PlaySfx
                    ReadWriteUtils.WriteU32ToROM(0xDFF8D8, 0x24044824); // ADDIU  A0, R0, 0x4824 // NA_SE_SY_GET_ITEM
                }

                // if stray fairies still play the fanfare when you collect them all
                if (ReadWriteUtils.ReadU32(0xF32F24) != 0)
                {
                    ReadWriteUtils.WriteU32ToROM(0xF32F24, 0x0C067C32); // JAL    PlaySfx
                    ReadWriteUtils.WriteU32ToROM(0xF32F28, 0x24044824); // ADDIU  A0, R0, 0x4824 // NA_SE_SY_GET_ITEM
                }
            }
        }

        #endregion

        private void WritePlayerModel()
        {
            // Apply mods for using environment colour for tunics.
            ResourceUtils.ApplyHack(Resources.models.envcolour_fdlink_mod);
            ResourceUtils.ApplyHack(Resources.models.envcolour_goronlink_mod);
            ResourceUtils.ApplyHack(Resources.models.envcolour_zoralink_mod);
            ResourceUtils.ApplyHack(Resources.models.gameplaykeep_fincolors);
            ResourceUtils.ApplyHack(Resources.models.envcolour_dekulink_mod);
            ResourceUtils.ApplyHack(Resources.models.gameplay_keep_swordenvs);
            if (_randomized.Settings.Character == Character.LinkMM)
            {
                ResourceUtils.ApplyHack(Resources.models.envcolour_humanlink_mod);
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

            switch (_cosmeticSettings.CameraStyle)
            {
                case CameraStyle.Responsive:
                    ReadWriteUtils.WriteCodeNOP(0x800DF44C);
                    ReadWriteUtils.WriteCodeNOP(0x800DF450);
                    break;
                case CameraStyle.Instant:
                    ResourceUtils.ApplyHack(Resources.mods.improved_camera);
                    break;
            }

            // Allow player to equip over masks handled by the DPad.
            if (_cosmeticSettings.DPad.State != DPadState.Disabled)
            {
                var formToDpad = new Dictionary<TransformationForm, DPadValue>
                {
                    //{ TransformationForm.FierceDeity, DPadValue.FierceDeityMask },
                    { TransformationForm.Goron, DPadValue.GoronMask },
                    { TransformationForm.Zora, DPadValue.ZoraMask },
                    { TransformationForm.Deku, DPadValue.DekuMask },
                    //{ TransformationForm.Human, DPadValue.HumanMask },
                };
                const int FileKaleidoScopeAddress = 0xC90550;
                const int sMaskPlayerFormItemsOffset = 0x1562C;
                const byte none = 0xFF;
                foreach (var (form, dpad) in formToDpad)
                {
                    if (_cosmeticSettings.DPad.Pad.Values.Contains(dpad))
                    {
                        ReadWriteUtils.WriteToROM(FileKaleidoScopeAddress + sMaskPlayerFormItemsOffset + form.Id(), none);
                    }
                }
            }

            WriteCrashDebuggerShow();

            // Dolphin/WiiVC audiothread shutdown workaround
            ReadWriteUtils.WriteU32ToROM(0xB3C000 + 0x0CD384, 0x00000000);

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

        private void WriteMoonChildDenialTextAndHack(MessageTable table)
        {
            table.UpdateMessages(new MessageEntryBuilder()
                .Id(0x21FD)
                .Header(it => it.FaintBlue().Y(1))
                .Message(it =>
                {
                    it.Text("Oh, it seems you're not worthy...").NewLine()
                    .Text("But I'll give you another chance...")
                    .EndTextBox()
                    .Text("Are you ready...to go back?").NewLine()
                    .StartGreenText()
                    .Text(" ").NewLine()
                    .TwoChoices()
                    .Text("Yes").NewLine()
                    .Text("No")
                    .EndFinalTextBox()
                    ;
                })
                .Build()
            );

            RomUtils.CheckCompressed(1501); // The Moon - Room 00
            var data = RomData.MMFileList[1501].Data.ToList();
            data.RemoveRange(0x194, 4); // Reduce end padding from actor list. 8 bytes remaining
            data.InsertRange(0x44, new byte[] { 0x01, 0xBE, 0x00, 0x00 }); // Add extra objects
            data[0x29] += 1; // Increase object count by 1. 1 object slot remaining before needing to increase available space.
            data[0x37] += 4; // Add 4 to the actor list address
            RomData.MMFileList[1501].Data = data.ToArray();

            ResourceUtils.ApplyHack(Resources.mods.fix_object_stk2_zbuffer);
        }

        private void WriteMiscText(MessageTable messageTable)
        {
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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
            messageTable.UpdateMessages(new MessageEntryBuilder()
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

        private void WriteCutscenes(MessageTable messageTable)
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

            if (_randomized.Settings.ShortenCutsceneSettings.General.HasFlag(ShortenCutsceneGeneral.FasterBankText) && _randomized.Settings.UpdateNPCText)
            {
                ResourceUtils.ApplyHack(Resources.mods.faster_bank_text_restore_intro);
            }

            if (_randomized.Settings.ShortenCutsceneSettings.General.HasFlag(ShortenCutsceneGeneral.AutomaticCredits))
            {
                for (ushort i = 0x1F5F; i <= 0x1F75; i++)
                {
                    var message = messageTable.GetMessage(i);
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
                                var newMessageId = (ushort) ((_extraMessages.Max(me => (ushort?)me.Id) ?? 0x9006) + 1);
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

            if (!_randomized.Settings.ShortenCutsceneSettings.General.HasFlag(ShortenCutsceneGeneral.ShortChestOpening) && _randomized.Settings.UpdateChests)
            {
                ResourceUtils.ApplyHack(Resources.mods.update_chest_cutscene);
                ReadWriteUtils.WriteU16ToROM(0xB3C000 + 0x12B2B2, 0xFFF6); // Replace Fairy Revive Cutscene with Large Chest Opening
                SceneUtils.InsertLargeChestCutscene();
            }
        }

        private void WriteDungeons()
        {
            if (_randomized.Settings.LogicMode == LogicMode.Vanilla)
            {
                return;
            }

            if (!_randomized.Settings.RandomizeDungeonEntrances && !_randomized.Settings.RandomizeBossRooms)
            {
                return;
            }

            SceneUtils.ReadSceneTable();
            SceneUtils.GetMaps();

            var entrances = new List<Item>();
            if (_randomized.Settings.RandomizeDungeonEntrances)
            {
                entrances.Add(Item.AreaWoodFallTempleAccess);
                entrances.Add(Item.AreaWoodFallTempleClear);
                entrances.Add(Item.AreaSnowheadTempleAccess);
                entrances.Add(Item.AreaSnowheadTempleClear);
                entrances.Add(Item.AreaGreatBayTempleAccess);
                entrances.Add(Item.AreaGreatBayTempleClear);
                entrances.Add(Item.AreaInvertedStoneTowerTempleAccess);
                entrances.Add(Item.AreaStoneTowerClear);
            }
            if (_randomized.Settings.RandomizeBossRooms)
            {
                entrances.Add(Item.AreaWoodFallTempleClear);
                entrances.Add(Item.AreaSnowheadTempleClear);
                entrances.Add(Item.AreaGreatBayTempleClear);
                entrances.Add(Item.AreaStoneTowerClear);
                entrances.Add(Item.AreaOdolwasLair);
                entrances.Add(Item.AreaGohtsLair);
                entrances.Add(Item.AreaGyorgsLair);
                entrances.Add(Item.AreaTwinmoldsLair);
            }

            foreach (var entrance in entrances.Distinct())
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

            if (_randomized.Settings.RandomizeBossRooms)
            {
                var bosses = new List<Item>
                {
                    Item.AreaOdolwasLair,
                    Item.AreaGohtsLair,
                    Item.AreaGyorgsLair,
                    Item.AreaTwinmoldsLair,
                };

                var bossDoorAddr = ResourceUtils.GetAddresses(Resources.addresses.d_boss_door);
                var bossWarpAddr = ResourceUtils.GetAddresses(Resources.addresses.d_boss_warp);
                var bossDoorValues = new List<byte[]>
                {
                    new byte[] { 0x00, 0x1F, 0x01 },
                    new byte[] { 0x00, 0x44, 0x02 },
                    new byte[] { 0x00, 0x5F, 0x03 },
                    new byte[] { 0x00, 0x36, 0x04 },
                };
                for (var i = 0; i < bosses.Count; i++)
                {
                    var boss = bosses[i];
                    var newBoss = _randomized.ItemList[boss].NewLocation ?? boss;
                    var addressIndex = bosses.IndexOf(newBoss);
                    ReadWriteUtils.WriteROMAddr(bossDoorAddr[addressIndex], bossDoorValues[i]);
                    ReadWriteUtils.WriteROMAddr(bossWarpAddr[addressIndex], new byte[] { (byte)(i + 2) });
                }

                var bossDoorTextureOffsets = new List<int> { 0x5BA0, 0x5C0, 0x4BA0, 0x3BA0 };

                var bossAtSTT = _randomized.ItemList.Find(io => io.NewLocation == Item.AreaTwinmoldsLair)?.Item ?? Item.AreaTwinmoldsLair;
                if (bossAtSTT != Item.AreaTwinmoldsLair)
                {
                    var indexToUse = bosses.IndexOf(bossAtSTT);

                    var bossDoorTexture = ReadWriteUtils.ReadBytes(0x012F8000 + bossDoorTextureOffsets[indexToUse], 0x1000);
                    var bossDoorTexturePixels = bossDoorTexture
                        .Chunk(2)
                        .Select(chunk => (ushort)((chunk[0] << 8) | chunk[1]))
                        .ToArray();
                    var paletteDict = bossDoorTexturePixels
                        .GroupBy(x => x)
                        .ToDictionary(x => x.Key, g => g.Count());
                    while (paletteDict.Keys.Count > 256)
                    {
                        var leastUsedColor = paletteDict.OrderBy(kvp => kvp.Value).First();
                        paletteDict.Remove(leastUsedColor.Key);
                        var nearestColor = ColorUtils.FindNearestColor(paletteDict.Keys.Select(c => ColorUtils.FromRGBA5551(c)).ToArray(), ColorUtils.FromRGBA5551(leastUsedColor.Key));
                        var toRGBA5551 = ColorUtils.ToRGBA5551(nearestColor);
                        paletteDict[toRGBA5551] += leastUsedColor.Value;
                        for (var i = 0; i < bossDoorTexturePixels.Length; i++)
                        {
                            if (bossDoorTexturePixels[i] == leastUsedColor.Key)
                            {
                                bossDoorTexturePixels[i] = toRGBA5551;
                            }
                        }
                    }
                    var palette = paletteDict.Keys.ToArray();
                    var ci8 = bossDoorTexturePixels.Select(pix => (byte)Array.IndexOf(palette, pix)).ToArray();

                    // STT Room 8
                    ReadWriteUtils.WriteToROM(0x0211D000 + 0x4428, ci8);
                    var f = RomUtils.GetFileIndexForWriting(0x0211D000);
                    ReadWriteUtils.Arr_Insert(new byte[] { 0x03, 0x00, 0x4C, 0x40 }, 0, 4, RomData.MMFileList[f].Data, 0x3D4);
                    var data = RomData.MMFileList[f].Data.ToList();
                    data.InsertRange(0x4C40, palette.SelectMany(s => new byte[] { (byte)(s >> 8), (byte)(s & 0xFF) }));
                    RomData.MMFileList[f].Data = data.ToArray();

                    // STT Room 10
                    ReadWriteUtils.WriteToROM(0x0212B000 + 0x4220, ci8);
                    f = RomUtils.GetFileIndexForWriting(0x0212B000);
                    ReadWriteUtils.Arr_Insert(new byte[] { 0x03, 0x00, 0x4A, 0x20 }, 0, 4, RomData.MMFileList[f].Data, 0x2434);
                    data = RomData.MMFileList[f].Data.ToList();
                    data.InsertRange(0x4A20, palette.SelectMany(s => new byte[] { (byte)(s >> 8), (byte)(s & 0xFF) }));
                    RomData.MMFileList[f].Data = data.ToArray();
                }
            }
        }

        private void WriteSpeedUps(MessageTable messageTable)
        {
            if (_randomized.Settings.SpeedupBeavers)
            {
                ResourceUtils.ApplyHack(Resources.mods.speedup_beavers);
                messageTable.UpdateMessages(new MessageEntryBuilder()
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
                messageTable.UpdateMessages(new MessageEntryBuilder()
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
                messageTable.UpdateMessages(new MessageEntryBuilder()
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
                messageTable.UpdateMessages(new MessageEntryBuilder()
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
                messageTable.UpdateMessages(new MessageEntryBuilder()
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

            if (_randomized.Settings.SpeedupBabyCuccos)
            {
                ResourceUtils.ApplyHack(Resources.mods.speedup_babycucco_minimap);
            }
        }

        private void WriteGimmicks(MessageTable messageTable)
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
                WriteSunsSong(messageTable);
            }

            if (_randomized.Settings.AllowFierceDeityAnywhere)
            {
                ResourceUtils.ApplyHack(Resources.mods.fierce_deity_anywhere);
            }

            if (_randomized.Settings.AllowFierceDeityAnywhere || _randomized.Settings.SaferGlitches)
            {
                ResourceUtils.ApplyHack(Resources.mods.safer_glitches_fierce_deity);
            }

            if (_randomized.Settings.GiantMaskAnywhere)
            {
                ResourceUtils.ApplyHack(Resources.mods.giant_mask_anywhere);
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

            if (!_randomized.Settings.VanillaMoonTrialAccess)
            {
                ResourceUtils.ApplyHack(Resources.mods.fix_moon_trial_access);
            }
        }

        private void WriteSunsSong(MessageTable messageTable)
        {
            messageTable.UpdateMessages(new MessageEntryBuilder()
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

        private void WriteSoundEffects(Random random)
        {
            if (!_cosmeticSettings.RandomizeSounds)
            {
                return;
            }

            var messageTable = MessageTable.ReadDefault();

            var shuffledSoundEffects = new Dictionary<SoundEffect, SoundEffect>();

            foreach (var sound in SoundEffects.All())
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

                oldSound.TryReplaceWith(newSound);

                Debug.WriteLine($"Writing SFX {newSound} --> {oldSound}");
            }

            messageTable.ApplyRandomSoundEffects(shuffledSoundEffects);

            MessageTable.WriteDefault(messageTable, false);
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
            else if (_cosmeticSettings.LowHealthSFX.HasAttribute<ValueAttribute>())
            {
                SoundEffect.LowHealthBeep.TryReplaceWith((SoundEffect)_cosmeticSettings.LowHealthSFX.GetAttribute<ValueAttribute>().Value);
            }
            else if(_cosmeticSettings.LowHealthSFX == LowHealthSFX.Random)
            {
                var soundPool = SoundEffects.FilterByTags(SoundEffect.LowHealthBeep.ReplacableByTags());
                if (soundPool.Count > 0)
                {
                    SoundEffect.LowHealthBeep.TryReplaceWith(soundPool.Random(random));
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

            if (_randomized.BlitzExtraItems != null)
            {
                itemList.AddRange(_randomized.BlitzExtraItems);
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

            if (_randomized.Settings.GaroHintStyle != GossipHintStyle.Default)
            {
                hacks.Add(Resources.mods.garo_hints);
            }

            if (_randomized.Settings.ChestGameMinimap != ChestGameMinimapState.Off)
            {
                // Write to chest game scene file the new minimap setting
                ReadWriteUtils.WriteU16ToROM(0x02131000 + 0x1D8, 0x0018);
                ReadWriteUtils.WriteToROM(0x02131000 + 0x1C8, new byte[] { 0x00, 0x01, 0xFD, 0x00, 0x00, 0x00, 0x02, 0x18, 0x00, 0x00});
                // Include bitflag to enabled minimaps when Map of Clock Town is aquired
                ReadWriteUtils.WriteToROM(0x00B3C000 + 0x0011C270 + 0x71, 0x80);
                // Overwrite one of the placeholder minimaps in dangeon_keep
                ReadWriteUtils.WriteToROM(0x01128000 + 0x42C8, Resources.mods.chestgame_minimap);
            }

            if (_randomized.Settings.SaferGlitches)
            {
                hacks.Add(Resources.mods.safer_glitches_sodt);
                hacks.Add(Resources.mods.safer_glitches_tatl_text_zero_fourth_day);
                hacks.Add(Resources.mods.safer_glitches_fix_0thday_erase);
                hacks.Add(Resources.mods.safer_glitches_fix_goron_bow);
                hacks.Add(Resources.mods.safer_glitches_index_warp);
                hacks.Add(Resources.mods.safer_glitches_fix_4thday_mayor);
                hacks.Add(Resources.mods.safer_glitches_fix_4thday_gossip);
                hacks.Add(Resources.mods.safer_glitches_fix_4thday_deku_playground);
                hacks.Add(Resources.mods.safer_glitches_fix_0thday_4thday_town_shooting);
            }

            if (_randomized.Settings.BombchuDrops)
            {
                hacks.Add(Resources.mods.add_bombchu_drops);
            }

            if (_randomized.Settings.RequiredBossRemains < 4)
            {
                var hack = Resources.mods.update_remains_required.ToArray();
                hack[0x33] = _randomized.Settings.RequiredBossRemains;
                hacks.Add(hack);
            }

            if (_randomized.Settings.TakeDamageWhileShielding)
            {
                hacks.Add(Resources.mods.take_damage_while_shielding);
            }

            if (_randomized.Settings.TakeDamageFromVoid)
            {
                hacks.Add(Resources.mods.take_damage_from_void);
            }

            if (_randomized.Settings.TakeDamageFromDog)
            {
                hacks.Add(Resources.mods.take_damage_from_dog);
            }

            if (_randomized.Settings.TakeDamageFromGorons)
            {
                hacks.Add(Resources.mods.take_damage_from_goron);
            }

            if (_randomized.Settings.TakeDamageGettingCaught)
            {
                hacks.Add(Resources.mods.take_damage_from_caught);

                if (_randomized.Settings.LogicMode != LogicMode.Vanilla)
                {
                    hacks.Add(Resources.mods.sonata_check_gentle_throw);
                }
            }

            if (_randomized.Settings.TakeDamageFromGibdosFaster)
            {
                hacks.Add(Resources.mods.take_damage_from_gibdo_immediately);
            }

            if (_randomized.Settings.TakeDamageFromDexihands)
            {
                hacks.Add(Resources.mods.take_damage_from_dexihands);
            }

            foreach (var hack in hacks)
            {
                ResourceUtils.ApplyHack(hack);
            }
        }

        private void WriteShopText(MessageTable messageTable, List<MessageEntry> newMessages)
        {
            // update tingle shops
            foreach (var messageShopText in Enum.GetValues<MessageShopText>())
            {
                var messageShop = messageShopText.GetAttribute<MessageShopAttribute>();
                var item1 = _randomized.ItemList.First(io => io.NewLocation == messageShop.Items[0]);
                var item2 = _randomized.ItemList.First(io => io.NewLocation == messageShop.Items[1]);
                var messageId = (ushort)messageShopText;
                var messageHeader = messageTable.GetMessage(messageId).Header;
                var cost1 = ReadWriteUtils.Arr_ReadU16(messageHeader, 5);
                var cost2 = ReadWriteUtils.Arr_ReadU16(messageHeader, 7);

                newMessages.Add(new MessageEntryBuilder()
                    .Id(messageId)
                    .Message(it =>
                    {
                        var item1Cost = $"{cost1} Rupee{(cost1 != 1 ? "s" : "")}";
                        var item2Cost = $"{cost2} Rupee{(cost2 != 1 ? "s" : "")}";
                        var maxLineLength = 35;
                        var maxItem1NameLength = maxLineLength - $": {item1Cost}".Length;
                        var maxItem2NameLength = maxLineLength - $": {item2Cost}".Length;
                        var item1Name = item1.DisplayName();
                        var item2Name = item2.DisplayName();
                        if (item1Name.Length > maxItem1NameLength)
                        {
                            item1Name = item1Name.Substring(0, maxItem1NameLength - 3) + "...";
                        }
                        if (item2Name.Length > maxItem2NameLength)
                        {
                            item2Name = item2Name.Substring(0, maxItem2NameLength - 3) + "...";
                        }
                        switch (messageShop.MessageShopStyle)
                        {
                            case MessageShopStyle.Tingle:
                                it.StartGreenText()
                                .ThreeChoices()
                                .RuntimeItemName(item1Name, item1.NewLocation.Value).Text(": ").Red(item1Cost).NewLine()
                                .RuntimeItemName(item2Name, item2.NewLocation.Value).Text(": ").Red(item2Cost).NewLine()
                                .Text("No Thanks")
                                .EndFinalTextBox();
                                break;
                            case MessageShopStyle.MilkBar:
                                it.Text("What'll it be?")
                                .EndTextBox()
                                .StartGreenText()
                                .ThreeChoices()
                                .RuntimeItemName(item1Name, item1.NewLocation.Value).Text(": ").Pink(item1Cost).NewLine()
                                .RuntimeItemName(item2Name, item2.NewLocation.Value).Text(": ").Pink(item2Cost).NewLine()
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

            var bigBombBagItem = _randomized.ItemList.First(io => io.NewLocation == Item.UpgradeBigBombBag);
            newMessages.Add(new MessageEntryBuilder()
                .Id(0x29D3)
                .Message(it =>
                {
                    it.PlaySoundEffect(0x6952)
                    .Text("Tonight's special bargain was just").NewLine()
                    .Text("stolen. It just came in seconds").NewLine()
                    .Text("ago. This is really hot!").NewLine()
                    .PauseText(20)
                    .QuickText(() => it.Text("I kid you not!"))
                    .EndTextBox()
                    .Text("Actually, it's somethin' sold over").NewLine()
                    .Text("at the ").Red("Bomb Shop").Text(", but a thief").NewLine()
                    .Text("sold it to me...")
                    .EndTextBox()
                    .RuntimeWrap(() =>
                    {
                        it.Text("It's ")
                        .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                        .QuickText(() =>
                        {
                            it.Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(".");
                        })
                        ;
                    })
                    .NewLine()
                    .PauseText(10)
                    .Text("Check it out! Buy it!")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );

            newMessages.Add(new MessageEntryBuilder()
                .Id(0x29D7)
                .Message(it =>
                {
                    it.PlaySoundEffect(0x6952)
                    .Text("Tonight's special bargain was just").NewLine()
                    .Text("stolen. It just came in seconds").NewLine()
                    .Text("ago. It's really hot!").NewLine()
                    .PauseText(20)
                    .QuickText(() => it.Text("I kid you not."))
                    .EndTextBox()
                    .Text("Actually, it's somethin' sold over").NewLine()
                    .Text("at the ").Red("Bomb Shop").Text(", but a thief").NewLine()
                    .Text("sold it to me.")
                    .EndTextBox()
                    .RuntimeWrap(() =>
                    {
                        it.Text("It's ")
                        .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                        .QuickText(() =>
                        {
                            it.Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(".");
                        })
                        ;
                    })
                    .NewLine()
                    .PauseText(10)
                    .Text("Check it out! Buy it!")
                    .DisableTextSkip2()
                    .EndFinalTextBox();
                })
                .Build()
            );

            var allNightMaskPurchaseItem = _randomized.ItemList.First(io => io.NewLocation == Item.MaskAllNight);
            if (allNightMaskPurchaseItem.Item != Item.MaskAllNight)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x29D4)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6952)
                        .Text("Tonight's bargain is").NewLine()
                        .RuntimeWrap(() =>
                        {
                            it.QuickText(() =>
                            {
                                it.RuntimeArticle(allNightMaskPurchaseItem.DisplayItem, allNightMaskPurchaseItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(allNightMaskPurchaseItem.DisplayName(), allNightMaskPurchaseItem.NewLocation.Value);
                                });
                            })
                            .Text(" ")
                            .PauseText(10)
                            .Text("for use at bedtime.")
                            ;
                        })
                        .EndTextBox()
                        .Text("I forgot when this was made, but").NewLine()
                        .Text("it sure is freaky...See?")
                        .EndTextBox()
                        .Text("Pretty creepy, huh?")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x29D8)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6952)
                        .Text("Tonight's bargain is").NewLine()
                        .RuntimeWrap(() =>
                        {
                            it.QuickText(() =>
                            {
                                it.RuntimeArticle(allNightMaskPurchaseItem.DisplayItem, allNightMaskPurchaseItem.NewLocation.Value)
                                .Red(() =>
                                {
                                    it.RuntimeItemName(allNightMaskPurchaseItem.DisplayName(), allNightMaskPurchaseItem.NewLocation.Value);
                                });
                            })
                            .Text(" ")
                            .PauseText(10)
                            .Text("for use at bedtime.")
                            ;
                        })
                        .EndTextBox()
                        .Text("I forgot when this was made, but").NewLine()
                        .Text("it sure is freaky...See?")
                        .EndTextBox()
                        .Text("Pretty creepy, huh?")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }
        }

        private void WriteWorldModelText(List<MessageEntry> newMessages)
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
                            wrapped.Text("He said he'd steal my ")
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

        private void WriteMiscItemText(List<MessageEntry> newMessages)
        {
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

            var kotakePotionItem = _randomized.ItemList.First(io => io.NewLocation == Item.ItemBottleWitch);
            if (kotakePotionItem.Item != Item.ItemBottleWitch)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x839)
                    .Message(it =>
                    {
                        it.CompileTimeWrap("If it's just the Skull Kid, then what harm could he possibly do?")
                        .EndTextBox()
                        .Text("Oh!!").NewLine()
                        .Text("Well, if that's true, then take this").NewLine()
                        .Text("and find a way to help her...").NewLine()
                        .Text("This isn't good...")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x83A)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3901)
                        .Text("Quick! Go help Koume!")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x83B)
                    .Message(it =>
                    {
                        it.Text("Oh! Did you find a way to help").NewLine()
                        .Text("Koume?")
                        .EndTextBox()
                        .Text("Don't tell me...")
                        .EndTextBox()
                        .Text("You need more help, do you?")
                        .EndTextBox()
                        .Text(".........")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            if (_randomized.ItemList[Item.ItemTingleMapTown].IsRandomized
                || _randomized.ItemList[Item.ItemTingleMapWoodfall].IsRandomized
                || _randomized.ItemList[Item.ItemTingleMapSnowhead].IsRandomized
                || _randomized.ItemList[Item.ItemTingleMapRanch].IsRandomized
                || _randomized.ItemList[Item.ItemTingleMapGreatBay].IsRandomized
                || _randomized.ItemList[Item.ItemTingleMapStoneTower].IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1D02)
                    .Message(it =>
                    {
                        it.CompileTimeWrap("Now while I stand here waiting for a fairy of my own, I sell items to help out my father.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1D03)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x697B)
                        .Text("Lucky! Lucky!").NewLine()
                        .Text("You're so lucky to have a fairy! ").NewLine()
                        .Text("I know! I know! We should be").NewLine()
                        .Text("friends!")
                        .EndTextBox()
                        .Text("Yes! Yes! In exchange, I will sell").NewLine()
                        .Text("you items for cheap as a sign of").NewLine()
                        .Text("my friendship.")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1D04)
                    .Message(it =>
                    {
                        it.Text("Will you buy one of Tingle's ").Red("items").Text("?")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var kafeiItem = _randomized.ItemList.First(io => io.NewLocation == Item.TradeItemPendant);
            if (kafeiItem.Item != Item.TradeItemPendant)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2975)
                    .Message(it =>
                    {
                        it.Text("Please take this...")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var oldLadyItem = _randomized.ItemList.First(io => io.NewLocation == Item.MaskBlast);
            if (oldLadyItem.Item != Item.MaskBlast)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2A35)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x690C)
                        .CompileTimeWrap("Yes, I must thank you. It's dangerous, but maybe you can use it.")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            if (_randomized.ItemList[Item.MundaneItemCuriosityShopBlueRupee].IsRandomized
                || _randomized.ItemList[Item.MundaneItemCuriosityShopRedRupee].IsRandomized
                || _randomized.ItemList[Item.MundaneItemCuriosityShopPurpleRupee].IsRandomized
                || _randomized.ItemList[Item.MundaneItemCuriosityShopGoldRupee].IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x29EF)
                    .Message(it =>
                    {
                        it.Text("For that, I'll give you").NewLine()
                        .Text("something worth \xDE.").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("I'll take it.").NewLine()
                        .Text("No thanks")
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

            // replace "Razor Sword is now blunt" message with get-item message for Kokiri Sword.
            newMessages.Add(new MessageEntryBuilder()
                .Id(0xF9)
                .Header(it =>
                {
                    it.Standard2().Icon(0x37);
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

            foreach (var fairy in ItemUtils.DungeonStrayFairies().Append(Item.CollectibleStrayFairyClockTown))
            {
                var io = _randomized.ItemList[fairy];
                if (io.IsRandomized || io.Item != fairy)
                {
                    ResourceUtils.ApplyHack(Resources.mods.fix_fairies);
                    break;
                }
            }
        }

        private void WriteOathHintText(List<MessageEntry> newMessages)
        {
            var oathItem = _randomized.ItemList[Item.SongOath];

            if (!_randomized.Settings.AddSongs)
            {
                switch (oathItem.NewLocation.Value)
                {
                    case Item.SongTime:
                        break;
                    case Item.SongHealing:
                        oathItem = null;
                        break;
                    case Item.SongEpona:
                        oathItem = _randomized.ItemList[Item.ItemPowderKeg];
                        break;
                    case Item.SongSoaring:
                        oathItem = _randomized.ItemList[Item.MaskDeku];
                        break;
                    case Item.SongStorms:
                        oathItem = _randomized.ItemList[Item.MaskCaptainHat];
                        break;
                    case Item.SongSonata:
                        oathItem = _randomized.ItemList[Item.MaskDeku];
                        break;
                    case Item.SongLullaby:
                        oathItem = _randomized.ItemList[Item.MaskGoron];
                        break;
                    case Item.SongLullabyIntro:
                        oathItem = _randomized.ItemList[Item.MaskGoron];
                        break;
                    case Item.SongNewWaveBossaNova:
                        oathItem = _randomized.ItemList[Item.MaskZora];
                        break;
                    case Item.SongElegy:
                        oathItem = _randomized.ItemList[Item.UpgradeMirrorShield];
                        break;
                    case Item.SongOath:
                        oathItem = null;
                        break;
                }
            }

            if (oathItem != null && oathItem.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x208B)
                    .Message(it =>
                    {
                        it.StartLightBlueText()
                        .PauseText(10)
                        .Text("\"");
                        var oathRegion = oathItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList).Name();
                        for (var i = 0; i < oathRegion.Length; i++)
                        {
                            var c = oathRegion[i];
                            it.Text(c.ToString());
                            if (i == oathRegion.Length - 1)
                            {
                                it.Text(".");
                            }
                            else if (c != ' ')
                            {
                                it.PauseText(20);
                            }
                        }
                        it.Text("\"").NewLine()
                        .Text(" ").NewLine()
                        .PauseText(10)
                        .Text("That's what they're saying.")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }
        }

        private void WriteRemainsHintText(List<MessageEntry> newMessages)
        {
            var remains = ItemUtils.BossRemains().Where(r => _randomized.ItemList[r].Item == r);
            var remainsAreRandomized = remains.Any(r => _randomized.ItemList[r].IsRandomized)
                && !_randomized.Settings.BossRemainsMode.HasFlag(BossRemainsMode.GreatFairyRewards)
                && !_randomized.Settings.BossRemainsMode.HasFlag(BossRemainsMode.ShuffleOnly)
                && !_randomized.Settings.BossRemainsMode.HasFlag(BossRemainsMode.KeepWithinTemples);
            if (remainsAreRandomized || (remains.Count() > 0 && remains.Count() < 4))
            {
                var random = new Random(_randomized.Seed);
                var remainRegions = remains
                    .OrderBy(_ => random.Next())
                    .Select(remain =>
                    {
                        var remainLocation = _randomized.ItemList[remain].NewLocation.Value;
                        if (remainsAreRandomized)
                        {
                            return remainLocation.RegionForDirectHint(_randomized.ItemList).Name();
                        }
                        else
                        {
                            return remainLocation.RegionArea(_randomized.ItemList).Value.ToString();
                        }
                    })
                    .Distinct()
                    .ToList();
                var remainsCount = MessageUtils.NumberToWords(remains.Count());
                var isAre = remains.Count() == 1 ? "is" : "are";
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x200B)
                    .Message(it =>
                    {
                        it.StartPinkText().PlaySoundEffect(0x6851).CompileTimeWrap((wrapped) =>
                        {
                            foreach (var remainRegion in remainRegions)
                            {
                                wrapped.Text(remainRegion).Text(". ").PauseText(10);
                            }
                            wrapped.Text("Hurry...").Red($"The {remainsCount}").Text($" who {isAre} there... Bring them ").Red("here").Text("...");
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x216)
                    .Message(it =>
                    {
                        it.StartLightBlueText()
                        .Text("That mask...").NewLine()
                        .PauseText(40)
                        .Text("The Skull Kid uses the power of").NewLine()
                        .Text("that mask to do those terrible").NewLine()
                        .Text("things.")
                        .EndTextBox()
                        .Text("Well...whatever it takes, we've").NewLine()
                        .Text("gotta do something about it.")
                        .EndTextBox()
                        .CompileTimeWrap((wrapped) =>
                        {
                            wrapped.Text("...The ");
                            for (var i = 0; i < remainRegions.Count; i++)
                            {
                                var remainRegion = remainRegions[i];
                                if (!remainsAreRandomized)
                                {
                                    remainRegion = remainRegion.ToLower();
                                }
                                wrapped.Red(remainRegion);
                                if (i < remainRegions.Count - 2)
                                {
                                    wrapped.Text(", ");
                                }
                                else if (i < remainRegions.Count - 1)
                                {
                                    wrapped.Text(" and ");
                                }
                            }
                            wrapped.Text(" that Tael was trying to tell us about...");
                        })
                        .EndTextBox()
                        .CompileTimeWrap("I have no idea what he was talking about...")
                        .EndTextBox()
                        .Text("And what do you suppose he").NewLine()
                        .Text("meant by \"").Red(() =>
                        {
                            it.Text($"the {remainsCount} who {isAre}").NewLine()
                            .Text("there");
                        })
                        .Text("?\"")
                        .EndTextBox()
                        .Text("I have no idea. He always").NewLine()
                        .Text("skips important stuff. I guess we").NewLine()
                        .Text("should just go and find out...")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                ResourceUtils.ApplyHack(Resources.mods.tatl_remains_hint);
            }
        }

        private void WriteFairyAndSkullHintText(List<MessageEntry> newMessages)
        {
            var clockTownFairyItem = _randomized.ItemList[Item.CollectibleStrayFairyClockTown];
            if (clockTownFairyItem.NewLocation != Item.CollectibleStrayFairyClockTown)
            {
                var region = clockTownFairyItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList);
                var regionPreposition = region.Preposition();
                var regionName = regionPreposition == null ? null : region.Name();
                if (!string.IsNullOrWhiteSpace(regionPreposition))
                {
                    regionPreposition += " ";
                }

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x578)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938)
                        .StartLightBlueText().Text("Young one! Please hear my plea!").NewLine()
                        .Text("I have been broken and shattered").NewLine()
                        .Text("to pieces by the masked Skull Kid.")
                        .EndTextBox()
                        .CompileTimeWrap((wrapped) =>
                        {
                            wrapped.Text("Please, find the").Red(" one ").Text("Stray Fairy lost ")
                            .Text(regionPreposition ?? "").Red(regionName).Text(", and bring her ")
                            .Text("to this ").Red("Fairy Fountain").Text(".")
                            ;
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x580)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938)
                        .StartLightBlueText().Text("You...kind young one.")
                        .EndTextBox()
                        .CompileTimeWrap((wrapped) =>
                        {
                            wrapped.Text("Please, find the").Red(" one ").Text("Stray Fairy who's lost ")
                            .Text(regionPreposition ?? "").Red(regionName ?? "somewhere").Text(" and bring her ")
                            .Text("back to this ").Red("Fairy's Fountain").Text(".")
                            ;
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var strayFairyRegionLocations = ItemUtils.DungeonStrayFairies()
                .Union(ItemUtils.SwampSkulltulaTokens())
                .Union(ItemUtils.OceanSkulltulaTokens())
                .Where(item => _randomized.ItemList[item].Item == item)
                .GroupBy(fairy => fairy.Region(_randomized.ItemList).Value)
                .ToDictionary(g => g.Key, g =>
                    g.SelectMany(fairy =>
                    {
                        var location = _randomized.ItemList[fairy].NewLocation.Value;
                        var locations = location.GetAttribute<MultiLocationAttribute>()?.Locations;
                        if (locations != null)
                        {
                            return locations.Select(loc => new
                            {
                                Location = location,
                                Region = loc.Region(_randomized.ItemList).Value,
                            });
                        }
                        else
                        {
                            return new[]
                            {
                                    new
                                    {
                                        Location = location,
                                        Region = location.Region(_randomized.ItemList).Value
                                    }
                            };
                        }
                    })
                    .GroupBy(x => x.Region)
                    .ToDictionary(g2 => g2.Key, g2 => g2.Select(x => x.Location).ToArray())
                );

            if (strayFairyRegionLocations.Any())
            {
                ResourceUtils.ApplyHack(Resources.mods.dungeon_fairy_npc_hint);
            }

            if (strayFairyRegionLocations.ContainsKey(Region.WoodfallTemple))
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x582)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Kind young one! Please hear my").NewLine()
                        .Text("plea! Please find the fairies").NewLine()
                        .Text("who match our ").Red("color").Text(".")
                        .EndTextBox()
                        .Text("Please bring them back to us!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x583)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Please fine a way to save the").NewLine()
                        .Text("fairies, and bring them back").NewLine()
                        .Text("here!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x584)
                    .Header(h => h.Icon(0x11))
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("There should still be...")
                        .EndTextBox();

                        foreach (var kvp in strayFairyRegionLocations[Region.WoodfallTemple])
                        {
                            it.RuntimeStrayFairyLocations(TextCommands.ColorLightBlue, "trapped", false, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it.Text("Please save the fairies so I can").NewLine()
                        .Text("be returned to my former shape!")
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );
            }

            if (strayFairyRegionLocations.ContainsKey(Region.SnowheadTemple))
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x585)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Oh, kind, young one!").NewLine()
                        .Text("Please hear our plea! Please save").NewLine()
                        .Text("the ").Green("fairies ").Text("who match our ").Green("color").Text(" and").NewLine()
                        .Text("bring them back to us!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x586)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Please find a way to save the").NewLine()
                        .Text("fairies and bring them back").NewLine()
                        .Text("here!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x587)
                    .Header(h => h.Icon(0x11))
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("There should still be...")
                        .EndTextBox();

                        foreach (var kvp in strayFairyRegionLocations[Region.SnowheadTemple])
                        {
                            it.RuntimeStrayFairyLocations(TextCommands.ColorLightBlue, "trapped", false, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it.Text("Please bring them back here so").NewLine()
                        .Text("I can be returned to my former").NewLine()
                        .Text("shape!")
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );
            }

            if (strayFairyRegionLocations.ContainsKey(Region.GreatBayTemple))
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x588)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Oh, kind young one!").NewLine()
                        .Text("Please find the fairies who are").NewLine()
                        .Text("the same ").DarkBlue("color").Text(" as we are and")
                        .Text("bring them back to us!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x589)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Please find a way to save the").NewLine()
                        .Text("fairies, and bring them back").NewLine()
                        .Text("here!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x58A)
                    .Header(h => h.Icon(0x11))
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("There should still be...")
                        .EndTextBox();

                        foreach (var kvp in strayFairyRegionLocations[Region.GreatBayTemple])
                        {
                            it.RuntimeStrayFairyLocations(TextCommands.ColorLightBlue, "trapped", false, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it.Text("Please save them and bring them").NewLine()
                        .Text("back here!")
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );
            }

            if (strayFairyRegionLocations.ContainsKey(Region.StoneTowerTemple))
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x58B)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Oh, kind young one!").NewLine()
                        .Text("Please hear our plea! Please find").NewLine()
                        .Text("the fairies who are the same").NewLine()
                        .Yellow("color").Text(" as we are and bring them").NewLine()
                        .Text("back to us!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x58C)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("Please save the the fairies and").NewLine()
                        .Text("bring them back here!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x58D)
                    .Header(h => h.Icon(0x11))
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6938).StartLightBlueText()
                        .Text("There should still be...")
                        .EndTextBox();

                        foreach (var kvp in strayFairyRegionLocations[Region.StoneTowerTemple])
                        {
                            it.RuntimeStrayFairyLocations(TextCommands.ColorLightBlue, "trapped", false, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it.Text("Please save them and bring them").NewLine()
                        .Text("back here so I can be returned").NewLine()
                        .Text("to my former shape!")
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );
            }

            if (strayFairyRegionLocations.ContainsKey(Region.SwampSpiderHouseItems) && ItemUtils.SwampSkulltulaTokens().Any(token => _randomized.ItemList[token].IsRandomized))
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x911)
                    .Message(it =>
                    {
                        it.CompileTimeWrap((wrapped) =>
                        {
                            wrapped.Text("I beg of you...To lift the curse...Find them all...The ").Red("golden spider tokens").Text("...");
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                var dict = strayFairyRegionLocations[Region.SwampSpiderHouseItems];

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x912)
                    .Header(h => h.Y(0).Icon(0x52).NextMessage(0x9912))
                    .Message(it =>
                    {
                        it.Text("There should still be...")
                        .EndTextBox();

                        var firstHalf = dict.Take(dict.Count / 2).ToList();

                        for (var i = 0; i < firstHalf.Count; i++)
                        {
                            var kvp = firstHalf[i];
                            it.RuntimeStrayFairyLocations(TextCommands.ColorWhite, "hiding", i == firstHalf.Count - 1, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );

                _extraMessages.Add(new MessageEntryBuilder()
                    .Id(0x9912)
                    .Header(h => h.Y(0).Icon(0x52))
                    .Message(it =>
                    {
                        var lastHalf = dict.Skip(dict.Count / 2).ToList();

                        for (var i = 0; i < lastHalf.Count; i++)
                        {
                            var kvp = lastHalf[i];
                            it.RuntimeStrayFairyLocations(TextCommands.ColorWhite, "hiding", false, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it
                        .Text("Please hurry...")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x914)
                    .Header(h => h.Y(0).Icon(0x52).NextMessage(0x9912))
                    .Message(it =>
                    {
                        it.Text("Please... There should still be...")
                        .EndTextBox();

                        var firstHalf = dict.Take(dict.Count / 2).ToList();

                        for (var i = 0; i < firstHalf.Count; i++)
                        {
                            var kvp = firstHalf[i];
                            it.RuntimeStrayFairyLocations(TextCommands.ColorWhite, "hiding", i == firstHalf.Count - 1, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );
            }

            if (strayFairyRegionLocations.ContainsKey(Region.OceanSpiderHouseItems) && ItemUtils.OceanSkulltulaTokens().Any(token => _randomized.ItemList[token].IsRandomized))
            {
                var dict = strayFairyRegionLocations[Region.OceanSpiderHouseItems];

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1135)
                    .Header(h => h.Y(0).Icon(0x52).NextMessage(0xA135))
                    .Message(it =>
                    {
                        it.Text("I beg you! Lift the curse on this").NewLine()
                        .Text("place! There should still be...")
                        .EndTextBox();

                        var firstHalf = dict.Take(dict.Count / 2).ToList();

                        for (var i = 0; i < firstHalf.Count; i++)
                        {
                            var kvp = firstHalf[i];
                            it.RuntimeStrayFairyLocations(TextCommands.ColorWhite, "hiding", i == firstHalf.Count - 1, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );

                _extraMessages.Add(new MessageEntryBuilder()
                    .Id(0xA135)
                    .Header(h => h.Y(0).Icon(0x52))
                    .Message(it =>
                    {
                        var lastHalf = dict.Skip(dict.Count / 2).ToList();

                        for (var i = 0; i < lastHalf.Count; i++)
                        {
                            var kvp = lastHalf[i];
                            it.RuntimeStrayFairyLocations(TextCommands.ColorWhite, "hiding", false, kvp.Key, kvp.Value); // RuntimeWrap, EndTextBox and Red handled within or in code
                            }

                        it.Text("If you lift the curse, I'll buy").NewLine()
                        .Text("this place off you! Please hurry...")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .ExcludeFromQuickText()
                    .ShouldTransfer()
                    .Build()
                );
            }
        }

        private void WriteNPCHintText(List<MessageEntry> newMessages)
        {
            /*

            The mask salesman said that if
            you got back the precious thing
            that was stolen from you, he
            could return you to normal!
            Did you completely forget or
            what?

            ---

            Go to the shrine near the
            North Gate. You'll find the
            Great Fairy in there!

            ---

            Hey, aren't you going to the
            Great Fairy's shrine near the
            North Gate?

            ---

            Quick! We have to find the
            Stray Fairy in town and return
            the Great Fairy to normal!

            ---

            What are you doing?
            Aren't you gonna take this fairy
            to the shrine near the North
            Gate?

            ---

            He said the secret route is in
            East Clock Town...
            So why aren't you going there?

            */

            var bank1Item = _randomized.ItemList.First(io => io.NewLocation == Item.UpgradeAdultWallet);
            if (bank1Item.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x44D)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("For example, if you deposit ").Pink("200 Rupees").Text(", you'll get ")
                            .RuntimeArticle(bank1Item.DisplayItem, bank1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bank1Item.DisplayName(), bank1Item.NewLocation.Value);
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
                    .Id(0x47A)
                    .Message(it =>
                    {
                        it.CompileTimeWrap("See! Wasn't that a great incentive for saving money? Keep at it and you'll get another special gift in no time!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var bank2Item = _randomized.ItemList.First(io => io.NewLocation == Item.MundaneItemBankBlueRupee);
            if (bank2Item.Item != Item.MundaneItemBankBlueRupee)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x47B)
                    .Message(it =>
                    {
                        it.CompileTimeWrap("That's what they call a rewards program!")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var romaniGameItem = _randomized.ItemList.First(io => io.NewLocation == Item.SongEpona);
            if (romaniGameItem.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x4C1)
                    .Message(it =>
                    {
                        it.ThreeChoices()
                        .StartGreenText()
                        .Text(romaniGameItem.DisplayName()).NewLine()
                        .Text("Song of Feelings").NewLine() // purposely changed from "Song of Healing" to avoid conflicting answers
                        .Text("Song of the Field")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var milkBarChateauItem = _randomized.ItemList.First(io => io.NewLocation == Item.ShopItemMilkBarChateau);
            if (milkBarChateauItem.Item != Item.ShopItemMilkBarChateau)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x4D7)
                    .Message(it =>
                    {
                        it.ThreeChoices()
                        .StartGreenText()
                        .Text("Romani Run").NewLine()
                        .Text(milkBarChateauItem.DisplayName()).NewLine()
                        .Text("Chateau Moroni")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2AFA)
                    .Message(it =>
                    {
                        it.Text("We're called a ").Red("Milk Bar").Text(", and we").NewLine()
                        .Text("serve the ").DarkBlue("item of the night").Text(".")
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Our most popular, of course, ")
                            .RuntimeVerb(milkBarChateauItem.DisplayItem, milkBarChateauItem.NewLocation.Value)
                            .Text(" ")
                            .RuntimeArticle(milkBarChateauItem.DisplayItem, milkBarChateauItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(milkBarChateauItem.DisplayName(), milkBarChateauItem.NewLocation.Value);
                            })
                            .Text(". Vintage.")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("We source ")
                            .RuntimePronoun(milkBarChateauItem.DisplayItem, milkBarChateauItem.NewLocation.Value)
                            .Text(" from Romani Ranch, and the current price is ")
                            .Pink("200 Rupees")
                            .Text("!")
                            ;
                        })
                        .EndTextBox()
                        .Text("...You're not surprised?")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            /*

            Please find a way to return me to
            the Fairy Fountain in North Clock
            Town.

            */

            var bigBombBagItem = _randomized.ItemList.First(io => io.NewLocation == Item.UpgradeBigBombBag);
            if (bigBombBagItem.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x648)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("We're expecting to get ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value, "our ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" back in stock pretty soon...")
                            ;
                        })
                        .EndTextBox()
                        .Text("But now that I think about it, it's").NewLine()
                        .Text("already late...")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x649)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("We just got ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" in stock.")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x64A)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Actually, we should've had ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" in stock, but it seems there was an accident getting it here to the store.")
                            ;
                        })
                        .EndTextBox()
                        .Text("I don't know when we'll be getting").NewLine()
                        .Text("it now...")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x65A)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("A nice fella helped me out, so we can finally sell ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text("!")
                            ;
                        })
                        .EndTextBox()
                        .Text("Once again, the Giants are looking").NewLine()
                        .Text("out for me!")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x65B)
                    .Message(it =>
                    {
                        it.Text("Umm, Mommy...").NewLine()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Don't go picking up ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" in the middle of the night anymore.")
                            ;
                        })
                        .EndTextBox()
                        .Text("It's like asking to be mugged.").NewLine()
                        .EndTextBox()
                        .Text("And I've actually heard that a").NewLine()
                        .Text("thief has been lurking on the").NewLine()
                        .Text("outskirts of town...")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x65C)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("But it's been our lifelong dream to sell ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" since back in your Daddy's day.")
                            ;
                        })
                        .EndTextBox()
                        .Text("Since we can't get the Goron-made").NewLine()
                        .Text("goods, this was our big chance,").NewLine()
                        .Text("sonny!")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x65E)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Please try ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value, "our ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
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
                    .Id(0x660)
                    .Message(it =>
                    {
                        it.Text("It's such a shame... ").NewLine()
                        .RuntimeWrap(() =>
                        {
                            it.Text("I thought we could finally sell ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text("...")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x661)
                    .Message(it =>
                    {
                        it.Text("Umm, Mommy...").NewLine()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Don't go picking up ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" in the middle of the night anymore.")
                            ;
                        })
                        .EndTextBox()
                        .Text("It's like asking to be mugged.").NewLine()
                        .EndTextBox()
                        .Text("And doesn't the North Gate have").NewLine()
                        .Text("a reputation for being a dangerous").NewLine()
                        .Text("place?")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2A32)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6909)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Oh my. And I thought we would finally be able to stock ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" in our store. What a shame...")
                            ;
                        })
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2A33)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x6909)
                        .Text("Oh my. You think it'll never").NewLine()
                        .Text("happen to you. Well, now I've").NewLine()
                        .Text("learned my lesson.")
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("And I thought we would finally be able to stock ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" in our shop. It's too bad...")
                            ;
                        })
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x2A34)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x690A)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Thank you. Since he didn't make off with them, I can finally stock ")
                            .RuntimeArticle(bigBombBagItem.DisplayItem, bigBombBagItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(bigBombBagItem.DisplayName(), bigBombBagItem.NewLocation.Value);
                            })
                            .Text(" at our shop. Maybe I'll put 'em out tomorrow.")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var kegItem = _randomized.ItemList[Item.ItemPowderKeg];
            if (kegItem.IsRandomized && kegItem.Item == Item.ItemPowderKeg)
            {
                var region = kegItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList);
                var regionPreposition = region.Preposition();
                var regionName = regionPreposition == null ? null : region.Name();
                if (!string.IsNullOrWhiteSpace(regionPreposition))
                {
                    regionPreposition += " ";
                }

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x67D)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3ABB)
                        .RuntimeWrap(() =>
                        {
                            it.Text("OK, listen here. You didn't hear it from me, but there's a ")
                            .Red("Powder Keg")
                            .Text(" somewhere ").Text(regionPreposition ?? "").Red(regionName).Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("Come back here after you learn to use that, then I can sell you one.")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x680)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3ABB)
                        .RuntimeWrap(() =>
                        {
                            it.Text("OK, listen here. You didn't hear it from me, but there's a ")
                            .Red("Powder Keg")
                            .Text(" somewhere ").Text(regionPreposition ?? "").Red(regionName).Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("Come back here after you learn to use that, then I can sell you one.")
                        .EndTextBox()
                        .CompileTimeWrap("But the next time you come, I might not be here.")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x681)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3ABB)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Quick! Find the ")
                            .Red("Powder Keg")
                            .Text(" somewhere ").Text(regionPreposition ?? "").Red(regionName)
                            .Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("Learn how to use it.")
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x683)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x3ABB)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Quick! Find the ")
                            .Red("Powder Keg")
                            .Text(" somewhere ").Text(regionPreposition ?? "").Red(regionName)
                            .Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("Learn how to use it.")
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            /*

            This is the Bombers' Notebook.
            It contains the words we live by!
            Read it over!

            1. Find troubled people and add
                their names and pictures.
                Only 20 people will fit in
                your book.
            2. Promise to help them.
                Mark promises with Promise
                Stickers. Never be late with
                fulfilling your promises.
            3. Whenever you solve someone's
                problem, it makes you happy,
                so a Happy Sticker will be
                added to your book.
            4. No removing stickers!
                Use Promise Stickers to keep
                track of people until everyone
                is happy.
            Don't forget the rules!

            */

            var goodPictoContestItem = _randomized.ItemList.First(io => io.NewLocation == Item.MundaneItemPictographContestRedRupee);
            if (goodPictoContestItem.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x9CD)
                    .Message(it =>
                    {
                        it.Text("Now then, please choose your").NewLine()
                        .Text("prize:").NewLine()
                        .TwoChoices()
                        .StartGreenText()
                        .RuntimeItemName(goodPictoContestItem.DisplayName(), goodPictoContestItem.NewLocation.Value).NewLine()
                        .Text("Another boat cruise")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var standardPictoContestItem = _randomized.ItemList.First(io => io.NewLocation == Item.MundaneItemPictographContestBlueRupee);
            if (standardPictoContestItem.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x9D5)
                    .Message(it =>
                    {
                        it.Text("All right...Choose your prize").NewLine()
                        .Text(" ").NewLine()
                        .TwoChoices()
                        .StartGreenText()
                        .RuntimeItemName(standardPictoContestItem.DisplayName(), standardPictoContestItem.NewLocation.Value).NewLine()
                        .Text("Another boat cruise")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            /*

            Huh? Haven't you gotten a
            pictograph box yet?

            If you take the boat cruise, they'll
            give you one for sure. Go ask at
            that window.

            */

            var goldDustItem = _randomized.ItemList[Item.ItemBottleGoronRace];
            if (goldDustItem.IsRandomized && goldDustItem.Item == Item.ItemBottleGoronRace)
            {
                var region = goldDustItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList);
                var regionPreposition = region.Preposition();
                var regionName = regionPreposition == null ? null : region.Name();
                if (!string.IsNullOrWhiteSpace(regionPreposition))
                {
                    regionPreposition += " ";
                }

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0xC49)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("What? What?!? You say that ").Red("gold dust").Text(" can be found ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text("? How do you even know that?")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0xC4A)
                    .Message(it =>
                    {
                        it.Text("What if you tried searching there?")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            /*

            But...
            If you have gold dust, I'll buy it
            off you for 40 Rupees.
            How about it?

            ---

            So, come on...Will you sell it for
            40 Rupees?

            ---

            See, it was long ago when I used
            to use this thing called a
            hookshot to catch fish off the
            coast...

            But I was attacked by these
            pirates, and they took it away
            with them.

            They say...
            it's a legendary treasure..

            ---

            If you're set on meeting those
            beautiful pirates, try going to the
            valley on the other side of town!

            I've heard there's a mask there
            that lets the wearer escape
            detection.

            */

            var beaverRace1Item = _randomized.ItemList.First(io => io.NewLocation == Item.ItemBottleBeavers);
            if (beaverRace1Item.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10CF)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x291A)
                        .RuntimeWrap(() =>
                        {
                            it.Text("You want ")
                            .RuntimeArticle(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace1Item.DisplayName(), beaverRace1Item.NewLocation.Value);
                            })
                            .Text(", don't you? You never learn your lesson!");
                        })
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10D0)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x291A)
                        .Text("Koo, koo, koo.").NewLine()
                        .Text("OK, I'll give you")
                        .RuntimePronounOrAmount(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                        .Text(".")
                        .EndTextBox()
                        .Text("But...")
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10D4)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x291A)
                        .Text("Koo, koo, koo.").NewLine()
                        .RuntimeWrap(() =>
                        {
                            it.Text("If you want ")
                            .RuntimeArticle(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace1Item.DisplayName(), beaverRace1Item.NewLocation.Value);
                            })
                            .Text(", I can give you one...");
                        })
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("But only if you can swim through all the ")
                            .Red("rings")
                            .Text(" in the river in under ")
                            .Red("two minutes")
                            .Text(".")
                            ;
                        })
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10E0)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x2919)
                        .Text("What?").NewLine()
                        .PauseText(20)
                        .RuntimeWrap(() =>
                        {
                            it.Text("He wants ")
                            .RuntimeArticle(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace1Item.DisplayName(), beaverRace1Item.NewLocation.Value);
                            })
                            .Text("?");
                        })
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10E1)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x291A)
                        .RuntimeWrap(() =>
                        {
                            it.Text("Yeah... ")
                            .RuntimeArticle(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace1Item.DisplayName(), beaverRace1Item.NewLocation.Value);
                            })
                            .Text(".")
                            ;
                        })
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10E3)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x2919)
                        .RuntimeWrap(() =>
                        {
                            it.Text("To us, ")
                            .RuntimeArticle(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace1Item.DisplayName(), beaverRace1Item.NewLocation.Value);
                            })
                            .Text(" ")
                            .RuntimeVerb(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Text(" a treasure.")
                            ;
                        })
                        .EndTextBox()
                        .Text("I can't give you")
                        .RuntimePronounOrAmount(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                        .Text(" just because").NewLine()
                        .Text("you beat my little brother.")
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10E4)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x291A)
                        .Text("We can't just give you")
                        .RuntimePronounOrAmount(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                        .Text(".")
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10E5)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x2919)
                        .Text("I'll give you")
                        .RuntimePronounOrAmount(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                        .Text(" after you race").NewLine()
                        .Text("once more against me.")
                        .EndTextBox()
                        .Text("So, will you try?").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("Sure").NewLine()
                        .Text("No thanks")
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x125F)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Hey, aren't you Mikau? Were you able to get ")
                            .RuntimeArticle(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace1Item.DisplayName(), beaverRace1Item.NewLocation.Value);
                            })
                            .Text(" from the beavers on top of the ").Red("waterfall").Text("?");
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1261)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("But what do you need ")
                            .RuntimeArticle(beaverRace1Item.DisplayItem, beaverRace1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace1Item.DisplayName(), beaverRace1Item.NewLocation.Value);
                            })
                            .Text(" for, Mikau?");
                        })
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var beaverRace2Item = _randomized.ItemList.First(io => io.NewLocation == Item.HeartPieceBeaverRace);
            if (beaverRace2Item.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10F5)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x2919)
                        .RuntimeWrap(() =>
                        {
                            it.Text("But all we have left ")
                            .RuntimeVerb(beaverRace2Item.DisplayItem, beaverRace2Item.NewLocation.Value)
                            .Text(" ")
                            .RuntimeArticle(beaverRace2Item.DisplayItem, beaverRace2Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(beaverRace2Item.DisplayName(), beaverRace2Item.NewLocation.Value);
                            })
                            .Text("...right, little brother?")
                            ;
                        })
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x10F6)
                    .Message(it =>
                    {
                        it.PlaySoundEffect(0x291A)
                        .CompileTimeWrap("Yes, that's all we have, big brother.")
                        .DisableTextSkip()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var oceanSpiderHouseDay1Item = _randomized.ItemList.First(io => io.NewLocation == Item.UpgradeGiantWallet);
            var oceanSpiderHouseDay2Item = _randomized.ItemList.First(io => io.NewLocation == Item.MundaneItemOceanSpiderHouseDay2PurpleRupee);
            var oceanSpiderHouseDay3Item = _randomized.ItemList.First(io => io.NewLocation == Item.MundaneItemOceanSpiderHouseDay3RedRupee);
            if (oceanSpiderHouseDay1Item.IsRandomized || oceanSpiderHouseDay2Item.IsRandomized || oceanSpiderHouseDay3Item.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1131)
                    .Message(it =>
                    {
                        it.Text("It's gotten bigger again.").NewLine()
                        .Text("Th-this is bad...")
                        .EndTextBox()
                        .CompileTimeWrap("I'll have to sell my life savings to find somewhere to hide!")
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("But all I have left is ")
                            .RuntimeArticle(oceanSpiderHouseDay1Item.DisplayItem, oceanSpiderHouseDay1Item.NewLocation.Value, "my ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(oceanSpiderHouseDay1Item.DisplayName(), oceanSpiderHouseDay1Item.NewLocation.Value);
                            })
                            .Text("...")
                            ;
                        })
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Though maybe someone will be happy with ")
                            .RuntimeArticle(oceanSpiderHouseDay2Item.DisplayItem, oceanSpiderHouseDay2Item.NewLocation.Value, "my ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(oceanSpiderHouseDay2Item.DisplayName(), oceanSpiderHouseDay2Item.NewLocation.Value);
                            })
                            .Text("...")
                            ;
                        })
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Or I can try ")
                            .RuntimeArticle(oceanSpiderHouseDay3Item.DisplayItem, oceanSpiderHouseDay3Item.NewLocation.Value, "my ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(oceanSpiderHouseDay3Item.DisplayName(), oceanSpiderHouseDay3Item.NewLocation.Value);
                            })
                            .Text(" as a last resort...")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("Well... I'd better start searching.")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }
            if (oceanSpiderHouseDay1Item.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x113B)
                    .Message(it =>
                    {
                        it.Text("If only you could have done").NewLine()
                        .Text("something about this place").NewLine()
                        .Text("yesterday...")
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("I could have given you ")
                            .RuntimeArticle(oceanSpiderHouseDay1Item.DisplayItem, oceanSpiderHouseDay1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(oceanSpiderHouseDay1Item.DisplayName(), oceanSpiderHouseDay1Item.NewLocation.Value);
                            })
                            .Text("...")
                            ;
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1140)
                    .Message(it =>
                    {
                        it.Text("But dang, if only you could have").NewLine()
                        .Text("done something ").Red("two days ago").Text("...")
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("I could have given you ")
                            .RuntimeArticle(oceanSpiderHouseDay1Item.DisplayItem, oceanSpiderHouseDay1Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(oceanSpiderHouseDay1Item.DisplayName(), oceanSpiderHouseDay1Item.NewLocation.Value);
                            })
                            .Text("...")
                            ;
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var beaverBottleItem = _randomized.ItemList[Item.ItemBottleBeavers];
            if (beaverBottleItem.IsRandomized)
            {
                var region = beaverBottleItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList);
                var regionPreposition = region.Preposition();
                var regionName = regionPreposition == null ? null : region.Name();
                if (!string.IsNullOrWhiteSpace(regionPreposition))
                {
                    regionPreposition += " ";
                }

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1240)
                    .Message(it =>
                    {
                        it.CompileTimeWrap((it) =>
                        {
                            it.Text("I told you that there's an ").Red("Empty Bottle").Text(" somewhere ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text(", right?")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("If you don't have any yet, Mikau, you should go there before heaging off to the Pirates' Fortress.")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1242)
                    .Message(it =>
                    {
                        it.CompileTimeWrap((it) =>
                        {
                            it.Text("I already told you that there's an ").Red("Empty Bottle").Text(" somewhere ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text(", right?")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("If you don't have any yet, you should go there before heaging off to the Pirates' Fortress.")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var stormsTabletItem = _randomized.ItemList.First(io => io.NewLocation == Item.SongStorms);
            if (stormsTabletItem.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x13F9)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Here lies ")
                            .RuntimeArticle(stormsTabletItem.DisplayItem, stormsTabletItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(stormsTabletItem.DisplayName(), stormsTabletItem.NewLocation.Value);
                            })
                            .Text(".")
                            ;
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var dampeDiggingItem = _randomized.ItemList.First(io => io.NewLocation == Item.ItemBottleDampe);
            if (dampeDiggingItem.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x13FA)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Here lies ")
                            .RuntimeArticle(dampeDiggingItem.DisplayItem, dampeDiggingItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(dampeDiggingItem.DisplayName(), dampeDiggingItem.NewLocation.Value);
                            })
                            .Text(".")
                            ;
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var graveyardNight2Item = _randomized.ItemList.First(io => io.NewLocation == Item.HeartPieceKnuckle);
            if (graveyardNight2Item.IsRandomized)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x13FB)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("Here lies ")
                            .RuntimeArticle(graveyardNight2Item.DisplayItem, graveyardNight2Item.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(graveyardNight2Item.DisplayName(), graveyardNight2Item.NewLocation.Value);
                            })
                            .Text(".")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap("When one who possesses eyes that can see the truth arrives, the treasure far below shall awaken.")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            /*

            Look!
            There's a mask there!!!

            It's the Sun's Mask!

            ---

            I got the Sun's Mask back!

            ---

            It's 'cause I'm about as impressive
            as a stone, right?...I'm used to it,
            though.

            ---

            I am no longer part of the
            living...My sadness to the moon...
            I haven't left my dance to the
            world...I am filled with regret.
            (Translation)             
            I am disappointed, oh moon.
            I have died!

            Oh, I planned to bring the world
            together and stir it into a giant
            melting pot with my dance!

            If only I had taught my new dance
            to someone...

            */

            var garoMaskItem = _randomized.ItemList[Item.MaskGaro];
            if (garoMaskItem.IsRandomized)
            {
                var region = garoMaskItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList);
                var regionPreposition = region.Preposition();
                var regionName = regionPreposition == null ? null : region.Name();
                if (!string.IsNullOrWhiteSpace(regionPreposition))
                {
                    regionPreposition += " ";
                }

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x14EE)
                    .Message(it =>
                    {
                        it.Text("Yee-hee-hee.").NewLine()
                        .Text("Do you have your own ").Red("horse").Text("?")
                        .EndTextBox()
                        .CompileTimeWrap(it =>
                        {
                            it.Text("And if you are not wearing the ")
                            .Red("mask")
                            .Text(" that houses the ")
                            .Red("wandering spirits...")
                            ;
                        })
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Which can be found ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text("...")
                            ;
                        })
                        .EndTextBox()
                        .Text("I will not let you pass.").NewLine()
                        .Text("Yee-hee-hee.")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x14F2)
                    .Message(it =>
                    {
                        it.CompileTimeWrap(it =>
                        {
                            it.Text("But if you must enter, then you must obtain the ")
                            .Red("mask")
                            .Text(" containing ")
                            .Red("wandering spirits...")
                            ;
                        })
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("That can be found ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text("...")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x14F4)
                    .Message(it =>
                    {
                        it.Text("Yee-hee-hee.").NewLine()
                        .Text("Do you have your own ").Red("horse").Text("?")
                        .EndTextBox()
                        .CompileTimeWrap(it =>
                        {
                            it.Text("And if you are not wearing the ")
                            .Red("mask")
                            .Text(" containing ")
                            .Red("wandering spirits...")
                            ;
                        })
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("Which can be found ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text("...")
                            ;
                        })
                        .EndTextBox()
                        .Text("I will not let you pass.").NewLine()
                        .Text("Yee-hee-hee.")
                        .EndConversation()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            var songOfSoaringItem = _randomized.ItemList[Item.SongSoaring];
            if (songOfSoaringItem.IsRandomized && songOfSoaringItem.Item == Item.SongSoaring)
            {
                var region = songOfSoaringItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList);
                var regionPreposition = region.Preposition();
                var regionName = regionPreposition == null ? null : region.Name();
                if (!string.IsNullOrWhiteSpace(regionPreposition))
                {
                    regionPreposition += " ";
                }

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0xBFC)
                    .Message(it =>
                    {
                        it.CompileTimeWrap((it) =>
                        {
                            it.Text("If you have left ").Red("proof ").Text("of our encounter on any of those ").Red("stone statues...");
                        })
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("Then the ").Red("song").Text(" ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text(" will certainly be of some assistance...");
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                ResourceUtils.ApplyHack(Resources.mods.misc_changes_restore_swamp_owl);
            }

            var songOfTimeItem = _randomized.ItemList[Item.SongTime];
            if (songOfTimeItem.IsRandomized && songOfTimeItem.Item == Item.SongTime)
            {
                var region = songOfTimeItem.NewLocation.Value.RegionForDirectHint(_randomized.ItemList);
                var regionPreposition = region.Preposition();
                var regionName = regionPreposition == null ? null : region.Name();
                if (!string.IsNullOrWhiteSpace(regionPreposition))
                {
                    regionPreposition += " ";
                }

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x164A)
                    .Message(it =>
                    {
                        it.QuickText(() => it.Text("Oh, yeah!")).NewLine()
                        .Text("Now listen up!")
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("There's a mysterious song ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text(", and if you ").Red("play").Text(" that song ").Red("backwards")
                            .Text(", you can ").Red("slow").Text(" the flow of time.")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("And if you play each note ").Red("twice").Text(" in a row, you can move a ")
                            .Red("half day").Text(" forward in time!")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x1658)
                    .Message(it =>
                    {
                        it.QuickText(() => it.Text("Oh, yeah!")).NewLine()
                        .Text("Now listen up!")
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("There's a strange song ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text(", and if you ").Red("play").Text(" that song ").Red("backwards")
                            .Text(", you can ").Red("slow").Text(" the flow of time.")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("And if you play each note ").Red("twice").Text(" in a row, you can move a ")
                            .Red("half day").Text(" forward through time!")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x165E)
                    .Message(it =>
                    {
                        it.QuickText(() => it.Text("Oh, yeah!")).NewLine()
                        .Text("Now listen up!")
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("There's a mysterious song ")
                            .Text(regionPreposition ?? "").Red(regionName)
                            .Text(", and it seems if you ").Red("play").Text(" that song ").Red("backwards")
                            .Text(", you can ").Red("slow").Text(" the passage of time.")
                            ;
                        })
                        .EndTextBox()
                        .CompileTimeWrap((it) =>
                        {
                            it.Text("And if you play each note ").Red("twice").Text(" in a row, you can move a ")
                            .Red("half day").Text(" forward through time!")
                            ;
                        })
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            /*

            Patron Guidelines for the 
            Milk Bar, Latte:

            We offer our customers limited-
            run milk. Thus, we are a members-
            only establishment.

            Those who do not have proof of
            membership will be refused
            service.
                         The Owner

            ---

            If you can get back the precious
            item that was stolen from you,
            I will return you to normal.

            ---

            Oh! Oh! Ohhh!!!
            You got it! You got it!
            You got it! You got it!!!

            Then listen to me. Please play this
            song that I am about to perform,
            and remember it well...

            This is a melody that heals evil
            magic and troubled spirits, turning
            them into masks.

            I am sure it will be of assistance
            to you in the future.

            Ah, yes. I give you this mask in
            commemoration of this day.

            Fear not, for the magic has been
            sealed inside the mask.

            When you wear it, you will
            transform into the shape you just
            were. When you remove it, you
            will return to normal.

            ---

            Green hat...
            Green clothes...

            */

            var curiosityShopMan1Item = _randomized.ItemList.First(io => io.NewLocation == Item.MaskKeaton);
            if (curiosityShopMan1Item.Item != Item.MaskKeaton)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0x29E2)
                    .Message(it =>
                    {
                        it.Text("Now Kafei...").NewLine()
                        .Text("I've known him since he was real").NewLine()
                        .Text("little...")
                        .EndTextBox()
                        .Text("But when he showed up looking all").NewLine()
                        .Text("young in that little ").Red("brat body").Text(",").NewLine()
                        .Text("I didn't know what I was seeing!").NewLine()
                        .Text(" ")
                        .EndTextBox()
                        .RuntimeWrap(() =>
                        {
                            it.Text("All it took was one glance at the ")
                            .Red(() =>
                            {
                                it.RuntimeItemName(curiosityShopMan1Item.DisplayName(), curiosityShopMan1Item.NewLocation.Value);
                            })
                            .Text(" he was carrying for me to realize that I was looking at my old friend.")
                            ;
                        })
                        .EndTextBox()
                        .Text("I gave ").RuntimePronoun(curiosityShopMan1Item.DisplayItem, curiosityShopMan1Item.NewLocation.Value).Text(" to him a long time").NewLine()
                        .Text("ago when he was just li'l Kafei").NewLine()
                        .Text("Didn't know he kept ").RuntimePronoun(curiosityShopMan1Item.DisplayItem, curiosityShopMan1Item.NewLocation.Value).Text(" that well").NewLine()
                        .Text("for so long...")
                        .DisableTextSkip2()
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            // Update Keg Challenge
            var kegChallengeItem = _randomized.ItemList.First(io => io.NewLocation == Item.ItemPowderKeg);
            if (kegChallengeItem.Item != Item.ItemPowderKeg)
            {
                newMessages.Add(new MessageEntryBuilder()
                    .Id(0xDEE)
                    .Message(it =>
                    {
                        it.RuntimeWrap(() =>
                        {
                            it.Text("There's a Goron in this village who sells ")
                            .RuntimeArticle(kegChallengeItem.DisplayItem, kegChallengeItem.NewLocation.Value)
                            .Red(() =>
                            {
                                it.RuntimeItemName(kegChallengeItem.AlternateName(), kegChallengeItem.NewLocation.Value);
                            })
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

        private void WriteDungeonItemText(List<MessageEntry> newMessages)
        {
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

            var dungeonLocations = new string[]
            {
                _randomized.ItemList[Item.AreaWoodFallTempleAccess].NewLocation?.Entrance(),
                _randomized.ItemList[Item.AreaSnowheadTempleAccess].NewLocation?.Entrance(),
                _randomized.ItemList[Item.AreaGreatBayTempleAccess].NewLocation?.Entrance(),
                _randomized.ItemList[Item.AreaInvertedStoneTowerTempleAccess].NewLocation?.Entrance(),
            };

            var dungeonBosses = new string[]
            {
                _randomized.ItemList.FirstOrDefault(io => io.NewLocation == Item.AreaOdolwasLair)?.Item.Entrance(),
                _randomized.ItemList.FirstOrDefault(io => io.NewLocation == Item.AreaGohtsLair)?.Item.Entrance(),
                _randomized.ItemList.FirstOrDefault(io => io.NewLocation == Item.AreaGyorgsLair)?.Item.Entrance(),
                _randomized.ItemList.FirstOrDefault(io => io.NewLocation == Item.AreaTwinmoldsLair)?.Item.Entrance(),
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
                0x3C, 0x3D, 0x3E, 0x3F, 0x11
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
                        if (itemTypeIndex == 2 && _randomized.Settings.DungeonNavigationMode.HasFlag(DungeonNavigationMode.MapRevealsLocation) && dungeonLocations[dungeonIndex] != null)
                        {
                            it.PauseText(0x10).NewLine()
                            .CompileTimeWrap((wrapped) =>
                            {
                                wrapped.Text("The entrance is at ").Red(dungeonLocations[dungeonIndex]).Text(".");
                            });
                        }
                        else if (itemTypeIndex == 3 && _randomized.Settings.DungeonNavigationMode.HasFlag(DungeonNavigationMode.CompassRevealsBoss) && dungeonBosses[dungeonIndex] != null)
                        {
                            it.PauseText(0x10).NewLine()
                            .CompileTimeWrap((wrapped) =>
                            {
                                wrapped.Text("It points at ").Red(dungeonBosses[dungeonIndex]).Text(".");
                            });
                        }
                        else if (itemTypeIndex == 4)
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
        }

        private void WritePrices(MessageTable messageTable, List<MessageEntry> newMessages)
        {
            // TODO if costs randomized
            var messageCostRegex = new Regex("\\b[0-9]{1,3}( |\u0011|\u0010)Rupees?");
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
                    var oldMessage = messageTable.GetMessage(messageId);
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
                        return replacementIndex++ == costIndex ? $"{cost}{match.Groups[1].Value}Rupee{(cost != 1 && messageId != 1143 ? "s" : "")}" : match.Value;
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
        }

        private void WriteItems(MessageTable messageTable)
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
            var killBosses = new List<Item>
            {
                Item.OtherKillOdolwa,
                Item.OtherKillGoht,
                Item.OtherKillGyorg,
                Item.OtherKillTwinmold,
            };
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

                if (killBosses.Contains(item.Item))
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
                    bool itemIsUsed(int itemId, Stack<int> path)
                    {
                        if (path.Contains(itemId))
                        {
                            return false;
                        }
                        try
                        {
                            path.Push(itemId);
                            var usedBy = _randomized.Logic
                                .Where(il => !ItemUtils.IsLocationJunk((Item)il.ItemId, _randomized.Settings))
                                .Where(il => il.RequiredItemIds?.Contains(itemId) == true || il.ConditionalItemIds?.Any(c => c.Contains(itemId)) == true);
                            if (usedBy.Any(il => !il.IsFakeItem))
                            {
                                return true;
                            }

                            return usedBy.Any(il => itemIsUsed(il.ItemId, path));
                        }
                        finally
                        {
                            path.Pop();
                        }
                    }
                    if ((item.Item.Name().Contains("Bombchu") || item.Item.Name().Contains("Shield")) && itemIsUsed((int)item.Item, new Stack<int>()))
                    {
                        overrideChestType = item.Item.IsTemporary() ? ChestTypeAttribute.ChestType.SmallGold : ChestTypeAttribute.ChestType.LargeGold;
                    }
                    if (item.Item.Name().Contains("Compass") && _randomized.Settings.DungeonNavigationMode.HasFlag(DungeonNavigationMode.CompassRevealsBoss))
                    {
                        overrideChestType = ChestTypeAttribute.ChestType.LargeGold;
                    }
                    if (item.Item.Name().Contains("Map") && item.Item.ClassicCategory() == ClassicCategory.DungeonItems && _randomized.Settings.DungeonNavigationMode.HasFlag(DungeonNavigationMode.MapRevealsLocation))
                    {
                        overrideChestType = ChestTypeAttribute.ChestType.LargeGold;
                    }
                    if (_randomized.Settings.VictoryMode.HasFlag(VictoryMode.Hearts) && (item.Item.ItemCategory() == ItemCategory.PiecesOfHeart || item.Item.ItemCategory() == ItemCategory.HeartContainers))
                    {
                        overrideChestType = ChestTypeAttribute.ChestType.LargeGold;
                    }
                    if (_randomized.Settings.VictoryMode.HasFlag(VictoryMode.Fairies) && ItemUtils.DungeonStrayFairies().Contains(item.Item))
                    {
                        overrideChestType = ChestTypeAttribute.ChestType.LargeGold;
                    }
                    if (_randomized.Settings.VictoryMode.HasFlag(VictoryMode.SkullTokens) && item.Item.ItemCategory() == ItemCategory.SkulltulaTokens)
                    {
                        overrideChestType = ChestTypeAttribute.ChestType.LargeGold;
                    }
                    ItemSwapUtils.WriteNewItem(item, newMessages, _randomized.Settings, item.Mimic?.ChestType ?? overrideChestType, messageTable, _extendedObjects);
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

            _randomized.Settings.AsmOptions.MMRConfig.LocationLullaby = GetLocationIdOfItem(Item.SongLullaby);
            _randomized.Settings.AsmOptions.MMRConfig.LocationLullabyIntro = GetLocationIdOfItem(Item.SongLullabyIntro);

            if (_randomized.Settings.UpdateShopAppearance)
            {
                WriteShopText(messageTable, newMessages);
            }

            // Update messages to match updated world models.
            if (_randomized.Settings.UpdateWorldModels)
            {
                WriteWorldModelText(newMessages);
            }

            WriteMiscItemText(newMessages);

            var itemsWithCustomMessage = new List<Item>
            {
                Item.CollectableIkanaGraveyardDay2Bats1,
                Item.FrogWoodfallTemple,
                Item.FrogGreatBayTemple,
                Item.FrogSwamp,
                Item.FrogLaundryPool,
            };
            foreach (var item in itemsWithCustomMessage)
            {
                var messageAttribute = item.GetAttribute<ExclusiveItemMessageAttribute>();
                var entry = new MessageEntry(
                    messageAttribute.Id,
                    messageAttribute.Message);
                _extraMessages.Add(entry);
            }

            if (_randomized.Settings.OathHint)
            {
                WriteOathHintText(newMessages);
            }

            if (_randomized.Settings.RemainsHint)
            {
                WriteRemainsHintText(newMessages);
            }

            if (_randomized.Settings.FairyAndSkullHint)
            {
                WriteFairyAndSkullHintText(newMessages);
            }

            if (_randomized.Settings.UpdateNPCText)
            {
                WriteNPCHintText(newMessages);
            }

            WriteDungeonItemText(newMessages);

            WritePrices(messageTable, newMessages);

            messageTable.UpdateMessages(newMessages);

            ResourceUtils.ApplyHack(Resources.mods.fix_shop_curiosity_bigbombbag);
        }

        private void WriteGossipQuotes(MessageTable messageTable)
        {
            if (_randomized.Settings.LogicMode == LogicMode.Vanilla)
            {
                return;
            }

            if (_randomized.Settings.FreeHints)
            {
                ResourceUtils.ApplyHack(Resources.mods.gossip_hints_free);
            }

            if (_randomized.Settings.TolerantGossipStones)
            {
                ResourceUtils.ApplyHack(Resources.mods.gossip_hints_tolerant);
            }

            if (_randomized.Settings.FreeGaroHints)
            {
                ResourceUtils.ApplyHack(Resources.mods.free_garo_hints);

                messageTable.UpdateMessages(new MessageEntryBuilder()
                    .Id(0x24E)
                    .Message(it =>
                    {
                        it.LightBlue(() =>
                        {
                            it.Text("I can't see it, but I sense there's").NewLine()
                            .Text("a thirst for blood looming all").NewLine()
                            .Text("around us...");
                        })
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }

            if (_randomized.Settings.GossipHintStyle != GossipHintStyle.Default)
            {
                messageTable.UpdateMessages(_randomized.GossipQuotes);
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
            /// in vanilla, if you trigger the crash screen you have to know the secret button combo
            /// to bypass the red bar and to see debug info
            /// here we bypass this to allow players to see the debug info and post it for us to help fix

            var bootFile     = RomData.MMFileList[1].Data;
            var codeFileData = RomData.MMFileList[31].Data;

            // when you first enter the crash debugger you are greeted by red bar on black screen:
            //   this means the crash debugger is waiting for a secret code to actually start showing data (Fault_WaitForButtonCombo)
            // we want to avoid this dumb password, so we skip over the code that calls it
            // but we also have to tell the fault to not auto-scroll, since that bugs me, you can still turn it back on with Gamepad-L
            //  vanilla: 
            // 003998 800839F8 91CF07CF /  lbu         $t7, 0x7CF($t6)
            // 00399C 800839FC 11E00005 /  beqz        $t7, .L80083A14
            // 0039A0 80083A00 00000000 /   nop
            //  replacement:
            // 003998 800839F8 240F0000 /  addiu       $t7 $zero $zero  # load 0 into t7
            // 00399C 800839FC 1000000A /  b           0xA              # we want to skip the rest of the code
            // 0039A0 80083A00 A1CF07CF /   sb         $t7, 0x7CF($t6)  # sFaultContext->autoscroll = 0 (false)
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x3998, 0x240F0000);
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x399C, 0x1000000A);
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x39A0, 0xA1CF07CF);

            // a few lines later, autoscroll is set to ON: remove
            // we don't branch past it because its buried in a jal delay slot for the color setting functions, which we want
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x39D0, 0x00000000); // disable autoscroll set to ON

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

            // the "H" character after each hex value for registers (H for base 16 'hex' ?)
            //   is hard for me to read, my dumb brain keeps thinking its a valid digit and seeing 9 digits
            // so I want to remove it from all of the hex values:
            // starts RAM 80098648, on rom it starts on 0x19640, within boot file its 0x185E0
            SwapBytes(0x185E0, 0x18720, (byte) 'H', (byte) ' '); // general registers
            SwapBytes(0x18760, 0x188D0, (byte) 'H', (byte) ' '); // floating point registers

            // convert lower case hex values to upper case (eg 8000abcd to 8000ABCD)
            // they used c printf string substitution, so just change %x to %X (8 byte wide though, "%08x")
            SwapBytes(0x181E0, 0x18230, (byte) 'x', (byte) 'X'); // dma section
            SwapBytes(0x18470, 0x18B04, (byte) 'x', (byte) 'X'); // the rest of hex values for the whole debug crasher

            // show V-PC (VRAM function address) for overlays in the stack trace, which identifies the actor it belongs to
            // this turns the stack trace into a version that is shown LAST, why they show two, one with question marks and one with data?
            //  probably does something useful in debug rom, but the questionmarks is useless to us, so switch to useful version
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x31D8, 0x00000000); // replace branch with nop

            // again, convert lower case hex to upper case, this time for the actor overlay table printers
            //  but also, these tables have other problems, so multiple changes:

            // change the table labels so "Name", the unused field of filenames, is removed and tiny "cn" is replaced with Num
            // also "No." changed to A.ID, as "No." could be confused for count, but its actorId
            byte[] newTableLabelString = Encoding.ASCII.GetBytes("A.Id RAMStart -   RAMEnd Num."); // replaces "No. RamStart- RamEnd cn  Name\n"
            var newTableSubString = Encoding.ASCII.GetBytes("%4d %08X - %08X %4d "); // replaces "%3d %08x-%08x %3d %s\n"
            ReadWriteUtils.Arr_Insert(newTableLabelString, 0, newTableLabelString.Length, codeFileData, 0x137104);
            ReadWriteUtils.Arr_Insert(newTableSubString,   0, newTableSubString.Length,   codeFileData, 0x137124);

            // for some reason, the second table is even worse:
            //   it shows both actor category and actor group, the two should only be different
            //   if the actor has chosen to change its category after init... useless information 99% of the time
            // we can change the second one to params with only two lines of code changed
            //  lbu  t6,2(s0)   ->  lhu  t6,0x1C(s0)
            codeFileData[0xE088] = 0x96; // change from load signed byte to load unsigned short
            codeFileData[0xE08B] = 0x1C; // change offset from actor category to params
            codeFileData[0xE0B0] = 0x96; // repeated for delay slot duplicate found later
            codeFileData[0xE0B3] = 0x1C;

            // again, convert lower case hex to upper case, this time for the actor struct table printers
            newTableLabelString = Encoding.ASCII.GetBytes("A.Grp  RAM       A.Id Params "); // replaces "No. Actor   Name Part SegName"
            //newTableSubString = Encoding.ASCII.GetBytes("%5d  %08X  %4X %04X "); // replaces "%3d %08x %04x %3d %s\n"
            newTableSubString = Encoding.ASCII.GetBytes("%5d  %08X  %4X %04X "); // replaces "%3d %08x %04x %3d %s\n"
            ReadWriteUtils.Arr_Insert(newTableLabelString, 0, newTableLabelString.Length, codeFileData, 0x136F18);
            ReadWriteUtils.Arr_Insert(newTableSubString, 0, newTableSubString.Length, codeFileData, 0x136F38);


            // they set the text padding of this text to -2 so they could show filenames, but retail rom doesnt show those anyway
            // changing it back to zero requires changing one instruction
            ReadWriteUtils.Arr_WriteU32(codeFileData, 0x19F00, 0x00002725); //  li a0, -2   ->   or a0, $ZERO
            ReadWriteUtils.Arr_WriteU32(codeFileData, 0xE034,  0x00002725); //  li a0, -2   ->   or a0, $ZERO

            // the stack dump by default shows us code that crashed around PC...
            // but I think there are better ways to see that since our code is not self-modifying
            // this should load SP instead of PC into the default address
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x2DA4, 0x00A0B025); // move  $s6, $a0   ->   move  $s6, $a1

            // now that the first stack trace shows us what we want, the second one is redundant and meaningless: remove
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x3A38, 0x00000000); // jal to Fault_DrawStackTrace -> NOP
            // and after that second stack trace there is an extra wait, have to press two A buttons to get through it so remove
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x3A4C, 0x00000000); // jal to Fault_WaitForInput -> NOP

            // the stack dump page, if you get there without autoscroll, resets the pos offset with the A button... but it seems broken
            //  instead, A should just leave the stack dump, we can re-set to SP or PC from C buttons
            ReadWriteUtils.Arr_WriteU32(bootFile, 0x2EBC, 0x10000022); // branch all the way down to the function exit

            // todo: figure out if we can make the screenshots say they are screen shots at the top
            // todo: can we add a symbol to the screen if autoscroll is turned on
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

        public void OutputHashJson(IEnumerable<byte> iconIndices, string filename)
        {
            var iconNames = new Dictionary<byte, string>
            {
                { 0, "ITEM_OCARINA" },
                { 1, "ITEM_BOW" },
                { 2, "ITEM_FIRE_ARROW" },
                { 3, "ITEM_ICE_ARROW" },
                { 4, "ITEM_LIGHT_ARROW" },
                { 6, "ITEM_BOMB" },
                { 7, "ITEM_BOMBCHU" },
                { 8, "ITEM_DEKU_STICK" },
                { 9, "ITEM_DEKU_NUT" },
                { 10, "ITEM_MAGIC_BEAN" },
                { 12, "ITEM_POWDER_KEG" },
                { 13, "ITEM_PICTOGRAPH" },
                { 14, "ITEM_LENS" },
                { 15, "ITEM_HOOKSHOT" },
                { 16, "ITEM_FAIRY_SWORD" },
                { 18, "ITEM_EMPTY_BOTTLE" },
                { 23, "ITEM_DEKU_PRINCESS" },
                { 27, "ITEM_BUGS" },
                { 30, "ITEM_BIG_POE" },
                { 32, "ITEM_HOT_WATER" },
                { 33, "ITEM_ZORA_EGG" },
                { 34, "ITEM_GOLD_DUST_BOTTLE" },
                { 35, "ITEM_MUSHROOM" },
                { 36, "ITEM_SEAHORSE" },
                { 37, "ITEM_CHATEAU_ROMANI_BOTTLE" },
                { 40, "ITEM_MOON_TEAR" },
                { 41, "ITEM_TOWN_DEED" },
                { 45, "ITEM_ROOM_KEY" },
                { 46, "ITEM_MAMA_LETTER" },
                { 47, "ITEM_KAFEI_LETTER" },
                { 48, "ITEM_PENDANT" },
                { 49, "ITEM_MAP" },
                { 50, "ITEM_DEKU_MASK" },
                { 51, "ITEM_GORON_MASK" },
                { 52, "ITEM_ZORA_MASK" },
                { 53, "ITEM_FIERCE_DEITY_MASK" },
                { 54, "ITEM_MASK_OF_TRUTH" },
                { 55, "ITEM_KAFEI_MASK" },
                { 56, "ITEM_ALL_NIGHT_MASK" },
                { 57, "ITEM_BUNNY_HOOD" },
                { 58, "ITEM_KEATON_MASK" },
                { 59, "ITEM_GARO_MASK" },
                { 60, "ITEM_ROMANI_MASK" },
                { 61, "ITEM_CIRCUS_LEADER_MASK" },
                { 62, "ITEM_POSTMAN_HAT" },
                { 63, "ITEM_COUPLE_MASK" },
                { 64, "ITEM_GREAT_FAIRY_MASK" },
                { 65, "ITEM_GIBDO_MASK" },
                { 66, "ITEM_DON_GERO_MASK" },
                { 67, "ITEM_KAMARO_MASK" },
                { 68, "ITEM_CAPTAIN_HAT" },
                { 69, "ITEM_STONE_MASK" },
                { 70, "ITEM_BREMEN_MASK" },
                { 71, "ITEM_BLAST_MASK" },
                { 72, "ITEM_MASK_OF_SCENTS" },
                { 73, "ITEM_GIANT_MASK" },
                { 77, "ITEM_KOKIRI_SWORD" },
                { 79, "ITEM_GILDED_SWORD" },
                { 80, "ITEM_HELIX_SWORD" },
                { 81, "ITEM_HERO_SHIELD" },
                { 82, "ITEM_MIRROR_SHIELD" },
                { 84, "ITEM_QUIVER_40" },
                { 90, "ITEM_ADULT_WALLET" },
                { 97, "ITEM_BOMBERS_NOTEBOOK" },
            };
            var hashOutput = new HashOutputJson
            {
                Hash = iconIndices.Select(iconIndex => iconNames.GetValueOrDefault(iconIndex)).ToList()
            };
            File.WriteAllText(filename, JsonSerializer.Serialize(hashOutput));
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
            asm.ExtraMessages.AddMessage(_extraMessages.ToArray());
            if (_randomized.Settings.QuickTextEnabled)
            {
                asm.ExtraMessages.ApplyQuickText();
            }
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
                config.MagicOverride = ColorSelectionManager.MagicMeter.GetItems().FirstOrDefault(csi => csi.Name == _cosmeticSettings.MagicSelection)?.GetColors(random);
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

            var smithy1Item = _randomized.ItemList.First(io => io.NewLocation == Item.UpgradeRazorSword).DisplayItem;
            var smithy2Item = _randomized.ItemList.First(io => io.NewLocation == Item.UpgradeGildedSword).DisplayItem;

            var extended = _extendedObjects = ExtendedObjects.Create(smithy1Item, smithy2Item, addFairies, addSkulltulas, _randomized.Settings.ProgressiveUpgrades);

            _randomized.Settings.AsmOptions.MiscConfig.Smithy.Models = extended.SmithyModels;
        }

        /// <summary>
        /// Write data related to traps to ROM.
        /// </summary>
        public void WriteTraps()
        {
            // Add mimic graphic to graphic overrides table.
            foreach (var item in _randomized.Traps)
            {
                var newLocation = item.NewLocation.Value;
                if (newLocation.IsModelVisible(_randomized.Settings) && (item.Item != Item.Rupoor || newLocation.IsShopModelVisible()) && (item.Item != Item.Nothing))
                {
                    var giIndex = item.NewLocation.Value.GetItemIndex().Value;
                    var graphic = item.Mimic.ResolveGraphic();
                    _graphicOverrides.Add(giIndex, graphic);
                }
            }

            // Add "You are a FOOL!" message to extra messages table.
            _extraMessages.Add(new MessageEntry(Item.IceTrap.ExclusiveItemEntry().Message, Item.IceTrap.ExclusiveItemMessage()));
            _extraMessages.Add(new MessageEntry(Item.BombTrap.ExclusiveItemEntry().Message, Item.BombTrap.ExclusiveItemMessage()));
            _extraMessages.Add(new MessageEntry(Item.Rupoor.ExclusiveItemEntry().Message, Item.Rupoor.ExclusiveItemMessage()));
            _extraMessages.Add(new MessageEntry(Item.Nothing.ExclusiveItemEntry().Message, Item.Nothing.ExclusiveItemMessage()));
        }

        public void MakeROM(OutputSettings outputSettings, IProgressReporter progressReporter)
        {
            using (BinaryReader OldROM = new BinaryReader(File.OpenRead(outputSettings.InputROMFilename)))
            {
                RomUtils.ReadFileTable(OldROM);
            }

            RomData.SceneList = null;

            var originalMMFileList = RomData.MMFileList.Select(file => file.Clone()).ToList();
            List<MMFile> cosmeticMMFileList;

            byte[] hash;
            AsmContext asm;
            if (!string.IsNullOrWhiteSpace(outputSettings.InputPatchFilename))
            {
                progressReporter.ReportProgress(50, "Applying patch...");
                hash = Patch.Patcher.ApplyPatch(outputSettings.InputPatchFilename);

                // Parse Symbols data from the ROM (specific MMFile)
                asm = AsmContext.LoadFromROM();

                cosmeticMMFileList = RomData.MMFileList.Select(file => file.Clone()).ToList();

                // Apply Asm configuration post-patch
                WriteAsmConfigPostPatch(asm, hash);
            }
            else
            {
                var messageTable = MessageTable.ReadDefault();

                progressReporter.ReportProgress(55, "Writing player model...");
                WritePlayerModel();

                if (_randomized.Settings.LogicMode != LogicMode.Vanilla)
                {
                    progressReporter.ReportProgress(60, "Applying hacks...");
                    ResourceUtils.ApplyHack(Resources.mods.title_screen);
                    WriteTitleScreen();
                    ResourceUtils.ApplyHack(Resources.mods.misc_changes);
                    WriteMiscText(messageTable);
                    ResourceUtils.ApplyHack(Resources.mods.cm_cs);
                    ResourceUtils.ApplyHack(Resources.mods.fix_song_of_healing);
                    WriteFileSelect();
                }
                ResourceUtils.ApplyHack(Resources.mods.init_file);
                ResourceUtils.ApplyHack(Resources.mods.fix_deku_drowning);
                ResourceUtils.ApplyHack(Resources.mods.fix_collectable_flags);
                ResourceUtils.ApplyHack(Resources.mods.fix_great_bay_clear_mikau);
                ResourceUtils.ApplyHack(Resources.mods.fix_deku_playground_softlock);

                // TODO: Move this to a helper function?
                if (_randomized.Settings.EnablePictoboxSubject)
                {
                    WritePictographPromptText(messageTable);

                    // NOP call to update pictobox flags after message prompt.
                    ReadWriteUtils.WriteCodeNOP(0x801127D0);
                }

                if (_randomized.Settings.ShortenCutsceneSettings.General.HasFlag(ShortenCutsceneGeneral.FasterBankText))
                {
                    WriteBankPromptText(messageTable);
                }

                // TODO? if respawn combo is enabled
                ushort newMessageId = 0x9002;
                _extraMessages.Add(new MessageEntryBuilder()
                    .Id(newMessageId)
                    .Message((it) =>
                    {
                        it.Text("Return to spawn?").NewLine()
                        .StartGreenText().Text(" ").NewLine()
                        .TwoChoices().Text("Yes").NewLine()
                        .Text("No")
                        .EndFinalTextBox();
                    })
                    .Build()
                );


                WriteArcheryDoubleRewardText(messageTable);
                WriteBankPostRewardText(messageTable);
                WriteRoyalWalletText(messageTable);
                WriteMoonChildDenialTextAndHack(messageTable);

                progressReporter.ReportProgress(61, "Writing quick text...");
                WriteQuickText();

                progressReporter.ReportProgress(62, "Writing dungeons...");
                WriteDungeons();

                progressReporter.ReportProgress(63, "Writing gimmicks...");
                WriteGimmicks(messageTable);

                progressReporter.ReportProgress(64, "Writing speedups...");
                WriteSpeedUps(messageTable);

                progressReporter.ReportProgress(65, "Writing enemies...");
                WriteEnemies();

                progressReporter.ReportProgress(66, "Writing items...");
                WriteItems(messageTable);
                WriteMiscHacks();

                progressReporter.ReportProgress(67, "Writing cutscenes...");
                WriteCutscenes(messageTable);

                progressReporter.ReportProgress(68, "Writing messages...");
                WriteGossipQuotes(messageTable);

                var messagesToTransfer = messageTable.RemoveMessagesToTransfer();
                _extraMessages.AddRange(messagesToTransfer);

                MessageTable.WriteDefault(messageTable, _randomized.Settings.QuickTextEnabled);

                progressReporter.ReportProgress(69, "Writing startup...");
                WriteStartupStrings();

                // Overwrite existing items with traps.
                if (_randomized.Settings.LogicMode != LogicMode.Vanilla && _randomized.Settings.TrapAmount != TrapAmount.None)
                {
                    progressReporter.ReportProgress(70, "Writing traps...");
                    WriteTraps();
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

                cosmeticMMFileList = RomData.MMFileList.Select(file => file.Clone()).ToList();

                // Write subset of Asm config post-patch
                WriteAsmConfig(asm, hash);

                if (_randomized.Settings.DrawHash || outputSettings.GeneratePatch)
                {
                    var iconStripIcons = asm.Symbols.ReadHashIconsTable();
                    var iconFileIndices = ImageUtils.GetIconIndices(hash).Select(index => iconStripIcons[index]);
                    var directory = Path.GetDirectoryName(outputSettings.OutputROMFilename);
                    var filename = Path.GetFileNameWithoutExtension(outputSettings.OutputROMFilename);
                    OutputHashIcons(iconFileIndices, Path.Combine(directory, filename + ".png"));

                    if (outputSettings.GenerateHashJson)
                    {
                        OutputHashJson(iconFileIndices, Path.Combine(directory, filename + "_Hash.json"));
                    }
                }
            }

            WriteMiscellaneousChanges();

            progressReporter.ReportProgress(72, "Writing cosmetics...");
            WriteTatlColour(new Random(BitConverter.ToInt32(hash, 0)));
            WriteInstruments(new Random(BitConverter.ToInt32(hash, 0)));

            progressReporter.ReportProgress(73, "Writing sound effects...");
            WriteSoundEffects(new Random(BitConverter.ToInt32(hash, 0)));
            WriteLowHealthSound(new Random(BitConverter.ToInt32(hash, 0)));

            // Music conversion process
            if (Directory.Exists(Values.MusicDirectory))
            {
                // Check for old music files
                progressReporter.ReportProgress(74, "Checking for old music files...");
                MusicConversionUtils.CheckForOldFiles(Values.MusicDirectory);

                // Backup and convert if any old music files were found
                if (MusicConversionUtils.OLD_MUSIC_FILES.Any())
                {
                    progressReporter.ReportProgress(75, "Backing up music folder to 'music.old'...");
                    MusicConversionUtils.BackupMusicFolder(Values.MusicDirectory);
                    
                    progressReporter.ReportProgress(76, "Converting old music files...");
                    MusicConversionUtils.ConvertMusicFiles();

                    // Clear the list for secondary checks during WriteAudioSeq()
                    // where the music files get searched again and injected
                    MusicConversionUtils.OLD_MUSIC_FILES.Clear();
                }
            }

            progressReporter.ReportProgress(77, "Writing music...");
            SequenceUtils.MoveAudioBankTable();
            WriteMuteMusic();
            WriteEnemyCombatMusicMute();
            WriteRemoveMinorMusic();
            WriteDisableFanfares();

            if (outputSettings.GenerateCosmeticsPatch)
            {
                var directory = Path.GetDirectoryName(outputSettings.OutputROMFilename);
                var filename = Path.GetFileNameWithoutExtension(outputSettings.OutputROMFilename);

                Patch.Patcher.CreatePatch(Path.Combine(directory, filename + "_Cosmetics.mmr"), cosmeticMMFileList);
            }

            WriteAudioSeq(new Random(BitConverter.ToInt32(hash, 0)), outputSettings);

            if (outputSettings.GenerateROM || outputSettings.OutputVC)
            {
                progressReporter.ReportProgress(80, "Building ROM...");

                if (outputSettings.GenerateROM)
                {
                    byte[] ROM = RomUtils.BuildROM();
                    if (ROM.Length > 0x4000000) // over 64mb
                    {
                        throw new ROMOverflowException("64 MB", "hardware (Everdrive)");
                    }
                    progressReporter.ReportProgress(85, "Writing ROM...");
                    RomUtils.WriteROM(outputSettings.OutputROMFilename, ROM);
                }

                if (outputSettings.OutputVC)
                {
                    var smithyFiles = new List<int> { 958 };
                    var extObjectsFileTableAddr = (int)asm.Symbols["EXT_OBJECTS"];
                    var extObjectsFileAddr = ReadWriteUtils.ReadU32(extObjectsFileTableAddr + 8);
                    if (extObjectsFileAddr > 0)
                    {
                        var extObjectsFile = RomUtils.GetFileIndexForWriting((int)extObjectsFileAddr);
                        smithyFiles.Add(extObjectsFile);
                    }
                    foreach (var file in smithyFiles)
                    {
                        RomUtils.CheckCompressed(file);
                        RomData.MMFileList[file].Data = RomData.MMFileList[file].Data
                            .FindAndReplace(
                                new byte[] { 0xFC, 0x27, 0x2C, 0x40, 0x21, 0x0E, 0x92, 0xFF },
                                new byte[] { 0xFC, 0x27, 0x2C, 0x03, 0x21, 0x0C, 0x92, 0xFF }
                            );
                    }

                    byte[] ROM = RomUtils.BuildROM();
                    if (ROM.Length > 0x2800000) // Over 40MB. The upper limit is likely 48MB, but let's stick with 40 for now.
                    {
                        throw new ROMOverflowException("40 MB", "WiiVC");
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
