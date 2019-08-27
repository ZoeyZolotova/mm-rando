namespace MMRando.Forms
{
    public class ItemTreeView : System.Windows.Forms.TreeView
    {
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == 0x0203 && this.CheckBoxes)
            {
                var localPos = this.PointToClient(MousePosition);
                var hitTestInfo = this.HitTest(localPos);
                if (hitTestInfo.Location == System.Windows.Forms.TreeViewHitTestLocations.StateImage)
                {
                    m.Msg = 0x0201;
                }
            }
            base.WndProc(ref m);
        }
    }
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Starting Items");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Items");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Masks");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Hearts");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Rupies");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Tingle Maps");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Songs");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Dungeon Items");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Bottled Items");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Shop Items");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Great Fairy Rewards");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Other Items");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemEditForm));
            this.tSetting = new System.Windows.Forms.TextBox();
            this.tLayout = new System.Windows.Forms.TableLayoutPanel();
            this.ItemListEditorTree = new ItemTreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tLayout.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tSetting
            // 
            this.tSetting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tSetting.Location = new System.Drawing.Point(3, 434);
            this.tSetting.Name = "tSetting";
            this.tSetting.Size = new System.Drawing.Size(334, 20);
            this.tSetting.TabIndex = 1;
            this.tSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tSetting_KeyDown);
            // 
            // tLayout
            // 
            this.tLayout.ColumnCount = 1;
            this.tLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLayout.Controls.Add(this.tSetting, 0, 1);
            this.tLayout.Controls.Add(this.ItemListEditorTree, 0, 0);
            this.tLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLayout.Location = new System.Drawing.Point(0, 24);
            this.tLayout.Name = "tLayout";
            this.tLayout.RowCount = 2;
            this.tLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tLayout.Size = new System.Drawing.Size(340, 457);
            this.tLayout.TabIndex = 0;
            // 
            // ItemListEditorTree
            // 
            this.ItemListEditorTree.CheckBoxes = true;
            this.ItemListEditorTree.Location = new System.Drawing.Point(3, 3);
            this.ItemListEditorTree.Name = "ItemListEditorTree";
            treeNode1.Name = "nodeStarting";
            treeNode1.Text = "Starting Items";
            treeNode2.Name = "nodeItems";
            treeNode2.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode2.Text = "Items";
            treeNode3.Name = "nodeMasks";
            treeNode3.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode3.Text = "Masks";
            treeNode4.Name = "nodeHearts";
            treeNode4.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode4.Text = "Hearts";
            treeNode5.Name = "nodeRupies";
            treeNode5.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode5.Text = "Rupies";
            treeNode6.Name = "nodeMaps";
            treeNode6.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode6.Text = "Tingle Maps";
            treeNode7.Name = "nodeSongs";
            treeNode7.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode7.Text = "Songs";
            treeNode8.Name = "nodeDungeonItems";
            treeNode8.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode8.Text = "Dungeon Items";
            treeNode9.Name = "nodeBottledItems";
            treeNode9.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode9.Text = "Bottled Items";
            treeNode10.Name = "nodeShopItems";
            treeNode10.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode10.Text = "Shop Items";
            treeNode11.Name = "nodeFairy";
            treeNode11.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode11.Text = "Great Fairy Rewards";
            treeNode12.Name = "nodeOther";
            treeNode12.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            treeNode12.Text = "Other Items";
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
            treeNode11,
            treeNode12});
            this.ItemListEditorTree.Size = new System.Drawing.Size(332, 423);
            this.ItemListEditorTree.TabIndex = 0;
            this.ItemListEditorTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.ItemListEditorTree_AfterCheck);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(340, 24);
            this.menuStrip1.TabIndex = 1;
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
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.CollapseAllToolStripMenuItem_Click);
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
            this.ClientSize = new System.Drawing.Size(340, 481);
            this.Controls.Add(this.tLayout);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ItemEditForm";
            this.Text = "Item List Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fItemEdit_FormClosing);
            this.tLayout.ResumeLayout(false);
            this.tLayout.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ItemTreeView ItemListEditorTree;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox tSetting;
        private System.Windows.Forms.TableLayoutPanel tLayout;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
    }
}