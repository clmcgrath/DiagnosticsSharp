using System.Diagnostics;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using DiagnosticsSharp.Core.Services;
using Microsoft.Practices.ObjectBuilder2;

namespace DiagnosticsSharp.Plugins
{
    [DebuggerDisplay("{Name} : {SectionTitle}")]
    public class MachineInfoPlugin : IDiagnosticRenderer
    {
        private readonly IConsoleService _console;
        private readonly CommandLineOptions _opts;
        private readonly IMachineInfoService _machineInfoService;

        public MachineInfoPlugin(IConsoleService console, IMachineInfoService machineInfoService, CommandLineOptions opts)
        {
            this._console = console;
            _opts = opts;
            _machineInfoService = machineInfoService;
        }

        public string Name { get; set; } = nameof(MachineInfoPlugin);
        public string SectionTitle { get; set; } = "Machine Information";

        public void Render()
        {
            _console.Log($"ETAG: { _machineInfoService.MachineName }");
            _console.Log($"Current Domain: { _machineInfoService.UserDomain }");
            _console.Log($"Current User: { _machineInfoService.Username }");
            _console.Log($"Processor:  { _machineInfoService.Processor }");
            _console.Log($"System RAM: { _machineInfoService.TotalRam } MB");

            _machineInfoService.Screens.ForEach(q =>
             {
                 var primary = q.Primary ? "(Primary)" : string.Empty;
                 _console.Log($"Monitor: {q.DeviceName} Resolution: {q.Bounds.Width} x {q.Bounds.Height} {primary} ");
             });

            _console.Log("Display Adapters:");
            _machineInfoService.DisplayAdapters.ForEach(d =>
            {
                _console.Log($"{d.Name}");
            });
        }
    }
}