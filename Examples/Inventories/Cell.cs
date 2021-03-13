using System.Collections;
using System.Collections.Generic;
using Seyren.Examples.Items;
using Seyren.System.Items;
using Seyren.System.Inventories;
using Seyren.System.Units;
using UnityEngine;
using Seyren.System.Generics;
using UnityEngine.UI;

namespace Seyren.Examples.Inventories
{
    public class Cell : MonoBehaviour
    {
        public event GameEventHandler<Cell> OnClick;
        public event GameEventHandler<Cell> OnHover;
        public event GameEventHandler<Cell> OnOut;
        // an image here
        public Item item;
        public Button button;
        public int row;
        public int column;
        int ammount;

        private void Awake() {
            button.onClick.AddListener(() => {
                OnClick?.Invoke(this);
            });

        }

        public void onConfirmDialog()
        {
            // yesButton.onClick.RemoveAllListeners();
            // noButton.onClick.RemoveAllListeners();
            // InventoryManager.instance.discardOrUse(item);
            // ConfirmDiscardDialog.SetActive(false);
        }

        public void onCancelDialog()
        {
            // yesButton.onClick.RemoveAllListeners();
            // noButton.onClick.RemoveAllListeners();
            // ConfirmDiscardDialog.SetActive(false);
        }

        public void OnPointerEnter()
        {
            OnHover?.Invoke(this);
        }

        public void OnPointerExit()
        {
            OnOut?.Invoke(this);
            // tooltip.SetActive(false);
        }

    }
}
