using Base2D.System.AbilitySystem;
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
        }

        protected override void AuraInterval()
        {
            Debug.Log("interval");
            foreach (Unit unit in dummy.AffectedUnits)
            {
                unit.Armor += 3;
            }
        }

        protected override bool Condition()
        {
            return true;
        }
    }
}
