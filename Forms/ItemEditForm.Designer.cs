namespace MMRando.Forms
{
    partial class ItemEditForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Items");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Masks");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Hearts");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Rupies");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Tingle Maps");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Songs");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Dungeon Items");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Bottled Items");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Shop Items");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Other Items");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemEditForm));
            this.ItemListEditorTree = new System.Windows.Forms.TreeView();
            this.TSetting = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CollapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemListEditorTree
            // 
            this.ItemListEditorTree.CheckBoxes = true;
            this.ItemListEditorTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ItemListEditorTree.Location = new System.Drawing.Point(0, 24);
            this.ItemListEditorTree.Name = "ItemListEditorTree";
            treeNode1.Name = "nodeItems";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode1.Text = "Items";
            treeNode2.Name = "nodeMasks";
            treeNode2.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode2.Text = "Masks";
            treeNode3.Name = "nodeHearts";
            treeNode3.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode3.Text = "Hearts";
            treeNode4.Name = "nodeRupies";
            treeNode4.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode4.Text = "Rupies";
            treeNode5.Name = "nodeMaps";
            treeNode5.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode5.Text = "Tingle Maps";
            treeNode6.Name = "nodeSongs";
            treeNode6.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode6.Text = "Songs";
            treeNode7.Name = "nodeDungeonItems";
            treeNode7.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode7.Text = "Dungeon Items";
            treeNode8.Name = "nodeBottledItems";
            treeNode8.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode8.Text = "Bottled Items";
            treeNode9.Name = "nodeShopItems";
            treeNode9.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode9.Text = "Shop Items";
            treeNode10.Name = "nodeOther";
            treeNode10.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode10.Text = "Other Items";
            treeNode11.Name = "blanknode";
            treeNode11.Text = "";
            this.ItemListEditorTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            this.ItemListEditorTree.Size = new System.Drawing.Size(441, 639);
            this.ItemListEditorTree.TabIndex = 0;
            this.ItemListEditorTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.ItemListEditorTree_AfterCheck);
            // 
            // TSetting
            // 
            this.TSetting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TSetting.Location = new System.Drawing.Point(0, 641);
            this.TSetting.Name = "TSetting";
            this.TSetting.Size = new System.Drawing.Size(441, 22);
            this.TSetting.TabIndex = 1;
            this.TSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TSetting_KeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.CollapseAllToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(441, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.ExpandAllToolStripMenuItem_Click);
            // 
            // CollapseAllToolStripMenuItem
            // 
            this.CollapseAllToolStripMenuItem.Name = "CollapseAllToolStripMenuItem";
            this.CollapseAllToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.CollapseAllToolStripMenuItem.Text = "Collapse All";
            this.CollapseAllToolStripMenuItem.Click += new System.EventHandler(this.CollapseAllToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.SelectNoneToolStripMenuItem_Click);
            // 
            // ItemEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 663);
            this.Controls.Add(this.TSetting);
            this.Controls.Add(this.ItemListEditorTree);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ItemEditForm";
            this.Text = "Item List Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fItemEdit_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView ItemListEditorTree;
        private System.Windows.Forms.TextBox TSetting;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CollapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
    }
}