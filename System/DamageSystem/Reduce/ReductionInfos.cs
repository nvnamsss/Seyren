using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.Reduce
{
    public class ReductionInfos : IEnumerableModification<ReductionInfo>, IEnumerable
    {
        public int Count => _modification.Count;
        public ReductionInfo this[int id]
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

        private Dictionary<int, ReductionInfo> _modification;

        public bool AddModification(ReductionInfo modification)
        {
            throw new NotImplementedException();
        }

        public bool RemoveModification(ReductionInfo modification)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return _modification.GetEnumerator();
        }

    }
}
