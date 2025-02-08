using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seyren.System.Forces
{
    public class Player
    {
        public static Dictionary<string, Player> Players { get; } = new Dictionary<string, Player>();
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
        public string Name;
        private Force _force;

        Player(string name, Force force)
        {
            Name = name;
            _force = force;
        }

        public static Player CreatePlayer(string name, Force force)
        {
            if (Players.ContainsKey(name))
            {
                return Players[name];
            }

            Player player = new Player(name, force);
            Players.Add(name, player);

            return new Player(name, force);
        }
    }
}
