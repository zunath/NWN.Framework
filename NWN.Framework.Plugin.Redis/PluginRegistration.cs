using System;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: IPlugin
    {
        public void Register()
        {
            Console.WriteLine("Registered Redis plugin");

            MessageHub.Instance.Subscribe<OnModuleHeartbeat>(msg => Run());
        }

        public void Unregister()
        {
            Console.WriteLine("Unregistered Redis plugin");
        }

        private void Run()
        {
            Console.WriteLine("Hello from heartbeat");
        }
    }
}
