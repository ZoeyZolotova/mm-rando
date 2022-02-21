using MMR.Randomizer.Models;
using MMR.Randomizer.Utils;
using MMR.UI.Forms.Tooltips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MMR.UI.Forms
{
    public partial class ToggleTricksForm : Form
    {
        public List<string> Result { get; private set; }
        public LogicFile LogicFile { get; private set; }

        public ToggleTricksForm(LogicMode logicMode, string userLogicFilename, IEnumerable<string> tricksEnabled)
        {
            InitializeComponent();
            Result = tricksEnabled.ToList();
            LogicFile = LogicUtils.ReadRulesetFromResources(logicMode, userLogicFilename);
            Write_Tricks();
        }

        private void Write_Tricks()
        {
            pTricks.Controls.Clear();
            var itemList = LogicUtils.PopulateItemListFromLogicData(LogicFile);

            var y = 9;
            var deltaY = 23;
            var tricks = itemList.Where(io => io.IsTrick);
            var categories = tricks.Select(io => string.IsNullOrWhiteSpace(io.TrickCategory) ? "Misc" : io.TrickCategory).Distinct().ToList();

            if (categories.Count > 1)
            {
                foreach (var i in tricks)
                {
                    i.TrickCategory = string.IsNullOrWhiteSpace(i.TrickCategory) ? "Misc" : i.TrickCategory;
                }
                tricks = tricks.OrderBy(io => categories.IndexOf(io.TrickCategory));

            }

            string currentCategory = string.Empty;
            foreach (var itemObject in tricks)
            {
                if (!itemObject.Name.ToLower().Contains(txtSearch.Text.ToLower())) { continue; }
                if (itemObject.TrickCategory != null && currentCategory != itemObject.TrickCategory)
                {
                    currentCategory = itemObject.TrickCategory;
                    Label CategoryLabel = new Label();
                    CategoryLabel.Text = currentCategory + ":";
                    CategoryLabel.Location = new Point(5, y + 3);
                    CategoryLabel.Size = new Size(pTricks.Width - 50, deltaY);
                    pTricks.Controls.Add(CategoryLabel);
                    y += deltaY;
                }
                var cTrick = new CheckBox();
                cTrick.Tag = itemObject;
                cTrick.Checked = Result.Contains(itemObject.Name);
                cTrick.Text = itemObject.Name;
                TooltipBuilder.SetTooltip(cTrick, itemObject.TrickTooltip);
                cTrick.Location = new Point(9, y);
                cTrick.Size = new Size(pTricks.Width - 50, deltaY);
                cTrick.CheckedChanged += cTrick_CheckedChanged;
                pTricks.Controls.Add(cTrick);
                y += deltaY;
            }
        }

        private void cTrick_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var itemObject = (ItemObject)checkbox.Tag;
            if (checkbox.Checked)
            {
                Result.Add(itemObject.Name);
            }
            else
            {
                Result.Remove(itemObject.Name);
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Write_Tricks();
        }
    }
}
