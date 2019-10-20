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
            int maxHealth;
            int currHealth;

        public 
            void Init(HealthBar h, Object character)
        {
            h.maxHealth = character.maxHealth;
            h.currHealth = character.currHealth;
        }

        void takeDmg(HealthBar h)
        {
            h.currHealth = h.currHealth - 1;
        }
    }
}
