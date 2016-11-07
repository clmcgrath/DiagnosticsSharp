using System;

namespace DiagnosticsSharp.Core.Interfaces
{
    public interface IDeviceInfo
    {
        string Caption { get; set; }
        string Description { get; set; }
        string StatusInfo { get; set; }
        string Manufacturer { get; set; }
        string DeviceId { get; set; }
        string DeviceType { get; set; }
        string DeviceName { get; set; }
        string Status { get; set; }

        Guid DeviceGuid { get; set; }

    }
}