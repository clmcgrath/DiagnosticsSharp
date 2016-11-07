using System.Drawing;
using System.Windows.Forms;
using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Core.Models
{
    public static class HelperExtensions
    {
        public static IScreenInfo ScreenInfo(this Screen screen)
        {
            return new ScreenInfo(screen);
        }
    }

    public class ScreenInfo : IScreenInfo
    {
        private readonly Screen _screen;

        public ScreenInfo(Screen screen)
        {
            _screen = screen;
            Primary = screen.Primary;
            Bounds = screen.Bounds;
            DeviceName = screen.DeviceName;
        }

        public bool Primary { get; set; }
        public Rectangle Bounds { get; set; }
        public string DeviceName { get; set; }
    }
}
