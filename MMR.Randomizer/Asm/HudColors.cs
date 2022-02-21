using Be.IO;
using MMR.Common.Extensions;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.Models.Colors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace MMR.Randomizer.Asm
{
    /// <summary>
    /// Color values to use for various HUD elements.
    /// </summary>
    public class HudColors
    {
        public Color ButtonA { get; set; } = Color.FromArgb(0x64, 0xC8, 0xFF);
        public Color ButtonB { get; set; } = Color.FromArgb(0x64, 0xFF, 0x78);
        public Color ButtonC { get; set; } = Color.FromArgb(0xFF, 0xF0, 0x00);
        public Color ButtonStart { get; set; } = Color.FromArgb(0xFF, 0x82, 0x3C);
        public Color ClockEmblem { get; set; } = Color.FromArgb(0x00, 0xAA, 0x64);
        public Color ClockEmblemInverted1 { get; set; } = Color.FromArgb(0x64, 0xCD, 0xFF);
        public Color ClockEmblemInverted2 { get; set; } = Color.FromArgb(0x00, 0x9B, 0xFF);
        public Color ClockEmblemSun { get; set; } = Color.FromArgb(0xFF, 0xFF, 0x6E);
        public Color ClockMoon { get; set; } = Color.FromArgb(0xFF, 0xFF, 0x37);
        public Color ClockSun { get; set; } = Color.FromArgb(0xFF, 0x64, 0x6E);
        public Color Heart { get; set; } = Color.FromArgb(0xFF, 0x46, 0x32);
        public Color HeartDD { get; set; } = Color.FromArgb(0xC8, 0x00, 0x00);
        public Color Magic { get; set; } = Color.FromArgb(0x00, 0xC8, 0x00);
        public Color MagicInf { get; set; } = Color.FromArgb(0x00, 0x00, 0xC8);
        public Color Map { get; set; } = Color.FromArgb(0x00, 0xFF, 0xFF);
        public Color MapEntranceCursor { get; set; } = Color.FromArgb(0xC8, 0x00, 0x00);
        public Color MapPlayerCursor { get; set; } = Color.FromArgb(0xC8, 0xFF, 0x00);
        public Color RupeeIcon1 { get; set; } = Color.FromArgb(0xC8, 0xFF, 0x64);
        public Color RupeeIcon2 { get; set; } = Color.FromArgb(0xAA, 0xAA, 0xFF);
        public Color RupeeIcon3 { get; set; } = Color.FromArgb(0xFF, 0x69, 0x69);
        public Color ButtonIconA { get; set; } = Color.FromArgb(0x50, 0x5A, 0xFF);
        public Color ButtonIconB { get; set; } = Color.FromArgb(0x46, 0xFF, 0x50);
        public Color ButtonIconC { get; set; } = Color.FromArgb(0xFF, 0xFF, 0x32);
        public Color MenuAInner1 { get; set; } = Color.FromArgb(0x64, 0x96, 0xFF);
        public Color MenuAInner2 { get; set; } = Color.FromArgb(0x64, 0xFF, 0xFF);
        public Color MenuAOuter1 { get; set; } = Color.FromArgb(0x00, 0x00, 0x64);
        public Color MenuAOuter2 { get; set; } = Color.FromArgb(0x00, 0x96, 0xFF);
        public Color MenuCInner1 { get; set; } = Color.FromArgb(0xFF, 0xFF, 0x00);
        public Color MenuCInner2 { get; set; } = Color.FromArgb(0xFF, 0xFF, 0x00);
        public Color MenuCOuter1 { get; set; } = Color.FromArgb(0x00, 0x00, 0x00);
        public Color MenuCOuter2 { get; set; } = Color.FromArgb(0xFF, 0xA0, 0x00);
        public Color NoteA1 { get; set; } = Color.FromArgb(0x50, 0x96, 0xFF);
        public Color NoteA2 { get; set; } = Color.FromArgb(0x64, 0xC8, 0xFF);
        public Color NoteAShadow1 { get; set; } = Color.FromArgb(0x0A, 0x0A, 0x0A);
        public Color NoteAShadow2 { get; set; } = Color.FromArgb(0x32, 0x32, 0xFF);
        public Color NoteC1 { get; set; } = Color.FromArgb(0xFF, 0xFF, 0x32);
        public Color NoteC2 { get; set; } = Color.FromArgb(0xFF, 0xFF, 0xB4);
        public Color NoteCShadow1 { get; set; } = Color.FromArgb(0x0A, 0x0A, 0x0A);
        public Color NoteCShadow2 { get; set; } = Color.FromArgb(0x6E, 0x6E, 0x32);
        public Color PauseTitleA { get; set; } = Color.FromArgb(0x00, 0x64, 0xFF);
        public Color PauseTitleC { get; set; } = Color.FromArgb(0xFF, 0x96, 0x00);
        public Color Prompt1 { get; set; } = Color.FromArgb(0x00, 0x50, 0xC8);
        public Color Prompt2 { get; set; } = Color.FromArgb(0x32, 0x82, 0xFF);
        public Color PromptGlow { get; set; } = Color.FromArgb(0x00, 0x82, 0xFF);
        public Color ScoreLines { get; set; } = Color.FromArgb(0xFF, 0x00, 0x00);
        public Color ScoreNote { get; set; } = Color.FromArgb(0xFF, 0x64, 0x00);
        public Color DPad { get; set; } = Color.FromArgb(0x80, 0x80, 0x80);
        public Color MenuBorder1 { get; set; } = Color.FromArgb(0xB4, 0xB4, 0x78);
        public Color MenuBorder2 { get; set; } = Color.FromArgb(0x96, 0x8C, 0x5A);
        public Color MenuSubtitleText { get; set; } = Color.FromArgb(0xFF, 0xC8, 0x00);
        public Color ShopCursor1 { get; set; } = Color.FromArgb(0x00, 0x00, 0xFF);
        public Color ShopCursor2 { get; set; } = Color.FromArgb(0x00, 0x50, 0xFF);
        public Color RupeeIcon4 { get; set; } = Color.FromArgb(0xA0, 0x60, 0xFF);

        /// <summary>
        /// Get all colors in the order of serialization.
        /// </summary>
        public Color[] All()
        {
            return new Color[] {
                ButtonA,
                ButtonB,
                ButtonC,
                ButtonStart,
                ClockEmblem,
                ClockEmblemInverted1,
                ClockEmblemInverted2,
                ClockEmblemSun,
                ClockMoon,
                ClockSun,
                Heart,
                HeartDD,
                Magic,
                MagicInf,
                Map,
                MapEntranceCursor,
                MapPlayerCursor,
                RupeeIcon1,
                RupeeIcon2,
                RupeeIcon3,
                ButtonIconA,
                ButtonIconB,
                ButtonIconC,
                MenuAInner1,
                MenuAInner2,
                MenuAOuter1,
                MenuAOuter2,
                MenuCInner1,
                MenuCInner2,
                MenuCOuter1,
                MenuCOuter2,
                NoteA1,
                NoteA2,
                NoteAShadow1,
                NoteAShadow2,
                NoteC1,
                NoteC2,
                NoteCShadow1,
                NoteCShadow2,
                PauseTitleA,
                PauseTitleC,
                Prompt1,
                Prompt2,
                PromptGlow,
                ScoreLines,
                ScoreNote,
                DPad,
                MenuBorder1,
                MenuBorder2,
                MenuSubtitleText,
                ShopCursor1,
                ShopCursor2,
                RupeeIcon4,
            };
        }

        public HudColors()
        {
        }

        /// <summary>
        /// Clone to a new <see cref="HudColors"/>.
        /// </summary>
        /// <returns>Cloned colors</returns>
        public HudColors Clone()
        {
            return (HudColors)this.MemberwiseClone();
        }

        /// <summary>
        /// Get all colors for a specific <see cref="HudColorsConfigStruct"/> version value.
        /// </summary>
        /// <param name="version">Version</param>
        /// <returns>Colors</returns>
        public Color[] GetAllColors(uint version)
        {
            if (version == 0)
            {
                return this.All().Take(20).ToArray();
            }
            else if (version == 1)
            {
                return this.All().Take(46).ToArray();
            }
            else if (version == 2)
            {
                return this.All().Take(47).ToArray();
            }
            else
            {
                return this.All().Take(53).ToArray();
            }
        }

        /// <summary>
        /// Update certain colors which correspond to the A, B and C buttons.
        /// </summary>
        public void UpdateColorsUsingButtons()
        {
            var defaults = new HudColors();
            if (ButtonA != defaults.ButtonA)
            {
                UpdateColorsUsingAButton();
            }
            if (ButtonB != defaults.ButtonB)
            {
                UpdateColorsUsingBButton();
            }
            if (ButtonC != defaults.ButtonC)
            {
                UpdateColorsUsingCButton();
            }
        }

        /// <summary>
        /// Update certain colors which correspond to the A button.
        /// </summary>
        void UpdateColorsUsingAButton()
        {
            // Update text button icon.
            ButtonIconA = ButtonA;
            // Update pause menu cursor colors (blue).
            MenuAInner1 = ButtonA;
            MenuAInner2 = ButtonA.Brighten(0.4f);
            MenuAOuter1 = ButtonA.Darken(0.9f);
            MenuAOuter2 = ButtonA.Darken(0.2f);
            // Update note button A colors.
            NoteA1 = ButtonA;
            NoteA2 = ButtonA.Brighten(0.4f);
            NoteAShadow1 = Color.FromArgb(0x0A, 0x0A, 0x0A);
            NoteAShadow2 = ButtonA.Darken(0.2f);
            // Update pause menu title icon.
            PauseTitleA = ButtonA;
            // Update message prompt colors.
            Prompt1 = ButtonA.Darken(0.3f);
            Prompt2 = ButtonA;
            PromptGlow = ButtonA.Brighten(0.2f);
            // Update shop cursor colors.
            ShopCursor1 = ButtonA.Darken(0.4f);
            ShopCursor2 = ButtonA.Brighten(0.2f);
        }

        /// <summary>
        /// Update certain colors which correspond to the B button.
        /// </summary>
        void UpdateColorsUsingBButton()
        {
            // Update text button icon.
            ButtonIconB = ButtonB;
        }

        /// <summary>
        /// Update certain colors which correspond to the C button.
        /// </summary>
        void UpdateColorsUsingCButton()
        {
            // Update text button icon.
            ButtonIconC = ButtonC;
            // Update pause menu cursor colors (yellow).
            MenuCInner1 = ButtonC;
            MenuCInner2 = ButtonC.Brighten(0.4f);
            MenuCOuter1 = Color.Black;
            MenuCOuter2 = ButtonC.Darken(0.2f);
            // Update note button C colors.
            NoteC1 = ButtonC;
            NoteC2 = ButtonC.Brighten(0.4f);
            NoteCShadow1 = Color.FromArgb(0x0A, 0x0A, 0x0A);
            NoteCShadow2 = ButtonC.Darken(0.2f);
            // Update pause menu title icon.
            PauseTitleC = ButtonC;
        }
    }

    /// <summary>
    /// HUD colors configuration.
    /// </summary>
    public class HudColorsConfig : AsmConfig
    {
        /// <summary>
        /// HUD color values.
        /// </summary>
        public HudColors Colors { get; set; } = new HudColors();

        /// <summary>
        /// Optional color overrides for hearts.
        /// </summary>
        [JsonIgnore]
        public Tuple<Color, Color> HeartsOverride { get; set; } = null;

        /// <summary>
        /// Optional color overrides for magic meter.
        /// </summary>
        [JsonIgnore]
        public Tuple<Color, Color> MagicOverride { get; set; } = null;

        /// <summary>
        /// Optional hue shift for color of miscellaneous UI elements (pause menu border).
        /// </summary>
        [JsonIgnore]
        public Tuple<float, float, float> HueShift { get; set; } = null;

        /// <summary>
        /// Get the finalized <see cref="HudColors"/> after applying color overrides.
        /// </summary>
        /// <returns>Finalized colors</returns>
        public HudColors FinalizedColors()
        {
            var colors = this.Colors.Clone();

            // Update certain colors using the configured button colors.
            colors.UpdateColorsUsingButtons();

            if (this.HeartsOverride != null)
            {
                colors.Heart = this.HeartsOverride.Item1;
                colors.HeartDD = this.HeartsOverride.Item2;
            }

            if (this.MagicOverride != null)
            {
                colors.Magic = this.MagicOverride.Item1;
                colors.MagicInf = this.MagicOverride.Item2;
            }

            if (this.HueShift != null)
            {
                // Apply hue shift for pause menu border color.
                colors.MenuBorder1 = colors.MenuBorder1.ShiftHue(this.HueShift.Item1);
                colors.MenuBorder2 = colors.MenuBorder2.ShiftHue(this.HueShift.Item1);
                colors.MenuSubtitleText = colors.MenuSubtitleText.ShiftHue(this.HueShift.Item1).Brighten(0.3f);

                // Apply hue shift for score lines color.
                colors.ScoreLines = colors.ScoreLines.ShiftHue(this.HueShift.Item2);

                // Apply hue shift for score note color.
                colors.ScoreNote = colors.ScoreNote.ShiftHue(this.HueShift.Item3);
            }

            return colors;
        }

        /// <summary>
        /// Convert to a <see cref="HudColorsConfigStruct"/> structure.
        /// </summary>
        /// <param name="version">Structure version</param>
        /// <returns>Structure</returns>
        public override IAsmConfigStruct ToStruct(uint version)
        {
            return new HudColorsConfigStruct
            {
                Version = version,
                Colors = this.FinalizedColors().GetAllColors(version),
            };
        }
    }

    /// <summary>
    /// HUD colors configuration structure.
    /// </summary>
    public struct HudColorsConfigStruct : IAsmConfigStruct
    {
        public uint Version;
        public Color[] Colors;

        /// <summary>
        /// Convert to bytes.
        /// </summary>
        /// <returns>Bytes</returns>
        public byte[] ToBytes()
        {
            using (var memStream = new MemoryStream())
            using (var writer = new BeBinaryWriter(memStream))
            {
                writer.WriteUInt32(this.Version);

                foreach (var color in this.Colors)
                {
                    writer.WriteBytes(color.ToBytesRGB(0));
                }
                return memStream.ToArray();
            }
        }
    }

    /// <summary>
    /// Preset HUD colors.
    /// </summary>
    public static class HudColorPresets
    {
        /// <summary>
        /// Standard color pairings, with first element as "darker" and second as "lighter".
        /// </summary>
        /// <returns>Array of pairs</returns>
        public static NamedColorPair[] StandardPairs()
        {
            return new NamedColorPair[]
            {
                new NamedColorPair("Red", Color.FromArgb(0xFF, 0x33, 0x00), Color.FromArgb(0xCC, 0x00, 0x66)),
                new NamedColorPair("Orange", Color.FromArgb(0xFF, 0x99, 0x00), Color.FromArgb(0xFF, 0xC2, 0x66)),
                new NamedColorPair("Yellow", Color.FromArgb(0xFF, 0xFF, 0x00), Color.FromArgb(0xFF, 0xFF, 0x66)),
                new NamedColorPair("Green", Color.FromArgb(0x00, 0x66, 0x00), Color.FromArgb(0x33, 0xCC, 0x33)),
                new NamedColorPair("Blue", Color.FromArgb(0x33, 0x66, 0xCC), Color.FromArgb(0x33, 0xCC, 0xFF)),
                new NamedColorPair("Purple", Color.FromArgb(0x99, 0x00, 0xFF), Color.FromArgb(0x99, 0x66, 0xFF)),
                new NamedColorPair("Pink", Color.FromArgb(0xFF, 0x66, 0xD9), Color.FromArgb(0xFF, 0xB3, 0xEC)),
            };
        }

        /// <summary>
        /// Get <see cref="NamedColorPair"/> pairings for magic meter color selection.
        /// </summary>
        /// <returns>Pairs</returns>
        public static NamedColorPair[] MagicMeter()
        {
            var pairs = new NamedColorPair[]
            {
                new NamedColorPair("Classic", Color.FromArgb(0x00, 0xC8, 0x00), Color.FromArgb(0x00, 0x00, 0xC8)),
                new NamedColorPair("Orange Yoshi", Color.FromArgb(0xFF, 0xAE, 0x1A), Color.FromArgb(0x6F, 0xD2, 0x51)),
                new NamedColorPair("Vaporwave", Color.FromArgb(0xFF, 0x71, 0xCE), Color.FromArgb(0x01, 0xCD, 0xFE)),
            };

            return new List<NamedColorPair>()
                .Concat(pairs)
                .Concat(StandardPairs())
                .ToArray();
        }

        /// <summary>
        /// Get <see cref="NamedColorPair"/> pairings for hearts color selection.
        /// </summary>
        /// <returns>Pairs</returns>
        public static NamedColorPair[] Hearts()
        {
            return StandardPairs().Select((pair) => pair.Reverse()).ToArray();
        }
    }
}
