﻿using Seyren.System.Abilities;
using Seyren.System.Units.Projectiles;
using Seyren.System.Units;
using System;
using System.Collections;
using UnityEngine;
using Seyren.System.Generics;
using Seyren.System.Actions;
using Seyren.Examples.Actions;

namespace Seyren.Examples.Abilities
{
    public class CharacterSwordAttack : ActiveAbility
    {
        public static readonly int Id = 0x67658401;
        public Unit unit;
        private Sprite sprite;
        public CharacterSwordAttack(Unit u) : base( 0.25f, 10 / u.Attribute.AttackSpeed.Total, 1)
        {

        }

        public override IAction Action(Unit by)
        {
            throw new NotImplementedException();
        }

        public override IAction Action(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        public override IAction Action(Unit by, Vector3 target)
        {
            return new ActionPipeline(new IThing[] {
                new AnimationThing("attack"),
                new DelayThing(10),
                new DoThing(() => {
                    Cast(by, target);        
                })
            });
        }

        public override long CastTime(Unit unit)
        {
            return 0;
        }

        public GameObject Create(Vector2 location, Quaternion rotation)
        {
            return null;
            // MissileProjectile missile = MissileProjectile.Create("CharacterSwordAttack",
            //     location,
            //     rotation,
            //     sprite,
            //     0,
            //     0.4f);
            // //missile.Collider.size = new Vector2(2.4f, 2.4f);
            // missile.MaxHit = 10000000;
            // missile.Owner = unit;
            // missile.OnHit += (sender, e) =>
            // {
            //     Unit u = e.GetComponent<Unit>();
            //     if (u != null && unit.IsEnemy(u))
            //     {
            //         Debug.Log("Damage");
            //     }
            //     else
            //     {
            //         Debug.Log("Cannot damage");
            //     }
            // };

            // return missile.gameObject;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override Error Condition(Unit by)
        {
            throw new NotImplementedException();
        }

        protected override Error Condition(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        protected override Error Condition(Unit by, Vector3 target)
        {
            throw new NotImplementedException();
        }

        protected override void DoCastAbility()
        {
            Debug.Log("Character sword attack");
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by)
        {
            Debug.Log("Character sword attack");
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            Debug.Log("Character sword attack");
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            Debug.Log("Character sword attack");
            throw new NotImplementedException();
        }



        // protected override void DoCastAbility()
        // {
        //     //unit.Action.Animator.SetTrigger("end-atk1");
        //     Vector2 location = unit.transform.position;
        //     Quaternion rotation = unit.transform.rotation;

        //     location = location + (Vector2)(rotation * Vector2.left * 2);
        //     Create(location, rotation);
        //     unit.Action.Type = System.Actions.ActionType.None;
        //     CooldownRemaining = Cooldown;
        // }



    }
}