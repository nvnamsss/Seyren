using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem
{
    public abstract class DamageModifications<T> : IEnumerableModification<T>, IEnumerable where T : IDamageModification<T>
    {
        public int Count => _modification.Count;
        public T this[int id]
        {
            get
            {
                if (_modification.ContainsKey(id))
                {
                    return _modification[id];
                }

                return default(T);
            }
        }
        protected Dictionary<int, T> _modification;
        public virtual bool AddModification(T modification)
        {
            if (_modification.ContainsKey(modification.Id))
            {
                switch (modification.Stacks)
                {
                    default:
                        return false;
                }
            }
            else
            {
                _modification.Add(modification.Id, modification);
            }

            return true;
        }

        public virtual bool RemoveModification(T modification)
        {
            if (!_modification.ContainsKey(modification.Id))
            {
                return false;
            }

            _modification.Remove(modification.Id);
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            return _modification.GetEnumerator();
        }

    }
}
