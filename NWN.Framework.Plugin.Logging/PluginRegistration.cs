using System;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Logging
{
    public class PluginRegistration: IPlugin
    {
        public void Register()
        {
            Console.WriteLine("Registered Logging plugin a new version");
        }

        public void Unregister()
        {
            Console.WriteLine("Unregistered logging plugin a new version");
        }

        public void SubscribeEvents(IMessageHub messageHub)
        {
        }
    }
}
