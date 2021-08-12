using Seyren.Examples.Actions;
using Seyren.System.Abilities;
using Seyren.System.Actions;
using Seyren.System.Forces;
using Seyren.System.Generics;
using Seyren.System.Units;
using Seyren.System.Units.Projectiles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.Examples.Abilities
{
    public class AncientSlam : ActiveAbility
    {
        public static readonly int Id = 0x65678301;
        private Sprite sprite;
        private RuntimeAnimatorController controller;
        private Dictionary<Unit, int> hitList;
        private static string ancientEnergyPath = "Effect/AncientEnergy/AncientEnergy";

        public ActionConditionHandler RunCondition { get; }

        private GameObject go;
        private bool actionRun;

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;

        public AncientSlam(Unit u) : base(1)
        {

        }

        protected override void DoWhenCasting()
        {
            throw new NotImplementedException();
        }

        protected override void DoCastAbility()
        {
            throw new NotImplementedException();
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
    }
}
