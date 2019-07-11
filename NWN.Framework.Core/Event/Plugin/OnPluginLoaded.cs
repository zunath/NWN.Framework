using System;

namespace NWN.Framework.Core.Event.Plugin
{
    public class OnPluginLoaded: EventBase
    {
        public string DLLPath { get; set; }

        public OnPluginLoaded(string dllPath)
        {
            DLLPath = dllPath;
        }
    }
}
