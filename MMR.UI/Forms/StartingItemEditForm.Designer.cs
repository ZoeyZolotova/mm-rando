namespace MMR.UI.Forms
{
    partial class StartingItemEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartingItemEditForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lStartingItems = new System.Windows.Forms.ListView();
            this.tStartingItemsString = new System.Windows.Forms.TextBox();
            this.tSearchString = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lStartingItems, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tStartingItemsString, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tSearchString, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(512, 580);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lStartingItems
            // 
            this.lStartingItems.CheckBoxes = true;
            this.lStartingItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lStartingItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lStartingItems.HideSelection = false;
            this.lStartingItems.Location = new System.Drawing.Point(4, 3);
            this.lStartingItems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lStartingItems.Name = "lStartingItems";
            this.lStartingItems.Size = new System.Drawing.Size(504, 510);
            this.lStartingItems.TabIndex = 0;
            this.lStartingItems.UseCompatibleStateImageBehavior = false;
            this.lStartingItems.View = System.Windows.Forms.View.List;
            this.lStartingItems.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lStartingItems_ItemChecked);
            // 
            // tStartingItemsString
            // 
            this.tStartingItemsString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tStartingItemsString.Location = new System.Drawing.Point(4, 551);
            this.tStartingItemsString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tStartingItemsString.Name = "tStartingItemsString";
            this.tStartingItemsString.Size = new System.Drawing.Size(504, 23);
            this.tStartingItemsString.TabIndex = 1;
            this.tStartingItemsString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tStartingItemsString_KeyDown);
            // 
            // tSearchString
            // 
            this.tSearchString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSearchString.Location = new System.Drawing.Point(3, 519);
            this.tSearchString.Name = "tSearchString";
            this.tSearchString.Size = new System.Drawing.Size(506, 23);
            this.tSearchString.TabIndex = 2;
            this.tSearchString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tSearchString_KeyDown);
            // 
            // StartingItemEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 580);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "StartingItemEditForm";
            this.Text = "Starting Item Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartingItemEditForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView lStartingItems;
        private System.Windows.Forms.TextBox tStartingItemsString;
        private System.Windows.Forms.TextBox tSearchString;
    }
}