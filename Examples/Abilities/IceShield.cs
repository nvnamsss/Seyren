using Seyren.System.Abilities;
using Seyren.System.Units.Dummies;
using Seyren.System.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Seyren.System.Generics;

namespace Seyren.Examples.Abilities
{
    public class IceShield : ToggleAbility
    {
        private Dummy shield;
        public IceShield(Unit caster) : base()
        {
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

        }

        protected override void onCast(Unit by)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Unit target)
        {
            throw new NotImplementedException();
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            throw new NotImplementedException();
        }

    }
}
