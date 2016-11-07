using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace DiagnosticsSharp.Core.Interfaces
{
    public interface IMachineInfoService
    {
        IEnumerable<FileSystemInfo> GetFileList(string path);

        string OperatingSystemName { get; set; }
        string OperatingSystemArchitecture { get; set; }
        string OperatingSystemServicePack { get; set; }
        double TotalRam { get; set; }
        string Processor { get; set; }
        string MachineName { get; set; }
        string UserDomain { get; set; }
        string Username { get; set; }
        IEnumerable<IProcessInfo> Processes { get; set; }
        IEnumerable<IDeviceInfo> InstalledDevices{ get; set; }
        IEnumerable<IScreenInfo> Screens { get; set; }
        IEnumerable<IDisplayAdapter> DisplayAdapters { get; set; }
        IEnumerable<IServiceInfo> Services { get; set; }
        AppSettingsSection LoadConfiguration(FileInfo file);
        IFileVersionInfo GetVersionInfo(FileInfo fi);

    }
}
