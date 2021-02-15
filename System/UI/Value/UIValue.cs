using Seyren.System.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.UISystem.Value
{
    class UIValue
    {
        public float maxHealth;
        public float currHealth;
        public float maxMana;
        public float currMana;
        public void Init(Unit character)
        {
            maxHealth = character.Attribute.MaxHp;
            currHealth = character.CurrentHp;
            maxMana = character.Attribute.MaxMp;
            currMana = character.CurrentMp;
        }

        public void takeDmg(float dmg)
        {
            currHealth = currHealth - dmg;
        }

        public void useSkill(int drain)
        {
            currMana = currMana - drain;
        }
    }
}
