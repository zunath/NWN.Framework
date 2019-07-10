using System;

namespace NWN.Framework.Core.Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        void Register();
        void Unregister();
        void SignalEvent(Type type, object payload);
    }
}
