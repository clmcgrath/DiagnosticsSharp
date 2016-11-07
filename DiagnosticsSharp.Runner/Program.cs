using System;
using DiagnosticsSharp.Services;

namespace DiagnosticsSharp
{
    class Program : IDisposable
    {
        static void Main(string[] args)
        {
            using (var bootstrapper = new Bootstrapper(args))
            {
                var program = bootstrapper.Build<App>();
                program.Run();
            }
        }
        
        public void Dispose() => GC.SuppressFinalize(this);
    }
}