using System;
using NWN.Framework.Core.Messaging;

namespace NWN.Framework.Core.Plugin.Contracts
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        void Register();
        void Unregister();
        void SubscribeEvents();
        void UnsubscribeEvents();
        void SignalEvent(Type type, object payload);
        IMessageHub PluginMessageHub { get; }
    }
}
