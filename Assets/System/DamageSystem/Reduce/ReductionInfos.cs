using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem.Reduce
{
    public class ReductionInfos : IEnumerableModification
    {
        public Hashtable Modifications { get; set; }

        public bool AddModification(IDamageModification modification)
        {
            throw new NotImplementedException();
        }

        public IDamageModification GetModification(string id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveModification(IDamageModification modification)
        {
            throw new NotImplementedException();
        }

        public bool SetModification(string id, IDamageModification modification)
        {
            throw new NotImplementedException();
        }

        public ReductionInfos()
        {
            Modifications = new Hashtable();
        }
    }
}
