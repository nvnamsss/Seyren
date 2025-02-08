﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages.PrePassive
{
    public abstract class PrePassiveInfo : IDamageModification<PrePassiveInfo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public bool CanStack { get; set; }
        public List<PrePassiveInfo> Stacks { get; set; }
        public StackType StackType { get; set; }

        public PrePassiveInfo()
        {
            Stacks = new List<PrePassiveInfo>();
        }

        public virtual void Trigger(Damage info)
        {
            Triggered(info);
        }

        /// <summary>
        /// Doing something when post passive is triggered
        /// </summary>
        public abstract void Triggered(Damage info);
    }
}
