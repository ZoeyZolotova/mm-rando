using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MMRando.Models
{
    public class Settings : GenerationSettings
    {
        #region General settings

        /// <summary>
        /// Checks if a game needs to be built
        /// </summary>
        public bool OutputGame => OutputN64ROM || OutputVC;

        /// <summary>
        /// Checks if a spoiler needs to be generated
        /// </summary>
        public bool OutputSpoiler => OutputTextSpoiler || OutputHTMLSpoiler;

        /// <summary>
        ///  Outputs n64 rom if true (default: true)
        /// </summary>
        public bool OutputN64ROM
        {
            get => outputN64ROM;
            set => SetField(ref outputN64ROM, value);
        }
        private bool outputN64ROM = true;


        /// <summary>
        ///  Outputs virtual channel if true
        /// </summary>
        public bool OutputVC
        {
            get => outputVC;
            set => SetField(ref outputVC, value);
        }
        private bool outputVC;

        /// <summary>
        /// Filepath to the input ROM
        /// </summary>
        public string InputROMFilename
        {
            get => inputROMFilename;
            set => SetField(ref inputROMFilename, value);
        }
        private string inputROMFilename;

        /// <summary>
        /// Filepath to the input patch file
        /// </summary>
        public string InputPatchFilename
        {
            get => inputPatchFilename;
            set => SetField(ref inputPatchFilename, value);
        }
        private string inputPatchFilename;

        /// <summary>
        /// Filepath to the input logic file
        /// </summary>
        public string UserLogicFileName
        {
            get => userLogicFileName;
            set => SetField(ref userLogicFileName, value);
        }
        private string userLogicFileName;

        /// <summary>
        /// Default Filename for the output ROM
        /// </summary>
        public string DefaultOutputROMFilename
        {
            get
            {
                return $"MMR_{Seed}.z64";
            }
        }

        /// <summary>
        /// Filepath to the output ROM
        /// </summary>
        public string OutputROMFilename
        {
            get => outputROMFilename;
            set => SetField(ref outputROMFilename, value);
        }
        private string outputROMFilename;

        /// <summary>
        /// Generate spoiler log on randomizing
        /// </summary>
        public bool OutputTextSpoiler
        {
            get => outputTextSpoiler;
            set
            {
                SetField(ref outputTextSpoiler, value);
            }
        }
        private bool outputTextSpoiler;

        /// <summary>
        /// Generate HTML spoiler log on randomizing
        /// </summary>
        public bool OutputHTMLSpoiler
        {
            get => outputHTMLSpoiler;
            set => SetField(ref outputHTMLSpoiler, value);
        }
        private bool outputHTMLSpoiler = true;

        /// <summary>
        /// Generate patch file
        /// </summary>
        public bool OutputROMPatch
        {
            get => outputROMPatch;
            set => SetField(ref outputROMPatch, value);
        }
        private bool outputROMPatch;

        /// <summary>
        /// Apply patch file during output generation
        /// </summary>
        public bool ApplyPatch
        {
            get => applyPatch;
            set => SetField(ref applyPatch, value);
        }
        private bool applyPatch;

        /// <summary>
        /// Checks if the InputROM is needed for generation
        /// </summary>
        public bool NeedInputROM => ApplyPatch || OutputGame || OutputROMPatch;

        /// <summary>
        /// Stores the byte order of the rom
        /// </summary>
        public ValidateRomResult InputROMFormat
        {
            get => inputROMFormat;
            set => SetField(ref inputROMFormat, value);
        }
        private ValidateRomResult inputROMFormat;

        #endregion

        #region Random Elements

        /// <summary>
        /// Randomize background music (includes bgm from other video games)
        /// </summary>
        public bool RandomizeBGM
        {
            get => randomizeBGM;
            set => SetField(ref randomizeBGM, value);
        }
        private bool randomizeBGM;

        /// <summary>
        /// Mute background music
        /// </summary>
        public bool NoBGM
        {
            get => noBGM;
            set => SetField(ref noBGM, value);
        }
        private bool noBGM;

        /// <summary>
        /// Prevent downgrades
        /// </summary>
        public bool PreventDowngrades
        {
            get => preventDowngrades;
            set => SetField(ref preventDowngrades, value);
        }
        private bool preventDowngrades = true;

        /// <summary>
        /// Updates shop models and text
        /// </summary>
        public bool UpdateShopAppearance
        {
            get => updateShopAppearance;
            set => SetField(ref updateShopAppearance, value);
        }
        private bool updateShopAppearance = true;

        /// <summary>
        /// Updates chest appearance to match contents
        /// </summary>
        public bool UpdateChests
        { 
            get => updateChests;
            set => SetField(ref updateChests, value);
        }
        private bool updateChests = true;

        /// <summary>
        /// Change epona B button behavior to prevent player losing sword if they don't have a bow.
        /// </summary>
        public bool FixEponaSword
        {
            get => fixEponaSword;
            set => SetField(ref fixEponaSword, value);
        }
        private bool fixEponaSword = true;

        #endregion

        #region Gimmicks

        /// <summary>
        /// Modifies the damage value when Link is damaged
        /// </summary>
        public DamageMode DamageMode
        {
            get => damageMode;
            set => SetField(ref damageMode, value);
        }
        private DamageMode damageMode = DamageMode.Default;

        /// <summary>
        /// Adds an additional effect when Link is damaged
        /// </summary>
        public DamageEffect DamageEffect { get => damageEffect; set => SetField(ref damageEffect, value); }
        private DamageEffect damageEffect = DamageEffect.Default;

        /// <summary>
        /// Modifies Link's movement
        /// </summary>
        public MovementMode MovementMode { get => movementMode; set => SetField(ref movementMode, value); }
        private MovementMode movementMode = MovementMode.Default;

        /// <summary>
        /// Sets the type of floor globally
        /// </summary>
        public FloorType FloorType { get => floorType; set => SetField(ref floorType, value); }
        private FloorType floorType = FloorType.Default;

        /// <summary>
        /// Sets the clock speed.
        /// </summary>
        public ClockSpeed ClockSpeed { get => clockSpeed; set => SetField(ref clockSpeed, value); }
        private ClockSpeed clockSpeed = ClockSpeed.Default;

        /// <summary>
        /// Hides the clock UI.
        /// </summary>
        public bool HideClock { get => hideClock; set => SetField(ref hideClock, value); }
        private bool hideClock;

        #endregion

        #region Comfort / Cosmetics

        /// <summary>
        /// Certain cutscenes will play shorter, or will be skipped
        /// </summary>
        public bool ShortenCutscenes { get => shortenCutscenes; set => SetField(ref shortenCutscenes, value); }
        private bool shortenCutscenes = true;

        /// <summary>
        /// Text is fast-forwarded
        /// </summary>
        public bool QuickTextEnabled { get => quickTextEnabled; set => SetField(ref quickTextEnabled, value); }
        private bool quickTextEnabled = true;

        /// <summary>
        /// The color of Link's tunic
        /// </summary>
        public Color TunicColor { get => tunicColor; set => SetField(ref tunicColor, value); }
        private Color tunicColor = Color.FromArgb(0x1E, 0x69, 0x1B);

        /// <summary>
        /// Replaces Link's default model
        /// </summary>
        public Character Character { get => character; set => SetField(ref character, value); }
        private Character character = Character.LinkMM;

        /// <summary>
        /// Replaces Tatl's colors
        /// </summary>
        public TatlColorSchema TatlColorSchema { get => tatlColorSchema; set => SetField(ref tatlColorSchema, value); }
        private TatlColorSchema tatlColorSchema = TatlColorSchema.Default;


        /// <summary>
        ///  Custom item list selections
        /// </summary>
        public List<int> CustomItemList
        {
            get => customItemList;
            set => SetField(ref customItemList, value);
        }
        private List<int> customItemList = new List<int>();


        [JsonIgnore]
        /// <summary>
        ///  Custom item list string
        /// </summary>
        public string CustomItemListString
        {
            get => customItemListString;
            set => SetField(ref customItemListString, value);
        }
        private string customItemListString;

        #endregion


        public string GetGenerationSettings()
        {
            string settings = JsonConvert.SerializeObject(this);
            var settingsObj = JsonConvert.DeserializeObject<GenerationSettings>
                (settings);
            return JsonConvert.SerializeObject(settingsObj);
        }

        public byte[] GetGenerationSettingsHash()
        {
            string field = GetGenerationSettings();
            if (!OutputSpoiler)
            {
                field += "bad salt by mzxrules";
            }

            byte[] hash;
            using (var sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(field));
            }
            return hash;
        }

        public override string ToString()
        {
            var hash = GetGenerationSettingsHash();
            return string.Concat(hash.Select(x => x.ToString("X2")));
        }

        public static Settings LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return new Settings();
            }

            var json = File.ReadAllText(filename);
            var jsonSettings = new JsonSerializerSettings
            {
                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                {
                    System.Diagnostics.Debug.WriteLine(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                },
            };
            return JsonConvert.DeserializeObject<Settings>(json, jsonSettings);
        }
    }
}
