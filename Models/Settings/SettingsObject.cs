
using MMRando.Constants;
using MMRando.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MMRando.Models.Settings
{

    public class SettingsObject
    {
        #region General settings

        /// <summary>
        ///  Outputs n64 rom if true (default: true)
        /// </summary>
        public bool GenerateROM { get; set; } = true;

        /// <summary>
        ///  Outputs virtual channel if true
        /// </summary>
        public bool OutputVC { get; set; }

        /// <summary>
        /// Filepath to the input ROM
        /// </summary>
        public string InputROMFilename { get; set; }

        /// <summary>
        /// Filepath to the input patch file
        /// </summary>
        public string InputPatchFilename { get; set; }

        /// <summary>
        /// Filepath to the input logic file
        /// </summary>
        public string UserLogicFileName { get; set; }

        /// <summary>
        /// Default Filename for the output ROM
        /// </summary>
        public string DefaultOutputROMFilename
        {
            get
            {
                string settings = this.ToString();
                string appendSeed = GenerateSpoilerLog ? $"{Seed}_" : "";
                string filename = $"MMR_{appendSeed}{settings}";

                return filename + ".z64";
            }
        }

        /// <summary>
        /// Filepath to the output ROM
        /// </summary>
        public string OutputROMFilename { get; set; }

        /// <summary>
        /// Generate spoiler log on randomizing
        /// </summary>
        public bool GenerateSpoilerLog { get; set; }

        /// <summary>
        /// Generate HTML spoiler log on randomizing
        /// </summary>
        public bool GenerateHTMLLog { get; set; }

        /// <summary>
        /// Generate spoiler log only on randomizing
        /// </summary>
        public bool LogOnly { get; set; }

        /// <summary>
        /// Use Custom Item list for the logic.
        /// </summary>
        public bool UseCustomItemList { get; set; }

        /// <summary>
        /// Generate patch file
        /// </summary>
        public bool GeneratePatch { get; set; }

        #endregion

        #region Random Elements

        private int _seed;

        /// <summary>
        /// The randomizer seed
        /// </summary>
        public int Seed
        {
            get => _seed;
            set
            {
                _seed = value;
            }
        }

        /// <summary>
        /// Selected mode of logic (affects randomization rules)
        /// </summary>
        public LogicMode LogicMode { get; set; }

        /// <summary>
        /// Add songs to the randomization pool
        /// </summary>
        public bool AddSongs { get; set; }

        /// <summary>
        /// (KeySanity) Add dungeon items (maps, compasses, keys) to the randomization pool
        /// </summary>
        public bool AddDungeonItems { get; set; }

        /// <summary>
        /// Add shop items to the randomization pool
        /// </summary>
        public bool AddShopItems { get; set; }

        /// <summary>
        /// Add moon items to the randomization pool
        /// </summary>
        public bool AddMoonItems { get; set; }

        /// <summary>
        /// Add great fairy rewards to the randomization pool
        /// </summary>
        public bool AddFairyRewards { get; set; }

        /// <summary>
        /// Add everything else to the randomization pool
        /// </summary>
        public bool AddOther { get; set; }

        /// <summary>
        /// Add pre-clocktown nut chest to the randomization pool
        /// </summary>
        public bool AddNutChest { get; set; }

        /// <summary>
        /// Add starting sword/shield/heart containers to the randomization pool
        /// </summary>
        public bool CrazyStartingItems { get; set; }

        /// <summary>
        /// Randomize the content of a bottle when catching (e.g. catching a fairy puts poe in bottle)
        /// </summary>
        public bool RandomizeBottleCatchContents { get; set; }

        /// <summary>
        /// Exclude song of soaring from randomization (it will be found in vanilla location)
        /// </summary>
        public bool ExcludeSongOfSoaring { get; set; }

        /// <summary>
        /// Randomize which dungeon you appear in when entering one
        /// </summary>
        public bool RandomizeDungeonEntrances { get; set; }

        /// <summary>
        /// (Beta) Randomize enemies
        /// </summary>
        public bool RandomizeEnemies { get; set; }

        /// <summary>
        /// Prevents player starting with any items that are randomized.
        /// </summary>
        public bool NoStartingItems { get; set; }


        /// <summary>
        ///  Custom item list selections
        /// </summary>
        public List<int> CustomItemList { get; set; } = new List<int>();

        /// <summary>
        ///  Custom item list string
        /// </summary>
        public string CustomItemListString { get; set; }

        #endregion

        #region Gimmicks

        /// <summary>
        /// Modifies the damage value when Link is damaged
        /// </summary>
        public DamageMode DamageMode { get; set; }

        /// <summary>
        /// Adds an additional effect when Link is damaged
        /// </summary>
        public DamageEffect DamageEffect { get; set; }

        /// <summary>
        /// Modifies Link's movement
        /// </summary>
        public MovementMode MovementMode { get; set; }

        /// <summary>
        /// Sets the type of floor globally
        /// </summary>
        public FloorType FloorType { get; set; }

        /// <summary>
        /// Sets the clock speed.
        /// </summary>
        public ClockSpeed ClockSpeed { get; set; } = ClockSpeed.Default;

        /// <summary>
        /// Hides the clock UI.
        /// </summary>
        public bool HideClock { get; set; }

        #endregion

        #region Comfort / Cosmetics

        /// <summary>
        /// Certain cutscenes will play shorter, or will be skipped
        /// </summary>
        public bool ShortenCutscenes { get; set; }

        /// <summary>
        /// Text is fast-forwarded
        /// </summary>
        public bool QuickTextEnabled { get; set; }

        /// <summary>
        /// The color of Link's tunic
        /// </summary>
        public Color TunicColor { get; set; }

        /// <summary>
        /// Replaces Link's default model
        /// </summary>
        public Character Character { get; set; }

        /// <summary>
        /// Replaces Tatl's colors
        /// </summary>
        public TatlColorSchema TatlColorSchema { get; set; }

        /// <summary>
        /// Method to write the gossip stone hints.
        /// </summary>
        public GossipHintStyle GossipHintStyle { get; set; }

        /// <summary>
        /// Randomize background music (includes bgm from other video games)
        /// </summary>
        public bool RandomizeBGM { get; set; }

        /// <summary>
        /// Mute background music
        /// </summary>
        public bool NoBGM { get; set; }

        /// <summary>
        /// FrEe HiNtS FoR WeNiEs
        /// </summary>
        public bool FreeHints { get; set; }

        /// <summary>
        /// Clear hints
        /// </summary>
        public bool ClearHints { get; set; }

        /// <summary>
        /// Prevent downgrades
        /// </summary>
        public bool PreventDowngrades { get; set; }

        /// <summary>
        /// Updates shop models and text
        /// </summary>
        public bool UpdateShopAppearance { get; set; }

        /// <summary>
        /// Updates chest appearance to match contents
        /// </summary>
        public bool UpdateChests { get; set; }

        /// <summary>
        /// Change epona B button behavior to prevent player losing sword if they don't have a bow.
        /// </summary>
        public bool FixEponaSword { get; set; }

        #endregion

        // Functions

        public void Update(string settings)
        {
            var parts = settings.Split('-')
                .Select(p => Base36Utils.Decode(p))
                .ToArray();

            if (parts.Any(p => p > int.MaxValue))
            {
                throw new ArgumentException(nameof(settings));
            }
            //xfe8z--16psr-
            int part1 = (int)parts[0];
            int part2 = (int)parts[1];
            int part3 = (int)parts[2];
            int part4 = (int)parts[3];

            UseCustomItemList = (part1 & 8192) > 0;

            if (UseCustomItemList)
            {
                CrazyStartingItems = false;
                AddNutChest = false;
                AddMoonItems = false;
                AddFairyRewards = false;
                AddOther = false;
                ExcludeSongOfSoaring = false;
                RandomizeBottleCatchContents = false;
                AddDungeonItems = false;
                AddShopItems = false;
            }
            else
            {
                AddFairyRewards = (part1 & 67108864) > 0;
                CrazyStartingItems = (part1 & 4194304) > 0;
                AddNutChest = (part1 & 2097152) > 0;
                AddMoonItems = (part1 & 32768) > 0;
                AddOther = (part1 & 4096) > 0;
                ExcludeSongOfSoaring = (part1 & 1024) > 0;
                RandomizeBottleCatchContents = (part1 & 128) > 0;
                AddDungeonItems = (part1 & 64) > 0;
                AddShopItems = (part1 & 32) > 0;
            }

            UpdateChests = (part1 & 33554432) > 0;
            FixEponaSword = (part1 & 16777216) > 0;
            NoStartingItems = (UseCustomItemList || AddOther) && (part1 & 8388608) > 0;
            UpdateShopAppearance = (part1 & 1048576) > 0;
            PreventDowngrades = (part1 & 524288) > 0;
            NoBGM = (part1 & 262144) > 0;
            HideClock = (part1 & 131072) > 0;
            ClearHints = (part1 & 65536) > 0;
            FreeHints = (part1 & 16384) > 0;
            // 8192 - UseCustomItemList, see above
            // 2048
            GenerateSpoilerLog = (part1 & 512) > 0;
            AddSongs = (part1 & 256) > 0;
            RandomizeDungeonEntrances = (part1 & 16) > 0;
            RandomizeBGM = (part1 & 8) > 0;
            RandomizeEnemies = (part1 & 4) > 0;
            ShortenCutscenes = (part1 & 2) > 0;
            QuickTextEnabled = (part1 & 1) > 0;

            var damageMultiplierIndex = (int)((part2 & 0xF0000000) >> 28);
            var damageTypeIndex = (part2 & 0xF000000) >> 24;
            var modeIndex = (part2 & 0xFF0000) >> 16;
            var characterIndex = (part2 & 0xFF00) >> 8;
            var tatlColorIndex = part2 & 0xFF;

            var gravityTypeIndex = (int)((part3 & 0xF0000000) >> 28);
            var floorTypeIndex = (part3 & 0xF000000) >> 24;
            var tunicColor = Color.FromArgb(
                (part3 & 0xFF0000) >> 16,
                (part3 & 0xFF00) >> 8,
                part3 & 0xFF);

            var clockSpeedIndex = (byte)(part4 & 0xFF);
            var gossipHintsIndex = (byte)((part4 & 0xFF00) >> 8);

            DamageMode = (DamageMode)damageMultiplierIndex;
            DamageEffect = (DamageEffect)damageTypeIndex;
            LogicMode = (LogicMode)modeIndex;
            Character = (Character)characterIndex;
            TatlColorSchema = (TatlColorSchema)tatlColorIndex;
            MovementMode = (MovementMode)gravityTypeIndex;
            FloorType = (FloorType)floorTypeIndex;
            TunicColor = tunicColor;
            ClockSpeed = (ClockSpeed)clockSpeedIndex;
            GossipHintStyle = (GossipHintStyle)gossipHintsIndex;

        }


        private int[] BuildSettingsBytes()
        {
            int[] parts = new int[4];

            if (UseCustomItemList)
            {
                parts[0] += 8192;
            }
            else
            {
                if (AddFairyRewards) { parts[0] += 67108864; }
                if (CrazyStartingItems) { parts[0] += 4194304; }
                if (AddNutChest) { parts[0] += 2097152; }
                if (AddMoonItems) { parts[0] += 32768; }
                if (AddOther) { parts[0] += 4096; }
                if (ExcludeSongOfSoaring) { parts[0] += 1024; }
                if (RandomizeBottleCatchContents) { parts[0] += 128; }
                if (AddDungeonItems) { parts[0] += 64; }
                if (AddShopItems) { parts[0] += 32; }
            }
            if (UpdateChests) { parts[0] += 33554432; }
            if (FixEponaSword) { parts[0] += 16777216; }
            if (NoStartingItems && (UseCustomItemList || AddOther)) { parts[0] += 8388608; }
            if (UpdateShopAppearance) { parts[0] += 1048576; }
            if (PreventDowngrades) { parts[0] += 524288; }
            if (NoBGM) { parts[0] += 262144; }
            if (HideClock) { parts[0] += 131072; };
            if (ClearHints) { parts[0] += 65536; };
            if (FreeHints) { parts[0] += 16384; };
            // 2048
            if (GenerateSpoilerLog) { parts[0] += 512; };
            if (AddSongs) { parts[0] += 256; };
            if (RandomizeDungeonEntrances) { parts[0] += 16; };
            if (RandomizeBGM) { parts[0] += 8; };
            if (RandomizeEnemies) { parts[0] += 4; };
            if (ShortenCutscenes) { parts[0] += 2; };
            if (QuickTextEnabled) { parts[0] += 1; };

            parts[1] = ((byte)LogicMode << 16)
                | ((byte)Character << 8)
                | ((byte)TatlColorSchema)
                | ((byte)DamageEffect << 24)
                    | ((byte)DamageMode << 28);

            parts[2] = (TunicColor.R << 16)
                | (TunicColor.G << 8)
                | (TunicColor.B)
                | ((byte)FloorType << 24)
                    | ((byte)MovementMode << 28);

            parts[3] = (byte) ClockSpeed
                | ((byte)GossipHintStyle << 8);

            return parts;
        }

        private string EncodeSettings()
        {
            var partsEncoded = BuildSettingsBytes()
                .Select(p => Base36Utils.Encode(p))
                .ToArray();

            return string.Join("-", partsEncoded);
        }

        public override string ToString()
        {
            return EncodeSettings();
        }
    }
}
