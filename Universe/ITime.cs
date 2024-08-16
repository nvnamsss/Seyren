using Seyren.System.Generics;

namespace Seyren.Universe {
    public interface ITime {
        public event GameEventHandler<ITime> OnTick;
        float DeltaTime { get; }
        float CurrentTime { get; }
    }
}