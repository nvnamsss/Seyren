// using Seyren.System.Generics;
// using Seyren.System.Units;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;
// using UnityEngine;

// namespace Seyren.System.Abilities
// {
//     public abstract class ActiveAbility : Ability
//     {
//         protected enum CooldownType
//         {
//             WhenCasting = 1 >> 1,
//             WhenCancelled = 1 >> 2,
//             WhenCast = 1 >> 3,
//         }

//         public float castTime;
//         protected bool casting;


//         protected CooldownType cooldownType;
//         protected abstract void DoWhenCasting();
//         protected abstract void DoCastAbility();
//         Task task;
//         CancellationTokenSource tokenSource;
//         public ActiveAbility(int level) : base(level)
//         {
//             CastType = CastType.Active;
//             casting = false;
//             tokenSource = new CancellationTokenSource();

//             // cancel = false;
//         }

//         public void Cancel()
//         {
//             tokenSource.Cancel();
//             casting = false;
//             if ((cooldownType | CooldownType.WhenCancelled) == cooldownType)
//             {
//                 cooldown();
//             }
//         }

//         public override Error Cast(IUnit by)
//         {
//             abilityTarget = AbilityTarget.NoTarget(by);
//             onCast();
//             return null;
//         }

//         public override Error Cast(IUnit by, IUnit target)
//         {
//             abilityTarget = AbilityTarget.UnitTarget(by, target);
//             onCast();
//             return null;
//         }
//         public override Error Cast(IUnit by, Vector3 location)
//         {
//             abilityTarget = AbilityTarget.PointTarget(by, location);
//             onCast();
//             return null;
//         }

//         protected override void onCast()
//         {
//             casting = true;
//             int c = (int)(castTime * 1000);

//             task = Task.Run(async () =>
//             {
//                 if ((cooldownType | CooldownType.WhenCasting) == cooldownType)
//                 {
//                     cooldown();
//                 }
//                 DoWhenCasting();

//                 await Task.Delay(1000);
//                 if (!casting) return;
//                 DoCastAbility();
//                 if (cooldownType == CooldownType.WhenCast)
//                 {
//                     cooldown();
//                 }
//                 casting = false;
//             }, tokenSource.Token);

//         }

//         protected void cooldown()
//         {
//             nextCooldown = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (long)(Cooldown * 1000);
//         }
//     }
// }
