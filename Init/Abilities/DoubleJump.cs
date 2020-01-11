using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Units;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class DoubleJump : Ability
    {
        public static readonly int Id = 0x74857701;
        public Unit unit;
        public GameObject Effect;
        private static string cyclonePath = "Effect/Cyclone/Cyclone_Effect";
        public DoubleJump(Unit u) : base(u, 0, 1.0f, 1)
        {
            Effect = Resources.Load<GameObject>(cyclonePath);
            unit = u;
            BaseCoolDown = 1.0f;
        }

        public override bool Cast()
        {
            if (TimeCoolDownLeft > 0)
            {
                return false;
            }

            Vector2 force = new Vector2(0, 0.8f * unit.JumpSpeed);
            unit.Body.AddForce(force, ForceMode2D.Impulse);
            GameObject effect = Object.Instantiate(Effect, unit.transform.position, unit.transform.rotation);
            Object.Destroy(effect, 0.5f);
            TimeCoolDownLeft = BaseCoolDown;
            return true;
        }

        public GameObject Create(Vector2 location, Quaternion rotation)
        {

            return null;
        }

        protected override void DoCastAbility()
        {
        }

        protected override bool Condition()
        {
            throw new global::System.NotImplementedException();
        }
    }
}
