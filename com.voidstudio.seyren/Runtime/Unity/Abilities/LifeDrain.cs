// using System.Collections.Generic;
// using Seyren.System.Abilities;
// using Seyren.System.Actions;
// using Seyren.System.Generics;
// using Seyren.System.Units;
// using Seyren.System.Damages;
// using UnityEngine;

// namespace Seyren.Examples.Abilities
// {
//     public class LifeDrain : System.Abilities.ChannelAbility
//     {
//         int damage;
//         DamageType damageType;
//         TriggerType triggerType;
//         public LifeDrain(int level) : base(level)
//         {
//             Cooldown = 7;
//             Targeting = TargetingType.UnitTarget;
//             damageType = DamageType.Magical;
//             triggerType = TriggerType.All;
//             ChannelTime = 10;
//             ChannelInterval = 0.1f;
//             switch (level) {
//                 case 1:
//                 damage = 100;
//                 break;
//                 case 2:
//                 damage = 200;
//                 break;
//                 case 3:
//                 damage = 300;
//                 break;
//             }
//         }

//         public ActionConditionHandler RunCondition => throw new global::System.NotImplementedException();

//         public event GameEventHandler<IAction> ActionStart;
//         public event GameEventHandler<IAction> ActionEnd;

//         public override IAction Action(IUnit by)
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public override IAction Action(IUnit by, IUnit target)
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public override IAction Action(IUnit by, Vector3 target)
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public override Ability Clone()
//         {
//             throw new global::System.NotImplementedException();
//         }

//         protected override Error Condition(IUnit by)
//         {
//             return null;
//         }

//         protected override Error Condition(IUnit by, IUnit target)
//         {
//             return null;
//         }

//         protected override Error Condition(IUnit by, Vector3 target)
//         {
//             return null;
//         }

//         protected override void DoChannelAbility()
//         {
//             Debug.Log("Channeling life drain");
//             DamageEngine.DamageTarget(abilityTarget.Source, abilityTarget.Target, damage,damageType, triggerType);
//         }
//     }
// }