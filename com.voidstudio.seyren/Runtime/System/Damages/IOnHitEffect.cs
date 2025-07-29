using Seyren.System.Units;

namespace Seyren.System.Damages
{
    public interface IOnHitEffect
    {
        void Trigger(IUnit target);
    }
}
