using System.Drawing;
using System.Windows.Forms;

namespace Diagram.Extensions
{
    public static class ControlExtensions
    {
        public static bool IsVisibleToUser(this Control control)
        {
            if (control == null || !control.Visible || control.IsDisposed) return false;

            Rectangle screenBounds = control.RectangleToScreen(control.ClientRectangle);
            Rectangle formBounds = control.FindForm()?.Bounds ?? Screen.PrimaryScreen.Bounds;

            return screenBounds.IntersectsWith(formBounds);
        }
    }
}
