using System;
using UnityEngine;
using Seyren.System.Units;
using Seyren.Universe;

namespace Seyren.System.Damages
{
    /// <summary>
    /// Represents a damage over time effect that can be applied to a unit
    /// </summary>
    public class DamageOverTime
    {
        /// <summary>
        /// Unique identifier for this DOT effect
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The source unit that applied this DOT effect
        /// </summary>
        public IUnit Source { get; }

        /// <summary>
        /// The target unit receiving the damage
        /// </summary>
        public IUnit Target { get; }

        /// <summary>
        /// Base damage amount per tick
        /// </summary>
        public float DamagePerTick { get; set; }

        /// <summary>
        /// Type of damage inflicted
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        /// Total duration of the DOT effect in seconds
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// Time interval between damage ticks in seconds
        /// </summary>
        public float TickInterval { get; set; }

        /// <summary>
        /// Whether the damage ticks can be stacked from multiple sources
        /// </summary>
        public bool IsStackable { get; set; }

        /// <summary>
        /// Current stack count if stackable
        /// </summary>
        public int StackCount { get; private set; } = 1;

        /// <summary>
        /// Maximum number of stacks allowed
        /// </summary>
        public int MaxStacks { get; set; } = 1;

        /// <summary>
        /// Name of the DOT effect for identification
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Optional visual effect identifier
        /// </summary>
        public string VisualEffectId { get; set; }

        // Internal tracking
        private float _elapsedTime = 0f;
        private float _timeSinceLastTick = 0f;
        private bool _isActive = true;

        /// <summary>
        /// Create a new damage over time effect
        /// </summary>
        public DamageOverTime(string id, IUnit source, IUnit target, float damagePerTick, 
            DamageType damageType, float duration, float tickInterval)
        {
            Id = id;
            Source = source;
            Target = target;
            DamagePerTick = damagePerTick;
            DamageType = damageType;
            Duration = duration;
            TickInterval = tickInterval;
            IsStackable = false;
        }

        /// <summary>
        /// Updates the DOT effect, applies damage when necessary
        /// </summary>
        /// <param name="deltaTime">Time elapsed since last update</param>
        /// <returns>True if the effect is still active, false if expired</returns>
        public bool Update(float deltaTime)
        {
            if (!_isActive) return false;

            _elapsedTime += deltaTime;
            _timeSinceLastTick += deltaTime;

            // Check if it's time to apply damage
            if (_timeSinceLastTick >= TickInterval)
            {
                ApplyDamageTick();
                _timeSinceLastTick = 0f;
            }

            // Check if the effect has expired
            if (_elapsedTime >= Duration)
            {
                _isActive = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Apply a single tick of damage to the target
        /// </summary>
        private void ApplyDamageTick()
        {
            if (Target == null || Source == null) 
            {
                _isActive = false;
                return;
            }

            float damage = DamagePerTick * StackCount;
            DamageEngine.DamageTarget(Source, Target, damage, DamageType, TriggerType.All);
        }

        /// <summary>
        /// Attempt to stack this DOT effect
        /// </summary>
        /// <returns>True if stacking was successful</returns>
        public bool AddStack()
        {
            if (!IsStackable || StackCount >= MaxStacks) return false;
            
            StackCount++;
            return true;
        }

        /// <summary>
        /// Refresh the duration of this DOT effect
        /// </summary>
        public void Refresh()
        {
            _elapsedTime = 0f;
        }

        /// <summary>
        /// Immediately remove this DOT effect
        /// </summary>
        public void Remove()
        {
            _isActive = false;
        }

        /// <summary>
        /// Check if this DOT effect is still active
        /// </summary>
        public bool IsActive => _isActive;

        /// <summary>
        /// Get remaining duration of this DOT effect
        /// </summary>
        public float RemainingDuration => Math.Max(0, Duration - _elapsedTime);

        /// <summary>
        /// Get the percentage of duration remaining
        /// </summary>
        public float RemainingPercentage => Duration > 0 ? RemainingDuration / Duration : 0;
    }
}