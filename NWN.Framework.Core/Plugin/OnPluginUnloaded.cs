using System;

namespace NWN.Framework.Core.Plugin
{
    public class OnPluginUnloaded: MarshalByRefObject
    {
        public string DLLPath { get; set; }

        public OnPluginUnloaded(string dllPath)
        {
            DLLPath = dllPath;
        }
    }
}
