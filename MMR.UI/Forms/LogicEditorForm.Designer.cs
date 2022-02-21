namespace MMR.UI.Forms
{
    partial class LogicEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogicEditorForm));
            this.nItem = new System.Windows.Forms.NumericUpDown();
            this.lINo = new System.Windows.Forms.Label();
            this.lIName = new System.Windows.Forms.Label();
            this.tMain = new System.Windows.Forms.TableLayoutPanel();
            this.lTime = new System.Windows.Forms.Label();
            this.lCond = new System.Windows.Forms.Label();
            this.lReq = new System.Windows.Forms.Label();
            this.tTimes = new System.Windows.Forms.TableLayoutPanel();
            this.lNote = new System.Windows.Forms.Label();
            this.cSNight3 = new System.Windows.Forms.CheckBox();
            this.cSDay3 = new System.Windows.Forms.CheckBox();
            this.cSNight2 = new System.Windows.Forms.CheckBox();
            this.cSDay2 = new System.Windows.Forms.CheckBox();
            this.cSNight1 = new System.Windows.Forms.CheckBox();
            this.cSDay1 = new System.Windows.Forms.CheckBox();
            this.lTSetup = new System.Windows.Forms.Label();
            this.cANight3 = new System.Windows.Forms.CheckBox();
            this.cADay3 = new System.Windows.Forms.CheckBox();
            this.cANight2 = new System.Windows.Forms.CheckBox();
            this.cADay2 = new System.Windows.Forms.CheckBox();
            this.cANight1 = new System.Windows.Forms.CheckBox();
            this.cADay1 = new System.Windows.Forms.CheckBox();
            this.cNNight3 = new System.Windows.Forms.CheckBox();
            this.cNDay3 = new System.Windows.Forms.CheckBox();
            this.cNNight2 = new System.Windows.Forms.CheckBox();
            this.cNDay2 = new System.Windows.Forms.CheckBox();
            this.cNNight1 = new System.Windows.Forms.CheckBox();
            this.lTNeed = new System.Windows.Forms.Label();
            this.lTAvail = new System.Windows.Forms.Label();
            this.cNDay1 = new System.Windows.Forms.CheckBox();
            this.tReq = new System.Windows.Forms.TableLayoutPanel();
            this.bReqAdd = new System.Windows.Forms.Button();
            this.bReqClear = new System.Windows.Forms.Button();
            this.lRequired = new System.Windows.Forms.ListBox();
            this.tCond = new System.Windows.Forms.TableLayoutPanel();
            this.bConClone = new System.Windows.Forms.Button();
            this.bConAdd = new System.Windows.Forms.Button();
            this.bConClear = new System.Windows.Forms.Button();
            this.lConditional = new System.Windows.Forms.ListBox();
            this.bConEdit = new System.Windows.Forms.Button();
            this.openLogic = new System.Windows.Forms.OpenFileDialog();
            this.saveLogic = new System.Windows.Forms.SaveFileDialog();
            this.mMenu = new System.Windows.Forms.MenuStrip();
            this.mFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.casualToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.glitchedToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_new_item = new System.Windows.Forms.Button();
            this.button_goto = new System.Windows.Forms.Button();
            this.bDeleteItem = new System.Windows.Forms.Button();
            this.bRenameItem = new System.Windows.Forms.Button();
            this.cTrick = new System.Windows.Forms.CheckBox();
            this.tTrickDescription = new System.Windows.Forms.TextBox();
            this.bCopy = new System.Windows.Forms.Button();
            this.tTrickCategory = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nItem)).BeginInit();
            this.tMain.SuspendLayout();
            this.tTimes.SuspendLayout();
            this.tReq.SuspendLayout();
            this.tCond.SuspendLayout();
            this.mMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // nItem
            // 
            this.nItem.Location = new System.Drawing.Point(132, 62);
            this.nItem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nItem.Name = "nItem";
            this.nItem.Size = new System.Drawing.Size(140, 23);
            this.nItem.TabIndex = 0;
            this.nItem.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nItem.ValueChanged += new System.EventHandler(this.nItem_ValueChanged);
            // 
            // lINo
            // 
            this.lINo.AutoSize = true;
            this.lINo.Location = new System.Drawing.Point(46, 65);
            this.lINo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lINo.Name = "lINo";
            this.lINo.Size = new System.Drawing.Size(79, 15);
            this.lINo.TabIndex = 1;
            this.lINo.Text = "Item number:";
            // 
            // lIName
            // 
            this.lIName.AutoSize = true;
            this.lIName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lIName.Location = new System.Drawing.Point(128, 44);
            this.lIName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lIName.Name = "lIName";
            this.lIName.Size = new System.Drawing.Size(70, 13);
            this.lIName.TabIndex = 2;
            this.lIName.Text = "Item_Name";
            // 
            // tMain
            // 
            this.tMain.BackColor = System.Drawing.SystemColors.Control;
            this.tMain.ColumnCount = 3;
            this.tMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tMain.Controls.Add(this.lTime, 2, 0);
            this.tMain.Controls.Add(this.lCond, 1, 0);
            this.tMain.Controls.Add(this.lReq, 0, 0);
            this.tMain.Controls.Add(this.tTimes, 2, 1);
            this.tMain.Controls.Add(this.tReq, 0, 1);
            this.tMain.Controls.Add(this.tCond, 1, 1);
            this.tMain.Location = new System.Drawing.Point(14, 126);
            this.tMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tMain.Name = "tMain";
            this.tMain.RowCount = 2;
            this.tMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tMain.Size = new System.Drawing.Size(796, 482);
            this.tMain.TabIndex = 3;
            // 
            // lTime
            // 
            this.lTime.AutoSize = true;
            this.lTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lTime.Location = new System.Drawing.Point(534, 0);
            this.lTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lTime.Name = "lTime";
            this.lTime.Size = new System.Drawing.Size(258, 32);
            this.lTime.TabIndex = 2;
            this.lTime.Text = "Time dependence";
            this.lTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lCond
            // 
            this.lCond.AutoSize = true;
            this.lCond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lCond.Location = new System.Drawing.Point(269, 0);
            this.lCond.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lCond.Name = "lCond";
            this.lCond.Size = new System.Drawing.Size(257, 32);
            this.lCond.TabIndex = 1;
            this.lCond.Text = "Conditional items";
            this.lCond.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lReq
            // 
            this.lReq.AutoSize = true;
            this.lReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lReq.Location = new System.Drawing.Point(4, 0);
            this.lReq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lReq.Name = "lReq";
            this.lReq.Size = new System.Drawing.Size(257, 32);
            this.lReq.TabIndex = 0;
            this.lReq.Text = "Required items";
            this.lReq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tTimes
            // 
            this.tTimes.ColumnCount = 2;
            this.tTimes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tTimes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tTimes.Controls.Add(this.lNote, 0, 12);
            this.tTimes.Controls.Add(this.cSNight3, 1, 11);
            this.tTimes.Controls.Add(this.cSDay3, 0, 11);
            this.tTimes.Controls.Add(this.cSNight2, 1, 10);
            this.tTimes.Controls.Add(this.cSDay2, 0, 10);
            this.tTimes.Controls.Add(this.cSNight1, 1, 9);
            this.tTimes.Controls.Add(this.cSDay1, 0, 9);
            this.tTimes.Controls.Add(this.lTSetup, 0, 8);
            this.tTimes.Controls.Add(this.cANight3, 1, 7);
            this.tTimes.Controls.Add(this.cADay3, 0, 7);
            this.tTimes.Controls.Add(this.cANight2, 1, 6);
            this.tTimes.Controls.Add(this.cADay2, 0, 6);
            this.tTimes.Controls.Add(this.cANight1, 1, 5);
            this.tTimes.Controls.Add(this.cADay1, 0, 5);
            this.tTimes.Controls.Add(this.cNNight3, 1, 3);
            this.tTimes.Controls.Add(this.cNDay3, 0, 3);
            this.tTimes.Controls.Add(this.cNNight2, 1, 2);
            this.tTimes.Controls.Add(this.cNDay2, 0, 2);
            this.tTimes.Controls.Add(this.cNNight1, 1, 1);
            this.tTimes.Controls.Add(this.lTNeed, 0, 0);
            this.tTimes.Controls.Add(this.lTAvail, 0, 4);
            this.tTimes.Controls.Add(this.cNDay1, 0, 1);
            this.tTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tTimes.Location = new System.Drawing.Point(534, 35);
            this.tTimes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tTimes.Name = "tTimes";
            this.tTimes.RowCount = 13;
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.330333F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.330333F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.330333F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.001F));
            this.tTimes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tTimes.Size = new System.Drawing.Size(258, 444);
            this.tTimes.TabIndex = 3;
            // 
            // lNote
            // 
            this.lNote.AutoSize = true;
            this.tTimes.SetColumnSpan(this.lNote, 2);
            this.lNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNote.Location = new System.Drawing.Point(4, 420);
            this.lNote.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lNote.Name = "lNote";
            this.lNote.Size = new System.Drawing.Size(250, 24);
            this.lNote.TabIndex = 22;
            this.lNote.Text = "If there is no time restriction, leave blank.";
            this.lNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cSNight3
            // 
            this.cSNight3.AutoSize = true;
            this.cSNight3.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSNight3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSNight3.Location = new System.Drawing.Point(133, 381);
            this.cSNight3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cSNight3.Name = "cSNight3";
            this.cSNight3.Size = new System.Drawing.Size(121, 36);
            this.cSNight3.TabIndex = 21;
            this.cSNight3.Text = "Night 3";
            this.cSNight3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSNight3.UseVisualStyleBackColor = true;
            this.cSNight3.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cSDay3
            // 
            this.cSDay3.AutoSize = true;
            this.cSDay3.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSDay3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSDay3.Location = new System.Drawing.Point(4, 381);
            this.cSDay3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cSDay3.Name = "cSDay3";
            this.cSDay3.Size = new System.Drawing.Size(121, 36);
            this.cSDay3.TabIndex = 20;
            this.cSDay3.Text = "Day 3";
            this.cSDay3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSDay3.UseVisualStyleBackColor = true;
            this.cSDay3.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cSNight2
            // 
            this.cSNight2.AutoSize = true;
            this.cSNight2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSNight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSNight2.Location = new System.Drawing.Point(133, 339);
            this.cSNight2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cSNight2.Name = "cSNight2";
            this.cSNight2.Size = new System.Drawing.Size(121, 36);
            this.cSNight2.TabIndex = 19;
            this.cSNight2.Text = "Night 2";
            this.cSNight2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSNight2.UseVisualStyleBackColor = true;
            this.cSNight2.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cSDay2
            // 
            this.cSDay2.AutoSize = true;
            this.cSDay2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSDay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSDay2.Location = new System.Drawing.Point(4, 339);
            this.cSDay2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cSDay2.Name = "cSDay2";
            this.cSDay2.Size = new System.Drawing.Size(121, 36);
            this.cSDay2.TabIndex = 18;
            this.cSDay2.Text = "Day 2";
            this.cSDay2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSDay2.UseVisualStyleBackColor = true;
            this.cSDay2.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cSNight1
            // 
            this.cSNight1.AutoSize = true;
            this.cSNight1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSNight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSNight1.Location = new System.Drawing.Point(133, 297);
            this.cSNight1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cSNight1.Name = "cSNight1";
            this.cSNight1.Size = new System.Drawing.Size(121, 36);
            this.cSNight1.TabIndex = 17;
            this.cSNight1.Text = "Night 1";
            this.cSNight1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSNight1.UseVisualStyleBackColor = true;
            this.cSNight1.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cSDay1
            // 
            this.cSDay1.AutoSize = true;
            this.cSDay1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSDay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cSDay1.Location = new System.Drawing.Point(4, 297);
            this.cSDay1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cSDay1.Name = "cSDay1";
            this.cSDay1.Size = new System.Drawing.Size(121, 36);
            this.cSDay1.TabIndex = 16;
            this.cSDay1.Text = "Day 1";
            this.cSDay1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cSDay1.UseVisualStyleBackColor = true;
            this.cSDay1.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // lTSetup
            // 
            this.lTSetup.AutoSize = true;
            this.tTimes.SetColumnSpan(this.lTSetup, 2);
            this.lTSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTSetup.Location = new System.Drawing.Point(4, 280);
            this.lTSetup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lTSetup.Name = "lTSetup";
            this.lTSetup.Size = new System.Drawing.Size(250, 14);
            this.lTSetup.TabIndex = 15;
            this.lTSetup.Text = "Setup on";
            this.lTSetup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cANight3
            // 
            this.cANight3.AutoSize = true;
            this.cANight3.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cANight3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cANight3.Location = new System.Drawing.Point(133, 241);
            this.cANight3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cANight3.Name = "cANight3";
            this.cANight3.Size = new System.Drawing.Size(121, 36);
            this.cANight3.TabIndex = 13;
            this.cANight3.Text = "Night 3";
            this.cANight3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cANight3.UseVisualStyleBackColor = true;
            this.cANight3.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cADay3
            // 
            this.cADay3.AutoSize = true;
            this.cADay3.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cADay3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cADay3.Location = new System.Drawing.Point(4, 241);
            this.cADay3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cADay3.Name = "cADay3";
            this.cADay3.Size = new System.Drawing.Size(121, 36);
            this.cADay3.TabIndex = 12;
            this.cADay3.Text = "Day 3";
            this.cADay3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cADay3.UseVisualStyleBackColor = true;
            this.cADay3.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cANight2
            // 
            this.cANight2.AutoSize = true;
            this.cANight2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cANight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cANight2.Location = new System.Drawing.Point(133, 199);
            this.cANight2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cANight2.Name = "cANight2";
            this.cANight2.Size = new System.Drawing.Size(121, 36);
            this.cANight2.TabIndex = 11;
            this.cANight2.Text = "Night 2";
            this.cANight2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cANight2.UseVisualStyleBackColor = true;
            this.cANight2.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cADay2
            // 
            this.cADay2.AutoSize = true;
            this.cADay2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cADay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cADay2.Location = new System.Drawing.Point(4, 199);
            this.cADay2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cADay2.Name = "cADay2";
            this.cADay2.Size = new System.Drawing.Size(121, 36);
            this.cADay2.TabIndex = 10;
            this.cADay2.Text = "Day 2";
            this.cADay2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cADay2.UseVisualStyleBackColor = true;
            this.cADay2.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cANight1
            // 
            this.cANight1.AutoSize = true;
            this.cANight1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cANight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cANight1.Location = new System.Drawing.Point(133, 157);
            this.cANight1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cANight1.Name = "cANight1";
            this.cANight1.Size = new System.Drawing.Size(121, 36);
            this.cANight1.TabIndex = 9;
            this.cANight1.Text = "Night 1";
            this.cANight1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cANight1.UseVisualStyleBackColor = true;
            this.cANight1.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cADay1
            // 
            this.cADay1.AutoSize = true;
            this.cADay1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cADay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cADay1.Location = new System.Drawing.Point(4, 157);
            this.cADay1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cADay1.Name = "cADay1";
            this.cADay1.Size = new System.Drawing.Size(121, 36);
            this.cADay1.TabIndex = 8;
            this.cADay1.Text = "Day 1";
            this.cADay1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cADay1.UseVisualStyleBackColor = true;
            this.cADay1.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cNNight3
            // 
            this.cNNight3.AutoSize = true;
            this.cNNight3.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNNight3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cNNight3.Location = new System.Drawing.Point(133, 101);
            this.cNNight3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cNNight3.Name = "cNNight3";
            this.cNNight3.Size = new System.Drawing.Size(121, 36);
            this.cNNight3.TabIndex = 7;
            this.cNNight3.Text = "Night 3";
            this.cNNight3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNNight3.UseVisualStyleBackColor = true;
            this.cNNight3.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cNDay3
            // 
            this.cNDay3.AutoSize = true;
            this.cNDay3.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNDay3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cNDay3.Location = new System.Drawing.Point(4, 101);
            this.cNDay3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cNDay3.Name = "cNDay3";
            this.cNDay3.Size = new System.Drawing.Size(121, 36);
            this.cNDay3.TabIndex = 6;
            this.cNDay3.Text = "Day 3";
            this.cNDay3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNDay3.UseVisualStyleBackColor = true;
            this.cNDay3.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cNNight2
            // 
            this.cNNight2.AutoSize = true;
            this.cNNight2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNNight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cNNight2.Location = new System.Drawing.Point(133, 59);
            this.cNNight2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cNNight2.Name = "cNNight2";
            this.cNNight2.Size = new System.Drawing.Size(121, 36);
            this.cNNight2.TabIndex = 5;
            this.cNNight2.Text = "Night 2";
            this.cNNight2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNNight2.UseVisualStyleBackColor = true;
            this.cNNight2.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cNDay2
            // 
            this.cNDay2.AutoSize = true;
            this.cNDay2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNDay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cNDay2.Location = new System.Drawing.Point(4, 59);
            this.cNDay2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cNDay2.Name = "cNDay2";
            this.cNDay2.Size = new System.Drawing.Size(121, 36);
            this.cNDay2.TabIndex = 4;
            this.cNDay2.Text = "Day 2";
            this.cNDay2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNDay2.UseVisualStyleBackColor = true;
            this.cNDay2.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // cNNight1
            // 
            this.cNNight1.AutoSize = true;
            this.cNNight1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNNight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cNNight1.Location = new System.Drawing.Point(133, 17);
            this.cNNight1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cNNight1.Name = "cNNight1";
            this.cNNight1.Size = new System.Drawing.Size(121, 36);
            this.cNNight1.TabIndex = 3;
            this.cNNight1.Text = "Night 1";
            this.cNNight1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNNight1.UseVisualStyleBackColor = true;
            this.cNNight1.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // lTNeed
            // 
            this.lTNeed.AutoSize = true;
            this.tTimes.SetColumnSpan(this.lTNeed, 2);
            this.lTNeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTNeed.Location = new System.Drawing.Point(4, 0);
            this.lTNeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lTNeed.Name = "lTNeed";
            this.lTNeed.Size = new System.Drawing.Size(250, 14);
            this.lTNeed.TabIndex = 0;
            this.lTNeed.Text = "Needed on";
            this.lTNeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lTAvail
            // 
            this.lTAvail.AutoSize = true;
            this.tTimes.SetColumnSpan(this.lTAvail, 2);
            this.lTAvail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTAvail.Location = new System.Drawing.Point(4, 140);
            this.lTAvail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lTAvail.Name = "lTAvail";
            this.lTAvail.Size = new System.Drawing.Size(250, 14);
            this.lTAvail.TabIndex = 1;
            this.lTAvail.Text = "Available on";
            this.lTAvail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cNDay1
            // 
            this.cNDay1.AutoSize = true;
            this.cNDay1.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNDay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cNDay1.Location = new System.Drawing.Point(4, 17);
            this.cNDay1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cNDay1.Name = "cNDay1";
            this.cNDay1.Size = new System.Drawing.Size(121, 36);
            this.cNDay1.TabIndex = 2;
            this.cNDay1.Text = "Day 1";
            this.cNDay1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cNDay1.UseVisualStyleBackColor = true;
            this.cNDay1.CheckedChanged += new System.EventHandler(this.cNDay1_CheckedChanged);
            // 
            // tReq
            // 
            this.tReq.ColumnCount = 2;
            this.tReq.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tReq.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tReq.Controls.Add(this.bReqAdd, 0, 1);
            this.tReq.Controls.Add(this.bReqClear, 1, 1);
            this.tReq.Controls.Add(this.lRequired, 0, 0);
            this.tReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tReq.Location = new System.Drawing.Point(4, 35);
            this.tReq.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tReq.Name = "tReq";
            this.tReq.RowCount = 2;
            this.tReq.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tReq.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tReq.Size = new System.Drawing.Size(257, 444);
            this.tReq.TabIndex = 4;
            // 
            // bReqAdd
            // 
            this.bReqAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bReqAdd.Location = new System.Drawing.Point(4, 405);
            this.bReqAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bReqAdd.Name = "bReqAdd";
            this.bReqAdd.Size = new System.Drawing.Size(120, 36);
            this.bReqAdd.TabIndex = 0;
            this.bReqAdd.Text = "Add";
            this.bReqAdd.UseVisualStyleBackColor = true;
            this.bReqAdd.Click += new System.EventHandler(this.bReqAdd_Click);
            // 
            // bReqClear
            // 
            this.bReqClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bReqClear.Location = new System.Drawing.Point(132, 405);
            this.bReqClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bReqClear.Name = "bReqClear";
            this.bReqClear.Size = new System.Drawing.Size(121, 36);
            this.bReqClear.TabIndex = 1;
            this.bReqClear.Text = "Clear selected";
            this.bReqClear.UseVisualStyleBackColor = true;
            this.bReqClear.Click += new System.EventHandler(this.bReqClear_Click);
            // 
            // lRequired
            // 
            this.tReq.SetColumnSpan(this.lRequired, 2);
            this.lRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRequired.FormattingEnabled = true;
            this.lRequired.HorizontalScrollbar = true;
            this.lRequired.ItemHeight = 15;
            this.lRequired.Location = new System.Drawing.Point(4, 3);
            this.lRequired.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lRequired.Name = "lRequired";
            this.lRequired.Size = new System.Drawing.Size(249, 396);
            this.lRequired.TabIndex = 2;
            this.lRequired.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lRequired_MouseDoubleClick);
            // 
            // tCond
            // 
            this.tCond.ColumnCount = 2;
            this.tCond.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tCond.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tCond.Controls.Add(this.bConClone, 0, 2);
            this.tCond.Controls.Add(this.bConAdd, 0, 1);
            this.tCond.Controls.Add(this.bConClear, 1, 1);
            this.tCond.Controls.Add(this.lConditional, 0, 0);
            this.tCond.Controls.Add(this.bConEdit, 1, 2);
            this.tCond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tCond.Location = new System.Drawing.Point(269, 35);
            this.tCond.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tCond.Name = "tCond";
            this.tCond.RowCount = 3;
            this.tCond.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tCond.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tCond.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tCond.Size = new System.Drawing.Size(257, 444);
            this.tCond.TabIndex = 5;
            // 
            // bConClone
            // 
            this.bConClone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bConClone.Location = new System.Drawing.Point(4, 405);
            this.bConClone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bConClone.Name = "bConClone";
            this.bConClone.Size = new System.Drawing.Size(120, 36);
            this.bConClone.TabIndex = 4;
            this.bConClone.Text = "Clone";
            this.bConClone.UseVisualStyleBackColor = true;
            this.bConClone.Click += new System.EventHandler(this.bConClone_Click);
            // 
            // bConAdd
            // 
            this.bConAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bConAdd.Location = new System.Drawing.Point(4, 363);
            this.bConAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bConAdd.Name = "bConAdd";
            this.bConAdd.Size = new System.Drawing.Size(120, 36);
            this.bConAdd.TabIndex = 0;
            this.bConAdd.Text = "Add";
            this.bConAdd.UseVisualStyleBackColor = true;
            this.bConAdd.Click += new System.EventHandler(this.bConAdd_Click);
            // 
            // bConClear
            // 
            this.bConClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bConClear.Location = new System.Drawing.Point(132, 363);
            this.bConClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bConClear.Name = "bConClear";
            this.bConClear.Size = new System.Drawing.Size(121, 36);
            this.bConClear.TabIndex = 1;
            this.bConClear.Text = "Remove";
            this.bConClear.UseVisualStyleBackColor = true;
            this.bConClear.Click += new System.EventHandler(this.bConClear_Click);
            // 
            // lConditional
            // 
            this.tCond.SetColumnSpan(this.lConditional, 2);
            this.lConditional.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lConditional.FormattingEnabled = true;
            this.lConditional.HorizontalScrollbar = true;
            this.lConditional.ItemHeight = 15;
            this.lConditional.Location = new System.Drawing.Point(4, 3);
            this.lConditional.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lConditional.Name = "lConditional";
            this.lConditional.Size = new System.Drawing.Size(249, 354);
            this.lConditional.TabIndex = 2;
            this.lConditional.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lConditional_MouseDoubleClick);
            // 
            // bConEdit
            // 
            this.bConEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bConEdit.Location = new System.Drawing.Point(132, 405);
            this.bConEdit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bConEdit.Name = "bConEdit";
            this.bConEdit.Size = new System.Drawing.Size(121, 36);
            this.bConEdit.TabIndex = 3;
            this.bConEdit.Text = "Edit";
            this.bConEdit.UseVisualStyleBackColor = true;
            this.bConEdit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bConEdit_MouseClick);
            // 
            // mMenu
            // 
            this.mMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mFile});
            this.mMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.mMenu.Location = new System.Drawing.Point(0, 0);
            this.mMenu.Name = "mMenu";
            this.mMenu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.mMenu.Size = new System.Drawing.Size(824, 24);
            this.mMenu.TabIndex = 4;
            this.mMenu.Text = "menuStrip1";
            // 
            // mFile
            // 
            this.mFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mNew,
            this.mImport,
            this.mSave,
            this.toolStripSeparator1,
            this.importTemplateToolStripMenuItem});
            this.mFile.Name = "mFile";
            this.mFile.Size = new System.Drawing.Size(37, 20);
            this.mFile.Text = "File";
            // 
            // mNew
            // 
            this.mNew.Name = "mNew";
            this.mNew.Size = new System.Drawing.Size(182, 22);
            this.mNew.Text = "New";
            this.mNew.Click += new System.EventHandler(this.mNew_Click);
            // 
            // mImport
            // 
            this.mImport.Name = "mImport";
            this.mImport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mImport.Size = new System.Drawing.Size(182, 22);
            this.mImport.Text = "Import logic";
            this.mImport.Click += new System.EventHandler(this.mImport_Click);
            // 
            // mSave
            // 
            this.mSave.Name = "mSave";
            this.mSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mSave.Size = new System.Drawing.Size(182, 22);
            this.mSave.Text = "Save logic";
            this.mSave.Click += new System.EventHandler(this.mSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // importTemplateToolStripMenuItem
            // 
            this.importTemplateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.casualToolStripMenuItem1,
            this.glitchedToolStripMenuItem1});
            this.importTemplateToolStripMenuItem.Name = "importTemplateToolStripMenuItem";
            this.importTemplateToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.importTemplateToolStripMenuItem.Text = "Import template";
            // 
            // casualToolStripMenuItem1
            // 
            this.casualToolStripMenuItem1.Name = "casualToolStripMenuItem1";
            this.casualToolStripMenuItem1.Size = new System.Drawing.Size(118, 22);
            this.casualToolStripMenuItem1.Text = "Casual";
            this.casualToolStripMenuItem1.Click += new System.EventHandler(this.casualToolStripMenuItem1_Click);
            // 
            // glitchedToolStripMenuItem1
            // 
            this.glitchedToolStripMenuItem1.Name = "glitchedToolStripMenuItem1";
            this.glitchedToolStripMenuItem1.Size = new System.Drawing.Size(118, 22);
            this.glitchedToolStripMenuItem1.Text = "Glitched";
            this.glitchedToolStripMenuItem1.Click += new System.EventHandler(this.glitchedToolStripMenuItem1_Click);
            // 
            // btn_new_item
            // 
            this.btn_new_item.Location = new System.Drawing.Point(132, 93);
            this.btn_new_item.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_new_item.Name = "btn_new_item";
            this.btn_new_item.Size = new System.Drawing.Size(140, 27);
            this.btn_new_item.TabIndex = 5;
            this.btn_new_item.Text = "New Item";
            this.btn_new_item.UseVisualStyleBackColor = true;
            this.btn_new_item.Click += new System.EventHandler(this.btn_new_item_Click);
            // 
            // button_goto
            // 
            this.button_goto.Location = new System.Drawing.Point(70, 93);
            this.button_goto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_goto.Name = "button_goto";
            this.button_goto.Size = new System.Drawing.Size(55, 27);
            this.button_goto.TabIndex = 6;
            this.button_goto.Text = "Go To";
            this.button_goto.UseVisualStyleBackColor = true;
            this.button_goto.Click += new System.EventHandler(this.button_goto_Click);
            // 
            // bDeleteItem
            // 
            this.bDeleteItem.Location = new System.Drawing.Point(280, 93);
            this.bDeleteItem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bDeleteItem.Name = "bDeleteItem";
            this.bDeleteItem.Size = new System.Drawing.Size(88, 27);
            this.bDeleteItem.TabIndex = 7;
            this.bDeleteItem.Text = "Delete";
            this.bDeleteItem.UseVisualStyleBackColor = true;
            this.bDeleteItem.Visible = false;
            this.bDeleteItem.Click += new System.EventHandler(this.bDeleteItem_Click);
            // 
            // bRenameItem
            // 
            this.bRenameItem.Location = new System.Drawing.Point(376, 93);
            this.bRenameItem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bRenameItem.Name = "bRenameItem";
            this.bRenameItem.Size = new System.Drawing.Size(88, 27);
            this.bRenameItem.TabIndex = 8;
            this.bRenameItem.Text = "Rename";
            this.bRenameItem.UseVisualStyleBackColor = true;
            this.bRenameItem.Visible = false;
            this.bRenameItem.Click += new System.EventHandler(this.bRenameItem_Click);
            // 
            // cTrick
            // 
            this.cTrick.AutoSize = true;
            this.cTrick.Location = new System.Drawing.Point(470, 98);
            this.cTrick.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cTrick.Name = "cTrick";
            this.cTrick.Size = new System.Drawing.Size(50, 19);
            this.cTrick.TabIndex = 9;
            this.cTrick.Text = "Trick";
            this.cTrick.UseVisualStyleBackColor = true;
            this.cTrick.Visible = false;
            this.cTrick.CheckedChanged += new System.EventHandler(this.cTrick_CheckedChanged);
            // 
            // tTrickDescription
            // 
            this.tTrickDescription.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.tTrickDescription.Location = new System.Drawing.Point(536, 96);
            this.tTrickDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tTrickDescription.Name = "tTrickDescription";
            this.tTrickDescription.Size = new System.Drawing.Size(274, 23);
            this.tTrickDescription.TabIndex = 10;
            this.tTrickDescription.Text = "(optional tooltip)";
            this.tTrickDescription.TextChanged += new System.EventHandler(this.tTrickDescription_TextChanged);
            this.tTrickDescription.Enter += new System.EventHandler(this.tTrickDescription_Enter);
            this.tTrickDescription.Leave += new System.EventHandler(this.tTrickDescription_Leave);
            // 
            // bCopy
            // 
            this.bCopy.Location = new System.Drawing.Point(14, 93);
            this.bCopy.Name = "bCopy";
            this.bCopy.Size = new System.Drawing.Size(49, 27);
            this.bCopy.TabIndex = 11;
            this.bCopy.Text = "Copy";
            this.bCopy.UseVisualStyleBackColor = true;
            this.bCopy.Click += new System.EventHandler(this.bCopy_Click);
            // 
            // tTrickCategory
            // 
            this.tTrickCategory.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.tTrickCategory.Location = new System.Drawing.Point(536, 67);
            this.tTrickCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tTrickCategory.Name = "tTrickCategory";
            this.tTrickCategory.Size = new System.Drawing.Size(274, 23);
            this.tTrickCategory.TabIndex = 12;
            this.tTrickCategory.Text = "(optional category)";
            this.tTrickCategory.TextChanged += new System.EventHandler(this.tCategoryDescription_TextChanged);
            this.tTrickCategory.Enter += new System.EventHandler(this.tCategoryDescription_Enter);
            this.tTrickCategory.Leave += new System.EventHandler(this.tCategoryDescription_Leave);
            // 
            // LogicEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 622);
            this.Controls.Add(this.tTrickCategory);
            this.Controls.Add(this.bCopy);
            this.Controls.Add(this.tTrickDescription);
            this.Controls.Add(this.cTrick);
            this.Controls.Add(this.bRenameItem);
            this.Controls.Add(this.bDeleteItem);
            this.Controls.Add(this.button_goto);
            this.Controls.Add(this.btn_new_item);
            this.Controls.Add(this.tMain);
            this.Controls.Add(this.lIName);
            this.Controls.Add(this.lINo);
            this.Controls.Add(this.nItem);
            this.Controls.Add(this.mMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "LogicEditorForm";
            this.Text = "Logic Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fLogicEdit_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nItem)).EndInit();
            this.tMain.ResumeLayout(false);
            this.tMain.PerformLayout();
            this.tTimes.ResumeLayout(false);
            this.tTimes.PerformLayout();
            this.tReq.ResumeLayout(false);
            this.tCond.ResumeLayout(false);
            this.mMenu.ResumeLayout(false);
            this.mMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nItem;
        private System.Windows.Forms.Label lINo;
        private System.Windows.Forms.Label lIName;
        private System.Windows.Forms.TableLayoutPanel tMain;
        private System.Windows.Forms.Label lTime;
        private System.Windows.Forms.Label lCond;
        private System.Windows.Forms.Label lReq;
        private System.Windows.Forms.TableLayoutPanel tTimes;
        private System.Windows.Forms.Label lTNeed;
        private System.Windows.Forms.Label lTAvail;
        private System.Windows.Forms.CheckBox cANight3;
        private System.Windows.Forms.CheckBox cADay3;
        private System.Windows.Forms.CheckBox cANight2;
        private System.Windows.Forms.CheckBox cADay2;
        private System.Windows.Forms.CheckBox cANight1;
        private System.Windows.Forms.CheckBox cADay1;
        private System.Windows.Forms.CheckBox cNNight3;
        private System.Windows.Forms.CheckBox cNDay3;
        private System.Windows.Forms.CheckBox cNNight2;
        private System.Windows.Forms.CheckBox cNDay2;
        private System.Windows.Forms.CheckBox cNNight1;
        private System.Windows.Forms.CheckBox cNDay1;
        private System.Windows.Forms.OpenFileDialog openLogic;
        private System.Windows.Forms.SaveFileDialog saveLogic;
        private System.Windows.Forms.MenuStrip mMenu;
        private System.Windows.Forms.ToolStripMenuItem mFile;
        private System.Windows.Forms.ToolStripMenuItem mImport;
        private System.Windows.Forms.ToolStripMenuItem mSave;
        private System.Windows.Forms.TableLayoutPanel tReq;
        private System.Windows.Forms.Button bReqAdd;
        private System.Windows.Forms.Button bReqClear;
        private System.Windows.Forms.ListBox lRequired;
        private System.Windows.Forms.TableLayoutPanel tCond;
        private System.Windows.Forms.ListBox lConditional;
        private System.Windows.Forms.ToolStripMenuItem mNew;
        private System.Windows.Forms.Button btn_new_item;
        private System.Windows.Forms.Button button_goto;
        private System.Windows.Forms.Button bConAdd;
        private System.Windows.Forms.Button bConClear;
        private System.Windows.Forms.Button bConEdit;
        private System.Windows.Forms.ToolStripMenuItem importTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem casualToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem glitchedToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Button bDeleteItem;
        private System.Windows.Forms.Button bRenameItem;
        private System.Windows.Forms.CheckBox cTrick;
        private System.Windows.Forms.TextBox tTrickDescription;
        private System.Windows.Forms.Button bConClone;
        private System.Windows.Forms.Button bCopy;
        private System.Windows.Forms.Label lNote;
        private System.Windows.Forms.CheckBox cSNight3;
        private System.Windows.Forms.CheckBox cSDay3;
        private System.Windows.Forms.CheckBox cSNight2;
        private System.Windows.Forms.CheckBox cSDay2;
        private System.Windows.Forms.CheckBox cSNight1;
        private System.Windows.Forms.CheckBox cSDay1;
        private System.Windows.Forms.Label lTSetup;
        private System.Windows.Forms.TextBox tTrickCategory;
    }
}
