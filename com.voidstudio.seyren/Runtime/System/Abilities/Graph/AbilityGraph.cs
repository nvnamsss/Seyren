namespace Seyren.System.Abilities
{
    /// <summary>
    /// Immutable definition of an ability execution graph.
    /// Shared across all casts of the same ability; never mutated at runtime.
    /// </summary>
    public class AbilityGraph
    {
        /// <summary>The root node that is ticked when the graph instance runs.</summary>
        public IAbilityNode EntryNode { get; }

        public AbilityGraph(IAbilityNode entryNode)
        {
            EntryNode = entryNode;
        }
    }
}
