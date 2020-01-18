﻿using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Dummies;
using Base2D.System.UnitSystem.Units;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class DevotionAura : AuraAbility
    {
        public static readonly int Id = 0x68696501;
        private Dummy dummy;
        public DevotionAura(Unit caster, float aoe, int level) : base(caster, aoe, level)
        {
            dummy = Dummy.CreateCircleDummy();
            CircleCollider2D collider = dummy.Collider as CircleCollider2D;
            collider.radius = AoE;
            dummy.Collider.isTrigger = true;
            dummy.Body.constraints = RigidbodyConstraints2D.FreezeAll;

            dummy.UnitIn += UnitInCallback;
            dummy.UnitOut += UnitOutCallback;
        }

        protected override void AuraInterval()
        {
    
        }

        protected override bool Condition()
        {
            return true;
        }

        private void UnitInCallback(Dummy sender, Unit triggerUnit)
        {
            triggerUnit.Attribute.Armor += 3;
        }

        private void UnitOutCallback(Dummy sender, Unit triggerUnit)
        {
            triggerUnit.Attribute.Armor -= 3;
        }
    }
}