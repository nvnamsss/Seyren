namespace Seyren.Universe {
    public interface IConnector {
        void Notify(string eventName, string message);
        // void Notify(string eventName, string message);
        void Register(IConnector connector);
    }
}