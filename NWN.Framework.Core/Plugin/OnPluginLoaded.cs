namespace NWN.Framework.Core.Plugin
{
    public class OnPluginLoaded
    {
        public string DLLPath { get; set; }

        public OnPluginLoaded(string dllPath)
        {
            DLLPath = dllPath;
        }
    }
}
