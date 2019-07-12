using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWN.Framework.Core.NWNX;
using NWN.Framework.Core.Providers.Contracts;
using StackExchange.Redis;

namespace NWN.Framework.Plugin.Redis
{
    public class RedisCacheProvider: MarshalByRefObject, ICacheProvider
    {
        private ConnectionMultiplexer Connection { get; set; }

        public void Initialize()
        {
            Console.WriteLine("Redis cache is initializing...");

            string ip = Environment.GetEnvironmentVariable("NWN_FRAMEWORK_REDIS_IP");
            string port = Environment.GetEnvironmentVariable("NWN_FRAMEWORK_REDIS_PORT");

            if (string.IsNullOrWhiteSpace(ip))
            {
                Console.WriteLine("ERROR: Redis cache IP not set. Set the environment variable 'NWN_FRAMEWORK_REDIS_IP' in your configuration.");
                NWNXAdmin.ShutdownServer();
                return;
            }
            if (string.IsNullOrWhiteSpace(port))
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
                throw;
            }
        }

        public T Get<T>(Guid key)
        {
            string json = Connection.GetDatabase().StringGet(key.ToString());
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Set<T>(Guid key, T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            Connection.GetDatabase().StringSet(key.ToString(), json);
        }

        public IEnumerable<KeyValuePair<Guid, object>> GetAllKeys()
        {
            Console.WriteLine("getting env ip");
            string ip = Environment.GetEnvironmentVariable("NWN_FRAMEWORK_REDIS_IP");

            Console.WriteLine("getting env port");
            string port = Environment.GetEnvironmentVariable("NWN_FRAMEWORK_REDIS_PORT");

            Console.WriteLine("ip = " + ip + ", port = " + port);

            List<KeyValuePair<Guid, object>> results = new List<KeyValuePair<Guid, object>>();
            Console.WriteLine("getting server keys");

            string uri = ip + ":" + port;
            var keys = Connection.GetServer(uri).Keys();
            foreach (var key in keys)
            {
                Console.WriteLine("converting guid");
                var guid = new Guid(key.ToString());
                Console.WriteLine("getting json value for ID: " + guid);
                var json = Connection.GetDatabase().StringGet(key);
                Console.WriteLine("got json: " + json);
                var deserialized = JsonConvert.DeserializeObject<object>(json);
                Console.WriteLine("deserialized: " + deserialized.GetType().FullName);
                results.Add(new KeyValuePair<Guid, object>(guid, deserialized));
                Console.WriteLine("added to results");
            }

            return results;
        }
    }
}
