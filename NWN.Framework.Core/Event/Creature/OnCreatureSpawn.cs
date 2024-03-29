﻿using System;
using NWN.Framework.Core.GameObject;

namespace NWN.Framework.Core.Event.Creature
{
    public class OnCreatureSpawn: EventBase
    {
        public NWCreature Self { get; }

        public OnCreatureSpawn()
        {
            Self = Object.OBJECT_SELF;
        }
    }
}
