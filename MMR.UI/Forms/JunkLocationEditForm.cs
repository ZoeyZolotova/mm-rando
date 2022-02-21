using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Utils;
using MMR.Randomizer.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MMR.Randomizer.Extensions;

namespace MMR.UI.Forms
{
    public partial class JunkLocationEditForm : Form
    {
        private readonly List<Item> _junkLocations;
        private bool updating = false;
        private const int ItemGroupCount = 33;

        public string ExternalLabel { get; private set; }
        public List<Item> CustomJunkLocations { get; private set; } = new List<Item>();
        public string CustomJunkLocationsString { get; private set; }

        public JunkLocationEditForm()
        {
            InitializeComponent();

            _junkLocations = ItemUtils.AllLocations().ToList();

            PrintToListView();

            if (CustomJunkLocations != null)
            {
                UpdateString(CustomJunkLocations);
                ExternalLabel = $"{CustomJunkLocations.Count}/{_junkLocations.Count} items selected";
            }
            else
            {
                tJunkLocationsString.Text = "--";
            }
        }

        private void PrintToListView()
        {
            foreach (var item in _junkLocations)
            {
                if (!item.Location().ToLower().Contains(tSearchString.Text.ToLower())) { continue; }
                lJunkLocations.Items.Add(new ListViewItem { Text = item.Location(), Tag = item, Checked = CustomJunkLocations.Contains(item) });
            }
        }

        private void JunkLocationEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void UpdateString(List<Item> selections)
        {
            int[] n = new int[ItemGroupCount];
            string[] ns = new string[ItemGroupCount];
            foreach (var item in selections)
            {
                var i = _junkLocations.IndexOf(item);
                int j = i / 32;
                int k = i % 32;
                n[j] |= (int)(1 << k);
                ns[j] = Convert.ToString(n[j], 16);
            }

            tJunkLocationsString.Text = string.Join("-", ns.Reverse());
            CustomJunkLocationsString = tJunkLocationsString.Text;
        }

        public void UpdateChecks(string c)
        {
            updating = true;
            try
            {
                tJunkLocationsString.Text = c;
                CustomJunkLocationsString = c;
                CustomJunkLocations.Clear();
                string[] v = c.Split('-');
                int[] vi = new int[ItemGroupCount];
                if (v.Length != vi.Length)
                {
                    ExternalLabel = "Invalid junk locations string";
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
                        CustomJunkLocations.Add(_junkLocations[i]);
                    }
                }
                foreach (ListViewItem l in lJunkLocations.Items)
                {
                    if (CustomJunkLocations.Contains((Item)l.Tag))
                    {
                        l.Checked = true;
                    }
                    else
                    {
                        l.Checked = false;
                    }
                }
                ExternalLabel = $"{CustomJunkLocations.Count}/{_junkLocations.Count} locations selected";
            }
            catch
            {
                CustomJunkLocations.Clear();
                ExternalLabel = "Invalid junk locations string";
            }
            finally
            {
                updating = false;
            }
        }

        private void tJunkLocationsString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                UpdateChecks(tJunkLocationsString.Text);
            }
        }

        private void lJunkLocations_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (updating)
            {
                return;
            }
            updating = true;
            if (e.Item.Checked)
            {
                CustomJunkLocations.Add((Item)e.Item.Tag);
            }
            else
            {
                CustomJunkLocations.Remove((Item)e.Item.Tag);
            }
            UpdateString(CustomJunkLocations);
            updating = false;
        }

        private void tSearchString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                updating = true;
                lJunkLocations.Items.Clear();
                PrintToListView();
                updating = false;
            }
        }
    }
}
