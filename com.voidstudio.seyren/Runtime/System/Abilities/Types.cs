using System;

namespace Seyren.System.Abilities
{
    public enum UnitTargetingType
    {
        Self,
        Allied,
        Enemy,
    }

    public enum AffectType {
        Point,
        Area,
    }


    public enum TargetingType
    {
        NoTarget = 1,
        UnitTarget = 2,
        PointTarget = 4,
        UnitOrPointTarget = UnitTarget | PointTarget,
    }

    [Flags]
    public enum BreakType
    {
        CannotBreak = 1,
        CannotCancel = 2,
        CannotGetKnockDown = 4,
        CanKnockDownWithSoonRelease = 8,
        CanKnockDown = 16,
        CanCancelNoCoolDown = 32,
        CanCancelWithCoolDown = 64,
        All = ~0,
    }

    public enum CastType
    {
        /// <summary>
        /// Ability that cast for a while and the effect is occur while channeling
        /// </summary>
        Channel,
        /// <summary>
        /// Ability with casting time, the effect will be occured after cast success
        /// </summary>
        Active,
        /// <summary>
        /// Ability with out casting time (instant cast)
        /// </summary>
        Instant,
        /// <summary>
        /// Ability affect an area with out cast or mana cost
        /// </summary>
        Aura,
        Autocast,
        /// <summary>
        /// Ability with 2 state On or Off
        /// </summary>
        Toggle,
    }
}
