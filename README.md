# DiagnosticsSharp
## Version 1.0-alpha 


DiagnosticsSharp is a command line utility and tooling framework to generate system, configuration, and environment details about a user's machine


## Usage 

***[c, console ]***  	Log output to terminal/console (default: false)

***[l, launch]***		Opens environment file in default editor after completed (default false)

***[w, writeToFile ]***   output diagnostics to file (default: true)

***[o, output]*** 		Specify output file save directory (Default: .\Environment.txt).

***[p, plugins]*** 		Specify plugins directory location (Default: Current Directory).

***[t, target]***		Specify unity directory location (Default: .\\)

## Supported Syntax

[-|--|/][switch_name][=|:| ][value]

**Supports boolean names****

diagnostics -s  // enable

diagnostics -s- // disabled

diagnostics -s+ // enable

**Supports combined (grouped) options**

diagnostics -xyz  // enable option x, y and z

diagnostics -xyz- // disable option x, y and z

diagnostics -xyz+ // enable option x, y and z


see: [fluent-command-line-parser](https://github.com/fclp/fluent-command-line-parser)


## Example 

    c:\> diagnostics -c

----------

    =====================================================================
    Diagnostics Tool
    =====================================================================
    Report Generated: 10/26/2016 2:56:49 PM
    =====================================================================
    Machine Info
    =====================================================================
    Machine Name: DUMMY_PC
    Current Domain: DUMMY_DOMAIN
    Current User: DUMMEY_USER
    Processor: Intel(R) Core(TM) i5-6300U CPU @ 2.40GHz Intel64 Family 6 Model 78 Stepping 3, GenuineIntel
    System RAM: 7603 MB
    Monitor: \\.\DISPLAY1 Resolution: 1920 x 1080 (Primary)
    Monitor: \\.\DISPLAY2 Resolution: 1680 x 1050 
    
    =====================================================================
    System Info
    =====================================================================
    Operating System Name  :  Microsoft Windows 10 Enterprise 
    Operating System Architecture  :  64-bit
    Operating System Service Pack   :
    

## Extending Functionality ##


DiagnosticsSharp is designed to be etexensible 
to add functionality / new sections to the tools 
the preferred way is to simply create a class library and specify where to load plugin dlls from via command line
or by using the default \<YourPath>\Plugins\ for application specific plugins or \<InstallDirectory>\Plugins\ for global plugins if using in a system wide context 

(Coming Soon!) To customize load order use a config file and specify your plugin configuration this way 


Plugins should Implement the `IDiagnosticsRenderer` interface 

    using System;
    using System.Diagnostics;
    using Microsoft.Practices.ObjectBuilder2;
    using DiagnosticsSharp.Core.Interfaces;
    using DiagnosticsSharp.Core.Services;
    
    namespace DiagnosticsSharp.Plugins.MachineInfoPlugins
    {
	    [DebuggerDisplay("{Name} : {SectionTitle}")]
	    public class MachineInfoPlugin : IDiagnosticRenderer
	    {
		    private readonly IConsoleService _console;
		    private readonly CommandLineOptions _opts;
		    private readonly IMachineInfo _machineInfo;
		    
		    public MachineInfoPlugin(IConsoleService console, IMachineInfo machineInfo, CommandLineOptions opts)
		    {
			    this._console = console;
			    _opts = opts;
			    _machineInfo = machineInfo;
		    }
		    
		    public string Name { get; set; } = nameof(MachineInfoPlugin);
		    public string SectionTitle { get; set; } = "Machine Information";
		    
		    public void Render()
		    {
			    _console.Log($"ETAG: { _machineInfo.ETag }");
			    _console.Log($"Current Domain: { _machineInfo.UserDomain }");
			    _console.Log($"Current User: { _machineInfo.Username }");
			    _console.Log($"Processor:  { _machineInfo.Processor }");
			    _console.Log($"System RAM: { _machineInfo.TotalRam } MB");
			    
			    _machineInfo.Screens.ForEach(q =>
			     {
			     var primary = q.Primary ? "(Primary)" : string.Empty;
			     _console.Log($"Monitor: {q.DeviceName} Resolution: {q.Bounds.Width} x {q.Bounds.Height} {primary} ");
			     });
		    
		    }
		  }
    }


## Development ##

For Development / Debugging of the Diagnostics CLI  it is recommended that you point your working directory setting in the visual studio project settings (Project=>Properties=>Debug) to a working project directory or install as nuget package to your solution if you are writing application specific dianostics plugins 