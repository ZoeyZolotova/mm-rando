using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace MMR.UI.Utils
{
    [SupportedOSPlatform("windows")]
    public static class FormUtils
    {
        /// <summary>
        /// Find the control located at a specific <see cref="Point"/> relative to a container <see cref="Control"/>.
        /// </summary>
        /// <param name="container">Container control</param>
        /// <param name="pos">Point</param>
        /// <see>https://stackoverflow.com/a/16543294</see>
        /// <returns>Control at point, or null if none</returns>
        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Find the control located at the cursor.
        /// </summary>
        /// <param name="form">Form</param>
        /// <see>https://stackoverflow.com/a/16543294</see>
        /// <returns>Control at point, or null if none</returns>
        public static Control FindControlAtCursor(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(pos));
            return null;
        }
    }
}
