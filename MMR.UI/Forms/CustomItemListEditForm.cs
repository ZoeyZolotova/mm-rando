using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Utils;
using MMR.Randomizer.GameObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MMR.Randomizer.Extensions;

namespace MMR.UI.Forms
{
    public partial class CustomItemListEditForm : Form
    {
        public List<Item> BaseItemList { get; private set; }
        private bool updating = false;
        private readonly int _itemGroupCount;
        private readonly string _invalidErrorMessage;
        private readonly Func<Item, string> _labelSelector;

        public string ExternalLabel { get; private set; }
        public List<Item> ItemList { get; private set; } = new List<Item>();
        public string ItemListString { get; private set; }

        public CustomItemListEditForm(IEnumerable<Item> baseItemList, Func<Item, string> labelSelector, string invalidErrorMessage)
        {
            _invalidErrorMessage = invalidErrorMessage;
            InitializeComponent();

            BaseItemList = baseItemList.ToList();
            _itemGroupCount = (int)Math.Ceiling(BaseItemList.Count / 32.0);

            _labelSelector = labelSelector;
            PrintToListView();

            UpdateString(ItemList);
            ExternalLabel = $"{ItemList.Count}/{BaseItemList.Count} items randomized";
        }

        private void PrintToListView()
        {
            foreach (var item in BaseItemList)
            {
                if (!_labelSelector(item).ToLower().Contains(tSearchString.Text.ToLower())) { continue; }
                lStartingItems.Items.Add(new ListViewItem { Text = _labelSelector(item), Tag = item, Checked = ItemList.Contains(item) });
            }
        }

        private void StartingItemEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void UpdateString(List<Item> selections)
        {
            int[] n = new int[_itemGroupCount];
            string[] ns = new string[_itemGroupCount];
            foreach (var item in selections)
            {
                var i = BaseItemList.IndexOf(item);
                int j = i / 32;
                int k = i % 32;
                n[j] |= (int)(1 << k);
                ns[j] = Convert.ToString(n[j], 16);
            }
            tStartingItemsString.Text = string.Join("-", ns.Reverse());
            ItemListString = tStartingItemsString.Text;
        }

        public void UpdateChecks(string c)
        {
            updating = true;
            try
            {
                tStartingItemsString.Text = c;
                ItemListString = c;
                ItemList.Clear();
                string[] v = c.Split('-');
                int[] vi = new int[_itemGroupCount];
                if (v.Length != vi.Length)
                {
                    ExternalLabel = "Invalid custom starting item string";
                    return;
                }
                for (int i = 0; i < _itemGroupCount; i++)
                {
                    if (v[_itemGroupCount - 1 - i] != "")
                    {
                        vi[i] = Convert.ToInt32(v[_itemGroupCount - 1 - i], 16);
                    }
                }
                for (int i = 0; i < 32 * _itemGroupCount; i++)
                {
                    int j = i / 32;
                    int k = i % 32;
                    if (((vi[j] >> k) & 1) > 0)
                    {
                        if (i >= ItemUtils.AllLocations().Count())
                        {
                            throw new IndexOutOfRangeException();
                        }
                        ItemList.Add(BaseItemList[i]);
                    }
                }
                foreach (ListViewItem l in lStartingItems.Items)
                {
                    if (ItemList.Contains((Item)l.Tag))
                    {
                        l.Checked = true;
                    }
                    else
                    {
                        l.Checked = false;
                    }
                }
                ExternalLabel = $"{ItemList.Count}/{BaseItemList.Count} items randomized";
            }
            catch
            {
                ItemList.Clear();
                ExternalLabel = "Invalid custom starting item string";
            }
            finally
            {
                updating = false;
            }
        }

        private void tStartingItemsString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                UpdateChecks(tStartingItemsString.Text);
            }
        }

        private void lStartingItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (updating)
            {
                return;
            }
            updating = true;
            if (e.Item.Checked)
            {
                ItemList.Add((Item)e.Item.Tag);
            }
            else
            {
                ItemList.Remove((Item)e.Item.Tag);
            }
            UpdateString(ItemList);
            updating = false;
        }

        private void tSearchString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                updating = true;
                lStartingItems.Items.Clear();
                PrintToListView();
                updating = false;
            }
        }
    }
}
