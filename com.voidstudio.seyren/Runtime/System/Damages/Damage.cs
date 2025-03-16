using Seyren.System.Units;

namespace Seyren.System.Damages
{
    public struct Damage
    {
        public IUnit Source { get; set; }
        public IUnit Target { get; set; }
        public TriggerType TriggerType { get; set; }
        public DamageType DamageType { get; set; }

        public float BaseDamage;
        public float CriticalDamage;
        public float ReducedDamage;
        public bool Evaded;

        public float ArmorPenetration;
        public float ArmorPenetrationFlat;

        public float MagicPenetration;
        public float MagicPenetrationFlat;
        public bool TriggerOnHitEffect;

        public float FinalDamage()
        {
            if (Evaded) return 0;
            return BaseDamage + CriticalDamage - ReducedDamage;
        } 

        public override string ToString()
        {
            return $"Damage[Source={Source}, Target={Target}, Type={DamageType}, Base={BaseDamage}, Crit={CriticalDamage}, Red={ReducedDamage}, Final={FinalDamage()}]";
        }
    }
}
