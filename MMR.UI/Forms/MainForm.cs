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

namespace MMR.UI.Forms
{
    using Randomizer = Randomizer.Randomizer;
    public partial class MainForm : Form
    {
        private bool _isUpdating = false;
        private int _seedOld = 0;
        public Configuration _configuration { get; set; }

        public AboutForm About { get; private set; }
        public ManualForm Manual { get; private set; }
        public LogicEditorForm LogicEditor { get; private set; }
        public CustomItemListEditForm ItemEditor { get; }
        public StartingItemEditForm StartingItemEditor { get; private set; }
        public JunkLocationEditForm JunkLocationEditor { get; private set; }
        public HudConfigForm HudConfig { get; private set; }


        public const string SETTINGS_EXTENSION = ".json";

        public MainForm()
        {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();
            InitializeHUDGroupBox();
            InitializeTransformationFormSettings();
            InitializeShortenCutsceneSettings();
            InitializeItemPoolSettings();
            InitializeDungeonModeSettings();
            InitalizeLowHealthSFXOptions();

            StartingItemEditor = new StartingItemEditForm();

            JunkLocationEditor = new JunkLocationEditForm();

            ItemEditor = new CustomItemListEditForm(ItemUtils.AllLocations(), item => $"{item.Location()} ({item.Name()})", "Invalid custom item string");

            LogicEditor = new LogicEditorForm();
            Manual = new ManualForm();
            About = new AboutForm();
            HudConfig = new HudConfigForm();


            Text = $"Majora's Mask Randomizer v{Randomizer.AssemblyVersion}";
        }

        private void InitializeTooltips()
        {
            // ROM Settings
            TooltipBuilder.SetTooltip(cN64, "Output a randomized .z64 ROM that can be loaded into a N64 Emulator.");
            TooltipBuilder.SetTooltip(cVC, "Output a randomized .WAD file that can be loaded into a Wii Virtual Channel.");
            TooltipBuilder.SetTooltip(cSpoiler, "Output a spoiler log.\n\n The spoiler log contains a list over all items, and their shuffled locations.\n In addition, the spoiler log contains version information, seed and settings string used in the randomization.");
            TooltipBuilder.SetTooltip(cHTMLLog, "Output a html spoiler log (Requires spoiler log to be checked).\n\n Similar to the regular spoiler log, but readable in browsers. The locations/items are hidden by default, and hovering over them will make them visible.");
            TooltipBuilder.SetTooltip(cPatch, "Output a patch file that can be applied using the Patch settings tab to reproduce the same ROM.\nPatch file includes all settings except Tunic and Tatl color.");

            // Main Settings
            TooltipBuilder.SetTooltip(cMode, "Select mode of logic:\n - Casual: The randomization logic ensures that the game can be beaten casually.\n - Using glitches: The randomization logic allows for placement of items that are only obtainable using known glitches.\n - Vanilla Layout: All items are left vanilla.\n - User logic: Upload your own custom logic to be used in the randomization.\n - No logic: Completely random, no guarantee the game is beatable.");

            TooltipBuilder.SetTooltip(cMixSongs, "Enable songs being placed among items in the randomization pool.");
            TooltipBuilder.SetTooltip(cProgressiveUpgrades, "Enable swords, wallets, magic, bomb bags and quivers to be found in the intended order.");
            TooltipBuilder.SetTooltip(cDEnt, "Enable randomization of dungeon entrances. \n\nStone Tower Temple is always vanilla, but Inverted Stone Tower Temple is randomized.");
            TooltipBuilder.SetTooltip(cEnemy, "Enable randomization of enemies. May cause softlocks in some circumstances, use at your own risk.");

            // Gimmicks
            TooltipBuilder.SetTooltip(cDMult, "Select a damage mode, affecting how much damage Link takes:\n\n - Default: Link takes normal damage.\n - 2x: Link takes double damage.\n - 4x: Link takes quadruple damage.\n - 1-hit KO: Any damage kills Link.\n - Doom: Hardcore mode. Link's hearts are slowly being drained continuously.");
            TooltipBuilder.SetTooltip(cDType, "Select an effect to occur whenever Link is being damaged:\n\n - Default: Vanilla effects occur.\n - Fire: All damage burns Link.\n - Ice: All damage freezes Link.\n - Shock: All damage shocks link.\n - Knockdown: All damage knocks Link down.\n - Random: Any random effect of the above.");
            TooltipBuilder.SetTooltip(cGravity, "Select a movement modifier:\n\n - Default: No movement modifier.\n - High speed: Link moves at a much higher velocity.\n - Super low gravity: Link can jump very high.\n - Low gravity: Link can jump high.\n - High gravity: Link can barely jump.");
            TooltipBuilder.SetTooltip(cLowHealthSFXComboBox, "Select a Low Health SFX setting:\n\n - Default: Vanilla sound.\n - Disabled: No sound will play.\n - Random: a random SFX will be chosen.\n - Specific SFX: a specific SFX will play as the low health sfx.");
            TooltipBuilder.SetTooltip(cNutAndStickDrops, "Adds Deku nuts and Deku sticks to drop tables in the field:\n\n - Default: No change, vanilla behavior.\n - Light: one stick and nut 1/16 chance termina bush.\n - Medium: More nuts, twice the chance\n - Extra: More sticks, more nuts, more drop locations.\n - Mayhem: You're crazy in the coconut!");
            TooltipBuilder.SetTooltip(cFloors, "Select a floortype for every floor ingame:\n\n - Default: Vanilla floortypes.\n - Sand: Link sinks slowly into every floor, affecting movement speed.\n - Ice: Every floor is slippery.\n - Snow: Similar to sand. \n - Random: Any random floortypes of the above.");
            TooltipBuilder.SetTooltip(cClockSpeed, "Modify the speed of time.");
            TooltipBuilder.SetTooltip(cAutoInvert, "Auto-invert time at the start of a cycle.");
            TooltipBuilder.SetTooltip(cHideClock, "Clock UI will be hidden.");
            TooltipBuilder.SetTooltip(cStartingItems, "Select a starting item mode:\n\nNone - You will not start with any randomized starting items.\nRandom - You will start with randomized starting items.\nAllow Temporary Items - You will start with randomized starting items including Keg, Magic Bean and Bottles with X.");
            TooltipBuilder.SetTooltip(cBlastCooldown, "Adjust the cooldown timer after using the Blast Mask.");
            TooltipBuilder.SetTooltip(cIceTraps, "Amount of ice traps to be added to pool by replacing junk items.");
            TooltipBuilder.SetTooltip(cIceTrapsAppearance, "Appearance of ice traps in pool for world models.");
            TooltipBuilder.SetTooltip(cSunsSong, "Enable using the Sun's Song, which speeds up time to 400 units per frame (normal time speed is 3 units per frame) until dawn or dusk or a loading zone.");
            TooltipBuilder.SetTooltip(cUnderwaterOcarina, "Enable using the ocarina underwater.");
            TooltipBuilder.SetTooltip(cTargettingStyle, "Default Z-Targeting style to Hold.");
            TooltipBuilder.SetTooltip(cFDAnywhere, "Allow the Fierce Deity's Mask to be used anywhere. Also addresses some softlocks caused by Fierce Deity.");
            TooltipBuilder.SetTooltip(cByoAmmo, "Arrows, Bombs, and Bombchu will not be provided for minigames. You must bring your own. Logic Modes other than No Logic will account for this.");
            TooltipBuilder.SetTooltip(cDeathMoonCrash, "Dying causes the moon to crash, with all that that implies.");
            TooltipBuilder.SetTooltip(cContinuousDekuHopping, "Press A while hopping across water to keep hopping.");
            TooltipBuilder.SetTooltip(cHookshotAnySurface, "Hookshot can hook to any surface.");
            TooltipBuilder.SetTooltip(cClimbMostSurfaces, "Link can climb most surfaces.");
            TooltipBuilder.SetTooltip(cIceTrapQuirks, "Ice traps will behave slightly differently from other items in certain situations.");

            // Comforts/cosmetics
            TooltipBuilder.SetTooltip(cQText, "Enable quick text. Dialogs are fast-forwarded to choices/end of dialog.");
            TooltipBuilder.SetTooltip(cSFX, "Randomize sound effects that are played throughout the game.");
            TooltipBuilder.SetTooltip(cMusic, "Select a music option\n\n - Default: Vanilla background music.\n - Random: Randomized background music.\n - None: No background music. Causes softlock on Frog Choir HP.");
            TooltipBuilder.SetTooltip(cFreeHints, "Enable reading gossip stone hints without requiring the Mask of Truth.");
            TooltipBuilder.SetTooltip(cClearHints, "Gossip stone hints will give clear item and location names.");
            TooltipBuilder.SetTooltip(cNoDowngrades, "Downgrading items will be prevented.");
            TooltipBuilder.SetTooltip(cShopAppearance, "Shops models and text will be updated to match the item they give.");
            TooltipBuilder.SetTooltip(cUpdateChests, "Chest appearance will be updated to match the item they contain.");
            TooltipBuilder.SetTooltip(cEponaSword, "Change Epona's B button behavior to prevent you from losing your sword if you don't have a bow.\nMay affect vanilla glitches that use Epona's B button.");
            TooltipBuilder.SetTooltip(cDrawHash, "Draw hash icons on the File Select screen.");
            TooltipBuilder.SetTooltip(cQuestItemStorage, "Enable Quest Item Storage, which allows for storing multiple quest items in their dedicated inventory slot. Quest items will also always be consumed when used.");
            TooltipBuilder.SetTooltip(cDisableCritWiggle, "Disable crit wiggle movement modification when 1 heart of health or less.");
            TooltipBuilder.SetTooltip(cLink, "Select a character model to replace Link's default model.");
            TooltipBuilder.SetTooltip(cTatl, "Select a color scheme to replace Tatl's default color scheme.");
            TooltipBuilder.SetTooltip(cGossipHints, "Select a Gossip Stone hint style\n\n - Default: Vanilla Gossip Stone hints.\n - Random: Hints will contain locations of random items.\n - Relevant: Hints will contain locations of items loosely related to the vanilla hint or the area.\n - Competitive: Guaranteed hints about time-consuming checks, 2 hints about locations with important items, 3 hints about locations with no important items.");
            TooltipBuilder.SetTooltip(cSkipBeaver, "Modify Beavers to not have to race the younger beaver.");
            TooltipBuilder.SetTooltip(cGoodDampeRNG, "Change Dampe ghost flames to always have two on the ground floor and one up the ladder.");
            TooltipBuilder.SetTooltip(cGoodDogRaceRNG, "Make Gold Dog always win if you have the Mask of Truth.");
            TooltipBuilder.SetTooltip(cFasterLabFish, "Change Lab Fish to only need to be fed one fish.");
            TooltipBuilder.SetTooltip(cFasterBank, "Change the Bank reward thresholds to 200/500/1000 instead of 200/1000/5000. Also reduces maximum bank capacity from 5000 to 1000.");
            TooltipBuilder.SetTooltip(cDoubleArcheryRewards, "Grant both archery rewards with a sufficient score.");
            TooltipBuilder.SetTooltip(cFastPush, "Increase the speed of pushing and pulling blocks and faucets.");
            TooltipBuilder.SetTooltip(cFreestanding, "Show world models as their actual item instead of the original item. This includes Pieces of Heart, Heart Containers, Skulltula Tokens, Stray Fairies, Moon's Tear and the Seahorse.");
            TooltipBuilder.SetTooltip(cEnableNightMusic, "Enables playing daytime Background music during nighttime in the field.\n(Clocktown night music can be weird)");
            TooltipBuilder.SetTooltip(cArrowCycling, "Cycle through arrow types when pressing R while an arrow is out when using the bow.");
            TooltipBuilder.SetTooltip(cCloseCows, "When playing Epona's Song for a group of cows, the closest cow will respond, instead of the default behavior.");
            TooltipBuilder.SetTooltip(cCombatMusicDisable, "Disables combat music around all regular (non boss or miniboss) enemies in the game.");
            TooltipBuilder.SetTooltip(cHueShiftMiscUI, "Shifts the color of miscellaneous UI elements.");
            TooltipBuilder.SetTooltip(cElegySpeedups, "Applies various Elegy of Emptiness speedups.");
            TooltipBuilder.SetTooltip(cInstantPictobox, "Remove anti-aliasing from the Pictobox pictures, which is what makes Pictobox on emulator so slow.");
            TooltipBuilder.SetTooltip(cImprovedPictobox, "Display extra text showing which type of picture was captured by the Pictobox.");
            TooltipBuilder.SetTooltip(cLenientGoronSpikes, "Goron spikes can charge midair and keep their charge. Minimum speed for goron spikes is removed.");
            TooltipBuilder.SetTooltip(cTargetHealth, "Targeting an enemy shows their health bar.");
            TooltipBuilder.SetTooltip(cFreeScarecrow, "Spawn scarecrow automatically when using ocarina if within range.");
            TooltipBuilder.SetTooltip(cFillWallet, "Fills wallet with max rupees upon finding a wallet upgrade.");
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

        private void InitializeDungeonModeSettings()
        {
            //var dungeonModeSettings = new List<Type> { typeof(SmallKeyMode), typeof(BossKeyMode), typeof(StrayFairyMode) };
            var properties = new List<PropertyInfo>();
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.SmallKeyMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.BossKeyMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.StrayFairyMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.BossRemainsMode)));
            properties.Add(typeof(GameplaySettings).GetProperty(nameof(GameplaySettings.PriceMode)));
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

        private class InvertIndeterminateCheckBox : CheckBox
        {
            protected override void OnClick(EventArgs e)
            {
                CheckState = CheckState switch
                {
                    CheckState.Checked => CheckState.Unchecked,
                    CheckState.Unchecked => CheckState.Checked,
                    CheckState.Indeterminate => CheckState.Checked,
                    _ => CheckState,
                };
            }
        }

        private class LocationCategoryLabel : Label
        {
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
            var itemsByItemCategory = ItemUtils.ItemsByItemCategory();

            var itemsByLocationCategory = ItemUtils.ItemsByLocationCategory();

            tableItemPool.RowCount = Enum.GetValues<ItemCategory>().Count() - 2;
            tableItemPool.ColumnCount = Enum.GetValues<LocationCategory>().Count() - 1;

            var locationCategoriesX = 141;

            var locationCategoryLabel = new LocationCategoryLabel();
            locationCategoryLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            locationCategoryLabel.Location = new Point(locationCategoriesX + 24, 30);
            locationCategoryLabel.Width = 610;
            locationCategoryLabel.Height = 105;
            locationCategoryLabel.Lines = Enum.GetValues<LocationCategory>().Where(c => c > 0).Select(c => $"{addSpacesRegex.Replace(c.ToString(), " $1")}: +{itemsByLocationCategory[c].Count}").ToList();

            tabItemPool.Controls.Add(locationCategoryLabel);

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
                checkbox.Location = new Point(locationCategoriesX, 140);
                checkbox.Width = 17;
                checkbox.Height = 17;
                checkbox.CheckStateChanged += cItemCategory2_CheckStateChanged;
                tableItemPool.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25));
                tabItemPool.Controls.Add(checkbox);

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
                checkboxes.AddRange(tabItemPool.Controls.OfType<CheckBox>().ToList());
                checkboxes.AddRange(tableItemPool.Controls.OfType<CheckBox>().ToList());

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

        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pProgress.Value = e.ProgressPercentage;
            var message = (string)e.UserState;
            lStatus.Text = message;
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

            cMixSongs.Checked = _configuration.GameplaySettings.AddSongs;
            cProgressiveUpgrades.Checked = _configuration.GameplaySettings.ProgressiveUpgrades;
            cDEnt.Checked = _configuration.GameplaySettings.RandomizeDungeonEntrances;
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
            cClearHints.Checked = _configuration.GameplaySettings.ClearHints;
            cHideClock.Checked = _configuration.GameplaySettings.HideClock;
            cSunsSong.Checked = _configuration.GameplaySettings.EnableSunsSong;
            cFDAnywhere.Checked = _configuration.GameplaySettings.AllowFierceDeityAnywhere;
            cByoAmmo.Checked = _configuration.GameplaySettings.ByoAmmo;
            cDeathMoonCrash.Checked = _configuration.GameplaySettings.DeathMoonCrash;
            cIceTrapQuirks.Checked = _configuration.GameplaySettings.IceTrapQuirks;
            cClockSpeed.SelectedIndex = (int)_configuration.GameplaySettings.ClockSpeed;
            cAutoInvert.SelectedIndex = (int)_configuration.GameplaySettings.AutoInvert;
            cNoDowngrades.Checked = _configuration.GameplaySettings.PreventDowngrades;
            cShopAppearance.Checked = _configuration.GameplaySettings.UpdateShopAppearance;
            cStartingItems.SelectedIndex = (int)_configuration.GameplaySettings.StartingItemMode;
            cEponaSword.Checked = _configuration.GameplaySettings.FixEponaSword;
            cUpdateChests.Checked = _configuration.GameplaySettings.UpdateChests;
            cSkipBeaver.Checked = _configuration.GameplaySettings.SpeedupBeavers;
            cGoodDampeRNG.Checked = _configuration.GameplaySettings.SpeedupDampe;
            cGoodDogRaceRNG.Checked = _configuration.GameplaySettings.SpeedupDogRace;
            cFasterLabFish.Checked = _configuration.GameplaySettings.SpeedupLabFish;
            cFasterBank.Checked = _configuration.GameplaySettings.SpeedupBank;
            cDoubleArcheryRewards.Checked = _configuration.GameplaySettings.DoubleArcheryRewards;

            cDMult.SelectedIndex = (int)_configuration.GameplaySettings.DamageMode;
            cDType.SelectedIndex = (int)_configuration.GameplaySettings.DamageEffect;
            cMode.SelectedIndex = (int)_configuration.GameplaySettings.LogicMode;
            cLink.SelectedIndex = (int)_configuration.GameplaySettings.Character;
            cTatl.SelectedIndex = (int)_configuration.CosmeticSettings.TatlColorSchema;
            cGravity.SelectedIndex = (int)_configuration.GameplaySettings.MovementMode;
            cLowHealthSFXComboBox.SelectedIndex = cLowHealthSFXComboBox.Items.IndexOf(_configuration.CosmeticSettings.LowHealthSFX.ToString());
            cNutAndStickDrops.SelectedIndex = (int)_configuration.GameplaySettings.NutandStickDrops;
            cFloors.SelectedIndex = (int)_configuration.GameplaySettings.FloorType;
            cGossipHints.SelectedIndex = (int)_configuration.GameplaySettings.GossipHintStyle;
            cBlastCooldown.SelectedIndex = (int)_configuration.GameplaySettings.BlastMaskCooldown;
            cIceTraps.SelectedIndex = (int)_configuration.GameplaySettings.IceTraps;
            cIceTrapsAppearance.SelectedIndex = (int)_configuration.GameplaySettings.IceTrapAppearance;
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

            // Misc config options
            cDisableCritWiggle.Checked = _configuration.GameplaySettings.CritWiggleDisable;
            _drawHashChecked = _configuration.GameplaySettings.DrawHash;
            cDrawHash.Checked = _configuration.OutputSettings.GeneratePatch || (_drawHashChecked && (_configuration.OutputSettings.GenerateROM || _configuration.OutputSettings.OutputVC));
            cFastPush.Checked = _configuration.GameplaySettings.FastPush;
            cQuestItemStorage.Checked = _configuration.GameplaySettings.QuestItemStorage;
            cContinuousDekuHopping.Checked = _configuration.GameplaySettings.ContinuousDekuHopping;
            cHookshotAnySurface.Checked = _configuration.GameplaySettings.HookshotAnySurface;
            cClimbMostSurfaces.Checked = _configuration.GameplaySettings.ClimbMostSurfaces;
            cUnderwaterOcarina.Checked = _configuration.GameplaySettings.OcarinaUnderwater;
            cFreestanding.Checked = _configuration.GameplaySettings.UpdateWorldModels;
            cArrowCycling.Checked = _configuration.GameplaySettings.ArrowCycling;
            cCloseCows.Checked = _configuration.GameplaySettings.CloseCows;
            cCombatMusicDisable.Checked = _configuration.CosmeticSettings.DisableCombatMusic != CombatMusic.Normal;
            cHueShiftMiscUI.Checked = _configuration.CosmeticSettings.ShiftHueMiscUI;
            cElegySpeedups.Checked = _configuration.GameplaySettings.ElegySpeedup;
            cImprovedPictobox.Checked = _configuration.GameplaySettings.EnablePictoboxSubject;
            cLenientGoronSpikes.Checked = _configuration.GameplaySettings.LenientGoronSpikes;
            cTargetHealth.Checked = _configuration.GameplaySettings.TargetHealthBar;
            cFreeScarecrow.Checked = _configuration.GameplaySettings.FreeScarecrow;
            cFillWallet.Checked = _configuration.GameplaySettings.FillWallet;

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

        private void cN64_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.OutputSettings.GenerateROM = cN64.Checked);
        }

        private void cSpoiler_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.OutputSettings.GenerateSpoilerLog = cSpoiler.Checked);
        }

        private bool _drawHashChecked;
        private void cPatch_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.OutputSettings.GeneratePatch = cPatch.Checked);

            cDrawHash.CheckedChanged -= cDrawHash_CheckedChanged;
            cDrawHash.Checked = cPatch.Checked ? true : _drawHashChecked && (_configuration.OutputSettings.GenerateROM || _configuration.OutputSettings.OutputVC);
            _configuration.GameplaySettings.DrawHash = cDrawHash.Checked;
            cDrawHash.CheckedChanged += cDrawHash_CheckedChanged;
        }

        private void cHTMLLog_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.OutputSettings.GenerateHTMLLog = cHTMLLog.Checked);
        }

        private void cSFX_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.RandomizeSounds = cSFX.Checked);
        }

        private void cEnableNightMusic_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.EnableNightBGM = cEnableNightMusic.Checked);
        }


        private void cMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.Music = (Music)cMusic.SelectedIndex);
        }

        private void cDEnt_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.RandomizeDungeonEntrances = cDEnt.Checked);
        }

        private void cDMult_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.DamageMode = (DamageMode)cDMult.SelectedIndex);
        }

        private void cDType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.DamageEffect = (DamageEffect)cDType.SelectedIndex);
        }

        private void cEnemy_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.RandomizeEnemies = cEnemy.Checked);
        }

        private void cFloors_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.FloorType = (FloorType)cFloors.SelectedIndex);
        }

        private void cGravity_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.MovementMode = (MovementMode)cGravity.SelectedIndex);
        }

        private void cNutAndStickDrops_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.NutandStickDrops = (NutAndStickDrops)cNutAndStickDrops.SelectedIndex);
        }

        private void cLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.Character = (Character)cLink.SelectedIndex);
        }

        private void cMixSongs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.AddSongs = cMixSongs.Checked);
        }

        private void cProgressiveUpgrades_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ProgressiveUpgrades = cProgressiveUpgrades.Checked);
        }

        private void cFreeHints_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.FreeHints = cFreeHints.Checked);
        }

        private void cClearHints_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ClearHints = cClearHints.Checked);
        }

        private void cNoDowngrades_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.PreventDowngrades = cNoDowngrades.Checked);
        }

        private void cShopAppearance_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.UpdateShopAppearance = cShopAppearance.Checked);
        }

        private void cUpdateChests_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.UpdateChests = cUpdateChests.Checked);
        }

        private void cEponaSword_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.FixEponaSword = cEponaSword.Checked);
        }

        private void cHideClock_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.HideClock = cHideClock.Checked);
        }

        private void cSunsSong_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.EnableSunsSong = cSunsSong.Checked);
        }

        private void cFDAnywhere_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.AllowFierceDeityAnywhere = cFDAnywhere.Checked);
        }

        private void cByoAmmo_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ByoAmmo = cByoAmmo.Checked);
        }

        private void cDeathMoonCrash_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.DeathMoonCrash = cDeathMoonCrash.Checked);
        }

        private void cIceTrapQuirks_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.IceTrapQuirks = cIceTrapQuirks.Checked);
        }

        private void cStartingItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.StartingItemMode = (StartingItemMode)cStartingItems.SelectedIndex);
        }

        private void cQText_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.QuickTextEnabled = cQText.Checked);
        }

        private void cTatl_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.TatlColorSchema = (TatlColorSchema)cTatl.SelectedIndex);
        }

        private void cSkipBeaver_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.SpeedupBeavers = cSkipBeaver.Checked);
        }

        private void cGoodDampeRNG_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.SpeedupDampe = cGoodDampeRNG.Checked);
        }

        private void cGoodDogRaceRNG_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.SpeedupDogRace = cGoodDogRaceRNG.Checked);
        }

        private void cFasterLabFish_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.SpeedupLabFish = cFasterLabFish.Checked);
        }

        private void cFasterBank_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.SpeedupBank = cFasterBank.Checked);
        }

        private void cDrawHash_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.DrawHash = cDrawHash.Checked);
            _drawHashChecked = cDrawHash.Checked;
        }

        private void cQuestItemStorage_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.QuestItemStorage = cQuestItemStorage.Checked);
        }

        private void cContinuousDekuHopping_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ContinuousDekuHopping = cContinuousDekuHopping.Checked);
        }

        private void cHookshotAnySurface_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.HookshotAnySurface = cHookshotAnySurface.Checked);
        }

        private void cClimbMostSurfaces_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ClimbMostSurfaces = cClimbMostSurfaces.Checked);
        }

        private void cDisableCritWiggle_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.CritWiggleDisable = cDisableCritWiggle.Checked);
        }

        private void cFastPush_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.FastPush = cFastPush.Checked);
        }

        private void cUnderwaterOcarina_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.OcarinaUnderwater = cUnderwaterOcarina.Checked);
        }

        private void cFreestanding_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.UpdateWorldModels = cFreestanding.Checked);
        }

        private void cTargettingStyle_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.EnableHoldZTargeting = cTargettingStyle.Checked);
        }

        private void cArrowCycling_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ArrowCycling = cArrowCycling.Checked);
        }

        private void cCloseCows_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.CloseCows = cCloseCows.Checked);
        }

        private void cCombatMusicDisable_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.DisableCombatMusic = cCombatMusicDisable.Checked ? CombatMusic.All : CombatMusic.Normal);
        }

        private void cHueShiftMiscUI_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.ShiftHueMiscUI = cHueShiftMiscUI.Checked);
        }

        private void cElegySpeedups_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ElegySpeedup = cElegySpeedups.Checked);
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
                }
                _configuration.GameplaySettings.LogicMode = logicMode;
            });
        }

        private void cClockSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.ClockSpeed = (ClockSpeed)cClockSpeed.SelectedIndex);
        }

        private void cGossipHints_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.GossipHintStyle = (GossipHintStyle)cGossipHints.SelectedIndex);
        }


        private void cBlastCooldown_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.BlastMaskCooldown = (BlastMaskCooldown)cBlastCooldown.SelectedIndex);
        }

        private void cIceTraps_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.IceTraps = (IceTraps)cIceTraps.SelectedIndex);
        }

        private void cIceTrapsAppearance_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.IceTrapAppearance = (IceTrapAppearance)cIceTrapsAppearance.SelectedIndex);
        }

        private void cVC_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.OutputSettings.OutputVC = cVC.Checked);
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
            cSpoiler.Enabled = !vanillaMode;
            cHTMLLog.Enabled = !vanillaMode;
            cGossipHints.Enabled = !vanillaMode;
            cStartingItems.Enabled = !vanillaMode;
            tJunkLocationsList.Enabled = !vanillaMode && _configuration.GameplaySettings.LogicMode != LogicMode.NoLogic;
            bJunkLocationsEditor.Enabled = !vanillaMode && _configuration.GameplaySettings.LogicMode != LogicMode.NoLogic;
            bToggleTricks.Enabled = !vanillaMode && _configuration.GameplaySettings.LogicMode != LogicMode.NoLogic;
            cIceTraps.Enabled = !vanillaMode;
            cIceTrapsAppearance.Enabled = !vanillaMode;
            cIceTrapQuirks.Enabled = !vanillaMode;

            bLoadLogic.Enabled = _configuration.GameplaySettings.LogicMode == LogicMode.UserLogic;

            var oldEnabled = cDrawHash.Enabled;
            cDrawHash.Enabled = !_configuration.OutputSettings.GeneratePatch && (_configuration.OutputSettings.GenerateROM || _configuration.OutputSettings.OutputVC);
            if (cDrawHash.Enabled != oldEnabled)
            {
                cDrawHash.CheckedChanged -= cDrawHash_CheckedChanged;
                cDrawHash.Checked = cDrawHash.Enabled ? _drawHashChecked : false;
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
            cStartingItems.Enabled = v;
            cMixSongs.Enabled = v;
            cProgressiveUpgrades.Enabled = v;
            cEnemy.Enabled = v;

            //bHumanTunic.Enabled = v;
            tFormCosmetics.Enabled = v;
            cTatl.Enabled = v;
            cMusic.Enabled = v;
            cEnableNightMusic.Enabled = v;
            cLink.Enabled = v;

            cHUDHeartsComboBox.Enabled = v;
            cHUDMagicComboBox.Enabled = v;
            btn_hud.Enabled = v;

            cGossipHints.Enabled = v;
            cFreeHints.Enabled = v;
            cClearHints.Enabled = v;

            cTargettingStyle.Enabled = v;
            cInstantPictobox.Enabled = v;
            cSFX.Enabled = v;
            cDisableCritWiggle.Enabled = v;
            cQText.Enabled = v;
            cFastPush.Enabled = v;
            cShopAppearance.Enabled = v;
            cUpdateChests.Enabled = v;
            cNoDowngrades.Enabled = v;
            cEponaSword.Enabled = v;
            cQuestItemStorage.Enabled = v;
            cFreestanding.Enabled = v;
            cArrowCycling.Enabled = v;
            cCloseCows.Enabled = v;
            cElegySpeedups.Enabled = v;
            cImprovedPictobox.Enabled = v;
            cLenientGoronSpikes.Enabled = v;
            cTargetHealth.Enabled = v;
            cFreeScarecrow.Enabled = v;
            cFillWallet.Enabled = v;

            cSkipBeaver.Enabled = v;
            cGoodDampeRNG.Enabled = v;
            cFasterLabFish.Enabled = v;
            cGoodDogRaceRNG.Enabled = v;
            cFasterBank.Enabled = v;
            cDoubleArcheryRewards.Enabled = v;

            cDMult.Enabled = v;
            cDType.Enabled = v;
            cGravity.Enabled = v;
            cFloors.Enabled = v;
            cClockSpeed.Enabled = v;
            cAutoInvert.Enabled = v;
            cBlastCooldown.Enabled = v;
            cIceTraps.Enabled = v;
            cIceTrapsAppearance.Enabled = v;
            cHideClock.Enabled = v;
            cUnderwaterOcarina.Enabled = v;
            cSunsSong.Enabled = v;
            cFDAnywhere.Enabled = v;
            cByoAmmo.Enabled = v;
            cDeathMoonCrash.Enabled = v;
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
                        if (_configuration.GameplaySettings.Logic.StartsWith("{"))
                        {
                            var logicConfiguration = Configuration.FromJson(_configuration.GameplaySettings.Logic);
                            _configuration.GameplaySettings.Logic = logicConfiguration.GameplaySettings.Logic;
                        }
                    }
                }
                configurationToSave = new Configuration
                {
                    GameplaySettings = _configuration.GameplaySettings,
                };
            }
            using (var settingsFile = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                settingsFile.Write(configurationToSave.ToString());
            }
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

            if (_configuration.GameplaySettings.ItemCategoriesRandomized != null || _configuration.GameplaySettings.LocationCategoriesRandomized != null)
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

        private void cLowHealthSFXComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // should probably make the object[] obj support both string and index to avoid this search, but it's low use
            var comboboxArrayObj = cLowHealthSFXComboBox.Items[ cLowHealthSFXComboBox.SelectedIndex ];
            var SFXOptionList = Enum.GetValues(typeof(LowHealthSFX)).Cast<LowHealthSFX>().ToList();
            var SFXOption = SFXOptionList.Find(u => u.ToString() == comboboxArrayObj.ToString());
            UpdateSingleSetting(() => _configuration.CosmeticSettings.LowHealthSFX = SFXOption);
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

        private void cInstantPictobox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.CosmeticSettings.KeepPictoboxAntialiasing = !cInstantPictobox.Checked);
        }

        private void cImprovedPictobox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.EnablePictoboxSubject = cImprovedPictobox.Checked);
        }

        private void cLenientGoronSpikes_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.LenientGoronSpikes = cLenientGoronSpikes.Checked);
        }

        private void cTargetHealth_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.TargetHealthBar = cTargetHealth.Checked);
        }

        private void cFreeScarecrow_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.FreeScarecrow = cFreeScarecrow.Checked);
        }

        private void cDoubleArcheryRewards_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.DoubleArcheryRewards = cDoubleArcheryRewards.Checked);
        }

        private void cFillWallet_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.FillWallet = cFillWallet.Checked);
        }

        private void cAutoInvert_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSingleSetting(() => _configuration.GameplaySettings.AutoInvert = (AutoInvertState)cAutoInvert.SelectedIndex);
        }
    }
}
