using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public override string Description => "Replaces the in-memory cache with a Redis cache provider.";

        public override void Register()
        {
            MessageHub.Instance.Publish(new OnCacheProviderChanging(new RedisCacheProvider()));

        }

        public override void Unregister()
        {
        }

        public override void SubscribeEvents()
        {
        }

        public override void UnsubscribeEvents()
        {

        }
    }
}
