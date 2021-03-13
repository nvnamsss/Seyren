using System;
using Seyren.System.Items;
using Seyren.System.Units;

namespace Seyren.Examples.Items {
    public abstract class Potion : Item, IConsumables
    {
        public int Cooldown;
        public long LastUse;

        public bool Consume(Unit by)
        {
            bool used = consume(by);
            if (used) {
                LastUse = DateTimeOffset.Now.ToUnixTimeSeconds();
            }
            return used;
        }

        protected abstract bool consume(Unit by);
        protected override int stack()
        {
            return 99;
        }
        protected override int width()
        {
            return 1;
        }

        protected override int height()
        {
            return 1;
        }
    }

    public class LesserHPPotion : Potion
    {
        protected override bool consume(Unit by)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() < LastUse + Cooldown) {
                return false;
            }

            by.CurrentHp += 1000;
            return true;
        }

        protected override string name()
        {
            return "Lesser HP Potion";
        }

        protected override string tooltip()
        {
            throw new NotImplementedException();
        }
    }

    public class LesserMPPotion : Potion
    {
        protected override bool consume(Unit by)
        {
            by.CurrentMp += 1000;
            return true;
        }

        protected override string name()
        {
            return "Lesser Mana Potion";
        }

        protected override string tooltip()
        {
            throw new NotImplementedException();
        }
    }
}
