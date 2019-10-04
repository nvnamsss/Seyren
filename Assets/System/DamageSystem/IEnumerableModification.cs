using System.Collections;

namespace Crom.System.DamageSystem
{
    public interface IEnumerableModification
    {
        Hashtable Modifications { get; set; }

        bool AddModification(IDamageModification modification);
        IDamageModification GetModification(string id);
        bool SetModification(string id, IDamageModification modification);
        bool RemoveModification(IDamageModification modification);
    }
}
