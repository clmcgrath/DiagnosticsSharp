namespace DiagnosticsSharp.Core.Interfaces
{
    public interface IProcessInfo
    {
        ushort PriorityBase { get; set; }
        long ThreadCount { get; set; }
        long PageFaultsPersec { get; set; }
        long HandleCount { get; set; }
        string CreatingProcessID { get; set; }
        ushort PercentUserTime { get; set; }
        ushort PercentPrivilegedTime { get; set; }
        ushort CPU { get; set; }
        string WindowName { get; set; }
        string PID { get; set; }
        string ProcessName { get; set; }
        long MemoryUsage { get; set; }

    }
}