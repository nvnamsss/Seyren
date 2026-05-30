using System.Collections.Generic;
using System.Linq;
using Seyren.System.Units;
using Seyren.Universe;

namespace Seyren.System.Buffs
{
    public interface IBuffSystem
    {
        void ApplyBuffToUnit(IUnit target, IBuff buff);  // Add a buff to a unit
        void RemoveBuffFromUnit(IUnit target, string buffId); // Remove by ID
        bool IsUnitHasBuff(IUnit target, string buffId);   // Check if buff is active
        List<IBuff> GetBuffsFromUnit(IUnit target);          // Retrieve active buffs
        void Update(float deltaTime);               // Update all buffs each frame/tick
    }

    /// <summary>
    /// Concrete implementation of IBuffSystem that manages buffs for units.
    /// Uses a dictionary to store buffs per unit for efficient lookups and updates.
    /// </summary>
    public class BuffSystem : IBuffSystem
    {
        // Dictionary mapping unit ID to list of active buffs
        private readonly Dictionary<string, List<IBuff>> _unitBuffs = new Dictionary<string, List<IBuff>>();
        // Dictionary mapping unit ID to unit reference for proper cleanup
        private readonly Dictionary<string, IUnit> _unitReferences = new Dictionary<string, IUnit>();

        /// <summary>
        /// Applies a buff to the specified unit
        /// </summary>
        /// <param name="target">The unit to apply the buff to</param>
        /// <param name="buff">The buff to apply</param>
        public void ApplyBuffToUnit(IUnit target, IBuff buff)
        {
            if (target == null || buff == null)
                return;

            string unitId = target.ID;

            // Store unit reference for later use
            _unitReferences[unitId] = target;

            // Initialize buff list for this unit if it doesn't exist
            if (!_unitBuffs.ContainsKey(unitId))
            {
                _unitBuffs[unitId] = new List<IBuff>();
            }

            // Check if buff with same ID already exists
            var existingBuff = _unitBuffs[unitId].FirstOrDefault(b => b.GetId() == buff.GetId());
            if (existingBuff != null)
            {
                // Remove existing buff before applying new one
                existingBuff.OnExpire();
                _unitBuffs[unitId].Remove(existingBuff);
            }

            // Add new buff and apply its effects
            _unitBuffs[unitId].Add(buff);
            
            // Let the buff handle its own application logic
            buff.ApplyBuffToUnit(target);
        }

        /// <summary>
        /// Removes a buff from the specified unit by buff ID
        /// </summary>
        /// <param name="target">The unit to remove the buff from</param>
        /// <param name="buffId">The ID of the buff to remove</param>
        public void RemoveBuffFromUnit(IUnit target, string buffId)
        {
            if (target == null || string.IsNullOrEmpty(buffId))
                return;

            string unitId = target.ID;

            if (!_unitBuffs.ContainsKey(unitId))
                return;

            var buffToRemove = _unitBuffs[unitId].FirstOrDefault(b => b.GetId() == buffId);
            if (buffToRemove != null)
            {
                buffToRemove.OnExpire();
                _unitBuffs[unitId].Remove(buffToRemove);

                // Clean up empty lists
                if (_unitBuffs[unitId].Count == 0)
                {
                    _unitBuffs.Remove(unitId);
                    _unitReferences.Remove(unitId);
                }
            }
        }

        /// <summary>
        /// Checks if the specified unit has a buff with the given ID
        /// </summary>
        /// <param name="target">The unit to check</param>
        /// <param name="buffId">The ID of the buff to check for</param>
        /// <returns>True if the unit has the specified buff, false otherwise</returns>
        public bool IsUnitHasBuff(IUnit target, string buffId)
        {
            if (target == null || string.IsNullOrEmpty(buffId))
                return false;

            string unitId = target.ID;

            if (!_unitBuffs.ContainsKey(unitId))
                return false;

            return _unitBuffs[unitId].Any(b => b.GetId() == buffId);
        }

        /// <summary>
        /// Retrieves all active buffs for the specified unit
        /// </summary>
        /// <param name="target">The unit to get buffs for</param>
        /// <returns>List of active buffs, or empty list if none</returns>
        public List<IBuff> GetBuffsFromUnit(IUnit target)
        {
            if (target == null)
                return new List<IBuff>();

            string unitId = target.ID;

            if (!_unitBuffs.ContainsKey(unitId))
                return new List<IBuff>();

            // Return a copy to prevent external modification
            return new List<IBuff>(_unitBuffs[unitId]);
        }

        /// <summary>
        /// Updates all buffs for all units, handling expiration and periodic effects
        /// </summary>
        /// <param name="deltaTime">Time elapsed since last update</param>
        public void Update(float deltaTime)
        {
            var unitsToRemove = new List<string>();

            foreach (var kvp in _unitBuffs.ToList()) // ToList to avoid modification during iteration
            {
                string unitId = kvp.Key;
                var buffs = kvp.Value;
                var buffsToRemove = new List<IBuff>();
                
                // Get unit reference for proper buff handling
                var unit = _unitReferences.TryGetValue(unitId, out var unitRef) ? unitRef : null;

                // Update each buff and check for expiration
                foreach (var buff in buffs.ToList()) // ToList to avoid modification during iteration
                {
                    // Call the buff's tick method
                    buff.OnTick(deltaTime);

                    // Check if buff has expired
                    if (buff.IsExpired())
                    {
                        buffsToRemove.Add(buff);
                    }
                }

                // Remove expired buffs
                foreach (var expiredBuff in buffsToRemove)
                {
                    expiredBuff.OnExpire();
                    buffs.Remove(expiredBuff);
                }

                // Mark units with no buffs for cleanup
                if (buffs.Count == 0)
                {
                    unitsToRemove.Add(unitId);
                }
            }

            // Clean up units with no active buffs
            foreach (var unitId in unitsToRemove)
            {
                _unitBuffs.Remove(unitId);
                _unitReferences.Remove(unitId);
            }
        }

        /// <summary>
        /// Removes all buffs from all units (useful for cleanup)
        /// </summary>
        public void ClearAllBuffs()
        {
            foreach (var kvp in _unitBuffs)
            {
                foreach (var buff in kvp.Value)
                {
                    buff.OnExpire();
                }
            }
            _unitBuffs.Clear();
            _unitReferences.Clear();
        }

        /// <summary>
        /// Removes all buffs from a specific unit
        /// </summary>
        /// <param name="target">The unit to clear buffs from</param>
        public void ClearBuffs(IUnit target)
        {
            if (target == null)
                return;

            string unitId = target.ID;

            if (!_unitBuffs.ContainsKey(unitId))
                return;

            // Expire all buffs for this unit
            foreach (var buff in _unitBuffs[unitId])
            {
                buff.OnExpire();
            }

            _unitBuffs.Remove(unitId);
            _unitReferences.Remove(unitId);
        }

        /// <summary>
        /// Gets the total number of active buffs across all units
        /// </summary>
        /// <returns>Total number of active buffs</returns>
        public int GetTotalBuffCount()
        {
            return _unitBuffs.Values.Sum(buffList => buffList.Count);
        }

        /// <summary>
        /// Gets the number of units that currently have active buffs
        /// </summary>
        /// <returns>Number of units with buffs</returns>
        public int GetAffectedUnitCount()
        {
            return _unitBuffs.Count;
        }
    }
}