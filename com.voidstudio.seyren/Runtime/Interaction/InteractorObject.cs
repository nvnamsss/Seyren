using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// Component for objects that can initiate interactions
    /// </summary>
    public class InteractorObject : MonoBehaviour, IInteractor
    {
        [SerializeField] private InteractorType interactorType;
        [SerializeField] private List<ElementType> elements = new List<ElementType>();
        
        [SerializeField] private float interactionRange = 2f;
        [SerializeField] private LayerMask interactableLayers = ~0; // All layers by default
        
        public InteractorType InteractorType => interactorType;
        public float InteractionRange => interactionRange;
        
        /// <summary>
        /// Add an element to this interactor
        /// </summary>
        public void AddElement(ElementType element)
        {
            if (!elements.Contains(element))
            {
                elements.Add(element);
            }
        }
        
        /// <summary>
        /// Remove an element from this interactor
        /// </summary>
        public bool RemoveElement(ElementType element)
        {
            return elements.Remove(element);
        }

        public InteractorType GetInteractorType()
        {
            return interactorType;
        }

        public IReadOnlyList<ElementType> GetElements()
        {
            return elements.AsReadOnly();
        }

        public void OnInteractionPerformed(IInteractable target, InteractionDefinition interaction)
        {
            // Can be overridden in derived classes to respond to successful interactions
        }
        
        /// <summary>
        /// Find the nearest interactable in range
        /// </summary>
        public IInteractable GetNearestInteractable()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactableLayers);
            
            float closestDistance = float.MaxValue;
            IInteractable closest = null;
            
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = interactable;
                    }
                }
            }
            
            return closest;
        }
        
        /// <summary>
        /// Try to interact with the nearest interactable
        /// </summary>
        public bool TryInteractWithNearest()
        {
            var nearest = GetNearestInteractable();
            if (nearest != null)
            {
                return InteractionSystem.Instance.TryInteract(this, nearest);
            }
            
            return false;
        }
        
        /// <summary>
        /// Try to interact with a specific interactable
        /// </summary>
        public bool TryInteractWith(IInteractable interactable)
        {
            if (interactable == null) return false;
            
            // Check if in range, if it's a GameObject
            if (interactable is MonoBehaviour interactableBehaviour)
            {
                float distance = Vector3.Distance(transform.position, interactableBehaviour.transform.position);
                if (distance > interactionRange)
                {
                    return false;
                }
            }
            
            return InteractionSystem.Instance.TryInteract(this, interactable);
        }
    }
}
