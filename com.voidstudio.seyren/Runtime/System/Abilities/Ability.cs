using System.Collections.Generic;
using System.Collections;
using Seyren.System.Actions;
using Seyren.System.Units;
using Seyren.System.Generics;
using UnityEngine;
using System;

namespace Seyren.System.Abilities
{
    public class AbilityTarget
    {
        public TargetingType TargetingType => targetType;
        public IUnit Source => source;
        public IUnit Target => target;
        public Vector3 Location => location;
        TargetingType targetType;
        IUnit source;
        IUnit target;
        Vector3 location;
        float radius;

        public static AbilityTarget NoTarget(IUnit source)
        {
            AbilityTarget at = new AbilityTarget();
            at.source = source;
            at.targetType = TargetingType.NoTarget;
            return at;
        }

        public static AbilityTarget PointTarget(IUnit source, Vector3 location)
        {
            AbilityTarget at = new AbilityTarget();
            at.source = source;
            at.location = location;
            at.targetType = TargetingType.PointTarget;
            return at;
        }

        public static AbilityTarget UnitTarget(IUnit source, IUnit target)
        {
            AbilityTarget at = new AbilityTarget();
            at.source = source;
            at.target = target;
            at.targetType = TargetingType.UnitTarget;
            return at;
        }
    }

    public abstract class Ability
    {
        public static Error ErrorCooldown { get; } = new Error("ability is cooldownling");
        public static Error ErrorCondition { get; } = new Error("condition is not satisfying");
        public static Error ErrorCannotNoTarget { get; } = new Error("targeting must be no target");
        public static Error ErrorCannotUnitTarget { get; } = new Error("targeting must be unit target");
        public static Error ErrorCannotPointTarget { get; } = new Error("targeting must be point target");
        public static Error ErrorCannotCastOnAllied { get; } = new Error("cannot cast on allied");
        public static Error ErrorCannotCastOnSelf { get; } = new Error("cannot cast on self");
        public static Error ErrorCannotCastOnEnemy { get; } = new Error("cannot cast on enemy");

        /// <summary>
        /// Trigger when ability cooldown is done and ability is ready to use
        /// </summary>
        public CastType CastType { get; protected set; }
        public TargetingType Targeting { get; protected set; }
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

        public Ability(int level)
        {
            _level = level;
        }

        protected AbilityTarget abilityTarget;
        public virtual Error Cast(IUnit by)
        {
            abilityTarget = AbilityTarget.NoTarget(by);
            onCast();
            nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);
            return null;
        }
        public virtual Error Cast(IUnit by, IUnit target)
        {
            Debug.Log("Cast");
            abilityTarget = AbilityTarget.UnitTarget(by, target);
            onCast();
            nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);

            return null;
        }

        public virtual Error Cast(IUnit by, Vector3 location)
        {
            Debug.Log("Cast");
            abilityTarget = AbilityTarget.PointTarget(by, location);
            onCast();
            nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);

            return null;
        }

        public Error CanCast(IUnit by)
        {
            if (Targeting != TargetingType.NoTarget)
            {
                return ErrorCannotCastOnSelf;
            }

            if (CooldownRemaining > 0) return ErrorCooldown;
            return Condition(by);
        }

        public Error CanCast(IUnit unit, IUnit target)
        {
            if ((Targeting | TargetingType.UnitTarget) != Targeting)
            {
                return ErrorCannotCastOnSelf;
            }

            if (CooldownRemaining > 0) return ErrorCooldown;
            return Condition(unit, target);
        }

        public Error CanCast(Unit by, Vector3 target)
        {
            if ((Targeting | TargetingType.PointTarget) != Targeting)
            {
                return ErrorCannotCastOnSelf;
            }
            if (CooldownRemaining > 0) return ErrorCooldown;
            return Condition(by, target);
        }

        public abstract IAction Action(IUnit by);
        public abstract IAction Action(IUnit by, IUnit target);
        public abstract IAction Action(IUnit by, Vector3 target);

        protected abstract void onCast();
        // protected abstract void onCast(Unit by);
        // protected abstract void onCast(Unit by, Unit target);
        // protected abstract void onCast(Unit by, Vector3 target);

        /// <summary>
        /// Ability will be cast if condition is true
        /// </summary>
        /// <returns></returns>
        protected abstract Error Condition(IUnit by);
        protected abstract Error Condition(IUnit by, IUnit target);
        protected abstract Error Condition(IUnit by, Vector3 target);
        public abstract Ability Clone();
    }

}