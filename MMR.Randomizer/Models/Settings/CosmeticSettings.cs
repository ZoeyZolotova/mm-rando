using MMR.Randomizer.Asm;
using MMR.Randomizer.Models.Colors;
using Newtonsoft.Json;
using System.Drawing;

namespace MMR.Randomizer.Models.Settings
{

    public class CosmeticSettings
    {
        /// <summary>
        /// Options for the Asm <see cref="Patcher"/>.
        /// </summary>
        [JsonIgnore]
        public AsmOptionsCosmetic AsmOptions { get; set; } = new AsmOptionsCosmetic();

        /// <summary>
        /// Hearts color selection used for HUD color override.
        /// </summary>
        public string HeartsSelection { get; set; } = ColorSelectionManager.Hearts.GetItems()[0].Name;

        /// <summary>
        /// Magic color selection used for HUD color override.
        /// </summary>
        public string MagicSelection { get; set; } = ColorSelectionManager.MagicMeter.GetItems()[0].Name;

        /// <summary>
        /// Randomize sound effects
        /// </summary>
        public bool RandomizeSounds { get; set; }

        /// <summary>
        /// The color of Link's tunic
        /// </summary>
        public Color TunicColor { get; set; } = Color.FromArgb(0x1E, 0x69, 0x1B);

        /// <summary>
        /// Replaces Tatl's colors
        /// </summary>
        public TatlColorSchema TatlColorSchema { get; set; }

        /// <summary>
        /// Randomize background music (includes bgm from other video games)
        /// </summary>
        public Music Music { get; set; }

        /// <summary>
        /// Default Z-Targeting style to Hold
        /// </summary>
        public bool EnableHoldZTargeting { get; set; }

        /// <summary>
        /// Enables playing BGM at night for scenes that switch to night sfx
        /// </summary>
        public bool EnableNightBGM { get; set; }

        /// <summary>
        /// Enables stray fairy colors to be randomized.
        /// </summary>
        public bool RandomStrayFairyColors { get; set; }

        #region Asm Getters / Setters

        /// <summary>
        /// D-Pad configuration.
        /// </summary>
        public DPadConfig DPad {
            get { return this.AsmOptions.DPadConfig; }
            set { this.AsmOptions.DPadConfig = value; }
        }

        /// <summary>
        /// HUD colors.
        /// </summary>
        public HudColors HudColors {
            get { return this.AsmOptions.HudColorsConfig.Colors; }
            set { this.AsmOptions.HudColorsConfig.Colors = value; }
        }

        #endregion
    }
}
