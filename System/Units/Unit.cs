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

        void Awake()
        {
            // Body = GetComponent<Rigidbody2D>();
            // Collider = GetComponent<Collider2D>();
            // Ability = new Abilities.AbilityCollection();
            // Abilites = new Dictionary<int, Abilities.Ability>();
            Active = true;
            // Attribute = BaseAttribute + Attribute.zero;
        }
        void Start()
        {
            // _currentJump = JumpTimes;
            // _currentHp = Attribute.MaxHp;
            // _currentMp = Attribute.MaxMp;
            // _currentPShield = Attribute.PShield;
            // _currentMShield = Attribute.MShield;
        }

        void Update()
        {
            //Attribute.Update();
            //UpdateGrounding();
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

        // public bool Jump()
        // {
        //     if (!Active)
        //     {
        //         return false;
        //     }

        //     if (_currentJump > 0)
        //     {
        //         if (StandOn == GroundType.Ground || StandOn == GroundType.Grass)
        //         {
        //             Body.AddForce(Vector2.up * Attribute.JumpSpeed, ForceMode2D.Impulse);
        //             _currentJump -= 1;

        //             return true;
        //         }
        //     }
        //     else
        //     {

        //     }

        //     return false;
        // }
        
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

        /// <summary>
        /// Move unit follow direction
        /// </summary>
        // /// <param name="direction"></param>
        // public virtual void Move(Vector2 direction)
        // {
        //     if (!Active)
        //     {
        //         return;
        //     }

        //     if ((UnitStatus | UnitStatus.Stun) == UnitStatus ||
        //         (UnitStatus | UnitStatus.Knockback) == UnitStatus)
        //     {
        //         return;
        //     }

        //     Vector2 translate = direction * Attribute.MovementSpeed;
        //     transform.position += (Vector3)translate;
        // }

        /// <summary>
        /// Move unit follow direction for a time
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="time"></param>
        // public virtual void Move(Vector2 direction, int tick, float delay)
        // {
        //     if (!Active)
        //     {
        //         return;
        //     }

        //     StartCoroutine(MoveTo(direction, tick, delay));
        // }




        // public void Look(Vector2 direction)
        // {
        //     Look(Forward, direction);
        // }

        // public void Look(Vector2 forward, Vector2 direction)
        // {
        //     if (!Active)
        //     {
        //         return;
        //     }

        //     float forwardDot = Vector2.Dot(forward, direction);
        //     if (forwardDot == 0)
        //     {
        //         float angle = Vector2.SignedAngle(forward, direction);
        //         forwardDot = angle > 0 ? -1 : 1;
        //     }
        //     Vector2 f = forward * forwardDot;
        //     Quaternion q1 = Quaternion.FromToRotation(forward, f);
        //     Quaternion q2 = Quaternion.FromToRotation(f, direction);
        //     transform.rotation = q2 * q1;
        // }

        // public bool Spell(int spellId)
        // {
        //     if (!Active)
        //     {
        //         return false;
        //     }

        //     if (Abilites.ContainsKey(spellId))
        //     {
        //         return Abilites[spellId].Cast();
        //     }
        //     else
        //     {
        //         Debug.LogWarning("[Unit] - Unit " + name + " do not contain " + spellId + " ability");
        //     }

        //     return false;
        // }


    }

}
