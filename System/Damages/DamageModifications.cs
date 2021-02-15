using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages
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
                switch (modification.StackType)
                {
                    case StackType.Unique:
                        UniqueStack(modification);
                        break;
                    case StackType.Diminishing:
                        DiminishingStack(modification);
                        break;
                    case StackType.ByHalf:
                        ByHalfStack(modification);
                        break;
                    case StackType.Directly:
                        DirectlyStack(modification);
                        break;
                }
                _modification[modification.Id].Stacks.Add(modification);
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

        protected void DiminishingStack(T modification)
        {
            _modification[modification.Id].Chance += (100 - _modification[modification.Id].Chance) * modification.Chance;
        }

        protected void DirectlyStack(T modification)
        {
            _modification[modification.Id].Chance += modification.Chance;
        }

        protected void ByHalfStack(T modification)
        {
            float plus = modification.Chance / (_modification[modification.Id].Stacks.Count + 1);
            _modification[modification.Id].Chance += plus;
        }

        protected void UniqueStack(T modification)
        {
            if (modification.Chance > _modification[modification.Id].Chance)
            {
                _modification[modification.Id].Chance = modification.Chance;
            }
        }
    }
}
