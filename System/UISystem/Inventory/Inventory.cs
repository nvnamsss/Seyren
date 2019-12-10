using Assets.System.ItemSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.UISystem.Inventory
{
    class Inventory : UIComponent
    {
        private
            List<Item> items;

        public void pickUp(Item item)
        {
            items.Add(item);
        }

        public void discardOrUse(int position)
        {
            items[position].ammount--;
            if (items[position].ammount == 0)
                items.RemoveAt(position);
        }

        public void sortInventory()
        {
            items = items.OrderBy(i => i.itemType).ToList();
        }

        public void populate(UIInventory ui, Inventory inven) {
            foreach (Item i in inven.items)
            {
                //fill the inventory
            }
        }

        public static void Swap<Item>(List<Item> list, int indexA, int indexB, UIInventory ui)
        {
            Item tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            ui.update();
        }
    }
}
