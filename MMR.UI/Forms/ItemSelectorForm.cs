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
    public partial class ItemSelectorForm : Form
    {
        public List<Item> ReturnItems;

        private IEnumerable<Item> _baseItemList;

        private bool _showLocationNames;

        public ItemSelectorForm(IEnumerable<Item> baseItemList, IEnumerable<Item> selectedItems, bool checkboxes = true, bool showLocationNames = true)
        {
            InitializeComponent();
            _baseItemList = baseItemList;
            _showLocationNames = showLocationNames;
            ReturnItems = selectedItems.ToList();
            UpdateItems();
            this.ActiveControl = textBoxFilter;
            lItems.CheckBoxes = checkboxes;
        }

        public void UpdateItems(string filter = null)
        {
            lItems.Clear();
            var items = _baseItemList;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                items = items.Where(item => GetLabel(item).ToLower().Contains(filter));
            }
            foreach (var item in items)
            {
                var label = GetLabel(item);
                var listViewItem = new ListViewItem(label);
                listViewItem.Tag = item;
                listViewItem.Checked = ReturnItems.Contains(item);
                lItems.Items.Add(listViewItem);
            }
        }

        private void bDone_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private string GetLabel(Item item)
        {
            return (_showLocationNames ? item.Location() : item.Name()) ?? item.ToString();
        }

        private void lItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lItems.CheckBoxes || lItems.SelectedItems.Count == 0)
            {
                return;
            }
            ;
            ReturnItems = new List<Item> { (Item)lItems.SelectedItems[0].Tag };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked)
            {
                ReturnItems.Add((Item)e.Item.Tag);
            }
            else
            {
                ReturnItems.Remove((Item)e.Item.Tag);
            }
        }

        private void textBoxFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                var filter = textBoxFilter.Text.ToLower();
                //_filteredItems = _logic.Logic.Where(item => GetLabel(item.Id).ToLower().Contains(filter)).Select(item => item.Id).ToList();
                UpdateItems(filter);
            }
        }
    }
}
