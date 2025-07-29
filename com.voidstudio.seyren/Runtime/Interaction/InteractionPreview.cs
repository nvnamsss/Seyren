using System.Collections.Generic;

namespace Seyren.Interaction
{
    /// <summary>
    /// Contains preview information about a potential interaction
    /// </summary>
    public class InteractionPreview
    {
        /// <summary>
        /// Whether the interaction can be performed
        /// </summary>
        public bool CanInteract { get; private set; }
        
        /// <summary>
        /// Description of the interaction
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// List of effect descriptions that will occur if the interaction is performed
        /// </summary>
        public List<string> Effects { get; private set; }

        /// <summary>
        /// Creates a new interaction preview
        /// </summary>
        /// <param name="canInteract">Whether the interaction can be performed</param>
        /// <param name="description">Description of the interaction</param>
        /// <param name="effects">List of effect descriptions</param>
        public InteractionPreview(bool canInteract, string description, List<string> effects = null)
        {
            CanInteract = canInteract;
            Description = description;
            Effects = effects ?? new List<string>();
        }

        /// <summary>
        /// Get a formatted string representation of this preview
        /// </summary>
        public override string ToString()
        {
            if (!CanInteract)
            {
                return $"Cannot interact: {Description}";
            }

            var result = $"Interaction: {Description}";
            
            if (Effects.Count > 0)
            {
                result += "\nEffects:";
                foreach (var effect in Effects)
                {
                    result += $"\n- {effect}";
                }
            }
            
            return result;
        }
    }
}
