﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.DamageSystem.PostPassive
{
    public abstract class PostPassiveInfo : IDamageModification<PostPassiveInfo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public bool CanStack { get; set; }
        public List<PostPassiveInfo> Stacks { get; set; }
        public StackType StackType { get; set; }

        public virtual void Trigger(DamageInfo info)
        {
            Triggered(info);
        }

        /// <summary>
        /// Doing something when post passive is triggered
        /// </summary>
        public abstract void Triggered(DamageInfo info);
    }
}
