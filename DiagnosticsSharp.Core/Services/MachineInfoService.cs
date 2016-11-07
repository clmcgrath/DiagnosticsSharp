using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using DiagnosticsSharp.Core.Interfaces;
using DiagnosticsSharp.Core.Models;

namespace DiagnosticsSharp.Core.Services
{
    public class MachineInfoServiceService : IMachineInfoService
    {

        public string OperatingSystemName { get; set; }
        public string OperatingSystemArchitecture { get; set; }
        public string OperatingSystemServicePack { get; set; }
        public double TotalRam { get; set; }
        public string Processor { get; set; }
        public string MachineName { get; set; }
        public string UserDomain { get; set; }
        public string Username { get; set; }
        public IEnumerable<IProcessInfo> Processes { get; set; }
        public IEnumerable<IDeviceInfo> InstalledDevices { get; set; }
        public IEnumerable<IServiceInfo> Services { get; set; }

        public IEnumerable<IScreenInfo> Screens { get; set; }
        public IEnumerable<IDisplayAdapter> DisplayAdapters { get; set; }

        public AppSettingsSection LoadConfiguration(FileInfo file)
        {
            var configMap = new ExeConfigurationFileMap {ExeConfigFilename = file.FullName};

            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            return config.AppSettings;
        }

        public IFileVersionInfo GetVersionInfo(FileInfo fi)
        {
            var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(fi.FullName);
            return new Models.FileVersionInfo {Version = fvi.FileVersion};
        }

        protected static string GetProcessor()
        {
            var sb = new StringBuilder();
            using (var mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor"))
            {
                foreach (var mo in mos.Get())
                {
                    sb.AppendLine($"{mo["Name"]} {Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")}");
                    // CPU Name and addtl Info 
                }
            }
            return sb.ToString();
        }

        private static string OperatingSystemInfo(string key)
        {
            //display important information about operating system 
            using (var mos = new ManagementObjectSearcher(@"select * from Win32_OperatingSystem"))
            {
                foreach (var o in mos.Get())
                {
                    var managementObject = (ManagementObject) o;
                    if (managementObject[key] != null)
                    {
                        return managementObject[key].ToString(); //Display operating system caption
                    }
                }
            }
            return string.Empty;
        }

        private static IEnumerable<IDisplayAdapter> GetGraphicsHardware()
        {
            return new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration").Get()
                .Cast<ManagementBaseObject>().ToList().Select(BuildAdapter);
        }

        private static IDisplayAdapter BuildAdapter(ManagementBaseObject mo)
        {
            var adapter = new DisplayAdapter();
            foreach (var property in mo.Properties)
            {
                if (property.Name == "Caption")
                {
                    adapter.Name = (property.Value?.ToString()) ?? "";
                }
                if (property.Name == "DriverVersion")
                {
                    adapter.DriverVersion = (property.Value?.ToString()) ?? "";
                }
                if (property.Name == "DisplayFrequency")
                {
                    adapter.Frequency = (property.Value?.ToString()) ?? "";
                }
            }
            return adapter;
        }


        public static MachineInfoServiceService Resolve()
        {
            var machineInfo = new MachineInfoServiceService
            {
                TotalRam = GetTotalRam(),
                Screens = Screen.AllScreens.Select(q => q.ScreenInfo()),
                Username = Environment.UserName,
                UserDomain = Environment.UserDomainName,
                MachineName = Environment.MachineName,
                Processor = GetProcessor(),
                OperatingSystemArchitecture = OperatingSystemInfo("OSArchitecture"),
                OperatingSystemName = OperatingSystemInfo("Caption"),
                OperatingSystemServicePack = OperatingSystemInfo("CSDVersion"),
                DisplayAdapters = GetGraphicsHardware(),
                Processes = GetProcesses(),
                Services = GetServices(),
                InstalledDevices = GetDeviceList()
            };

            return machineInfo;
        }

        private static IEnumerable<IServiceInfo> GetServices()
        {
            return ServiceController.GetServices().Select(s => new ServiceInfo()
            {
                ServiceName = s.ServiceName,
                DisplayName = s.DisplayName,
                Status = s.Status,
                ServiceType = s.ServiceType
            });
        }

        
        private static IEnumerable<IProcessInfo> GetProcesses()
        {
            using (var mc = new ManagementClass("Win32_PerfFormattedData_PerfProc_Process"))
            {
                var moc = mc.GetInstances();
                var objs = moc.Cast<ManagementObject>();
                return objs.Select(q =>
                {
                    var pid = Convert.ToInt32(q.GetPropertyValue("IDProcess"));
                    return new ProcessInfo()
                    {
                        PID = pid.ToString(),
                        ProcessName = q.GetPropertyValue("Name").ToString(),
                        MemoryUsage = Convert.ToInt64(q.GetPropertyValue("WorkingSet")),
//                        WindowName = process.MainWindowTitle,
                        CPU = Convert.ToUInt16(q.GetPropertyValue("PercentProcessorTime")),
                        PercentPrivilegedTime = Convert.ToUInt16(q.GetPropertyValue("PercentPrivilegedTime")),
                        PercentProcessorTime = Convert.ToUInt16(q.GetPropertyValue("PercentPrivilegedTime")),
                        PercentUserTime = Convert.ToUInt16(q.GetPropertyValue("PercentPrivilegedTime")),
                        PriorityBase = Convert.ToUInt16(q.GetPropertyValue("PriorityBase")),
                        ThreadCount = Convert.ToInt64(q.GetPropertyValue("ThreadCount")),
                        CreatingProcessID = q.GetPropertyValue("CreatingProcessID").ToString(),
                        HandleCount = Convert.ToInt64(q.GetPropertyValue("HandleCount").ToString()),
                        PageFaultsPersec = Convert.ToInt64(q.GetPropertyValue("PageFaultsPersec").ToString())
                    };
                });
            }
        }

        public IEnumerable<FileSystemInfo> GetFileList(string path)
        {
            var files = Glob.Glob.Expand(path);
            return files.Where(q => q is FileInfo);
        }

        static IEnumerable<IDeviceInfo> GetDeviceList()
        {
            using (var mc = new ManagementClass("Win32_PnPEntity"))
            {
                var moc = mc.GetInstances();
                var objs = moc.Cast<ManagementObject>();
                return objs.Select(q => new DeviceInfo()
                {
                    DeviceGuid = q.GetPropertyValue("ClassGuid") == null ? Guid.Empty : Guid.Parse(q.GetPropertyValue("ClassGuid").ToString()),
                    Status = q.GetPropertyValue("Status").ToString(),
                    StatusInfo = q.GetPropertyValue("StatusInfo")?.ToString() ?? string.Empty,
                    Description = q.GetPropertyValue("Description")?.ToString() ?? string.Empty,
                    Caption = q.GetPropertyValue("Caption")?.ToString() ?? "",
                    DeviceName = q.GetPropertyValue("Name")?.ToString() ?? "",
                    DeviceType = q.GetPropertyValue("PNPClass")?.ToString() ?? "",
                    DeviceId = q.GetPropertyValue("DeviceId")?.ToString() ?? "",
                    Manufacturer = q.GetPropertyValue("Manufacturer")?.ToString() ?? "",
                }).OrderBy(q=>q.DeviceType).ThenBy(q=>q.DeviceName);
            }
        }

        protected static double GetTotalRam()
        {
            using (var mc = new ManagementClass("Win32_ComputerSystem"))
            {
                var moc = mc.GetInstances();

                return moc.Cast<ManagementObject>()
                    .Sum(item => Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value)/1048576, 0));
            }

        }

    }
}