using NWN.Framework.Core.Messaging;

namespace NWN.Framework.Core.Plugin
{
    public interface IPlugin
    {
        void Register();
        void Unregister();
        void SubscribeEvents(IMessageHub messageHub);
        void UnsubscribeEvents(IMessageHub messageHub);
    }
}
