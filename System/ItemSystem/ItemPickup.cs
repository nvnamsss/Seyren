using UnityEngine;

namespace Base2D.System.ItemSystem{
    class ItemPickup: MonoBehaviour{

        public Item item;

        public void Start(){
            if(item != null){
                gameObject.GetComponent<SpriteRenderer>().sprite = item.icon;
            }
        }
        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player"){
                Pickup();
            }
        }
          
        void Pickup(){

            bool isPickedUp = InventoryManager.instance.pickUp(item);

            if(isPickedUp)
                Destroy(gameObject);
        }
    }
}