using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.System.UISystem.Mana
{
    class ManaBar
    {
        private
            int maxMana;
        int currMana;

        public
            void Init(ManaBar m, Object character)
        {
            m.maxMana = character.maxMana;
            m.currMana = character.currMana;
        }

        void useSkill(ManaBar m, int drain)
        {
            m.currMana = m.currMana - drain;
        }
    }
}
