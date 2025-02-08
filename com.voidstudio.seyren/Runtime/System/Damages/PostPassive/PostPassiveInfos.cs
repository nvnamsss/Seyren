using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages.PostPassive
{
    public class PostPassiveInfos : DamageModifications<PostPassiveInfo>
    {
        public PostPassiveInfos()
        {
            _modification = new Dictionary<int, PostPassiveInfo>();
        }
    }
}
