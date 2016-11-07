using System;
using System.Linq;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using Microsoft.Practices.ObjectBuilder2;

namespace DiagnosticsSharp.Plugins
{
    public class EnvironmentInfo : IDiagnosticRenderer
    {
        private readonly IConsoleService _console;

        public EnvironmentInfo(IConsoleService console)
        {
            _console = console;
        }

        public string Name { get; set; } = nameof(EnvironmentInfo);
        public string SectionTitle { get; set; } = "Environment Info";
        public void Render()
        {
            typeof(EnvironmentVariableTarget).GetEnumNames().ForEach(e=> RenderEnvironmentVarsSection((EnvironmentVariableTarget)Enum.Parse(typeof(EnvironmentVariableTarget) , e) ));
        }

        private void RenderEnvironmentVarsSection(EnvironmentVariableTarget envTarget)
        {
            _console.Log($"---------------------------------------------------------- ");
            _console.Log($"{envTarget} Environment Variables");
            _console.Log($"---------------------------------------------------------- ");

            foreach (string key in Environment.GetEnvironmentVariables(envTarget).Keys.Cast<string>().OrderBy(q=>q))
            {
                _console.Log($"\t{key} : {Environment.GetEnvironmentVariables(envTarget)[key]}");
            }
        
        }
    }
}