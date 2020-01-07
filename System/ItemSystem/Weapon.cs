using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem;

namespace Base2D.System.ItemSystem
{
    class Weapon : Item
    {
        public ItemType itemType { get; } = ItemType.Weapon;
        public int ammount { get; set; }
        public string name { get;set; }
        public string description { get;set; }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }
    }
}
