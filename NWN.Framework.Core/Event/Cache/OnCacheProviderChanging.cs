using System;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core.Event.Cache
{
    public class OnCacheProviderChanging: EventBase
    {
        public ICacheProvider CacheProvider { get; set; }

        public OnCacheProviderChanging(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }
    }
}
