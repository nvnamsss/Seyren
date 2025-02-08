namespace Seyren.System.States
{
    public class StateChangeEventArgs
    {
        public StateValue State { get; }
        public float OldValue { get; }
        public float NewValue { get; }

        public StateChangeEventArgs(StateValue state, float oldValue, float newValue)
        {
            State = state;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}