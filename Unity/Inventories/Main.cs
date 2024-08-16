using System.Collections;
using System.Collections.Generic;
using Seyren.Examples.Items;
using UnityEngine;


namespace Seyren.Examples.Inventories
{
    public class Main : MonoBehaviour {
        public MultiCellInventoryManager inventory;
        public MyItem item;
        public MyItem item2;
        private void Start() {
            Debug.Log("AAA");
            inventory.pickUp(item);
            inventory.pickUp(item2);
        }

        private void Update() {
            
        }
    }
}