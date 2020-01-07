using Base2D.System.UnitSystem.Units;

namespace Base2D.System.UISystem.Mana
{
    class ManaBar
    {
        private float maxMana;
        private float currMana;

        public
            void Init(Unit character)
        {
            maxMana = character.Attribute.MaxMp;
            currMana = character.CurrentMp;
        }

        void useSkill( int drain)
        {
           currMana = currMana - drain;
        }
    }
}
