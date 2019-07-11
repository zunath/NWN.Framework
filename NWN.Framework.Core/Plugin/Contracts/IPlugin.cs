using System;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Providers.Contracts;

namespace NWN.Framework.Core.Plugin.Contracts
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        void Register();
        void Unregister();
        void SignalEvent(Type type, object payload);
        IMessageHub PluginMessageHub { get; }
    }
}
