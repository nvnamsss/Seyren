using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Forces
{
    public class ForceChangedEventArgs : EventArgs
    {
        public Force OldForce { get; }
        public Force CurrentForce { get; }
        public ForceChangedEventArgs(Force old, Force current)
        {
            OldForce = old;
            CurrentForce = current;
        }
    }
}
