using System;
using System.Collections.Generic;
using System.Diagnostics;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using DiagnosticsSharp.Core.Services;
using Fclp.Internals.Extensions;

//prevent ambiguity between NLog ConfigurationManager and .net 


namespace DiagnosticsSharp
{
    public class App 
    {
        private readonly CommandLineOptions _opts;
        private readonly IConsoleService _console;
        private readonly IEnumerable<IDiagnosticRenderer> _plugins;


        public App(CommandLineOptions opts, IConsoleService  console, IEnumerable<IDiagnosticRenderer> plugins)
        {
            _opts = opts;
            _console = console;
            _plugins = plugins;
        }

        public void Run()
        {
            // Main Application Logic 
            _console.Log("=====================================================================");
            _console.Log("Digital Paradox Diagnostics");
            _console.Log("=====================================================================");
            _console.Log($"Report Generated: {DateTime.Now}");
            _console.Log($"Path: { _opts.TargetPath }");

            _plugins.ForEach(RenderPlugin);
            
            if (_opts.WriteToFile) _console.WriteLog();

            if (_opts.OpenFile && _opts.WriteToFile)
            {
                Process.Start(_opts.OutputFilePath);
            }

            if (Debugger.IsAttached)
            {
                Console.ReadKey();
            }

        }

        public void RenderPlugin(IDiagnosticRenderer plugin)
        {
            _console.Log("=====================================================================");
            _console.Log( plugin.SectionTitle );
            _console.Log("=====================================================================");
            plugin.Render();
        }

    }
}
