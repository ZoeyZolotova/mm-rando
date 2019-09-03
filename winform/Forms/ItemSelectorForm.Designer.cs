namespace MMRando
{
    partial class ItemSelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSelectorForm));
            this.lItems = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tLayout = new System.Windows.Forms.TableLayoutPanel();
            this.bDone = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.tLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // lItems
            // 
            this.lItems.CheckBoxes = true;
            this.lItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lItems.HideSelection = false;
            this.lItems.Location = new System.Drawing.Point(3, 3);
            this.lItems.Name = "lItems";
            this.lItems.Size = new System.Drawing.Size(1076, 481);
            this.lItems.TabIndex = 1;
            this.lItems.UseCompatibleStateImageBehavior = false;
            this.lItems.View = System.Windows.Forms.View.List;
            this.lItems.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lItems_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 160;
            // 
            // tLayout
            // 
            this.tLayout.ColumnCount = 1;
            this.tLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLayout.Controls.Add(this.lItems, 0, 0);
            this.tLayout.Controls.Add(this.bDone, 0, 2);
            this.tLayout.Controls.Add(this.textBoxFilter, 0, 1);
            this.tLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLayout.Location = new System.Drawing.Point(0, 0);
            this.tLayout.Name = "tLayout";
            this.tLayout.RowCount = 3;
            this.tLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tLayout.Size = new System.Drawing.Size(1082, 543);
            this.tLayout.TabIndex = 4;
            // 
            // bDone
            // 
            this.bDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bDone.Location = new System.Drawing.Point(3, 518);
            this.bDone.Name = "bDone";
            this.bDone.Size = new System.Drawing.Size(1076, 22);
            this.bDone.TabIndex = 2;
            this.bDone.Text = "Done";
            this.bDone.UseVisualStyleBackColor = true;
            this.bDone.Click += new System.EventHandler(this.bDone_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilter.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBoxFilter.Location = new System.Drawing.Point(3, 490);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(1076, 20);
            this.textBoxFilter.TabIndex = 3;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // ItemSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 543);
            this.Controls.Add(this.tLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ItemSelectorForm";
            this.Text = "Select items...";
            this.tLayout.ResumeLayout(false);
            this.tLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lItems;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TableLayoutPanel tLayout;
        private System.Windows.Forms.Button bDone;
        private System.Windows.Forms.TextBox textBoxFilter;
    }
}