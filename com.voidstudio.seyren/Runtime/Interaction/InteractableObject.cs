using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// Component for objects that can be interacted with
    /// </summary>
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractableType interactableType;
        [SerializeField] private List<InteractableState> states = new List<InteractableState>();
        [SerializeField] private List<ElementType> elements = new List<ElementType>();
        
        [SerializeField] private bool highlightWhenInteractable = true;
        [SerializeField] private Material highlightMaterial;
        
        private Material originalMaterial;
        private Renderer objectRenderer;
        private bool isHighlighted = false;
        
        public InteractableType InteractableType => interactableType;
        
        private void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalMaterial = objectRenderer.material;
            }
        }

        public InteractableType GetInteractableType()
        {
            return interactableType;
        }

        public IReadOnlyList<InteractableState> GetStates()
        {
            return states.AsReadOnly();
        }
        
        public IReadOnlyList<ElementType> GetElements()
        {
            return elements.AsReadOnly();
        }

        public void OnInteractedWith(IInteractor source, InteractionDefinition interaction)
        {
            // Can be overridden in derived classes to respond to being interacted with
        }
        
        /// <summary>
        /// Add a state to this interactable
        /// </summary>
        public void AddState(InteractableState state)
        {
            if (!states.Contains(state))
            {
                states.Add(state);
                OnStateChanged();
            }
        }
        
        /// <summary>
        /// Remove a state from this interactable
        /// </summary>
        public bool RemoveState(InteractableState state)
        {
            bool removed = states.Remove(state);
            if (removed)
            {
                OnStateChanged();
            }
            return removed;
        }
        
        /// <summary>
        /// Add an element to this interactable
        /// </summary>
        public void AddElement(ElementType element)
        {
            if (!elements.Contains(element))
            {
                elements.Add(element);
                OnElementChanged();
            }
        }
        
        /// <summary>
        /// Remove an element from this interactable
        /// </summary>
        public bool RemoveElement(ElementType element)
        {
            bool removed = elements.Remove(element);
            if (removed)
            {
                OnElementChanged();
            }
            return removed;
        }
        
        /// <summary>
        /// Called when the states of this object change
        /// </summary>
        protected virtual void OnStateChanged()
        {
            // Override in derived classes to respond to state changes
        }
        
        /// <summary>
        /// Called when the elements of this object change
        /// </summary>
        protected virtual void OnElementChanged()
        {
            // Override in derived classes to respond to element changes
        }
        
        /// <summary>
        /// Highlight this object to show it's interactable
        /// </summary>
        public void Highlight(bool highlight)
        {
            if (!highlightWhenInteractable || objectRenderer == null || highlightMaterial == null)
                return;
            
            if (highlight && !isHighlighted)
            {
                objectRenderer.material = highlightMaterial;
                isHighlighted = true;
            }
            else if (!highlight && isHighlighted)
            {
                objectRenderer.material = originalMaterial;
                isHighlighted = false;
            }
        }
    }
}
