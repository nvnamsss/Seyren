using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.PostPassive
{
    public class PostPassiveInfos : DamageModifications<PostPassiveInfo>
    {
        public PostPassiveInfos()
        {
            _modification = new Dictionary<int, PostPassiveInfo>();
        }
    }
}
