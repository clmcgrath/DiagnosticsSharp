using System.Drawing;

namespace DiagnosticsSharp.Core.Interfaces
{
    public interface IScreenInfo 
    {
        bool Primary { get; set; }
        Rectangle Bounds { get; set; }
        string DeviceName { get; set; }

    }
}
