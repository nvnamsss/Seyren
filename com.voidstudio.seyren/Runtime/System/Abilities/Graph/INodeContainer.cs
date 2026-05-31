namespace Seyren.System.Abilities
{
    /// <summary>
    /// Implemented by composite nodes that own child nodes.
    /// Used by modifiers to traverse and structurally transform the graph tree
    /// without mutating live node instances.
    /// </summary>
    public interface INodeContainer
    {
        /// <summary>The direct children of this node.</summary>
        IAbilityNode[] GetChildren();

        /// <summary>
        /// Returns a new node of the same type and policy with <paramref name="oldChild"/>
        /// replaced by <paramref name="newChild"/>. The original node is not modified.
        /// If <paramref name="oldChild"/> is not a direct child, returns this node unchanged.
        /// </summary>
        IAbilityNode WithChild(IAbilityNode oldChild, IAbilityNode newChild);
    }
}
