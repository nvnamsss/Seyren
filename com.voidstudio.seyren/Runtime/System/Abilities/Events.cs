using System;

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
