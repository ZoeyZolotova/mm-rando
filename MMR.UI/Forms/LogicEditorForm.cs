using MMR.Randomizer.LogicMigrator;
using MMR.Randomizer.Properties;
using MMR.Randomizer.GameObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.Models;
using MMR.Common.Utils;

namespace MMR.UI.Forms
{
    public partial class LogicEditorForm : Form
    {
        bool updating = false;
        int n;

        private readonly int _defaultItemCount;

        private LogicFile _logic;
        private Dictionary<string, JsonFormatLogicItem> _itemsById;

        private readonly ItemSelectorForm _singleItemSelectorForm;
        private readonly ItemSelectorForm _multiItemSelectorForm;

        public LogicEditorForm()
        {
            InitializeComponent();
            Reset();
            _defaultItemCount = _logic.Logic.Count;
            nItem.Minimum = 0;
            nItem.Maximum = _logic.Logic.Count - 1;
            nItem.Value = 0;
            _singleItemSelectorForm = new ItemSelectorForm(_logic, false);
            _multiItemSelectorForm = new ItemSelectorForm(_logic, true);
        }

        private void FillDependence(int n)
        {
            lRequired.Items.Clear();
            foreach (var itemId in _logic.Logic[n].RequiredItems)
            {
                if (Enum.TryParse(itemId, out Item item))
                {
                    lRequired.Items.Add(item.Name() ?? itemId);
                }
                else
                {
                    lRequired.Items.Add(itemId);
                }
            }
        }

        private void UpdateDependence(int n)
        {
            _multiItemSelectorForm.SetSelectedItems(null);
            _multiItemSelectorForm.SetHighlightedItems(null);
            _multiItemSelectorForm.UpdateItems();
            DialogResult R = _multiItemSelectorForm.ShowDialog();
            if (R == DialogResult.OK)
            {
                var returned = _multiItemSelectorForm.ReturnItems;
                if (returned.Count == 0)
                {
                    return;
                }
                foreach (var returnedItemId in returned)
                {
                    var requiredItems = _logic.Logic[n].RequiredItems;
                    if (!requiredItems.Contains(returnedItemId))
                    {
                        requiredItems.Add(returnedItemId);
                    }
                }
                FillDependence(n);
            }
        }

        private void FillConditional(int n)
        {
            lConditional.Items.Clear();
            foreach (var conditional in _logic.Logic[n].ConditionalItems)
            {
                var label = string.Join(",", conditional.Select(c =>
                {
                    if (Enum.TryParse(c, out Item item))
                    {
                        return item.Name() ?? c;
                    }
                    else
                    {
                        return c;
                    }
                }));
                lConditional.Items.Add(label);
            }
        }

        private void UpdateConditional(int n, int? conditionalIndex = null)
        {
            List<string> selectedItems = null;
            if (conditionalIndex.HasValue)
            {
                selectedItems = _logic.Logic[n].ConditionalItems[conditionalIndex.Value].ToList();
            }
            _multiItemSelectorForm.SetSelectedItems(selectedItems);
            _multiItemSelectorForm.SetHighlightedItems(null);
            _multiItemSelectorForm.UpdateItems();
            DialogResult result = _multiItemSelectorForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                var returned = _multiItemSelectorForm.ReturnItems;
                if (returned.Count == 0)
                {
                    return;
                }
                if (conditionalIndex.HasValue)
                {
                    _logic.Logic[n].ConditionalItems[conditionalIndex.Value] = returned.ToList();
                }
                else
                {
                    _logic.Logic[n].ConditionalItems.Add(returned.ToList());
                }
                FillConditional(n);
            }
        }

        private void FillTime(int n)
        {
            var itemLogic = _logic.Logic[n];

            cNDay1.Checked = itemLogic.TimeNeeded.HasFlag(TimeOfDay.Day1);
            cNNight1.Checked = itemLogic.TimeNeeded.HasFlag(TimeOfDay.Night1);
            cNDay2.Checked = itemLogic.TimeNeeded.HasFlag(TimeOfDay.Day2);
            cNNight2.Checked = itemLogic.TimeNeeded.HasFlag(TimeOfDay.Night2);
            cNDay3.Checked = itemLogic.TimeNeeded.HasFlag(TimeOfDay.Day3);
            cNNight3.Checked = itemLogic.TimeNeeded.HasFlag(TimeOfDay.Night3);

            cADay1.Checked = itemLogic.TimeAvailable.HasFlag(TimeOfDay.Day1);
            cANight1.Checked = itemLogic.TimeAvailable.HasFlag(TimeOfDay.Night1);
            cADay2.Checked = itemLogic.TimeAvailable.HasFlag(TimeOfDay.Day2);
            cANight2.Checked = itemLogic.TimeAvailable.HasFlag(TimeOfDay.Night2);
            cADay3.Checked = itemLogic.TimeAvailable.HasFlag(TimeOfDay.Day3);
            cANight3.Checked = itemLogic.TimeAvailable.HasFlag(TimeOfDay.Night3);

            cSDay1.Checked = itemLogic.TimeSetup.HasFlag(TimeOfDay.Day1);
            cSNight1.Checked = itemLogic.TimeSetup.HasFlag(TimeOfDay.Night1);
            cSDay2.Checked = itemLogic.TimeSetup.HasFlag(TimeOfDay.Day2);
            cSNight2.Checked = itemLogic.TimeSetup.HasFlag(TimeOfDay.Night2);
            cSDay3.Checked = itemLogic.TimeSetup.HasFlag(TimeOfDay.Day3);
            cSNight3.Checked = itemLogic.TimeSetup.HasFlag(TimeOfDay.Night3);
        }

        private void UpdateTime(int n)
        {
            var Av = TimeOfDay.None;
            if (cADay1.Checked) { Av |= TimeOfDay.Day1; }
            if (cANight1.Checked) { Av |= TimeOfDay.Night1; }
            if (cADay2.Checked) { Av |= TimeOfDay.Day2; }
            if (cANight2.Checked) { Av |= TimeOfDay.Night2; }
            if (cADay3.Checked) { Av |= TimeOfDay.Day3; }
            if (cANight3.Checked) { Av |= TimeOfDay.Night3; }
            _logic.Logic[n].TimeAvailable = Av;

            var Ne = TimeOfDay.None;
            if (cNDay1.Checked) { Ne |= TimeOfDay.Day1; }
            if (cNNight1.Checked) { Ne |= TimeOfDay.Night1; }
            if (cNDay2.Checked) { Ne |= TimeOfDay.Day2; }
            if (cNNight2.Checked) { Ne |= TimeOfDay.Night2; }
            if (cNDay3.Checked) { Ne |= TimeOfDay.Day3; }
            if (cNNight3.Checked) { Ne |= TimeOfDay.Night3; }
            _logic.Logic[n].TimeNeeded = Ne;

            var Se = TimeOfDay.None;
            if (cSDay1.Checked) { Se |= TimeOfDay.Day1; }
            if (cSNight1.Checked) { Se |= TimeOfDay.Night1; }
            if (cSDay2.Checked) { Se |= TimeOfDay.Day2; }
            if (cSNight2.Checked) { Se |= TimeOfDay.Night2; }
            if (cSDay3.Checked) { Se |= TimeOfDay.Day3; }
            if (cSNight3.Checked) { Se |= TimeOfDay.Night3; }
            _logic.Logic[n].TimeSetup = Se;
        }

        private void FillTrick(int n)
        {
            cTrick.Checked = _logic.Logic[n].IsTrick;
            tTrickDescription.Text = _logic.Logic[n].TrickTooltip ?? "(optional tooltip)";
            tTrickDescription.ForeColor = _logic.Logic[n].TrickTooltip != null ? SystemColors.WindowText : SystemColors.WindowFrame;
            tTrickCategory.Text = _logic.Logic[n].TrickCategory ?? "(optional category)";
            tTrickCategory.ForeColor = _logic.Logic[n].TrickCategory != null ? SystemColors.WindowText : SystemColors.WindowFrame;
        }

        private void Reset()
        {
            _logic = new LogicFile
            {
                Logic = Enum.GetValues<Item>().Where(item => item >= 0).Select(item => new JsonFormatLogicItem
                {
                    Id = item.ToString(),
                    RequiredItems = new List<string>(),
                    ConditionalItems = new List<List<string>>(),
                    TimeAvailable = TimeOfDay.None,
                    TimeNeeded = TimeOfDay.None,
                    IsTrick = false,
                    TrickTooltip = string.Empty,
                }).ToList(),
            };
            _singleItemSelectorForm?.SetLogicFile(_logic);
            _multiItemSelectorForm?.SetLogicFile(_logic);
            _itemsById = _logic.Logic.ToDictionary(item => item.Id);
        }

        private void fLogicEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            };
        }

        private void nItem_ValueChanged(object sender, EventArgs e)
        {
            SetIndex((int)nItem.Value);
        }

        private void SetIndex(int index)
        {
            n = index;
            if (Enum.TryParse(_logic.Logic[n].Id, out Item item))
            {
                lIName.Text = item.Location() ?? item.ToString();
                var itemName = item.Name();
                if (!string.IsNullOrWhiteSpace(itemName))
                {
                    lIName.Text += $" ({itemName})";
                }
            }
            else
            {
                lIName.Text = _logic.Logic[n].Id;
            }
            updating = true;
            FillDependence(n);
            FillConditional(n);
            FillTime(n);
            FillTrick(n);

            var isCustomItem = index >= _defaultItemCount;
            bRenameItem.Visible = isCustomItem;
            bDeleteItem.Visible = isCustomItem;
            cTrick.Visible = isCustomItem;
            tTrickDescription.Visible = isCustomItem;
            tTrickCategory.Visible = isCustomItem;

            var isMultiLocation = _logic.Logic[n].IsMultiLocation;
            tMain.Enabled = !isMultiLocation;

            updating = false;
        }

        private void cNDay1_CheckedChanged(object sender, EventArgs e)
        {
            if (updating)
            {
                return;
            };
            UpdateTime(n);
        }

        private void bReqAdd_Click(object sender, EventArgs e)
        {
            UpdateDependence(n);
        }

        private void bConAdd_Click(object sender, EventArgs e)
        {
            UpdateConditional(n);
        }

        private void bReqClear_Click(object sender, EventArgs e)
        {
            if (lRequired.SelectedIndex != -1)
            {
                _logic.Logic[n].RequiredItems.RemoveAt(lRequired.SelectedIndex);
                FillDependence(n);
            };
        }

        private void bConClear_Click(object sender, EventArgs e)
        {
            if (lConditional.SelectedIndex != -1)
            {
                _logic.Logic[n].ConditionalItems.RemoveAt(lConditional.SelectedIndex);
                FillConditional(n);
            };
        }

        private void mNew_Click(object sender, EventArgs e)
        {
            //ItemSelectorForm.ResetItems();
            Reset();
            nItem.Minimum = 0;
            nItem.Maximum = _logic.Logic.Count - 1;
            nItem.Value = 1;
            nItem.Value = 0;
        }

        private void mImport_Click(object sender, EventArgs e)
        {
            if (openLogic.ShowDialog() == DialogResult.OK)
            {
                using (var logicFile = new StreamReader(File.OpenRead(openLogic.FileName)))
                {
                    var logicString = logicFile.ReadToEnd();
                    LoadLogic(logicString);
                }
            }
        }

        private void mSave_Click(object sender, EventArgs e)
        {
            if (saveLogic.ShowDialog() == DialogResult.OK)
            {
                _logic.Version = Migrator.CurrentVersion;
                using (var writer = new StreamWriter(File.Open(saveLogic.FileName, FileMode.Create)))
                {
                    writer.Write(JsonSerializer.Serialize(_logic));
                }
            }
        }

        private void btn_new_item_Click(object sender, EventArgs e)
        {
            using (var newItemForm = new NewItemForm())
            {
                var result = newItemForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var newItem = new JsonFormatLogicItem
                    {
                        Id = newItemForm.ReturnValue,
                    };
                    _logic.Logic.Add(newItem);
                    _itemsById[newItem.Id] = newItem;
                    nItem.Maximum = _logic.Logic.Count - 1;
                    nItem.Value = nItem.Maximum;
                    //ItemSelectorForm.AddItem(newItemForm.ReturnValue);
                }
            }
        }

        private void lConditional_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = lConditional.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var conditions = _logic.Logic[n].ConditionalItems[index];
                var conditionIndices = conditions.ToList();
                if (conditionIndices.Count == 1)
                {
                    nItem.Value = _logic.Logic.IndexOf(_itemsById[conditionIndices[0]]);
                }
                else
                {
                    _singleItemSelectorForm.SetHighlightedItems(conditionIndices);
                    _singleItemSelectorForm.SetShowLocationNames(false);
                    _singleItemSelectorForm.UpdateItems();
                    var result = _singleItemSelectorForm.ShowDialog();
                    if (result == DialogResult.OK && _singleItemSelectorForm.ReturnItems.Any())
                    {
                        var itemId = _singleItemSelectorForm.ReturnItems.First();
                        nItem.Value = _logic.Logic.IndexOf(_itemsById[itemId]);
                    }
                }
            }
        }

        private void button_goto_Click(object sender, EventArgs e)
        {
            _singleItemSelectorForm.SetHighlightedItems(null);
            _singleItemSelectorForm.SetShowLocationNames(true);
            _singleItemSelectorForm.UpdateItems();
            var result = _singleItemSelectorForm.ShowDialog();
            if (result == DialogResult.OK && _singleItemSelectorForm.ReturnItems.Any())
            {
                var itemId = _singleItemSelectorForm.ReturnItems.First();
                nItem.Value = _logic.Logic.IndexOf(_itemsById[itemId]);
            }
        }

        private void lRequired_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var index = lRequired.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var gotoItemIndex = _logic.Logic[n].RequiredItems[index];
                nItem.Value = _logic.Logic.IndexOf(_itemsById[gotoItemIndex]);
            }
        }

        private void bConEdit_MouseClick(object sender, MouseEventArgs e)
        {
            var index = lConditional.SelectedIndex;
            if (index != ListBox.NoMatches)
            {
                UpdateConditional(n, index);
            }
        }

        private void bConClone_Click(object sender, EventArgs e)
        {
            var index = lConditional.SelectedIndex;
            if (index != ListBox.NoMatches)
            {
                _logic.Logic[n].ConditionalItems.Insert(index + 1, _logic.Logic[n].ConditionalItems[index]);
                FillConditional(n);
            }
        }

        private void casualToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadLogic(Resources.REQ_CASUAL);
        }

        private void glitchedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadLogic(Resources.REQ_GLITCH);
        }

        private void LoadLogic(string logicString)
        {
            logicString = Migrator.ApplyMigrations(logicString);
            var logic = LogicFile.FromJson(logicString);
            try
            {
                var itemsById = logic.Logic.ToDictionary(item => item.Id);

                VerifyLogic(logic, itemsById);

                _logic = logic;
                _itemsById = itemsById;

                _singleItemSelectorForm.SetLogicFile(_logic);
                _multiItemSelectorForm.SetLogicFile(_logic);

                nItem.Maximum = _logic.Logic.Count - 1;
                SetIndex((int)nItem.Value);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VerifyLogic(LogicFile logic, Dictionary<string, JsonFormatLogicItem> itemsById)
        {
            foreach (var item in _logic.Logic)
            {
                foreach (var requiredItem in item.RequiredItems)
                {
                    if (!_itemsById.ContainsKey(requiredItem))
                    {
                        throw new Exception($"Item '{requiredItem}' not found.");
                    }
                }

                foreach (var conditionals in item.ConditionalItems)
                {
                    foreach (var conditionalItem in conditionals)
                    {
                        if (!_itemsById.ContainsKey(conditionalItem))
                        {
                            throw new Exception($"Item '{conditionalItem}' not found.");
                        }
                    }
                }
            }
        }

        private void bRenameItem_Click(object sender, EventArgs e)
        {
            using (var newItemForm = new NewItemForm())
            {
                var result = newItemForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var oldValue = _logic.Logic[n].Id;
                    var newValue = newItemForm.ReturnValue;
                    foreach (var item in _logic.Logic)
                    {
                        item.RequiredItems = item.RequiredItems.Select(ri => ri == oldValue ? newValue : ri).ToList();
                        item.ConditionalItems = item.ConditionalItems.Select(c => c.Select(ci => ci == oldValue ? newValue : ci).ToList()).ToList();
                    }
                    _logic.Logic[n].Id = newValue;
                    //ItemSelectorForm.RenameItem(n, newItemForm.ReturnValue);
                    lIName.Text = newItemForm.ReturnValue;
                }
            }
        }

        private void bDeleteItem_Click(object sender, EventArgs e)
        {
            string message;
            string caption;
            MessageBoxButtons buttons;
            var id = _logic.Logic[n].Id;
            var usedBy = _logic.Logic.Where(il => il.RequiredItems.Contains(id) || il.ConditionalItems.Any(c => c.Contains(id))).ToList();
            if (usedBy.Any())
            {
                // in use
                caption = "Error";
                message = "This item is in use by:\n"+ string.Join("\n", usedBy.Take(5).Select(il => "* " + il.Id));
                if (usedBy.Count > 5)
                {
                    message += $"\nand {usedBy.Count-5} other{(usedBy.Count > 6 ? "s" : "")}.";
                }
                buttons = MessageBoxButtons.OK;
            }
            else
            {
                // not in use
                caption = "Warning";
                message = "Are you sure you want to delete this item?";
                buttons = MessageBoxButtons.YesNo;
            }
            var result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.Yes)
            {
                _logic.Logic.RemoveAt(n);
                //ItemSelectorForm.RemoveItem(n);
                nItem.Maximum = _logic.Logic.Count - 1;
                SetIndex(n);
            }
        }

        private const string DEFAULT_TRICK_TOOLTIP = "(optional tooltip)";
        private const string DEFAULT_CATEGORY_TOOLTIP = "(optional category)";

        private void tTrickDescription_TextChanged(object sender, EventArgs e)
        {
            _logic.Logic[n].TrickTooltip = string.IsNullOrWhiteSpace(tTrickDescription.Text) || tTrickDescription.Text == DEFAULT_TRICK_TOOLTIP
                ? null
                : tTrickDescription.Text;
        }

        private void tTrickDescription_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_logic.Logic[n].TrickTooltip))
            {
                tTrickDescription.Text = string.Empty;
                tTrickDescription.ForeColor = SystemColors.WindowText;
            }
        }

        private void cTrick_CheckedChanged(object sender, EventArgs e)
        {
            _logic.Logic[n].IsTrick = cTrick.Checked;
        }

        private void tTrickDescription_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_logic.Logic[n].TrickTooltip))
            {
                tTrickDescription.Text = DEFAULT_TRICK_TOOLTIP;
                tTrickDescription.ForeColor = SystemColors.WindowFrame;
            }
        }

        private void bCopy_Click(object sender, EventArgs e)
        {
            _singleItemSelectorForm.SetHighlightedItems(null);
            _singleItemSelectorForm.SetShowLocationNames(true);
            _singleItemSelectorForm.UpdateItems();
            var result = _singleItemSelectorForm.ShowDialog();
            if (result == DialogResult.OK && _singleItemSelectorForm.ReturnItems.Any())
            {
                var itemId = _singleItemSelectorForm.ReturnItems.First();

                _logic.Logic[n].RequiredItems = _itemsById[itemId].RequiredItems.ToList();
                _logic.Logic[n].ConditionalItems = _itemsById[itemId].ConditionalItems.Select(c => c.ToList()).ToList();
                _logic.Logic[n].IsTrick = _itemsById[itemId].IsTrick;
                _logic.Logic[n].TimeAvailable = _itemsById[itemId].TimeAvailable;
                _logic.Logic[n].TimeNeeded = _itemsById[itemId].TimeNeeded;
                _logic.Logic[n].TimeSetup = _itemsById[itemId].TimeSetup;
                _logic.Logic[n].TrickTooltip = _itemsById[itemId].TrickTooltip;

                SetIndex(n);
                //nItem.Value = itemIndex;
            }
        }

        private void tCategoryDescription_TextChanged(object sender, EventArgs e)
        {
            _logic.Logic[n].TrickCategory = string.IsNullOrWhiteSpace(tTrickCategory.Text) || tTrickCategory.Text == DEFAULT_CATEGORY_TOOLTIP
                ? null
                : tTrickCategory.Text;
        }

        private void tCategoryDescription_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_logic.Logic[n].TrickCategory))
            {
                tTrickCategory.Text = string.Empty;
                tTrickCategory.ForeColor = SystemColors.WindowText;
            }
        }
        private void tCategoryDescription_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_logic.Logic[n].TrickCategory))
            {
                tTrickCategory.Text = DEFAULT_CATEGORY_TOOLTIP;
                tTrickCategory.ForeColor = SystemColors.WindowFrame;
            }
        }
    }
}
