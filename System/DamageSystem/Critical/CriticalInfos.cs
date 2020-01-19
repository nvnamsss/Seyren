using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Base2D.System.DamageSystem.Critical
{
    [Serializable]
    public class CriticalInfos : IEnumerableModification<CriticalInfo>, IEnumerable
    {
        public int Count => _modification.Count;
        private Dictionary<int, CriticalInfo> _modification;
        public CriticalInfos()
        {
            _modification = new Dictionary<int, CriticalInfo>();
        }

        public bool AddModification(CriticalInfo critical)
        {

            if (_modification.ContainsKey(critical.Id))
            {
                switch (critical.Stacks)
                {
                    default:
                        return false;
                }
            }
            else
            {
                _modification.Add(critical.Id, critical);
            }

            return true;
        }

        public bool RemoveModification(CriticalInfo modification)
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

        public CriticalInfo this[int id]
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
