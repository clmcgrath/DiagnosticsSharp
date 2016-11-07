using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace DiagnosticsSharp.Plugins
{
    public class ProcessInfoPlugin : IDiagnosticRenderer
    {
        private readonly IConsoleService _console;
        private readonly IMachineInfoService _machineInfo;

        public ProcessInfoPlugin(IConsoleService console, IMachineInfoService machineInfo)
        {
            _console = console;
            _machineInfo = machineInfo;
        }

        public string Name { get; set; } = nameof(ProcessInfoPlugin);
        public string SectionTitle { get; set; } = "Processes";
        public void Render()
        {
            _machineInfo.Processes.ForEach(q=> _console.Log($"{q.PID, 5}|{q.ProcessName, 60}|{q.CPU, 5}|{q.MemoryUsage, 15}"));
        }

        
    }
}