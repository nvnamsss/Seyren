using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// Base class for requirements that must be met for an interaction to occur
    /// </summary>
    public abstract class InteractionRequirement : ScriptableObject
    {
        [SerializeField] protected string failureMessage;
        
        /// <summary>
        /// Check if this requirement is satisfied
        /// </summary>
        public abstract bool IsSatisfied(IInteractor interactor, IInteractable interactable);
        
        /// <summary>
        /// Get the message explaining why the requirement failed
        /// </summary>
        public string GetFailureMessage()
        {
            return failureMessage;
        }
    }
    
    /// <summary>
    /// Requires the interactor to have specific elements
    /// </summary>
    [CreateAssetMenu(fileName = "ElementRequirement", menuName = "Seyren/Interaction/Requirements/Element Requirement")]
    public class ElementRequirement : InteractionRequirement
    {
        [SerializeField] private List<ElementType> requiredElements;
        [SerializeField] private bool requireAll = false;
        
        public override bool IsSatisfied(IInteractor interactor, IInteractable interactable)
        {
            var interactorElements = interactor.GetElements();
            
            if (requireAll)
            {
                // All required elements must be present
                foreach (var required in requiredElements)
                {
                    bool found = false;
                    foreach (var element in interactorElements)
                    {
                        if (element == required)
                        {
                            found = true;
                            break;
                        }
                    }
                    
                    if (!found) return false;
                }
                
                return true;
            }
            else
            {
                // At least one required element must be present
                foreach (var required in requiredElements)
                {
                    foreach (var element in interactorElements)
                    {
                        if (element == required)
                        {
                            return true;
                        }
                    }
                }
                
                return requiredElements.Count == 0; // If no elements required, it's satisfied
            }
        }
    }
    
    /// <summary>
    /// Requires the interactable to be in specific states
    /// </summary>
    [CreateAssetMenu(fileName = "StateRequirement", menuName = "Seyren/Interaction/Requirements/State Requirement")]
    public class StateRequirement : InteractionRequirement
    {
        [SerializeField] private List<InteractableState> requiredStates;
        [SerializeField] private List<InteractableState> forbiddenStates;
        
        public override bool IsSatisfied(IInteractor interactor, IInteractable interactable)
        {
            var currentStates = interactable.GetStates();
            
            // Check for required states
            foreach (var required in requiredStates)
            {
                bool found = false;
                foreach (var state in currentStates)
                {
                    if (state == required)
                    {
                        found = true;
                        break;
                    }
                }
                
                if (!found) return false;
            }
            
            // Check for forbidden states
            foreach (var forbidden in forbiddenStates)
            {
                foreach (var state in currentStates)
                {
                    if (state == forbidden)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}
