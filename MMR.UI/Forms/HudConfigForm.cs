using MMR.Randomizer.Asm;
using MMR.Randomizer.Utils;
using MMR.UI.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MMR.UI.Forms
{
    public partial class HudConfigForm : Form
    {
        /// <summary>
        /// Dialog for selecting colors.
        /// </summary>
        ColorDialog ColorDialog { get; set; } = new ColorDialog();

        /// <summary>
        /// Default HUD colors.
        /// </summary>
        HudColors DefaultColors { get; set; } = new HudColors();

        /// <summary>
        /// Randomness provider.
        /// </summary>
        Random Random { get; set; } = new Random();

        /// <summary>
        /// Color controls.
        /// </summary>
        ColorControl[] ColorControls { get; set; }

        /// <summary>
        /// The <see cref="Control"/> currently selected using the <see cref="ContextMenuStrip"/>.
        /// </summary>
        Control ContextSelected { get; set; }

        public HudConfigForm()
        {
            InitializeComponent();
            InitializeColorDialog();
            InitializeContextMenu();

            this.ColorControls = GetColorControls();
        }

        /// <summary>
        /// Initialize the <see cref="System.Windows.Forms.ColorDialog"/>.
        /// </summary>
        void InitializeColorDialog()
        {
            var dialog = this.ColorDialog;
            dialog.AllowFullOpen = true;
        }

        /// <summary>
        /// Update the current colors and show as a dialog.
        /// </summary>
        /// <param name="owner">Owner</param>
        /// <param name="config">Config</param>
        /// <returns>Result</returns>
        public DialogResult ShowDialog(IWin32Window owner, HudColorsConfig config)
        {
            FromColors(config.Colors);
            return ShowDialog(owner);
        }

        /// <summary>
        /// Update the currently selected colors.
        /// </summary>
        /// <param name="colors">Colors</param>
        public void Update(HudColors colors)
        {
            FromColors(colors);
        }

        /// <summary>
        /// Update all component colors from <see cref="HudColors"/>.
        /// </summary>
        /// <param name="colors">Colors</param>
        void FromColors(HudColors colors)
        {
            btn_a.BackColor = colors.ButtonA;
            btn_b.BackColor = colors.ButtonB;
            btn_c.BackColor = colors.ButtonC;
            btn_start.BackColor = colors.ButtonStart;
            btn_clockemblem.BackColor = colors.ClockEmblem;
            btn_inverted.BackColor = colors.ClockEmblemInverted1;
            btn_inverted2.BackColor = colors.ClockEmblemInverted2;
            btn_clockminutes.BackColor = colors.ClockEmblemSun;
            btn_clockmoon.ForeColor = colors.ClockMoon;
            btn_clocksun.ForeColor = colors.ClockSun;
            btn_hearts.ForeColor = colors.Heart;
            btn_hearts2.ForeColor = colors.HeartDD;
            btn_magic.BackColor = colors.Magic;
            btn_chateau.BackColor = colors.MagicInf;
            btn_map.BackColor = colors.Map;
            btn_mapentrance.ForeColor = colors.MapEntranceCursor;
            btn_mapplayer.ForeColor = colors.MapPlayerCursor;
            btn_wallet99.BackColor = colors.RupeeIcon1;
            btn_wallet200.BackColor = colors.RupeeIcon2;
            btn_wallet500.BackColor = colors.RupeeIcon3;
            btn_wallet999.BackColor = colors.RupeeIcon4;
            btn_dpad.BackColor = colors.DPad;
        }

        /// <summary>
        /// Get all component colors as <see cref="HudColors"/>.
        /// </summary>
        /// <returns>HUD colors</returns>
        public HudColors ToColors()
        {
            var colors = new HudColors();
            colors.ButtonA = btn_a.BackColor;
            colors.ButtonB = btn_b.BackColor;
            colors.ButtonC = btn_c.BackColor;
            colors.ButtonStart = btn_start.BackColor;
            colors.ClockEmblem = btn_clockemblem.BackColor;
            colors.ClockEmblemInverted1 = btn_inverted.BackColor;
            colors.ClockEmblemInverted2 = btn_inverted2.BackColor;
            colors.ClockEmblemSun = btn_clockminutes.BackColor;
            colors.ClockMoon = btn_clockmoon.ForeColor;
            colors.ClockSun = btn_clocksun.ForeColor;
            colors.Heart = btn_hearts.ForeColor;
            colors.HeartDD = btn_hearts2.ForeColor;
            colors.Magic = btn_magic.BackColor;
            colors.MagicInf = btn_chateau.BackColor;
            colors.Map = btn_map.BackColor;
            colors.MapEntranceCursor = btn_mapentrance.ForeColor;
            colors.MapPlayerCursor = btn_mapplayer.ForeColor;
            colors.RupeeIcon1 = btn_wallet99.BackColor;
            colors.RupeeIcon2 = btn_wallet200.BackColor;
            colors.RupeeIcon3 = btn_wallet500.BackColor;
            colors.RupeeIcon4 = btn_wallet999.BackColor;
            colors.DPad = btn_dpad.BackColor;
            return colors;
        }

        /// <summary>
        /// Show the <see cref="System.Windows.Forms.ColorDialog"/> to select the color for a specific <see cref="Control"/>.
        /// </summary>
        /// <param name="control">Control</param>
        private void SelectColor(Control control)
        {
            var dialog = this.ColorDialog;
            dialog.Color = GetControlColor(control);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                SetControlColor(control, dialog.Color);
            }
        }

        #region Button Events

        private void btn_a_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_b_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_c_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_clockemblem_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_inverted_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_inverted2_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_clockminutes_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_clockmoon_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_clocksun_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_hearts_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_hearts2_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_magic_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_chateau_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_map_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_mapentrance_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_mapplayer_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_wallet99_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_wallet200_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_wallet500_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_wallet999_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        private void btn_dpad_Click(object sender, EventArgs e)
        {
            SelectColor((Button)sender);
        }

        #endregion

        #region ColorControl

        /// <summary>
        /// Color mode which describes how to get or set the color of a control.
        /// </summary>
        enum ColorMode
        {
            Back,
            Fore,
        }

        /// <summary>
        /// Abstraction used to conveniently get, set or reset the color of controls.
        /// </summary>
        class ColorControl
        {
            public Control Control { get; private set; }
            public ColorMode Mode { get; private set; }
            public Color Default { get; private set; }

            public ColorControl(Control control, ColorMode mode, Color defaultColor)
            {
                this.Control = control;
                this.Mode = mode;
                this.Default = defaultColor;
            }

            /// <summary>
            /// Get the control's color.
            /// </summary>
            /// <returns>Color</returns>
            public Color GetControlColor()
            {
                if (this.Mode == ColorMode.Back)
                    return this.Control.BackColor;
                else if (this.Mode == ColorMode.Fore)
                    return this.Control.ForeColor;
                else
                    throw new Exception("Unknown ColorMode value");
            }

            /// <summary>
            /// Reset the control's color to default.
            /// </summary>
            public void ResetControlColor()
            {
                this.SetControlColor(this.Default);
            }

            /// <summary>
            /// Set the control's color.
            /// </summary>
            /// <param name="color">Color</param>
            public void SetControlColor(Color color)
            {
                if (this.Mode == ColorMode.Back)
                    this.Control.BackColor = color;
                else if (this.Mode == ColorMode.Fore)
                    this.Control.ForeColor = color;
            }
        }

        #endregion

        #region Context Menu

        /// <summary>
        /// Initialize the <see cref="ContextMenuStrip"/>.
        /// </summary>
        void InitializeContextMenu()
        {
            var menu = ctxtMenu;
            menu.Opening += new CancelEventHandler(ctxtMenu_Opening);
        }

        void ctxtMenu_Opening(object sender, CancelEventArgs e)
        {
            var menu = ctxtMenu;
            menu.Items.Clear();

            var randomizeSubMenu = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("Buttons", null, ctxtMenu_RandomizeButtons),
                new ToolStripMenuItem("Clock Emblem", null, ctxtMenu_RandomizeClockEmblem),
                new ToolStripMenuItem("Clock Misc", null, ctxtMenu_RandomizeClockMisc),
                new ToolStripMenuItem("Hearts", null, ctxtMenu_RandomizeHearts),
                new ToolStripMenuItem("Magic", null, ctxtMenu_RandomizeMagic),
                new ToolStripMenuItem("Map", null, ctxtMenu_RandomizeMap),
                new ToolStripMenuItem("Rupees", null, ctxtMenu_RandomizeRupees),
            };

            var resetSubMenu = new ToolStripMenuItem[]
            {
                new ToolStripMenuItem("Buttons", null, ctxtMenu_ResetButtons),
                new ToolStripMenuItem("Clock Emblem", null, ctxtMenu_ResetClockEmblem),
                new ToolStripMenuItem("Clock Misc", null, ctxtMenu_ResetClockMisc),
                new ToolStripMenuItem("Hearts", null, ctxtMenu_ResetHearts),
                new ToolStripMenuItem("Magic", null, ctxtMenu_ResetMagic),
                new ToolStripMenuItem("Map", null, ctxtMenu_ResetMap),
                new ToolStripMenuItem("Rupees", null, ctxtMenu_ResetRupees),
            };

            var mRandomizeAll = new ToolStripMenuItem("Randomize All", null, ctxtMenu_RandomizeAll);
            var mRandomize = new ToolStripMenuItem("Randomize", null, randomizeSubMenu);
            var mResetAll = new ToolStripMenuItem("Reset All Colors", null, ctxtMenu_ResetAllColors);
            var mReset = new ToolStripMenuItem("Reset", null, resetSubMenu);

            // Check source control
            ToolStripMenuItem mRandomizeCurrent = null;
            ToolStripMenuItem mResetCurrent = null;
            var source = this.ContextSelected = FormUtils.FindControlAtCursor(this);
            if (source != null && GetColorControl(source) != null)
            {
                mRandomizeCurrent = new ToolStripMenuItem("Randomize Current", null, ctxtMenu_RandomizeCurrent);
                mResetCurrent = new ToolStripMenuItem("Reset Current", null, ctxtMenu_ResetCurrent);
            }

            // Build menu
            menu.Items.Add(mRandomizeAll);
            if (mRandomizeCurrent != null)
                menu.Items.Add(mRandomizeCurrent);
            menu.Items.Add(mRandomize);
            menu.Items.Add(new ToolStripSeparator());
            if (mResetCurrent != null)
                menu.Items.Add(mResetCurrent);
            menu.Items.Add(mReset);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(mResetAll);

            e.Cancel = false;
        }

        void ctxtMenu_RandomizeAll(object sender, EventArgs e)
        {
            ctxtMenu_RandomizeButtons(sender, e);
            ctxtMenu_RandomizeClockEmblem(sender, e);
            ctxtMenu_RandomizeClockMisc(sender, e);
            ctxtMenu_RandomizeHearts(sender, e);
            ctxtMenu_RandomizeMagic(sender, e);
            ctxtMenu_RandomizeMap(sender, e);
            ctxtMenu_RandomizeRupees(sender, e);
            RandomizeControlColor(btn_dpad);
        }

        void ctxtMenu_RandomizeCurrent(object sender, EventArgs e)
        {
            var source = this.ContextSelected;
            SetControlColor(source, RandomUtils.GetRandomColor(this.Random));
        }

        void ctxtMenu_RandomizeButtons(object sender, EventArgs e)
        {
            RandomizeControlColor(btn_a, btn_b, btn_c, btn_start);
        }

        void ctxtMenu_RandomizeClockEmblem(object sender, EventArgs e)
        {
            RandomizeControlColor(btn_clockemblem, btn_inverted, btn_inverted2);
        }

        void ctxtMenu_RandomizeClockMisc(object sender, EventArgs e)
        {
            RandomizeControlColor(btn_clockminutes, btn_clockmoon, btn_clocksun);
        }

        void ctxtMenu_RandomizeHearts(object sender, EventArgs e)
        {
            RandomizeControlColor(btn_hearts, btn_hearts2);
        }

        void ctxtMenu_RandomizeMagic(object sender, EventArgs e)
        {
            RandomizeControlColor(btn_magic, btn_chateau);
        }

        void ctxtMenu_RandomizeMap(object sender, EventArgs e)
        {
            RandomizeControlColor(btn_map, btn_mapentrance, btn_mapplayer);
        }

        void ctxtMenu_RandomizeRupees(object sender, EventArgs e)
        {
            RandomizeControlColor(btn_wallet99, btn_wallet200, btn_wallet500, btn_wallet999);
        }

        void ctxtMenu_ResetAllColors(object sender, EventArgs e)
        {
            FromColors(DefaultColors);
        }

        void ctxtMenu_ResetCurrent(object sender, EventArgs e)
        {
            var source = this.ContextSelected;
            ResetControlColor(source);
        }

        void ctxtMenu_ResetButtons(object sender, EventArgs e)
        {
            ResetControlColor(btn_a, btn_b, btn_c, btn_start);
        }

        void ctxtMenu_ResetClockEmblem(object sender, EventArgs e)
        {
            ResetControlColor(btn_clockemblem, btn_inverted, btn_inverted2);
        }

        void ctxtMenu_ResetClockMisc(object sender, EventArgs e)
        {
            ResetControlColor(btn_clockminutes, btn_clockmoon, btn_clocksun);
        }

        void ctxtMenu_ResetHearts(object sender, EventArgs e)
        {
            ResetControlColor(btn_hearts, btn_hearts2);
        }

        void ctxtMenu_ResetMagic(object sender, EventArgs e)
        {
            ResetControlColor(btn_magic, btn_chateau);
        }

        void ctxtMenu_ResetMap(object sender, EventArgs e)
        {
            ResetControlColor(btn_map, btn_mapentrance, btn_mapplayer);
        }

        void ctxtMenu_ResetRupees(object sender, EventArgs e)
        {
            ResetControlColor(btn_wallet99, btn_wallet200, btn_wallet500, btn_wallet999);
        }

        #endregion

        #region Form ColorControl Methods

        /// <summary>
        /// Get all color-able controls as <see cref="ColorControl"/> items.
        /// </summary>
        /// <remarks>Uses the same order as <see cref="HudColors"/></remarks>
        /// <returns>Controls</returns>
        ColorControl[] GetColorControls()
        {
            var defaultColors = this.DefaultColors;
            return new ColorControl[]
            {
                new ColorControl(btn_a, ColorMode.Back, defaultColors.ButtonA),
                new ColorControl(btn_b, ColorMode.Back, defaultColors.ButtonB),
                new ColorControl(btn_c, ColorMode.Back, defaultColors.ButtonC),
                new ColorControl(btn_start, ColorMode.Back, defaultColors.ButtonStart),
                new ColorControl(btn_clockemblem, ColorMode.Back, defaultColors.ClockEmblem),
                new ColorControl(btn_inverted, ColorMode.Back, defaultColors.ClockEmblemInverted1),
                new ColorControl(btn_inverted2, ColorMode.Back, defaultColors.ClockEmblemInverted2),
                new ColorControl(btn_clockminutes, ColorMode.Back, defaultColors.ClockEmblemSun),
                new ColorControl(btn_clockmoon, ColorMode.Fore, defaultColors.ClockMoon),
                new ColorControl(btn_clocksun, ColorMode.Fore, defaultColors.ClockSun),
                new ColorControl(btn_hearts, ColorMode.Fore, defaultColors.Heart),
                new ColorControl(btn_hearts2, ColorMode.Fore, defaultColors.HeartDD),
                new ColorControl(btn_magic, ColorMode.Back, defaultColors.Magic),
                new ColorControl(btn_chateau, ColorMode.Back, defaultColors.MagicInf),
                new ColorControl(btn_map, ColorMode.Back, defaultColors.Map),
                new ColorControl(btn_mapentrance, ColorMode.Fore, defaultColors.MapEntranceCursor),
                new ColorControl(btn_mapplayer, ColorMode.Fore, defaultColors.MapPlayerCursor),
                new ColorControl(btn_wallet99, ColorMode.Back, defaultColors.RupeeIcon1),
                new ColorControl(btn_wallet200, ColorMode.Back, defaultColors.RupeeIcon2),
                new ColorControl(btn_wallet500, ColorMode.Back, defaultColors.RupeeIcon3),
                new ColorControl(btn_wallet999, ColorMode.Back, defaultColors.RupeeIcon4),
                new ColorControl(btn_dpad, ColorMode.Back, defaultColors.DPad),
            };
        }

        /// <summary>
        /// Get the <see cref="ColorControl"/> corresponding to a specific <see cref="Control"/>.
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>ColorControl</returns>
        ColorControl GetColorControl(Control control)
        {
            foreach (var c in this.ColorControls)
                if (c.Control == control)
                    return c;
            return null;
        }

        Color GetControlColor(Control control)
        {
            var c = GetColorControl(control);
            return c.GetControlColor();
        }

        void RandomizeControlColor(params Control[] controls)
        {
            foreach (var control in controls)
            {
                var c = GetColorControl(control);
                var color = RandomUtils.GetRandomColor(this.Random);
                c.SetControlColor(color);
            }
        }

        void ResetControlColor(params Control[] controls)
        {
            foreach (var control in controls)
            {
                var c = GetColorControl(control);
                c.ResetControlColor();
            }
        }

        void SetControlColor(Control control, Color color)
        {
            var c = GetColorControl(control);
            c.SetControlColor(color);
        }

        #endregion
    }
}
