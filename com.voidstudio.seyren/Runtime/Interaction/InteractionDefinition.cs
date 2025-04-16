using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// Defines a possible interaction between an interactor and an interactable
    /// </summary>
    [CreateAssetMenu(fileName = "New Interaction", menuName = "Seyren/Interaction/Interaction Definition")]
    public class InteractionDefinition : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        
        [SerializeField] private InteractorType interactorType;
        [SerializeField] private InteractableType interactableType;
        
        [SerializeField] private List<InteractionRequirement> requirements = new List<InteractionRequirement>();
        [SerializeField] private List<InteractionEffect> effects = new List<InteractionEffect>();
        
        [SerializeField] private AudioClip interactionSound;
        [SerializeField] private GameObject interactionVFX;
        [SerializeField] private float interactionCooldown = 0.5f;

        public string DisplayName => displayName;
        public string Description => description;
        public InteractorType InteractorType => interactorType;
        public InteractableType InteractableType => interactableType;

        /// <summary>
        /// Check if the interaction can be performed
        /// </summary>
        public bool CanInteract(IInteractor interactor, IInteractable interactable)
        {
            // Check if interactor and interactable types match
            if (interactorType != interactor.GetInteractorType() || 
                interactableType != interactable.GetInteractableType())
                return false;
            
            // Check all requirements
            foreach (var requirement in requirements)
            {
                if (!requirement.IsSatisfied(interactor, interactable))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Perform the interaction
        /// </summary>
        public void Interact(IInteractor interactor, IInteractable interactable)
        {
            // Play effects if assigned
            if (interactionSound != null)
            {
                AudioSource.PlayClipAtPoint(interactionSound, Vector3.zero);
            }
            
            if (interactionVFX != null)
            {
                // Instantiate VFX at the interaction point
                // We'd need position info for this in a real implementation
                GameObject.Instantiate(interactionVFX);
            }
            
            // Apply all effects
            foreach (var effect in effects)
            {
                effect.ApplyEffect(interactor, interactable);
            }
        }
        
        /// <summary>
        /// Get a description of what this interaction does
        /// </summary>
        public string GetDescription()
        {
            return description;
        }
        
        /// <summary>
        /// Get a preview of effects without applying them
        /// </summary>
        public List<string> GetPreviewEffects(IInteractor interactor, IInteractable interactable)
        {
            List<string> previewEffects = new List<string>();
            
            foreach (var effect in effects)
            {
                var previewText = effect.GetPreviewText(interactor, interactable);
                if (!string.IsNullOrEmpty(previewText))
                {
                    previewEffects.Add(previewText);
                }
            }
            
            return previewEffects;
        }
    }
}
