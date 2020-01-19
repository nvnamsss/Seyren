using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.Evasion
{
    public class EvasionInfos : IEnumerableModification<EvasionInfo>, IEnumerable
    {
        private Dictionary<int, EvasionInfo> _modification;

        public EvasionInfos()
        {
            _modification = new Dictionary<int, EvasionInfo>();
        }
        public bool AddModification(EvasionInfo modification)
        {
            EvasionInfo evasion = modification as EvasionInfo;

            if (_modification.ContainsKey(evasion.Id))
            {
                evasion.Stacks.Add(modification);
                switch (evasion.Stacks)
                {
                    default:
                        return false;
                }
            }
            else
            {
                _modification.Add(evasion.Id, evasion);
            }

            return true;
        }

        public bool RemoveModification(EvasionInfo modification)
        {
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            return _modification.GetEnumerator();
        }

        public EvasionInfo this[int id]
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
