using MMRando.Forms;
using MMRando.Forms.Tooltips;
using MMRando.Models;
using MMRando.Utils;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MMRando
{
    public partial class MainForm : Form
    {
        private BindingSource settingsBindingSource;

        public Settings _settings { get; set; } = new Settings();

        public AboutForm About { get; private set; }
        public ManualForm Manual { get; private set; }
        public LogicEditorForm LogicEditor { get; private set; }
        public ItemEditForm ItemEditor { get; private set; }


        public static string AssemblyVersion
        {
            get
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;
                return $"Majora's Mask Randomizer v{v}";
            }
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeSettings();
            InitializeTooltips();

            ItemEditor = new ItemEditForm(_settings);
            LogicEditor = new LogicEditorForm();
            Manual = new ManualForm();
            About = new AboutForm();
            BindProperties();

            Text = AssemblyVersion;
        }

        private void InitializeTooltips()
        {
            // Output Settings
            TooltipBuilder.SetTooltip(cN64, "Output a randomized .z64 ROM that can be loaded into a N64 Emulator.");
            TooltipBuilder.SetTooltip(cVC, "Output a randomized .WAD file that can be loaded into a Wii Virtual Channel.");
            TooltipBuilder.SetTooltip(cSpoiler, "Output a spoiler log.\n\n The spoiler log contains a list over all items, and their shuffled locations.\n In addition, the spoiler log contains version information, seed and settings string used in the randomization.");
            TooltipBuilder.SetTooltip(cHTMLLog, "Output a html spoiler log (Requires spoiler log to be checked).\n\n Similar to the regular spoiler log, but readable in browsers. The locations/items are hidden by default, and hovering over them will make them visible.");
            TooltipBuilder.SetTooltip(cPatch, "Output a patch file that can be applied using the Patch settings tab to reproduce the same ROM.\nPatch file includes all settings except Tunic and Tatl color.");

            // Main Settings
            TooltipBuilder.SetTooltip(cMode, "Select mode of logic:\n - Casual: The randomization logic ensures that the game can be beaten casually.\n - Using glitches: The randomization logic allows for placement of items that are only obtainable using known glitches.\n - Vanilla Layout: All items are left vanilla.\n - User logic: Upload your own custom logic to be used in the randomization.\n - No logic: Completely random, no guarantee the game is beatable.");

            TooltipBuilder.SetTooltip(cUserItems, "Only randomize a custom list of items.\n\nThe item list can be edited from the menu: Customize -> Item List Editor. When checked, some settings will become disabled.");
            TooltipBuilder.SetTooltip(cMixSongs, "Enable songs being placed among items in the randomization pool.");
            TooltipBuilder.SetTooltip(cDChests, "Enable keys, boss keys, maps and compasses being placed in the randomization pool.");
            TooltipBuilder.SetTooltip(cShop, "Enable shop items being placed in the randomization pool.");
            TooltipBuilder.SetTooltip(cBottled, "Enable captured bottle contents being randomized.");
            TooltipBuilder.SetTooltip(cSoS, "Exclude song of soaring from being placed in the randomization pool.");
            TooltipBuilder.SetTooltip(cDEnt, "Enable randomization of dungeon entrances. \n\nStone Tower Temple is always vanilla, but Inverted Stone Tower Temple is randomized.");
            TooltipBuilder.SetTooltip(cAdditional, "Enable miscellaneous items being placed in the randomization pool.\n\nAmong the miscellaneous items are:\nFreestanding heartpieces, overworld chests, (hidden) grotto chests, Tingle's maps and bank heartpiece.");
            TooltipBuilder.SetTooltip(cEnemy, "Enable randomization of enemies. May cause softlocks in some circumstances, use at your own risk.");
            TooltipBuilder.SetTooltip(cMoonItems, "Enable moon items being placed in the randomization pool.\n\nIncludes the four Moon Trial Heart Pieces, Fierce Deity's Mask and the two Link Trial chests.");
            TooltipBuilder.SetTooltip(cFairyRewards, "Enable great fairy rewards being placed in the randomization pool.\n\nIncludes Magic Power, Great Spin Attack, Extended Magic Power, Double Defense, Great Fairy's Sword and Great Fairy's Mask.");
            TooltipBuilder.SetTooltip(cNutChest, "Enable randomization of the pre-clocktown deku nut chest. Not available when using Casual logic.");
            TooltipBuilder.SetTooltip(cCrazyStartingItems, "Enable randomization of starting Sword, Shield, and two Heart Containers.");

            // Gimmicks
            TooltipBuilder.SetTooltip(cDMult, "Select a damage mode, affecting how much damage Link takes:\n\n - Default: Link takes normal damage.\n - 2x: Link takes double damage.\n - 4x: Link takes quadruple damage.\n - 1-hit KO: Any damage kills Link.\n - Doom: Hardcore mode. Link's hearts are slowly being drained continuously.");
            TooltipBuilder.SetTooltip(cDType, "Select an effect to occur whenever Link is being damaged:\n\n - Default: Vanilla effects occur.\n - Fire: All damage burns Link.\n - Ice: All damage freezes Link.\n - Shock: All damage shocks link.\n - Knockdown: All damage knocks Link down.\n - Random: Any random effect of the above.");
            TooltipBuilder.SetTooltip(cGravity, "Select a movement modifier:\n\n - Default: No movement modifier.\n - High speed: Link moves at a much higher velocity.\n - Super low gravity: Link can jump very high.\n - Low gravity: Link can jump high.\n - High gravity: Link can barely jump.");
            TooltipBuilder.SetTooltip(cFloors, "Select a floortype for every floor ingame:\n\n - Default: Vanilla floortypes.\n - Sand: Link sinks slowly into every floor, affecting movement speed.\n - Ice: Every floor is slippery.\n - Snow: Similar to sand. \n - Random: Any random floortypes of the above.");
            TooltipBuilder.SetTooltip(cClockSpeed, "Modify the speed of time.");
            TooltipBuilder.SetTooltip(cHideClock, "Clock UI will be hidden.");
            TooltipBuilder.SetTooltip(cNoStartingItems, "You will not start with any randomized starting items.");

            // Comforts/cosmetics
            TooltipBuilder.SetTooltip(cCutsc, "Enable shortened cutscenes.\n\nCertain cutscenes are skipped or otherwise shortened.\nDISCLAIMER: This may cause crashing in certain emulators.");
            TooltipBuilder.SetTooltip(cQText, "Enable quick text. Dialogs are fast-forwarded to choices/end of dialog.");
            TooltipBuilder.SetTooltip(cBGM, "Randomize background music sequences that are played throughout the game.");
            TooltipBuilder.SetTooltip(cNoMusic, "Mute background music.");
            TooltipBuilder.SetTooltip(cFreeHints, "Enable reading gossip stone hints without requiring the Mask of Truth.");
            TooltipBuilder.SetTooltip(cClearHints, "Gossip stone hints will give clear item and location names.");
            TooltipBuilder.SetTooltip(cNoDowngrades, "Downgrading items will be prevented.");
            TooltipBuilder.SetTooltip(cShopAppearance, "Shops models and text will be updated to match the item they give.");
            TooltipBuilder.SetTooltip(cUpdateChests, "Chest appearance will be updated to match the item they contain.");
            TooltipBuilder.SetTooltip(cEponaSword, "Change Epona's B button behavior to prevent you from losing your sword if you don't have a bow.\nMay affect vanilla glitches that use Epona's B button.");
            TooltipBuilder.SetTooltip(bTunic, "Select the color of Link's Tunic.");
            TooltipBuilder.SetTooltip(cLink, "Select a character model to replace Link's default model.");
            TooltipBuilder.SetTooltip(cTatl, "Select a color scheme to replace Tatl's default color scheme.");
            TooltipBuilder.SetTooltip(cGossipHints, "Select a Gossip Stone hint style\n\n - Default: Vanilla Gossip Stone hints.\n - Random: Hints will contain locations of random items.\n - Relevant: Hints will contain locations of items loosely related to the vanilla hint or the area.\n - Competitive: Guaranteed hints about time-consuming checks, 3 hints about locations with logically-required items, 2 hints about locations with no logically-required items.");
        }

        #region Forms Code

        private void mmrMain_Load(object sender, EventArgs e)
        {
            // initialise some stuff
            InitializeBackgroundWorker();
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
            settingsBindingSource.ResumeBinding();
            pProgress.Value = 0;
            lStatus.Text = "Ready...";
            EnableAllControls(true);
            ToggleCheckBoxes();
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            RandomizedResult randomized;

            if (!_settings.ApplyPatch)
            {
                if (!TryRandomize(worker, out randomized))
                {
                    return;
                }
            }
            else
            {
                randomized = new RandomizedResult(_settings, null);
            }

            if (_settings.OutputSpoiler
                && _settings.LogicMode != LogicMode.Vanilla)
            {
                SpoilerUtils.CreateSpoilerLog(randomized, _settings);
            }


            if (_settings.ApplyPatch || _settings.OutputGame || _settings.OutputROMPatch)
            {
                try
                {
                    var _builder = new Builder(randomized);
                    _builder.MakeROM(_settings.InputROMFilename, _settings.OutputROMFilename, worker);
                }
                catch (Exception ex)
                {
                    string nl = Environment.NewLine;
                    MessageBox.Show($"Error building ROM: {ex.Message}{nl}{nl}Please contact the development team and provide them more information",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("Generation complete!",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void bTunic_Click(object sender, EventArgs e)
        {
            cTunic.ShowDialog();
            _settings.TunicColor = cTunic.Color;
        }

        private void bopen_Click(object sender, EventArgs e)
        {
            var result = openROM.ShowDialog();

            if (result == DialogResult.OK)
            {
                var validate = RomUtils.ValidateROM(openROM.FileName);

                if (validate == ValidateRomResult.InvalidFile)
                {
                    MessageBox.Show("Input file is not a clean Majora's Mask (U).",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _settings.InputROMFilename = openROM.FileName;
            }
        }
        private void BUserLogic_Click(object sender, EventArgs e)
        {
            if (openLogic.ShowDialog() == DialogResult.OK)
            {
                _settings.UserLogicFileName = openLogic.FileName;
                _settings.LogicMode = LogicMode.User;
            }
        }


        private void bRandomise_Click(object sender, EventArgs e)
        {
            _settings.OutputROMFilename = Path.Combine("Output", _settings.DefaultOutputROMFilename);
            _settings.ApplyPatch = false;

            GenerateOutput();
        }

        private void bApplyPatch_Click(object sender, EventArgs e)
        {
            var filename = Path.ChangeExtension(Path.GetFileName(_settings.InputPatchFilename), "z64");

            _settings.OutputROMFilename = Path.Combine("Output", filename);
            _settings.ApplyPatch = true;
            GenerateOutput();
        }

        private void GenerateOutput()
        {
            ValidateRomResult result = ValidateRomResult.NoFile;
            if (_settings.NeedInputROM)
            {
                result = RomUtils.ValidateROM(_settings.InputROMFilename);
                if (result == ValidateRomResult.NoFile)
                {
                    MessageBox.Show("Input ROM not found, cannot generate output.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (result == ValidateRomResult.InvalidFile)
                {
                    MessageBox.Show("Input file is not a clean Majora's Mask (U).",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            _settings.InputROMFormat = result;

            if (_settings.ApplyPatch && !File.Exists(_settings.InputPatchFilename))
            {
                MessageBox.Show("No patch file selected.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(_settings.ApplyPatch || _settings.OutputGame || _settings.OutputROMPatch || _settings.OutputSpoiler))
            {
                MessageBox.Show("No output selected.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EnableAllControls(false);
            settingsBindingSource.SuspendBinding();
            SaveSettings();
            bgWorker.RunWorkerAsync();
        }

        private void tSeed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cDummy.Select();
            }
        }
        private void cUserItems_CheckedChanged(object sender, EventArgs e)
        {

            cDChests.Visible = !cUserItems.Checked;

            cShop.Visible = !cUserItems.Checked;

            cBottled.Visible = !cUserItems.Checked;

            cSoS.Visible = !cUserItems.Checked;

            cAdditional.Visible = !cUserItems.Checked;

            cMoonItems.Visible = !cUserItems.Checked;

            cFairyRewards.Visible = !cUserItems.Checked;

            cNutChest.Visible = !cUserItems.Checked;

            cCrazyStartingItems.Visible = !cUserItems.Checked;

            bItemListEditor.Visible = cUserItems.Checked;
            tCustomItemList.Visible = cUserItems.Checked;
            lCustomItemAmount.Visible = cUserItems.Checked;
        }

        private void mExit_Click(object sender, EventArgs e)
        {
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

        private void bItemListEditor_Click(object sender, EventArgs e)
        {
            ItemEditor.ShowDialog();
            _settings.CustomItemList = ItemEditor.CustomItemList.ToList();
            _settings.CustomItemList.Sort();
            _settings.CustomItemListString = ItemEditForm.GetCustomItemListString(_settings.CustomItemList);
        }

        private void tCustomItemList_TextChanged(object sender, EventArgs e)
        {
            UpdateCustomItemAmountLabel();
        }

        private void UpdateCustomItemAmountLabel()
        {
            if (_settings.CustomItemList.Contains(-1))
            {
                lCustomItemAmount.Text = "Invalid custom item string";
            }
            else
            {
                lCustomItemAmount.Text = $"{_settings.CustomItemList.Count}/{ItemUtils.AllLocations().Count()} items randomized";
            }
        }



        /// <summary>
        /// Checks for settings that invalidate others, and disable the checkboxes for them.
        /// </summary>
        private void ToggleCheckBoxes()
        {
            var onMainTab = ttOutput.SelectedTab.TabIndex == 0;

            if (_settings.LogicMode == LogicMode.Vanilla)
            {
                cMixSongs.Enabled = false;
                cSoS.Enabled = false;
                cDChests.Enabled = false;
                cDEnt.Enabled = false;
                cBottled.Enabled = false;
                cShop.Enabled = false;
                cSpoiler.Enabled = false;
                cGossipHints.Enabled = false;
                cAdditional.Enabled = false;
                cUserItems.Enabled = false;
                cMoonItems.Enabled = false;
                cFairyRewards.Enabled = false;
                cNutChest.Enabled = false;
                cCrazyStartingItems.Enabled = false;
                cNoStartingItems.Enabled = false;
            }
            else
            {
                cMixSongs.Enabled = onMainTab;
                cDEnt.Enabled = onMainTab;
                cSpoiler.Enabled = onMainTab;
                cGossipHints.Enabled = onMainTab;
                cUserItems.Enabled = onMainTab;

                cSoS.Enabled = onMainTab;
                cDChests.Enabled = onMainTab;
                cBottled.Enabled = onMainTab;
                cShop.Enabled = onMainTab;
                cAdditional.Enabled = onMainTab;
                cMoonItems.Enabled = onMainTab;
                cFairyRewards.Enabled = onMainTab;
                cNutChest.Enabled = onMainTab && _settings.LogicMode != LogicMode.Casual;
                cCrazyStartingItems.Enabled = onMainTab;

                cNoStartingItems.Enabled = onMainTab && (_settings.AddOtherItems || _settings.UseCustomItemList);
                if (!cNoStartingItems.Enabled)
                {
                    cNoStartingItems.Checked = false;
                    _settings.NoStartingItems = false;
                }
            }

            cHTMLLog.Enabled = onMainTab;

            if (_settings.GossipHintStyle == GossipHintStyle.Default || _settings.LogicMode == LogicMode.Vanilla)
            {
                cClearHints.Enabled = false;
            }
            else
            {
                cClearHints.Enabled = onMainTab;
            }
        }

        private void EnableAllControls(bool v)
        {
            cAdditional.Enabled = v;
            cBGM.Enabled = v;
            cNoMusic.Enabled = v;
            cBottled.Enabled = v;
            cCutsc.Enabled = v;
            cDChests.Enabled = v;
            cDEnt.Enabled = v;
            cMode.Enabled = v;
            cDMult.Enabled = v;
            cDType.Enabled = v;
            cDummy.Enabled = v;
            cEnemy.Enabled = v;
            cFloors.Enabled = v;
            cClockSpeed.Enabled = v;
            cGossipHints.Enabled = v;
            cHideClock.Enabled = v;
            cGravity.Enabled = v;
            cLink.Enabled = v;
            cMixSongs.Enabled = v;
            cSoS.Enabled = v;
            cShop.Enabled = v;
            cUserItems.Enabled = v;
            cVC.Enabled = v;
            cQText.Enabled = v;
            cSpoiler.Enabled = v;
            cTatl.Enabled = v;
            cFreeHints.Enabled = v;
            cClearHints.Enabled = v;
            cNoDowngrades.Enabled = v;
            cShopAppearance.Enabled = v;
            cEponaSword.Enabled = v;
            cHTMLLog.Enabled = v;
            cN64.Enabled = v;
            cMoonItems.Enabled = v;
            cFairyRewards.Enabled = v;
            cNutChest.Enabled = v;
            cCrazyStartingItems.Enabled = v;
            cNoStartingItems.Enabled = v;
            cPatch.Enabled = v;
            bApplyPatch.Enabled = v;
            cUpdateChests.Enabled = v;

            bopen.Enabled = v;
            bRandomise.Enabled = v;
            bTunic.Enabled = v;

            tSeed.Enabled = v;
        }

        #endregion

        #region Settings

        private void InitializeSettings()
        {
            _settings = Settings.LoadFromFile("settings.json");
            _settings.CustomItemListString = ItemEditForm.GetCustomItemListString(_settings.CustomItemList);
        }

        private void SaveSettings()
        {
            var settings = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText("settings.json", settings);
        }

        private void BindProperties()
        {
            settingsBindingSource = new BindingSource { DataSource = typeof(Settings) };

            // Input Settings
            BindTextBoxToSetting(tROMName, nameof(_settings.InputROMFilename));
            BindTextBoxToSetting(tSeed, nameof(_settings.Seed));
            BindTextBoxToSetting(tUserLogic, nameof(_settings.UserLogicFileName));
            BindTextBoxToSetting(tPatch, nameof(_settings.InputPatchFilename));
            BindTextBoxToSetting(tCustomItemList, nameof(_settings.CustomItemListString));

            // Output Settings
            BindCheckboxToSetting(cN64, nameof(_settings.OutputN64ROM));
            BindCheckboxToSetting(cVC, nameof(_settings.OutputVC));
            BindCheckboxToSetting(cSpoiler, nameof(_settings.OutputTextSpoiler));
            BindCheckboxToSetting(cHTMLLog, nameof(_settings.OutputHTMLSpoiler));
            BindCheckboxToSetting(cPatch, nameof(_settings.OutputROMPatch));

            // Main generation settings
            BindCheckboxToSetting(cUserItems, nameof(_settings.UseCustomItemList));
            BindCheckboxToSetting(cMixSongs, nameof(_settings.AddSongs));
            BindCheckboxToSetting(cDChests, nameof(_settings.AddDungeonItems));
            BindCheckboxToSetting(cShop, nameof(_settings.AddShopItems));
            BindCheckboxToSetting(cBottled, nameof(_settings.RandomizeBottleCatchContents));
            BindCheckboxToSetting(cSoS, nameof(_settings.ExcludeSongOfSoaring));
            BindCheckboxToSetting(cDEnt, nameof(_settings.RandomizeDungeonEntrances));
            BindCheckboxToSetting(cAdditional, nameof(_settings.AddOtherItems));
            BindCheckboxToSetting(cEnemy, nameof(_settings.RandomizeEnemies));
            BindCheckboxToSetting(cMoonItems, nameof(_settings.AddMoonItems));
            BindCheckboxToSetting(cFairyRewards, nameof(_settings.AddFairyRewards));

            BindCheckboxToSetting(cShopAppearance, nameof(_settings.UpdateShopAppearance));
            BindCheckboxToSetting(cNutChest, nameof(_settings.AddNutChest));
            BindCheckboxToSetting(cCrazyStartingItems, nameof(_settings.CrazyStartingItems));
            BindCheckboxToSetting(cNoStartingItems, nameof(_settings.NoStartingItems));
            BindCheckboxToSetting(cEponaSword, nameof(_settings.FixEponaSword));
            BindCheckboxToSetting(cUpdateChests, nameof(_settings.UpdateChests));

            // Gimmicks
            BindCheckboxToSetting(cHideClock, nameof(_settings.HideClock));

            // Comforts/cosmetics
            BindCheckboxToSetting(cCutsc, nameof(_settings.ShortenCutscenes));
            BindCheckboxToSetting(cQText, nameof(_settings.QuickTextEnabled));
            BindCheckboxToSetting(cBGM, nameof(_settings.RandomizeBGM));
            BindCheckboxToSetting(cNoMusic, nameof(_settings.NoBGM));
            BindCheckboxToSetting(cFreeHints, nameof(_settings.FreeHints));
            BindCheckboxToSetting(cClearHints, nameof(_settings.ClearHints));
            BindCheckboxToSetting(cNoDowngrades, nameof(_settings.PreventDowngrades));

            BindComboBoxToEnumSetting(cDMult, nameof(_settings.DamageMode), DamageMode.Default);
            BindComboBoxToEnumSetting(cDType, nameof(_settings.DamageEffect), DamageEffect.Default);
            BindComboBoxToEnumSetting(cMode, nameof(_settings.LogicMode), LogicMode.Casual);
            BindComboBoxToEnumSetting(cLink, nameof(_settings.Character), Character.LinkMM);
            BindComboBoxToEnumSetting(cTatl, nameof(_settings.TatlColorSchema), TatlColorSchema.Default);
            BindComboBoxToEnumSetting(cGravity, nameof(_settings.MovementMode), MovementMode.Default);
            BindComboBoxToEnumSetting(cFloors, nameof(_settings.FloorType), FloorType.Default);
            BindComboBoxToEnumSetting(cClockSpeed, nameof(_settings.ClockSpeed), ClockSpeed.Default);
            BindComboBoxToEnumSetting(cGossipHints, nameof(_settings.GossipHintStyle), GossipHintStyle.Default);

            bTunic.DataBindings.Add("BackColor", settingsBindingSource, nameof(_settings.TunicColor), true, DataSourceUpdateMode.OnPropertyChanged);
            settingsBindingSource.DataSource = _settings;
        }

        private void BindCheckboxToSetting(CheckBox checkbox, string property)
        {
            checkbox.DataBindings.Add("Checked", settingsBindingSource, property, true, DataSourceUpdateMode.OnPropertyChanged);
        }
        
        private void BindTextBoxToSetting(TextBox textBox, string property)
        {
            textBox.DataBindings.Add("Text", settingsBindingSource, property);
        }

        private void BindComboBoxToEnumSetting<T>(ComboBox comboBox, string property, T enumDefault)
        {
            comboBox.BindEnumToCombobox(enumDefault);
            comboBox.DataBindings.Add("SelectedValue", settingsBindingSource, property, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion

        #region Randomization

        /// <summary>
        /// Try to create a RandomizedResult
        /// </summary>
        private bool TryRandomize(BackgroundWorker worker, out RandomizedResult randomized)
        {
            randomized = null;
            try
            {
                Randomizer randomizer = new Randomizer(_settings);
                randomized = randomizer.Randomize(worker);
                return true;
            }
            catch (InvalidDataException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
            catch (Exception ex)
            {
                string nl = Environment.NewLine;
                MessageBox.Show($"Error randomizing logic: {ex.Message}{nl}{nl}Please try a different seed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        private void BLoadPatch_Click(object sender, EventArgs e)
        {
            if (openPatch.ShowDialog() == DialogResult.OK)
            {
                _settings.InputPatchFilename = openPatch.FileName;
            }
        }

        private void ttOutput_Changed(object sender, EventArgs e)
        {
            ToggleCheckBoxes();

            TogglePatchSettings(ttOutput.SelectedTab.TabIndex == 0);
        }


        private void TogglePatchSettings(bool v)
        {
            // ROM Settings
            cPatch.Enabled = v;

            // Main Settings
            cMode.Enabled = v;
            cEnemy.Enabled = v;

            //Gimmicks
            cDMult.Enabled = v;
            cDType.Enabled = v;
            cGravity.Enabled = v;
            cFloors.Enabled = v;
            cClockSpeed.Enabled = v;
            cHideClock.Enabled = v;
            cNoStartingItems.Enabled = v;


            // Comfort/Cosmetics
            cCutsc.Enabled = v;
            cQText.Enabled = v;
            cBGM.Enabled = v;
            cFreeHints.Enabled = v;
            cNoDowngrades.Enabled = v;
            cShopAppearance.Enabled = v;
            cUpdateChests.Enabled = v;
            cEponaSword.Enabled = v;
            cClearHints.Enabled = _settings.LogicMode != LogicMode.Vanilla && _settings.GossipHintStyle != GossipHintStyle.Default && v;
            cGossipHints.Enabled = _settings.LogicMode != LogicMode.Vanilla && v;

            cLink.Enabled = v;

            // Other..?
            cDummy.Enabled = v;
        }
    }
}
