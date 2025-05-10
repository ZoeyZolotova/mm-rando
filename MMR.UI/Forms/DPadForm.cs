using MMR.Randomizer.Asm;
using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MMR.UI.Forms
{
    /// <summary>
    /// <see cref="Form"/> for configuring the <see cref="DPad"/>.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class DPadForm : Form
    {
        private DPadValue[] _custom;
        private DPadItem[] _items;
        private DPadPreset[] _presets;

        /// <summary>
        /// Get the selected <see cref="DPadConfig"/>.
        /// </summary>
        public DPadConfig Config => new DPadConfig(Selected, State, Display);

        /// <summary>
        /// Get the <see cref="DPad"/> from current custom values.
        /// </summary>
        public DPad Custom => new DPad(_custom);

        /// <summary>
        /// Get whether or not the DPad is enabled.
        /// </summary>
        public bool DPadEnabled => EnabledCheckBox.Checked;

        /// <summary>
        /// Whether or not custom values are currently selected.
        /// </summary>
        public bool IsCustomSelected => !IsPresetSelected;

        /// <summary>
        /// Whether or not a <see cref="DPadPreset"/> is currently selected.
        /// </summary>
        public bool IsPresetSelected => PresetComboBox.SelectedItem is DPadPreset;

        /// <summary>
        /// All <see cref="ComboBox"/> controls relating to <see cref="DPadItem"/> values.
        /// </summary>
        public ComboBox[] ItemComboBoxes => new ComboBox[]
        {
            DPadItemUpComboBox,
            DPadItemRightComboBox,
            DPadItemDownComboBox,
            DPadItemLeftComboBox,
        };

        /// <summary>
        /// Get the <see cref="DPad"/> from selected values.
        /// </summary>
        public DPad Selected {
            get {
                var selected = PresetComboBox.SelectedItem;
                if (selected is DPadPreset)
                    return ((DPadPreset)selected).Pad;
                else
                    return this.Custom;
            }
        }

        public DPadDisplay Display {
            get {
                if (DPadHideCheckBox.Checked)
                    return DPadDisplay.None;
                else if (DPadDisplayLeftRadioButton.Checked)
                    return DPadDisplay.Left;
                else
                    return DPadDisplay.Right;
            }
        }

        /// <summary>
        /// Get the selected <see cref="DPadState"/>.
        /// </summary>
        public DPadState State => DPadEnabled ? DPadState.Enabled : DPadState.Disabled;

        public DPadForm(DPadPreset[] presets, DPadItem[] items, DPadConfig config)
        {
            _presets = presets;
            _items = items;
            _custom = config.Pad.Values;

            InitializeComponent();
            Initialize(config);
        }

        /// <summary>
        /// Infer which <see cref="DPadPreset"/> is selected from a pre-existing <see cref="DPad"/>.
        /// </summary>
        /// <returns><see cref="DPadPreset"/>, or null if none found</returns>
        private DPadPreset InferPreset()
        {
            foreach (var preset in _presets)
                if (this.Custom.Equals(preset.Pad))
                    return preset;
            return null;
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="config">Configuration</param>
        private void Initialize(DPadConfig config)
        {
            // Initialize presets
            foreach (var preset in _presets)
                PresetComboBox.Items.Add(preset);
            PresetComboBox.Items.Add("Custom");

            // Initialize items
            foreach (var combo in ItemComboBoxes)
                foreach (var item in _items)
                    combo.Items.Add(item);

            // Try to infer which preset to select based on current values
            var inferred = InferPreset();
            if (inferred != null)
                PresetComboBox.SelectedItem = inferred;
            else
                PresetComboBox.SelectedItem = "Custom";

            // Set enabled/disabled based on current state
            if (config.State == DPadState.Enabled)
                EnabledCheckBox.Checked = true;
            else
                EnabledCheckBox.Checked = false;

            // Initialize the display groupbox
            if (config.Display != DPadDisplay.Right)
                DPadDisplayLeftRadioButton.Checked = true;
            else
                DPadDisplayRightRadioButton.Checked = true;
            DPadHideCheckBox.Checked = (config.Display == DPadDisplay.None);

            UpdatePresetComboBox();
            UpdateItemComboBoxes();
            UpdateDisplayGroupBox();
        }

        #region Forms Code

        private void FillItemComboBoxes(ComboBox[] combos, DPadValue[] values, bool enable)
        {
            for (var i = 0; i < combos.Length; i++)
            {
                combos[i].SelectedItem = DPadItem.From(values[i]);
                combos[i].Enabled = enable;
            }
        }

        private void OnItemComboBoxChanged(ComboBox combo, int index)
        {
            if (IsCustomSelected)
            {
                var item = (DPadItem)combo.SelectedItem;
                _custom[index] = item.Value;
            }
        }

        private void UpdateDisplayGroupBox()
        {
            var buttons = new RadioButton[]
            {
                DPadDisplayLeftRadioButton,
                DPadDisplayRightRadioButton,
            };

            bool stateEnabled = EnabledCheckBox.Checked;

            // Update radio buttons to be enabled/disabled depending on if "hide" is checked
            bool displayEnabled = !DPadHideCheckBox.Checked && stateEnabled;
            foreach (var button in buttons)
                button.Enabled = displayEnabled;

            DPadHideCheckBox.Enabled = stateEnabled;
        }

        private void UpdateItemComboBoxes()
        {
            var selected = PresetComboBox.SelectedItem;
            if (selected is DPadPreset)
            {
                // Fill ComboBox values based on preset
                var preset = (DPadPreset)selected;
                FillItemComboBoxes(ItemComboBoxes, preset.Pad.Values, false);
            }
            else
            {
                // Show custom-selected values
                FillItemComboBoxes(ItemComboBoxes, _custom, EnabledCheckBox.Checked);
            }
        }

        private void UpdatePresetComboBox()
        {
            PresetComboBox.Enabled = EnabledCheckBox.Checked;
        }

        private void PresetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateItemComboBoxes();
        }

        private void DPadItemUpComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnItemComboBoxChanged((ComboBox)sender, 0);
        }

        private void DPadItemRightComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnItemComboBoxChanged((ComboBox)sender, 1);
        }

        private void DPadItemDownComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnItemComboBoxChanged((ComboBox)sender, 2);
        }

        private void DPadItemLeftComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnItemComboBoxChanged((ComboBox)sender, 3);
        }

        private void EnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePresetComboBox();
            UpdateItemComboBoxes();
            UpdateDisplayGroupBox();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DPadHideCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDisplayGroupBox();
        }
        #endregion
    }
}
