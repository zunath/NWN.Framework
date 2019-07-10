using System;
using System.IO;
using System.Runtime.Remoting;
using NWN.Framework.Core;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: PluginBase
    {
        private Guid _heartbeatEvent;

        public override void Register()
        {
            Console.WriteLine("Registered Redis plugin");
        }

        public override void Unregister()
        {
            MessageHub.Instance.Unsubscribe(_heartbeatEvent);
            Console.WriteLine("Unregistered Redis plugin");
        }


        public override void OnModuleHeartbeat()
        {
            Run();
        }

        private void Run()
        {
            Console.WriteLine("Getting files in Redis plugin");
            Console.WriteLine("HALLELUJAH IT WORKS");
        }
    }
}
