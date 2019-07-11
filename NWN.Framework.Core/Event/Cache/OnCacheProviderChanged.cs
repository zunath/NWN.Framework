using System;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core.Event.Cache
{
    public class OnCacheProviderChanged: EventBase
    {
        public ICacheProvider CacheProvider { get; set; }

        public OnCacheProviderChanged(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }
    }
}
