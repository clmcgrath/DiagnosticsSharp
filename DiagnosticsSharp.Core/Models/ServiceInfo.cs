using System.ServiceProcess;
using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Core.Models
{
    public class ServiceInfo : IServiceInfo
    {
        public string ServiceName { get; set; }
        public string DisplayName { get; set; }
        public ServiceControllerStatus Status { get; set; }
        public ServiceType ServiceType { get; set; }
    }
}