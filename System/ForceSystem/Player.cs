using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ForceSystem
{
    public class Player
    {
        public delegate void ForceChangedHandler(Player sender, ForceChangedEventArgs e);
        public event ForceChangedHandler ForceChanged;
        public Force Force
        {
            get
            {
                return _force;
            }
            set
            {
                ForceChangedEventArgs e = new ForceChangedEventArgs(_force, value);
                _force = value;
                ForceChanged?.Invoke(this, e);
            }
        }

        private Force _force;
    }
}
