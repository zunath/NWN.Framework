using System;
using NWN.Framework.Core;
using NWN.Framework.Core.Messaging;
using NWN.Framework.Core.Plugin;

namespace NWN.Framework.Plugin.Logging
{
    public class PluginRegistration: PluginBase
    {
        public override string Name => "Logging";
        public override string Description => "General purpose logging tools.";

        public override void Register(Hub hub)
        {
        }

        public override void Unregister()
        {
        }

    }
}
