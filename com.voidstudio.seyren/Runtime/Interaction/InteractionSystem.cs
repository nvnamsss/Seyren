using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// The main system for handling interactions between game objects
    /// </summary>
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField] private InteractionDatabase interactionDatabase;
        
        private static InteractionSystem _instance;
        public static InteractionSystem Instance
        {
            get
            {                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<InteractionSystem>();
                    if (_instance == null)
                    {
                        var go = new GameObject("InteractionSystem");
                        _instance = go.AddComponent<InteractionSystem>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (interactionDatabase == null)
            {
                Debug.LogWarning("InteractionDatabase not assigned to InteractionSystem!");
            }
        }

        /// <summary>
        /// Attempts to perform an interaction between an interactor and an interactable
        /// </summary>
        /// <param name="interactor">The object initiating the interaction (e.g., player, tool)</param>
        /// <param name="interactable">The object being interacted with</param>
        /// <returns>True if the interaction was successful</returns>
        public bool TryInteract(IInteractor interactor, IInteractable interactable)
        {
            if (interactionDatabase == null)
            {
                Debug.LogError("Cannot interact: InteractionDatabase is not assigned!");
                return false;
            }

            // Find applicable interactions
            var applicableInteractions = interactionDatabase.FindInteractions(interactor.GetInteractorType(), interactable.GetInteractableType());
            
            foreach (var interaction in applicableInteractions)
            {
                // Check if the interaction can be performed
                if (interaction.CanInteract(interactor, interactable))
                {
                    // Perform the interaction
                    interaction.Interact(interactor, interactable);
                    
                    // Notify both parties that interaction occurred
                    interactor.OnInteractionPerformed(interactable, interaction);
                    interactable.OnInteractedWith(interactor, interaction);
                    
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Gets a preview of the interaction result without performing it
        /// </summary>
        /// <param name="interactor">The object initiating the interaction</param>
        /// <param name="interactable">The object being interacted with</param>
        /// <returns>Information about the potential interaction</returns>
        public InteractionPreview GetInteractionPreview(IInteractor interactor, IInteractable interactable)
        {
            if (interactionDatabase == null)
            {
                return new InteractionPreview(false, "No interaction database assigned");
            }

            var applicableInteractions = interactionDatabase.FindInteractions(interactor.GetInteractorType(), interactable.GetInteractableType());
            
            foreach (var interaction in applicableInteractions)
            {
                if (interaction.CanInteract(interactor, interactable))
                {
                    return new InteractionPreview(
                        true,
                        interaction.GetDescription(),
                        interaction.GetPreviewEffects(interactor, interactable)
                    );
                }
            }
            
            return new InteractionPreview(false, "No valid interaction found");
        }
    }
}
