using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem;

namespace Base2D.System.ItemSystem
{
    public class Armor : Item
    {
        public string name { get;set; }
        public string description { get;set; }
        public int ammount { get;set; }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }

        public ItemType itemType { get; }
    }
}
