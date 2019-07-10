using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWN.Framework.Core.Caching;
using NWN.Framework.Core.Event;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using SWLOR.Game.Core;

namespace NWN.Framework.TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("NWN_FRAMEWORK_REDIS_IP", "192.168.0.108");
            Environment.SetEnvironmentVariable("NWN_FRAMEWORK_REDIS_PORT", "6379");

            PluginLoader pluginLoader = new PluginLoader();
            pluginLoader.Start();
            Cache.Instance.Initialize();

            MessageHub.Instance.Publish(new OnModuleLoad());
            MessageHub.Instance.Publish(new OnModuleHeartbeat());

            Console.WriteLine("Press any key to end.");
            Console.ReadKey();
        }
    }
}
