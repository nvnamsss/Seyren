using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem.PrePassive
{
    public class PrePassiveInfos : IEnumerableModification
    {
        public Hashtable Modifications { get; set; }

        public PrePassiveInfos()
        {
            Modifications = new Hashtable();
        }
        public bool AddModification(IDamageModification modification)
        {
            PrePassiveInfo passive = modification as PrePassiveInfo;

            if (Modifications.ContainsKey(passive.Id))
            {
                switch (passive.Stacks)
                {
                    default:
                        return false;
                }
            }
            else
            {
                Modifications.Add(passive.Id, passive);
            }

            return true;
        }

        public IDamageModification GetModification(string id)
        {
            if (!Modifications.ContainsKey(id))
            {
                return PrePassiveInfo.None;
            }

            return Modifications[id] as PrePassiveInfo;
        }

        public bool RemoveModification(IDamageModification modification)
        {
            throw new NotImplementedException();
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
    }
}
