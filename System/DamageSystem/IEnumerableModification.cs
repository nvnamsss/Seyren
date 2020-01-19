using System.Collections;

namespace Base2D.System.DamageSystem
{
    public interface IEnumerableModification<T> where T : IDamageModification<T>
    {
        int Count { get; }
        bool AddModification(T modification);
        bool RemoveModification(T modification);
    }
}
