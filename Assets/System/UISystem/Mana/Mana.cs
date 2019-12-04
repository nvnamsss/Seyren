using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crom.System.UnitSystem;

namespace Assets.System.UISystem.Mana
{
    class ManaBar
    {
        private
            float maxMana;
        float currMana;

        public
            void Init(Unit character)
        {
            maxMana = character.MaxMp;
            currMana = character.CurrentMp;
        }

        void useSkill( int drain)
        {
           currMana = currMana - drain;
        }
    }
}
