using System;

namespace NWN.Framework.Core.Event
{
    public abstract class EventBase: MarshalByRefObject
    {
        public AppDomain OriginatingAppDomain { get; }

        public EventBase()
        {
            OriginatingAppDomain = AppDomain.CurrentDomain;
        }
    }
}
