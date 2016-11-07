using System.Diagnostics;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Plugins
{
    [DebuggerDisplay("{Name} : {SectionTitle}")]
    public class OperatingSystemInfoPlugin : IDiagnosticRenderer
    {
        private readonly IConsoleService _console;
        private readonly IMachineInfoService _machineInfoService;

        public OperatingSystemInfoPlugin(IConsoleService console, IMachineInfoService machineInfoService)
        {
            _console = console;
            _machineInfoService = machineInfoService;
        }

        public string Name { get; set; } = nameof(OperatingSystemInfoPlugin);
        public string SectionTitle { get; set; } = "Operating System";
        public virtual void Render()
        {
            _console.Log($"OS Version: {_machineInfoService.OperatingSystemName }");
            _console.Log($"OS Architecture: {_machineInfoService.OperatingSystemArchitecture }");
            _console.Log($"OS Service Pack: {_machineInfoService.OperatingSystemServicePack}");
        }
    
    }
}
