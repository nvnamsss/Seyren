namespace Seyren.System.Abilities
{
    /// <summary>
    /// Optional interface for <see cref="IAbilityGraphModifier"/> implementations that need
    /// to automatically register a companion <see cref="IGraphAbilityVFX"/> on the casting
    /// <see cref="GraphAbility"/>.
    ///
    /// <see cref="GraphAbility.Cast"/> checks each modifier for this interface after applying
    /// it and calls <see cref="GraphAbility.AddVFX"/> with the returned handler, so it is
    /// always appended after any ability-level VFX already in the chain.
    /// </summary>
    public interface IGraphAbilityVFXProvider
    {
        IGraphAbilityVFX GetVFX();
    }
}
