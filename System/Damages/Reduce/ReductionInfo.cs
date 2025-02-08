﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages.Reduce
{
    public abstract class ReductionInfo : IDamageModification<ReductionInfo>, IModifier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public List<ReductionInfo> Stacks { get; set; }
        public StackType StackType { get; set; }
        public bool CanEvade { get; set; }
        public bool CanCritical { get; set; }
        public bool CanReduce { get; set; }

        public void Trigger(Damage info)
        {
            Reduced(info);
        }

        public void Reduced(Damage damageInfo)
        {

        }
    }
}
