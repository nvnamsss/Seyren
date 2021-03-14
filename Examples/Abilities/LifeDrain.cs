using System.Collections.Generic;
using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Generics;
using Seyren.System.Units;
using UnityEngine;

namespace Seyren.Examples.Abilities
{
    public class LifeDrain : ChannelAbility, IAction
    {
        public LifeDrain(float channelTime, float interval, float cooldown, int level) : base(channelTime, interval, cooldown, level)
        {
        }

        public ActionConditionHandler RunCondition => throw new global::System.NotImplementedException();

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;

        public override IAction Action(Unit by)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(Unit by, Unit target)
        {
            throw new global::System.NotImplementedException();
        }

        public override IAction Action(Unit by, Vector3 target)
        {
            throw new global::System.NotImplementedException();
        }

        public bool Break()
        {
            throw new global::System.NotImplementedException();
        }

        public override long CastTime(Unit unit)
        {
            throw new global::System.NotImplementedException();
        }

        public bool Constraint(IAction action)
        {
            throw new global::System.NotImplementedException();
        }

        public IEnumerable<IThing> Do(params object[] obj)
        {
            throw new global::System.NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void Invoke()
        {
            throw new global::System.NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
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

        protected override void DoChannelAbility()
        {
            throw new global::System.NotImplementedException();
        }

        protected override void onCast(Unit by)
        {
            base.onCast(by);
        }

        protected override void onCast(Unit by, Unit target)
        {
            base.onCast(by, target);
        }

        protected override void onCast(Unit by, Vector3 target)
        {
            base.onCast(by, target);
        }
    }
}