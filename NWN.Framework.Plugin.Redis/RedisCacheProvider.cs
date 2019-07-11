using System;
using NWN.Framework.Core.Providers.Contracts;
using StackExchange.Redis;

namespace NWN.Framework.Plugin.Redis
{
    public class RedisCacheProvider: MarshalByRefObject, ICacheProvider
    {
        private ConnectionMultiplexer Connection { get; set; }

        public void Initialize()
        {
            Console.WriteLine("initializing redis cache");
        }
    }
}
