using UnityEngine;

namespace Base2D.System.ItemSystem
{
    public class Consumables : Item
    {
        void OnAwake(){
            this.itemType = ItemType.Consumable;
        }

        public float hp;

        public float mp;

        public override void Use(){
            HeroManager.instance.character.CurrentHp += hp;
            if(HeroManager.instance.character.CurrentHp > HeroManager.instance.character.Attribute.MaxHp)
                 HeroManager.instance.character.CurrentHp = HeroManager.instance.character.Attribute.MaxHp;
            HeroManager.instance.character.CurrentMp += mp;
            if(HeroManager.instance.character.CurrentMp > HeroManager.instance.character.Attribute.MaxMp)
                 HeroManager.instance.character.CurrentMp = HeroManager.instance.character.Attribute.MaxMp;
            if(this.instaUse == false){
                RemoveFromInventory(this);
            }
                HUDManager.instance.updateMana();
                HUDManager.instance.heal();
            
        }
    }
}
