using System;
using Newtonsoft.Json;
using NWN.Framework.Core.Caching;
using StackExchange.Redis;

namespace NWN.Framework.Core.Messaging
{
    [Serializable]
    public class Hub
    {
        [NonSerialized]
        private static Hub _instance;
        public static Hub Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Hub();
                }

                return _instance;
            }
        }

        public void Publish<T>(T payload)
        {
            var key = typeof(T).Name;
            var db = Cache.Instance.Connection.GetSubscriber();

            Console.WriteLine("publishing event: " + key);

            string json = JsonConvert.SerializeObject(payload);
            db.Publish(new RedisChannel(key, RedisChannel.PatternMode.Literal), json);
            Console.WriteLine("event published: " + key);
        }

        //public void Subscribe<T>(Action<T> action)
        //{
        //    var key = typeof(T).Name;
        //    var subscriber = Cache.Instance.Connection.GetSubscriber();

        //    Console.WriteLine("building redisAction for: " + key);
        //    var redisAction = new Action<RedisChannel, RedisValue>((channel, value) =>
        //    {
        //        string json = value.ToString();
        //        Console.WriteLine("json = " + json);
        //        action.Invoke(JsonConvert.DeserializeObject<T>(json));
        //    });

        //    Console.WriteLine("subscribing to: " + key);
        //    subscriber.Subscribe(new RedisChannel(key, RedisChannel.PatternMode.Literal), redisAction);

        //    Console.WriteLine("subscription for " + key + " is done");
        //}
    }
}
