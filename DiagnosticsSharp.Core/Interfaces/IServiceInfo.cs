using System.ServiceProcess;

namespace DiagnosticsSharp.Core.Interfaces
{
    public interface IServiceInfo
    {
        ServiceType ServiceType { get; set; }
        ServiceControllerStatus Status { get; set; }
        string DisplayName { get; set; }
        string ServiceName { get; set; }
    }
}