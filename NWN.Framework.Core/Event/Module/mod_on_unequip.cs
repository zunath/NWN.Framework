﻿using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.GameObject;
using NWN.Framework.Core.Messaging;


// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal class mod_on_unequip
#pragma warning restore IDE1006 // Naming Styles
    {
        // ReSharper disable once UnusedMember.Local
        private static void Main()
        {
            NWObject equipper = Object.OBJECT_SELF;
            // Bioware Default
            _.ExecuteScript("x2_mod_def_unequ", equipper);

            MessageHub.Instance.Publish(new OnModuleUnequipItem());
        }
    }
}
