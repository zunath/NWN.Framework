using System;
using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.GameObject;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Redis
{
    public class PluginRegistration: PluginBase
    {
        public override string Name => "Redis";
        public override string Description => "Enables the use of a Redis cache.";

        private Guid _heartbeatID;

        public override void Register()
        {
            _heartbeatID = MessageHub.Instance.Subscribe<OnModuleHeartbeat>(heartbeat => DoStuff());
        }

        public override void Unregister()
        {
            MessageHub.Instance.Unsubscribe(_heartbeatID);
        }

        public void DoStuff()
        {
            Console.WriteLine("loaded on the fly like a bawwwwwssss");

            NWPlayer player = _.GetFirstPC();

            player.SendMessage("what's up?!?!?");
        }
    }
}
