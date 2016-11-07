using System;
using System.IO;
using Fclp;

namespace DiagnosticsSharp.Core.Services
{
    //to add new options update Bootstrapper ParseCommandLine(args)
    public class CommandLineOptions
    {
        public bool LogToConsole { get; set; }
        public string OutputFilePath { get; set; } 
        public string TargetPath { get; set; } =  Environment.CurrentDirectory;
        public bool OpenFile { get; set; }
        public bool WriteToFile { get; set; } = true;
        public string PluginsDirectory { get; set; } = Path.Combine(Environment.CurrentDirectory, @"\Plugins\");

        public static CommandLineOptions ParseCommandLine(string[] args)
        {

            //setup commandline arguments 
            var opts = new FluentCommandLineParser<CommandLineOptions>();

            opts.Setup(q => q.LogToConsole)
                .As(CaseType.CaseInsensitive, "c", "console")
                .WithDescription("Log output to terminal/console (use with jenkins to provide additional log detail )")
                .SetDefault(false);

            opts.Setup(q => q.OpenFile)
                .As(CaseType.CaseInsensitive, "l", "launch")
                .WithDescription("Opens environment file in default text editor after completed")
                .SetDefault(false);

            opts.Setup(q => q.WriteToFile)
                .As(CaseType.CaseInsensitive, "w", "writeToFile")
                .WithDescription("output diagnostics to file")
                .SetDefault(true);

            opts.Setup(q => q.OutputFilePath)
                .As(CaseType.CaseInsensitive, "o", "output")
                .SetDefault(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "environment.txt"))
                .WithDescription("Specify output file save directory.");

            opts.Setup(q => q.TargetPath)
                .As(CaseType.CaseInsensitive, "t", "target")
                .SetDefault(Path.Combine(Environment.CurrentDirectory))
                .WithDescription("Specify unity directory location");

            opts.SetupHelp("h", "help", "?")
                .Callback(q =>
                {
                    Console.Write(q);
                    Environment.Exit(0);
                });

            var result = opts.Parse(args);
            if (result.HasErrors)
            {
                Environment.Exit(1);
            }
            return opts.Object;
        }

        public static CommandLineOptions ParseCommandLine()
        {
            return ParseCommandLine(Environment.GetCommandLineArgs());
        }
    }


}