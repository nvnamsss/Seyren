using Seyren.System.Units;
using UnityEngine;

namespace Seyren.System.Damages
{
    public static class Function
    {
        public static void Damage(Unit source, Unit target, float damage, DamageType type, TriggerType trigger)
        {
            if ((target.UnitStatus | UnitStatus.Invulnerable) == target.UnitStatus)
            {
                Debug.Log("Invulnerable");
                return;
            }

            DamageInfo damageInfo = new DamageInfo(source, target);

            damageInfo.TriggerType = trigger;
            damageInfo.PrePassive = source.Modification.PrePassive;
            damageInfo.Critical = source.Modification.Critical;
            damageInfo.Evasion = target.Modification.Evasion;
            damageInfo.Reduction = target.Modification.Reduction;
            damageInfo.PostPassive = source.Modification.PostPassive;

            damageInfo.DamageAmount = damage;
            damageInfo.DamageType = type;
            damageInfo.CalculateDamage();

            if (target.CurrentShield > 0 || target.CurrentPShield > 0 || target.CurrentMShield > 0)
            {
                if (damageInfo.DamageType == DamageType.Physical)
                {
                    float min = Mathf.Min(target.CurrentPShield, damageInfo.DamageAmount);
                    target.CurrentPShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Physical shield prevent: " + min);
                }

                if (damageInfo.DamageType == DamageType.Magical)
                {
                    float min = Mathf.Min(target.CurrentMShield, damageInfo.DamageAmount);
                    target.CurrentMShield -= min;
                    damageInfo.DamageAmount -= min;
                }

                if (damageInfo.DamageAmount > 0)
                {
                    float min = Mathf.Min(target.CurrentShield, damageInfo.DamageAmount);
                    target.CurrentShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Shield prevent: " + min);
                }
            }

            target.Damage(damageInfo);
            //TakeDamage?.Invoke(this, new TakeDamageEventArgs(damageInfo));
        }
    }
}