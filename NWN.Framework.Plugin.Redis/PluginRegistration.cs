using System;
using System.Collections.Generic;
using NWN.Framework.Core.Event.Cache;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: PluginBase
    {
        public override string Name => "Redis";
        public override string Description => "Enables the use of a Redis cache.";

        private int _count;
        private readonly List<Guid> _ids = new List<Guid>();
        

        public override void Register()
        {
            MessageHub.Instance.Publish(new OnCacheProviderChanging(new RedisCacheProvider()));

        }

        public override void Unregister()
        {
        }

        public override void SubscribeEvents()
        {

            MessageHub.Instance.Subscribe<OnModuleHeartbeat>(x =>
            {

                if (Cache == null)
                {
                    Console.WriteLine("Cache is null");
                }

                _count++;

                if (_count % 2 == 0)
                {
                    foreach (var id in _ids)
                    {
                        string value = Cache.Get<string>(id);
                        Console.WriteLine("got value: " + value);
                    }
                }
                else
                {
                    Guid id = Guid.NewGuid();
                    Cache.Set(id, "this is a test. count = " + _count);
                    _ids.Add(id);
                }

            });
        }

        public override void UnsubscribeEvents()
        {

        }
    }
}
