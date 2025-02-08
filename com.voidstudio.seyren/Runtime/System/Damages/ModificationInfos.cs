using Seyren.System.Damages.Critical;
using Seyren.System.Damages.Evasion;
using Seyren.System.Damages.PostPassive;
using Seyren.System.Damages.PrePassive;
using Seyren.System.Damages.Reduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Seyren.System.Damages
{
    [Serializable]
    public class Modification
    {
        public PrePassiveInfos PrePassive { get; }
        public CriticalInfos Critical { get; }
        public EvasionInfos Evasion { get; }
        public ReductionInfos Reduction { get; }
        public PostPassiveInfos PostPassive { get; }
        public Modification()
        {
            PrePassive = new PrePassiveInfos();
            Critical = new CriticalInfos();
            Evasion = new EvasionInfos();
            Reduction = new ReductionInfos();
            PostPassive = new PostPassiveInfos();
        }

        public void Add(Modification modification)
        {
            foreach (var item in Critical)
            {
                
            }
        }

        public void Remove(Modification modification)
        {

        }
    }
}
