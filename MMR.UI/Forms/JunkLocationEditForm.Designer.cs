namespace MMR.UI.Forms
{
    partial class JunkLocationEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JunkLocationEditForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lJunkLocations = new System.Windows.Forms.ListView();
            this.tJunkLocationsString = new System.Windows.Forms.TextBox();
            this.tSearchString = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lJunkLocations, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tJunkLocationsString, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tSearchString, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1323, 580);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lJunkLocations
            // 
            this.lJunkLocations.CheckBoxes = true;
            this.lJunkLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lJunkLocations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lJunkLocations.HideSelection = false;
            this.lJunkLocations.Location = new System.Drawing.Point(4, 3);
            this.lJunkLocations.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lJunkLocations.Name = "lJunkLocations";
            this.lJunkLocations.Size = new System.Drawing.Size(1315, 512);
            this.lJunkLocations.TabIndex = 0;
            this.lJunkLocations.UseCompatibleStateImageBehavior = false;
            this.lJunkLocations.View = System.Windows.Forms.View.List;
            this.lJunkLocations.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lJunkLocations_ItemChecked);
            // 
            // tJunkLocationsString
            // 
            this.tJunkLocationsString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tJunkLocationsString.Location = new System.Drawing.Point(4, 551);
            this.tJunkLocationsString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tJunkLocationsString.Name = "tJunkLocationsString";
            this.tJunkLocationsString.Size = new System.Drawing.Size(1315, 23);
            this.tJunkLocationsString.TabIndex = 1;
            this.tJunkLocationsString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tJunkLocationsString_KeyDown);
            // 
            // tSearchString
            // 
            this.tSearchString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSearchString.Location = new System.Drawing.Point(3, 521);
            this.tSearchString.Name = "tSearchString";
            this.tSearchString.Size = new System.Drawing.Size(1317, 23);
            this.tSearchString.TabIndex = 2;
            this.tSearchString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tSearchString_KeyDown);
            // 
            // JunkLocationEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1323, 580);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "JunkLocationEditForm";
            this.Text = "Junk Location Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JunkLocationEditForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView lJunkLocations;
        private System.Windows.Forms.TextBox tJunkLocationsString;
        private System.Windows.Forms.TextBox tSearchString;
    }
}