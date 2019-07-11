using System;
using NWN.Framework.Core.Event.Cache;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin.Contracts;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core.Plugin
{
    public abstract class PluginBase: MarshalByRefObject, IPlugin
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void Register();
        public abstract void Unregister();
        public abstract void SubscribeEvents();
        public abstract void UnsubscribeEvents();

        protected PluginBase()
        {
            MessageHub.Instance.Subscribe<OnCacheProviderChanged>(CacheProviderChanged);
        }

        /// <summary>
        /// Every plugin runs on its own AppDomain with its own MessageHub.
        /// The parent application (core) will call this event any time an event is raised there.
        /// In turn, we need to publish the same event on this plugin's MessageHub.
        /// </summary>
        /// <param name="type">The type of object being signaled.</param>
        /// <param name="payload">The data payload.</param>
        public void SignalEvent(Type type, object payload)
        {
            MessageHub.Instance.Publish(type, payload);
        }

        public IMessageHub PluginMessageHub => MessageHub.Instance;

        protected ICacheProvider Cache { get; private set; }

        private void CacheProviderChanged(OnCacheProviderChanged obj)
        {
            Cache = obj.CacheProvider;
        }

    }
}
