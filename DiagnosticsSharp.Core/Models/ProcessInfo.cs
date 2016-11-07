using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Core.Models
{
    public class ProcessInfo : IProcessInfo
    {
        public string PID { get; set; }
        public string ProcessName { get; set; }
        public long MemoryUsage { get; set; }
        public ushort CPU { get; set; }
        public string WindowName { get; set; }
        public ushort PercentPrivilegedTime { get; set; }
        public ushort PercentProcessorTime { get; set; }
        public ushort PercentUserTime { get; set; }
        public ushort PriorityBase { get; set; }
        public long ThreadCount { get; set; }
        public long PageFaultsPersec { get; set; }
        public long HandleCount { get; set; }
        public string CreatingProcessID { get; set; }

    }
}
