using System;
using System.IO;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: IPlugin
    {
        private Guid _heartbeatEvent;

        public void Register()
        {
            _heartbeatEvent = MessageHub.Instance.Subscribe<OnModuleHeartbeat>(msg => Run());
            Console.WriteLine("Registered Redis plugin");
        }

        public void Unregister()
        {
            MessageHub.Instance.Unsubscribe(_heartbeatEvent);
            Console.WriteLine("Unregistered Redis plugin");
        }

        private void Run()
        {
            string[] files = Directory.GetFiles("/nwn/home/mono/");


            Console.WriteLine("===============================");
            foreach (var file in files)
            {
                Console.WriteLine("file found: " + file);
            }

            Console.WriteLine("===============================");
        }
    }
}
