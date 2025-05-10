using System;

namespace Seyren.System.Formulas
{
    public static class DotaFormula
    {
        private static float armorFormula = 0.06f;
        private static float magicResistanceFormula = 0.25f;
        public static float CalculatePhysicalDamage(float baseDamage, float armor)
        {
            float DMu = 1 - armorFormula * armor / (1 + armorFormula * Math.Abs(armor));
            DMu = Math.Clamp(DMu, 0.0f, 2.0f);
            return baseDamage * DMu;
        }
    }
}