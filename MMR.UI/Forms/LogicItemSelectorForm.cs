using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MMR.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class LogicItemSelectorForm : Form
    {
        public List<string> ReturnItems;

        private IEnumerable<string> _filteredItems;
        private List<string> _selectedItems;
        private LogicFile _logic;
        private bool _showLocationNames;

        public LogicItemSelectorForm(LogicFile logic, bool checkboxes)
        {
            _logic = logic;
            InitializeComponent();
            SetHighlightedItems(null);
            SetSelectedItems(null);
            UpdateItems();
            this.ActiveControl = textBoxFilter;
            lItems.CheckBoxes = checkboxes;
        }

        public void SetLogicFile(LogicFile logic)
        {
            _logic = logic;
        }

        public void SetSelectedItems(List<string> selectedItems)
        {
            _selectedItems = selectedItems ?? new List<string>();
        }

        public void SetHighlightedItems(List<string> highlightedItems)
        {
            _filteredItems = highlightedItems ?? _logic.Logic.Select(item => item.Id);
        }

        public void SetShowLocationNames(bool val)
        {
            _showLocationNames = val;
        }

        public void UpdateItems()
        {
            lItems.Clear();
            foreach (var filteredItem in _filteredItems)
            {
                var label = GetLabel(filteredItem);
                var listViewItem = new ListViewItem(label);
                listViewItem.Tag = filteredItem;
                listViewItem.Checked = _selectedItems?.Contains(filteredItem) ?? false;
                lItems.Items.Add(listViewItem);
            }
        }

        private void bDone_Click(object sender, EventArgs e)
        {
            ReturnItems = _selectedItems.ToList();
            DialogResult = DialogResult.OK;
            Close();
        }

        private string GetLabel(string itemId)
        {
            if (Enum.TryParse(itemId, out Item item))
            {
                return (_showLocationNames ? item.Location() : item.Name()) ?? itemId;
            }
            else
            {
                return itemId;
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            //var filter = textBoxFilter.Text.ToLower();
            //_filteredItems = _logic.Logic.Where(item => GetLabel(item.Id).ToLower().Contains(filter)).Select(item => item.Id).ToList();
            //UpdateItems();
        }

        private void lItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lItems.CheckBoxes || lItems.SelectedIndices.Count == 0)
            {
                return;
            }
            ReturnItems = new List<string> { _filteredItems.ToList()[lItems.SelectedIndices.Cast<int>().First()] };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                _selectedItems.Add((string)e.Item.Tag);
            }
            else
            {
                _selectedItems.Remove((string)e.Item.Tag);
            }
        }

        private void textBoxFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                var filter = textBoxFilter.Text.ToLower();
                _filteredItems = _logic.Logic.Where(item => GetLabel(item.Id).ToLower().Contains(filter)).Select(item => item.Id).ToList();
                UpdateItems();
            }
        }
    }
}
