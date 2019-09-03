namespace MMRando.Forms
{
    partial class ManualForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualForm));
            this.tManual = new System.Windows.Forms.TabControl();
            this.tMain = new System.Windows.Forms.TabPage();
            this.lUsage = new System.Windows.Forms.Label();
            this.tRules = new System.Windows.Forms.TabPage();
            this.lRules = new System.Windows.Forms.Label();
            this.tBaseChanges = new System.Windows.Forms.TabPage();
            this.lBaseGame = new System.Windows.Forms.Label();
            this.tLogic = new System.Windows.Forms.TabPage();
            this.lLogic = new System.Windows.Forms.Label();
            this.tItem = new System.Windows.Forms.TabPage();
            this.lItem = new System.Windows.Forms.Label();
            this.tShuffles = new System.Windows.Forms.TabPage();
            this.lShuffle = new System.Windows.Forms.Label();
            this.tGimmicks = new System.Windows.Forms.TabPage();
            this.lGimmick = new System.Windows.Forms.Label();
            this.tCosmetics = new System.Windows.Forms.TabPage();
            this.lCosmetic = new System.Windows.Forms.Label();
            this.tHow = new System.Windows.Forms.TabPage();
            this.lHow = new System.Windows.Forms.Label();
            this.tManual.SuspendLayout();
            this.tMain.SuspendLayout();
            this.tRules.SuspendLayout();
            this.tBaseChanges.SuspendLayout();
            this.tLogic.SuspendLayout();
            this.tItem.SuspendLayout();
            this.tShuffles.SuspendLayout();
            this.tGimmicks.SuspendLayout();
            this.tCosmetics.SuspendLayout();
            this.tHow.SuspendLayout();
            this.SuspendLayout();
            // 
            // tManual
            // 
            this.tManual.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tManual.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tManual.Controls.Add(this.tMain);
            this.tManual.Controls.Add(this.tRules);
            this.tManual.Controls.Add(this.tBaseChanges);
            this.tManual.Controls.Add(this.tLogic);
            this.tManual.Controls.Add(this.tItem);
            this.tManual.Controls.Add(this.tShuffles);
            this.tManual.Controls.Add(this.tGimmicks);
            this.tManual.Controls.Add(this.tCosmetics);
            this.tManual.Controls.Add(this.tHow);
            this.tManual.Location = new System.Drawing.Point(12, 12);
            this.tManual.Name = "tManual";
            this.tManual.SelectedIndex = 0;
            this.tManual.Size = new System.Drawing.Size(408, 471);
            this.tManual.TabIndex = 0;
            // 
            // tMain
            // 
            this.tMain.AutoScroll = true;
            this.tMain.Controls.Add(this.lUsage);
            this.tMain.Location = new System.Drawing.Point(4, 25);
            this.tMain.Name = "tMain";
            this.tMain.Padding = new System.Windows.Forms.Padding(3);
            this.tMain.Size = new System.Drawing.Size(400, 442);
            this.tMain.TabIndex = 0;
            this.tMain.Text = "Usage";
            this.tMain.UseVisualStyleBackColor = true;
            // 
            // lUsage
            // 
            this.lUsage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lUsage.Location = new System.Drawing.Point(6, 3);
            this.lUsage.Name = "lUsage";
            this.lUsage.Size = new System.Drawing.Size(388, 268);
            this.lUsage.TabIndex = 1;
            this.lUsage.Text = resources.GetString("lUsage.Text");
            // 
            // tRules
            // 
            this.tRules.AutoScroll = true;
            this.tRules.Controls.Add(this.lRules);
            this.tRules.Location = new System.Drawing.Point(4, 25);
            this.tRules.Name = "tRules";
            this.tRules.Padding = new System.Windows.Forms.Padding(3);
            this.tRules.Size = new System.Drawing.Size(400, 442);
            this.tRules.TabIndex = 1;
            this.tRules.Text = "Rulesets";
            this.tRules.UseVisualStyleBackColor = true;
            // 
            // lRules
            // 
            this.lRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lRules.Location = new System.Drawing.Point(6, 3);
            this.lRules.Name = "lRules";
            this.lRules.Size = new System.Drawing.Size(388, 357);
            this.lRules.TabIndex = 2;
            this.lRules.Text = resources.GetString("lRules.Text");
            // 
            // tBaseChanges
            // 
            this.tBaseChanges.AutoScroll = true;
            this.tBaseChanges.Controls.Add(this.lBaseGame);
            this.tBaseChanges.Location = new System.Drawing.Point(4, 25);
            this.tBaseChanges.Name = "tBaseChanges";
            this.tBaseChanges.Size = new System.Drawing.Size(400, 442);
            this.tBaseChanges.TabIndex = 5;
            this.tBaseChanges.Text = "Game changes";
            this.tBaseChanges.UseVisualStyleBackColor = true;
            // 
            // lBaseGame
            // 
            this.lBaseGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lBaseGame.Location = new System.Drawing.Point(6, 3);
            this.lBaseGame.Name = "lBaseGame";
            this.lBaseGame.Size = new System.Drawing.Size(391, 346);
            this.lBaseGame.TabIndex = 2;
            this.lBaseGame.Text = resources.GetString("lBaseGame.Text");
            // 
            // tLogic
            // 
            this.tLogic.AutoScroll = true;
            this.tLogic.Controls.Add(this.lLogic);
            this.tLogic.Location = new System.Drawing.Point(4, 25);
            this.tLogic.Name = "tLogic";
            this.tLogic.Padding = new System.Windows.Forms.Padding(3);
            this.tLogic.Size = new System.Drawing.Size(400, 442);
            this.tLogic.TabIndex = 7;
            this.tLogic.Text = "Logic editor";
            this.tLogic.UseVisualStyleBackColor = true;
            // 
            // lLogic
            // 
            this.lLogic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lLogic.Location = new System.Drawing.Point(6, 3);
            this.lLogic.Name = "lLogic";
            this.lLogic.Size = new System.Drawing.Size(388, 336);
            this.lLogic.TabIndex = 3;
            this.lLogic.Text = resources.GetString("lLogic.Text");
            // 
            // tItem
            // 
            this.tItem.Controls.Add(this.lItem);
            this.tItem.Location = new System.Drawing.Point(4, 25);
            this.tItem.Name = "tItem";
            this.tItem.Padding = new System.Windows.Forms.Padding(3);
            this.tItem.Size = new System.Drawing.Size(400, 442);
            this.tItem.TabIndex = 8;
            this.tItem.Text = "Item list";
            this.tItem.UseVisualStyleBackColor = true;
            // 
            // lItem
            // 
            this.lItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lItem.Location = new System.Drawing.Point(6, 3);
            this.lItem.Name = "lItem";
            this.lItem.Size = new System.Drawing.Size(388, 156);
            this.lItem.TabIndex = 4;
            this.lItem.Text = resources.GetString("lItem.Text");
            // 
            // tShuffles
            // 
            this.tShuffles.AutoScroll = true;
            this.tShuffles.Controls.Add(this.lShuffle);
            this.tShuffles.Location = new System.Drawing.Point(4, 25);
            this.tShuffles.Name = "tShuffles";
            this.tShuffles.Size = new System.Drawing.Size(400, 442);
            this.tShuffles.TabIndex = 2;
            this.tShuffles.Text = "Shuffles";
            this.tShuffles.UseVisualStyleBackColor = true;
            // 
            // lShuffle
            // 
            this.lShuffle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lShuffle.Location = new System.Drawing.Point(6, 3);
            this.lShuffle.Name = "lShuffle";
            this.lShuffle.Size = new System.Drawing.Size(372, 841);
            this.lShuffle.TabIndex = 2;
            this.lShuffle.Text = resources.GetString("lShuffle.Text");
            // 
            // tGimmicks
            // 
            this.tGimmicks.AutoScroll = true;
            this.tGimmicks.Controls.Add(this.lGimmick);
            this.tGimmicks.Location = new System.Drawing.Point(4, 25);
            this.tGimmicks.Name = "tGimmicks";
            this.tGimmicks.Size = new System.Drawing.Size(400, 442);
            this.tGimmicks.TabIndex = 3;
            this.tGimmicks.Text = "Gimmicks";
            this.tGimmicks.UseVisualStyleBackColor = true;
            // 
            // lGimmick
            // 
            this.lGimmick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lGimmick.Location = new System.Drawing.Point(6, 3);
            this.lGimmick.Name = "lGimmick";
            this.lGimmick.Size = new System.Drawing.Size(372, 294);
            this.lGimmick.TabIndex = 2;
            this.lGimmick.Text = resources.GetString("lGimmick.Text");
            // 
            // tCosmetics
            // 
            this.tCosmetics.AutoScroll = true;
            this.tCosmetics.Controls.Add(this.lCosmetic);
            this.tCosmetics.Location = new System.Drawing.Point(4, 25);
            this.tCosmetics.Name = "tCosmetics";
            this.tCosmetics.Size = new System.Drawing.Size(400, 442);
            this.tCosmetics.TabIndex = 4;
            this.tCosmetics.Text = "Cosmetics";
            this.tCosmetics.UseVisualStyleBackColor = true;
            // 
            // lCosmetic
            // 
            this.lCosmetic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lCosmetic.Location = new System.Drawing.Point(6, 3);
            this.lCosmetic.Name = "lCosmetic";
            this.lCosmetic.Size = new System.Drawing.Size(391, 268);
            this.lCosmetic.TabIndex = 2;
            this.lCosmetic.Text = resources.GetString("lCosmetic.Text");
            // 
            // tHow
            // 
            this.tHow.Controls.Add(this.lHow);
            this.tHow.Location = new System.Drawing.Point(4, 25);
            this.tHow.Name = "tHow";
            this.tHow.Padding = new System.Windows.Forms.Padding(3);
            this.tHow.Size = new System.Drawing.Size(400, 442);
            this.tHow.TabIndex = 6;
            this.tHow.Text = "How it works";
            this.tHow.UseVisualStyleBackColor = true;
            // 
            // lHow
            // 
            this.lHow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lHow.Location = new System.Drawing.Point(6, 3);
            this.lHow.Name = "lHow";
            this.lHow.Size = new System.Drawing.Size(388, 230);
            this.lHow.TabIndex = 3;
            this.lHow.Text = resources.GetString("lHow.Text");
            // 
            // ManualForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 495);
            this.Controls.Add(this.tManual);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "ManualForm";
            this.Text = "Manual";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fManual_FormClosing);
            this.tManual.ResumeLayout(false);
            this.tMain.ResumeLayout(false);
            this.tRules.ResumeLayout(false);
            this.tBaseChanges.ResumeLayout(false);
            this.tLogic.ResumeLayout(false);
            this.tItem.ResumeLayout(false);
            this.tShuffles.ResumeLayout(false);
            this.tGimmicks.ResumeLayout(false);
            this.tCosmetics.ResumeLayout(false);
            this.tHow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tManual;
        private System.Windows.Forms.TabPage tMain;
        private System.Windows.Forms.TabPage tRules;
        private System.Windows.Forms.TabPage tBaseChanges;
        private System.Windows.Forms.TabPage tShuffles;
        private System.Windows.Forms.TabPage tGimmicks;
        private System.Windows.Forms.TabPage tCosmetics;
        private System.Windows.Forms.Label lUsage;
        private System.Windows.Forms.Label lRules;
        private System.Windows.Forms.Label lBaseGame;
        private System.Windows.Forms.Label lShuffle;
        private System.Windows.Forms.Label lGimmick;
        private System.Windows.Forms.Label lCosmetic;
        private System.Windows.Forms.TabPage tHow;
        private System.Windows.Forms.Label lHow;
        private System.Windows.Forms.TabPage tLogic;
        private System.Windows.Forms.TabPage tItem;
        private System.Windows.Forms.Label lLogic;
        private System.Windows.Forms.Label lItem;
    }
}