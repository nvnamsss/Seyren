namespace Seyren.System.Abilities
{
    /// <summary>
    /// Result returned by an IAbilityNode each tick.
    /// </summary>
    public enum NodeState
    {
        /// <summary>The node is still executing and must be ticked again next frame.</summary>
        Running,

        /// <summary>The node completed successfully.</summary>
        Success,

        /// <summary>The node encountered a failure condition.</summary>
        Failure,
    }
}
