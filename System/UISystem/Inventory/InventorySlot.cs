using Base2D.System.ItemSystem;
using Base2D.System.UISystem.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item;

    int ammount;

    public Text itemAmmountUI;

    public Button removeButton;

    public Image icon;

    public Button useButton;

    public GameObject ConfirmDiscardDialog;

    public Button yesButton;

    public Button noButton;

    public void addItemToSlot(Item newItem, int ammountItem){
        item = newItem;

        ammount = ammountItem;

        icon.sprite = item.icon;
        icon.enabled = true;

        removeButton.interactable = true;

        if(ammount >= 2){
            itemAmmountUI.enabled = true;
            itemAmmountUI.text = ammount.ToString();
        }
    }

    public void clearItemFromSlot(){
        item = null;

        icon.sprite = null;
        icon.enabled = false;

        removeButton.interactable = false;

        itemAmmountUI.enabled = false;
    }

    public void onRemoveButton(){
        ConfirmDiscardDialog.SetActive(true);
        yesButton.onClick.AddListener(onConfirmDialog);
        noButton.onClick.AddListener(onCancelDialog);
        //Base2D.System.UISystem.Inventory.UIInventory.instance.discardOrUse(item);
    }

    public void onUseButton(){
        if (item!= null){
            item.Use();
        }
    }

    public void onConfirmDialog(){
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        Base2D.System.UISystem.Inventory.UIInventory.instance.discardOrUse(item);
        ConfirmDiscardDialog.SetActive(false);
    }

    public void onCancelDialog(){
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        ConfirmDiscardDialog.SetActive(false);
    }
}
