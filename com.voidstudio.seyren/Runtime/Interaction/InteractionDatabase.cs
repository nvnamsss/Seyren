using System.Collections.Generic;
using Seyren.System;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// Database of all possible interactions in the game
    /// </summary>
    [CreateAssetMenu(fileName = "InteractionDatabase", menuName = "Seyren/Interaction/Interaction Database")]
    public class InteractionDatabase : ScriptableObject
    {
        [SerializeField] private List<InteractionDefinition> interactions = new List<InteractionDefinition>();
        
        // Cache to speed up lookup by interactor and interactable types
        private SerializableDictionary<InteractorType, SerializableDictionary<InteractableType, List<InteractionDefinition>>> lookupTable 
            = new SerializableDictionary<InteractorType, SerializableDictionary<InteractableType, List<InteractionDefinition>>>();
        
        private bool isInitialized = false;
        
        /// <summary>
        /// Initialize the lookup table for faster interaction finding
        /// </summary>
        public void Initialize()
        {
            if (isInitialized) return;
            
            lookupTable.Clear();
            
            // Populate lookup table
            foreach (var interaction in interactions)
            {
                var interactorType = interaction.InteractorType;
                var interactableType = interaction.InteractableType;
                
                // Create entries if they don't exist
                if (!lookupTable.ContainsKey(interactorType))
                {
                    lookupTable[interactorType] = new SerializableDictionary<InteractableType, List<InteractionDefinition>>();
                }
                
                if (!lookupTable[interactorType].ContainsKey(interactableType))
                {
                    lookupTable[interactorType][interactableType] = new List<InteractionDefinition>();
                }
                
                // Add the interaction to the lookup table
                lookupTable[interactorType][interactableType].Add(interaction);
            }
            
            isInitialized = true;
        }
        
        /// <summary>
        /// Find all interactions that could apply to this interactor/interactable pair
        /// </summary>
        public List<InteractionDefinition> FindInteractions(InteractorType interactorType, InteractableType interactableType)
        {
            if (!isInitialized)
            {
                Initialize();
            }
            
            if (lookupTable.ContainsKey(interactorType) && 
                lookupTable[interactorType].ContainsKey(interactableType))
            {
                return lookupTable[interactorType][interactableType];
            }
            
            return new List<InteractionDefinition>();
        }
        
        /// <summary>
        /// Add a new interaction to the database
        /// </summary>
        public void AddInteraction(InteractionDefinition interaction)
        {
            if (!interactions.Contains(interaction))
            {
                interactions.Add(interaction);
                isInitialized = false; // Force reinitialization
            }
        }
        
        /// <summary>
        /// Remove an interaction from the database
        /// </summary>
        public void RemoveInteraction(InteractionDefinition interaction)
        {
            if (interactions.Contains(interaction))
            {
                interactions.Remove(interaction);
                isInitialized = false; // Force reinitialization
            }
        }
    }
}
