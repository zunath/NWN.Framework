using System;
using NWN.Framework.Core.Plugin.Contracts;

namespace NWN.Framework.Core.Plugin
{
    /// <summary>
    /// Tracks each plugin and which app domain they belong to.
    /// </summary>
    internal class RegisteredPlugin
    {
        public AppDomain AppDomain { get; }
        public IPlugin Plugin { get; }

        public RegisteredPlugin(AppDomain appDomain, IPlugin plugin)
        {
            AppDomain = appDomain;
            Plugin = plugin;
        }
    }
}
