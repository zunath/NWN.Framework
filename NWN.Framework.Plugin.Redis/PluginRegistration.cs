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

            try
            {
                _heartbeatEvent = messageHub.Subscribe<OnModuleHeartbeat>(msg => Run());
            }
            catch(Exception ex)
            {
                Console.WriteLine("subscribe exception: " + ex);
            }

            
        }

        public void UnsubscribeEvents(IMessageHub messageHub)
        {
            Console.WriteLine("Running unsubscribe");

            messageHub.Unsubscribe(_heartbeatEvent);
        }

        private void Run()
        {
            Console.WriteLine("Getting files in Redis plugin");
            Console.WriteLine("HALLELUJAH IT WORKS");
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
