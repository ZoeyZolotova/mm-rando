namespace MMRando.Forms
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.lAboutText = new System.Windows.Forms.Label();
            this.lAboutTitle = new System.Windows.Forms.Label();
            this.discordLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lAboutText
            // 
            this.lAboutText.Location = new System.Drawing.Point(27, 33);
            this.lAboutText.Name = "lAboutText";
            this.lAboutText.Size = new System.Drawing.Size(308, 145);
            this.lAboutText.TabIndex = 0;
            this.lAboutText.Text = resources.GetString("lAboutText.Text");
            this.lAboutText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lAboutTitle
            // 
            this.lAboutTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lAboutTitle.Location = new System.Drawing.Point(0, 1);
            this.lAboutTitle.Name = "lAboutTitle";
            this.lAboutTitle.Size = new System.Drawing.Size(359, 27);
            this.lAboutTitle.TabIndex = 1;
            this.lAboutTitle.Text = "Majora\'s Mask Randomizer";
            this.lAboutTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // discordLinkLabel
            // 
            this.discordLinkLabel.AutoSize = true;
            this.discordLinkLabel.Location = new System.Drawing.Point(108, 119);
            this.discordLinkLabel.Name = "discordLinkLabel";
            this.discordLinkLabel.Size = new System.Drawing.Size(141, 13);
            this.discordLinkLabel.TabIndex = 2;
            this.discordLinkLabel.TabStop = true;
            this.discordLinkLabel.Text = "https://discord.gg/8qbreUM";
            this.discordLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.discordLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.discordLinkLabel_LinkClicked);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 183);
            this.Controls.Add(this.discordLinkLabel);
            this.Controls.Add(this.lAboutTitle);
            this.Controls.Add(this.lAboutText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AboutForm";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lAboutText;
        private System.Windows.Forms.Label lAboutTitle;
        private System.Windows.Forms.LinkLabel discordLinkLabel;
    }
}