using System;
using System.Diagnostics;

namespace NWN.Framework.Core.Messaging
{
    internal sealed class Subscription: MarshalByRefObject
    {
        private const long TicksMultiplier = 1000 * TimeSpan.TicksPerMillisecond;
        private readonly long _throttleByTicks;
        private double? _lastHandleTimestamp;

        internal Subscription(Type type, Guid token, TimeSpan throttleBy, object handler)
        {
            Type = type;
            Token = token;
            Handler = handler;
            _throttleByTicks = throttleBy.Ticks;
        }

        internal void Handle<T>(T message)
        {
            if (!CanHandle()) { return; }

            // This is a bit of a hack to get around some type comparison issues between AppDomains.
            // If you have a better way to do this, please fix it.
            dynamic dynamicHandler = Handler;
            dynamic dynamicMessage = message;
            dynamicHandler(dynamicMessage);

            // Original code here.
            //((Action<T>)Handler)(message);
        }

        private bool CanHandle()
        {
            if (_throttleByTicks == 0) { return true; }

            if (_lastHandleTimestamp == null)
            {
                _lastHandleTimestamp = Stopwatch.GetTimestamp();
                return true;
            }

            var now = Stopwatch.GetTimestamp();
            var durationInTicks = (now - _lastHandleTimestamp) / Stopwatch.Frequency * TicksMultiplier;

            if (durationInTicks >= _throttleByTicks)
            {
                _lastHandleTimestamp = now;
                return true;
            }

            return false;
        }

        internal Guid Token { get; }
        internal Type Type { get; }
        private object Handler { get; }
    }
}