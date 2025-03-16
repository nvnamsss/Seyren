using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Damages.Resistances
{
    
    public abstract class Evasion :  IResistance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public StackType StackType { get; set; }

        public void Apply(Damage damage)
        {
            float chance = UnityEngine.Random.Range(0, 100);
            if (chance < Chance)
            {
                damage.Evaded = true;
            }
        }
    }
}
