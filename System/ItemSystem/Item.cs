using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem;
using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public partial class Item : MonoBehaviour, IAttribute
    {
        public Item(){
            Attribute = new Attribute();
        }
        public Attribute Attribute { get; set; }
        public ModificationInfos Modification { get ;set; }
        public string itemName;
        public string description;
        public ItemType itemType;

        public Sprite icon;
        public bool instaUse = false;

        public virtual void Use(){

        }

        void Start(){
            #if UNITY_EDITOR
            Attribute.Strength = Strength;
            Attribute.Agility = Agility;
            Attribute.Intelligent = Intelligent;

            Attribute.AttackDamage = AttackDamage;
            Attribute.MDamageAmplified = MDamageAmplified;

            Attribute.MaxHp = MaxHp;
            Attribute.MaxMp = MaxMp;
            Attribute.HpRegen = HpRegen;
            Attribute.MpRegen = MpRegen;
            Attribute.ShieldRegen = ShieldRegen;
            Attribute.MShieldRegen = MShieldRegen;
            Attribute.PShield = PShield;
            Attribute.HpRegenPercent = HpRegenPercent;
            Attribute.MpRegenPercent = MpRegenPercent;

            Attribute.Armor = Armor;
            Attribute.MArmor = MArmor;

            Attribute.AttackRange = AttackRange;
            Attribute.CastRange = CastRange;

            Attribute.MovementSpeed = MovementSpeed;
            Attribute.AttackSpeed = AttackSpeed;
            Attribute.JumpSpeed = JumpSpeed;
#endif
        }

        public void RemoveFromInventory(Item item){
            InventoryManager.instance.discardOrUse(item);
        }
    }
}
