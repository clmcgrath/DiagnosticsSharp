using System;
using System.IO;
using System.Text;
using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Core.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly CommandLineOptions _opts;

        public ConsoleService(CommandLineOptions opts)
        {
            _opts = opts;
        }
        public StringBuilder Output { get; set; } = new StringBuilder();
        public void Log(string message)
        {
            Output.AppendLine(message);
            if (_opts.LogToConsole)
            {
                Console.WriteLine(message);
            }
        }

        public void WriteLog()
        {
            try
            {
                var fi = new FileInfo(_opts.OutputFilePath);

                if (fi.Exists)
                {
                    fi.Delete();
                }

                File.WriteAllText(_opts.OutputFilePath, Output.ToString(), Encoding.Default);
                Console.WriteLine($"Environment file written to {_opts.OutputFilePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }
    }
}
