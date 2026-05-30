using System;

namespace Seyren.System.Damages.Modifier
{
    /// <summary>
    /// Adds a fixed flat bonus to <see cref="Damage.BaseDamage"/> before resistance is applied.
    /// </summary>
    [Serializable]
    public class FlatDamageModifier : IModifier
    {
        public float Bonus;

        public FlatDamageModifier(float bonus)
        {
            Bonus = bonus;
        }

        public FlatDamageModifier() { }

        public void Apply(Damage damage)
        {
            damage.BaseDamage += Bonus;
        }
    }
}
