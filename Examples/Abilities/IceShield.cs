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
using Seyren.System.Actions;

namespace Seyren.Examples.Abilities
{
    public class IceShield : ToggleAbility
    {
        private Dummy shield;
        public IceShield(Unit caster) : base(1)
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        protected override void onCast()
        {
            throw new NotImplementedException();
        }
    }
}
