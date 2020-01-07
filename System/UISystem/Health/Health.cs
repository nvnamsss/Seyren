using Base2D.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.UISystem.Health
{
    class HealthBar
    {
        private
           float maxHealth;
            float currHealth;

        public 
            void Init(Unit character)
        {
            maxHealth = character.Attribute.MaxHp;
            currHealth = character.CurrentHp;
        }

        void takeDmg(float dmg)
        {
            currHealth = currHealth - dmg;
        }
    }
}
