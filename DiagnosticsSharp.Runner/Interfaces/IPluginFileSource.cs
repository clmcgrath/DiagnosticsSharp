using System;
using System.Collections.Generic;

namespace DiagnosticsSharp.Interfaces
{
    public interface IPluginFileSource
    {
        IEnumerable<Type> GetPlugins();
    }
}
