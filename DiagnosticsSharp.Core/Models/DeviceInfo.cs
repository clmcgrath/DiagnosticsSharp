using System;
using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Core.Models
{
    public class DeviceInfo: IDeviceInfo
    {
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string Status { get; set; }
        public Guid DeviceGuid { get; set; }
        public string DeviceId { get; set; }
        public string Manufacturer { get; set; }
        public string StatusInfo { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
    }
}