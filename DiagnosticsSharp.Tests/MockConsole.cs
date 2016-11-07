using System.Text;
using DiagnosticsSharp.Core.Interfaces;

namespace DiagnosticsSharp.Tests
{
    public class MockConsole : IConsoleService
    {
        public MockConsole()
        {
            Output = new StringBuilder();
        }
        StringBuilder Output { get; set; }
        public string ConsoleOutput => GetConsoleOut();

        private string GetConsoleOut()
        {

            return Output.ToString();
        }

        public void Log(string message)
        {
            Output.AppendLine(message);
        }

        public void WriteLog()
        {
            Output.AppendLine("{ Write to File... }");

        }
    }
}
