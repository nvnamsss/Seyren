using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.Critical
{
    public class CriticalInfos : IEnumerableModification
    {
        public Hashtable Modifications { get; set; }

        public CriticalInfos()
        {
            Modifications = new Hashtable();
        }

        public bool AddModification(IDamageModification modification)
        {
            CriticalInfo critical = modification as CriticalInfo;

            if (Modifications.ContainsKey(critical.Id))
            {
                switch (critical.Stacks)
                {
                    default:
                        return false;
                }
            }
            else
            {
                Modifications.Add(critical.Id, critical);
            }

            return true;
        }

        public IDamageModification GetModification(string id)
        {
            if (!Modifications.ContainsKey(id))
            {
                return CriticalInfo.None;
            }

            return Modifications[id] as CriticalInfo;
        }

        public bool SetModification(string id, IDamageModification modification)
        {

            if (!Modifications.ContainsKey(id))
            {
                return false;
            }

            Modifications[id] = modification;

            return true;
        }

        public bool RemoveModification(IDamageModification modification)
        {
            return false;
        }
    }
}
