namespace Seyren.System.Abilities
{
    /// <summary>
    /// Transforms an AbilityGraph definition before a runtime instance is created.
    /// Modifiers are applied in order in GraphAbility.Cast() and may restructure,
    /// augment, or replace nodes within the graph.
    /// </summary>
    public interface IAbilityGraphModifier
    {
        /// <summary>
        /// Returns a (possibly new) graph that incorporates this modifier's transformation.
        /// Implementations must not mutate <paramref name="graph"/> in place;
        /// return a new <see cref="AbilityGraph"/> if the structure changes.
        /// </summary>
        AbilityGraph Apply(AbilityGraph graph, AbilityContext context);
    }
}
