using System;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: PluginBase
    {
        public override string Name => "Redis";
        public override string Description => "Enables the use of a Redis cache.";

        public override void Register()
        {
        }

        public override void Unregister()
        {
        }
    }
}
