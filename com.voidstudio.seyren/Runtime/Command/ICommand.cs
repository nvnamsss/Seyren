namespace Seyren.Command
{
    public interface ICommand
    {
        // Preview the command without executing it
        // This can be used to show a preview of the command's effect
        void Preview();
        // Execute the command
        void Execute();
        // Revoke the command, undoing its effects even if it has been executed
        void Undo();
    }
}