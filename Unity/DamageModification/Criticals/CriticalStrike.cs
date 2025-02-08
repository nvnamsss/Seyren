using Seyren.System.Damages;
using Seyren.System.Damages.Critical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.Examples.DamageModification
{
    public class CriticalStrike : CriticalInfo
    {
        public CriticalStrike()
        {
            Id = 0xc01;
            Name = "Critical Strike";
            Chance = 80.0f;
            Multiple = 2.0f;
            StackType = StackType.Unique;
        }
        //public static CriticalInfo CriticalStrike()
        //{
        //    CriticalInfo critical = new CriticalInfo();

        //    critical.Id = 0xc01;
        //    critical.Name = "Critical Strike";
        //    critical.Chance = 80.0f;
        //    critical.Multiple = 2.0f;
        //    critical.Bonus = 0.0f;
        //    critical.StackType = System.DamageSystem.StackType.Diminishing;
        //    critical.Stacks = new List<System.DamageSystem.IDamageModification>();
        //    critical.CanEvade = true;
        //    critical.CanCritical = false;
        //    critical.CanReduce = false;

        //    return critical;
        //}
        public override void Critical(Damage info, bool success, float chance)
        {
          
        }
    }
}
