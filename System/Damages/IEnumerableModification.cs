using System.Collections;

namespace Seyren.System.Damages
{
    public interface IEnumerableModification<T>
    {
        int Count { get; }
        bool AddModification(T modification);
        bool RemoveModification(T modification);
    }
}
