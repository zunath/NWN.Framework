using System;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: PluginBase
    {
        public override string Name => "Redis";
        public override string Description => "Enables the use of a Redis cache.";

        public override void Register(Hub hub)
        {
            Console.WriteLine("about to subscribe in redis plugin registration");
            //hub.Subscribe<OnModuleHeartbeat>(heartbeat =>
            //{
            //    Console.WriteLine("Hello from redis pluginregistration");
            //});

            hub.Publish(new OnModuleHeartbeat());
            Console.WriteLine("subscribed in plugin registration");
        }

        public override void Unregister()
        {
        }
    }
}
