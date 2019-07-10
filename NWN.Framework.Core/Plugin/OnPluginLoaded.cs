using System;

namespace NWN.Framework.Core.Plugin
{
    public class OnPluginLoaded: MarshalByRefObject
    {
        public string DLLPath { get; set; }

        public OnPluginLoaded(string dllPath)
        {
            DLLPath = dllPath;
        }
    }
}
