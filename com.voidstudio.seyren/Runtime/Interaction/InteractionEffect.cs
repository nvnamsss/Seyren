using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// Base class for effects that occur as a result of interactions
    /// </summary>
    public abstract class InteractionEffect : ScriptableObject
    {
        [SerializeField] protected string effectName;
        [SerializeField] protected string effectDescription;
        
        /// <summary>
        /// Apply this effect when an interaction occurs
        /// </summary>
        public abstract void ApplyEffect(IInteractor interactor, IInteractable interactable);
        
        /// <summary>
        /// Get descriptive text of what this effect will do
        /// </summary>
        public virtual string GetPreviewText(IInteractor interactor, IInteractable interactable)
        {
            return effectDescription;
        }
    }
    
    /// <summary>
    /// Effect that changes the state of an interactable
    /// </summary>
    [CreateAssetMenu(fileName = "ChangeStateEffect", menuName = "Seyren/Interaction/Effects/Change State Effect")]
    public class ChangeStateEffect : InteractionEffect
    {
        [SerializeField] private List<InteractableState> statesToAdd;
        [SerializeField] private List<InteractableState> statesToRemove;
        
        public override void ApplyEffect(IInteractor interactor, IInteractable interactable)
        {
            if (interactable is InteractableObject targetObject)
            {
                // Add states
                foreach (var state in statesToAdd)
                {
                    targetObject.AddState(state);
                }
                
                // Remove states
                foreach (var state in statesToRemove)
                {
                    targetObject.RemoveState(state);
                }
            }
        }
        
        public override string GetPreviewText(IInteractor interactor, IInteractable interactable)
        {
            string preview = effectDescription;
            
            if (statesToAdd.Count > 0 || statesToRemove.Count > 0)
            {
                preview = "Will ";
                
                if (statesToAdd.Count > 0)
                {
                    preview += "add " + string.Join(", ", statesToAdd.ConvertAll(s => s.DisplayName));
                    
                    if (statesToRemove.Count > 0)
                        preview += " and ";
                }
                
                if (statesToRemove.Count > 0)
                {
                    preview += "remove " + string.Join(", ", statesToRemove.ConvertAll(s => s.DisplayName));
                }
                
                return preview;
            }
            
            return base.GetPreviewText(interactor, interactable);
        }
    }
    
    /// <summary>
    /// Effect that spawns a new object as a result of interaction
    /// </summary>
    [CreateAssetMenu(fileName = "SpawnObjectEffect", menuName = "Seyren/Interaction/Effects/Spawn Object Effect")]
    public class SpawnObjectEffect : InteractionEffect
    {
        [SerializeField] private GameObject objectToSpawn;
        [SerializeField] private bool replaceInteractable = false;
        [SerializeField] private bool inheritElements = true;
        
        public override void ApplyEffect(IInteractor interactor, IInteractable interactable)
        {
            // Get position from interactable if it's a MonoBehaviour
            Vector3 spawnPosition = Vector3.zero;
            
            if (interactable is MonoBehaviour interactableBehaviour)
            {
                spawnPosition = interactableBehaviour.transform.position;
                
                // If replacing, destroy original
                if (replaceInteractable)
                {
                    GameObject.Destroy(interactableBehaviour.gameObject);
                }
            }
            
            // Spawn the new object
            GameObject newObject = GameObject.Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            
            // Transfer elements if needed
            if (inheritElements && newObject.TryGetComponent<InteractorObject>(out var newInteractor))
            {
                foreach (var element in interactor.GetElements())
                {
                    newInteractor.AddElement(element);
                }
            }
        }
        
        public override string GetPreviewText(IInteractor interactor, IInteractable interactable)
        {
            if (objectToSpawn != null)
            {
                return $"Creates {objectToSpawn.name}" + (replaceInteractable ? " (replaces target)" : "");
            }
            
            return base.GetPreviewText(interactor, interactable);
        }
    }
    
    /// <summary>
    /// Effect that transfers elements from interactor to interactable
    /// </summary>
    [CreateAssetMenu(fileName = "TransferElementEffect", menuName = "Seyren/Interaction/Effects/Transfer Element Effect")]
    public class TransferElementEffect : InteractionEffect
    {
        [SerializeField] private List<ElementType> elementsToTransfer;
        [SerializeField] private bool transferAllElements = false;
        [SerializeField] private bool consumeElementsFromSource = false;
        
        public override void ApplyEffect(IInteractor interactor, IInteractable interactable)
        {
            if (interactable is InteractableObject targetObject)
            {
                if (transferAllElements)
                {
                    foreach (var element in interactor.GetElements())
                    {
                        targetObject.AddElement(element);
                    }
                }
                else
                {
                    foreach (var element in elementsToTransfer)
                    {
                        targetObject.AddElement(element);
                    }
                }
                
                // Remove elements from source if consuming
                if (consumeElementsFromSource && interactor is InteractorObject sourceObject)
                {
                    if (transferAllElements)
                    {
                        foreach (var element in interactor.GetElements())
                        {
                            sourceObject.RemoveElement(element);
                        }
                    }
                    else
                    {
                        foreach (var element in elementsToTransfer)
                        {
                            sourceObject.RemoveElement(element);
                        }
                    }
                }
            }
        }
        
        public override string GetPreviewText(IInteractor interactor, IInteractable interactable)
        {
            if (transferAllElements)
            {
                return "Transfers all elements" + (consumeElementsFromSource ? " (consumed)" : "");
            }
            else if (elementsToTransfer.Count > 0)
            {
                return $"Transfers {string.Join(", ", elementsToTransfer.ConvertAll(e => e.DisplayName))}" 
                    + (consumeElementsFromSource ? " (consumed)" : "");
            }
            
            return base.GetPreviewText(interactor, interactable);
        }
    }
}
