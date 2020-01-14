using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.AbilitySystem
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
