using System;

namespace NWN.Framework.Core.Event.Plugin
{
    public class OnPluginUnloaded: EventBase
    {
        public string DLLPath { get; set; }

        public OnPluginUnloaded(string dllPath)
        {
            DLLPath = dllPath;
        }
    }
}
