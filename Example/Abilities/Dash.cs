using Base2D.System.AbilitySystem;
using Base2D.System.ActionSystem;
using Base2D.System.UnitSystem.Dummies;
using Base2D.System.UnitSystem.Units;
using System.Collections;
using UnityEngine;

namespace Base2D.Example.Abilities
{
    public class Dash : InstantAbility, IAction
    {
        public static readonly int Id = 0x68658301;
        public ActionConditionHandler RunCondition { get; }

        public ActionType ActionType { get; }

        public event ActionHandler ActionStart;
        public event ActionHandler ActionEnd;
        public Dash(Unit u) : base(u, 1, 1)
        {
            ActionType = ActionType.Dash;
            RunCondition += (s) =>
            {
                return true;
            };
        }

        public void Invoke()
        {
            bool cast = Cast();
            if (cast)
            {
                ActionStart?.Invoke(this);
            }
        }

        public void Revoke()
        {
            ActionEnd?.Invoke(this);
        }

        protected override bool Condition()
        {
            return !(!Active ||
                CooldownRemaining > 0);
        } 

        protected override void DoCastAbility()
        {
            Vector2 direction = Caster.transform.rotation * Caster.Forward * Time.fixedDeltaTime * Caster.Attribute.MovementSpeed * 1.4f;
            Caster.Move(direction, 10, 0.02f);
            //Caster.Body.AddForce(direction * 1000, ForceMode2D.Impulse);
            Caster.StartCoroutine(DashEffect(5, 0.04f));
        }

        private GameObject CreateClone()
        {
            GameObject go = Unit.CreateShadow(Caster, 0.6f);
            //Renderer renderer = go.GetComponent<Renderer>();
            //Color color = renderer.material.color;
            //color.a *= 0.6f;
            //renderer.material.color = color;

            return go;
        }

        private IEnumerator DashEffect(int tick, float delay)
        {
            WaitForSeconds wait = new WaitForSeconds(delay);

            while (tick > 0)
            {
                yield return wait;
                tick -= 1;
                GameObject clone = CreateClone();
                Object.Destroy(clone, delay * 3);
            }
        }

        protected override bool UnlockCondition()
        {
            return true;
        }
    }
}
