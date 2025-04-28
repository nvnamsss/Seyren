using Seyren.System.Generics;

namespace Seyren.Universe {
    public interface ILoop
    {
        public void Loop(ITime time);
    }
    
    public interface ITime
    {
        public event GameEventHandler<ITime> OnTick;
        float DeltaTime { get; }
        float CurrentTime { get; }
    }
}