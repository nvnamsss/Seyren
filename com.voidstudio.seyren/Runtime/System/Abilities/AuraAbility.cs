// using System.Collections;
// using System.Threading.Tasks;
// using UnityEngine;

// namespace Seyren.System.Abilities
// {
//     public abstract class AuraAbility : Ability
//     {
//         public bool active;
//         public float aoe;
//         public float interval;
//         public AuraAbility(float aoe, int level) : base(level)
//         {
//             this.active = false;
//             this.aoe = aoe;
//         }

//         protected abstract void AuraInterval();
//         protected virtual async void OnActiveAsync(float interval) {
//             int delay = (int)(interval * 1000);
//             while (active) {
//                 AuraInterval();
//                 await Task.Delay(delay);
//             }   
//         }

//         protected override void onCast()
//         {
//             active = !active;
//             if (active) {
//                 OnActiveAsync(interval);
//             }
//         }
//     }
// }
