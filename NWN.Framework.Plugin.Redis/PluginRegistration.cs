using System;
using System.IO;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: MarshalByRefObject, IPlugin
    {
        private Guid _heartbeatEvent;

        public void Register()
        {
            Console.WriteLine("Registered Redis plugin");
        }

        public void Unregister()
        {
            MessageHub.Instance.Unsubscribe(_heartbeatEvent);
            Console.WriteLine("Unregistered Redis plugin");
        }

        public void SubscribeEvents(IMessageHub messageHub)
        {
            Console.WriteLine("running message hub");

            _heartbeatEvent = messageHub.Subscribe<OnModuleHeartbeat>(msg => Run());
        }

        private void Run()
        {
            Console.WriteLine("Getting files in Redis plugin");
            //string[] files = Directory.GetFiles(Environment.CurrentDirectory);


            //Console.WriteLine("===============================");
            //foreach (var file in files)
            //{
            //    Console.WriteLine("file found: " + file);
            //}

            //Console.WriteLine("===============================");
        }
    }
}
