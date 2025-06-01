using Seyren.System.Common;

namespace Seyren.Universe {
    public interface ILoop
    {
        public void Loop(ITime time);
        // public void Loop(ITime time, ISpace space);
    }
    
    public interface ITime
    {
        public event GameEventHandler<ITime> OnTick;
        float DeltaTime { get; }
        float CurrentTime { get; }
    }
}