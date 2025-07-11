using System;
using System.Collections.Generic;
using System.Linq;
using Seyren.System.Units;

namespace Seyren.System.Forces
{
    [Serializable]
    public class Force
    {
        public static Force None = new Force("None");
        public static Force Force1 = new Force("Force1");
        public static Force Force2 = new Force("Force2");

        // Lock object for thread safety
        private static readonly object syncLock = new object();
        
        private static Dictionary<string, Force> forces = new Dictionary<string, Force>
        {
            { "None", None },
            { "Force1", Force1 },
            { "Force2", Force2 }
        };

        public static Force Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return None;
            }
            
            lock (syncLock)
            {
                if (forces != null && forces.ContainsKey(name))
                {
                    return forces[name];
                }

                return None;
            }
        }

        public static Force Create(string name)
        {
            lock (syncLock)
            {
                if (forces.ContainsKey(name))
                {
                    UnityEngine.Debug.LogWarning($"[Force] - Force with name '{name}' already exists. Returning existing force.");
                    return forces[name];
                }

                Force newForce = new Force(name);
                forces[name] = newForce;
                Forces.Add(newForce);

                // Automatically make alliances with all existing forces
                foreach (var force in Forces)
                {
                    if (force != newForce)
                    {
                        newForce.MakeAlliance(force);
                    }
                }

                return newForce;
            }
        }

        public static Force[] List()
        {
            lock (syncLock)
            {
                return forces.Values.ToArray();
            }
        }
        
        
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

            lock (syncLock)
            {
                if (!Alliances.ContainsKey(force.Name) && force != this)
                {
                    Alliances.Add(force.Name, force);
                    // Enemies.Remove(force.Name);
                    force.MakeAlliance(this);

                    return true;
                }
            }

            return false;
        }

        public bool MakeEnemy(Force force)
        {
            //if (force == null) return false;

            lock (syncLock)
            {
                if (Alliances.ContainsKey(force.Name) && force != this)
                {
                    Alliances.Remove(force.Name);
                    return true;
                }
            }

            return false;
        }

        public static bool IsEnemy(IUnit a, IUnit b) {
            return a.Force.IsEnemy(b.Force);
        }
        
        public static bool operator ==(Force lhs, Force rhs)
        {
            if (ReferenceEquals(lhs, null) && ReferenceEquals(rhs, null))
            {
                return true;
            }

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
            {
                return false;
            }
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
