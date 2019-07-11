using NWN.Framework.Core.Event.Cache;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: PluginBase
    {
        public override string Name => "Redis";
        public override string Description => "Enables the use of a Redis cache.";

        public override void Register()
        {
            MessageHub.Instance.Publish(new OnChangeApplicationCacheProvider(new RedisCacheProvider()));
        }

        public override void Unregister()
        {
        }

    }
}
