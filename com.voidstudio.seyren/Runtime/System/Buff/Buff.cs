using Seyren.System.Units;

namespace Seyren.System.Buffs
{
    public interface IBuff
    {
        string GetId();                 // Unique ID or name
        IUnit GetOwner();                 // The unit this buff is applied to
        IUnit GetTarget();                // The unit this buff affects
        float GetDuration();             // How long buff lasts
        float GetRemainingTime();        // Time left
        bool IsExpired();                // Whether buff expired
        void OnApply();                  // What happens when applied
        void OnExpire();                 // What happens when removed
        void OnTick(float deltaTime);    // Periodic update
        void ApplyBuffToUnit(IUnit target); // Apply this buff to a unit
    }

}