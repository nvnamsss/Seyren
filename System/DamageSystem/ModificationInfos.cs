using Crom.System.DamageSystem.Critical;
using Crom.System.DamageSystem.Evasion;
using Crom.System.DamageSystem.PostPassive;
using Crom.System.DamageSystem.PrePassive;
using Crom.System.DamageSystem.Reduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crom.System.DamageSystem
{
    public class ModificationInfos
    {
        public PrePassiveInfos PrePassive { get; }
        public CriticalInfos Critical { get; }
        public EvasionInfos Evasion { get; }
        public ReductionInfos Reduction { get; }
        public PostPassiveInfos PostPassive { get; }

        public ModificationInfos()
        {
            PrePassive = new PrePassiveInfos();
            Critical = new CriticalInfos();
            Evasion = new EvasionInfos();
            Reduction = new ReductionInfos();
            PostPassive = new PostPassiveInfos();
        }

        public void Add(ModificationInfos modification)
        {

        }

        public void Remove(ModificationInfos modification)
        {

        }
    }
}
