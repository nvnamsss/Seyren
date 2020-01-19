using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.PostPassive
{
    public class PostPassiveInfos : IEnumerableModification<PostPassiveInfo>, IEnumerable
    {
        public int Count => _modification.Count;
        private Dictionary<int, PostPassiveInfo> _modification;

        public PostPassiveInfos()
        {
            _modification = new Dictionary<int, PostPassiveInfo>();
        }
        public bool AddModification(PostPassiveInfo modification)
        {
            //if (_modification.ContainsKey(modification.Id))
            //{
            //    switch (modification.Stacks)
            //    {
            //        default:
            //            return false;
            //    }
            //}
            //else
            //{
            //    Modifications.Add(modification.Id, modification);
            //}

            return true;
        }


        public bool RemoveModification(PostPassiveInfo modification)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return _modification.GetEnumerator();
        }

        public PostPassiveInfo this[int id]
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
