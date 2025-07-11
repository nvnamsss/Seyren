using System;

namespace Seyren.System.Common
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
