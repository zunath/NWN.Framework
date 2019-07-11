using System;
using NWN.Framework.Core.Event.Cache;
using NWN.Framework.Core.Event.Plugin;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.NWNX;
using NWN.Framework.Core.Providers;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core
{
    internal class AppCache
    {
        private ICacheProvider Cache { get; set; }
        private bool _loadedExternalCache;

        public void Initialize()
        {
            MessageHub.Instance.Subscribe<OnCacheProviderChanging>(x => ChangeCacheProvider(x.CacheProvider, true));
            MessageHub.Instance.Subscribe<OnPluginLoaded>(x =>
            {
                // The cache provider hasn't actually changed, but the newly loaded plugin needs to receive it.
                MessageHub.Instance.Publish(new OnCacheProviderChanged(Cache));
            });

            ChangeCacheProvider(new InMemoryCacheProvider(), false);
        }

        private void ChangeCacheProvider(ICacheProvider provider, bool isExternal)
        {
            // We've already loaded one external caching system. We don't know which one to
            // pick, so let's inform the user and shut down the server.
            if (_loadedExternalCache && isExternal)
            {
                Console.WriteLine("ERROR: More than one caching implementation detected. You have " + Cache.GetType().FullName + " loaded already but another plugin is trying to load " + provider.GetType().FullName + ". Disable one of these plugins and restart the server.");
                NWNXAdmin.ShutdownServer();
                return;
            }

            Cache = provider;
            Cache.Initialize();
            _loadedExternalCache = isExternal;

            Console.WriteLine("Cache provider changed to: " + provider.GetType().FullName);
            MessageHub.Instance.Publish(new OnCacheProviderChanged(provider));
        }
    }
}
