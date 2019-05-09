
using System.Drawing;

namespace MMRando.Models
{

    public class Settings
    {
        // TODO make base36-string from settings
        // TODO make settings from base36-string

        // General

        /// <summary>
        /// Filepath to the input ROM
        /// </summary>
        public string InputROMFilename { get; set; }

        /// <summary>
        /// Default Filename for the output ROM
        /// </summary>
        public string DefaultOutputROMFilename { get; set; }

        /// <summary>
        /// Filepath to the output ROM
        /// </summary>
        public string OutputROMFilename { get; set; }

        /// <summary>
        /// Generate spoiler log on randomizing
        /// </summary>
        public bool GenerateSpoilerLog { get; set; }

        /// <summary>
        /// Use Custom Item list for the logic.
        /// </summary>
        public bool UseCustomItemList { get; set; }


        // Random Elements

        /// <summary>
        /// The randomizer seed
        /// </summary>
        public int Seed { get; set; }

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
        /// Add everything else to the randomization pool
        /// </summary>
        public bool AddOther { get; set; }

        /// <summary>
        /// Randomize the content of a bottle when catching (e.g. catching a fairy puts poe in bottle)
        /// </summary>
        public bool RandomizeBottleCatchContents { get; set; }

        /// <summary>
        /// Exclude song of soaring from randomization (it will be found in vanilla location)
        /// </summary>
        public bool ExcludeSongOfSoaring { get; set; }

        /// <summary>
        /// Gossip stones give hints on where to find items, and sometimes junk
        /// </summary>
        public bool EnableGossipHints { get; set; }

        /// <summary>
        /// Randomize which dungeon you appear in when entering one
        /// </summary>
        public bool RandomizeDungeonEntrances { get; set; }

        /// <summary>
        /// (Beta) Randomize enemies
        /// </summary>
        public bool RandomizeEnemies { get; set; }

        /// <summary>
        /// Randomize background music (includes bgm from other video games)
        /// </summary>
        public bool RandomizeBGM { get; set; }

        /// <summary>
        /// FrEe HiNtS FoR WeNiEs
        /// </summary>
        public bool FreeHints { get; set; }



        // Gimmicks

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

        // Comfort / Cosmetics

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
    }
}
