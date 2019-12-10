using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crom.System.DamageSystem;
using Crom.System.UnitSystem;

namespace Assets.System.ItemSystem
{
    class Consumables : Item
    {
        public string name { get;set; }
        public string description { get;set; }

        public ItemType itemType { get; } = ItemType.Consumable;

        public int ammount { get;set; }
        public Crom.System.UnitSystem.Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }
    }
}
