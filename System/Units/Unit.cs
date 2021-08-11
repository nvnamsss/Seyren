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
using System.Threading;

namespace Seyren.System.Units
{
    public partial class Unit : IUnit, IAttribute
    {
        private static long id;
        public Unit() : this(Interlocked.Increment(ref id))
        {
        }

        Unit(long uid)
        {
            UnitID = uid;
            JumpTimes = 1;
            Modification = new ModificationInfos();
            Attribute = new Attribute();
        }

        public Error Damage(DamageInfo damageInfo)
        {
            Error err = null;
            CurrentHp -= damageInfo.DamageAmount;
            OnDamaged?.Invoke(this, new TakeDamageEventArgs(damageInfo));
            if (CurrentHp < 0) err = Kill(this);

            return err;
        }

        /// <summary>
        /// Kill this unit
        /// </summary>
        /// <param name="killer"></param>
        public Error Kill(IUnit by)
        {
            State = 0;
            GameEventHandler<Unit, UnitDyingEventArgs> dying = Dying;
            UnitDyingEventArgs udinge = new UnitDyingEventArgs();
            if (dying != null)
            {
                dying.Invoke(this, udinge);
            }

            if (udinge.Cancel)
            {
                return new Error($"kill unit is cancel due to {udinge.CancelReason}");
            }

            GameEventHandler<Unit, UnitDiedEventArgs> died = Died;
            UnitDiedEventArgs udede = new UnitDiedEventArgs(by);
            if (died != null)
            {
                died.Invoke(this, udede);
            }

            return null;
        }

        public Error Move(Vector3 location)
        {
            Vector3 old = _position;
            this._position = location;
            OnMoved?.Invoke(this, new UnitMovedEventArgs(old, _position));
            return null;
        }

        public Error Look(Quaternion quaternion)
        {
            Quaternion old = rotation;
            rotation = quaternion;
            Rotated?.Invoke(this, new UnitRotatedEventArgs(old, rotation));
            return null;
        }
    }

}
