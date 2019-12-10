using Base2D.System.UnitSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ItemSystem
{
    interface Item : IAttribute
    {
        string name { get; set; }
        string description { get; set; }
        ItemType itemType { get; }
        int ammount { get; set; }
    }
}
