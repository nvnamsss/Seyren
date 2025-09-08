using UnityEngine;
using Seyren.System.Units;
using Seyren.System.States;

namespace Seyren.System.Buffs
{
    /// <summary>
    /// Base class for all buff implementations providing common functionality
    /// </summary>
    public abstract class BaseBuff : IBuff
    {
        protected string id;
        protected IUnit owner;
        protected IUnit target;
        protected float duration;
        protected float remainingTime;

        public BaseBuff(string buffId, float buffDuration)
        {
            id = buffId;
            duration = buffDuration;
            remainingTime = buffDuration;
        }

        public virtual string GetId() => id;
        public virtual IUnit GetOwner() => owner;
        public virtual IUnit GetTarget() => target;
        public virtual float GetDuration() => duration;
        public virtual float GetRemainingTime() => remainingTime;
        public virtual bool IsExpired() => remainingTime <= 0f;

        /// <summary>
        /// Applies this buff to the specified target unit
        /// </summary>
        /// <param name="buffTarget">The unit this buff will affect</param>
        public virtual void ApplyBuffToUnit(IUnit buffTarget)
        {
            target = buffTarget;
            OnApply();
        }

        public abstract void OnApply();
        public abstract void OnExpire();

        public virtual void OnTick(float deltaTime)
        {
            remainingTime -= deltaTime;
            PerformTickEffect(deltaTime);
        }

        protected virtual void PerformTickEffect(float deltaTime) { }
    }

    /// <summary>
    /// Devotion Aura - Increases armor of nearby allies (Warcraft 3)
    /// </summary>
    public class DevotionAuraBuff : BaseBuff
    {
        private readonly float armorBonus;
        private readonly string bonusKey;

        public DevotionAuraBuff(float armorIncrease, float duration = -1f) 
            : base("devotion_aura", duration)
        {
            armorBonus = armorIncrease;
            bonusKey = $"devotion_aura_defense_{GetHashCode()}";
        }

        public override void OnApply()
        {
            var defenseAttribute = target.Attribute.GetBaseFloat(AttributeName.DEFENSE);
            defenseAttribute.AddBonus(bonusKey, armorBonus);
            
            Debug.Log($"Devotion Aura applied to {target.ID}: +{armorBonus} armor");
        }

        public override void OnExpire()
        {
            var defenseAttribute = target.Attribute.GetBaseFloat(AttributeName.DEFENSE);
            defenseAttribute.RemoveBonus(bonusKey);
            
            Debug.Log($"Devotion Aura expired on {target.ID}");
        }
    }

    /// <summary>
    /// Curse - Reduces attack damage and movement speed (Warcraft 3)
    /// </summary>
    public class CurseBuff : BaseBuff
    {
        private readonly float damageReduction;
        private readonly float speedReduction;
        private readonly string damageKey;
        private readonly string speedKey;

        public CurseBuff(float damageReduce, float speedReduce, float duration = 120f) 
            : base("curse", duration)
        {
            damageReduction = -Mathf.Abs(damageReduce); // Ensure negative
            speedReduction = -Mathf.Abs(speedReduce);   // Ensure negative
            damageKey = $"curse_attack_damage_{GetHashCode()}";
            speedKey = $"curse_movement_speed_{GetHashCode()}";
        }

        public override void OnApply()
        {
            var attackAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE);
            var speedAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            
            attackAttribute.AddBonus(damageKey, damageReduction);
            speedAttribute.AddBonus(speedKey, speedReduction);
            
            Debug.Log($"Curse applied to {target.ID}: {damageReduction} damage, {speedReduction} speed");
        }

        public override void OnExpire()
        {
            var attackAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE);
            var speedAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            
            attackAttribute.RemoveBonus(damageKey);
            speedAttribute.RemoveBonus(speedKey);
            
            Debug.Log($"Curse expired on {target.ID}");
        }
    }

    /// <summary>
    /// Bloodlust - Increases attack speed and movement speed (Warcraft 3)
    /// </summary>
    public class BloodlustBuff : BaseBuff
    {
        private readonly float attackSpeedBonus;
        private readonly float movementSpeedBonus;
        private readonly string attackSpeedKey;
        private readonly string movementSpeedKey;

        public BloodlustBuff(float attackSpeedIncrease, float movementSpeedIncrease, float duration = 60f) 
            : base("bloodlust", duration)
        {
            attackSpeedBonus = attackSpeedIncrease;
            movementSpeedBonus = movementSpeedIncrease;
            attackSpeedKey = $"bloodlust_attack_speed_{GetHashCode()}";
            movementSpeedKey = $"bloodlust_movement_speed_{GetHashCode()}";
        }

        public override void OnApply()
        {
            var attackSpeedAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED);
            var movementSpeedAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            
            attackSpeedAttribute.AddBonus(attackSpeedKey, attackSpeedBonus);
            movementSpeedAttribute.AddBonus(movementSpeedKey, movementSpeedBonus);
            
            Debug.Log($"Bloodlust applied to {target.ID}: +{attackSpeedBonus} attack speed, +{movementSpeedBonus} movement speed");
        }

        public override void OnExpire()
        {
            var attackSpeedAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED);
            var movementSpeedAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            
            attackSpeedAttribute.RemoveBonus(attackSpeedKey);
            movementSpeedAttribute.RemoveBonus(movementSpeedKey);
            
            Debug.Log($"Bloodlust expired on {target.ID}");
        }
    }

    /// <summary>
    /// Inner Fire - Increases attack damage and armor (Warcraft 3)
    /// </summary>
    public class InnerFireBuff : BaseBuff
    {
        private readonly float damageBonus;
        private readonly float armorBonus;
        private readonly string damageKey;
        private readonly string armorKey;

        public InnerFireBuff(float damageIncrease, float armorIncrease, float duration = 60f) 
            : base("inner_fire", duration)
        {
            damageBonus = damageIncrease;
            armorBonus = armorIncrease;
            damageKey = $"inner_fire_attack_damage_{GetHashCode()}";
            armorKey = $"inner_fire_defense_{GetHashCode()}";
        }

        public override void OnApply()
        {
            var damageAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE);
            var armorAttribute = target.Attribute.GetBaseFloat(AttributeName.DEFENSE);
            
            damageAttribute.AddBonus(damageKey, damageBonus);
            armorAttribute.AddBonus(armorKey, armorBonus);
            
            Debug.Log($"Inner Fire applied to {target.ID}: +{damageBonus} damage, +{armorBonus} armor");
        }

        public override void OnExpire()
        {
            var damageAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_DAMAGE);
            var armorAttribute = target.Attribute.GetBaseFloat(AttributeName.DEFENSE);
            
            damageAttribute.RemoveBonus(damageKey);
            armorAttribute.RemoveBonus(armorKey);
            
            Debug.Log($"Inner Fire expired on {target.ID}");
        }
    }

    /// <summary>
    /// Regeneration - Restores health over time (Warcraft 3)
    /// </summary>
    public class RegenerationBuff : BaseBuff
    {
        private readonly float healPerSecond;
        private float healTimer;

        public RegenerationBuff(float healingPerSecond, float duration = 45f) 
            : base("regeneration", duration)
        {
            healPerSecond = healingPerSecond;
            healTimer = 0f;
        }

        public override void OnApply()
        {
            Debug.Log($"Regeneration applied to {target.ID}: {healPerSecond} HP/sec for {duration}s");
        }

        public override void OnExpire()
        {
            Debug.Log($"Regeneration expired on {target.ID}");
        }

        protected override void PerformTickEffect(float deltaTime)
        {
            healTimer += deltaTime;
            
            // Heal every second
            if (healTimer >= 1f)
            {
                float currentHp = target.Attribute.GetFloat(AttributeName.CUR_HP);
                float maxHp = target.Attribute.GetBaseFloat(AttributeName.MAX_HP).Total;
                float newHp = Mathf.Min(currentHp + healPerSecond, maxHp);
                
                target.Attribute.SetFloat(AttributeName.CUR_HP, newHp);
                healTimer = 0f;
                
                Debug.Log($"Regeneration healed {target.ID} for {healPerSecond} HP");
            }
        }
    }

    /// <summary>
    /// Poison - Damages health over time (Warcraft 3)
    /// </summary>
    public class PoisonBuff : BaseBuff
    {
        private readonly float damagePerSecond;
        private float damageTimer;

        public PoisonBuff(float damage, float duration = 30f) 
            : base("poison", duration)
        {
            damagePerSecond = damage;
            damageTimer = 0f;
        }

        public override void OnApply()
        {
            Debug.Log($"Poison applied to {target.ID}: {damagePerSecond} damage/sec for {duration}s");
        }

        public override void OnExpire()
        {
            Debug.Log($"Poison expired on {target.ID}");
        }

        protected override void PerformTickEffect(float deltaTime)
        {
            damageTimer += deltaTime;
            
            // Damage every second
            if (damageTimer >= 1f)
            {
                float currentHp = target.Attribute.GetFloat(AttributeName.CUR_HP);
                float newHp = Mathf.Max(currentHp - damagePerSecond, 0f);
                
                target.Attribute.SetFloat(AttributeName.CUR_HP, newHp);
                damageTimer = 0f;
                
                Debug.Log($"Poison damaged {target.ID} for {damagePerSecond} HP");
            }
        }
    }

    /// <summary>
    /// Slow - Reduces movement speed and attack speed (Warcraft 3)
    /// </summary>
    public class SlowBuff : BaseBuff
    {
        private readonly float movementSpeedReduction;
        private readonly float attackSpeedReduction;
        private readonly string movementKey;
        private readonly string attackKey;

        public SlowBuff(float movementReduction, float attackReduction, float duration = 20f) 
            : base("slow", duration)
        {
            movementSpeedReduction = -Mathf.Abs(movementReduction);
            attackSpeedReduction = -Mathf.Abs(attackReduction);
            movementKey = $"slow_movement_speed_{GetHashCode()}";
            attackKey = $"slow_attack_speed_{GetHashCode()}";
        }

        public override void OnApply()
        {
            var movementAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            var attackAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED);
            
            movementAttribute.AddBonus(movementKey, movementSpeedReduction);
            attackAttribute.AddBonus(attackKey, attackSpeedReduction);
            
            Debug.Log($"Slow applied to {target.ID}: {movementSpeedReduction} movement, {attackSpeedReduction} attack speed");
        }

        public override void OnExpire()
        {
            var movementAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            var attackAttribute = target.Attribute.GetBaseFloat(AttributeName.ATTACK_SPEED);
            
            movementAttribute.RemoveBonus(movementKey);
            attackAttribute.RemoveBonus(attackKey);
            
            Debug.Log($"Slow expired on {target.ID}");
        }
    }

    /// <summary>
    /// Thorns Aura - Reflects damage back to attackers (Warcraft 3)
    /// </summary>
    public class ThornsAuraBuff : BaseBuff
    {
        private readonly float reflectDamage;

        public ThornsAuraBuff(float reflectAmount, float duration = -1f) 
            : base("thorns_aura", duration)
        {
            reflectDamage = reflectAmount;
        }

        public override void OnApply()
        {
            Debug.Log($"Thorns Aura applied to {target.ID}: reflects {reflectDamage} damage");
            // Note: Actual damage reflection would need to be implemented in the damage system
        }

        public override void OnExpire()
        {
            Debug.Log($"Thorns Aura expired on {target.ID}");
        }
    }

    /// <summary>
    /// Invisibility - Makes unit invisible and increases movement speed (Warcraft 3)
    /// </summary>
    public class InvisibilityBuff : BaseBuff
    {
        private readonly float speedBonus;
        private readonly string speedKey;

        public InvisibilityBuff(float movementSpeedBonus, float duration = 30f) 
            : base("invisibility", duration)
        {
            speedBonus = movementSpeedBonus;
            speedKey = $"invisibility_movement_speed_{GetHashCode()}";
        }

        public override void OnApply()
        {
            var speedAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            speedAttribute.AddBonus(speedKey, speedBonus);
            
            Debug.Log($"Invisibility applied to {target.ID}: +{speedBonus} movement speed");
            // Note: Visual invisibility would need to be implemented in the rendering system
        }

        public override void OnExpire()
        {
            var speedAttribute = target.Attribute.GetBaseFloat(AttributeName.MOVEMENT_SPEED);
            speedAttribute.RemoveBonus(speedKey);
            
            Debug.Log($"Invisibility expired on {target.ID}");
        }
    }
}