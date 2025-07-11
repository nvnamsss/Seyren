using System.Collections.Generic;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Represents a phase in a multi-phase ability
    /// </summary>
    public class AbilityPhase
    {
        /// <summary>
        /// Unique identifier for the phase
        /// </summary>
        public int phaseId;
        
        /// <summary>
        /// Duration of the phase in seconds. Set to -1 for indefinite duration.
        /// </summary>
        public float duration;
        
        /// <summary>
        /// Whether this phase can be interrupted/skipped
        /// </summary>
        public bool canBeInterrupted;
        
        /// <summary>
        /// Custom data specific to this phase
        /// </summary>
        public object phaseData;

        public AbilityPhase(int phaseId, float duration, bool canBeInterrupted = true, object phaseData = null)
        {
            this.phaseId = phaseId;
            this.duration = duration;
            this.canBeInterrupted = canBeInterrupted;
            this.phaseData = phaseData;
        }
    }

    /// <summary>
    /// Tracks the current state of a phase instance
    /// </summary>
    public class PhaseState
    {
        /// <summary>
        /// The phase definition
        /// </summary>
        public AbilityPhase phase;
        
        /// <summary>
        /// Time elapsed in this phase
        /// </summary>
        public float elapsedTime;
        
        /// <summary>
        /// Whether the phase has been initialized
        /// </summary>
        public bool initialized;
        
        /// <summary>
        /// Whether the phase has completed
        /// </summary>
        public bool completed;

        public PhaseState(AbilityPhase phase)
        {
            this.phase = phase;
            this.elapsedTime = 0f;
            this.initialized = false;
            this.completed = false;
        }
    }
    
    /// <summary>
    /// Base class for an ability instance with multiple phases
    /// </summary>
    public class PhaseAbilityInstance
    {
        /// <summary>
        /// The caster of the ability
        /// </summary>
        public IUnit caster;
        
        /// <summary>
        /// Target position if applicable
        /// </summary>
        public Vector3? location;
        
        /// <summary>
        /// Target unit if applicable
        /// </summary>
        public IUnit targetUnit;
        
        /// <summary>
        /// Level of the ability
        /// </summary>
        public int abilityLevel;
        
        /// <summary>
        /// Current phase state
        /// </summary>
        public PhaseState currentPhase;
        
        /// <summary>
        /// Whether this instance is active
        /// </summary>
        public bool active = true;
        
        /// <summary>
        /// Custom data for the instance
        /// </summary>
        public object instanceData;
    }

    /// <summary>
    /// Abstract base class for abilities with multiple phases
    /// </summary>
    public abstract class PhaseAbility<T> : Ability where T : PhaseAbilityInstance
    {
        /// <summary>
        /// List of active ability instances
        /// </summary>
        protected List<T> instances = new List<T>();
        
        /// <summary>
        /// Definition of phases for this ability
        /// </summary>
        protected List<AbilityPhase> phaseDefinitions = new List<AbilityPhase>();

        protected PhaseAbility(Universe.Universe universe) : base(universe)
        {
            DefinePhases();
        }

        /// <summary>
        /// Define the phases of this ability. Must be implemented by derived classes.
        /// </summary>
        protected abstract void DefinePhases();

        /// <summary>
        /// Update logic for a specific phase
        /// </summary>
        /// <param name="instance">The ability instance</param>
        /// <param name="phaseState">The current phase state</param>
        /// <param name="time">Game time information</param>
        protected abstract void UpdatePhase(T instance, PhaseState phaseState, ITime time);

        /// <summary>
        /// Called when a phase completes
        /// </summary>
        /// <param name="instance">The ability instance</param>
        /// <param name="phaseState">The completed phase state</param>
        protected abstract void CompletePhase(T instance, PhaseState phaseState);

        protected abstract void onAllPhasesComplete(T instance);
        /// <summary>
        /// Get the next phase after the current one
        /// </summary>
        /// <param name="instance">The ability instance</param>
        /// <param name="currentPhase">The current phase</param>
        /// <returns>The next phase or null if there are no more phases</returns>
        protected virtual AbilityPhase GetNextPhase(T instance, AbilityPhase currentPhase)
        {
            int idx = currentPhase.phaseId + 1;
            if (idx < phaseDefinitions.Count)
                return phaseDefinitions[idx];
            return null;
        }

        /// <summary>
        /// Advance the instance to the next phase
        /// </summary>
        /// <param name="instance">The ability instance</param>
        /// <returns>True if advanced to a new phase, false if there are no more phases</returns>
        protected virtual bool NextPhase(T instance)
        {
            // check if next phase is available
            if (instance.currentPhase.phase.phaseId == phaseDefinitions.Count - 1)
            {
                return false;
            }
            
            AbilityPhase nextPhase = GetNextPhase(instance, instance.currentPhase.phase);
            // Complete current phase
            CompletePhase(instance, instance.currentPhase);
            
            // Set up new phase
            instance.currentPhase = new PhaseState(nextPhase);
            
            return true;
        }

        protected override void TickEffect(ITime time)
        {
            // Process all active instances
            for (int i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];
                
                // Skip inactive instances
                if (!instance.active)
                {
                    instances.RemoveAt(i);
                    --i;
                    continue;
                }

                if (instance.currentPhase == null)
                {
                    // Initialize the first phase if not already done
                    instance.currentPhase = new PhaseState(phaseDefinitions[0]);
                }
                
                var phaseState = instance.currentPhase;
                
                // Update phase
                UpdatePhase(instance, phaseState, time);
                
                // Increment phase timer
                phaseState.elapsedTime += time.DeltaTime;
                
                // Check if phase is complete
                if (phaseState.phase.duration > 0 && phaseState.elapsedTime >= phaseState.phase.duration)
                {
                    // Try to advance to next phase
                    if (!NextPhase(instance))
                    {
                        // No more phases, deactivate instance
                        onAllPhasesComplete(instance);
                        instance.active = false;
                    }
                }
            }
        }
    }
}
