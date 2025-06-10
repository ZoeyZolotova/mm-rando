using MMR.Randomizer.Extensions;
using MMR.Randomizer.Models;
using MMR.Randomizer.Utils;
using MMR.UI.Controls;
using MMR.UI.Forms.Tooltips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MMR.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class ToggleTricksForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HashSet<string> Result { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LogicFile LogicFile { get; private set; }

        public ToggleTricksForm(LogicMode logicMode, string userLogicFilename, IEnumerable<string> tricksEnabled)
        {
            InitializeComponent();
            Result = tricksEnabled.ToHashSet();
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

            var searchTerm = txtSearch.Text.ToLower();
            Func<ItemObject, bool> searchPredicate = (io) => io.Name.ToLower().Contains(searchTerm) || io.TrickTooltip.ToLower().Contains(searchTerm);

            string currentCategory = string.Empty;
            foreach (var itemObject in tricks)
            {
                if (!searchPredicate(itemObject))
                {
                    continue;
                }
                if (itemObject.TrickCategory != null && currentCategory != itemObject.TrickCategory)
                {
                    currentCategory = itemObject.TrickCategory;
                    var cCategory = new InvertIndeterminateCheckBox();
                    cCategory.Text = currentCategory + ":";
                    cCategory.Location = new Point(9, y);
                    cCategory.Size = new Size(pTricks.Width - 50, deltaY);
                    cCategory.CheckStateChanged += cTrick_CheckStateChanged;
                    cCategory.Tag = tricks
                        .Where(io => io.TrickCategory == currentCategory && searchPredicate(io))
                        .Select(io => io.Name)
                        .ToHashSet();
                    pTricks.Controls.Add(cCategory);
                    y += deltaY;
                }
                var cTrick = new CheckBox();
                cTrick.Tag = new HashSet<string> { itemObject.Name };
                cTrick.Checked = Result.Contains(itemObject.Name);
                cTrick.Text = itemObject.Name;
                var size = TextRenderer.MeasureText(itemObject.Name, cTrick.Font);
                TooltipBuilder.SetTooltip(cTrick, itemObject.TrickTooltip);
                cTrick.Location = new Point(18, y);
                cTrick.Size = new Size(size.Width + 20, deltaY);
                cTrick.CheckStateChanged += cTrick_CheckStateChanged;
                pTricks.Controls.Add(cTrick);
                if (itemObject.TrickUrl.IsValidTrickUrl())
                {
                    var link = new Label
                    {
                        Text = "Video",
                        Location = new Point(18 + size.Width + 20, y + 3)
                    };
                    link.Font = new Font(link.Font, FontStyle.Underline);
                    link.ForeColor = Color.Blue;
                    link.Click += (object sender, EventArgs e) =>
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = itemObject.TrickUrl,
                            UseShellExecute = true,
                        });
                    };
                    pTricks.Controls.Add(link);
                }
                y += deltaY;
            }
            CalculateCategoryCheckboxes();
        }

        private void CalculateCategoryCheckboxes()
        {
            foreach (Control control in pTricks.Controls)
            {
                var checkbox = control as CheckBox;
                if (checkbox != null)
                {
                    var tricks = checkbox.Tag as HashSet<string>;
                    if (tricks != null)
                    {
                        checkbox.CheckStateChanged -= cTrick_CheckStateChanged;
                        var totalCount = tricks.Count;
                        var selectedCount = tricks.Count(trick => Result.Contains(trick));
                        checkbox.CheckState = totalCount == selectedCount ? CheckState.Checked : selectedCount == 0 ? CheckState.Unchecked : CheckState.Indeterminate;
                        checkbox.CheckStateChanged += cTrick_CheckStateChanged;
                    }
                }
            }
        }

        private void cTrick_CheckStateChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var tricks = (HashSet<string>)checkbox.Tag;
            if (checkbox.Checked)
            {
                foreach (var trick in tricks)
                {
                    Result.Add(trick);
                }
            }
            else
            {
                foreach (var trick in tricks)
                {
                    Result.Remove(trick);
                }
            }

            CalculateCategoryCheckboxes();
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
