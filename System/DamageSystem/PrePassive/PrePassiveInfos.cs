using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.PrePassive
{
    public class PrePassiveInfos : IEnumerableModification<PrePassiveInfo>, IEnumerable
    {
        public int Count => _modification.Count;
        private Dictionary<int, PrePassiveInfo> _modification;
        public PrePassiveInfos()
        {
            _modification = new Dictionary<int, PrePassiveInfo>();
        }


        public bool AddModification(PrePassiveInfo modification)
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

        public bool RemoveModification(PrePassiveInfo modification)

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

        public PrePassiveInfo this[int id]
        {
            get
            {
                if (_modification.ContainsKey(id))
                {
                    return _modification[id];
                }

                return null;
            }
        }
    }
}
