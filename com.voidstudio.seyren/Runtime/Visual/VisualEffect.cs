using Seyren.System.Units;

namespace Seyren.Visual
{
    public interface IVisualEffect : ICoordinate
    {
        void Play();
        // Stop the effect, it will not be removed
        void Stop();
        void SetState(string state);
        // Remove the effect from the scene
        void Remove();

    }
    
}