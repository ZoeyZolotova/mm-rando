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
    public partial class ManualForm : Form
    {
        public ManualForm()
        {
            InitializeComponent();
        }

        private void fManual_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            };
        }
    }
}
