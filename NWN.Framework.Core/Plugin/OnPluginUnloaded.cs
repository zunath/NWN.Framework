namespace NWN.Framework.Core.Plugin
{
    public class OnPluginUnloaded
    {
        public string DLLPath { get; set; }

        public OnPluginUnloaded(string dllPath)
        {
            DLLPath = dllPath;
        }
    }
}
