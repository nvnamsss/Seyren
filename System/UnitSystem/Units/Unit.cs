﻿using Base2D.Init.DamageModification;
using Base2D.System.TerrainSystem;
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
            //CurrentHp = Attribute.MaxHp = 100;
            //CurrentPShield = 52;
            //CurrentMShield = 0;
            //CurrentShield = 22;
            JumpTimes = 1;
            Modification = new ModificationInfos();
        }

        void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            Action = gameObject.AddComponent<Action>();
            Abilites = new Dictionary<int, AbilitySystem.Ability>();
            Active = true;
        }
        void Start()
        {

#if UNITY_EDITOR
            Attribute.Strength = Strength;
            Attribute.Agility = Agility;
            Attribute.Intelligent = Intelligent;

            Attribute.AttackDamage = AttackDamage;
            Attribute.MDamageAmplified = MDamageAmplified;

            Attribute.MaxHp = MaxHp;
            Attribute.MaxMp = MaxMp;
            Attribute.HpRegen = HpRegen;
            Attribute.MpRegen = MpRegen;
            Attribute.ShieldRegen = ShieldRegen;
            Attribute.MShieldRegen = MShieldRegen;
            Attribute.PShield = PShield;
            Attribute.HpRegenPercent = HpRegenPercent;
            Attribute.MpRegenPercent = MpRegenPercent;

            Attribute.Armor = Armor;
            Attribute.MArmor = MArmor;

            Attribute.AttackRange = AttackRange;
            Attribute.CastRange = CastRange;

            Attribute.MovementSpeed = MovementSpeed;
            Attribute.AttackSpeed = AttackSpeed;
            Attribute.JumpSpeed = JumpSpeed;
#endif
            _currentJump = JumpTimes;
            _currentHp = Attribute.MaxHp;
            _currentMp = Attribute.MaxMp;
            _currentPShield = Attribute.PShield;
            _currentMShield = Attribute.MShield;

            if(this.GetType() == typeof(Hero)){
                HUDManager.instance.setHealth();
                HUDManager.instance.updateMana(CurrentMp);
                StatusUIManager.instance.setStats();
            }
        }

        void Update()
        {
            Attribute.Update();
            //UpdateGrounding();
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
            Damage(source, source.Attribute.AttackDamage, type, trigger);
        }

        public void Damage(Unit source, float damage, DamageType type, TriggerType trigger)
        {
            if ((UnitStatus | UnitStatus.Invulnerable) == UnitStatus)
            {
                Debug.Log("Invulnerable");
                return;
            }
            
            DamageInfo damageInfo = new DamageInfo(source, this);

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
            if (TakeDamage != null)
            {
                TakeDamage.Invoke(this, new TakeDamageEventArgs(damageInfo));
            }
            damageSource = source;
            //TakeDamage?.Invoke(this, new TakeDamageEventArgs(damageInfo));
        }

        /// <summary>
        /// Kill this unit
        /// </summary>
        /// <param name="killer"></param>
        public void Kill(Unit killer)
        {
            Active = false;
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

        public bool Jump()
        {
            if (!Active)
            {
                return false;
            }

            if (_currentJump > 0)
            {
                if (StandOn == GroundType.Ground || StandOn == GroundType.Grass)
                {
                    Body.AddForce(Vector2.up * Attribute.JumpSpeed, ForceMode2D.Impulse);
                    _currentJump -= 1;

                    return true;
                }
            }
            else
            {

            }

            return false;
        }

        public virtual void Move(Vector2 direction)
        {
            if (!Active)
            {
                return;
            }

            if ((UnitStatus | UnitStatus.Stun) == UnitStatus ||
                (UnitStatus | UnitStatus.Knockback) == UnitStatus)
            {
                return;
            }

            Vector2 translate = direction * Attribute.MovementSpeed;
            transform.position += (Vector3)translate;
        }

        public void Look(Vector2 direction)
        {
            if (!Active)
            {
                return;
            }

            transform.rotation = Quaternion.Euler(direction.x, direction.y, 0);
        }

        public void Attack()
        {
            if (!Active)
            {
                return;
            }
        }

        public bool Spell(int spellId)
        {
            if (!Active)
            {
                return false;
            }

            if (Abilites.ContainsKey(spellId))
            {
                return Abilites[spellId].Cast();
            }
            else
            {
                Debug.LogWarning("[Unit] - Unit " + name + " do not contain " + spellId + " ability");
            }

            return false;
        }

        private void UpdateGrounding()
        {
            if (Collider.IsTouchingLayers(Ground.Grass))
            {
                StandOn = GroundType.Grass;
                _currentJump = JumpTimes;
            }
            else if (Collider.IsTouchingLayers(Ground.Hard))
            {
                StandOn = GroundType.Ground;
                _currentJump = JumpTimes;
            }
            else
                StandOn = GroundType.Unknown;
        }

        public void OnCollisionEnter2D(Collision2D other) {
            if(other.transform.tag == "GrassGround"){
            StandOn = GroundType.Grass;
            _currentJump = JumpTimes;
            }
            else if(other.transform.tag == "HardGround"){
            StandOn = GroundType.Ground;
            _currentJump = JumpTimes;
            }
            else if(other.transform.tag == "Enemy"){
            StandOn = GroundType.Ground;
            _currentJump = JumpTimes;
            }
            
        }

        public void OnCollisionExit2D(Collision2D other) {
                StandOn = GroundType.Unknown;
            
        }
    }

}
