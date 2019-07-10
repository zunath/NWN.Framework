using System;
using NWN.Framework.Core;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Logging
{
    public class PluginRegistration: PluginBase
    {
        public override void Register()
        {
            Console.WriteLine("Registered Logging plugin a new version");
        }

        public override void Unregister()
        {
            Console.WriteLine("Unregistered logging plugin a new version");
        }

    }
}
