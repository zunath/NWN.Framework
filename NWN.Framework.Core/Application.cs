using System;
using NWN.Framework.Core.Event.Cache;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.NWNX;
using NWN.Framework.Core.Providers;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core
{
    public class Application: MarshalByRefObject
    {
        public ICacheProvider Cache { get; private set; }
        private bool _loadedExternal;

        public void Initialize()
        {
            ChangeCacheProvider(new InMemoryCacheProvider(), false);
            MessageHub.Instance.RegisterGlobalErrorHandler(OnMessageHubError);
            MessageHub.Instance.Subscribe<OnChangeApplicationCacheProvider>(x => ChangeCacheProvider(x.CacheProvider, true));
        }

        private void OnMessageHubError(Guid id, Exception ex)
        {
            Console.WriteLine("MessageHub error: " + ex);
        }

        private void ChangeCacheProvider(ICacheProvider provider, bool isExternal)
        {
            // We've already loaded one external caching system. We don't know which one to
            // pick, so let's inform the user and shut down the server.
            if (_loadedExternal && isExternal)
            {
                Console.WriteLine("ERROR: More than one caching implementation detected. You have " + Cache.GetType().FullName + " loaded already but another plugin is trying to load " + provider.GetType().FullName + ". Disable one of these plugins and restart the server.");
                NWNXAdmin.ShutdownServer();
                return;
            }

            Cache = provider;
            Cache.Initialize();
            _loadedExternal = isExternal;

            MessageHub.Instance.Publish(new OnApplicationCacheLoaded());
        }
    }
}
