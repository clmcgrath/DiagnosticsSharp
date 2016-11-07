using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace DiagnosticsSharp.Plugins
{
    public class DeviceInfoPlugin : IDiagnosticRenderer
    {
        private readonly IConsoleService _console;
        private readonly IMachineInfoService _machineInfo;

        public DeviceInfoPlugin(IConsoleService console, IMachineInfoService machineInfo)
        {
            _console = console;
            _machineInfo = machineInfo;
        }

        public string Name { get; set; } = nameof(DeviceInfoPlugin);
        public string SectionTitle { get; set; } = "Installed Devices";
        public void Render()
        {
            _machineInfo.InstalledDevices.ForEach(q => _console.Log($"{q.DeviceType, 15} | {q.Manufacturer,-50} {q.Status, -10} {q.DeviceName, -75}"));
        }
    }
}