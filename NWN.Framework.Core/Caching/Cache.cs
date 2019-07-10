using System;
using NWN.Framework.Core.Event.Custom;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.NWNX;
using StackExchange.Redis;

namespace NWN.Framework.Core.Caching
{
    public class Cache: MarshalByRefObject
    {
        internal ConnectionMultiplexer Connection { get; private set; }

        private static Cache _instance;
        public static Cache Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Cache();
                }

                return _instance;
            }
        }

        public void Initialize()
        {
            string ip = Environment.GetEnvironmentVariable("NWN_FRAMEWORK_REDIS_IP");
            string port = Environment.GetEnvironmentVariable("NWN_FRAMEWORK_REDIS_PORT");

            if (string.IsNullOrWhiteSpace(ip))
            {
                Console.WriteLine("ERROR: Redis cache IP not set. Set the environment variable 'NWN_FRAMEWORK_REDIS_IP' in your configuration.");
                NWNXAdmin.ShutdownServer();
                return;
            }
            if(string.IsNullOrWhiteSpace(port))
            {
                Console.WriteLine("ERROR: Redis cache Port not set. Set the environment variable 'NWN_FRAMEWORK_REDIS_PORT' in your configuration.");
                NWNXAdmin.ShutdownServer();
                return;
            }

            string uri = ip + ":" + port;
            Console.WriteLine("Connecting to Redis Cache at " + uri);

            try
            {
                Connection = ConnectionMultiplexer.Connect(uri);
                Console.WriteLine("Cache connected successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to connect to Redis cache. Exception details: " + ex);
                NWNXAdmin.ShutdownServer();
                return;
            }

            Hub.Instance.Publish(new OnCacheConnected());
        }
    }
}
