﻿using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal class mod_on_dying
#pragma warning restore IDE1006 // Naming Styles
    {
        // ReSharper disable once UnusedMember.Local
        private static void Main()
        {
            // Bioware Default
            _.ExecuteScript("nw_o0_dying", Object.OBJECT_SELF); 
            MessageHub.Instance.Publish(new OnModuleDying());
        }
    }
}
