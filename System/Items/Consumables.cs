using UnityEngine;

namespace Seyren.System.Items
{
    public class Consumables : Item
    {
        void OnAwake(){
            this.itemType = ItemType.Consumable;
        }

        public float hp;

        public float mp;

        public override void Use(){
            // Debug.Log("Current HP: " + HeroManager.instance.character.CurrentHp);
            // Debug.Log("Current MP: " + HeroManager.instance.character.CurrentMp);
            // Debug.Log(hp);
            // Debug.Log(mp);

            // HeroManager.instance.character.CurrentHp += hp;
            // if(HeroManager.instance.character.CurrentHp > HeroManager.instance.character.Attribute.MaxHp)
            //      HeroManager.instance.character.CurrentHp = HeroManager.instance.character.Attribute.MaxHp;
            // HeroManager.instance.character.CurrentMp += mp;
            // if(HeroManager.instance.character.CurrentMp > HeroManager.instance.character.Attribute.MaxMp)
            //      HeroManager.instance.character.CurrentMp = HeroManager.instance.character.Attribute.MaxMp;
            // if(this.instaUse == false){
            //     RemoveFromInventory(this);
            // }
            //     HUDManager.instance.updateMana(HeroManager.instance.character.CurrentMp);
            //     HUDManager.instance.heal();
            
        }
    }
}
