using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.Generic
{
    public delegate void GameEventCancellableHandler<TSender, TEvent>(TSender s, TEvent e) where TEvent : CancelableEventArgs;
    public delegate void GameEventHandler<TSender>(TSender s);
    public delegate void GameEventHandler<TSender, TEvent>(TSender s, TEvent e);

    public class CancelableEventArgs : EventArgs
    {
        public bool Cancel;
        public CancelableEventArgs()
        {
            Cancel = false;
        }
    }
}
