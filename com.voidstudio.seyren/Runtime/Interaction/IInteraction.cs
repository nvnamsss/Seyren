using System.Collections.Generic;

namespace Seyren.Interaction
{
    /// <summary>
    /// Interface for objects that can initiate interactions
    /// </summary>
    public interface IInteractor
    {
        /// <summary>
        /// Get the type of the interactor
        /// </summary>
        InteractorType GetInteractorType();
        
        /// <summary>
        /// Get any elements or properties this interactor has
        /// </summary>
        IReadOnlyList<ElementType> GetElements();

        /// <summary>
        /// Called when this interactor successfully performs an interaction
        /// </summary>
        void OnInteractionPerformed(IInteractable target, InteractionDefinition interaction);
    }
    
    /// <summary>
    /// Interface for objects that can be interacted with
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Get the type of the interactable
        /// </summary>
        InteractableType GetInteractableType();
        
        /// <summary>
        /// Get the current state of the interactable
        /// </summary>
        IReadOnlyList<InteractableState> GetStates();

        /// <summary>
        /// Called when an interactor successfully interacts with this object
        /// </summary>
        void OnInteractedWith(IInteractor source, InteractionDefinition interaction);
    }
}
