using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.PostPassive
{
    public class PostPassiveInfos : IEnumerableModification
    {
        public Hashtable Modifications { get; set; }

        public PostPassiveInfos()
        {
            Modifications = new Hashtable();
        }
        public bool AddModification(IDamageModification modification)
        {
            PostPassiveInfo passive = modification as PostPassiveInfo;

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
                return PostPassiveInfo.None;
            }

            return Modifications[id] as PostPassiveInfo;
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
