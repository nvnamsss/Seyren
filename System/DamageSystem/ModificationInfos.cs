using Base2D.System.DamageSystem.Critical;
using Base2D.System.DamageSystem.Evasion;
using Base2D.System.DamageSystem.PostPassive;
using Base2D.System.DamageSystem.PrePassive;
using Base2D.System.DamageSystem.Reduce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Base2D.System.DamageSystem
{
    [Serializable]
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
            foreach (var item in Critical)
            {
                
            }
        }

        public void Remove(ModificationInfos modification)
        {

        }
    }
}
