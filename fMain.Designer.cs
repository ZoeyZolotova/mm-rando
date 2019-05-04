namespace MMRando
{
    partial class mmrMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mmrMain));
            this.bopen = new System.Windows.Forms.Button();
            this.openROM = new System.Windows.Forms.OpenFileDialog();
            this.tROMName = new System.Windows.Forms.TextBox();
            this.tSeed = new System.Windows.Forms.TextBox();
            this.lSeed = new System.Windows.Forms.Label();
            this.gSettings = new System.Windows.Forms.GroupBox();
            this.lHrule = new System.Windows.Forms.Label();
            this.cUserItems = new System.Windows.Forms.CheckBox();
            this.tSettings = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.cAdditional = new System.Windows.Forms.CheckBox();
            this.cGossip = new System.Windows.Forms.CheckBox();
            this.cSoS = new System.Windows.Forms.CheckBox();
            this.cMixSongs = new System.Windows.Forms.CheckBox();
            this.cEnemy = new System.Windows.Forms.CheckBox();
            this.cDChests = new System.Windows.Forms.CheckBox();
            this.cBGM = new System.Windows.Forms.CheckBox();
            this.cBottled = new System.Windows.Forms.CheckBox();
            this.cDEnt = new System.Windows.Forms.CheckBox();
            this.cShop = new System.Windows.Forms.CheckBox();
            this.tabGimmick = new System.Windows.Forms.TabPage();
            this.lFloors = new System.Windows.Forms.Label();
            this.lGravity = new System.Windows.Forms.Label();
            this.lDType = new System.Windows.Forms.Label();
            this.lDMult = new System.Windows.Forms.Label();
            this.cFloors = new System.Windows.Forms.ComboBox();
            this.cDType = new System.Windows.Forms.ComboBox();
            this.cDMult = new System.Windows.Forms.ComboBox();
            this.cGravity = new System.Windows.Forms.ComboBox();
            this.tabComfort = new System.Windows.Forms.TabPage();
            this.cTatl = new System.Windows.Forms.ComboBox();
            this.cCutsc = new System.Windows.Forms.CheckBox();
            this.lTatl = new System.Windows.Forms.Label();
            this.cQText = new System.Windows.Forms.CheckBox();
            this.cLink = new System.Windows.Forms.ComboBox();
            this.lTunic = new System.Windows.Forms.Label();
            this.lLink = new System.Windows.Forms.Label();
            this.bTunic = new System.Windows.Forms.Button();
            this.cDummy = new System.Windows.Forms.CheckBox();
            this.cSpoiler = new System.Windows.Forms.CheckBox();
            this.lSString = new System.Windows.Forms.Label();
            this.tSString = new System.Windows.Forms.TextBox();
            this.cMode = new System.Windows.Forms.ComboBox();
            this.lMode = new System.Windows.Forms.Label();
            this.saveROM = new System.Windows.Forms.SaveFileDialog();
            this.cTunic = new System.Windows.Forms.ColorDialog();
            this.bRandomise = new System.Windows.Forms.Button();
            this.saveWad = new System.Windows.Forms.SaveFileDialog();
            this.cVC = new System.Windows.Forms.CheckBox();
            this.cN64 = new System.Windows.Forms.CheckBox();
            this.lOutput = new System.Windows.Forms.Label();
            this.mMenu = new System.Windows.Forms.MenuStrip();
            this.mFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mByteswap = new System.Windows.Forms.ToolStripMenuItem();
            this.mCustomise = new System.Windows.Forms.ToolStripMenuItem();
            this.mLogicEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mItemIncl = new System.Windows.Forms.ToolStripMenuItem();
            this.mHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mManual = new System.Windows.Forms.ToolStripMenuItem();
            this.mSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.openBROM = new System.Windows.Forms.OpenFileDialog();
            this.openLogic = new System.Windows.Forms.OpenFileDialog();
            this.gSettings.SuspendLayout();
            this.tSettings.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabGimmick.SuspendLayout();
            this.tabComfort.SuspendLayout();
            this.mMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // bopen
            // 
            this.bopen.Location = new System.Drawing.Point(13, 37);
            this.bopen.Name = "bopen";
            this.bopen.Size = new System.Drawing.Size(103, 23);
            this.bopen.TabIndex = 0;
            this.bopen.Text = "Open ROM...";
            this.bopen.UseVisualStyleBackColor = true;
            this.bopen.Click += new System.EventHandler(this.bopen_Click);
            // 
            // openROM
            // 
            this.openROM.Filter = "ROM files|*.z64";
            // 
            // tROMName
            // 
            this.tROMName.Location = new System.Drawing.Point(122, 40);
            this.tROMName.Name = "tROMName";
            this.tROMName.ReadOnly = true;
            this.tROMName.Size = new System.Drawing.Size(230, 20);
            this.tROMName.TabIndex = 1;
            // 
            // tSeed
            // 
            this.tSeed.Location = new System.Drawing.Point(14, 136);
            this.tSeed.MaxLength = 10;
            this.tSeed.Name = "tSeed";
            this.tSeed.Size = new System.Drawing.Size(140, 20);
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
            this.lSeed.Location = new System.Drawing.Point(11, 120);
            this.lSeed.Name = "lSeed";
            this.lSeed.Size = new System.Drawing.Size(76, 13);
            this.lSeed.TabIndex = 3;
            this.lSeed.Text = "Random seed:";
            // 
            // gSettings
            // 
            this.gSettings.BackColor = System.Drawing.Color.Transparent;
            this.gSettings.Controls.Add(this.lHrule);
            this.gSettings.Controls.Add(this.cUserItems);
            this.gSettings.Controls.Add(this.tSettings);
            this.gSettings.Controls.Add(this.cDummy);
            this.gSettings.Controls.Add(this.cSpoiler);
            this.gSettings.Controls.Add(this.lSString);
            this.gSettings.Controls.Add(this.tSString);
            this.gSettings.Controls.Add(this.lSeed);
            this.gSettings.Controls.Add(this.tSeed);
            this.gSettings.Controls.Add(this.cMode);
            this.gSettings.Controls.Add(this.lMode);
            this.gSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gSettings.ForeColor = System.Drawing.Color.Black;
            this.gSettings.Location = new System.Drawing.Point(13, 148);
            this.gSettings.Name = "gSettings";
            this.gSettings.Size = new System.Drawing.Size(339, 387);
            this.gSettings.TabIndex = 4;
            this.gSettings.TabStop = false;
            this.gSettings.Text = "Settings";
            // 
            // lHrule
            // 
            this.lHrule.AutoSize = true;
            this.lHrule.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.lHrule.Location = new System.Drawing.Point(7, 91);
            this.lHrule.Name = "lHrule";
            this.lHrule.Size = new System.Drawing.Size(325, 13);
            this.lHrule.TabIndex = 12;
            this.lHrule.Text = "_____________________________________________________";
            // 
            // cUserItems
            // 
            this.cUserItems.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cUserItems.AutoSize = true;
            this.cUserItems.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cUserItems.Location = new System.Drawing.Point(199, 70);
            this.cUserItems.Name = "cUserItems";
            this.cUserItems.Size = new System.Drawing.Size(119, 17);
            this.cUserItems.TabIndex = 11;
            this.cUserItems.Text = "Use custom item list";
            this.cUserItems.UseVisualStyleBackColor = true;
            this.cUserItems.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // tSettings
            // 
            this.tSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tSettings.Controls.Add(this.tabMain);
            this.tSettings.Controls.Add(this.tabGimmick);
            this.tSettings.Controls.Add(this.tabComfort);
            this.tSettings.Location = new System.Drawing.Point(6, 201);
            this.tSettings.Name = "tSettings";
            this.tSettings.SelectedIndex = 0;
            this.tSettings.Size = new System.Drawing.Size(327, 171);
            this.tSettings.TabIndex = 10;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.cAdditional);
            this.tabMain.Controls.Add(this.cGossip);
            this.tabMain.Controls.Add(this.cSoS);
            this.tabMain.Controls.Add(this.cMixSongs);
            this.tabMain.Controls.Add(this.cEnemy);
            this.tabMain.Controls.Add(this.cDChests);
            this.tabMain.Controls.Add(this.cBGM);
            this.tabMain.Controls.Add(this.cBottled);
            this.tabMain.Controls.Add(this.cDEnt);
            this.tabMain.Controls.Add(this.cShop);
            this.tabMain.Location = new System.Drawing.Point(4, 25);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(319, 142);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Random elements";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // cAdditional
            // 
            this.cAdditional.AutoSize = true;
            this.cAdditional.BackColor = System.Drawing.Color.Transparent;
            this.cAdditional.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cAdditional.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cAdditional.ForeColor = System.Drawing.Color.Black;
            this.cAdditional.Location = new System.Drawing.Point(46, 114);
            this.cAdditional.Name = "cAdditional";
            this.cAdditional.Size = new System.Drawing.Size(98, 17);
            this.cAdditional.TabIndex = 12;
            this.cAdditional.Text = "Everything else";
            this.cAdditional.UseVisualStyleBackColor = false;
            this.cAdditional.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cGossip
            // 
            this.cGossip.AutoSize = true;
            this.cGossip.BackColor = System.Drawing.Color.Transparent;
            this.cGossip.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cGossip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cGossip.ForeColor = System.Drawing.Color.Black;
            this.cGossip.Location = new System.Drawing.Point(196, 45);
            this.cGossip.Name = "cGossip";
            this.cGossip.Size = new System.Drawing.Size(112, 17);
            this.cGossip.TabIndex = 11;
            this.cGossip.Text = "Gossip stone hints";
            this.cGossip.UseVisualStyleBackColor = false;
            this.cGossip.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cSoS
            // 
            this.cSoS.AutoSize = true;
            this.cSoS.BackColor = System.Drawing.Color.Transparent;
            this.cSoS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cSoS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSoS.ForeColor = System.Drawing.Color.Black;
            this.cSoS.Location = new System.Drawing.Point(165, 22);
            this.cSoS.Name = "cSoS";
            this.cSoS.Size = new System.Drawing.Size(143, 17);
            this.cSoS.TabIndex = 10;
            this.cSoS.Text = "Exclude Song of Soaring";
            this.cSoS.UseVisualStyleBackColor = false;
            this.cSoS.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cMixSongs
            // 
            this.cMixSongs.AutoSize = true;
            this.cMixSongs.BackColor = System.Drawing.Color.Transparent;
            this.cMixSongs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cMixSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cMixSongs.ForeColor = System.Drawing.Color.Black;
            this.cMixSongs.Location = new System.Drawing.Point(22, 22);
            this.cMixSongs.Name = "cMixSongs";
            this.cMixSongs.Size = new System.Drawing.Size(122, 17);
            this.cMixSongs.TabIndex = 3;
            this.cMixSongs.Text = "Mix songs with items";
            this.cMixSongs.UseVisualStyleBackColor = false;
            this.cMixSongs.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cEnemy
            // 
            this.cEnemy.AutoSize = true;
            this.cEnemy.BackColor = System.Drawing.Color.Transparent;
            this.cEnemy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cEnemy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cEnemy.ForeColor = System.Drawing.Color.Black;
            this.cEnemy.Location = new System.Drawing.Point(212, 91);
            this.cEnemy.Name = "cEnemy";
            this.cEnemy.Size = new System.Drawing.Size(96, 17);
            this.cEnemy.TabIndex = 9;
            this.cEnemy.Text = "Enemies (beta)";
            this.cEnemy.UseVisualStyleBackColor = false;
            this.cEnemy.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cDChests
            // 
            this.cDChests.AutoSize = true;
            this.cDChests.BackColor = System.Drawing.Color.Transparent;
            this.cDChests.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cDChests.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cDChests.ForeColor = System.Drawing.Color.Black;
            this.cDChests.Location = new System.Drawing.Point(47, 45);
            this.cDChests.Name = "cDChests";
            this.cDChests.Size = new System.Drawing.Size(97, 17);
            this.cDChests.TabIndex = 4;
            this.cDChests.Text = "Dungeon items";
            this.cDChests.UseVisualStyleBackColor = false;
            this.cDChests.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cBGM
            // 
            this.cBGM.AutoSize = true;
            this.cBGM.BackColor = System.Drawing.Color.Transparent;
            this.cBGM.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cBGM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBGM.ForeColor = System.Drawing.Color.Black;
            this.cBGM.Location = new System.Drawing.Point(258, 114);
            this.cBGM.Name = "cBGM";
            this.cBGM.Size = new System.Drawing.Size(50, 17);
            this.cBGM.TabIndex = 8;
            this.cBGM.Text = "BGM";
            this.cBGM.UseVisualStyleBackColor = false;
            this.cBGM.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cBottled
            // 
            this.cBottled.AutoSize = true;
            this.cBottled.BackColor = System.Drawing.Color.Transparent;
            this.cBottled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cBottled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cBottled.ForeColor = System.Drawing.Color.Black;
            this.cBottled.Location = new System.Drawing.Point(11, 91);
            this.cBottled.Name = "cBottled";
            this.cBottled.Size = new System.Drawing.Size(133, 17);
            this.cBottled.TabIndex = 5;
            this.cBottled.Text = "Caught bottle contents";
            this.cBottled.UseVisualStyleBackColor = false;
            this.cBottled.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cDEnt
            // 
            this.cDEnt.AutoSize = true;
            this.cDEnt.BackColor = System.Drawing.Color.Transparent;
            this.cDEnt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cDEnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cDEnt.ForeColor = System.Drawing.Color.Black;
            this.cDEnt.Location = new System.Drawing.Point(188, 68);
            this.cDEnt.Name = "cDEnt";
            this.cDEnt.Size = new System.Drawing.Size(120, 17);
            this.cDEnt.TabIndex = 7;
            this.cDEnt.Text = "Dungeon entrances";
            this.cDEnt.UseVisualStyleBackColor = false;
            this.cDEnt.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cShop
            // 
            this.cShop.AutoSize = true;
            this.cShop.BackColor = System.Drawing.Color.Transparent;
            this.cShop.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cShop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cShop.ForeColor = System.Drawing.Color.Black;
            this.cShop.Location = new System.Drawing.Point(66, 68);
            this.cShop.Name = "cShop";
            this.cShop.Size = new System.Drawing.Size(78, 17);
            this.cShop.TabIndex = 6;
            this.cShop.Text = "Shop items";
            this.cShop.UseVisualStyleBackColor = false;
            this.cShop.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // tabGimmick
            // 
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
            this.tabGimmick.Size = new System.Drawing.Size(319, 142);
            this.tabGimmick.TabIndex = 2;
            this.tabGimmick.Text = "Gimmicks";
            this.tabGimmick.UseVisualStyleBackColor = true;
            // 
            // lFloors
            // 
            this.lFloors.AutoSize = true;
            this.lFloors.Location = new System.Drawing.Point(83, 106);
            this.lFloors.Name = "lFloors";
            this.lFloors.Size = new System.Drawing.Size(61, 13);
            this.lFloors.TabIndex = 1;
            this.lFloors.Text = "Floor types:";
            // 
            // lGravity
            // 
            this.lGravity.AutoSize = true;
            this.lGravity.Location = new System.Drawing.Point(84, 79);
            this.lGravity.Name = "lGravity";
            this.lGravity.Size = new System.Drawing.Size(60, 13);
            this.lGravity.TabIndex = 1;
            this.lGravity.Text = "Movement:";
            // 
            // lDType
            // 
            this.lDType.AutoSize = true;
            this.lDType.Location = new System.Drawing.Point(59, 52);
            this.lDType.Name = "lDType";
            this.lDType.Size = new System.Drawing.Size(85, 13);
            this.lDType.TabIndex = 1;
            this.lDType.Text = "Damage effects:";
            // 
            // lDMult
            // 
            this.lDMult.AutoSize = true;
            this.lDMult.Location = new System.Drawing.Point(65, 25);
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
            this.cFloors.Location = new System.Drawing.Point(150, 103);
            this.cFloors.Name = "cFloors";
            this.cFloors.Size = new System.Drawing.Size(158, 21);
            this.cFloors.TabIndex = 0;
            this.cFloors.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
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
            this.cDType.Location = new System.Drawing.Point(150, 49);
            this.cDType.Name = "cDType";
            this.cDType.Size = new System.Drawing.Size(158, 21);
            this.cDType.TabIndex = 0;
            this.cDType.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
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
            this.cDMult.Location = new System.Drawing.Point(150, 22);
            this.cDMult.Name = "cDMult";
            this.cDMult.Size = new System.Drawing.Size(158, 21);
            this.cDMult.TabIndex = 0;
            this.cDMult.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
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
            this.cGravity.Location = new System.Drawing.Point(150, 76);
            this.cGravity.Name = "cGravity";
            this.cGravity.Size = new System.Drawing.Size(158, 21);
            this.cGravity.TabIndex = 0;
            this.cGravity.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
            // 
            // tabComfort
            // 
            this.tabComfort.Controls.Add(this.cTatl);
            this.tabComfort.Controls.Add(this.cCutsc);
            this.tabComfort.Controls.Add(this.lTatl);
            this.tabComfort.Controls.Add(this.cQText);
            this.tabComfort.Controls.Add(this.cLink);
            this.tabComfort.Controls.Add(this.lTunic);
            this.tabComfort.Controls.Add(this.lLink);
            this.tabComfort.Controls.Add(this.bTunic);
            this.tabComfort.Location = new System.Drawing.Point(4, 25);
            this.tabComfort.Name = "tabComfort";
            this.tabComfort.Padding = new System.Windows.Forms.Padding(3);
            this.tabComfort.Size = new System.Drawing.Size(319, 142);
            this.tabComfort.TabIndex = 1;
            this.tabComfort.Text = "Comfort/Cosmetics";
            this.tabComfort.UseVisualStyleBackColor = true;
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
            this.cTatl.Location = new System.Drawing.Point(150, 108);
            this.cTatl.Name = "cTatl";
            this.cTatl.Size = new System.Drawing.Size(158, 21);
            this.cTatl.TabIndex = 12;
            this.cTatl.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
            // 
            // cCutsc
            // 
            this.cCutsc.AutoSize = true;
            this.cCutsc.BackColor = System.Drawing.Color.Transparent;
            this.cCutsc.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cCutsc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cCutsc.ForeColor = System.Drawing.Color.Black;
            this.cCutsc.Location = new System.Drawing.Point(29, 22);
            this.cCutsc.Name = "cCutsc";
            this.cCutsc.Size = new System.Drawing.Size(115, 17);
            this.cCutsc.TabIndex = 5;
            this.cCutsc.Text = "Shorten cutscenes";
            this.cCutsc.UseVisualStyleBackColor = false;
            this.cCutsc.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // lTatl
            // 
            this.lTatl.AutoSize = true;
            this.lTatl.BackColor = System.Drawing.Color.Transparent;
            this.lTatl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTatl.ForeColor = System.Drawing.Color.Black;
            this.lTatl.Location = new System.Drawing.Point(44, 111);
            this.lTatl.Name = "lTatl";
            this.lTatl.Size = new System.Drawing.Size(100, 13);
            this.lTatl.TabIndex = 11;
            this.lTatl.Text = "Tatl colour scheme:";
            // 
            // cQText
            // 
            this.cQText.AutoSize = true;
            this.cQText.BackColor = System.Drawing.Color.Transparent;
            this.cQText.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cQText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cQText.ForeColor = System.Drawing.Color.Black;
            this.cQText.Location = new System.Drawing.Point(234, 22);
            this.cQText.Name = "cQText";
            this.cQText.Size = new System.Drawing.Size(74, 17);
            this.cQText.TabIndex = 6;
            this.cQText.Text = "Quick text";
            this.cQText.UseVisualStyleBackColor = false;
            this.cQText.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // cLink
            // 
            this.cLink.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cLink.FormattingEnabled = true;
            this.cLink.Items.AddRange(new object[] {
            "Link (MM)",
            "Link (OoT)",
            "Adult Link (changes physics)",
            "Kafei"});
            this.cLink.Location = new System.Drawing.Point(150, 81);
            this.cLink.Name = "cLink";
            this.cLink.Size = new System.Drawing.Size(158, 21);
            this.cLink.TabIndex = 10;
            this.cLink.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
            // 
            // lTunic
            // 
            this.lTunic.AutoSize = true;
            this.lTunic.BackColor = System.Drawing.Color.Transparent;
            this.lTunic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTunic.ForeColor = System.Drawing.Color.Black;
            this.lTunic.Location = new System.Drawing.Point(75, 57);
            this.lTunic.Name = "lTunic";
            this.lTunic.Size = new System.Drawing.Size(69, 13);
            this.lTunic.TabIndex = 7;
            this.lTunic.Text = "Tunic colour:";
            // 
            // lLink
            // 
            this.lLink.AutoSize = true;
            this.lLink.BackColor = System.Drawing.Color.Transparent;
            this.lLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLink.ForeColor = System.Drawing.Color.Black;
            this.lLink.Location = new System.Drawing.Point(100, 84);
            this.lLink.Name = "lLink";
            this.lLink.Size = new System.Drawing.Size(44, 13);
            this.lLink.TabIndex = 9;
            this.lLink.Text = "Play as:";
            // 
            // bTunic
            // 
            this.bTunic.BackColor = System.Drawing.Color.White;
            this.bTunic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bTunic.Location = new System.Drawing.Point(150, 52);
            this.bTunic.Name = "bTunic";
            this.bTunic.Size = new System.Drawing.Size(158, 23);
            this.bTunic.TabIndex = 8;
            this.bTunic.UseVisualStyleBackColor = false;
            this.bTunic.Click += new System.EventHandler(this.bTunic_Click);
            // 
            // cDummy
            // 
            this.cDummy.AutoSize = true;
            this.cDummy.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cDummy.Enabled = false;
            this.cDummy.Location = new System.Drawing.Point(74, 162);
            this.cDummy.Name = "cDummy";
            this.cDummy.Size = new System.Drawing.Size(80, 17);
            this.cDummy.TabIndex = 9;
            this.cDummy.Text = "checkBox1";
            this.cDummy.UseVisualStyleBackColor = true;
            this.cDummy.Visible = false;
            // 
            // cSpoiler
            // 
            this.cSpoiler.AutoSize = true;
            this.cSpoiler.BackColor = System.Drawing.Color.Transparent;
            this.cSpoiler.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cSpoiler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSpoiler.ForeColor = System.Drawing.Color.Black;
            this.cSpoiler.Location = new System.Drawing.Point(210, 162);
            this.cSpoiler.Name = "cSpoiler";
            this.cSpoiler.Size = new System.Drawing.Size(108, 17);
            this.cSpoiler.TabIndex = 8;
            this.cSpoiler.Text = "Output spoiler log";
            this.cSpoiler.UseVisualStyleBackColor = false;
            this.cSpoiler.CheckedChanged += new System.EventHandler(this.cSpoiler_CheckedChanged);
            // 
            // lSString
            // 
            this.lSString.AutoSize = true;
            this.lSString.BackColor = System.Drawing.Color.Transparent;
            this.lSString.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSString.ForeColor = System.Drawing.Color.Black;
            this.lSString.Location = new System.Drawing.Point(175, 120);
            this.lSString.Name = "lSString";
            this.lSString.Size = new System.Drawing.Size(76, 13);
            this.lSString.TabIndex = 7;
            this.lSString.Text = "Settings string:";
            // 
            // tSString
            // 
            this.tSString.Location = new System.Drawing.Point(178, 136);
            this.tSString.Name = "tSString";
            this.tSString.Size = new System.Drawing.Size(140, 20);
            this.tSString.TabIndex = 6;
            this.tSString.Enter += new System.EventHandler(this.tSString_Enter);
            this.tSString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tSString_KeyDown);
            this.tSString.Leave += new System.EventHandler(this.tSString_Leave);
            // 
            // cMode
            // 
            this.cMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMode.FormattingEnabled = true;
            this.cMode.Items.AddRange(new object[] {
            "Casual/Glitchless",
            "Using Glitches",
            "Vanilla Layout",
            "User Logic",
            "No Logic"});
            this.cMode.Location = new System.Drawing.Point(96, 35);
            this.cMode.Name = "cMode";
            this.cMode.Size = new System.Drawing.Size(222, 21);
            this.cMode.TabIndex = 1;
            this.cMode.SelectedIndexChanged += new System.EventHandler(this.cMode_SelectedIndexChanged);
            // 
            // lMode
            // 
            this.lMode.AutoSize = true;
            this.lMode.BackColor = System.Drawing.Color.Transparent;
            this.lMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lMode.ForeColor = System.Drawing.Color.Black;
            this.lMode.Location = new System.Drawing.Point(12, 38);
            this.lMode.Name = "lMode";
            this.lMode.Size = new System.Drawing.Size(78, 13);
            this.lMode.TabIndex = 0;
            this.lMode.Text = "Mode/Ruleset:";
            // 
            // saveROM
            // 
            this.saveROM.DefaultExt = "z64";
            this.saveROM.Filter = "ROM files|*.z64";
            // 
            // cTunic
            // 
            this.cTunic.Color = System.Drawing.Color.ForestGreen;
            // 
            // bRandomise
            // 
            this.bRandomise.Location = new System.Drawing.Point(13, 99);
            this.bRandomise.Name = "bRandomise";
            this.bRandomise.Size = new System.Drawing.Size(339, 41);
            this.bRandomise.TabIndex = 5;
            this.bRandomise.Text = "Randomise";
            this.bRandomise.UseVisualStyleBackColor = true;
            this.bRandomise.Click += new System.EventHandler(this.bRandomise_Click);
            // 
            // saveWad
            // 
            this.saveWad.DefaultExt = "wad";
            this.saveWad.Filter = "VC files|*.wad";
            // 
            // cVC
            // 
            this.cVC.AutoSize = true;
            this.cVC.BackColor = System.Drawing.Color.Transparent;
            this.cVC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cVC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cVC.ForeColor = System.Drawing.Color.Black;
            this.cVC.Location = new System.Drawing.Point(197, 71);
            this.cVC.Name = "cVC";
            this.cVC.Size = new System.Drawing.Size(155, 17);
            this.cVC.TabIndex = 9;
            this.cVC.Text = "Wii Virtual Console channel";
            this.cVC.UseVisualStyleBackColor = false;
            this.cVC.CheckedChanged += new System.EventHandler(this.cVC_CheckedChanged);
            // 
            // cN64
            // 
            this.cN64.AutoSize = true;
            this.cN64.BackColor = System.Drawing.Color.Transparent;
            this.cN64.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cN64.Checked = true;
            this.cN64.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cN64.Enabled = true;
            this.cN64.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cN64.ForeColor = System.Drawing.Color.Black;
            this.cN64.Location = new System.Drawing.Point(117, 71);
            this.cN64.Name = "cN64";
            this.cN64.Size = new System.Drawing.Size(74, 17);
            this.cN64.TabIndex = 10;
            this.cN64.Text = "N64 ROM";
            this.cN64.UseVisualStyleBackColor = false;
            this.cN64.CheckedChanged += cN64_CheckedChanged;
            // 
            // lOutput
            // 
            this.lOutput.AutoSize = true;
            this.lOutput.Location = new System.Drawing.Point(41, 72);
            this.lOutput.Name = "lOutput";
            this.lOutput.Size = new System.Drawing.Size(70, 13);
            this.lOutput.TabIndex = 11;
            this.lOutput.Text = "Output types:";
            // 
            // mMenu
            // 
            this.mMenu.BackColor = System.Drawing.SystemColors.Control;
            this.mMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFile,
            this.mTools,
            this.mCustomise,
            this.mHelp});
            this.mMenu.Location = new System.Drawing.Point(0, 0);
            this.mMenu.Name = "mMenu";
            this.mMenu.Size = new System.Drawing.Size(365, 24);
            this.mMenu.TabIndex = 12;
            this.mMenu.Text = "mMenu";
            // 
            // mFile
            // 
            this.mFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mExit});
            this.mFile.Name = "mFile";
            this.mFile.Size = new System.Drawing.Size(37, 20);
            this.mFile.Text = "File";
            // 
            // mExit
            // 
            this.mExit.Name = "mExit";
            this.mExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mExit.Size = new System.Drawing.Size(152, 22);
            this.mExit.Text = "Exit";
            this.mExit.Click += new System.EventHandler(this.mExit_Click);
            // 
            // mTools
            // 
            this.mTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mByteswap});
            this.mTools.Name = "mTools";
            this.mTools.Size = new System.Drawing.Size(48, 20);
            this.mTools.Text = "Tools";
            // 
            // mByteswap
            // 
            this.mByteswap.Name = "mByteswap";
            this.mByteswap.Size = new System.Drawing.Size(223, 22);
            this.mByteswap.Text = "Byteswap (.n64, .v64 -> .z64)";
            this.mByteswap.Click += new System.EventHandler(this.mByteswap_Click);
            // 
            // mCustomise
            // 
            this.mCustomise.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mLogicEdit,
            this.mItemIncl});
            this.mCustomise.Name = "mCustomise";
            this.mCustomise.Size = new System.Drawing.Size(75, 20);
            this.mCustomise.Text = "Customise";
            // 
            // mLogicEdit
            // 
            this.mLogicEdit.Name = "mLogicEdit";
            this.mLogicEdit.Size = new System.Drawing.Size(152, 22);
            this.mLogicEdit.Text = "Logic editor";
            this.mLogicEdit.Click += new System.EventHandler(this.mLogicEdit_Click);
            // 
            // mItemIncl
            // 
            this.mItemIncl.Name = "mItemIncl";
            this.mItemIncl.Size = new System.Drawing.Size(152, 22);
            this.mItemIncl.Text = "Item list editor";
            this.mItemIncl.Click += new System.EventHandler(this.mItemIncl_Click);
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
            this.mManual.Size = new System.Drawing.Size(152, 22);
            this.mManual.Text = "Manual";
            this.mManual.Click += new System.EventHandler(this.mManual_Click);
            // 
            // mSep1
            // 
            this.mSep1.Name = "mSep1";
            this.mSep1.Size = new System.Drawing.Size(149, 6);
            // 
            // mAbout
            // 
            this.mAbout.Name = "mAbout";
            this.mAbout.Size = new System.Drawing.Size(152, 22);
            this.mAbout.Text = "About";
            this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // openBROM
            // 
            this.openBROM.Filter = "ROM files|*.z64;*.v64;*.n64";
            // 
            // mmrMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(365, 547);
            this.Controls.Add(this.lOutput);
            this.Controls.Add(this.cN64);
            this.Controls.Add(this.cVC);
            this.Controls.Add(this.bRandomise);
            this.Controls.Add(this.gSettings);
            this.Controls.Add(this.tROMName);
            this.Controls.Add(this.bopen);
            this.Controls.Add(this.mMenu);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mMenu;
            this.MaximizeBox = false;
            this.Name = "mmrMain";
            this.Text = "Majora\'s Mask Randomiser";
            this.Load += new System.EventHandler(this.mmrMain_Load);
            this.gSettings.ResumeLayout(false);
            this.gSettings.PerformLayout();
            this.tSettings.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            this.tabGimmick.ResumeLayout(false);
            this.tabGimmick.PerformLayout();
            this.tabComfort.ResumeLayout(false);
            this.tabComfort.PerformLayout();
            this.mMenu.ResumeLayout(false);
            this.mMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bopen;
        private System.Windows.Forms.OpenFileDialog openROM;
        private System.Windows.Forms.TextBox tROMName;
        private System.Windows.Forms.TextBox tSeed;
        private System.Windows.Forms.Label lSeed;
        private System.Windows.Forms.GroupBox gSettings;
        private System.Windows.Forms.ComboBox cMode;
        private System.Windows.Forms.Label lMode;
        private System.Windows.Forms.SaveFileDialog saveROM;
        private System.Windows.Forms.CheckBox cSpoiler;
        private System.Windows.Forms.Label lSString;
        private System.Windows.Forms.TextBox tSString;
        private System.Windows.Forms.ComboBox cTatl;
        private System.Windows.Forms.Label lTatl;
        private System.Windows.Forms.ComboBox cLink;
        private System.Windows.Forms.Label lLink;
        private System.Windows.Forms.Button bTunic;
        private System.Windows.Forms.Label lTunic;
        private System.Windows.Forms.CheckBox cQText;
        private System.Windows.Forms.CheckBox cCutsc;
        private System.Windows.Forms.CheckBox cEnemy;
        private System.Windows.Forms.CheckBox cBGM;
        private System.Windows.Forms.CheckBox cDEnt;
        private System.Windows.Forms.CheckBox cShop;
        private System.Windows.Forms.CheckBox cBottled;
        private System.Windows.Forms.CheckBox cDChests;
        private System.Windows.Forms.CheckBox cMixSongs;
        private System.Windows.Forms.ColorDialog cTunic;
        private System.Windows.Forms.Button bRandomise;
        private System.Windows.Forms.CheckBox cDummy;
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
        private System.Windows.Forms.CheckBox cGossip;
        private System.Windows.Forms.SaveFileDialog saveWad;
        private System.Windows.Forms.CheckBox cVC;
        private System.Windows.Forms.CheckBox cN64;
        private System.Windows.Forms.Label lOutput;
        private System.Windows.Forms.MenuStrip mMenu;
        private System.Windows.Forms.ToolStripMenuItem mFile;
        private System.Windows.Forms.ToolStripMenuItem mExit;
        private System.Windows.Forms.ToolStripMenuItem mTools;
        private System.Windows.Forms.ToolStripMenuItem mByteswap;
        private System.Windows.Forms.ToolStripMenuItem mHelp;
        private System.Windows.Forms.ToolStripMenuItem mManual;
        private System.Windows.Forms.ToolStripMenuItem mAbout;
        private System.Windows.Forms.ToolStripSeparator mSep1;
        private System.Windows.Forms.OpenFileDialog openBROM;
        private System.Windows.Forms.CheckBox cAdditional;
        private System.Windows.Forms.ToolStripMenuItem mCustomise;
        private System.Windows.Forms.ToolStripMenuItem mLogicEdit;
        private System.Windows.Forms.ToolStripMenuItem mItemIncl;
        private System.Windows.Forms.Label lHrule;
        private System.Windows.Forms.CheckBox cUserItems;
        private System.Windows.Forms.OpenFileDialog openLogic;
    }
}

