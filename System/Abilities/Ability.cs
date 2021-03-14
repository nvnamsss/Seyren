using System.Collections.Generic;
using System.Collections;
using Seyren.System.Actions;
using Seyren.System.Buffs;
using Seyren.System.Units;
using Seyren.System.Generics;
using UnityEngine;
using System;

namespace Seyren.System.Abilities
{
    public abstract class Ability
    {
        public static Error CooldownError { get; } = new Error("ability is cooldownling");
        public static Error ConditionError { get; } = new Error("condition is not satisfying");

        /// <summary>
        /// Trigger when ability cooldown is done and ability is ready to use
        /// </summary>
        public CastType CastType { get; protected set; }
        public float Cooldown { get; set; }
        public string abilityName;
        /// <summary>
        /// Time between every process for cooldown of an ability <br></br>
        /// </summary>
        // public float CooldownInterval { get; set; }
        public float CooldownRemaining
        {
            get
            {
                return (nextCooldown - DateTimeOffset.Now.ToUnixTimeMilliseconds()) / 1000;
            }
        }
        protected long nextCooldown;
        public float ManaCost { get; set; }
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value < 0)
                {
                    Debug.Log($"ability {abilityName} level cannot be set to {value}");
                    return;
                }

                _level = value;
            }
        }

        [SerializeField]
        protected int _level;

        public Ability(float cooldown, int level)
        {
            Cooldown = cooldown;
            _level = level;
        }

        public abstract long CastTime(Unit unit);

        public Error Cast(Unit by)
        {
            if (CooldownRemaining > 0) return CooldownError;
            Error err = Condition(by);
            if (err != null) return err;

            onCast(by);
            nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);
            return null;
        }

        public Error Cast(Unit by, Unit target)
        {
            if (CooldownRemaining > 0) return CooldownError;
            Error err = Condition(by, target);
            if (err != null) return err;

            onCast(by, target);
            nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);

            return null;
        }

        public Error CanCast(Unit by) {
            return Condition(by);
        }

        public Error CanCast(Unit unit, Unit target) {
            return Condition(unit ,target);
        }

        public Error CanCast(Unit by, Vector3 target) {
            return Condition(by, target);
        }

        public Error Cast(Unit by, Vector3 location)
        {
            if (CooldownRemaining > 0) return CooldownError;
            Error err = Condition(by, location);
            if (err != null) return err;

            onCast(by, location);
            nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);

            return null;
        }

        public abstract IAction Action(Unit by);
        public abstract IAction Action(Unit by, Unit target);
        public abstract IAction Action(Unit by, Vector3 target);
        
        protected abstract void onCast(Unit by);
        protected abstract void onCast(Unit by, Unit target);
        protected abstract void onCast(Unit by, Vector3 target);

        /// <summary>
        /// Ability will be cast if condition is true
        /// </summary>
        /// <returns></returns>
        protected abstract Error Condition(Unit by);
        protected abstract Error Condition(Unit by, Unit target);
        protected abstract Error Condition(Unit by, Vector3 target);
    }

}