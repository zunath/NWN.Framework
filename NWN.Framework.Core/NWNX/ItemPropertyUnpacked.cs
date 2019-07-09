using NWN.Framework.Core.GameObject;

namespace NWN.Framework.Core.NWNX
{
    public class ItemPropertyUnpacked
    {
        public int ItemPropertyID { get; set; }
        public int Property { get; set; }
        public int SubType { get; set; }
        public int CostTable { get; set; }
        public int CostTableValue { get; set; }
        public int Param1 { get; set; }
        public int Param1Value { get; set; }
        public int UsesPerDay { get; set; }
        public int ChanceToAppear { get; set; }
        public bool IsUseable { get; set; }
        public int SpellID { get; set; }
        public NWObject Creator { get; set; }
        public string Tag { get; set; }

        public ItemPropertyUnpacked()
        {
            Creator = new NWObject(Object.OBJECT_INVALID);
            Tag = string.Empty;
        }
    }
}
