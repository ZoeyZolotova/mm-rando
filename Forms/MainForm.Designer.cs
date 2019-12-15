using System;
using System.Drawing;

namespace MMRando
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bopen = new System.Windows.Forms.Button();
            this.openROM = new System.Windows.Forms.OpenFileDialog();
            this.openLogic = new System.Windows.Forms.OpenFileDialog();
            this.tROMName = new System.Windows.Forms.TextBox();
            this.cUserItems = new System.Windows.Forms.CheckBox();
            this.tSettings = new System.Windows.Forms.TabControl();
            this.tabROMSettings = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cHTMLLog = new System.Windows.Forms.CheckBox();
            this.cSpoiler = new System.Windows.Forms.CheckBox();
            this.gGameOutput = new System.Windows.Forms.GroupBox();
            this.cPatch = new System.Windows.Forms.CheckBox();
            this.cN64 = new System.Windows.Forms.CheckBox();
            this.cVC = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lJunkLocationsAmount = new System.Windows.Forms.Label();
            this.bJunkLocationsEditor = new System.Windows.Forms.Button();
            this.tJunkLocationsList = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lCustomStartingItemAmount = new System.Windows.Forms.Label();
            this.bStartingItemEditor = new System.Windows.Forms.Button();
            this.tStartingItemList = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cNoStartingItems = new System.Windows.Forms.CheckBox();
            this.cDEnt = new System.Windows.Forms.CheckBox();
            this.cEnemy = new System.Windows.Forms.CheckBox();
            this.cMixSongs = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cMundaneRewards = new System.Windows.Forms.CheckBox();
            this.cStrayFairies = new System.Windows.Forms.CheckBox();
            this.cSpiders = new System.Windows.Forms.CheckBox();
            this.cCowMilk = new System.Windows.Forms.CheckBox();
            this.cFairyRewards = new System.Windows.Forms.CheckBox();
            this.lCustomItemAmount = new System.Windows.Forms.Label();
            this.tCustomItemList = new System.Windows.Forms.TextBox();
            this.bItemListEditor = new System.Windows.Forms.Button();
            this.cSoS = new System.Windows.Forms.CheckBox();
            this.cCrazyStartingItems = new System.Windows.Forms.CheckBox();
            this.cDChests = new System.Windows.Forms.CheckBox();
            this.cBottled = new System.Windows.Forms.CheckBox();
            this.cNutChest = new System.Windows.Forms.CheckBox();
            this.cShop = new System.Windows.Forms.CheckBox();
            this.cAdditional = new System.Windows.Forms.CheckBox();
            this.cMoonItems = new System.Windows.Forms.CheckBox();
            this.lMode = new System.Windows.Forms.Label();
            this.cMode = new System.Windows.Forms.ComboBox();
            this.tabGimmick = new System.Windows.Forms.TabPage();
            this.cBlastCooldown = new System.Windows.Forms.ComboBox();
            this.lBlastMask = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cHideClock = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cClockSpeed = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lFloors = new System.Windows.Forms.Label();
            this.lGravity = new System.Windows.Forms.Label();
            this.lDType = new System.Windows.Forms.Label();
            this.lDMult = new System.Windows.Forms.Label();
            this.cFloors = new System.Windows.Forms.ComboBox();
            this.cDType = new System.Windows.Forms.ComboBox();
            this.cDMult = new System.Windows.Forms.ComboBox();
            this.cGravity = new System.Windows.Forms.ComboBox();
            this.tabComfort = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cGoodDogRaceRNG = new System.Windows.Forms.CheckBox();
            this.cFasterLabFish = new System.Windows.Forms.CheckBox();
            this.cGoodDampeRNG = new System.Windows.Forms.CheckBox();
            this.cSkipBeaver = new System.Windows.Forms.CheckBox();
            this.lMusic = new System.Windows.Forms.Label();
            this.cMusic = new System.Windows.Forms.ComboBox();
            this.cSFX = new System.Windows.Forms.CheckBox();
            this.cUpdateChests = new System.Windows.Forms.CheckBox();
            this.cEponaSword = new System.Windows.Forms.CheckBox();
            this.cShopAppearance = new System.Windows.Forms.CheckBox();
            this.lGossip = new System.Windows.Forms.Label();
            this.cNoDowngrades = new System.Windows.Forms.CheckBox();
            this.cGossipHints = new System.Windows.Forms.ComboBox();
            this.cClearHints = new System.Windows.Forms.CheckBox();
            this.cFreeHints = new System.Windows.Forms.CheckBox();
            this.cCutsc = new System.Windows.Forms.CheckBox();
            this.cQText = new System.Windows.Forms.CheckBox();
            this.lTunic = new System.Windows.Forms.Label();
            this.cTatl = new System.Windows.Forms.ComboBox();
            this.cLink = new System.Windows.Forms.ComboBox();
            this.bTunic = new System.Windows.Forms.Button();
            this.lTatl = new System.Windows.Forms.Label();
            this.lLink = new System.Windows.Forms.Label();
            this.bApplyPatch = new System.Windows.Forms.Button();
            this.saveROM = new System.Windows.Forms.SaveFileDialog();
            this.cTunic = new System.Windows.Forms.ColorDialog();
            this.bRandomise = new System.Windows.Forms.Button();
            this.saveWad = new System.Windows.Forms.SaveFileDialog();
            this.mMenu = new System.Windows.Forms.MenuStrip();
            this.mFile = new System.Windows.Forms.ToolStripMenuItem();
            this.importCustomLogicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mCustomise = new System.Windows.Forms.ToolStripMenuItem();
            this.mDPadConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mLogicEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mManual = new System.Windows.Forms.ToolStripMenuItem();
            this.mSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.openBROM = new System.Windows.Forms.OpenFileDialog();
            this.pProgress = new System.Windows.Forms.ProgressBar();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.lStatus = new System.Windows.Forms.Label();
            this.tSeed = new System.Windows.Forms.TextBox();
            this.lSeed = new System.Windows.Forms.Label();
            this.tSString = new System.Windows.Forms.TextBox();
            this.lSString = new System.Windows.Forms.Label();
            this.cDummy = new System.Windows.Forms.CheckBox();
            this.lHrule = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.openPatch = new System.Windows.Forms.OpenFileDialog();
            this.ttOutput = new System.Windows.Forms.TabControl();
            this.tpOutputSettings = new System.Windows.Forms.TabPage();
            this.tpPatchSettings = new System.Windows.Forms.TabPage();
            this.tPatch = new System.Windows.Forms.TextBox();
            this.bLoadPatch = new System.Windows.Forms.Button();
            this.tSettings.SuspendLayout();
            this.tabROMSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gGameOutput.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabGimmick.SuspendLayout();
            this.tabComfort.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.mMenu.SuspendLayout();
            this.ttOutput.SuspendLayout();
            this.tpOutputSettings.SuspendLayout();
            this.tpPatchSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // bopen
            // 
            this.bopen.Location = new System.Drawing.Point(3, 37);
            this.bopen.Name = "bopen";
            this.bopen.Size = new System.Drawing.Size(103, 27);
            this.bopen.TabIndex = 0;
            this.bopen.Text = "Open ROM...";
            this.bopen.UseVisualStyleBackColor = true;
            this.bopen.Click += new System.EventHandler(this.bopen_Click);
            // 
            // openROM
            // 
            this.openROM.Filter = "ROM files|*.z64";
            // 
            // openLogic
            // 
            this.openLogic.Filter = "Logic File|*.txt";
            // 
            // tROMName
            // 
            this.tROMName.Location = new System.Drawing.Point(109, 41);
            this.tROMName.Name = "tROMName";
            this.tROMName.ReadOnly = true;
            this.tROMName.Size = new System.Drawing.Size(230, 20);
            this.tROMName.TabIndex = 1;
            // 
            // cUserItems
            // 
            this.cUserItems.AutoSize = true;
            this.cUserItems.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cUserItems.Location = new System.Drawing.Point(191, -1);
            this.cUserItems.Name = "cUserItems";
            this.cUserItems.Size = new System.Drawing.Size(119, 17);
            this.cUserItems.TabIndex = 11;
            this.cUserItems.Text = "Use custom item list";
            this.cUserItems.UseVisualStyleBackColor = true;
            this.cUserItems.CheckedChanged += new System.EventHandler(this.cUserItems_CheckedChanged);
            // 
            // tSettings
            // 
            this.tSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tSettings.Controls.Add(this.tabROMSettings);
            this.tSettings.Controls.Add(this.tabMain);
            this.tSettings.Controls.Add(this.tabGimmick);
            this.tSettings.Controls.Add(this.tabComfort);
            this.tSettings.Location = new System.Drawing.Point(0, 23);
            this.tSettings.Name = "tSettings";
            this.tSettings.SelectedIndex = 0;
            this.tSettings.Size = new System.Drawing.Size(355, 506);
            this.tSettings.TabIndex = 10;
            // 
            // tabROMSettings
            // 
            this.tabROMSettings.Controls.Add(this.groupBox1);
            this.tabROMSettings.Controls.Add(this.gGameOutput);
            this.tabROMSettings.Controls.Add(this.label1);
            this.tabROMSettings.Controls.Add(this.tROMName);
            this.tabROMSettings.Controls.Add(this.bopen);
            this.tabROMSettings.Location = new System.Drawing.Point(4, 25);
            this.tabROMSettings.Name = "tabROMSettings";
            this.tabROMSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabROMSettings.Size = new System.Drawing.Size(347, 509);
            this.tabROMSettings.TabIndex = 3;
            this.tabROMSettings.Text = "ROM Settings";
            this.tabROMSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cHTMLLog);
            this.groupBox1.Controls.Add(this.cSpoiler);
            this.groupBox1.Location = new System.Drawing.Point(177, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(141, 107);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spoiler Log Output";
            // 
            // cHTMLLog
            // 
            this.cHTMLLog.AutoSize = true;
            this.cHTMLLog.BackColor = System.Drawing.Color.Transparent;
            this.cHTMLLog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cHTMLLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cHTMLLog.ForeColor = System.Drawing.Color.Black;
            this.cHTMLLog.Location = new System.Drawing.Point(20, 51);
            this.cHTMLLog.Name = "cHTMLLog";
            this.cHTMLLog.Size = new System.Drawing.Size(98, 17);
            this.cHTMLLog.TabIndex = 14;
            this.cHTMLLog.Text = "Webpage .html";
            this.cHTMLLog.UseVisualStyleBackColor = false;
            this.cHTMLLog.CheckedChanged += new System.EventHandler(this.cHTMLLog_CheckedChanged);
            // 
            // cSpoiler
            // 
            this.cSpoiler.AutoSize = true;
            this.cSpoiler.BackColor = System.Drawing.Color.Transparent;
            this.cSpoiler.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cSpoiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSpoiler.ForeColor = System.Drawing.Color.Black;
            this.cSpoiler.Location = new System.Drawing.Point(54, 28);
            this.cSpoiler.Name = "cSpoiler";
            this.cSpoiler.Size = new System.Drawing.Size(64, 17);
            this.cSpoiler.TabIndex = 8;
            this.cSpoiler.Text = "Text .txt";
            this.cSpoiler.UseVisualStyleBackColor = false;
            this.cSpoiler.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // gGameOutput
            // 
            this.gGameOutput.Controls.Add(this.cPatch);
            this.gGameOutput.Controls.Add(this.cN64);
            this.gGameOutput.Controls.Add(this.cVC);
            this.gGameOutput.Location = new System.Drawing.Point(30, 75);
            this.gGameOutput.Name = "gGameOutput";
            this.gGameOutput.Size = new System.Drawing.Size(141, 107);
            this.gGameOutput.TabIndex = 16;
            this.gGameOutput.TabStop = false;
            this.gGameOutput.Text = "Game Output";
            // 
            // cPatch
            // 
            this.cPatch.AutoSize = true;
            this.cPatch.BackColor = System.Drawing.Color.Transparent;
            this.cPatch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cPatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cPatch.ForeColor = System.Drawing.Color.Black;
            this.cPatch.Location = new System.Drawing.Point(39, 68);
            this.cPatch.Name = "cPatch";
            this.cPatch.Size = new System.Drawing.Size(79, 17);
            this.cPatch.TabIndex = 15;
            this.cPatch.Text = "Patch .mmr";
            this.cPatch.UseVisualStyleBackColor = false;
            this.cPatch.CheckedChanged += new System.EventHandler(this.cPatch_CheckedChanged);
            // 
            // cN64
            // 
            this.cN64.AutoSize = true;
            this.cN64.BackColor = System.Drawing.Color.Transparent;
            this.cN64.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cN64.Checked = true;
            this.cN64.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cN64.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cN64.ForeColor = System.Drawing.Color.Black;
            this.cN64.Location = new System.Drawing.Point(21, 22);
            this.cN64.Name = "cN64";
            this.cN64.Size = new System.Drawing.Size(97, 17);
            this.cN64.TabIndex = 10;
            this.cN64.Text = "N64 ROM .z64";
            this.cN64.UseVisualStyleBackColor = false;
            this.cN64.CheckedChanged += new System.EventHandler(this.cN64_CheckedChanged);
            // 
            // cVC
            // 
            this.cVC.AutoSize = true;
            this.cVC.BackColor = System.Drawing.Color.Transparent;
            this.cVC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cVC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cVC.ForeColor = System.Drawing.Color.Black;
            this.cVC.Location = new System.Drawing.Point(34, 45);
            this.cVC.Name = "cVC";
            this.cVC.Size = new System.Drawing.Size(84, 17);
            this.cVC.TabIndex = 9;
            this.cVC.Text = "Wii VC .wad";
            this.cVC.UseVisualStyleBackColor = false;
            this.cVC.CheckedChanged += new System.EventHandler(this.cVC_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "ROM must be Majora\'s Mask (U) ending with \".z64\"";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.groupBox6);
            this.tabMain.Controls.Add(this.groupBox4);
            this.tabMain.Controls.Add(this.groupBox3);
            this.tabMain.Controls.Add(this.groupBox2);
            this.tabMain.Controls.Add(this.lMode);
            this.tabMain.Controls.Add(this.cMode);
            this.tabMain.Location = new System.Drawing.Point(4, 25);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(347, 477);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Main Settings";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lJunkLocationsAmount);
            this.groupBox6.Controls.Add(this.bJunkLocationsEditor);
            this.groupBox6.Controls.Add(this.tJunkLocationsList);
            this.groupBox6.Location = new System.Drawing.Point(10, 398);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(325, 72);
            this.groupBox6.TabIndex = 28;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Enforce Junk Locations";
            // 
            // lJunkLocationsAmount
            // 
            this.lJunkLocationsAmount.AutoSize = true;
            this.lJunkLocationsAmount.Location = new System.Drawing.Point(7, 27);
            this.lJunkLocationsAmount.Name = "lJunkLocationsAmount";
            this.lJunkLocationsAmount.Size = new System.Drawing.Size(112, 13);
            this.lJunkLocationsAmount.TabIndex = 27;
            this.lJunkLocationsAmount.Text = "0/0 locations selected";
            // 
            // bJunkLocationsEditor
            // 
            this.bJunkLocationsEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bJunkLocationsEditor.Location = new System.Drawing.Point(233, 12);
            this.bJunkLocationsEditor.Name = "bJunkLocationsEditor";
            this.bJunkLocationsEditor.Size = new System.Drawing.Size(86, 27);
            this.bJunkLocationsEditor.TabIndex = 26;
            this.bJunkLocationsEditor.Text = "Edit";
            this.bJunkLocationsEditor.UseVisualStyleBackColor = true;
            this.bJunkLocationsEditor.Click += new System.EventHandler(this.bJunkLocationsEditor_Click);
            // 
            // tJunkLocationsList
            // 
            this.tJunkLocationsList.Location = new System.Drawing.Point(6, 46);
            this.tJunkLocationsList.Name = "tJunkLocationsList";
            this.tJunkLocationsList.Size = new System.Drawing.Size(313, 20);
            this.tJunkLocationsList.TabIndex = 26;
            this.tJunkLocationsList.Text = "--";
            this.tJunkLocationsList.TextChanged += new System.EventHandler(this.tJunkLocationsList_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lCustomStartingItemAmount);
            this.groupBox4.Controls.Add(this.bStartingItemEditor);
            this.groupBox4.Controls.Add(this.tStartingItemList);
            this.groupBox4.Location = new System.Drawing.Point(10, 320);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(325, 72);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Extra Starting Items";
            // 
            // lCustomStartingItemAmount
            // 
            this.lCustomStartingItemAmount.AutoSize = true;
            this.lCustomStartingItemAmount.Location = new System.Drawing.Point(7, 27);
            this.lCustomStartingItemAmount.Name = "lCustomStartingItemAmount";
            this.lCustomStartingItemAmount.Size = new System.Drawing.Size(94, 13);
            this.lCustomStartingItemAmount.TabIndex = 27;
            this.lCustomStartingItemAmount.Text = "0/0 items selected";
            // 
            // bStartingItemEditor
            // 
            this.bStartingItemEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bStartingItemEditor.Location = new System.Drawing.Point(233, 12);
            this.bStartingItemEditor.Name = "bStartingItemEditor";
            this.bStartingItemEditor.Size = new System.Drawing.Size(86, 27);
            this.bStartingItemEditor.TabIndex = 26;
            this.bStartingItemEditor.Text = "Edit";
            this.bStartingItemEditor.UseVisualStyleBackColor = true;
            this.bStartingItemEditor.Click += new System.EventHandler(this.bStartingItemEditor_Click);
            // 
            // tStartingItemList
            // 
            this.tStartingItemList.Location = new System.Drawing.Point(6, 46);
            this.tStartingItemList.Name = "tStartingItemList";
            this.tStartingItemList.Size = new System.Drawing.Size(313, 20);
            this.tStartingItemList.TabIndex = 26;
            this.tStartingItemList.Text = "--";
            this.tStartingItemList.TextChanged += new System.EventHandler(this.tStartingItemList_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cNoStartingItems);
            this.groupBox3.Controls.Add(this.cDEnt);
            this.groupBox3.Controls.Add(this.cEnemy);
            this.groupBox3.Controls.Add(this.cMixSongs);
            this.groupBox3.Location = new System.Drawing.Point(10, 247);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(324, 71);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Other Customizations";
            // 
            // cNoStartingItems
            // 
            this.cNoStartingItems.AutoSize = true;
            this.cNoStartingItems.BackColor = System.Drawing.Color.Transparent;
            this.cNoStartingItems.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cNoStartingItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cNoStartingItems.ForeColor = System.Drawing.Color.Black;
            this.cNoStartingItems.Location = new System.Drawing.Point(203, 19);
            this.cNoStartingItems.Name = "cNoStartingItems";
            this.cNoStartingItems.Size = new System.Drawing.Size(107, 17);
            this.cNoStartingItems.TabIndex = 20;
            this.cNoStartingItems.Text = "No Starting Items";
            this.cNoStartingItems.UseVisualStyleBackColor = false;
            this.cNoStartingItems.CheckedChanged += new System.EventHandler(this.cNoStartingItems_CheckedChanged);
            // 
            // cDEnt
            // 
            this.cDEnt.AutoSize = true;
            this.cDEnt.BackColor = System.Drawing.Color.Transparent;
            this.cDEnt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cDEnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cDEnt.ForeColor = System.Drawing.Color.Black;
            this.cDEnt.Location = new System.Drawing.Point(29, 19);
            this.cDEnt.Name = "cDEnt";
            this.cDEnt.Size = new System.Drawing.Size(120, 17);
            this.cDEnt.TabIndex = 7;
            this.cDEnt.Text = "Dungeon entrances";
            this.cDEnt.UseVisualStyleBackColor = false;
            this.cDEnt.CheckedChanged += new System.EventHandler(this.cDEnt_CheckedChanged);
            // 
            // cEnemy
            // 
            this.cEnemy.AutoSize = true;
            this.cEnemy.BackColor = System.Drawing.Color.Transparent;
            this.cEnemy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cEnemy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cEnemy.ForeColor = System.Drawing.Color.Black;
            this.cEnemy.Location = new System.Drawing.Point(204, 42);
            this.cEnemy.Name = "cEnemy";
            this.cEnemy.Size = new System.Drawing.Size(106, 17);
            this.cEnemy.TabIndex = 9;
            this.cEnemy.Text = "Enemies (BETA!)";
            this.cEnemy.UseVisualStyleBackColor = false;
            this.cEnemy.CheckedChanged += new System.EventHandler(this.cEnemy_CheckedChanged);
            // 
            // cMixSongs
            // 
            this.cMixSongs.AutoSize = true;
            this.cMixSongs.BackColor = System.Drawing.Color.Transparent;
            this.cMixSongs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cMixSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cMixSongs.ForeColor = System.Drawing.Color.Black;
            this.cMixSongs.Location = new System.Drawing.Point(27, 42);
            this.cMixSongs.Name = "cMixSongs";
            this.cMixSongs.Size = new System.Drawing.Size(122, 17);
            this.cMixSongs.TabIndex = 3;
            this.cMixSongs.Text = "Mix songs with items";
            this.cMixSongs.UseVisualStyleBackColor = false;
            this.cMixSongs.CheckedChanged += new System.EventHandler(this.cMixSongs_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cMundaneRewards);
            this.groupBox2.Controls.Add(this.cStrayFairies);
            this.groupBox2.Controls.Add(this.cSpiders);
            this.groupBox2.Controls.Add(this.cCowMilk);
            this.groupBox2.Controls.Add(this.cFairyRewards);
            this.groupBox2.Controls.Add(this.lCustomItemAmount);
            this.groupBox2.Controls.Add(this.tCustomItemList);
            this.groupBox2.Controls.Add(this.bItemListEditor);
            this.groupBox2.Controls.Add(this.cSoS);
            this.groupBox2.Controls.Add(this.cCrazyStartingItems);
            this.groupBox2.Controls.Add(this.cDChests);
            this.groupBox2.Controls.Add(this.cBottled);
            this.groupBox2.Controls.Add(this.cNutChest);
            this.groupBox2.Controls.Add(this.cUserItems);
            this.groupBox2.Controls.Add(this.cShop);
            this.groupBox2.Controls.Add(this.cAdditional);
            this.groupBox2.Controls.Add(this.cMoonItems);
            this.groupBox2.Location = new System.Drawing.Point(10, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(325, 200);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Item Pool Options";
            // 
            // cMundaneRewards
            // 
            this.cMundaneRewards.AutoSize = true;
            this.cMundaneRewards.BackColor = System.Drawing.Color.Transparent;
            this.cMundaneRewards.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cMundaneRewards.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cMundaneRewards.ForeColor = System.Drawing.Color.Black;
            this.cMundaneRewards.Location = new System.Drawing.Point(33, 168);
            this.cMundaneRewards.Name = "cMundaneRewards";
            this.cMundaneRewards.Size = new System.Drawing.Size(116, 17);
            this.cMundaneRewards.TabIndex = 25;
            this.cMundaneRewards.Text = "Mundane Rewards";
            this.cMundaneRewards.UseVisualStyleBackColor = false;
            this.cMundaneRewards.CheckedChanged += new System.EventHandler(this.cMundaneRewards_CheckedChanged);
            // 
            // cStrayFairies
            // 
            this.cStrayFairies.AutoSize = true;
            this.cStrayFairies.BackColor = System.Drawing.Color.Transparent;
            this.cStrayFairies.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cStrayFairies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cStrayFairies.ForeColor = System.Drawing.Color.Black;
            this.cStrayFairies.Location = new System.Drawing.Point(227, 145);
            this.cStrayFairies.Name = "cStrayFairies";
            this.cStrayFairies.Size = new System.Drawing.Size(83, 17);
            this.cStrayFairies.TabIndex = 24;
            this.cStrayFairies.Text = "Stray Fairies";
            this.cStrayFairies.UseVisualStyleBackColor = false;
            this.cStrayFairies.CheckedChanged += new System.EventHandler(this.cStrayFairies_CheckedChanged);
            // 
            // cSpiders
            // 
            this.cSpiders.AutoSize = true;
            this.cSpiders.BackColor = System.Drawing.Color.Transparent;
            this.cSpiders.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cSpiders.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSpiders.ForeColor = System.Drawing.Color.Black;
            this.cSpiders.Location = new System.Drawing.Point(44, 145);
            this.cSpiders.Name = "cSpiders";
            this.cSpiders.Size = new System.Drawing.Size(105, 17);
            this.cSpiders.TabIndex = 23;
            this.cSpiders.Text = "Skulltula Tokens";
            this.cSpiders.UseVisualStyleBackColor = false;
            this.cSpiders.CheckedChanged += new System.EventHandler(this.cSpiders_CheckedChanged);
            // 
            // cCowMilk
            // 
            this.cCowMilk.AutoSize = true;
            this.cCowMilk.BackColor = System.Drawing.Color.Transparent;
            this.cCowMilk.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cCowMilk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cCowMilk.ForeColor = System.Drawing.Color.Black;
            this.cCowMilk.Location = new System.Drawing.Point(80, 122);
            this.cCowMilk.Name = "cCowMilk";
            this.cCowMilk.Size = new System.Drawing.Size(69, 17);
            this.cCowMilk.TabIndex = 22;
            this.cCowMilk.Text = "Cow Milk";
            this.cCowMilk.UseVisualStyleBackColor = false;
            this.cCowMilk.CheckedChanged += new System.EventHandler(this.cCowMilk_CheckedChanged);
            // 
            // cFairyRewards
            // 
            this.cFairyRewards.AutoSize = true;
            this.cFairyRewards.BackColor = System.Drawing.Color.Transparent;
            this.cFairyRewards.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cFairyRewards.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cFairyRewards.ForeColor = System.Drawing.Color.Black;
            this.cFairyRewards.Location = new System.Drawing.Point(188, 53);
            this.cFairyRewards.Name = "cFairyRewards";
            this.cFairyRewards.Size = new System.Drawing.Size(122, 17);
            this.cFairyRewards.TabIndex = 21;
            this.cFairyRewards.Text = "Great Fairy Rewards";
            this.cFairyRewards.UseVisualStyleBackColor = false;
            this.cFairyRewards.CheckedChanged += new System.EventHandler(this.cFairyRewards_CheckedChanged);
            // 
            // lCustomItemAmount
            // 
            this.lCustomItemAmount.AutoSize = true;
            this.lCustomItemAmount.Location = new System.Drawing.Point(6, 140);
            this.lCustomItemAmount.Name = "lCustomItemAmount";
            this.lCustomItemAmount.Size = new System.Drawing.Size(108, 13);
            this.lCustomItemAmount.TabIndex = 20;
            this.lCustomItemAmount.Text = "0/0 items randomized";
            this.lCustomItemAmount.Visible = false;
            // 
            // tCustomItemList
            // 
            this.tCustomItemList.Location = new System.Drawing.Point(6, 115);
            this.tCustomItemList.Name = "tCustomItemList";
            this.tCustomItemList.Size = new System.Drawing.Size(313, 20);
            this.tCustomItemList.TabIndex = 19;
            this.tCustomItemList.Text = "-----------";
            this.tCustomItemList.Visible = false;
            this.tCustomItemList.TextChanged += new System.EventHandler(this.tCustomItemList_TextChanged);
            // 
            // bItemListEditor
            // 
            this.bItemListEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bItemListEditor.Location = new System.Drawing.Point(120, 87);
            this.bItemListEditor.Name = "bItemListEditor";
            this.bItemListEditor.Size = new System.Drawing.Size(86, 24);
            this.bItemListEditor.TabIndex = 18;
            this.bItemListEditor.Text = "Item List Editor";
            this.bItemListEditor.UseVisualStyleBackColor = true;
            this.bItemListEditor.Visible = false;
            this.bItemListEditor.Click += new System.EventHandler(this.bItemListEditor_Click);
            // 
            // cSoS
            // 
            this.cSoS.AutoSize = true;
            this.cSoS.BackColor = System.Drawing.Color.Transparent;
            this.cSoS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cSoS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSoS.ForeColor = System.Drawing.Color.Black;
            this.cSoS.Location = new System.Drawing.Point(6, 30);
            this.cSoS.Name = "cSoS";
            this.cSoS.Size = new System.Drawing.Size(143, 17);
            this.cSoS.TabIndex = 10;
            this.cSoS.Text = "Exclude Song of Soaring";
            this.cSoS.UseVisualStyleBackColor = false;
            this.cSoS.CheckedChanged += new System.EventHandler(this.cSoS_CheckedChanged);
            // 
            // cCrazyStartingItems
            // 
            this.cCrazyStartingItems.AutoSize = true;
            this.cCrazyStartingItems.BackColor = System.Drawing.Color.Transparent;
            this.cCrazyStartingItems.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cCrazyStartingItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cCrazyStartingItems.ForeColor = System.Drawing.Color.Black;
            this.cCrazyStartingItems.Location = new System.Drawing.Point(191, 122);
            this.cCrazyStartingItems.Name = "cCrazyStartingItems";
            this.cCrazyStartingItems.Size = new System.Drawing.Size(119, 17);
            this.cCrazyStartingItems.TabIndex = 17;
            this.cCrazyStartingItems.Text = "Crazy Starting Items";
            this.cCrazyStartingItems.UseVisualStyleBackColor = false;
            this.cCrazyStartingItems.CheckedChanged += new System.EventHandler(this.cCrazyStartingItems_CheckedChanged);
            // 
            // cDChests
            // 
            this.cDChests.AutoSize = true;
            this.cDChests.BackColor = System.Drawing.Color.Transparent;
            this.cDChests.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cDChests.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cDChests.ForeColor = System.Drawing.Color.Black;
            this.cDChests.Location = new System.Drawing.Point(52, 53);
            this.cDChests.Name = "cDChests";
            this.cDChests.Size = new System.Drawing.Size(97, 17);
            this.cDChests.TabIndex = 4;
            this.cDChests.Text = "Dungeon items";
            this.cDChests.UseVisualStyleBackColor = false;
            this.cDChests.CheckedChanged += new System.EventHandler(this.cDChests_CheckedChanged);
            // 
            // cBottled
            // 
            this.cBottled.AutoSize = true;
            this.cBottled.BackColor = System.Drawing.Color.Transparent;
            this.cBottled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cBottled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBottled.ForeColor = System.Drawing.Color.Black;
            this.cBottled.Location = new System.Drawing.Point(16, 99);
            this.cBottled.Name = "cBottled";
            this.cBottled.Size = new System.Drawing.Size(133, 17);
            this.cBottled.TabIndex = 5;
            this.cBottled.Text = "Caught bottle contents";
            this.cBottled.UseVisualStyleBackColor = false;
            this.cBottled.CheckedChanged += new System.EventHandler(this.cBottled_CheckedChanged);
            // 
            // cNutChest
            // 
            this.cNutChest.AutoSize = true;
            this.cNutChest.BackColor = System.Drawing.Color.Transparent;
            this.cNutChest.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cNutChest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cNutChest.ForeColor = System.Drawing.Color.Black;
            this.cNutChest.Location = new System.Drawing.Point(166, 99);
            this.cNutChest.Name = "cNutChest";
            this.cNutChest.Size = new System.Drawing.Size(144, 17);
            this.cNutChest.TabIndex = 16;
            this.cNutChest.Text = "Pre-Clocktown Deku Nut";
            this.cNutChest.UseVisualStyleBackColor = false;
            this.cNutChest.CheckedChanged += new System.EventHandler(this.cNutChest_CheckedChanged);
            // 
            // cShop
            // 
            this.cShop.AutoSize = true;
            this.cShop.BackColor = System.Drawing.Color.Transparent;
            this.cShop.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cShop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cShop.ForeColor = System.Drawing.Color.Black;
            this.cShop.Location = new System.Drawing.Point(71, 76);
            this.cShop.Name = "cShop";
            this.cShop.Size = new System.Drawing.Size(78, 17);
            this.cShop.TabIndex = 6;
            this.cShop.Text = "Shop items";
            this.cShop.UseVisualStyleBackColor = false;
            this.cShop.CheckedChanged += new System.EventHandler(this.cShop_CheckedChanged);
            // 
            // cAdditional
            // 
            this.cAdditional.AutoSize = true;
            this.cAdditional.BackColor = System.Drawing.Color.Transparent;
            this.cAdditional.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cAdditional.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cAdditional.ForeColor = System.Drawing.Color.Black;
            this.cAdditional.Location = new System.Drawing.Point(212, 76);
            this.cAdditional.Name = "cAdditional";
            this.cAdditional.Size = new System.Drawing.Size(98, 17);
            this.cAdditional.TabIndex = 12;
            this.cAdditional.Text = "Everything else";
            this.cAdditional.UseVisualStyleBackColor = false;
            this.cAdditional.CheckedChanged += new System.EventHandler(this.cAdditional_CheckedChanged);
            // 
            // cMoonItems
            // 
            this.cMoonItems.AutoSize = true;
            this.cMoonItems.BackColor = System.Drawing.Color.Transparent;
            this.cMoonItems.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cMoonItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cMoonItems.ForeColor = System.Drawing.Color.Black;
            this.cMoonItems.Location = new System.Drawing.Point(230, 30);
            this.cMoonItems.Name = "cMoonItems";
            this.cMoonItems.Size = new System.Drawing.Size(80, 17);
            this.cMoonItems.TabIndex = 15;
            this.cMoonItems.Text = "Moon items";
            this.cMoonItems.UseVisualStyleBackColor = false;
            this.cMoonItems.CheckedChanged += new System.EventHandler(this.cMoonItems_CheckedChanged);
            // 
            // lMode
            // 
            this.lMode.AutoSize = true;
            this.lMode.BackColor = System.Drawing.Color.Transparent;
            this.lMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMode.ForeColor = System.Drawing.Color.Black;
            this.lMode.Location = new System.Drawing.Point(25, 17);
            this.lMode.Name = "lMode";
            this.lMode.Size = new System.Drawing.Size(68, 13);
            this.lMode.TabIndex = 0;
            this.lMode.Text = "Mode/Logic:";
            // 
            // cMode
            // 
            this.cMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMode.FormattingEnabled = true;
            this.cMode.Items.AddRange(new object[] {
            "Casual",
            "Glitched",
            "Vanilla Item Placement",
            "No Logic"});
            this.cMode.Location = new System.Drawing.Point(96, 13);
            this.cMode.Name = "cMode";
            this.cMode.Size = new System.Drawing.Size(222, 21);
            this.cMode.TabIndex = 1;
            this.cMode.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
            // 
            // tabGimmick
            // 
            this.tabGimmick.Controls.Add(this.cBlastCooldown);
            this.tabGimmick.Controls.Add(this.lBlastMask);
            this.tabGimmick.Controls.Add(this.label7);
            this.tabGimmick.Controls.Add(this.cHideClock);
            this.tabGimmick.Controls.Add(this.label6);
            this.tabGimmick.Controls.Add(this.cClockSpeed);
            this.tabGimmick.Controls.Add(this.label4);
            this.tabGimmick.Controls.Add(this.lFloors);
            this.tabGimmick.Controls.Add(this.lGravity);
            this.tabGimmick.Controls.Add(this.lDType);
            this.tabGimmick.Controls.Add(this.lDMult);
            this.tabGimmick.Controls.Add(this.cFloors);
            this.tabGimmick.Controls.Add(this.cDType);
            this.tabGimmick.Controls.Add(this.cDMult);
            this.tabGimmick.Controls.Add(this.cGravity);
            this.tabGimmick.Location = new System.Drawing.Point(4, 25);
            this.tabGimmick.Name = "tabGimmick";
            this.tabGimmick.Size = new System.Drawing.Size(347, 509);
            this.tabGimmick.TabIndex = 2;
            this.tabGimmick.Text = "Gimmicks";
            this.tabGimmick.UseVisualStyleBackColor = true;
            // 
            // cBlastCooldown
            // 
            this.cBlastCooldown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBlastCooldown.FormattingEnabled = true;
            this.cBlastCooldown.Items.AddRange(new object[] {
            "Default",
            "Instant",
            "Very short",
            "Short",
            "Long",
            "Very Long"});
            this.cBlastCooldown.Location = new System.Drawing.Point(127, 203);
            this.cBlastCooldown.Name = "cBlastCooldown";
            this.cBlastCooldown.Size = new System.Drawing.Size(158, 21);
            this.cBlastCooldown.TabIndex = 20;
            this.cBlastCooldown.SelectedIndexChanged += new System.EventHandler(this.cBlastCooldown_SelectedIndexChanged);
            // 
            // lBlastMask
            // 
            this.lBlastMask.AutoSize = true;
            this.lBlastMask.Location = new System.Drawing.Point(9, 205);
            this.lBlastMask.Name = "lBlastMask";
            this.lBlastMask.Size = new System.Drawing.Size(112, 13);
            this.lBlastMask.TabIndex = 19;
            this.lBlastMask.Text = "Blast Mask Cooldown:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label7.Location = new System.Drawing.Point(13, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(325, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "_____________________________________________________";
            // 
            // cHideClock
            // 
            this.cHideClock.AutoSize = true;
            this.cHideClock.BackColor = System.Drawing.Color.Transparent;
            this.cHideClock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cHideClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cHideClock.ForeColor = System.Drawing.Color.Black;
            this.cHideClock.Location = new System.Drawing.Point(55, 252);
            this.cHideClock.Name = "cHideClock";
            this.cHideClock.Size = new System.Drawing.Size(92, 17);
            this.cHideClock.TabIndex = 17;
            this.cHideClock.Text = "Hide Clock UI";
            this.cHideClock.UseVisualStyleBackColor = false;
            this.cHideClock.CheckedChanged += new System.EventHandler(this.cHideClock_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(52, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Clock speed:";
            // 
            // cClockSpeed
            // 
            this.cClockSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cClockSpeed.FormattingEnabled = true;
            this.cClockSpeed.Items.AddRange(new object[] {
            "Default",
            "1/3x",
            "2/3x",
            "2x",
            "3x",
            "6x"});
            this.cClockSpeed.Location = new System.Drawing.Point(127, 176);
            this.cClockSpeed.Name = "cClockSpeed";
            this.cClockSpeed.Size = new System.Drawing.Size(158, 21);
            this.cClockSpeed.TabIndex = 15;
            this.cClockSpeed.SelectedIndexChanged += new System.EventHandler(this.cClockSpeed_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(42, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(252, 52);
            this.label4.TabIndex = 14;
            this.label4.Text = "WARNING!\r\nThese settings are not considered in logic and some\r\ncan cause the seed" +
    " to be unbeatable.\r\nUse at your own risk!";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFloors
            // 
            this.lFloors.AutoSize = true;
            this.lFloors.Location = new System.Drawing.Point(60, 152);
            this.lFloors.Name = "lFloors";
            this.lFloors.Size = new System.Drawing.Size(61, 13);
            this.lFloors.TabIndex = 1;
            this.lFloors.Text = "Floor types:";
            // 
            // lGravity
            // 
            this.lGravity.AutoSize = true;
            this.lGravity.Location = new System.Drawing.Point(61, 125);
            this.lGravity.Name = "lGravity";
            this.lGravity.Size = new System.Drawing.Size(60, 13);
            this.lGravity.TabIndex = 1;
            this.lGravity.Text = "Movement:";
            // 
            // lDType
            // 
            this.lDType.AutoSize = true;
            this.lDType.Location = new System.Drawing.Point(36, 98);
            this.lDType.Name = "lDType";
            this.lDType.Size = new System.Drawing.Size(85, 13);
            this.lDType.TabIndex = 1;
            this.lDType.Text = "Damage effects:";
            // 
            // lDMult
            // 
            this.lDMult.AutoSize = true;
            this.lDMult.Location = new System.Drawing.Point(42, 71);
            this.lDMult.Name = "lDMult";
            this.lDMult.Size = new System.Drawing.Size(79, 13);
            this.lDMult.TabIndex = 1;
            this.lDMult.Text = "Damage mode:";
            // 
            // cFloors
            // 
            this.cFloors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cFloors.FormattingEnabled = true;
            this.cFloors.Items.AddRange(new object[] {
            "Default",
            "Sand",
            "Ice",
            "Snow",
            "Random"});
            this.cFloors.Location = new System.Drawing.Point(127, 149);
            this.cFloors.Name = "cFloors";
            this.cFloors.Size = new System.Drawing.Size(158, 21);
            this.cFloors.TabIndex = 0;
            this.cFloors.SelectedIndexChanged += new System.EventHandler(this.cFloors_SelectedIndexChanged);
            // 
            // cDType
            // 
            this.cDType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cDType.FormattingEnabled = true;
            this.cDType.Items.AddRange(new object[] {
            "Default",
            "Fire",
            "Ice",
            "Shock",
            "Knockdown",
            "Random"});
            this.cDType.Location = new System.Drawing.Point(127, 95);
            this.cDType.Name = "cDType";
            this.cDType.Size = new System.Drawing.Size(158, 21);
            this.cDType.TabIndex = 0;
            this.cDType.SelectedIndexChanged += new System.EventHandler(this.cDType_SelectedIndexChanged);
            // 
            // cDMult
            // 
            this.cDMult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cDMult.FormattingEnabled = true;
            this.cDMult.Items.AddRange(new object[] {
            "Default",
            "2x",
            "4x",
            "1-hit KO",
            "Doom"});
            this.cDMult.Location = new System.Drawing.Point(127, 68);
            this.cDMult.Name = "cDMult";
            this.cDMult.Size = new System.Drawing.Size(158, 21);
            this.cDMult.TabIndex = 0;
            this.cDMult.SelectedIndexChanged += new System.EventHandler(this.cDMult_SelectedIndexChanged);
            // 
            // cGravity
            // 
            this.cGravity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cGravity.FormattingEnabled = true;
            this.cGravity.Items.AddRange(new object[] {
            "Default",
            "High speed (many softlocks)",
            "Super low gravity",
            "Low gravity",
            "High gravity"});
            this.cGravity.Location = new System.Drawing.Point(127, 122);
            this.cGravity.Name = "cGravity";
            this.cGravity.Size = new System.Drawing.Size(158, 21);
            this.cGravity.TabIndex = 0;
            this.cGravity.SelectedIndexChanged += new System.EventHandler(this.cGravity_SelectedIndexChanged);
            // 
            // tabComfort
            // 
            this.tabComfort.Controls.Add(this.groupBox5);
            this.tabComfort.Controls.Add(this.lMusic);
            this.tabComfort.Controls.Add(this.cMusic);
            this.tabComfort.Controls.Add(this.cSFX);
            this.tabComfort.Controls.Add(this.cUpdateChests);
            this.tabComfort.Controls.Add(this.cEponaSword);
            this.tabComfort.Controls.Add(this.cShopAppearance);
            this.tabComfort.Controls.Add(this.lGossip);
            this.tabComfort.Controls.Add(this.cNoDowngrades);
            this.tabComfort.Controls.Add(this.cGossipHints);
            this.tabComfort.Controls.Add(this.cClearHints);
            this.tabComfort.Controls.Add(this.cFreeHints);
            this.tabComfort.Controls.Add(this.cCutsc);
            this.tabComfort.Controls.Add(this.cQText);
            this.tabComfort.Controls.Add(this.lTunic);
            this.tabComfort.Controls.Add(this.cTatl);
            this.tabComfort.Controls.Add(this.cLink);
            this.tabComfort.Controls.Add(this.bTunic);
            this.tabComfort.Controls.Add(this.lTatl);
            this.tabComfort.Controls.Add(this.lLink);
            this.tabComfort.Location = new System.Drawing.Point(4, 25);
            this.tabComfort.Name = "tabComfort";
            this.tabComfort.Padding = new System.Windows.Forms.Padding(3);
            this.tabComfort.Size = new System.Drawing.Size(347, 509);
            this.tabComfort.TabIndex = 1;
            this.tabComfort.Text = "Comfort/Cosmetics";
            this.tabComfort.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cGoodDogRaceRNG);
            this.groupBox5.Controls.Add(this.cFasterLabFish);
            this.groupBox5.Controls.Add(this.cGoodDampeRNG);
            this.groupBox5.Controls.Add(this.cSkipBeaver);
            this.groupBox5.Location = new System.Drawing.Point(7, 275);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(334, 127);
            this.groupBox5.TabIndex = 27;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Speed Ups";
            // 
            // cGoodDogRaceRNG
            // 
            this.cGoodDogRaceRNG.AutoSize = true;
            this.cGoodDogRaceRNG.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cGoodDogRaceRNG.Location = new System.Drawing.Point(14, 66);
            this.cGoodDogRaceRNG.Name = "cGoodDogRaceRNG";
            this.cGoodDogRaceRNG.Size = new System.Drawing.Size(131, 17);
            this.cGoodDogRaceRNG.TabIndex = 3;
            this.cGoodDogRaceRNG.Text = "Good Dog Race RNG";
            this.cGoodDogRaceRNG.UseVisualStyleBackColor = true;
            this.cGoodDogRaceRNG.CheckedChanged += new System.EventHandler(this.cGoodDogRaceRNG_CheckedChanged);
            // 
            // cFasterLabFish
            // 
            this.cFasterLabFish.AutoSize = true;
            this.cFasterLabFish.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cFasterLabFish.Location = new System.Drawing.Point(47, 89);
            this.cFasterLabFish.Name = "cFasterLabFish";
            this.cFasterLabFish.Size = new System.Drawing.Size(98, 17);
            this.cFasterLabFish.TabIndex = 2;
            this.cFasterLabFish.Text = "Faster Lab Fish";
            this.cFasterLabFish.UseVisualStyleBackColor = true;
            this.cFasterLabFish.CheckedChanged += new System.EventHandler(this.cFasterLabFish_CheckedChanged);
            // 
            // cGoodDampeRNG
            // 
            this.cGoodDampeRNG.AutoSize = true;
            this.cGoodDampeRNG.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cGoodDampeRNG.Location = new System.Drawing.Point(29, 43);
            this.cGoodDampeRNG.Name = "cGoodDampeRNG";
            this.cGoodDampeRNG.Size = new System.Drawing.Size(116, 17);
            this.cGoodDampeRNG.TabIndex = 1;
            this.cGoodDampeRNG.Text = "Good Dampe RNG";
            this.cGoodDampeRNG.UseVisualStyleBackColor = true;
            this.cGoodDampeRNG.CheckedChanged += new System.EventHandler(this.cGoodDampeRNG_CheckedChanged);
            // 
            // cSkipBeaver
            // 
            this.cSkipBeaver.AutoSize = true;
            this.cSkipBeaver.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cSkipBeaver.Location = new System.Drawing.Point(18, 20);
            this.cSkipBeaver.Name = "cSkipBeaver";
            this.cSkipBeaver.Size = new System.Drawing.Size(127, 17);
            this.cSkipBeaver.TabIndex = 0;
            this.cSkipBeaver.Text = "Skip Younger Beaver";
            this.cSkipBeaver.UseVisualStyleBackColor = true;
            this.cSkipBeaver.CheckedChanged += new System.EventHandler(this.cSkipBeaver_CheckedChanged);
            // 
            // lMusic
            // 
            this.lMusic.AutoSize = true;
            this.lMusic.BackColor = System.Drawing.Color.Transparent;
            this.lMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMusic.ForeColor = System.Drawing.Color.Black;
            this.lMusic.Location = new System.Drawing.Point(84, 251);
            this.lMusic.Name = "lMusic";
            this.lMusic.Size = new System.Drawing.Size(38, 13);
            this.lMusic.TabIndex = 26;
            this.lMusic.Text = "Music:";
            // 
            // cMusic
            // 
            this.cMusic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMusic.FormattingEnabled = true;
            this.cMusic.Items.AddRange(new object[] {
            "Default",
            "Random",
            "None (Risky)"});
            this.cMusic.Location = new System.Drawing.Point(128, 248);
            this.cMusic.Name = "cMusic";
            this.cMusic.Size = new System.Drawing.Size(167, 21);
            this.cMusic.TabIndex = 25;
            this.cMusic.SelectedIndexChanged += new System.EventHandler(this.cMusic_SelectedIndexChanged);
            // 
            // cSFX
            // 
            this.cSFX.AutoSize = true;
            this.cSFX.BackColor = System.Drawing.Color.Transparent;
            this.cSFX.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cSFX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSFX.ForeColor = System.Drawing.Color.Black;
            this.cSFX.Location = new System.Drawing.Point(43, 92);
            this.cSFX.Name = "cSFX";
            this.cSFX.Size = new System.Drawing.Size(102, 17);
            this.cSFX.TabIndex = 24;
            this.cSFX.Text = "Randomize SFX";
            this.cSFX.UseVisualStyleBackColor = false;
            this.cSFX.CheckedChanged += new System.EventHandler(this.cSFX_CheckedChanged);
            // 
            // cUpdateChests
            // 
            this.cUpdateChests.AutoSize = true;
            this.cUpdateChests.BackColor = System.Drawing.Color.Transparent;
            this.cUpdateChests.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cUpdateChests.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cUpdateChests.ForeColor = System.Drawing.Color.Black;
            this.cUpdateChests.Location = new System.Drawing.Point(200, 92);
            this.cUpdateChests.Name = "cUpdateChests";
            this.cUpdateChests.Size = new System.Drawing.Size(95, 17);
            this.cUpdateChests.TabIndex = 23;
            this.cUpdateChests.Text = "Update chests";
            this.cUpdateChests.UseVisualStyleBackColor = false;
            this.cUpdateChests.CheckedChanged += new System.EventHandler(this.cUpdateChests_CheckedChanged);
            // 
            // cEponaSword
            // 
            this.cEponaSword.AutoSize = true;
            this.cEponaSword.BackColor = System.Drawing.Color.Transparent;
            this.cEponaSword.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cEponaSword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cEponaSword.ForeColor = System.Drawing.Color.Black;
            this.cEponaSword.Location = new System.Drawing.Point(41, 69);
            this.cEponaSword.Name = "cEponaSword";
            this.cEponaSword.Size = new System.Drawing.Size(104, 17);
            this.cEponaSword.TabIndex = 22;
            this.cEponaSword.Text = "Fix Epona sword";
            this.cEponaSword.UseVisualStyleBackColor = false;
            this.cEponaSword.CheckedChanged += new System.EventHandler(this.cEponaSword_CheckedChanged);
            // 
            // cShopAppearance
            // 
            this.cShopAppearance.AutoSize = true;
            this.cShopAppearance.BackColor = System.Drawing.Color.Transparent;
            this.cShopAppearance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cShopAppearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cShopAppearance.ForeColor = System.Drawing.Color.Black;
            this.cShopAppearance.Location = new System.Drawing.Point(203, 69);
            this.cShopAppearance.Name = "cShopAppearance";
            this.cShopAppearance.Size = new System.Drawing.Size(92, 17);
            this.cShopAppearance.TabIndex = 21;
            this.cShopAppearance.Text = "Update shops";
            this.cShopAppearance.UseVisualStyleBackColor = false;
            this.cShopAppearance.CheckedChanged += new System.EventHandler(this.cShopAppearance_CheckedChanged);
            // 
            // lGossip
            // 
            this.lGossip.AutoSize = true;
            this.lGossip.BackColor = System.Drawing.Color.Transparent;
            this.lGossip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lGossip.ForeColor = System.Drawing.Color.Black;
            this.lGossip.Location = new System.Drawing.Point(24, 224);
            this.lGossip.Name = "lGossip";
            this.lGossip.Size = new System.Drawing.Size(98, 13);
            this.lGossip.TabIndex = 20;
            this.lGossip.Text = "Gossip Stone hints:";
            // 
            // cNoDowngrades
            // 
            this.cNoDowngrades.AutoSize = true;
            this.cNoDowngrades.BackColor = System.Drawing.Color.Transparent;
            this.cNoDowngrades.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cNoDowngrades.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cNoDowngrades.ForeColor = System.Drawing.Color.Black;
            this.cNoDowngrades.Location = new System.Drawing.Point(194, 115);
            this.cNoDowngrades.Name = "cNoDowngrades";
            this.cNoDowngrades.Size = new System.Drawing.Size(101, 17);
            this.cNoDowngrades.TabIndex = 18;
            this.cNoDowngrades.Text = "No downgrades";
            this.cNoDowngrades.UseVisualStyleBackColor = false;
            this.cNoDowngrades.CheckedChanged += new System.EventHandler(this.cNoDowngrades_CheckedChanged);
            // 
            // cGossipHints
            // 
            this.cGossipHints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cGossipHints.FormattingEnabled = true;
            this.cGossipHints.Items.AddRange(new object[] {
            "Default",
            "Random",
            "Relevant",
            "Competitive"});
            this.cGossipHints.Location = new System.Drawing.Point(128, 221);
            this.cGossipHints.Name = "cGossipHints";
            this.cGossipHints.Size = new System.Drawing.Size(167, 21);
            this.cGossipHints.TabIndex = 19;
            this.cGossipHints.SelectedIndexChanged += new System.EventHandler(this.cGossipHints_SelectedIndexChanged);
            // 
            // cClearHints
            // 
            this.cClearHints.AutoSize = true;
            this.cClearHints.BackColor = System.Drawing.Color.Transparent;
            this.cClearHints.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cClearHints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cClearHints.ForeColor = System.Drawing.Color.Black;
            this.cClearHints.Location = new System.Drawing.Point(220, 46);
            this.cClearHints.Name = "cClearHints";
            this.cClearHints.Size = new System.Drawing.Size(75, 17);
            this.cClearHints.TabIndex = 16;
            this.cClearHints.Text = "Clear hints";
            this.cClearHints.UseVisualStyleBackColor = false;
            this.cClearHints.CheckedChanged += new System.EventHandler(this.cClearHints_CheckedChanged);
            // 
            // cFreeHints
            // 
            this.cFreeHints.AutoSize = true;
            this.cFreeHints.BackColor = System.Drawing.Color.Transparent;
            this.cFreeHints.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cFreeHints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cFreeHints.ForeColor = System.Drawing.Color.Black;
            this.cFreeHints.Location = new System.Drawing.Point(223, 23);
            this.cFreeHints.Name = "cFreeHints";
            this.cFreeHints.Size = new System.Drawing.Size(72, 17);
            this.cFreeHints.TabIndex = 15;
            this.cFreeHints.Text = "Free hints";
            this.cFreeHints.UseVisualStyleBackColor = false;
            this.cFreeHints.CheckedChanged += new System.EventHandler(this.cFreeHints_CheckedChanged);
            // 
            // cCutsc
            // 
            this.cCutsc.AutoSize = true;
            this.cCutsc.BackColor = System.Drawing.Color.Transparent;
            this.cCutsc.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cCutsc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cCutsc.ForeColor = System.Drawing.Color.Black;
            this.cCutsc.Location = new System.Drawing.Point(30, 23);
            this.cCutsc.Name = "cCutsc";
            this.cCutsc.Size = new System.Drawing.Size(115, 17);
            this.cCutsc.TabIndex = 5;
            this.cCutsc.Text = "Shorten cutscenes";
            this.cCutsc.UseVisualStyleBackColor = false;
            this.cCutsc.CheckedChanged += new System.EventHandler(this.cCutsc_CheckedChanged);
            // 
            // cQText
            // 
            this.cQText.AutoSize = true;
            this.cQText.BackColor = System.Drawing.Color.Transparent;
            this.cQText.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cQText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cQText.ForeColor = System.Drawing.Color.Black;
            this.cQText.Location = new System.Drawing.Point(71, 46);
            this.cQText.Name = "cQText";
            this.cQText.Size = new System.Drawing.Size(74, 17);
            this.cQText.TabIndex = 6;
            this.cQText.Text = "Quick text";
            this.cQText.UseVisualStyleBackColor = false;
            this.cQText.CheckedChanged += new System.EventHandler(this.cQText_CheckedChanged);
            // 
            // lTunic
            // 
            this.lTunic.AutoSize = true;
            this.lTunic.BackColor = System.Drawing.Color.Transparent;
            this.lTunic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTunic.ForeColor = System.Drawing.Color.Black;
            this.lTunic.Location = new System.Drawing.Point(59, 143);
            this.lTunic.Name = "lTunic";
            this.lTunic.Size = new System.Drawing.Size(63, 13);
            this.lTunic.TabIndex = 7;
            this.lTunic.Text = "Tunic color:";
            // 
            // cTatl
            // 
            this.cTatl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cTatl.FormattingEnabled = true;
            this.cTatl.Items.AddRange(new object[] {
            "Default",
            "Dark",
            "Hot",
            "Cool",
            "Random",
            "Rainbow (cycle)"});
            this.cTatl.Location = new System.Drawing.Point(128, 194);
            this.cTatl.Name = "cTatl";
            this.cTatl.Size = new System.Drawing.Size(167, 21);
            this.cTatl.TabIndex = 12;
            this.cTatl.SelectedIndexChanged += new System.EventHandler(this.cTatl_SelectedIndexChanged);
            // 
            // cLink
            // 
            this.cLink.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cLink.FormattingEnabled = true;
            this.cLink.Items.AddRange(new object[] {
            "Link (MM)",
            "Link (OoT)",
            "Adult Link (Risky!)",
            "Kafei"});
            this.cLink.Location = new System.Drawing.Point(128, 167);
            this.cLink.Name = "cLink";
            this.cLink.Size = new System.Drawing.Size(167, 21);
            this.cLink.TabIndex = 10;
            this.cLink.SelectedIndexChanged += new System.EventHandler(this.cLink_SelectedIndexChanged);
            // 
            // bTunic
            // 
            this.bTunic.BackColor = System.Drawing.Color.White;
            this.bTunic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bTunic.Location = new System.Drawing.Point(128, 138);
            this.bTunic.Name = "bTunic";
            this.bTunic.Size = new System.Drawing.Size(167, 23);
            this.bTunic.TabIndex = 8;
            this.bTunic.UseVisualStyleBackColor = false;
            this.bTunic.Click += new System.EventHandler(this.bTunic_Click);
            // 
            // lTatl
            // 
            this.lTatl.AutoSize = true;
            this.lTatl.BackColor = System.Drawing.Color.Transparent;
            this.lTatl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTatl.ForeColor = System.Drawing.Color.Black;
            this.lTatl.Location = new System.Drawing.Point(28, 197);
            this.lTatl.Name = "lTatl";
            this.lTatl.Size = new System.Drawing.Size(94, 13);
            this.lTatl.TabIndex = 11;
            this.lTatl.Text = "Tatl color scheme:";
            // 
            // lLink
            // 
            this.lLink.AutoSize = true;
            this.lLink.BackColor = System.Drawing.Color.Transparent;
            this.lLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLink.ForeColor = System.Drawing.Color.Black;
            this.lLink.Location = new System.Drawing.Point(52, 170);
            this.lLink.Name = "lLink";
            this.lLink.Size = new System.Drawing.Size(70, 13);
            this.lLink.TabIndex = 9;
            this.lLink.Text = "Player model:";
            // 
            // bApplyPatch
            // 
            this.bApplyPatch.Location = new System.Drawing.Point(6, 32);
            this.bApplyPatch.Name = "bApplyPatch";
            this.bApplyPatch.Size = new System.Drawing.Size(335, 26);
            this.bApplyPatch.TabIndex = 16;
            this.bApplyPatch.Text = "Apply Patch";
            this.bApplyPatch.UseVisualStyleBackColor = true;
            this.bApplyPatch.Click += new System.EventHandler(this.bApplyPatch_Click);
            // 
            // saveROM
            // 
            this.saveROM.DefaultExt = "z64";
            this.saveROM.Filter = "ROM files|*.z64";
            // 
            // cTunic
            // 
            this.cTunic.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(105)))), ((int)(((byte)(27)))));
            // 
            // bRandomise
            // 
            this.bRandomise.Location = new System.Drawing.Point(251, 7);
            this.bRandomise.Name = "bRandomise";
            this.bRandomise.Size = new System.Drawing.Size(92, 50);
            this.bRandomise.TabIndex = 5;
            this.bRandomise.Text = "Randomize";
            this.bRandomise.UseVisualStyleBackColor = true;
            this.bRandomise.Click += new System.EventHandler(this.bRandomise_Click);
            // 
            // saveWad
            // 
            this.saveWad.DefaultExt = "wad";
            this.saveWad.Filter = "VC files|*.wad";
            // 
            // mMenu
            // 
            this.mMenu.BackColor = System.Drawing.SystemColors.Control;
            this.mMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFile,
            this.mCustomise,
            this.mHelp});
            this.mMenu.Location = new System.Drawing.Point(0, 0);
            this.mMenu.Name = "mMenu";
            this.mMenu.Size = new System.Drawing.Size(355, 24);
            this.mMenu.TabIndex = 12;
            this.mMenu.Text = "mMenu";
            // 
            // mFile
            // 
            this.mFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importCustomLogicToolStripMenuItem,
            this.mExit});
            this.mFile.Name = "mFile";
            this.mFile.Size = new System.Drawing.Size(37, 20);
            this.mFile.Text = "File";
            // 
            // importCustomLogicToolStripMenuItem
            // 
            this.importCustomLogicToolStripMenuItem.Name = "importCustomLogicToolStripMenuItem";
            this.importCustomLogicToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.importCustomLogicToolStripMenuItem.Text = "Import Custom Logic";
            this.importCustomLogicToolStripMenuItem.Click += new System.EventHandler(this.ImportCustomLogicToolStripMenuItem_Click);
            // 
            // mExit
            // 
            this.mExit.Name = "mExit";
            this.mExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mExit.Size = new System.Drawing.Size(187, 22);
            this.mExit.Text = "Exit";
            this.mExit.Click += new System.EventHandler(this.mExit_Click);
            // 
            // mCustomise
            // 
            this.mCustomise.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mDPadConfig,
            this.mLogicEdit});
            this.mCustomise.Name = "mCustomise";
            this.mCustomise.Size = new System.Drawing.Size(75, 20);
            this.mCustomise.Text = "Customize";
            // 
            // mDPadConfig
            // 
            this.mDPadConfig.Name = "mDPadConfig";
            this.mDPadConfig.Size = new System.Drawing.Size(184, 22);
            this.mDPadConfig.Text = "D-Pad Configuration";
            this.mDPadConfig.Click += new System.EventHandler(this.mDPadConfig_Click);
            // 
            // mLogicEdit
            // 
            this.mLogicEdit.Name = "mLogicEdit";
            this.mLogicEdit.Size = new System.Drawing.Size(184, 22);
            this.mLogicEdit.Text = "Logic editor";
            this.mLogicEdit.Click += new System.EventHandler(this.mLogicEdit_Click);
            // 
            // mHelp
            // 
            this.mHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mManual,
            this.mSep1,
            this.mAbout});
            this.mHelp.Name = "mHelp";
            this.mHelp.Size = new System.Drawing.Size(44, 20);
            this.mHelp.Text = "Help";
            // 
            // mManual
            // 
            this.mManual.Name = "mManual";
            this.mManual.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mManual.Size = new System.Drawing.Size(133, 22);
            this.mManual.Text = "Manual";
            this.mManual.Click += new System.EventHandler(this.mManual_Click);
            // 
            // mSep1
            // 
            this.mSep1.Name = "mSep1";
            this.mSep1.Size = new System.Drawing.Size(130, 6);
            // 
            // mAbout
            // 
            this.mAbout.Name = "mAbout";
            this.mAbout.Size = new System.Drawing.Size(133, 22);
            this.mAbout.Text = "About";
            this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // openBROM
            // 
            this.openBROM.Filter = "ROM files|*.z64;*.v64;*.n64";
            // 
            // pProgress
            // 
            this.pProgress.Location = new System.Drawing.Point(11, 649);
            this.pProgress.Margin = new System.Windows.Forms.Padding(2);
            this.pProgress.Name = "pProgress";
            this.pProgress.Size = new System.Drawing.Size(339, 19);
            this.pProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pProgress.TabIndex = 13;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.BackColor = System.Drawing.Color.Transparent;
            this.lStatus.Location = new System.Drawing.Point(15, 630);
            this.lStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(47, 13);
            this.lStatus.TabIndex = 13;
            this.lStatus.Text = "Ready...";
            // 
            // tSeed
            // 
            this.tSeed.Location = new System.Drawing.Point(77, 10);
            this.tSeed.MaxLength = 10;
            this.tSeed.Name = "tSeed";
            this.tSeed.Size = new System.Drawing.Size(168, 20);
            this.tSeed.TabIndex = 2;
            this.tSeed.Enter += new System.EventHandler(this.tSeed_Enter);
            this.tSeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tSeed_KeyDown);
            this.tSeed.Leave += new System.EventHandler(this.tSeed_Leave);
            // 
            // lSeed
            // 
            this.lSeed.AutoSize = true;
            this.lSeed.BackColor = System.Drawing.Color.Transparent;
            this.lSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSeed.ForeColor = System.Drawing.Color.Black;
            this.lSeed.Location = new System.Drawing.Point(1, 13);
            this.lSeed.Name = "lSeed";
            this.lSeed.Size = new System.Drawing.Size(76, 13);
            this.lSeed.TabIndex = 3;
            this.lSeed.Text = "Random seed:";
            // 
            // tSString
            // 
            this.tSString.Location = new System.Drawing.Point(77, 35);
            this.tSString.Name = "tSString";
            this.tSString.Size = new System.Drawing.Size(168, 20);
            this.tSString.TabIndex = 6;
            this.tSString.Enter += new System.EventHandler(this.tSString_Enter);
            this.tSString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tSString_KeyDown);
            this.tSString.Leave += new System.EventHandler(this.tSString_Leave);
            // 
            // lSString
            // 
            this.lSString.AutoSize = true;
            this.lSString.BackColor = System.Drawing.Color.Transparent;
            this.lSString.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSString.ForeColor = System.Drawing.Color.Black;
            this.lSString.Location = new System.Drawing.Point(1, 35);
            this.lSString.Name = "lSString";
            this.lSString.Size = new System.Drawing.Size(76, 13);
            this.lSString.TabIndex = 7;
            this.lSString.Text = "Settings string:";
            // 
            // cDummy
            // 
            this.cDummy.AutoSize = true;
            this.cDummy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cDummy.Enabled = false;
            this.cDummy.Location = new System.Drawing.Point(265, 626);
            this.cDummy.Name = "cDummy";
            this.cDummy.Size = new System.Drawing.Size(80, 17);
            this.cDummy.TabIndex = 9;
            this.cDummy.Text = "checkBox1";
            this.cDummy.UseVisualStyleBackColor = true;
            this.cDummy.Visible = false;
            // 
            // lHrule
            // 
            this.lHrule.AutoSize = true;
            this.lHrule.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.lHrule.Location = new System.Drawing.Point(20, 473);
            this.lHrule.Name = "lHrule";
            this.lHrule.Size = new System.Drawing.Size(325, 13);
            this.lHrule.TabIndex = 12;
            this.lHrule.Text = "_____________________________________________________";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label5.Location = new System.Drawing.Point(20, 611);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(325, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "_____________________________________________________";
            // 
            // openPatch
            // 
            this.openPatch.Filter = "MMR Patch files|*.mmr";
            // 
            // ttOutput
            // 
            this.ttOutput.Controls.Add(this.tpOutputSettings);
            this.ttOutput.Controls.Add(this.tpPatchSettings);
            this.ttOutput.Location = new System.Drawing.Point(4, 531);
            this.ttOutput.Name = "ttOutput";
            this.ttOutput.SelectedIndex = 0;
            this.ttOutput.Size = new System.Drawing.Size(354, 89);
            this.ttOutput.TabIndex = 15;
            this.ttOutput.SelectedIndexChanged += new System.EventHandler(this.ttOutput_Changed);
            // 
            // tpOutputSettings
            // 
            this.tpOutputSettings.Controls.Add(this.bRandomise);
            this.tpOutputSettings.Controls.Add(this.tSeed);
            this.tpOutputSettings.Controls.Add(this.lSeed);
            this.tpOutputSettings.Controls.Add(this.tSString);
            this.tpOutputSettings.Controls.Add(this.lSString);
            this.tpOutputSettings.Location = new System.Drawing.Point(4, 22);
            this.tpOutputSettings.Name = "tpOutputSettings";
            this.tpOutputSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutputSettings.Size = new System.Drawing.Size(346, 63);
            this.tpOutputSettings.TabIndex = 0;
            this.tpOutputSettings.Text = "Output settings";
            this.tpOutputSettings.UseVisualStyleBackColor = true;
            // 
            // tpPatchSettings
            // 
            this.tpPatchSettings.Controls.Add(this.tPatch);
            this.tpPatchSettings.Controls.Add(this.bLoadPatch);
            this.tpPatchSettings.Controls.Add(this.bApplyPatch);
            this.tpPatchSettings.Location = new System.Drawing.Point(4, 22);
            this.tpPatchSettings.Name = "tpPatchSettings";
            this.tpPatchSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpPatchSettings.Size = new System.Drawing.Size(346, 63);
            this.tpPatchSettings.TabIndex = 1;
            this.tpPatchSettings.Text = "Patch settings";
            this.tpPatchSettings.UseVisualStyleBackColor = true;
            // 
            // tPatch
            // 
            this.tPatch.Location = new System.Drawing.Point(111, 8);
            this.tPatch.Name = "tPatch";
            this.tPatch.ReadOnly = true;
            this.tPatch.Size = new System.Drawing.Size(230, 20);
            this.tPatch.TabIndex = 17;
            // 
            // bLoadPatch
            // 
            this.bLoadPatch.Location = new System.Drawing.Point(6, 5);
            this.bLoadPatch.Name = "bLoadPatch";
            this.bLoadPatch.Size = new System.Drawing.Size(103, 25);
            this.bLoadPatch.TabIndex = 16;
            this.bLoadPatch.Text = "Load Patch...";
            this.bLoadPatch.UseVisualStyleBackColor = true;
            this.bLoadPatch.Click += new System.EventHandler(this.BLoadPatch_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(355, 677);
            this.Controls.Add(this.ttOutput);
            this.Controls.Add(this.lStatus);
            this.Controls.Add(this.cDummy);
            this.Controls.Add(this.pProgress);
            this.Controls.Add(this.tSettings);
            this.Controls.Add(this.mMenu);
            this.Controls.Add(this.lHrule);
            this.Controls.Add(this.label5);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.mmrMain_Load);
            this.tSettings.ResumeLayout(false);
            this.tabROMSettings.ResumeLayout(false);
            this.tabROMSettings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gGameOutput.ResumeLayout(false);
            this.gGameOutput.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabGimmick.ResumeLayout(false);
            this.tabGimmick.PerformLayout();
            this.tabComfort.ResumeLayout(false);
            this.tabComfort.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.mMenu.ResumeLayout(false);
            this.mMenu.PerformLayout();
            this.ttOutput.ResumeLayout(false);
            this.tpOutputSettings.ResumeLayout(false);
            this.tpOutputSettings.PerformLayout();
            this.tpPatchSettings.ResumeLayout(false);
            this.tpPatchSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bopen;
        private System.Windows.Forms.OpenFileDialog openROM;
        private System.Windows.Forms.OpenFileDialog openPatch;
        private System.Windows.Forms.OpenFileDialog openLogic;
        private System.Windows.Forms.TextBox tROMName;
        private System.Windows.Forms.ComboBox cMode;
        private System.Windows.Forms.Label lMode;
        private System.Windows.Forms.SaveFileDialog saveROM;
        private System.Windows.Forms.ComboBox cTatl;
        private System.Windows.Forms.Label lTatl;
        private System.Windows.Forms.ComboBox cLink;
        private System.Windows.Forms.Label lLink;
        private System.Windows.Forms.Button bTunic;
        private System.Windows.Forms.Label lTunic;
        private System.Windows.Forms.CheckBox cQText;
        private System.Windows.Forms.CheckBox cCutsc;
        private System.Windows.Forms.CheckBox cEnemy;
        private System.Windows.Forms.CheckBox cDEnt;
        private System.Windows.Forms.CheckBox cShop;
        private System.Windows.Forms.CheckBox cBottled;
        private System.Windows.Forms.CheckBox cDChests;
        private System.Windows.Forms.CheckBox cMixSongs;
        private System.Windows.Forms.ColorDialog cTunic;
        private System.Windows.Forms.Button bRandomise;
        private System.Windows.Forms.CheckBox cSoS;
        private System.Windows.Forms.TabControl tSettings;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.TabPage tabComfort;
        private System.Windows.Forms.TabPage tabGimmick;
        private System.Windows.Forms.Label lFloors;
        private System.Windows.Forms.Label lGravity;
        private System.Windows.Forms.Label lDType;
        private System.Windows.Forms.Label lDMult;
        private System.Windows.Forms.ComboBox cFloors;
        private System.Windows.Forms.ComboBox cDType;
        private System.Windows.Forms.ComboBox cDMult;
        private System.Windows.Forms.ComboBox cGravity;
        private System.Windows.Forms.SaveFileDialog saveWad;
        private System.Windows.Forms.CheckBox cVC;
        private System.Windows.Forms.CheckBox cN64;
        private System.Windows.Forms.MenuStrip mMenu;
        private System.Windows.Forms.ToolStripMenuItem mFile;
        private System.Windows.Forms.ToolStripMenuItem mExit;
        private System.Windows.Forms.ToolStripMenuItem mHelp;
        private System.Windows.Forms.ToolStripMenuItem mManual;
        private System.Windows.Forms.ToolStripMenuItem mAbout;
        private System.Windows.Forms.ToolStripSeparator mSep1;
        private System.Windows.Forms.OpenFileDialog openBROM;
        private System.Windows.Forms.CheckBox cAdditional;
        private System.Windows.Forms.ToolStripMenuItem mCustomise;
        private System.Windows.Forms.ToolStripMenuItem mLogicEdit;
        private System.Windows.Forms.CheckBox cUserItems;
        private System.Windows.Forms.ProgressBar pProgress;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.TabPage tabROMSettings;
        private System.Windows.Forms.TextBox tSeed;
        private System.Windows.Forms.Label lSeed;
        private System.Windows.Forms.TextBox tSString;
        private System.Windows.Forms.Label lSString;
        private System.Windows.Forms.CheckBox cSpoiler;
        private System.Windows.Forms.CheckBox cDummy;
        private System.Windows.Forms.Label lHrule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cFreeHints;
        private System.Windows.Forms.CheckBox cHTMLLog;
        private System.Windows.Forms.CheckBox cMoonItems;
        private System.Windows.Forms.CheckBox cPatch;
        private System.Windows.Forms.Button bApplyPatch;
        private System.Windows.Forms.TabControl ttOutput;
        private System.Windows.Forms.TabPage tpOutputSettings;
        private System.Windows.Forms.TabPage tpPatchSettings;
        private System.Windows.Forms.TextBox tPatch;
        private System.Windows.Forms.Button bLoadPatch;
        private System.Windows.Forms.CheckBox cClearHints;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cClockSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cHideClock;
        private System.Windows.Forms.CheckBox cNoDowngrades;
        private System.Windows.Forms.Label lGossip;
        private System.Windows.Forms.ComboBox cGossipHints;
        private System.Windows.Forms.CheckBox cShopAppearance;
        private System.Windows.Forms.CheckBox cNutChest;
        private System.Windows.Forms.CheckBox cCrazyStartingItems;
        private System.Windows.Forms.CheckBox cEponaSword;
        private System.Windows.Forms.CheckBox cUpdateChests;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gGameOutput;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cNoStartingItems;
        private System.Windows.Forms.Button bItemListEditor;
        private System.Windows.Forms.TextBox tCustomItemList;
        private System.Windows.Forms.Label lCustomItemAmount;
        private System.Windows.Forms.CheckBox cFairyRewards;
        private System.Windows.Forms.CheckBox cCowMilk;
        private System.Windows.Forms.CheckBox cSpiders;
        private System.Windows.Forms.CheckBox cStrayFairies;
        private System.Windows.Forms.CheckBox cMundaneRewards;
        private System.Windows.Forms.ComboBox cBlastCooldown;
        private System.Windows.Forms.Label lBlastMask;
        private System.Windows.Forms.CheckBox cSFX;
        private System.Windows.Forms.Label lMusic;
        private System.Windows.Forms.ComboBox cMusic;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button bStartingItemEditor;
        private System.Windows.Forms.TextBox tStartingItemList;
        private System.Windows.Forms.Label lCustomStartingItemAmount;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cGoodDogRaceRNG;
        private System.Windows.Forms.CheckBox cFasterLabFish;
        private System.Windows.Forms.CheckBox cGoodDampeRNG;
        private System.Windows.Forms.CheckBox cSkipBeaver;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lJunkLocationsAmount;
        private System.Windows.Forms.Button bJunkLocationsEditor;
        private System.Windows.Forms.TextBox tJunkLocationsList;
        private System.Windows.Forms.ToolStripMenuItem mDPadConfig;
        private System.Windows.Forms.ToolStripMenuItem importCustomLogicToolStripMenuItem;
    }
}

