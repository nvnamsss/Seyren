using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Units;

namespace Seyren.System.Forces
{
    [Serializable]
    public class Force
    {
        public static Force None = new Force("None");
        public static Force Force1 = new Force("Force1");
        public static Force Force2 = new Force("Force2");
        
        public static List<Force> Forces { get; } = new List<Force>();
        /// <summary>
        /// Name of Force, Name is Unique
        /// </summary>
        public string Name;
        public Dictionary<string, Force> Alliances;
        // public Dictionary<string, Force> Enemies;

        Force(string name)
        {
            Name = name;
            Alliances = new Dictionary<string, Force>();
            // Enemies = new Dictionary<string, Force>();
        }

        Force(string name, Dictionary<string, Force> alliances, Dictionary<string, Force> enemies)
        {
            Name = name;
            Alliances = alliances;
            // Enemies = enemies;
        }

        public bool IsEnemy(Force force)
        {
            if (force == this) return false;
            return !Alliances.ContainsKey(force.Name);
        }

        public bool MakeAlliance(Force force)
        {
            //if (force == null) return false;

            if (!Alliances.ContainsKey(force.Name) && force != this)
            {
                Alliances.Add(force.Name, force);
                // Enemies.Remove(force.Name);
                force.MakeAlliance(this);

                return true;
            }

            return false;
        }

        public bool MakeEnemy(Force force)
        {
            //if (force == null) return false;

            if (Alliances.ContainsKey(force.Name) && force != this)
            {
                Alliances.Remove(force.Name);
                return true;
            }

            return false;
        }

        public static bool IsEnemy(IUnit a, IUnit b) {
            return a.Force.IsEnemy(b.Force);
        }
        
        public static Force CreateForce(string name)
        {
            Force force = new Force(name);
            int index = Forces.IndexOf(force);
            if (index >= 0)
            {
                UnityEngine.Debug.LogWarning("[Force] - Name of new force is existed");
                return Forces[index];
            }

            Forces.Add(force);

            for (int loop = 0; loop < Forces.Count; loop++)
            {
                force.MakeAlliance(Forces[loop]);
            }

            return force;
        }

        public static bool operator ==(Force lhs, Force rhs)
        {
            return lhs.Name == rhs.Name;
        }

        public static bool operator !=(Force lhs, Force rhs)
        {
            return lhs.Name != rhs.Name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
