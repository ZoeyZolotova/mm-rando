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
    public partial class StartingItemEditForm : Form
    {
        private readonly List<Item> _startingItems;
        private bool updating = false;
        private const int ItemGroupCount = 3;

        public string ExternalLabel { get; private set; }
        public List<Item> CustomStartingItemList { get; private set; } = new List<Item>();
        public string CustomStartingItemListString { get; private set; }

        public StartingItemEditForm()
        {
            InitializeComponent();

            _startingItems = ItemUtils.StartingItems().Where(item => !item.Name().Contains("Heart")).ToList();

            PrintToListView();

            if (CustomStartingItemList != null)
            {
                UpdateString(CustomStartingItemList);
                ExternalLabel = $"{CustomStartingItemList.Count}/{_startingItems.Count} items selected";
            }
            else
            {
                tStartingItemsString.Text = "--";
            }
        }

        private void PrintToListView()
        {
            foreach (var item in _startingItems)
            {
                if (!item.Name().ToLower().Contains(tSearchString.Text.ToLower())) { continue; }
                lStartingItems.Items.Add(new ListViewItem { Text = item.Name(), Tag = item, Checked = CustomStartingItemList.Contains(item) });
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
            int[] n = new int[3];
            string[] ns = new string[3];
            foreach (var item in selections)
            {
                var i = _startingItems.IndexOf(item);
                int j = i / 32;
                int k = i % 32;
                n[j] |= (int)(1 << k);
                ns[j] = Convert.ToString(n[j], 16);
            }
            tStartingItemsString.Text = ns[2] + "-" + ns[1] + "-" + ns[0];
            CustomStartingItemListString = tStartingItemsString.Text;
        }

        public void UpdateChecks(string c)
        {
            updating = true;
            try
            {
                tStartingItemsString.Text = c;
                CustomStartingItemListString = c;
                CustomStartingItemList.Clear();
                string[] v = c.Split('-');
                int[] vi = new int[ItemGroupCount];
                if (v.Length != vi.Length)
                {
                    ExternalLabel = "Invalid custom starting item string";
                    return;
                }
                for (int i = 0; i < ItemGroupCount; i++)
                {
                    if (v[ItemGroupCount - 1 - i] != "")
                    {
                        vi[i] = Convert.ToInt32(v[ItemGroupCount - 1 - i], 16);
                    }
                }
                for (int i = 0; i < 32 * ItemGroupCount; i++)
                {
                    int j = i / 32;
                    int k = i % 32;
                    if (((vi[j] >> k) & 1) > 0)
                    {
                        if (i >= ItemUtils.AllLocations().Count())
                        {
                            throw new IndexOutOfRangeException();
                        }
                        CustomStartingItemList.Add(_startingItems[i]);
                    }
                }
                foreach (ListViewItem l in lStartingItems.Items)
                {
                    if (CustomStartingItemList.Contains((Item)l.Tag))
                    {
                        l.Checked = true;
                    }
                    else
                    {
                        l.Checked = false;
                    }
                }
                ExternalLabel = $"{CustomStartingItemList.Count}/{_startingItems.Count} items selected";
            }
            catch
            {
                CustomStartingItemList.Clear();
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
                CustomStartingItemList.Add((Item)e.Item.Tag);
            }
            else
            {
                CustomStartingItemList.Remove((Item)e.Item.Tag);
            }
            UpdateString(CustomStartingItemList);
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
