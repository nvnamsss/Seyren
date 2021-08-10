using Seyren.Examples.DamageModification;
using Seyren.System.Terrains;
using Seyren.System.Actions;
using Seyren.System.Damages;
using Seyren.System.Damages.Critical;
using Seyren.System.Damages.Evasion;
using Seyren.System.Units.Projectiles;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using Seyren.System.Generics;

namespace Seyren.System.Units
{
    public partial class Unit : IUnit, IAttribute
    {
        public Unit()
        {
            JumpTimes = 1;
            Modification = new ModificationInfos();
            Attribute = new Attribute();
        }
        
        /// <summary>
        /// Kill this unit
        /// </summary>
        /// <param name="killer"></param>
        public void Kill(Unit killer)
        {
            Active = false;
            GameEventHandler<Unit, UnitDyingEventArgs> dying = Dying;
            UnitDyingEventArgs udinge = new UnitDyingEventArgs();
            if (dying != null)
            {
                dying.Invoke(this, udinge);
            }

            if (udinge.Cancel)
            {
                return;
            }

            GameEventHandler<Unit, UnitDiedEventArgs> died = Died;
            UnitDiedEventArgs udede = new UnitDiedEventArgs(killer);
            if (died != null)
            {
                died.Invoke(this, udede);
            }

            Killing?.Invoke(killer, this);
        }

        public void Damage(Unit source, float damage)
        {
            CurrentHp -= damage;
            if (CurrentHp < 0) Kill(source);
        }

        public virtual void Move(Vector3 location) {
            Vector3 old = position;
            position = location;
            Moved?.Invoke(this, new UnitMovedEventArgs(old, position));
        }

        public virtual void Look(Quaternion q) {
            Quaternion old = rotation;
            rotation = q;
            Rotated?.Invoke(this, new UnitRotatedEventArgs(old, rotation));
        }

    }

}
