using Seyren.System.Items;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public GameObject tooltip;

    public Text itemName;

    public Text description;

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
        else{
            itemAmmountUI.enabled = false;
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
        InventoryManager.instance.discardOrUse(item);
        ConfirmDiscardDialog.SetActive(false);
    }

    public void onCancelDialog(){
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        ConfirmDiscardDialog.SetActive(false);
    }

    public void OnPointerEnter() 
    {
        Debug.Log("MouseOver");
        if(item != null){
            tooltip.SetActive(true);
            itemName.text = item.itemName;
            description.text = item.description;

        }
    }

    public void OnPointerExit()  {
        tooltip.SetActive(false);
    }  
}
