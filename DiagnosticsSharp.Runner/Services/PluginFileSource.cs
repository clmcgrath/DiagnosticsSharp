using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using DiagnosticsSharp.Core.Services;
using DiagnosticsSharp.Interfaces;

namespace DiagnosticsSharp.Services
{
    public class PluginFileSource : IPluginFileSource
    {
        private readonly CommandLineOptions _opts;
        private readonly IMachineInfoService _machineInfo;
        public DirectoryInfo PluginsFolder;
       

        public PluginFileSource(CommandLineOptions opts, IMachineInfoService machineInfo)
        {
            PluginsFolder = new DirectoryInfo(opts.PluginsDirectory);
            _opts = opts;
            _machineInfo = machineInfo;
        }

        public IEnumerable<Type> GetPlugins()
        {
            var fileInfos = _machineInfo.GetFileList($"{_opts.PluginsDirectory}\\**\\*\\.(exe|dll)").ToList();


            if (!Directory.Exists(_opts.PluginsDirectory) || !fileInfos.Any()) return new List<Type>();


            var assemblies = fileInfos.Select(q => Assembly.LoadFile(q.FullName));

            var pluginTypes = from a in assemblies
                from t in a.GetExportedTypes()
                where typeof(IDiagnosticRenderer).IsAssignableFrom(t)
                select t;

            return pluginTypes;
        }
    }
}
