﻿using NWN.Framework.Core.Event.Module;
using NWN.Framework.Core.Messaging;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal class mod_on_csabort
#pragma warning restore IDE1006 // Naming Styles
    {
        // ReSharper disable once UnusedMember.Local
        private static void Main()
        {
            MessageHub.Instance.Publish(new OnModuleCutsceneAbort());
        }
    }
}
