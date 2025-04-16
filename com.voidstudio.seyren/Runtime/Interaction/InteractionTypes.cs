using System;
using UnityEngine;

namespace Seyren.Interaction
{
    /// <summary>
    /// Defines types of objects that can initiate interactions
    /// </summary>
    [CreateAssetMenu(fileName = "New Interactor Type", menuName = "Seyren/Interaction/Interactor Type")]
    public class InteractorType : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
    }

    /// <summary>
    /// Defines types of objects that can be interacted with
    /// </summary>
    [CreateAssetMenu(fileName = "New Interactable Type", menuName = "Seyren/Interaction/Interactable Type")]
    public class InteractableType : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
    }

    /// <summary>
    /// Defines elemental properties that can affect interactions
    /// </summary>
    [CreateAssetMenu(fileName = "New Element Type", menuName = "Seyren/Interaction/Element Type")]
    public class ElementType : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private Color elementColor = Color.white;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
        public Color ElementColor => elementColor;
    }

    /// <summary>
    /// Defines a possible state for an interactable object
    /// </summary>
    [CreateAssetMenu(fileName = "New Interactable State", menuName = "Seyren/Interaction/Interactable State")]
    public class InteractableState : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        public string DisplayName => displayName;
        public string Description => description;
        public Sprite Icon => icon;
    }
}
