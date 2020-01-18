using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Units;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class DoubleJump : InstantAbility
    {
        public static readonly int Id = 0x74857701;
        public Unit unit;
        public GameObject Effect;
        private static string cyclonePath = "Effect/Cyclone/Cyclone_Effect";
        public DoubleJump(Unit u) : base(u, 1.0f, 1)
        {
            Effect = Resources.Load<GameObject>(cyclonePath);
            unit = u;
            BaseCoolDown = 1.0f;
        }

        protected override void DoCastAbility()
        {
            Vector2 force = new Vector2(0, 0.8f * unit.Attribute.JumpSpeed);
            unit.Body.AddForce(force, ForceMode2D.Impulse);
            GameObject effect = Object.Instantiate(Effect, unit.transform.position, unit.transform.rotation);
            Object.Destroy(effect, 0.5f);
        }

        protected override bool Condition()
        {
            return TimeCoolDownLeft > 0;
        }
    }
}
