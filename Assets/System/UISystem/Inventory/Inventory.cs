using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.UISystem.Inventory
{
    class Inventory
    {
        private
            List<Item> items;

        public void pickUp(Inventory inven, Item item)
        {
            inven.items.Add(item);
        }

        public void discardOrUse(Inventory inven, Item item)
        {
            inven.items.Remove(item);
        }
    }
}
