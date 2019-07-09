using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;
using SWLOR.Game.Core;

namespace NWN.Framework.Core.Event
{
    internal static class EventSubscriptions
    {
        internal static void Subscribe()
        {
            MessageHub.Instance.Subscribe<OnModuleLoad>(msg => PluginLoader.Start());
        }
    }
}
