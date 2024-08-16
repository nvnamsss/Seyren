// using Seyren.System.Units;
// using UnityEngine;

// namespace Seyren.System.Damages
// {
//     public static class DamageEngine
//     {
//         public static void DamageTarget(IUnit source, IUnit target, float damage, DamageType type, TriggerType trigger)
//         {
//             if ((target.ObjectStatus | ObjectStatus.Invulnerable) == target.ObjectStatus)
//             {
//                 Debug.Log("Invulnerable");
//                 return;
//             }

//             Damage damageInfo = new Damage(source, target);

//             damageInfo.TriggerType = trigger;
//             damageInfo.PrePassive = source.Modification.PrePassive;
//             damageInfo.Critical = source.Modification.Critical;
//             damageInfo.Evasion = target.Modification.Evasion;
//             damageInfo.Reduction = target.Modification.Reduction;
//             damageInfo.PostPassive = source.Modification.PostPassive;

//             damageInfo.DamageAmount = damage;
//             damageInfo.DamageType = type;
//             damageInfo.CalculateDamage();

//             if (target.State.CurrentShield > 0 || target.State.CurrentPShield > 0 || target.State.CurrentMShield > 0)
//             {
//                 if (damageInfo.DamageType == DamageType.Physical)
//                 {
//                     float min = Mathf.Min(target.State.CurrentPShield, damageInfo.DamageAmount);
//                     target.State.CurrentPShield -= min;
//                     damageInfo.DamageAmount -= min;

//                     UnityEngine.Debug.Log("Physical shield prevent: " + min);
//                 }

//                 if (damageInfo.DamageType == DamageType.Magical)
//                 {
//                     float min = Mathf.Min(target.State.CurrentMShield, damageInfo.DamageAmount);
//                     target.State.CurrentMShield -= min;
//                     damageInfo.DamageAmount -= min;
//                 }

//                 if (damageInfo.DamageAmount > 0)
//                 {
//                     float min = Mathf.Min(target.State.CurrentShield, damageInfo.DamageAmount);
//                     target.State.CurrentShield -= min;
//                     damageInfo.DamageAmount -= min;

//                     UnityEngine.Debug.Log("Shield prevent: " + min);
//                 }
//             }

//             target.DamageTarget(damageInfo);
//             //TakeDamage?.Invoke(this, new TakeDamageEventArgs(damageInfo));
//         }
//     }
// }