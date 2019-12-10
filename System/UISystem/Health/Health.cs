using Crom.System.UnitSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.UISystem.Health
{
    class HealthBar
    {
        private
           float maxHealth;
            float currHealth;

        public 
            void Init(Unit character)
        {
            maxHealth = character.MaxHp;
            currHealth = character.CurrentHp;
        }

        void takeDmg(float dmg)
        {
            currHealth = currHealth - dmg;
        }
    }
}
