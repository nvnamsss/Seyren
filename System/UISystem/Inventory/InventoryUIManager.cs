using Base2D.System.UISystem.Inventory;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform itemParent;

    public GameObject inventoryUI;

    UIInventory inventory;

    InventorySlot[] slots;
    void Start()
    {
        inventory = Base2D.System.UISystem.Inventory.UIInventory.instance;
        inventory.onItemChangedCallBack += updateUI;

        slots = itemParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("i")){
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }   
    }

    void updateUI(){
        for (int i = 0 ; i< slots.Length;i++){
            if(i< inventory.items.Count){
                slots[i].addItemToSlot(inventory.items[i], inventory.itemCounts[i]);
            }
            else{
                slots[i].clearItemFromSlot();
            }
        }
    }
}
