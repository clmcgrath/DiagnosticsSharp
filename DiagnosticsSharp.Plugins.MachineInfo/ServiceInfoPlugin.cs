using System.Linq;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace DiagnosticsSharp.Plugins
{
    public class ServiceInfoPlugin : IDiagnosticRenderer
    {
        private readonly IConsoleService _console;
        private readonly IMachineInfoService _machineInfo;

        public ServiceInfoPlugin(IConsoleService console, IMachineInfoService machineInfo)
        {
            _console = console;
            _machineInfo = machineInfo;
        }

        public string Name { get; set; } = nameof(ServiceInfoPlugin);
        public string SectionTitle { get; set; } = "Services";
        public void Render()
        {
            _machineInfo.Services.OrderByDescending(q=>q.Status).ForEach(q=> _console.Log($"{q.Status, -10}|\t{q.DisplayName, -75} {q.ServiceName, -50}"));
        }
    }
}