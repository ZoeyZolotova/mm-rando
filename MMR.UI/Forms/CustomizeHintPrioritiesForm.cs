using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Models;
using MMR.Randomizer.Utils;
using MMR.UI.Forms.Tooltips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;

namespace MMR.UI.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class CustomizeHintPrioritiesForm : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<List<Item>> Result { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HashSet<int> ResultTiersIndicateImportance { get; private set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<int> ResultTiersCap { get; private set; }

        public CustomizeHintPrioritiesForm(IEnumerable<List<Item>> overrideHintPriorities, IEnumerable<int> tiersIndicateImportance, IEnumerable<int> tiersCap)
        {
            InitializeComponent();

            tHintPriorities.MaximumSize = new Size(panel1.Width - SystemInformation.VerticalScrollBarWidth, 0);

            Result = overrideHintPriorities?.ToList() ?? new List<List<Item>>();
            ResultTiersIndicateImportance = tiersIndicateImportance?.ToHashSet() ?? new HashSet<int>();
            ResultTiersCap = Enumerable.Repeat(0, Result.Count).ToList();

            if (Result != null)
            {
                for (var i = 0; i < Result.Count; i++)
                {
                    var list = Result[i];
                    ResultTiersCap[i] = tiersCap?.ElementAtOrDefault(i) ?? 0;
                    AddItems(list, ResultTiersIndicateImportance.Contains(i), ResultTiersCap[i]);
                }
            }
        }

        private void AddItems(IEnumerable<Item> items, bool indicateImportance, int cap)
        {
            var i = tHintPriorities.RowCount - 1;
            tHintPriorities.RowCount++;

            foreach (Control c in tHintPriorities.Controls)
            {
                if (tHintPriorities.GetRow(c) == i)
                {
                    tHintPriorities.SetRow(c, i + 1);
                }
            }

            var editButton = new Button
            {
                Text = "+",
            };
            editButton.Click += editButton_Click;
            tHintPriorities.Controls.Add(editButton, 0, i);

            var deleteButton = new Button
            {
                Text = "-",
            };
            deleteButton.Click += deleteButton_Click;
            tHintPriorities.Controls.Add(deleteButton, 1, i);

            var indicateImportanceCheckbox = new CheckBox
            {
                Checked = indicateImportance,
                Dock = DockStyle.Top,
                Text = string.Empty,
                CheckAlign = ContentAlignment.MiddleCenter,
            };
            indicateImportanceCheckbox.CheckedChanged += indicateImportanceCheckbox_CheckedChanged;
            tHintPriorities.Controls.Add(indicateImportanceCheckbox, 2, i);

            var label = new Label
            {
                Text = string.Join(", ", items.Select(item => item.Location())),
                AutoSize = true,
                Margin = new Padding(10, 10, 10, 10),
            };
            tHintPriorities.Controls.Add(label, 3, i);

            var capInput = new TextBox
            {
                Text = cap.ToString(),
                AutoSize = true,
            };
            capInput.TextChanged += textCap_TextChanged;
            tHintPriorities.Controls.Add(capInput, 4, i);

            var upButton = new Button
            {
                Text = "^",
            };
            upButton.Click += upButton_Click;
            tHintPriorities.Controls.Add(upButton, 5, i);

            var downButton = new Button
            {
                Text = "v",
            };
            downButton.Click += downButton_Click;
            tHintPriorities.Controls.Add(downButton, 6, i);
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var index = tHintPriorities.GetRow(control);
            if (index > 0)
            {
                tHintPriorities.SuspendLayout();
                var list = Result[index];
                Result.Remove(list);
                Result.Insert(index - 1, list);
                var importances = new List<bool>();
                for (var i = 0; i < Result.Count; i++)
                {
                    importances.Add(ResultTiersIndicateImportance.Contains(i));
                }
                var importance = importances[index];
                importances.RemoveAt(index);
                importances.Insert(index - 1, importance);
                ResultTiersIndicateImportance = importances.Select((x, i) => new { x, i }).Where(a => a.x).Select(a => a.i).ToHashSet();
                var cap = ResultTiersCap[index];
                ResultTiersCap.RemoveAt(index);
                ResultTiersCap.Insert(index - 1, cap);
                foreach (Control c in tHintPriorities.Controls)
                {
                    var row = tHintPriorities.GetRow(c);
                    if (row == index - 1)
                    {
                        tHintPriorities.SetRow(c, index);
                    }
                    if (row == index)
                    {
                        tHintPriorities.SetRow(c, index - 1);
                    }
                }
                tHintPriorities.ResumeLayout();
            }
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var index = tHintPriorities.GetRow(control);
            if (index < tHintPriorities.RowCount - 2)
            {
                tHintPriorities.SuspendLayout();
                var list = Result[index];
                Result.Remove(list);
                Result.Insert(index + 1, list);
                var importances = new List<bool>();
                for (var i = 0; i < Result.Count; i++)
                {
                    importances.Add(ResultTiersIndicateImportance.Contains(i));
                }
                var importance = importances[index];
                importances.RemoveAt(index);
                importances.Insert(index + 1, importance);
                ResultTiersIndicateImportance = importances.Select((x, i) => new { x, i }).Where(a => a.x).Select(a => a.i).ToHashSet();
                var cap = ResultTiersCap[index];
                ResultTiersCap.RemoveAt(index);
                ResultTiersCap.Insert(index + 1, cap);
                foreach (Control c in tHintPriorities.Controls)
                {
                    var row = tHintPriorities.GetRow(c);
                    if (row == index)
                    {
                        tHintPriorities.SetRow(c, index + 1);
                    }
                    if (row == index + 1)
                    {
                        tHintPriorities.SetRow(c, index);
                    }
                }
                tHintPriorities.ResumeLayout();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            var control = (Control)sender;
            var index = tHintPriorities.GetRow(control);

            var form = new ItemSelectorForm(ItemUtils.AllLocations(), Result[index]);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                if (form.ReturnItems.Count > 0)
                {
                    Result[index] = form.ReturnItems;
                    var label = (Label)tHintPriorities.GetControlFromPosition(3, index);
                    label.Text = string.Join(", ", Result[index].Select(item => item.Location()));
                }
                else
                {
                    DeleteRow(index);
                }
            }
        }

        private void DeleteRow(int index)
        {
            Result.RemoveAt(index);
            ResultTiersIndicateImportance.Remove(index);
            ResultTiersCap.RemoveAt(index);

            tHintPriorities.SuspendLayout();

            for (var x = 0; x < tHintPriorities.ColumnCount; x++)
            {
                var c = tHintPriorities.GetControlFromPosition(x, index);
                tHintPriorities.Controls.Remove(c);
            }

            for (var y = index + 1; y < tHintPriorities.RowCount; y++)
            {
                for (var x = 0; x < tHintPriorities.ColumnCount; x++)
                {
                    var c = tHintPriorities.GetControlFromPosition(x, y);
                    if (c != null)
                    {
                        tHintPriorities.SetRow(c, y - 1);
                    }
                }
            }

            tHintPriorities.RowCount--;

            tHintPriorities.ResumeLayout();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this row?", "Confirm", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                var control = (Control)sender;
                var index = tHintPriorities.GetRow(control);

                DeleteRow(index);
            }
        }

        private void indicateImportanceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var index = tHintPriorities.GetRow(checkBox);
            if (checkBox.Checked)
            {
                ResultTiersIndicateImportance.Add(index);
            }
            else
            {
                ResultTiersIndicateImportance.Remove(index);
            }
        }

        private void textCap_TextChanged(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            var index = tHintPriorities.GetRow(textBox);
            if (int.TryParse(textBox.Text, out var cap))
            {
                ResultTiersCap[index] = cap;
            }
            else
            {
                ResultTiersCap[index] = 0;
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (Result?.Count == 0)
            {
                Result = null;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bAddLevel_Click(object sender, EventArgs e)
        {
            var form = new ItemSelectorForm(ItemUtils.AllLocations(), Enumerable.Empty<Item>());
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK && form.ReturnItems.Count > 0)
            {
                Result.Add(form.ReturnItems);
                ResultTiersCap.Add(0);

                tHintPriorities.SuspendLayout();
                AddItems(form.ReturnItems, false, 0);
                tHintPriorities.ResumeLayout();
            }
        }
    }
}
