using Seyren.System.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Seyren.System.UISystem.Inventory
{
    class UIInventory: MonoBehaviour
    {
        #region Singleton
        public static UIInventory instance;
        void Awake() {

            if(instance!= null)
            {
                Debug.Log("singleton");
                return;
            }

            instance = this;
        }
        #endregion Singleton

        public int space = 20;

        public delegate void onItemChanged();
        public onItemChanged onItemChangedCallBack;

        public List<Item> items = new List<Item>();
        public List<int> itemCounts = new List<int>();
        public bool pickUp(Item item)
        {
            if(!item.instaUse){
                if(items.Contains(item)){
                    itemCounts[items.IndexOf(item)]++;
                    if (onItemChangedCallBack != null)
                        onItemChangedCallBack.Invoke();
                    return true;
                }
                else if(items.Count < space){
                    items.Add(item);
                    itemCounts.Add(1);
                    Debug.Log("idk");
                    if (onItemChangedCallBack != null)
                        onItemChangedCallBack.Invoke();
                    return true;                   
                }              
            }

            return false;
        }

        public void remove(Item item)
        {
            itemCounts.RemoveAt(items.IndexOf(item));
            items.Remove(item);      

            if (onItemChangedCallBack != null)
                onItemChangedCallBack.Invoke();
        }

        public void discardOrUse(Item item)
        {
            itemCounts[items.IndexOf(item)]--;
            Debug.Log(itemCounts[items.IndexOf(item)]);
            if (itemCounts[items.IndexOf(item)] <= 0)
            {
                remove(item);
            }
            else if (onItemChangedCallBack != null){
                onItemChangedCallBack.Invoke();
            }
        }

        public void sortInventory()
        {
            items = items.OrderBy(i => i.itemType).ToList();
        }

        public static void Swap<Item>(List<Item> list, int indexA, int indexB)
        {
            Item tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }
}
