using System;
using System.Collections.Generic;
using DiagnosticsSharp.Core;
using DiagnosticsSharp.Core.Interfaces;
using DiagnosticsSharp.Core.Services;
using DiagnosticsSharp.Interfaces;
using DiagnosticsSharp.Plugins;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace DiagnosticsSharp.Services
{
    internal class Bootstrapper : UnityBootstrapper, IDisposable
    {
        private readonly string[] _args;
        
        public Bootstrapper(string[] args)
        {
            _args = args;
        }

        protected override void ConfigureContainer()
        {
            
            Container.RegisterType<CommandLineOptions>(new InjectionFactory(c => CommandLineOptions.ParseCommandLine()));
            Container.RegisterType<IMachineInfoService>(new InjectionFactory(c=> MachineInfoServiceService.Resolve()));
            Container.RegisterType<IConsoleService, ConsoleService>(new ContainerControlledLifetimeManager());
           
            //register plugins 
            Container.RegisterType<IDiagnosticRenderer, MachineInfoPlugin>(nameof(MachineInfoPlugin), new ContainerControlledLifetimeManager());
            Container.RegisterType<IDiagnosticRenderer, OperatingSystemInfoPlugin>(nameof(OperatingSystemInfoPlugin), new ContainerControlledLifetimeManager());
            Container.RegisterType<IDiagnosticRenderer, EnvironmentInfo>(nameof(EnvironmentInfo), new ContainerControlledLifetimeManager());
            Container.RegisterType<IDiagnosticRenderer, ProcessInfoPlugin>(nameof(ProcessInfoPlugin), new ContainerControlledLifetimeManager());
            Container.RegisterType<IDiagnosticRenderer, ServiceInfoPlugin>(nameof(ServiceInfoPlugin), new ContainerControlledLifetimeManager());
            Container.RegisterType<IDiagnosticRenderer, DeviceInfoPlugin>(nameof(DeviceInfoPlugin), new ContainerControlledLifetimeManager());


            Container.RegisterType(typeof(IEnumerable<IDiagnosticRenderer>),
                new InjectionFactory(container => container.ResolveAll<IDiagnosticRenderer>()));
            
            Container.RegisterType<IPluginFileSource, PluginFileSource>();
            //RegisterExternalPlugins(Container);

            base.ConfigureContainer();
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static void RegisterExternalPlugins(IUnityContainer container)
        {

            var fileSource = container.Resolve<IPluginFileSource>();
            var pluginTypes = fileSource.GetPlugins();
 
            pluginTypes.ForEach( t=> container.RegisterType(typeof(IDiagnosticRenderer), t));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public T Build<T>()
        {
            Run();
            return Resolve<T>();
        }
    }
}