using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem.Evasion
{
    public class EvasionInfos : IEnumerableModification
    {
        public Hashtable Modifications { get; set; }

        public EvasionInfos()
        {
            Modifications = new Hashtable();
        }
        public bool AddModification(IDamageModification modification)
        {
            EvasionInfo evasion = modification as EvasionInfo;

            if (Modifications.ContainsKey(evasion.Id))
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
                Modifications.Add(evasion.Id, evasion);
            }

            return true;
        }

        public IDamageModification GetModification(string id)
        {
            if (!Modifications.ContainsKey(id))
            {
                return EvasionInfo.None;
            }

            return Modifications[id] as EvasionInfo;
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
