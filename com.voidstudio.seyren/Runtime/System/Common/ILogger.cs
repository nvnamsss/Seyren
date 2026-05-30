namespace Seyren.System.Common
{
    public interface ISeyrenLogger
    {
        void Log(string message);
        void Warn(string message);
        void Error(string message);
    }
}
