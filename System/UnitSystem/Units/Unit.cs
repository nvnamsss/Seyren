using Base2D.Init.DamageModification;
using Base2D.System.ActionSystem;
using Base2D.System.DamageSystem;
using Base2D.System.DamageSystem.Critical;
using Base2D.System.DamageSystem.Evasion;
using Base2D.System.UnitSystem.EventData;
using Base2D.System.UnitSystem.Projectiles;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Base2D.System.UnitSystem.Units
{
    public partial class Unit : MonoBehaviour, IObject, IAttribute
    {
        public Unit()
        {
            Attribute = new Attribute();
            Attribute.AttackDamage = 51;
            Attribute.HpRegen = 1;
            CurrentHp = Attribute.MaxHp = 100;
            CurrentPShield = 52;
            CurrentShield = 22;
            Modification = new ModificationInfos();
            Modification.Critical.AddModification(Critical.CriticalStrike());
        }

        void Start()
        {
            Body = GetComponent<Rigidbody2D>();
            Actions = new List<Action>();
            Abilites = new Dictionary<int, AbilitySystem.Ability>();
            Abilites.Add(Base2D.Init.Abilities.Attack.Id, new Base2D.Init.Abilities.Attack(this));
        }

        void Update()
        {
            Attribute.Update();
        }

        public bool IsEnemy(Unit unit)
        {
            return Player.Force.IsEnemy(unit.Player.Force);
        }
        /// <summary>
        /// Order source unit to deal damage to this unit
        /// </summary>
        /// <param name="source"></param>
        /// <param name="type"></param>
        public void Damage(Unit source, DamageType type)
        {
            Damage(source, type, TriggerType.All);
        }

        public void Damage(Unit source, DamageType type, TriggerType trigger)
        {
            Damage(source, Attribute.AttackDamage, type, trigger);
        }

        public void Damage(Unit source, float damage, DamageType type, TriggerType trigger)
        {
            DamageInfo damageInfo = new DamageInfo(this, source);

            damageInfo.TriggerType = trigger;
            damageInfo.PrePassive = source.Modification.PrePassive;
            damageInfo.Critical = source.Modification.Critical;
            damageInfo.Evasion = Modification.Evasion;
            damageInfo.Reduction = Modification.Reduction;
            damageInfo.PostPassive = source.Modification.PostPassive;

            damageInfo.DamageAmount = damage;
            damageInfo.DamageType = type;
            damageInfo.CalculateDamage();

            UnityEngine.Debug.Log(damageInfo);
            if (CurrentShield > 0 || CurrentPShield > 0 || CurrentMShield > 0)
            {
                //float min = 0;
                //switch (damageInfo.DamageType)
                //{
                //    case DamageType.Physical:
                //        min = Mathf.Min(Attribute.PhysicalShield, damageInfo.DamageAmount);
                //        Attribute.PhysicalShield -= min;
                //        damageInfo.DamageAmount -= min;
                //        break;
                //    case DamageType.Magical:
                //        min = Mathf.Min(Attribute.MagicShield, damageInfo.DamageAmount);
                //        Attribute.MagicShield -= min;
                //        damageInfo.DamageAmount -= min;
                //        break;
                //    case DamageType.Pure:
                //        min = Mathf.Min(Attribute.Shield, damageInfo.DamageAmount);
                //        Attribute.Shield -= min;
                //        damageInfo.DamageAmount -= min;
                //        break;
                //    case DamageType.OverTime:
                //        break;
                //    default:
                //        break;
                //}

                if (damageInfo.DamageType == DamageType.Physical)
                {
                    float min = Mathf.Min(CurrentPShield, damageInfo.DamageAmount);
                    CurrentPShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Physical shield prevent: " + min);
                }

                if (damageInfo.DamageType == DamageType.Magical)
                {
                    float min = Mathf.Min(CurrentMShield, damageInfo.DamageAmount);
                    CurrentMShield -= min;
                    damageInfo.DamageAmount -= min;
                }

                if (damageInfo.DamageAmount > 0)
                {
                    float min = Mathf.Min(CurrentShield, damageInfo.DamageAmount);
                    CurrentShield -= min;
                    damageInfo.DamageAmount -= min;

                    UnityEngine.Debug.Log("Shield prevent: " + min);

                }
            }

            CurrentHp = CurrentHp - damageInfo.DamageAmount;
            TakeDamage?.Invoke(this, new TakeDamageEventArgs(damageInfo));
        }

        /// <summary>
        /// Kill this unit
        /// </summary>
        /// <param name="killer"></param>
        public void Kill(Unit killer)
        {
            DyingHandler dying = Dying;
            UnitDyingEventArgs udinge = new UnitDyingEventArgs();
            if (dying != null)
            {
                dying.Invoke(this, udinge);
            }

            if (udinge.Cancel)
            {
                return;
            }

            DiedHandler died = Died;

            UnitDiedEventArgs udede = new UnitDiedEventArgs(killer);
            if (died != null)
            {
                died.Invoke(this, udede);
            }
        }

        public void Jump()
        {
            if (JumpTimes > 0)
            {
                Body?.AddForce(Vector2.up * Attribute.JumpSpeed, ForceMode2D.Impulse);
                JumpTimes -= 1;
            }
        }

        public void Move(Vector2 direction)
        {
            if ((UnitStatus | UnitStatus.Stun) == UnitStatus ||
                (UnitStatus | UnitStatus.Knockback) == UnitStatus)
            {
                return;
            }

            Body?.AddForce(direction * Attribute.MovementSpeed);
        }

        public void Look(Vector2 direction)
        {
            transform.rotation = Quaternion.Euler(direction.x, direction.y, 0);
        }

        public void Attack()
        {
            if (Abilites.ContainsKey(Base2D.Init.Abilities.Attack.Id))
            {
                Vector2 location = transform.position;
                Quaternion rotation = transform.rotation;
                Abilites[Base2D.Init.Abilities.Attack.Id].Create(location, rotation);
                
            }
        }
    }

}
       