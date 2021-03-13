using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Units.Dummies;
using Seyren.System.Units;
using System.Collections;
using UnityEngine;
using Seyren.System.Generics;

namespace Seyren.Examples.Abilities
{
    public class Dash : InstantAbility
    {
        public static readonly int Id = 0x68658301;
        public ActionConditionHandler RunCondition { get; }

        public ActionType ActionType { get; }

        public Dash(Unit u) : base(1, 1)
        {
            ActionType = ActionType.Dash;
            RunCondition += (s) =>
            {
                return true;
            };
        }

   

        private GameObject CreateClone()
        {
            // GameObject go = Unit.CreateShadow(Caster, 0.6f);
            //Renderer renderer = go.GetComponent<Renderer>();
            //Color color = renderer.material.color;
            //color.a *= 0.6f;
            //renderer.material.color = color;
    
            return null;
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



        protected override void DoCastAbility()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        protected override Error Condition(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }
    }
}
