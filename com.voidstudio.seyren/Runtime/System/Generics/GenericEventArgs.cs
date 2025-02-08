using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Generics
{
    public class CancelableEventArgs : EventArgs
    {
        public bool Cancel;
        public CancelableEventArgs()
        {
            Cancel = false;
        }
    }
}
