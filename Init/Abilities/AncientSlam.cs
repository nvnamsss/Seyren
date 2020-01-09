using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System.Collections;
using UnityEngine;

namespace Base2D.Init.Abilities
{
    public class AncientSlam : Ability
    {
        public static readonly int Id = 0x65678301;
        private Unit unit;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        public AncientSlam(Unit u)
        {
            unit = u;
            BaseCoolDown = 10;
            BaseCastingTime = 2;
            controller = ProjectileCollection.AncientEnergyController;
        }

        public override bool Cast()
        {
            if (TimeCoolDownLeft > 0 || 
                IsCasting ||
                unit.Action.Type == System.ActionSystem.ActionType.CastAbility ||
                unit.Action.Type == System.ActionSystem.ActionType.Attack)
            {
                return false;
            }

            unit.Action.Type = System.ActionSystem.ActionType.CastAbility;
            unit.Action.Animator.SetBool("Spell", true);
            unit.StartCoroutine(Casting(0.4f, BaseCastingTime));
            return true;
        }

        IEnumerator Casting(float timeDelay, float timeCasting)
        {
            IsCasting = true;
            yield return new WaitForSeconds(timeDelay);
            TimeCastingLeft = timeCasting - timeDelay;

            while (TimeCastingLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                TimeCastingLeft -= timeDelay;
            }

            if (IsCasting)
            {
                IsCasting = false;
                DoCastAbility();
            }

        }


        public override GameObject Create(Vector2 location, Quaternion rotation)
        {
            return null;
        }

        public void Create()
        {
            float angle = 180;
            for (int loop = 0; loop < 7; loop++)
            {
                float rad = (float)(angle * Mathf.Deg2Rad);
                Vector2 location = unit.transform.position;
                location.y -= 1;
                Quaternion rotation = unit.transform.rotation;
                Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

                MissileProjectile missile = MissileProjectile.Create("Ancient Slam - Energy",
                    location,
                    rotation,
                    sprite,
                    controller,
                    10,
                    2);

                missile.direction = direction;
                missile.transform.localScale = new Vector3(3, 3, 1);
                missile.MaxHit = 100;
                missile.Owner = unit;

                angle = angle - 30;
            }
        }

        protected override void DoCastAbility()
        {
            unit.Action.Animator.SetBool("Spell", false);

            Create();
            unit.Action.Type = System.ActionSystem.ActionType.None;
            TimeCoolDownLeft = BaseCoolDown;
            unit.StartCoroutine(StartCoolDown(Time.deltaTime, BaseCoolDown));
        }
    }
}
