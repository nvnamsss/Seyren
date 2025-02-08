using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Abilities
{
    public class CastingSpellEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public CastingSpellEventArgs()
        {
            Cancel = false;
        }
    }
}
