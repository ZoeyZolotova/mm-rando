using MMR.Randomizer.Models.Settings;
using MMR.UI.Forms.Tooltips;
using MMR.Randomizer;
using MMR.Common.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MMR.Randomizer.Models;
using MMR.Randomizer.Utils;
using MMR.Randomizer.Asm;
using MMR.Randomizer.Models.Colors;
using MMR.Common.Utils;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Extensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MMR.Randomizer.Constants;
using System.Threading;
using MMR.UI.Controls;
using System.Linq.Expressions;
using MMR.Randomizer.Attributes.Setting;
using System.Runtime.Versioning;

namespace MMR.UI.Forms
{
    using Randomizer = Randomizer.Randomizer;

    [SupportedOSPlatform("windows")]
    public partial class MainForm : Form
    {
        private bool _isUpdating = false;
        private int _seedOld = 0;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Configuration _configuration { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AboutForm About { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ManualForm Manual { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogicEditorForm LogicEditor { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomItemListEditForm ItemEditor { get; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StartingItemEditForm StartingItemEditor { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public JunkLocationEditForm JunkLocationEditor { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HudConfigForm HudConfig { get; private set; }


        public const string SETTINGS_EXTENSION = ".json";

        public MainForm()
        {
            InitializeComponent();
            InitializeSettings();
            InitializeSimpleControls();
            InitializeHUDGroupBox();
            InitializeTransformationFormSettings();
            InitializeShortenCutsceneSettings();
            InitializeItemPoolSettings();
            InitializeDungeonModeSettings();
            InitializeTrapSettings();
            InitalizeLowHealthSFXOptions();

            StartingItemEditor = new StartingItemEditForm();

            JunkLocationEditor = new JunkLocationEditForm();

            ItemEditor = new CustomItemListEditForm(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.CustomItemListString)).GetAttribute<SettingItemListAttribute>().ItemList, item => $"{item.Location()} ({item.Name()})", "Invalid custom item string");

            LogicEditor = new LogicEditorForm();
            Manual = new ManualForm();
            About = new AboutForm();
            HudConfig = new HudConfigForm();


            Text = $"Majora's Mask Randomizer v{Randomizer.AssemblyVersion}";
        }

        private void InitializeSimpleControls()
        {
            Dictionary<Control, Expression<Func<Configuration, object>>> simpleControls = new Dictionary<Control, Expression<Func<Configuration, object>>>
            {
                // ROM Settings
                { cN64, cfg => cfg.OutputSettings.GenerateROM },
                { cVC, cfg => cfg.OutputSettings.OutputVC },
                { cSpoiler, cfg => cfg.OutputSettings.GenerateSpoilerLog },
                { cHTMLLog, cfg => cfg.OutputSettings.GenerateHTMLLog },
                { cPatch, cfg => cfg.OutputSettings.GeneratePatch },

                // Main Settings
                { cMixSongs, cfg => cfg.GameplaySettings.AddSongs },
                { cProgressiveUpgrades, cfg => cfg.GameplaySettings.ProgressiveUpgrades },
                { cDEnt, cfg => cfg.GameplaySettings.RandomizeDungeonEntrances },
                { cShuffleBosses, cfg => cfg.GameplaySettings.RandomizeBossRooms },
                { cEnemy, cfg => cfg.GameplaySettings.RandomizeEnemies },
                { cMode, cfg => cfg.GameplaySettings.LogicMode },
                { cItemPlacement, cfg => cfg.GameplaySettings.ItemPlacement },

                // Gimmicks
                { cHideClock, cfg => cfg.GameplaySettings.HideClock },
                { cSunsSong, cfg => cfg.GameplaySettings.EnableSunsSong },
                { cUnderwaterOcarina, cfg => cfg.GameplaySettings.OcarinaUnderwater },
                { cFDAnywhere, cfg => cfg.GameplaySettings.AllowFierceDeityAnywhere },
                { cByoAmmo, cfg => cfg.GameplaySettings.ByoAmmo },
                { cDeathMoonCrash, cfg => cfg.GameplaySettings.DeathMoonCrash },
                { cMoonCrashFileErase, cfg => cfg.GameplaySettings.MoonCrashErasesFile },
                { cFewerHealthDrops, cfg => cfg.GameplaySettings.FewerHealthDrops },
                { cTakeDamageOnEpona, cfg => cfg.GameplaySettings.TakeDamageOnEpona },
                { cTakeDamageWhileShielding, cfg => cfg.GameplaySettings.TakeDamageWhileShielding },
                { cTakeDamageFromVoid, cfg => cfg.GameplaySettings.TakeDamageFromVoid },
                { cTakeDamageFromGorons, cfg => cfg.GameplaySettings.TakeDamageFromGorons },
                { cTakeDamageFromGibdosFaster, cfg => cfg.GameplaySettings.TakeDamageFromGibdosFaster },
                { cTakeDamageGettingCaught, cfg => cfg.GameplaySettings.TakeDamageGettingCaught },
                { cTakeDamageFromDog, cfg => cfg.GameplaySettings.TakeDamageFromDog },
                { cTakeDamageFromDexihands, cfg => cfg.GameplaySettings.TakeDamageFromDexihands },
                { cContinuousDekuHopping, cfg => cfg.GameplaySettings.ContinuousDekuHopping },
                { cIronGoron, cfg => cfg.GameplaySettings.IronGoron },
                { cHookshotAnySurface, cfg => cfg.GameplaySettings.HookshotAnySurface },
                { cClimbMostSurfaces, cfg => cfg.GameplaySettings.ClimbMostSurfaces },
                { cIceTrapQuirks, cfg => cfg.GameplaySettings.TrapQuirks },
                { cInstantTransformations, cfg => cfg.GameplaySettings.InstantTransform },
                { cBombArrows, cfg => cfg.GameplaySettings.BombArrows },
                { cVanillaMoonTrials, cfg => cfg.GameplaySettings.VanillaMoonTrialAccess },
                { cGiantMaskAnywhere, cfg => cfg.GameplaySettings.GiantMaskAnywhere },
                { cDMult, cfg => cfg.GameplaySettings.DamageMode },
                { cDType, cfg => cfg.GameplaySettings.DamageEffect },
                { cGravity, cfg => cfg.GameplaySettings.MovementMode },
                { cNutAndStickDrops, cfg => cfg.GameplaySettings.NutandStickDrops },
                { cChestGameMinimap, cfg => cfg.GameplaySettings.ChestGameMinimap },
                { cFloors, cfg => cfg.GameplaySettings.FloorType },
                { cClockSpeed, cfg => cfg.GameplaySettings.ClockSpeed },
                { cAutoInvert, cfg => cfg.GameplaySettings.AutoInvert },
                { cStartingItems, cfg => cfg.GameplaySettings.StartingItemMode },
                { cRequiredBossRemains, cfg => cfg.GameplaySettings.RequiredBossRemains },
                { cBlastCooldown, cfg => cfg.GameplaySettings.BlastMaskCooldown },
                { cTrapAmount, cfg => cfg.GameplaySettings.TrapAmount },
                { cTrapsAppearance, cfg => cfg.GameplaySettings.TrapAppearance },

                // Comfort
                { cQText, cfg => cfg.GameplaySettings.QuickTextEnabled },
                { cFreeHints, cfg => cfg.GameplaySettings.FreeHints },
                { cFreeGaroHints, cfg => cfg.GameplaySettings.FreeGaroHints },
                { cGossipsTolerant, cfg => cfg.GameplaySettings.TolerantGossipStones },
                { cEasyFrameByFrame, cfg => cfg.GameplaySettings.EasyFrameByFrame },
                { cMixGaroWithGossip, cfg => cfg.GameplaySettings.MixGossipAndGaroHints },
                { cClearHints, cfg => cfg.GameplaySettings.ClearHints },
                { cClearGaroHints, cfg => cfg.GameplaySettings.ClearGaroHints },
                { cImportanceCount, cfg => cfg.GameplaySettings.ImportanceCount },
                { cImportanceCountGaro, cfg => cfg.GameplaySettings.ImportanceCountGaro },
                { cHintImportance, cfg => cfg.GameplaySettings.HintsIndicateImportance },
                { cNoDowngrades, cfg => cfg.GameplaySettings.PreventDowngrades },
                { cShopAppearance, cfg => cfg.GameplaySettings.UpdateShopAppearance },
                { cUpdateChests, cfg => cfg.GameplaySettings.UpdateChests },
                { cUpdateNpcText, cfg => cfg.GameplaySettings.UpdateNPCText },
                { cOathHint, cfg => cfg.GameplaySettings.OathHint },
                { cRemainsHint, cfg => cfg.GameplaySettings.RemainsHint },
                { cFairyAndSkullHints, cfg => cfg.GameplaySettings.FairyAndSkullHint },
                { cEponaSword, cfg => cfg.GameplaySettings.FixEponaSword },
                { cDrawHash, cfg => cfg.GameplaySettings.DrawHash },
                { cQuestItemStorage, cfg => cfg.GameplaySettings.QuestItemStorage },
                { cQuestItemKeep, cfg => cfg.GameplaySettings.KeepQuestTradeThroughTime },
                { cDisableCritWiggle, cfg => cfg.GameplaySettings.CritWiggleDisable },
                { cLink, cfg => cfg.GameplaySettings.Character },
                { cGossipHints, cfg => cfg.GameplaySettings.GossipHintStyle },
                { cGaroHint, cfg => cfg.GameplaySettings.GaroHintStyle },
                { cSkipBeaver, cfg => cfg.GameplaySettings.SpeedupBeavers },
                { cGoodDampeRNG, cfg => cfg.GameplaySettings.SpeedupDampe },
                { cGoodDogRaceRNG, cfg => cfg.GameplaySettings.SpeedupDogRace },
                { cFasterLabFish, cfg => cfg.GameplaySettings.SpeedupLabFish },
                { cFasterBank, cfg => cfg.GameplaySettings.SpeedupBank },
                { cDoubleArcheryRewards, cfg => cfg.GameplaySettings.DoubleArcheryRewards },
                { cSpeedupBabyCucco, cfg => cfg.GameplaySettings.SpeedupBabyCuccos },
                { cFastPush, cfg => cfg.GameplaySettings.FastPush },
                { cFreestanding, cfg => cfg.GameplaySettings.UpdateWorldModels },
                { cArrowCycling, cfg => cfg.GameplaySettings.ArrowCycling },
                { cCloseCows, cfg => cfg.GameplaySettings.CloseCows },
                { cElegySpeedups, cfg => cfg.GameplaySettings.ElegySpeedup },
                { cImprovedPictobox, cfg => cfg.GameplaySettings.EnablePictoboxSubject },
                { cLenientGoronSpikes, cfg => cfg.GameplaySettings.LenientGoronSpikes },
                { cTargetHealth, cfg => cfg.GameplaySettings.TargetHealthBar },
                { cFreeScarecrow, cfg => cfg.GameplaySettings.FreeScarecrow },
                { cFillWallet, cfg => cfg.GameplaySettings.FillWallet },
                { cInvisSparkle, cfg => cfg.GameplaySettings.HiddenRupeesSparkle },
                { cSaferGlitches, cfg => cfg.GameplaySettings.SaferGlitches },
                { cAddBombchuDrops, cfg => cfg.GameplaySettings.BombchuDrops },
                { cFairyMaskShimmer, cfg => cfg.GameplaySettings.FairyMaskShimmer },
                { cSkulltulaTokenSounds, cfg => cfg.GameplaySettings.SkulltulaTokenSounds },
                { nMaxGossipWotH, cfg => cfg.GameplaySettings.OverrideNumberOfRequiredGossipHints },
                { nMaxGossipFoolish, cfg => cfg.GameplaySettings.OverrideNumberOfNonRequiredGossipHints },
                { nMaxGossipCT, cfg => cfg.GameplaySettings.OverrideMaxNumberOfClockTownGossipHints },
                { nMaxGaroWotH, cfg => cfg.GameplaySettings.OverrideNumberOfRequiredGaroHints },
                { nMaxGaroFoolish, cfg => cfg.GameplaySettings.OverrideNumberOfNonRequiredGaroHints },
                { nMaxGaroCT, cfg => cfg.GameplaySettings.OverrideMaxNumberOfClockTownGaroHints },

                // Cosmetics
                { cTargettingStyle, cfg => cfg.CosmeticSettings.EnableHoldZTargeting },
                { cLowHealthSFXComboBox, cfg => cfg.CosmeticSettings.LowHealthSFX },
                { cSFX, cfg => cfg.CosmeticSettings.RandomizeSounds },
                { cMusic, cfg => cfg.CosmeticSettings.Music },
                { lLuckRoll, cfg => cfg.CosmeticSettings.MusicLuckRollChance },
                { tLuckRollPercentage, cfg => cfg.CosmeticSettings.MusicLuckRollChance },
                { cTatl, cfg => cfg.CosmeticSettings.TatlColorSchema },
                { cEnableNightMusic, cfg => cfg.CosmeticSettings.EnableNightBGM },
                { cRemoveMinorMusic, cfg => cfg.CosmeticSettings.RemoveMinorMusic },
                { cMusicTrackNames, cfg => cfg.CosmeticSettings.ShowTrackName },
                { cDisableFanfares, cfg => cfg.CosmeticSettings.DisableFanfares },
                { cCombatMusicDisable, cfg => cfg.CosmeticSettings.DisableCombatMusic },
                { cHueShiftMiscUI, cfg => cfg.CosmeticSettings.ShiftHueMiscUI },
                { cBombTrapTunicColors, cfg => cfg.CosmeticSettings.BombTrapsRandomizeTunicColor },
                { cRainbowTunic, cfg => cfg.CosmeticSettings.RainbowTunic },
                { cCameraStyle, cfg => cfg.CosmeticSettings.CameraStyle },
            };

            foreach (var (control, expression) in simpleControls)
            {
                var body = expression.Body;
                if (body is UnaryExpression)
                {
                    body = ((UnaryExpression)body).Operand;
                }
                var memberExpression = body as MemberExpression;
                if (memberExpression != null)
                {
                    var description = memberExpression.Member.GetCustomAttribute<DescriptionAttribute>()?.Description;
                    if (description != null)
                    {
                        TooltipBuilder.SetTooltip(control, description);
                    }
                }

                var checkBox = control as CheckBox;
                if (checkBox != null)
                {
                    var newValueParameter = Expression.Parameter(typeof(bool), "newValue");
                    var assignExpression = Expression.Assign(body, newValueParameter);
                    var func = Expression.Lambda<Action<Configuration, bool>>(assignExpression, expression.Parameters[0], newValueParameter).Compile();

                    checkBox.CheckedChanged += (object sender, EventArgs e) =>
                    {
                        UpdateSingleSetting(() => func(_configuration, checkBox.Checked));
                    };
                }

                var comboBox = control as ComboBox;
                if (comboBox != null)
                {
                    var newValueParameter = Expression.Parameter(typeof(int), "newValue");
                    var assignExpression = Expression.Assign(body, Expression.Convert(newValueParameter, body.Type));
                    var func = Expression.Lambda<Action<Configuration, int>>(assignExpression, expression.Parameters[0], newValueParameter).Compile();

                    comboBox.SelectedIndexChanged += (object sender, EventArgs e) =>
                    {
                        UpdateSingleSetting(() => func(_configuration, comboBox.SelectedIndex));
                    };
                }

                var numericUpDown = control as NumericUpDown;
                if (numericUpDown != null)
                {
                    var newValueParameter = Expression.Parameter(typeof(decimal), "newValue");
                    var assignExpression = Expression.Assign(body, Expression.Convert(newValueParameter, body.Type));
                    var func = Expression.Lambda<Action<Configuration, decimal>>(assignExpression, expression.Parameters[0], newValueParameter).Compile();

                    numericUpDown.ValueChanged += (object sender, EventArgs e) =>
                    {
                        UpdateSingleSetting(() => func(_configuration, numericUpDown.Value));
                    };
                }
            }

            TooltipBuilder.SetTooltip(lTrapWeightings, "How much to weigh each type of trap when randomizing which one to use.");
            TooltipBuilder.SetTooltip(cInstantPictobox, "Remove anti-aliasing from the Pictobox pictures, which is what makes Pictobox on emulator so slow.");
        }

        void UpdateSettingLogicMarkers()
        {
            var data = LogicUtils.ReadRulesetFromResources(_configuration.GameplaySettings.LogicMode, _configuration.GameplaySettings.UserLogicFileName);
            ItemList itemList;
            if (data != null)
            {
                try
                {
                    itemList = LogicUtils.PopulateItemListFromLogicData(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    itemList = LogicUtils.PopulateItemListWithoutLogic();
                }
            }
            else
            {
                itemList = LogicUtils.PopulateItemListWithoutLogic();
            }

            var settingToControlMapping2 = new Dictionary<Control, (Type declaringType, string propertyName)>
            {
                { lGravity, (typeof(GameplaySettings), nameof(GameplaySettings.MovementMode)) },
                { lFloors, (typeof(GameplaySettings), nameof(GameplaySettings.FloorType)) },
                { cContinuousDekuHopping, (typeof(GameplaySettings), nameof(GameplaySettings.ContinuousDekuHopping)) },
                { cHookshotAnySurface, (typeof(GameplaySettings), nameof(GameplaySettings.HookshotAnySurface)) },
                { cClimbMostSurfaces, (typeof(GameplaySettings), nameof(GameplaySettings.ClimbMostSurfaces)) },
                { cIronGoron, (typeof(GameplaySettings), nameof(GameplaySettings.IronGoron)) },
                { lClockSpeed, (typeof(GameplaySettings), nameof(GameplaySettings.ClockSpeed)) },
                { lAutoInvert, (typeof(GameplaySettings), nameof(GameplaySettings.AutoInvert)) },
                { cHideClock, (typeof(GameplaySettings), nameof(GameplaySettings.HideClock)) },
                { lTrapAmount, (typeof(GameplaySettings), nameof(GameplaySettings.TrapAmount)) },
                { lTrapsAppearance, (typeof(GameplaySettings), nameof(GameplaySettings.TrapAppearance)) },
                { lTrapWeightings, (typeof(GameplaySettings), nameof(GameplaySettings.TrapWeights)) },
                { lDMult, (typeof(GameplaySettings), nameof(GameplaySettings.DamageMode)) },
                { lDType, (typeof(GameplaySettings), nameof(GameplaySettings.DamageEffect)) },
                { cDeathMoonCrash, (typeof(GameplaySettings), nameof(GameplaySettings.DeathMoonCrash)) },
                { cMoonCrashFileErase, (typeof(GameplaySettings), nameof(GameplaySettings.MoonCrashErasesFile)) },
                { cByoAmmo, (typeof(GameplaySettings), nameof(GameplaySettings.ByoAmmo)) },
                { cFewerHealthDrops, (typeof(GameplaySettings), nameof(GameplaySettings.FewerHealthDrops)) },
                { lBlastMask, (typeof(GameplaySettings), nameof(GameplaySettings.BlastMaskCooldown)) },
                { lNutAndStickDrops, (typeof(GameplaySettings), nameof(GameplaySettings.NutandStickDrops)) },
                { cUnderwaterOcarina, (typeof(GameplaySettings), nameof(GameplaySettings.OcarinaUnderwater)) },
                { cSunsSong, (typeof(GameplaySettings), nameof(GameplaySettings.EnableSunsSong)) },
                { cFreeScarecrow, (typeof(GameplaySettings), nameof(GameplaySettings.FreeScarecrow)) },
                { cFDAnywhere, (typeof(GameplaySettings), nameof(GameplaySettings.AllowFierceDeityAnywhere)) },
                { cGiantMaskAnywhere, (typeof(GameplaySettings), nameof(GameplaySettings.GiantMaskAnywhere)) },
                { cInstantTransformations, (typeof(GameplaySettings), nameof(GameplaySettings.InstantTransform)) },
                { cBombArrows, (typeof(GameplaySettings), nameof(GameplaySettings.BombArrows)) },
                { cTakeDamageOnEpona, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageOnEpona))},
                { cTakeDamageWhileShielding, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageWhileShielding))},
                { cTakeDamageFromVoid, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageFromVoid))},
                { cTakeDamageFromGorons, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageFromGorons))},
                { cTakeDamageFromGibdosFaster, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageFromGibdosFaster))},
                { cTakeDamageGettingCaught, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageGettingCaught))},
                { cTakeDamageFromDog, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageFromDog))},
                { cTakeDamageFromDexihands, (typeof(GameplaySettings), nameof(GameplaySettings.TakeDamageFromDexihands))},

            };

            foreach (var (control, settingInfo) in settingToControlMapping2)
            {
                control.Text = Regex.Replace(control.Text, "\\*$", "");
                if (itemList.Any(io => !string.IsNullOrWhiteSpace(io.SettingExpression) && LogicUtils.ParseSettingExpression(io.SettingExpression).VisitsMember(settingInfo.declaringType, settingInfo.propertyName)))
                {
                    control.Text += "*";
                }
            }
        }

        /// <summary>
        /// Initialize components in the HUD <see cref="GroupBox"/>.
        /// </summary>
        void InitializeHUDGroupBox()
        {
            // Initialize ComboBox for hearts colors
            cHUDHeartsComboBox.Items.AddRange(ColorSelectionManager.Hearts.GetItems());
            cHUDHeartsComboBox.SelectedIndex = 0;

            // Initialize ComboBox for magic meter color
            cHUDMagicComboBox.Items.AddRange(ColorSelectionManager.MagicMeter.GetItems());
            cHUDMagicComboBox.SelectedIndex = 0;
        }

        Regex addSpacesRegex = new Regex("(?<!^)([A-Z])");

        private void InitializeTrapSettings()
        {
            var initialX = 7;
            var initialY = 146;
            var deltaX = 87;
            var deltaY = 23;
            var maxLabelWidth = 100;
            var inputWidth = 31;
            var height = 23;
            var currentX = initialX;
            var currentY = initialY;
            var inputMarginLeft = 0;
            var inputMarginRight = 7;

            foreach (var trapType in Enum.GetValues<TrapType>())
            {
                if (Convert.ToInt32(trapType) == 0)
                {
                    continue;
                }

                var labelText = addSpacesRegex.Replace(trapType.ToString(), " $1");

                var label = new Label
                {
                    Name = $"lTrap_{trapType}",
                    Tag = trapType,
                    Text = labelText,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(currentX, currentY),
                    Size = new Size(maxLabelWidth, height),
                };

                var size = TextRenderer.MeasureText(labelText, label.Font);
                label.Width = size.Width;

                if (currentX + label.Width + inputMarginLeft + inputWidth > gTraps.Width)
                {
                    currentX = initialX;
                    currentY += deltaY;
                    label.Location = new Point(currentX, currentY);
                }

                var input = new NumericUpDown
                {
                    Name = $"nTrap_{trapType}",
                    Tag = trapType,
                    Location = new Point(currentX + label.Width + inputMarginLeft, currentY),
                    Size = new Size(inputWidth, height),
                };
                var description = trapType.GetAttribute<DescriptionAttribute>()?.Description;
                if (description != null)
                {
                    TooltipBuilder.SetTooltip(input, description);
                }

                input.ValueChanged += nTrap_CheckedChanged;

                gTraps.Controls.Add(label);
                gTraps.Controls.Add(input);

                currentX += label.Width + inputMarginLeft + inputWidth + inputMarginRight;
            }
        }

        private void nTrap_CheckedChanged(object sender, EventArgs e)
        {
            var input = (NumericUpDown)sender;
            var trapType = (TrapType)input.Tag;
            UpdateSingleSetting(() => _configuration.GameplaySettings.TrapWeights[trapType] = (int)input.Value);
        }

        private void InitializeDungeonModeSettings()
        {
            var properties = new List<PropertyInfo>();
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.VictoryMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.PriceMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.BossRemainsMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.BossKeyMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.SmallKeyMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.StrayFairyMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.DungeonNavigationMode)));
            foreach (var propertyInfo in properties)
            {
                var tabPage = new TabPage
                {
                    Tag = propertyInfo,
                    Text = propertyInfo.PropertyType.GetCustomAttribute<DescriptionAttribute>().Description,
                    UseVisualStyleBackColor = true,
                };
                tOtherCustomizations.TabPages.Add(tabPage);

                var initialX = 6;
                var initialY = 7;
                var deltaX = 187;
                var deltaY = 23;
                var width = 187;
                var height = 23;
                var currentX = initialX;
                var currentY = initialY;
                foreach (var value in Enum.GetValues(propertyInfo.PropertyType).Cast<Enum>())
                {
                    if (Convert.ToInt32(value) == 0)
                    {
                        continue;
                    }
                    var checkBox = new CheckBox
                    {
                        Tag = value,
                        Name = "cDungeonMode_" + value.ToString(),
                        Text = addSpacesRegex.Replace(value.ToString(), " $1"),
                        Location = new Point(currentX, currentY),
                        Size = new Size(width, height),
                    };
                    var description = value.GetAttribute<DescriptionAttribute>()?.Description;
                    if (description != null)
                    {
                        TooltipBuilder.SetTooltip(checkBox, description);
                    }
                    checkBox.CheckedChanged += cDungeonMode_CheckedChanged;
                    tabPage.Controls.Add(checkBox);
                    currentX += deltaX;
                    if (currentX > tOtherCustomizations.Width - width)
                    {
                        currentX = initialX;
                        currentY += deltaY;
                    }
                }
            }
        }

        private void cDungeonMode_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var propertyInfo = (PropertyInfo)checkBox.Parent.Tag;
            var cutsceneFlag = (int)checkBox.Tag;
            var value = (int)propertyInfo.GetValue(_configuration.GameplaySettings);
            var newValue = checkBox.Checked ? value | cutsceneFlag : value & ~cutsceneFlag;
            UpdateSingleSetting(() => propertyInfo.SetValue(_configuration.GameplaySettings, newValue));
        }

        private class LocationCategoryLabel : Label
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public List<string> Lines { get; set; }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                var b = new SolidBrush(ForeColor);
                e.Graphics.TranslateTransform(0, Height);
                e.Graphics.RotateTransform(-90);
                foreach (var line in Lines)
                {
                    e.Graphics.RotateTransform(15);
                    e.Graphics.DrawString(line, Font, b, 0f, 0f);
                    e.Graphics.RotateTransform(-15);
                    e.Graphics.TranslateTransform(0, 26);
                }
            }
        }

        private void InitializeItemPoolSettings()
        {
            var itemsByClassicCategory = ItemUtils.ItemsByClassicCategory();
            var classicCategoryX = 0;
            var classicCategoryY = 0; // 140?
            var classicCategoryWidth = 190;
            foreach (var classicCategory in Enum.GetValues<ClassicCategory>())
            {
                if (classicCategory <= 0)
                {
                    continue;
                }

                var checkbox = new InvertIndeterminateCheckBox();
                var items = itemsByClassicCategory[classicCategory];
                checkbox.Tag = items;
                checkbox.Text = $"{addSpacesRegex.Replace(classicCategory.ToString(), " $1")}: +{items.Count}";
                var description = classicCategory.GetAttribute<DescriptionAttribute>()?.Description;
                if (description != null)
                {
                    TooltipBuilder.SetTooltip(checkbox, description);
                }
                checkbox.Location = new Point(classicCategoryX, classicCategoryY);
                checkbox.Width = classicCategoryWidth;
                checkbox.Height = 26;
                checkbox.CheckStateChanged += cItemCategory2_CheckStateChanged;
                pClassicItemPool.Controls.Add(checkbox);

                classicCategoryX += classicCategoryWidth;
                if (classicCategoryX + classicCategoryWidth > pClassicItemPool.Width)
                {
                    classicCategoryX = 0;
                    classicCategoryY += 26;
                }
            }

            var itemsByItemCategory = ItemUtils.ItemsByItemCategory();

            var itemsByLocationCategory = ItemUtils.ItemsByLocationCategory();

            tableItemPool.RowCount = Enum.GetValues<ItemCategory>().Count() - 2;
            tableItemPool.ColumnCount = Enum.GetValues<LocationCategory>().Count() - 1;

            var locationCategoriesX = 134;

            var locationCategoryLabel = new LocationCategoryLabel();
            locationCategoryLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            locationCategoryLabel.Location = new Point(locationCategoriesX + 24, 0);
            locationCategoryLabel.Width = 714;
            locationCategoryLabel.Height = 105;
            locationCategoryLabel.Lines = Enum.GetValues<LocationCategory>().Where(c => c > 0).Select(c => $"{addSpacesRegex.Replace(c.ToString(), " $1")}: +{itemsByLocationCategory[c].Count}").ToList();

            pLocationCategories.Controls.Add(locationCategoryLabel);

            foreach (var locationCategory in Enum.GetValues<LocationCategory>())
            {
                if (locationCategory < 0)
                {
                    continue;
                }

                var checkbox = new InvertIndeterminateCheckBox();
                var items = locationCategory == 0 ? ItemUtils.AllLocations().ToList() : itemsByLocationCategory[locationCategory];
                checkbox.Tag = items;
                var description = locationCategory.GetAttribute<DescriptionAttribute>()?.Description;
                if (description != null)
                {
                    TooltipBuilder.SetTooltip(checkbox, description);
                }
                checkbox.Location = new Point(locationCategoriesX, 110);
                checkbox.Width = 17;
                checkbox.Height = 17;
                checkbox.CheckStateChanged += cItemCategory2_CheckStateChanged;
                tableItemPool.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25));
                pLocationCategories.Controls.Add(checkbox);

                locationCategoriesX += 26;
            }

            foreach (var itemCategory in Enum.GetValues<ItemCategory>())
            {
                if (itemCategory <= 0)
                {
                    continue;
                }

                foreach (var locationCategory in Enum.GetValues<LocationCategory>())
                {
                    if (locationCategory < 0)
                    {
                        continue;
                    }

                    if (itemCategory == 0 && locationCategory == 0)
                    {
                        continue;
                    }

                    InvertIndeterminateCheckBox checkbox = null;

                    if (locationCategory == 0)
                    {
                        checkbox = new InvertIndeterminateCheckBox();
                        var items = itemsByItemCategory[itemCategory];
                        checkbox.Tag = items;
                        checkbox.Text = $"{addSpacesRegex.Replace(itemCategory.ToString(), " $1")}: +{items.Count}";
                        checkbox.CheckAlign = ContentAlignment.MiddleRight;
                        checkbox.TextAlign = ContentAlignment.MiddleRight;
                        checkbox.Width = 148;
                        checkbox.Margin = new Padding(3, 3, 6, 3);
                        var description = itemCategory.GetAttribute<DescriptionAttribute>()?.Description;
                        if (description != null)
                        {
                            TooltipBuilder.SetTooltip(checkbox, description);
                        }
                        checkbox.CheckStateChanged += cItemCategory2_CheckStateChanged;
                    }

                    if (locationCategory > 0 && itemCategory > 0)
                    {
                        var items = itemsByItemCategory[itemCategory].Intersect(itemsByLocationCategory[locationCategory]).ToList();
                        if (items.Count > 0)
                        {
                            checkbox = new InvertIndeterminateCheckBox();
                            checkbox.Tag = items;
                            checkbox.Dock = DockStyle.Fill;
                            checkbox.CheckAlign = ContentAlignment.MiddleCenter;
                            checkbox.CheckStateChanged += cItemCategory2_CheckStateChanged;
                        }
                    }

                    if (checkbox != null)
                    {
                        tableItemPool.Controls.Add(checkbox, (int)locationCategory, (int)itemCategory - 1);
                    }
                }
            }
        }

        private bool _itemPoolRecalculating = false;
        private void cItemCategory2_CheckStateChanged(object sender, EventArgs e)
        {
            if (_itemPoolRecalculating)
            {
                return;
            }

            if (_configuration.GameplaySettings.CustomItemList == null)
            {
                _configuration.GameplaySettings.CustomItemList = new HashSet<Item>();
            }

            var checkbox = (CheckBox)sender;
            var items = (List<Item>)checkbox.Tag;
            if (checkbox.CheckState == CheckState.Unchecked)
            {
                foreach (var item in items)
                {
                    _configuration.GameplaySettings.CustomItemList.Remove(item);
                }
            }
            else
            {
                foreach (var item in items)
                {
                    _configuration.GameplaySettings.CustomItemList.Add(item);
                }
            }

            UpdateItemPoolCheckboxes();
        }

        private void UpdateItemPoolCheckboxes()
        {
            if (_itemPoolRecalculating)
            {
                return;
            }
            _itemPoolRecalculating = true;

            if (_configuration.GameplaySettings.CustomItemList != null)
            {
                // todo keep checkboxes cached
                var checkboxes = new List<CheckBox>();
                checkboxes.AddRange(pLocationCategories.Controls.OfType<CheckBox>().ToList());
                checkboxes.AddRange(tableItemPool.Controls.OfType<CheckBox>().ToList());
                checkboxes.AddRange(pClassicItemPool.Controls.OfType<CheckBox>().ToList());

                foreach (var otherCheckbox in checkboxes)
                {
                    var otherItems = (List<Item>)otherCheckbox.Tag;
                    var matchingItems = _configuration.GameplaySettings.CustomItemList.Intersect(otherItems).Count();
                    if (matchingItems == 0)
                    {
                        otherCheckbox.CheckState = CheckState.Unchecked;
                    }
                    else if (matchingItems == otherItems.Count)
                    {
                        otherCheckbox.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        otherCheckbox.CheckState = CheckState.Indeterminate;
                    }
                }

                tItemPool.Text = ItemUtils.ConvertItemListToString(ItemEditor.BaseItemList, _configuration.GameplaySettings.CustomItemList.ToList());
                _configuration.GameplaySettings.CustomItemListString = tItemPool.Text;
            }
            ItemEditor.UpdateChecks(tItemPool.Text);
            lItemPoolText.Text = ItemEditor.ExternalLabel;

            _itemPoolRecalculating = false;
        }

        private void InitializeShortenCutsceneSettings()
        {
            foreach (var shortenCutsceneGroup in typeof(ShortenCutsceneSettings)
                .GetProperties()
                )
            {
                var tabPage = new TabPage
                {
                    Tag = shortenCutsceneGroup,
                    Text = addSpacesRegex.Replace(shortenCutsceneGroup.Name, " $1"),
                    UseVisualStyleBackColor = true,
                };
                tShortenCutscenes.TabPages.Add(tabPage);

                var initialX = 6;
                var initialY = 7;
                var deltaX = 150;
                var deltaY = 23;
                var width = 150;
                var height = 23;
                var currentX = initialX;
                var currentY = initialY;
                foreach (var value in Enum.GetValues(shortenCutsceneGroup.PropertyType).Cast<Enum>())
                {
                    if (Convert.ToInt32(value) == 0)
                    {
                        continue;
                    }
                    var checkBox = new CheckBox
                    {
                        Tag = value,
                        Name = "cShortenCutscene_" + value.ToString(),
                        Text = addSpacesRegex.Replace(value.ToString(), " $1"),
                        Location = new Point(currentX, currentY),
                        Size = new Size(width, height),
                    };
                    var description = value.GetAttribute<DescriptionAttribute>()?.Description;
                    if (description != null)
                    {
                        TooltipBuilder.SetTooltip(checkBox, description);
                    }
                    checkBox.CheckedChanged += cShortenCutscene_CheckedChanged;
                    tabPage.Controls.Add(checkBox);
                    currentX += deltaX;
                    if (currentX > tShortenCutscenes.Width - width)
                    {
                        currentX = initialX;
                        currentY += deltaY;
                    }
                }
            }
        }

        private void InitalizeLowHealthSFXOptions()
        {
            string[] listOfOptions = Enum.GetNames(typeof(LowHealthSFX));
            this.cLowHealthSFXComboBox.Items.Clear();
            this.cLowHealthSFXComboBox.Items.AddRange(listOfOptions);
            this.cLowHealthSFXComboBox.SelectedItem = "Default";
        }

        private void cShortenCutscene_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var propertyInfo = (PropertyInfo)checkBox.Parent.Tag;
            var cutsceneFlag = (int)checkBox.Tag;
            if (_configuration.GameplaySettings.ShortenCutsceneSettings == null)
            {
                _configuration.GameplaySettings.ShortenCutsceneSettings = new ShortenCutsceneSettings();
            }
            var value = (int)propertyInfo.GetValue(_configuration.GameplaySettings.ShortenCutsceneSettings);
            var newValue = checkBox.Checked ? value | cutsceneFlag : value & ~cutsceneFlag;
            UpdateSingleSetting(() => propertyInfo.SetValue(_configuration.GameplaySettings.ShortenCutsceneSettings, newValue));
        }

        private void InitializeTransformationFormSettings()
        {
            foreach (var form in Enum.GetValues<TransformationForm>())
            {
                var tabPage = new TabPage
                {
                    Tag = form,
                    Text = addSpacesRegex.Replace(form.ToString(), " $1"),
                    UseVisualStyleBackColor = true,
                };
                var bTunic = CreateTunicColorButton(form);
                tabPage.Controls.Add(bTunic);
                tabPage.Controls.Add(CreateTunicColorCheckBox(form));
                tabPage.Controls.Add(CreateTunicColorRandomizeButton(form));
                tabPage.Controls.Add(new Label
                {
                    Name = "lTunicColorDefault",
                    Text = "Default",
                    Location = new Point(111, 3),
                    Size = new Size(135, 23),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Visible = !_configuration.CosmeticSettings.UseTunicColors.GetValueOrDefault(form, false),
                    BorderStyle = BorderStyle.FixedSingle,
                });

                if (form != TransformationForm.FierceDeity)
                {
                    tabPage.Controls.Add(new Label
                    {
                        Text = "Instrument:",
                        Location = new Point(24, 33),
                        Size = new Size(79, 13),
                    });
                    tabPage.Controls.Add(CreateInstrumentComboBox(form));
                }
                if (_configuration.CosmeticSettings.UseEnergyColors.TryGetValue(form, out var use))
                {
                    tabPage.Controls.Add(new Label
                    {
                        Name = "lEnergyColorDefault",
                        Text = "Default",
                        Location = new Point(111, 63),
                        Size = new Size(135, 23),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Visible = !use,
                        BorderStyle = BorderStyle.FixedSingle,
                    });
                    var bEnergy = CreateEnergyColorButtons(form);
                    foreach (var button in bEnergy)
                    {
                        tabPage.Controls.Add(button);
                    }
                    tabPage.Controls.Add(CreateEnergyColorCheckBox(form));
                    tabPage.Controls.Add(CreateEnergyColorRandomizeButton(form));
                }
                tFormCosmetics.TabPages.Add(tabPage);
            }
        }

        private CheckBox CreateEnergyColorRandomizeButton(TransformationForm transformationForm)
        {
            var button = new CheckBox
            {
                Appearance = Appearance.Button,
                Tag = transformationForm,
                Name = "cEnergyRandomize",
                Text = "🎲",
                Location = new Point(111 + (3 * 34), 62),
                Size = new Size(33, 25),
                TextAlign = ContentAlignment.TopRight,
            };
            button.Font = new Font(button.Font.FontFamily, 12);
            TooltipBuilder.SetTooltip(button, "Randomize the energy colors for this form.");
            button.Click += cEnergyRandomize_Click;
            return button;
        }

        private CheckBox CreateEnergyColorCheckBox(TransformationForm transformationForm)
        {
            var checkBox = new CheckBox
            {
                Tag = transformationForm,
                Name = "cEnergy",
                Text = "Energy color:",
                Location = new Point(6, 63),
                Size = new Size(120, 25),
            };
            checkBox.CheckedChanged += cEnergy_CheckedChanged;
            return checkBox;
        }

        private Button[] CreateEnergyColorButtons(TransformationForm transformationForm)
        {
            var current = _configuration.CosmeticSettings.EnergyColors[transformationForm];
            var buttons = new List<Button>();
            for (int i = 0; i < current.Length; i++)
            {
                var button = new Button
                {
                    Tag = transformationForm,
                    Name = $"bEnergy{i}",
                    Location = new Point(111 + (i * 34), 63),
                    Size = new Size(32, 23),
                    BackColor = current[i],
                    FlatStyle = FlatStyle.Flat,
                    Text = "",
                };
                TooltipBuilder.SetTooltip(button, "Select the energy color for this form.");
                button.Click += bEnergy_Click;
                buttons.Add(button);
            }
            return buttons.ToArray();
        }

        private CheckBox CreateTunicColorCheckBox(TransformationForm transformationForm)
        {
            var checkBox = new CheckBox
            {
                Tag = transformationForm,
                Name = "cTunic",
                Text = "Tunic color:",
                Location = new Point(6, 3),
                Size = new Size(102, 25),
            };
            checkBox.CheckedChanged += cTunic_CheckedChanged;
            return checkBox;
        }

        private Button CreateTunicColorButton(TransformationForm transformationForm)
        {
            var button = new Button
            {
                Tag = transformationForm,
                Name = "bTunic",
                Location = new Point(111, 3),
                Size = new Size(101, 23),
                BackColor = Color.FromArgb(0x1E, 0x69, 0x1B),
                FlatStyle = FlatStyle.Flat,
            };
            TooltipBuilder.SetTooltip(button, "Select the color of this form's Tunic.");
            button.Click += bTunic_Click;
            return button;
        }

        private void cTunicRandomize_Click(object sender, EventArgs e)
        {
            _isUpdating = true;

            var checkBox = (CheckBox)sender;
            var form = (TransformationForm)checkBox.Tag;
            var random = new Random();
            var selected = tFormCosmetics.SelectedTab;
            var bTunic = (Button)selected.Controls.Find($"bTunic", false)[0];
            if (checkBox.Checked)
            {
                var color = RandomUtils.GetRandomColor(random);
                // Update the color in cosmetic settings.
                _configuration.CosmeticSettings.TunicColors[form] = color;
                bTunic.BackColor = Color.Transparent;
                bTunic.Text = "?";
                bTunic.Enabled = false;
            }
            else
            {
                bTunic.BackColor = _configuration.CosmeticSettings.TunicColors[form];
                bTunic.Text = string.Empty;
                bTunic.Enabled = true;
            }

            _isUpdating = false;
        }

        private CheckBox CreateTunicColorRandomizeButton(TransformationForm transformationForm)
        {
            var button = new CheckBox
            {
                Appearance = Appearance.Button,
                Tag = transformationForm,
                Name = "cTunicRandomize",
                Text = "🎲",
                Location = new Point(112 + (3 * 34), 2),
                Size = new Size(33, 25),
                TextAlign = ContentAlignment.TopRight,
            };
            button.Font = new Font(button.Font.FontFamily, 12);
            TooltipBuilder.SetTooltip(button, "Randomize the tunic color for this form.");
            button.Click += cTunicRandomize_Click;
            return button;
        }

        private ComboBox CreateInstrumentComboBox(TransformationForm transformationForm)
        {
            var data = Enum.GetValues(typeof(Instrument)).Cast<Instrument>().ToDictionary(x => x, x => addSpacesRegex.Replace(x.ToString() + (x == transformationForm.DefaultInstrument() ? " *" : ""), " $1"));
            var comboBox = new ComboBox
            {
                Tag = transformationForm,
                Name = "cInstrument",
                Location = new Point(111, 30),
                Size = new Size(135, 21),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = new BindingSource(data, null),
                DisplayMember = "Value",
                ValueMember = "Key",
            };
            comboBox.SelectedIndexChanged += cInstruments_SelectedIndexChanged;
            return comboBox;
        }

        private void cInstruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var form = (TransformationForm)comboBox.Tag;
            var value = (Instrument)comboBox.SelectedValue;
            _configuration.CosmeticSettings.Instruments[form] = value;
        }

        #region Forms Code

        private void mmrMain_Load(object sender, EventArgs e)
        {
            // initialise some stuff
            _isUpdating = true;

            InitializeBackgroundWorker();

            LoadSettings();

            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var openWithArg = args[1];
                if (Path.GetExtension(openWithArg) == ".mmr")
                {
                    ttOutput.SelectedIndex = 1;
                    TogglePatchSettings(false);
                    _configuration.OutputSettings.InputPatchFilename = openWithArg;
                    tPatch.Text = _configuration.OutputSettings.InputPatchFilename;
                }
            }

            _isUpdating = false;
        }

        private void InitializeBackgroundWorker()
        {
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_WorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
        }

        private void bSkip_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cancellationTokenSource = (CancellationTokenSource)button.Tag;
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pProgress.Value = e.ProgressPercentage;
            var state = (BackgroundWorkerProgressState)e.UserState;
            lStatus.Text = state.Message;
            bSkip.Visible = state.CTSItemImportance != null;
            bSkip.Tag = state.CTSItemImportance;
        }

        private void bgWorker_WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pProgress.Value = 0;
            lStatus.Text = "Ready...";
            EnableAllControls(true);
            ToggleCheckBoxes();
            TogglePatchSettings(ttOutput.SelectedTab.TabIndex == 0);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TryRandomize(sender as BackgroundWorker, e);
        }

        private void cEnergy_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdating = true;

            var checkBox = (CheckBox)sender;
            var form = (TransformationForm)checkBox.Tag;
            _configuration.CosmeticSettings.UseEnergyColors[form] = checkBox.Checked;

            var current = _configuration.CosmeticSettings.EnergyColors[form];
            for (int i = 0; i < current.Length; i++)
            {
                var button = (Button)checkBox.Parent.Controls.Find($"bEnergy{i}", false)[0];
                button.Visible = checkBox.Checked;

                var label = (Label)checkBox.Parent.Controls.Find("lEnergyColorDefault", false)[0];
                label.Visible = !checkBox.Checked;

                var randomizeButton = (CheckBox)checkBox.Parent.Controls.Find("cEnergyRandomize", false)[0];
                randomizeButton.Visible = checkBox.Checked;
            }

            _isUpdating = false;
        }

        private void bEnergy_Click(object sender, EventArgs e)
        {
            var result = cEnergy.ShowDialog();
            if (result == DialogResult.OK)
            {
                _isUpdating = true;

                var button = (Button)sender;
                var form = (TransformationForm)button.Tag;
                var index = int.Parse(button.Name.Substring(7));
                _configuration.CosmeticSettings.EnergyColors[form][index] = cEnergy.Color;
                button.BackColor = cEnergy.Color;

                _isUpdating = false;
            }
        }

        private void cEnergyRandomize_Click(object sender, EventArgs e)
        {
            _isUpdating = true;

            var checkBox = (CheckBox)sender;
            var form = (TransformationForm)checkBox.Tag;
            var random = new Random();
            var selected = tFormCosmetics.SelectedTab;
            for (int i = 0; i < _configuration.CosmeticSettings.EnergyColors[form].Length; i++)
            {
                var bEnergy = (Button)selected.Controls.Find($"bEnergy{i}", false)[0];
                if (checkBox.Checked)
                {
                    var color = RandomUtils.GetRandomColor(random);
                    // Update the color in cosmetic settings.
                    _configuration.CosmeticSettings.EnergyColors[form][i] = color;
                    bEnergy.BackColor = Color.Transparent;
                    bEnergy.Text = "?";
                    bEnergy.Enabled = false;
                }
                else
                {
                    bEnergy.BackColor = _configuration.CosmeticSettings.EnergyColors[form][i];
                    bEnergy.Text = string.Empty;
                    bEnergy.Enabled = true;
                }
            }

            _isUpdating = false;
        }

        private void cTunic_CheckedChanged(object sender, EventArgs e)
        {
            _isUpdating = true;

            var checkBox = (CheckBox)sender;
            var form = (TransformationForm)checkBox.Tag;
            _configuration.CosmeticSettings.UseTunicColors[form] = checkBox.Checked;

            var button = (Button)checkBox.Parent.Controls.Find("bTunic", false)[0];
            button.Visible = checkBox.Checked;

            var label = (Label)checkBox.Parent.Controls.Find("lTunicColorDefault", false)[0];
            label.Visible = !checkBox.Checked;

            var randomizeButton = (CheckBox)checkBox.Parent.Controls.Find("cTunicRandomize", false)[0];
            randomizeButton.Visible = checkBox.Checked;

            _isUpdating = false;
        }

        private void bTunic_Click(object sender, EventArgs e)
        {
            var result = cTunic.ShowDialog();
            if (result == DialogResult.OK)
            {
                _isUpdating = true;

                var button = (Button)sender;
                var form = (TransformationForm)button.Tag;
                _configuration.CosmeticSettings.TunicColors[form] = cTunic.Color;
                button.BackColor = cTunic.Color;

                _isUpdating = false;
            }
        }

        private void bopen_Click(object sender, EventArgs e)
        {
            openROM.ShowDialog();

            _configuration.OutputSettings.InputROMFilename = openROM.FileName;
            tROMName.Text = _configuration.OutputSettings.InputROMFilename;
        }

        private void bLoadLogic_Click(object sender, EventArgs e)
        {
            if(openLogic.ShowDialog() == DialogResult.OK)
            {
                _configuration.GameplaySettings.UserLogicFileName = openLogic.FileName;
                tbUserLogic.Text = Path.GetFileNameWithoutExtension(_configuration.GameplaySettings.UserLogicFileName);
            }
            UpdateSettingLogicMarkers();
        }

        private void Randomize()
        {
            string validationResult;
            if (!string.IsNullOrWhiteSpace(_configuration.OutputSettings.InputPatchFilename))
            {
                validationResult = _configuration.OutputSettings.Validate();
                saveROM.FileName = Path.ChangeExtension(Path.GetFileName(_configuration.OutputSettings.InputPatchFilename), "z64");
            }
            else
            {
                validationResult = _configuration.GameplaySettings.Validate() ?? _configuration.OutputSettings.Validate();
                var defaultOutputROMFilename = FileUtils.MakeFilenameValid($"MMR-{typeof(Randomizer).Assembly.GetName().Version}-{DateTime.UtcNow:o}");
                saveROM.FileName = defaultOutputROMFilename;
            }

            if (validationResult != null)
            {
                MessageBox.Show(validationResult, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (saveROM.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _configuration.OutputSettings.OutputROMFilename = saveROM.FileName;

            EnableAllControls(false);
            bgWorker.RunWorkerAsync();
        }

        private void bRandomise_Click(object sender, EventArgs e)
        {
            Randomize();
        }

        private void bApplyPatch_Click(object sender, EventArgs e)
        {
            Randomize();
        }

        private void tSeed_Enter(object sender, EventArgs e)
        {
            _seedOld = Convert.ToInt32(tSeed.Text);
            _isUpdating = true;
        }

        private void tSeed_Leave(object sender, EventArgs e)
        {
            try
            {
                int seed = Convert.ToInt32(tSeed.Text);
                if (seed < 0)
                {
                    seed = Math.Abs(seed);
                    tSeed.Text = seed.ToString();
                }
            }
            catch
            {
                tSeed.Text = _seedOld.ToString();
                MessageBox.Show("Invalid seed: must be a positive integer.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            _isUpdating = false;
        }

        private void UpdateCheckboxes()
        {
            cSpoiler.Checked = _configuration.OutputSettings.GenerateSpoilerLog;
            cHTMLLog.Checked = _configuration.OutputSettings.GenerateHTMLLog;
            cN64.Checked = _configuration.OutputSettings.GenerateROM;
            cVC.Checked = _configuration.OutputSettings.OutputVC;
            cPatch.Checked = _configuration.OutputSettings.GeneratePatch;

            cItemPlacement.SelectedIndex = (int)_configuration.GameplaySettings.ItemPlacement;
            cMixSongs.Checked = _configuration.GameplaySettings.AddSongs;
            cProgressiveUpgrades.Checked = _configuration.GameplaySettings.ProgressiveUpgrades;
            cDEnt.Checked = _configuration.GameplaySettings.RandomizeDungeonEntrances;
            cShuffleBosses.Checked = _configuration.GameplaySettings.RandomizeBossRooms;
            cSFX.Checked = _configuration.CosmeticSettings.RandomizeSounds;
            cEnemy.Checked = _configuration.GameplaySettings.RandomizeEnemies;
            if (_configuration.GameplaySettings.ShortenCutsceneSettings == null)
            {
                _configuration.GameplaySettings.ShortenCutsceneSettings = new ShortenCutsceneSettings();
            }
            foreach (TabPage shortenCutsceneTab in tShortenCutscenes.TabPages)
            {
                var shortenCutsceneGroup = (PropertyInfo)shortenCutsceneTab.Tag;
                var value = (Enum)shortenCutsceneGroup.GetValue(_configuration.GameplaySettings.ShortenCutsceneSettings);
                foreach (var flagValue in Enum.GetValues(shortenCutsceneGroup.PropertyType).Cast<Enum>())
                {
                    if (Convert.ToInt32(flagValue) == 0)
                    {
                        continue;
                    }
                    var cShortenCutscene = (CheckBox)shortenCutsceneTab.Controls.Find("cShortenCutscene_" + flagValue.ToString(), false)[0];
                    cShortenCutscene.Checked = value.HasFlag(flagValue);
                }
            }
            foreach (TabPage dungeonModeTab in tOtherCustomizations.TabPages)
            {
                var propertyInfo = (PropertyInfo)dungeonModeTab.Tag;
                if (propertyInfo == null)
                {
                    continue;
                }
                var value = (Enum)propertyInfo.GetValue(_configuration.GameplaySettings);
                foreach (var flagValue in Enum.GetValues(propertyInfo.PropertyType).Cast<Enum>())
                {
                    if (Convert.ToInt32(flagValue) == 0)
                    {
                        continue;
                    }
                    var cDungeonMode = (CheckBox)dungeonModeTab.Controls.Find("cDungeonMode_" + flagValue.ToString(), false)[0];
                    cDungeonMode.Checked = value.HasFlag(flagValue);
                }
            }
            cQText.Checked = _configuration.GameplaySettings.QuickTextEnabled;
            cFreeHints.Checked = _configuration.GameplaySettings.FreeHints;
            cFreeGaroHints.Checked = _configuration.GameplaySettings.FreeGaroHints;
            cGossipsTolerant.Checked = _configuration.GameplaySettings.TolerantGossipStones;
            cEasyFrameByFrame.Checked = _configuration.GameplaySettings.EasyFrameByFrame;
            cMixGaroWithGossip.Checked = _configuration.GameplaySettings.MixGossipAndGaroHints;
            cClearHints.Checked = _configuration.GameplaySettings.ClearHints;
            cClearGaroHints.Checked = _configuration.GameplaySettings.ClearGaroHints;
            cImportanceCount.Checked = _configuration.GameplaySettings.ImportanceCount;
            cImportanceCountGaro.Checked = _configuration.GameplaySettings.ImportanceCountGaro;
            cHintImportance.Checked = _configuration.GameplaySettings.HintsIndicateImportance;
            cHideClock.Checked = _configuration.GameplaySettings.HideClock;
            cSunsSong.Checked = _configuration.GameplaySettings.EnableSunsSong;
            cFDAnywhere.Checked = _configuration.GameplaySettings.AllowFierceDeityAnywhere;
            cByoAmmo.Checked = _configuration.GameplaySettings.ByoAmmo;
            cDeathMoonCrash.Checked = _configuration.GameplaySettings.DeathMoonCrash;
            cMoonCrashFileErase.Checked = _configuration.GameplaySettings.MoonCrashErasesFile;
            cFewerHealthDrops.Checked = _configuration.GameplaySettings.FewerHealthDrops;
            cTakeDamageOnEpona.Checked = _configuration.GameplaySettings.TakeDamageOnEpona;
            cTakeDamageWhileShielding.Checked = _configuration.GameplaySettings.TakeDamageWhileShielding;
            cTakeDamageFromVoid.Checked = _configuration.GameplaySettings.TakeDamageFromVoid;
            cTakeDamageFromGorons.Checked = _configuration.GameplaySettings.TakeDamageFromGorons;
            cTakeDamageFromGibdosFaster.Checked = _configuration.GameplaySettings.TakeDamageFromGibdosFaster;
            cTakeDamageGettingCaught.Checked = _configuration.GameplaySettings.TakeDamageGettingCaught;
            cTakeDamageFromDog.Checked = _configuration.GameplaySettings.TakeDamageFromDog;
            cTakeDamageFromDexihands.Checked = _configuration.GameplaySettings.TakeDamageFromDexihands;
            cIceTrapQuirks.Checked = _configuration.GameplaySettings.TrapQuirks;
            cClockSpeed.SelectedIndex = (int)_configuration.GameplaySettings.ClockSpeed;
            cAutoInvert.SelectedIndex = (int)_configuration.GameplaySettings.AutoInvert;
            cNoDowngrades.Checked = _configuration.GameplaySettings.PreventDowngrades;
            cShopAppearance.Checked = _configuration.GameplaySettings.UpdateShopAppearance;
            cStartingItems.SelectedIndex = (int)_configuration.GameplaySettings.StartingItemMode;
            cRequiredBossRemains.SelectedIndex = _configuration.GameplaySettings.RequiredBossRemains;
            cEponaSword.Checked = _configuration.GameplaySettings.FixEponaSword;
            cUpdateChests.Checked = _configuration.GameplaySettings.UpdateChests;
            cUpdateNpcText.Checked = _configuration.GameplaySettings.UpdateNPCText;
            cOathHint.Checked = _configuration.GameplaySettings.OathHint;
            cRemainsHint.Checked = _configuration.GameplaySettings.RemainsHint;
            cFairyAndSkullHints.Checked = _configuration.GameplaySettings.FairyAndSkullHint;
            cSkipBeaver.Checked = _configuration.GameplaySettings.SpeedupBeavers;
            cGoodDampeRNG.Checked = _configuration.GameplaySettings.SpeedupDampe;
            cGoodDogRaceRNG.Checked = _configuration.GameplaySettings.SpeedupDogRace;
            cFasterLabFish.Checked = _configuration.GameplaySettings.SpeedupLabFish;
            cFasterBank.Checked = _configuration.GameplaySettings.SpeedupBank;
            cDoubleArcheryRewards.Checked = _configuration.GameplaySettings.DoubleArcheryRewards;
            cSpeedupBabyCucco.Checked = _configuration.GameplaySettings.SpeedupBabyCuccos;
            cGiantMaskAnywhere.Checked = _configuration.GameplaySettings.GiantMaskAnywhere;

            cDMult.SelectedIndex = (int)_configuration.GameplaySettings.DamageMode;
            cDType.SelectedIndex = (int)_configuration.GameplaySettings.DamageEffect;
            cMode.SelectedIndex = (int)_configuration.GameplaySettings.LogicMode;
            cLink.SelectedIndex = (int)_configuration.GameplaySettings.Character;
            cTatl.SelectedIndex = (int)_configuration.CosmeticSettings.TatlColorSchema;
            cCameraStyle.SelectedIndex = (int)_configuration.CosmeticSettings.CameraStyle;
            cGravity.SelectedIndex = (int)_configuration.GameplaySettings.MovementMode;
            cLowHealthSFXComboBox.SelectedIndex = cLowHealthSFXComboBox.Items.IndexOf(_configuration.CosmeticSettings.LowHealthSFX.ToString());
            cNutAndStickDrops.SelectedIndex = (int)_configuration.GameplaySettings.NutandStickDrops;
            cFloors.SelectedIndex = (int)_configuration.GameplaySettings.FloorType;
            cGossipHints.SelectedIndex = (int)_configuration.GameplaySettings.GossipHintStyle;
            cGaroHint.SelectedIndex = (int)_configuration.GameplaySettings.GaroHintStyle;
            cBlastCooldown.SelectedIndex = (int)_configuration.GameplaySettings.BlastMaskCooldown;
            cTrapAmount.SelectedIndex = (int)_configuration.GameplaySettings.TrapAmount;
            cTrapsAppearance.SelectedIndex = (int)_configuration.GameplaySettings.TrapAppearance;
            cMusic.SelectedIndex = (int)_configuration.CosmeticSettings.Music;
            foreach (TabPage cosmeticFormTab in tFormCosmetics.TabPages)
            {
                var form = (TransformationForm)cosmeticFormTab.Tag;

                var cTunic = (CheckBox)cosmeticFormTab.Controls.Find("cTunic", false)[0];
                cTunic.Checked = _configuration.CosmeticSettings.UseTunicColors.GetValueOrDefault(form);

                var bTunic = cosmeticFormTab.Controls.Find("bTunic", false)[0];
                bTunic.Visible = cTunic.Checked;
                bTunic.BackColor = _configuration.CosmeticSettings.TunicColors.GetValueOrDefault(form);

                var cTunicRandomize = (CheckBox)cosmeticFormTab.Controls.Find("cTunicRandomize", false)[0];
                cTunicRandomize.Visible = cTunic.Checked;

                var lTunicColorDefault = (Label)cosmeticFormTab.Controls.Find("lTunicColorDefault", false)[0];
                lTunicColorDefault.Visible = !cTunic.Checked;

                if (form != TransformationForm.FierceDeity)
                {
                    var cInstrument = (ComboBox)cosmeticFormTab.Controls.Find("cInstrument", false)[0];
                    cInstrument.SelectedValue = _configuration.CosmeticSettings.Instruments[form];
                }

                if (_configuration.CosmeticSettings.UseEnergyColors.TryGetValue(form, out var use))
                {
                    var cEnergy = (CheckBox)cosmeticFormTab.Controls.Find("cEnergy", false)[0];
                    cEnergy.Checked = use;

                    var colors = _configuration.CosmeticSettings.EnergyColors[form];
                    for (int i = 0; i < colors.Length; i++)
                    {
                        var bEnergy = (Button)cosmeticFormTab.Controls.Find($"bEnergy{i}", false)[0];
                        bEnergy.BackColor = colors[i];
                        bEnergy.Visible = use;

                        var cEnergyRandomize = (CheckBox)cosmeticFormTab.Controls.Find("cEnergyRandomize", false)[0];
                        cEnergyRandomize.Visible = use;

                        var lEnergyColorDefault = (Label)cosmeticFormTab.Controls.Find("lEnergyColorDefault", false)[0];
                        lEnergyColorDefault.Visible = !use;
                    }
                }
            }
            cTargettingStyle.Checked = _configuration.CosmeticSettings.EnableHoldZTargeting;
            cInstantPictobox.Checked = !_configuration.CosmeticSettings.KeepPictoboxAntialiasing;
            cEnableNightMusic.Checked = _configuration.CosmeticSettings.EnableNightBGM;
            cRemoveMinorMusic.Checked = _configuration.CosmeticSettings.RemoveMinorMusic;
            cMusicTrackNames.Checked = _configuration.CosmeticSettings.ShowTrackName;
            cDisableFanfares.Checked = _configuration.CosmeticSettings.DisableFanfares;
            cBombTrapTunicColors.Checked = _configuration.CosmeticSettings.BombTrapsRandomizeTunicColor;
            cRainbowTunic.Checked = _configuration.CosmeticSettings.RainbowTunic;

            // Misc config options
            cDisableCritWiggle.Checked = _configuration.GameplaySettings.CritWiggleDisable;
            _drawHashChecked = _configuration.GameplaySettings.DrawHash;
            cDrawHash.Checked = _configuration.OutputSettings.GeneratePatch || (_drawHashChecked && (_configuration.OutputSettings.GenerateROM || _configuration.OutputSettings.OutputVC));
            cFastPush.Checked = _configuration.GameplaySettings.FastPush;
            cQuestItemStorage.Checked = _configuration.GameplaySettings.QuestItemStorage;
            cQuestItemKeep.Checked = _configuration.GameplaySettings.KeepQuestTradeThroughTime;
            cContinuousDekuHopping.Checked = _configuration.GameplaySettings.ContinuousDekuHopping;
            cIronGoron.Checked = _configuration.GameplaySettings.IronGoron;
            cHookshotAnySurface.Checked = _configuration.GameplaySettings.HookshotAnySurface;
            cClimbMostSurfaces.Checked = _configuration.GameplaySettings.ClimbMostSurfaces;
            cUnderwaterOcarina.Checked = _configuration.GameplaySettings.OcarinaUnderwater;
            cFreestanding.Checked = _configuration.GameplaySettings.UpdateWorldModels;
            cArrowCycling.Checked = _configuration.GameplaySettings.ArrowCycling;
            cCloseCows.Checked = _configuration.GameplaySettings.CloseCows;
            cCombatMusicDisable.Checked = _configuration.CosmeticSettings.DisableCombatMusic;
            cHueShiftMiscUI.Checked = _configuration.CosmeticSettings.ShiftHueMiscUI;
            cElegySpeedups.Checked = _configuration.GameplaySettings.ElegySpeedup;
            cImprovedPictobox.Checked = _configuration.GameplaySettings.EnablePictoboxSubject;
            cLenientGoronSpikes.Checked = _configuration.GameplaySettings.LenientGoronSpikes;
            cTargetHealth.Checked = _configuration.GameplaySettings.TargetHealthBar;
            cFreeScarecrow.Checked = _configuration.GameplaySettings.FreeScarecrow;
            cFillWallet.Checked = _configuration.GameplaySettings.FillWallet;
            cInvisSparkle.Checked = _configuration.GameplaySettings.HiddenRupeesSparkle;
            cSaferGlitches.Checked = _configuration.GameplaySettings.SaferGlitches;
            cAddBombchuDrops.Checked = _configuration.GameplaySettings.BombchuDrops;
            cInstantTransformations.Checked = _configuration.GameplaySettings.InstantTransform;
            cBombArrows.Checked = _configuration.GameplaySettings.BombArrows;
            cVanillaMoonTrials.Checked = _configuration.GameplaySettings.VanillaMoonTrialAccess;
            cChestGameMinimap.SelectedIndex = (int)_configuration.GameplaySettings.ChestGameMinimap;
            cFairyMaskShimmer.Checked = _configuration.GameplaySettings.FairyMaskShimmer;
            cSkulltulaTokenSounds.Checked = _configuration.GameplaySettings.SkulltulaTokenSounds;

            foreach (var trapType in Enum.GetValues<TrapType>())
            {
                if (Convert.ToInt32(trapType) == 0)
                {
                    continue;
                }

                var nTrap = (NumericUpDown)gTraps.Controls.Find($"nTrap_{trapType}", false)[0];
                nTrap.Value = _configuration.GameplaySettings.TrapWeights.GetValueOrDefault(trapType);
            }

            nMaxGossipWotH.Value = _configuration.GameplaySettings.OverrideNumberOfRequiredGossipHints ?? 3;
            nMaxGossipFoolish.Value = _configuration.GameplaySettings.OverrideNumberOfNonRequiredGossipHints ?? 3;
            nMaxGossipCT.Value = _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGossipHints?? 2;

            nMaxGaroWotH.Value = _configuration.GameplaySettings.OverrideNumberOfRequiredGaroHints ?? 2;
            nMaxGaroFoolish.Value = _configuration.GameplaySettings.OverrideNumberOfNonRequiredGaroHints ?? 2;
            nMaxGaroCT.Value = _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGaroHints ?? 0;

            cCustomGossipWoth.Checked = _configuration.GameplaySettings.OverrideNumberOfRequiredGossipHints.HasValue
                || _configuration.GameplaySettings.OverrideNumberOfNonRequiredGossipHints.HasValue
                || _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGossipHints.HasValue;

            cCustomGaroWoth.Checked = _configuration.GameplaySettings.OverrideNumberOfRequiredGaroHints.HasValue
                || _configuration.GameplaySettings.OverrideNumberOfNonRequiredGaroHints.HasValue
                || _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGaroHints.HasValue;

            // HUD config options
            var heartItems = ColorSelectionManager.Hearts.GetItems();
            var heartSelection = heartItems.FirstOrDefault(csi => csi.Name == _configuration.CosmeticSettings.HeartsSelection);
            if (heartSelection != null)
            {
                cHUDHeartsComboBox.SelectedIndex = Array.IndexOf(heartItems, heartSelection);
            }

            var magicItems = ColorSelectionManager.MagicMeter.GetItems();
            var magicSelection = magicItems.FirstOrDefault(csi => csi.Name == _configuration.CosmeticSettings.MagicSelection);
            if (magicSelection != null)
            {
                cHUDMagicComboBox.SelectedIndex = Array.IndexOf(magicItems, magicSelection);
            }
        }

        private void tSeed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cDummy.Select();
            }
        }

        private bool _drawHashChecked;
        private void cPatch_CheckedChanged(object sender, EventArgs e)
        {
            var oldDrawHashChecked = _drawHashChecked;
            cDrawHash.Checked = cPatch.Checked ? true : _drawHashChecked && (_configuration.OutputSettings.GenerateROM || _configuration.OutputSettings.OutputVC);
            _configuration.GameplaySettings.DrawHash = cDrawHash.Checked;
            _drawHashChecked = oldDrawHashChecked;
        }

        private void cDrawHash_CheckedChanged(object sender, EventArgs e)
        {
            _drawHashChecked = cDrawHash.Checked;
        }

        private void cMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdating)
            {
                return;
            }

            var logicMode = (LogicMode)cMode.SelectedIndex;

            if (logicMode == LogicMode.UserLogic)
            {
                tbUserLogic.Enabled = true;
                bLoadLogic.Enabled = true;
            }
            else
            {
                tbUserLogic.Enabled = false;
                bLoadLogic.Enabled = false;
            }

            UpdateSingleSetting(() =>
            {
                if (_configuration.GameplaySettings.LogicMode != logicMode)
                {
                    _configuration.GameplaySettings.EnabledTricks.Clear();
                    List<string> tricksToAdd;
                    switch (logicMode)
                    {
                        case LogicMode.Casual:
                            tricksToAdd = new List<string>
                            {
                                "Exit OSH Without Goron",
                                "Lensless Chests",
                                "Day 2 Grave Without Lens of Truth",
                                "SHT Lensless Walls/Ceilings",
                                "Pinnacle Rock without Seahorse",
                                "Run Through Poisoned Water",
                                "WFT 2nd Floor Skip",
                            };
                            break;
                        case LogicMode.Glitched:
                            tricksToAdd = new List<string>
                            {
                                "Scarecrow's Song",
                                "Lensless Chests",
                                "Long Jump",
                                "Run Through Poisoned Water",
                                "Poisoned Water as Goron",
                                "Swim to Zora Hall as Human",
                                "Brute Force OSH Code",
                                "Climb Stone Tower with One Transformation",
                                "Deku Playground Rupee Displacement",
                                "Bomb Hovering",
                                "Lensless Jumping",
                                "Goron Roll Item Grabs",
                                "Melt Sun Blocks With Water",
                                "Powder Keg Storage",
                                "Hookshot Clip",
                                "Day 1 Grave Clip",
                                "Icicle Clip",
                                "Ocarina Dive",
                                "Item Dive",
                                "SHT BK Skip",
                                "Lensless Walking",
                                "Bomber Guard Skip",
                                "Deku Guard Skip",
                                "WFT 2nd Floor Skip",
                                "Kill Deku Shrine Big Octo",
                                "Deku Palace Bean Skip",
                                "Ikana Castle Falling Ceiling Skip",
                                "Goron Bomb Jump",
                                "GBT Fireless",
                                "Ikana Canyon Iceless",
                                "SHT Zora Jumps",
                                "Pinnacle Rock without Seahorse",
                                "Jump Slash through One Sided Geometry",
                                "Avoid Swamp Tree Bat",
                                "Goron Pound onto Ledges",
                                "Zora Hall Scrub Ledge Climb",
                                "Lensless Walls/Ceilings",
                                "Termina Stump with No Items",
                                "ISTT Hookshot to Eyegore",
                                "Ocean Skulltulas without Fire Arrows",
                                "SHT Jump to Stray Fairies",
                                "Clever Ice Platforms",
                                "Inn Balcony Jump",
                                "Shoot Goht",
                                "STT Water Tunnel as Human",
                                "Clever Bombchu Usage",
                                "STT Eyegore Bridge Jumps",
                                "Out of Bounds",
                                "Inn Balcony with Cucco",
                                "STT Updrafts without Deku Mask",
                                "Zora Boomerang Through Walls",
                                "Ocarina Items",
                                "Action Swap",
                                "Long Bomb Hovers",
                                "Keeta with Minimal Items",
                                "Time Stop",
                                "Blast Mask Hovers",
                                "Path to Snowhead without Magic",
                                "Recoil Flip Through Ice",
                                "Postman without Bunny Hood",
                                "Deku Recoil",
                                "Deliver Deku Princess Without Deku Mask",
                                "Restricted Items",
                                "Jump Slash Take Downs",
                                "STT BK Skip",
                                "Shoot Twinmold",
                                "SHT Pillar Skip",
                                "Lensless Climbing",
                                "ISTT Early Boss Key",
                                "Powder Kegs as Explosives",
                                "Recoil Flip",
                                "ISTT Lightless Boss Key",
                                "Superslide",
                            };
                            break;
                        default:
                            tricksToAdd = new List<string>();
                            break;
                    }
                    foreach (var trick in tricksToAdd)
                    {
                        _configuration.GameplaySettings.EnabledTricks.Add(trick);
                    }
                    UpdateNumTricksEnabled();
                }
                _configuration.GameplaySettings.LogicMode = logicMode;
                if (logicMode != LogicMode.UserLogic)
                {
                    _configuration.GameplaySettings.UserLogicFileName = string.Empty;
                    tbUserLogic.Text = string.Empty;
                }
                UpdateSettingLogicMarkers();
            });
        }

        private void UpdateNumTricksEnabled()
        {
            var count = _configuration.GameplaySettings.EnabledTricks.Count;
            lNumTricksEnabled.Text = $"{count} trick{(count != 1 ? "s" : "")} enabled";
        }

        private void mExit_Click(object sender, EventArgs e)
        {
            SaveAndClose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            SaveAndClose();
        }

        private void SaveAndClose()
        {
            SaveSettings();
            Application.Exit();
        }

        private void mAbout_Click(object sender, EventArgs e)
        {
            About.ShowDialog();
        }

        private void mManual_Click(object sender, EventArgs e)
        {
            Manual.Show();
        }

        private void mLogicEdit_Click(object sender, EventArgs e)
        {
            LogicEditor.Show();
        }

        private void bStartingItemEditor_Click(object sender, EventArgs e)
        {
            if (StartingItemEditor.ShowDialog() == DialogResult.Cancel)
            {
                tStartingItemList.Text = StartingItemEditor.CustomStartingItemListString;
            }
        }

        private void tStartingItemList_TextChanged(object sender, EventArgs e)
        {
            StartingItemEditor.UpdateChecks(tStartingItemList.Text);
            UpdateCustomStartingItemAmountLabel();
        }

        private void UpdateCustomStartingItemAmountLabel()
        {
            _configuration.GameplaySettings.CustomStartingItemList = StartingItemEditor.CustomStartingItemList.ToList();
            _configuration.GameplaySettings.CustomStartingItemListString = StartingItemEditor.CustomStartingItemListString;
            lCustomStartingItemAmount.Text = StartingItemEditor.ExternalLabel;
        }

        private void bJunkLocationsEditor_Click(object sender, EventArgs e)
        {
            if (JunkLocationEditor.ShowDialog() == DialogResult.Cancel)
            {
                tJunkLocationsList.Text = JunkLocationEditor.CustomJunkLocationsString;
            }
        }

        private void tJunkLocationsList_TextChanged(object sender, EventArgs e)
        {
            JunkLocationEditor.UpdateChecks(tJunkLocationsList.Text);
            UpdateJunkLocationAmountLabel();
        }

        private void UpdateJunkLocationAmountLabel()
        {
            _configuration.GameplaySettings.CustomJunkLocations = JunkLocationEditor.CustomJunkLocations.ToList();
            _configuration.GameplaySettings.CustomJunkLocationsString = JunkLocationEditor.CustomJunkLocationsString;
            lJunkLocationsAmount.Text = JunkLocationEditor.ExternalLabel;
        }



        /// <summary>
        /// Checks for settings that invalidate others, and disable the checkboxes for them.
        /// </summary>
        private void ToggleCheckBoxes()
        {
            var vanillaMode = _configuration.GameplaySettings.LogicMode == LogicMode.Vanilla;
            cMixSongs.Enabled = !vanillaMode;
            cItemPlacement.Enabled = !vanillaMode;
            cProgressiveUpgrades.Enabled = !vanillaMode;
            foreach (Control control in tabItemPool.Controls)
            {
                control.Enabled = !vanillaMode;
            }
            foreach (Control control in tableItemPool.Controls)
            {
                control.Enabled = !vanillaMode;
            }
            cDEnt.Enabled = !vanillaMode;
            cShuffleBosses.Enabled = !vanillaMode;
            cSpoiler.Enabled = !vanillaMode;
            cHTMLLog.Enabled = !vanillaMode;
            cGossipHints.Enabled = !vanillaMode;
            cGaroHint.Enabled = !vanillaMode;
            cStartingItems.Enabled = !vanillaMode;
            cRequiredBossRemains.Enabled = !vanillaMode;
            tJunkLocationsList.Enabled = !vanillaMode;
            bJunkLocationsEditor.Enabled = !vanillaMode;
            bToggleTricks.Enabled = !vanillaMode && _configuration.GameplaySettings.LogicMode != LogicMode.NoLogic;
            cTrapAmount.Enabled = !vanillaMode;
            cTrapsAppearance.Enabled = !vanillaMode;
            cIceTrapQuirks.Enabled = !vanillaMode;

            bLoadLogic.Enabled = _configuration.GameplaySettings.LogicMode == LogicMode.UserLogic;

            var oldEnabled = cDrawHash.Enabled;
            cDrawHash.Enabled = !_configuration.OutputSettings.GeneratePatch && (_configuration.OutputSettings.GenerateROM || _configuration.OutputSettings.OutputVC);
            if (cDrawHash.Enabled != oldEnabled)
            {
                cDrawHash.CheckedChanged -= cDrawHash_CheckedChanged;
                cDrawHash.Checked = cDrawHash.Enabled ? _drawHashChecked : _configuration.OutputSettings.GeneratePatch;
                cDrawHash.CheckedChanged += cDrawHash_CheckedChanged;
            }

            if (_configuration.GameplaySettings.GossipHintStyle == GossipHintStyle.Default || _configuration.GameplaySettings.LogicMode == LogicMode.Vanilla)
            {
                cClearHints.Enabled = false;
            }
            else
            {
                cClearHints.Enabled = true;
            }

            if (_configuration.GameplaySettings.GaroHintStyle == GossipHintStyle.Default || _configuration.GameplaySettings.LogicMode == LogicMode.Vanilla)
            {
                cClearGaroHints.Enabled = false;
            }
            else
            {
                cClearGaroHints.Enabled = true;
            }

            cCustomGossipWoth.Enabled = _configuration.GameplaySettings.GossipHintStyle == GossipHintStyle.Competitive;
            cImportanceCount.Enabled = _configuration.GameplaySettings.GossipHintStyle == GossipHintStyle.Competitive;
            nMaxGossipWotH.Enabled = cCustomGossipWoth.Checked && cCustomGossipWoth.Enabled;
            nMaxGossipFoolish.Enabled = nMaxGossipWotH.Enabled;
            nMaxGossipCT.Enabled = nMaxGossipWotH.Enabled;

            cCustomGaroWoth.Enabled = _configuration.GameplaySettings.GaroHintStyle == GossipHintStyle.Competitive;
            cImportanceCountGaro.Enabled = _configuration.GameplaySettings.GaroHintStyle == GossipHintStyle.Competitive;
            nMaxGaroWotH.Enabled = cCustomGaroWoth.Checked && cCustomGaroWoth.Enabled;
            nMaxGaroFoolish.Enabled = nMaxGaroWotH.Enabled;
            nMaxGaroCT.Enabled = nMaxGaroWotH.Enabled;

            cMixGaroWithGossip.Enabled = _configuration.GameplaySettings.GaroHintStyle == _configuration.GameplaySettings.GossipHintStyle && _configuration.GameplaySettings.GaroHintStyle == GossipHintStyle.Competitive;
            cHintImportance.Enabled = _configuration.GameplaySettings.GaroHintStyle == GossipHintStyle.Competitive || _configuration.GameplaySettings.GossipHintStyle == GossipHintStyle.Competitive;
            bCustomizeHintPriorities.Enabled = cHintImportance.Enabled;

            tLuckRollPercentage.Enabled = _configuration.CosmeticSettings.Music == Music.Random;

            cQuestItemKeep.Enabled = _configuration.GameplaySettings.QuestItemStorage;
            if (!cQuestItemKeep.Enabled)
            {
                _configuration.GameplaySettings.KeepQuestTradeThroughTime = false;
                cQuestItemKeep.Checked = false;
            }
        }

        /// <summary>
        /// Utility function that takes a function should update a single setting. 
        /// This function makes sure concurrent updates are not allowed, updates 
        /// settings string and enables/disables checkboxes automatically.
        /// </summary>
        /// <param name="update">A setting-updating function</param>
        private void UpdateSingleSetting(Action update)
        {
            if (_isUpdating)
            {
                return;
            }

            _isUpdating = true;

            update?.Invoke();
            ToggleCheckBoxes();

            _isUpdating = false;
        }

        private void EnableAllControls(bool v)
        {
            cMode.Enabled = v;
            bLoadLogic.Enabled = v;
            bStartingItemEditor.Enabled = v;
            tStartingItemList.Enabled = v;
            bJunkLocationsEditor.Enabled = v;
            tJunkLocationsList.Enabled = v;

            cDEnt.Enabled = v;
            cShuffleBosses.Enabled = v;
            cStartingItems.Enabled = v;
            cRequiredBossRemains.Enabled = v;
            cMixSongs.Enabled = v;
            cProgressiveUpgrades.Enabled = v;
            cEnemy.Enabled = v;
            cItemPlacement.Enabled = v;

            //bHumanTunic.Enabled = v;
            tFormCosmetics.Enabled = v;
            cTatl.Enabled = v;
            cMusic.Enabled = v;
            cEnableNightMusic.Enabled = v;
            cRemoveMinorMusic.Enabled = v;
            cDisableFanfares.Enabled = v;
            cMusicTrackNames.Enabled = v;
            cLink.Enabled = v;

            cHUDHeartsComboBox.Enabled = v;
            cHUDMagicComboBox.Enabled = v;
            btn_hud.Enabled = v;

            cGossipHints.Enabled = v;
            cFreeHints.Enabled = v;
            cFreeGaroHints.Enabled = v;
            cGossipsTolerant.Enabled = v;
            cEasyFrameByFrame.Enabled = v;
            cClearHints.Enabled = v;
            cGaroHint.Enabled = v;
            cClearGaroHints.Enabled = v;
            cMixGaroWithGossip.Enabled = v;
            cHintImportance.Enabled = v;

            cCameraStyle.Enabled = v;
            cTargettingStyle.Enabled = v;
            cInstantPictobox.Enabled = v;
            cRainbowTunic.Enabled = v;
            cBombTrapTunicColors.Enabled = v;
            cSFX.Enabled = v;
            cDisableCritWiggle.Enabled = v;
            cQText.Enabled = v;
            cFastPush.Enabled = v;
            cShopAppearance.Enabled = v;
            cUpdateChests.Enabled = v;
            cUpdateNpcText.Enabled = v;
            cNoDowngrades.Enabled = v;
            cEponaSword.Enabled = v;
            cQuestItemStorage.Enabled = v;
            cQuestItemKeep.Enabled = v;
            cFreestanding.Enabled = v;
            cArrowCycling.Enabled = v;
            cCloseCows.Enabled = v;
            cElegySpeedups.Enabled = v;
            cImprovedPictobox.Enabled = v;
            cLenientGoronSpikes.Enabled = v;
            cTargetHealth.Enabled = v;
            cFreeScarecrow.Enabled = v;
            cFillWallet.Enabled = v;
            cInvisSparkle.Enabled = v;
            cSaferGlitches.Enabled = v;
            cAddBombchuDrops.Enabled = v;
            cInstantTransformations.Enabled = v;
            cBombArrows.Enabled = v;
            cVanillaMoonTrials.Enabled = v;
            cChestGameMinimap.Enabled = v;
            cFairyMaskShimmer.Enabled = v;
            cSkulltulaTokenSounds.Enabled = v;

            cSkipBeaver.Enabled = v;
            cGoodDampeRNG.Enabled = v;
            cFasterLabFish.Enabled = v;
            cGoodDogRaceRNG.Enabled = v;
            cFasterBank.Enabled = v;
            cDoubleArcheryRewards.Enabled = v;
            cSpeedupBabyCucco.Enabled = v;

            cDMult.Enabled = v;
            cDType.Enabled = v;
            cGravity.Enabled = v;
            cFloors.Enabled = v;
            cClockSpeed.Enabled = v;
            cAutoInvert.Enabled = v;
            cBlastCooldown.Enabled = v;
            cTrapAmount.Enabled = v;
            cTrapsAppearance.Enabled = v;
            cHideClock.Enabled = v;
            cUnderwaterOcarina.Enabled = v;
            cSunsSong.Enabled = v;
            cFDAnywhere.Enabled = v;
            cByoAmmo.Enabled = v;
            cDeathMoonCrash.Enabled = v;
            cMoonCrashFileErase.Enabled = v;
            cFewerHealthDrops.Enabled = v;
            cTakeDamageOnEpona.Enabled = v;
            cTakeDamageWhileShielding.Enabled = v;
            cTakeDamageFromVoid.Enabled = v;
            cTakeDamageFromGorons.Enabled = v;
            cTakeDamageFromGibdosFaster.Enabled = v;
            cTakeDamageGettingCaught.Enabled = v;
            cTakeDamageFromDog.Enabled = v;
            cTakeDamageFromDexihands.Enabled = v;
            cIceTrapQuirks.Enabled = v;
            cHookshotAnySurface.Enabled = v;
            cClimbMostSurfaces.Enabled = v;

            foreach (Control control in tabItemPool.Controls)
            {
                control.Enabled = v;
            }

            foreach (Control control in tableItemPool.Controls)
            {
                control.Enabled = v;
            }

            cDummy.Enabled = v;
            bopen.Enabled = v;

            cN64.Enabled = v;
            cVC.Enabled = v;
            cPatch.Enabled = v;
            cSpoiler.Enabled = v;
            cHTMLLog.Enabled = v;
            cDrawHash.Enabled = v;

            bRandomise.Enabled = v;
            tSeed.Enabled = v;
            tSettings.Enabled = v;
            bLoadPatch.Enabled = v;
            bApplyPatch.Enabled = v;
        }

        private void mDPadConfig_Click(object sender, EventArgs e)
        {
            var items = DPadItem.All();
            var presets = DPadPreset.All();
            var config = _configuration.CosmeticSettings.AsmOptions.DPadConfig;

            DPadForm form = new DPadForm(presets, items, config);
            if (form.ShowDialog() == DialogResult.OK)
            {
                config.State = form.State;
                config.Pad = form.Selected;
                config.Display = form.Display;
            }
        }

        #endregion

        #region Settings

        public void InitializeSettings()
        {
            _configuration = new Configuration
            {
                OutputSettings = new OutputSettings(),
                GameplaySettings = new GameplaySettings
                {
                    ShortenCutsceneSettings = new ShortenCutsceneSettings(),
                },
                CosmeticSettings = new CosmeticSettings(),
            };

            tSeed.Text = Math.Abs(Environment.TickCount).ToString();

            tbUserLogic.Enabled = false;
            bLoadLogic.Enabled = false;
        }


        #endregion

        #region Randomization

        /// <summary>
        /// Try to perform randomization and make rom
        /// </summary>
        private void TryRandomize(BackgroundWorker worker, DoWorkEventArgs e)
        {
            var seed = Convert.ToInt32(tSeed.Text);
            var result = ConfigurationProcessor.Process(_configuration, seed, new BackgroundWorkerProgressReporter(worker));
            if (result != null)
            {
                MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _configuration.OutputSettings.InputPatchFilename = null;

            // let users know their old music files can't be used
            // maybe there's a better way to handle this...
            if (MusicConversionUtils.OLD_MUSIC_FILES.Any())
            {
                string old_music_message = "Unsupported music files were found during seed generation and could not be used. " +
                               "A log of the skipped files has been saved to the music folder.\n\n" +
                               "Log file: unsupported_music_files.txt";


                MessageBox.Show(old_music_message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            MessageBox.Show("Generation complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private bool CheckLogicFileExists()
        {
            if (_configuration.GameplaySettings.LogicMode == LogicMode.UserLogic && !File.Exists(_configuration.GameplaySettings.UserLogicFileName))
            {
                MessageBox.Show("User Logic not found or invalid, please load User Logic or change logic mode.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        #endregion

        private void BLoadPatch_Click(object sender, EventArgs e)
        {
            openPatch.ShowDialog();
            _configuration.OutputSettings.InputPatchFilename = openPatch.FileName;
            tPatch.Text = _configuration.OutputSettings.InputPatchFilename;
        }

        private void ttOutput_Changed(object sender, EventArgs e)
        {
            ToggleCheckBoxes();

            TogglePatchSettings(ttOutput.SelectedTab.TabIndex == 0);
        }


        private void TogglePatchSettings(bool v)
        {
            // Output Settings
            cPatch.Visible = v;
            cDrawHash.Visible = v;
            cSpoiler.Visible = v;
            cHTMLLog.Visible = v;

            // Tabs
            if (v)
            {
                if (!tSettings.TabPages.Contains(tabMain))
                {
                    tSettings.TabPages.Insert(0, tabMain);
                    tSettings.TabPages.Insert(1, tabItemPool);
                    tSettings.TabPages.Insert(2, tabGimmicks);
                    tSettings.TabPages.Insert(3, tabComfort);
                    tSettings.TabPages.Insert(4, tabShortenCutscenes);
                }
            }
            else
            {
                tSettings.TabPages.Remove(tabMain);
                tSettings.TabPages.Remove(tabItemPool);
                tSettings.TabPages.Remove(tabGimmicks);
                tSettings.TabPages.Remove(tabComfort);
                tSettings.TabPages.Remove(tabShortenCutscenes);
            }

            // Other..?
            cDummy.Enabled = v;

            _configuration.OutputSettings.InputPatchFilename = v ? null : string.Empty;
            tPatch.Text = v ? null : string.Empty;
        }

        private const string DEFAULT_SETTINGS_FILENAME = "settings";
        private void SaveSettings(string filename = null)
        {
            var path = Path.ChangeExtension(filename ?? Path.Combine(Values.MainDirectory, DEFAULT_SETTINGS_FILENAME), SETTINGS_EXTENSION);
            string logicFilePath = null;
            Configuration configurationToSave = _configuration;
            if (filename != null)
            {
                logicFilePath = _configuration.GameplaySettings.UserLogicFileName;
                _configuration.GameplaySettings.UserLogicFileName = null;
                if (_configuration.GameplaySettings.LogicMode == LogicMode.UserLogic && logicFilePath != null && File.Exists(logicFilePath))
                {
                    using (StreamReader Req = new StreamReader(File.OpenRead(logicFilePath)))
                    {
                        _configuration.GameplaySettings.Logic = Req.ReadToEnd();
                        var logicConfiguration = Configuration.FromJson(_configuration.GameplaySettings.Logic);
                        if (logicConfiguration.GameplaySettings != null)
                        {
                            _configuration.GameplaySettings.Logic = logicConfiguration.GameplaySettings.Logic;
                        }
                    }
                }
                configurationToSave = new Configuration
                {
                    GameplaySettings = _configuration.GameplaySettings,
                };
            }
            var fileInfo = new FileInfo(path);
            fileInfo.Directory.Create();
            File.WriteAllText(fileInfo.FullName, configurationToSave.ToString());
            if (logicFilePath != null)
            {
                _configuration.GameplaySettings.UserLogicFileName = logicFilePath;
                _configuration.GameplaySettings.Logic = null;
            }
        }
        
        private void LoadSettings(string filename = null)
        {
            var path = Path.ChangeExtension(filename ?? Path.Combine(Values.MainDirectory, DEFAULT_SETTINGS_FILENAME), SETTINGS_EXTENSION);
            if (File.Exists(path))
            {
                try
                {
                    Configuration newConfiguration;
                    using (StreamReader Req = new StreamReader(File.OpenRead(path)))
                    {
                        newConfiguration = Configuration.FromJson(Req.ReadToEnd());
                    }

                    if (newConfiguration.GameplaySettings.Logic != null)
                    {
                        newConfiguration.GameplaySettings.UserLogicFileName = path;
                        newConfiguration.GameplaySettings.Logic = null;
                    }
                    if (File.Exists(newConfiguration.GameplaySettings.UserLogicFileName))
                    {
                        tbUserLogic.Text = Path.GetFileNameWithoutExtension(newConfiguration.GameplaySettings.UserLogicFileName);
                    }
                    else
                    {
                        newConfiguration.GameplaySettings.UserLogicFileName = string.Empty;
                    }

                    if (filename != null)
                    {
                        _configuration.GameplaySettings = newConfiguration.GameplaySettings;
                    }
                    else
                    {
                        _configuration = newConfiguration;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error loading settings file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (_configuration.GameplaySettings.ItemCategoriesRandomized != null || _configuration.GameplaySettings.LocationCategoriesRandomized != null || _configuration.GameplaySettings.ClassicCategoriesRandomized != null)
            {
                var items = new List<Item>();
                if (_configuration.GameplaySettings.ItemCategoriesRandomized != null)
                {
                    items.AddRange(ItemUtils.ItemsByItemCategory().Where(kvp => _configuration.GameplaySettings.ItemCategoriesRandomized.Contains(kvp.Key)).SelectMany(kvp => kvp.Value));
                    _configuration.GameplaySettings.ItemCategoriesRandomized = null;
                }
                if (_configuration.GameplaySettings.LocationCategoriesRandomized != null)
                {
                    items.AddRange(ItemUtils.ItemsByLocationCategory().Where(kvp => _configuration.GameplaySettings.LocationCategoriesRandomized.Contains(kvp.Key)).SelectMany(kvp => kvp.Value));
                    _configuration.GameplaySettings.LocationCategoriesRandomized = null;
                }
                if (_configuration.GameplaySettings.ClassicCategoriesRandomized != null)
                {
                    items.AddRange(ItemUtils.ItemsByClassicCategory().Where(kvp => _configuration.GameplaySettings.ClassicCategoriesRandomized.Contains(kvp.Key)).SelectMany(kvp => kvp.Value));
                    _configuration.GameplaySettings.ClassicCategoriesRandomized = null;
                }
                _configuration.GameplaySettings.CustomItemList.Clear();
                foreach (var item in items)
                {
                    _configuration.GameplaySettings.CustomItemList.Add(item);
                }
                UpdateItemPoolCheckboxes();
            }
            else
            {
                _configuration.GameplaySettings.CustomItemList = ItemUtils.ConvertStringToItemList(ItemEditor.BaseItemList, _configuration.GameplaySettings.CustomItemListString)?.ToHashSet();
                UpdateItemPoolCheckboxes();
            }


            tStartingItemList.Text = _configuration.GameplaySettings.CustomStartingItemListString;

            tJunkLocationsList.Text = _configuration.GameplaySettings.CustomJunkLocationsString;

            HudConfig.Update(_configuration.CosmeticSettings.AsmOptions.HudColorsConfig.Colors);

            foreach (var form in Enum.GetValues<TransformationForm>())
            {
                if (!_configuration.CosmeticSettings.UseTunicColors.ContainsKey(form))
                {
                    _configuration.CosmeticSettings.UseTunicColors[form] = false;
                }
                if (!_configuration.CosmeticSettings.TunicColors.ContainsKey(form))
                {
                    // TODO unique default tunic colors
                    _configuration.CosmeticSettings.TunicColors[form] = Color.FromArgb(0x1E, 0x69, 0x1B);
                }
                if (!_configuration.CosmeticSettings.Instruments.ContainsKey(form))
                {
                    var def = form.DefaultInstrument();
                    if (def.HasValue)
                    {
                        _configuration.CosmeticSettings.Instruments[form] = def.Value;
                    }
                }
            }

            UpdateJunkLocationAmountLabel();
            UpdateCustomStartingItemAmountLabel();
            UpdateCheckboxes();
            ToggleCheckBoxes();
            tROMName.Text = _configuration.OutputSettings.InputROMFilename;
            tLuckRollPercentage.Value = _configuration.CosmeticSettings.MusicLuckRollChance;
            UpdateNumTricksEnabled();
            UpdateSettingLogicMarkers();
        }

        private void SaveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_configuration.GameplaySettings.LogicMode != LogicMode.UserLogic || (_configuration.GameplaySettings.LogicMode == LogicMode.UserLogic && CheckLogicFileExists()))
            {
                if (saveSettings.ShowDialog() == DialogResult.OK)
                {
                    SaveSettings(saveSettings.FileName);
                }
            }
        }

        private void LoadSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadSettings.Filter = "JSON Files|*.json";
            if (loadSettings.ShowDialog() == DialogResult.OK)
            {
                LoadSettings(loadSettings.FileName);
            }
        }

        private void btn_hud_Click(object sender, EventArgs e)
        {
            var config = _configuration.CosmeticSettings.AsmOptions.HudColorsConfig;
            if (HudConfig.ShowDialog(this, config) == DialogResult.Cancel)
            {
                var colors = HudConfig.ToColors();
                config.Colors = colors;
            }
        }

        private void cHUDHeartsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combobox = (ComboBox)sender;
            var selected = (ColorSelectionItem)combobox.SelectedItem;
            _configuration.CosmeticSettings.HeartsSelection = selected.Name;
        }

        private void cHUDMagicComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combobox = (ComboBox)sender;
            var selected = (ColorSelectionItem)combobox.SelectedItem;
            _configuration.CosmeticSettings.MagicSelection = selected.Name;
        }

        private void bToggleTricks_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new ToggleTricksForm(_configuration.GameplaySettings.LogicMode, _configuration.GameplaySettings.UserLogicFileName, _configuration.GameplaySettings.EnabledTricks);
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _configuration.GameplaySettings.EnabledTricks = dialog.Result;
                    UpdateNumTricksEnabled();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bItemPoolEdit_Click(object sender, EventArgs e)
        {
            if (ItemEditor.ShowDialog() == DialogResult.Cancel)
            {
                tItemPool.Text = ItemEditor.ItemListString;
            }
        }

        private void tItemPool_TextChanged(object sender, EventArgs e)
        {
            if (_itemPoolRecalculating)
            {
                return;
            }

            _configuration.GameplaySettings.CustomItemList = ItemUtils.ConvertStringToItemList(ItemEditor.BaseItemList, tItemPool.Text)?.ToHashSet();

            UpdateItemPoolCheckboxes();
        }

        private void cItemPoolAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            pLocationCategories.Visible = cItemPoolAdvanced.Checked;
            tableItemPool.Visible = cItemPoolAdvanced.Checked;
            pClassicItemPool.Visible = !cItemPoolAdvanced.Checked;
        }

        private void cCustomGossipWoth_CheckedChanged(object sender, EventArgs e)
        {
            if (cCustomGossipWoth.Checked)
            {
                nMaxGossipWotH.Controls[1].Text = nMaxGossipWotH.Value + "";
                nMaxGossipFoolish.Controls[1].Text = nMaxGossipFoolish.Value + "";
                nMaxGossipCT.Controls[1].Text = nMaxGossipCT.Value + "";
                _configuration.GameplaySettings.OverrideNumberOfRequiredGossipHints = (int)nMaxGossipWotH.Value;
                _configuration.GameplaySettings.OverrideNumberOfNonRequiredGossipHints = (int)nMaxGossipFoolish.Value;
                _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGossipHints = (int)nMaxGossipCT.Value;
            }
            else
            {
                _configuration.GameplaySettings.OverrideNumberOfRequiredGossipHints = null;
                _configuration.GameplaySettings.OverrideNumberOfNonRequiredGossipHints = null;
                _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGossipHints = null;
                nMaxGossipWotH.Controls[1].Text = string.Empty;
                nMaxGossipFoolish.Controls[1].Text = string.Empty;
                nMaxGossipCT.Controls[1].Text = string.Empty;
            }
            UpdateSingleSetting(null);
        }

        private void cCustomGaroWoth_CheckedChanged(object sender, EventArgs e)
        {
            if (cCustomGaroWoth.Checked)
            {
                nMaxGaroWotH.Controls[1].Text = nMaxGaroWotH.Value + "";
                nMaxGaroFoolish.Controls[1].Text = nMaxGaroFoolish.Value + "";
                nMaxGaroCT.Controls[1].Text = nMaxGaroCT.Value + "";
                _configuration.GameplaySettings.OverrideNumberOfRequiredGaroHints = (int)nMaxGaroWotH.Value;
                _configuration.GameplaySettings.OverrideNumberOfNonRequiredGaroHints = (int)nMaxGaroFoolish.Value;
                _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGaroHints = (int)nMaxGaroCT.Value;
            }
            else
            {
                _configuration.GameplaySettings.OverrideNumberOfRequiredGaroHints = null;
                _configuration.GameplaySettings.OverrideNumberOfNonRequiredGaroHints = null;
                _configuration.GameplaySettings.OverrideMaxNumberOfClockTownGaroHints = null;
                nMaxGaroWotH.Controls[1].Text = string.Empty;
                nMaxGaroFoolish.Controls[1].Text = string.Empty;
                nMaxGaroCT.Controls[1].Text = string.Empty;
            }
            UpdateSingleSetting(null);
        }

        private void bCustomizeHintPriorities_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new CustomizeHintPrioritiesForm(_configuration.GameplaySettings.OverrideHintPriorities, _configuration.GameplaySettings.OverrideImportanceIndicatorTiers, _configuration.GameplaySettings.OverrideHintItemCaps);
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _configuration.GameplaySettings.OverrideHintPriorities = dialog.Result;
                    _configuration.GameplaySettings.OverrideImportanceIndicatorTiers = dialog.ResultTiersIndicateImportance;
                    _configuration.GameplaySettings.OverrideHintItemCaps = dialog.ResultTiersCap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
