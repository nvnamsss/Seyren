// using Seyren.System.Damages;
// using Seyren.System.Units;
// using UnityEngine;

using Seyren.System.Units;

namespace Seyren.System.Items
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public class UseItemData
    {
        public IUnit user;
        public IUnit targetUnit;
        public IObject targetObject;
    }

    /// <summary>
    /// Interface for all items in the game system
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Unique identifier for this specific item instance
        /// </summary>
        string ID { get; set; }
        
        /// <summary>
        /// Type identifier that categorizes this item (e.g., "weapon", "armor", "consumable")
        /// </summary>
        string TypeId { get; set; }
        
        /// <summary>
        /// Display name of the item shown to players
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// Detailed description of the item's properties and effects
        /// </summary>
        string Description { get; set; }
        
        /// <summary>
        /// Current quantity or count of this item instance
        /// </summary>
        int Count { get; set; }
        
        /// <summary>
        /// Width of the item in inventory grid cells (horizontal size)
        /// </summary>
        int Width { get; set; }
        
        /// <summary>
        /// Height of the item in inventory grid cells (vertical size)
        /// </summary>
        int Height { get; set; }
        
        /// <summary>
        /// Rarity level of the item (affects value, color coding, drop rates)
        /// </summary>
        int Rarity { get; set; }
        
        /// <summary>
        /// Maximum number of items that can be stacked together in a single inventory slot.
        /// 
        /// MaxStack of 1 means the item is non-stackable
        /// </summary>
        int MaxStack { get; set; }
        
        /// <summary>
        /// Activates the item's effect when used by a unit
        /// </summary>
        /// <param name="data">Context data containing user, target unit, and target object</param>
        void Use(UseItemData data);
        
        /// <summary>
        /// Creates a deep copy of this item with identical properties
        /// </summary>
        /// <returns>A new IItem instance with the same values</returns>
        IItem Clone();
    }
}

// {
//     public interface IItem
//     {
//         string Name { get; }
//         string Tooltip { get; }
//         int MaxStack { get; }

//     }

//     public interface IEquipable : IItem
//     {
//         Unit EquipBy { get; }
//         bool Equip(Unit by);
//         bool UnEquip();
//     }

//     public interface IConsumables : IItem
//     {
//         bool Consume(Unit by);
//     }

//     public interface ICellItem : IItem
//     {
//         int Width { get; }
//         int Height { get; }
//     }

//     // public partial class Item : MonoBehaviour, IAttribute
//     // {
//     //     public Item()
//     //     {
//     //         Attribute = new Attribute();
//     //     }

//     //     public Attribute Attribute { get; set; }
//     //     public ModificationInfos Modification { get; set; }
//     //     public string itemName;
//     //     public string description;
//     //     public ItemType itemType;

//     //     public Sprite icon;
//     //     public bool instaUse = false;

//     //     public virtual void Use()
//     //     {

//     //     }

//     //     void Awake()
//     //     {
//     //         //Attribute = Attribute == null ? ScriptableObject.CreateInstance<Attribute>() : Attribute;
//     //     }

//     //     public void RemoveFromInventory(Item item)
//     //     {
//     //         InventoryManager.instance.discardOrUse(item);
//     //     }
//     // }
// }
