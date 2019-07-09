using NWN.Framework.Core.Event.Creature;
using NWN.Framework.Core.Messaging;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal class crea_on_attacked
#pragma warning restore IDE1006 // Naming Styles
    {
        public static void Main()
        {
            MessageHub.Instance.Publish(new OnCreaturePhysicalAttacked());
        }
    }
}
