using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MMR.UI.Controls
{
    [SupportedOSPlatform("windows")]
    internal class InvertIndeterminateCheckBox : CheckBox
    {
        protected override void OnClick(EventArgs e)
        {
            CheckState = CheckState switch
            {
                CheckState.Checked => CheckState.Unchecked,
                CheckState.Unchecked => CheckState.Checked,
                CheckState.Indeterminate => CheckState.Checked,
                _ => CheckState,
            };
        }
    }
}
