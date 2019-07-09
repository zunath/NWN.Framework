using System.Collections.Generic;
using System.Linq;

namespace NWN.Framework.Core.GameObject
{
    public class NWItem : NWObject
    {
        public NWItem(Object o)
            : base(o)
        {
        }

        public virtual NWCreature Possessor => _.GetItemPossessor(Object);

        public virtual int BaseItemType => _.GetBaseItemType(Object);

        public virtual bool IsDroppable
        {
            get => _.GetDroppableFlag(Object) == 1;
            set => _.SetDroppableFlag(Object, value ? 1 : 0);
        }

        public virtual bool IsCursed
        {
            get => _.GetItemCursedFlag(Object) == 1;
            set => _.SetItemCursedFlag(Object, value ? 1 : 0);
        }

        public virtual bool IsStolen
        {
            get => _.GetStolenFlag(Object) == 1;
            set => _.SetStolenFlag(Object, value ? 1 : 0);
        }

        public virtual bool IsIdentified
        {
            get => _.GetIdentified(Object) == 1;
            set => _.SetIdentified(Object, value ? 1 : 0);
        }
        public virtual int AC => _.GetItemACValue(Object);

        public virtual int Charges
        {
            get => _.GetItemCharges(Object);
            set => _.SetItemCharges(Object, value);
        }

        public virtual int ReduceCharges(int reduceBy = 1)
        {
            Charges = Charges - reduceBy;
            if (Charges < 0) Charges = 0;
            return Charges;
        }

        public virtual int StackSize
        {
            get => _.GetItemStackSize(Object);
            set => _.SetItemStackSize(Object, value);
        }

        public virtual float Weight => _.GetWeight(Object) * 0.1f;

        public virtual IEnumerable<ItemProperty> ItemProperties
        {
            get
            {
                for (ItemProperty ip = _.GetFirstItemProperty(Object); _.GetIsItemPropertyValid(ip) == _.TRUE; ip = _.GetNextItemProperty(Object))
                {
                    yield return ip;
                }
            }
        }

        // For the custom item properties:
        // If a toolset-placed item property exists, mark the value and remove the item property.
        // Otherwise, return the local variable stored on the item.
        

        public virtual void ReduceItemStack()
        {
            int stackSize = _.GetItemStackSize(Object);
            if (stackSize > 1)
            {
                _.SetItemStackSize(Object, stackSize - 1);
            }
            else
            {
                _.DestroyObject(Object);
            }
        }

        public virtual bool IsRanged => _.GetWeaponRanged(Object) == 1;

        //
        // -- BELOW THIS POINT IS JUNK TO MAKE THE API FRIENDLIER!
        //

        public static bool operator ==(NWItem lhs, NWItem rhs)
        {
            bool lhsNull = lhs is null;
            bool rhsNull = rhs is null;
            return (lhsNull && rhsNull) || (!lhsNull && !rhsNull && lhs.Object == rhs.Object);
        }

        public static bool operator !=(NWItem lhs, NWItem rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object o)
        {
            NWItem other = o as NWItem;
            return other != null && other == this;
        }

        public override int GetHashCode()
        {
            return Object.GetHashCode();
        }

        public static implicit operator Object(NWItem o)
        {
            return o.Object;
        }
        public static implicit operator NWItem(Object o)
        {
            return new NWItem(o);
        }
    }
}
