using Base2D.System.DamageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ItemSystem
{
    class Armor : Item
    {
        public string name { get;set; }
        public string description { get;set; }
        public int ammount { get;set; }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get; set; }

        public ItemType itemType { get; }
    }
}
