using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Core.Models
{
    class DisplayAdapter : IDisplayAdapter
    {
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string DriverVersion { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
    }
}