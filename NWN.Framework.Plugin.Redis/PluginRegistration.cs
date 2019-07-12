using System;
using NWN.Framework.Core.Event.Cache;
using NWN.Framework.Core.GameObject;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: PluginBase
    {
        public override string Name => "Redis";
        public override string Description => "Replaces the in-memory cache with a Redis cache provider.";

        public override void Register()
        {
            MessageHub.Instance.Publish(new OnCacheProviderChanging(new RedisCacheProvider()));


        }

        public override void Unregister()
        {
            MessageHub.Instance.Publish(new OnRemoveCustomCacheProvider());
        }

        public override void SubscribeEvents()
        {
            if (NWModule.Get().GetLocalInt("LOADED") == 1) return;

            Cache.Set(Guid.NewGuid(), "value 1");
            Cache.Set(Guid.NewGuid(), "value 2");
            Cache.Set(Guid.NewGuid(), "value 3");
            Cache.Set(Guid.NewGuid(), "value 4");

            NWModule.Get().SetLocalInt("LOADED", 1);
        }

        public override void UnsubscribeEvents()
        {

        }
    }
}
