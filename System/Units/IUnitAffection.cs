using System;
using Seyren.System.Damages;

namespace Seyren.System.Units {
    public interface IUnitAffection<T> where T : IUnit {
        bool OnAffect(T unit);
    }

    public class DealtDamage : IUnitAffection<IUnit>
    {
        DamageInfo damage;
        public DealtDamage(DamageInfo damage) {
            this.damage = damage;
        }

        public bool OnAffect(IUnit unit)
        {
            unit.Damage(damage);
            
            return false;
        }
    }

}
