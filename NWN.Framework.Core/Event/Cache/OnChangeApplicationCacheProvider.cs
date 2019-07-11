using System;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core.Event.Cache
{
    public class OnChangeApplicationCacheProvider: EventBase
    {
        public ICacheProvider CacheProvider { get; set; }

        public OnChangeApplicationCacheProvider(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }
    }
}
