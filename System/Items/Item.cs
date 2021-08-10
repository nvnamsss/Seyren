using Seyren.System.Damages;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.System.Items
{
    public interface IItem
    {
        string Name { get; }
        string Tooltip { get; }
        int MaxStack { get; }

    }

    public interface IEquipable : IItem
    {
        Unit EquipBy { get; }
        bool Equip(Unit by);
        bool UnEquip();
    }

    public interface IConsumables : IItem
    {
        bool Consume(Unit by);
    }

    public interface ICellItem : IItem
    {
        int Width { get; }
        int Height { get; }
    }

    // public partial class Item : MonoBehaviour, IAttribute
    // {
    //     public Item()
    //     {
    //         Attribute = new Attribute();
    //     }

    //     public Attribute Attribute { get; set; }
    //     public ModificationInfos Modification { get; set; }
    //     public string itemName;
    //     public string description;
    //     public ItemType itemType;

    //     public Sprite icon;
    //     public bool instaUse = false;

    //     public virtual void Use()
    //     {

    //     }

    //     void Awake()
    //     {
    //         //Attribute = Attribute == null ? ScriptableObject.CreateInstance<Attribute>() : Attribute;
    //     }

    //     public void RemoveFromInventory(Item item)
    //     {
    //         InventoryManager.instance.discardOrUse(item);
    //     }
    // }
}
