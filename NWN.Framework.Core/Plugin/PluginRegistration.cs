using System;

namespace NWN.Framework.Core.Plugin
{
    /// <summary>
    /// Tracks each plugin and which app domain they belong to.
    /// </summary>
    internal class PluginRegistration
    {
        public AppDomain AppDomain { get; }
        public IPlugin Plugin { get; }

        public PluginRegistration(AppDomain appDomain, IPlugin plugin)
        {
            AppDomain = appDomain;
            Plugin = plugin;
        }
    }
}
