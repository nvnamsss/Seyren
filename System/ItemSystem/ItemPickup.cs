using UnityEngine;

namespace Base2D.System.ItemSystem{
    class ItemPickup: MonoBehaviour{

        public Item item;
        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player"){
                Pickup();
            }
        }
          
        void Pickup(){

            bool isPickedUp = Base2D.System.UISystem.Inventory.UIInventory.instance.pickUp(item);

            if(isPickedUp)
                Destroy(gameObject);
        }
    }
}