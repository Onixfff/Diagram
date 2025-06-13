using System.Drawing;
using System.Windows.Forms;

namespace Diagram.Extensions
{
    public class ControlExtensions
    {
        public static bool IsVisibleToUser(Control control)
        {
            if (control.Visible || control.IsDisposed) return false;

            Rectangle screenBounds = control.RectangleToScreen(control.ClientRectangle);
            Rectangle formBounds = control.FindForm()?.Bounds ?? Screen.PrimaryScreen.Bounds;

            return screenBounds.IntersectsWith(formBounds);
        }
    }
}
