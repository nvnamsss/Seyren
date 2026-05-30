using System;

namespace Seyren.System.Damages.Resistances
{
    /// <summary>
    /// Reduces incoming damage by a fixed flat amount via <see cref="Damage.ReducedDamage"/>.
    /// </summary>
    [Serializable]
    public class FlatDamageResistance : IResistance
    {
        public float Reduction;

        public FlatDamageResistance(float reduction)
        {
            Reduction = reduction;
        }

        public FlatDamageResistance() { }

        public void Apply(Damage damage)
        {
            damage.ReducedDamage += Reduction;
        }
    }
}
