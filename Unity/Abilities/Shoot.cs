// using System;
// using System.Collections.Generic;
// using Seyren.Algorithms;
// using Seyren.System.Abilities;
// using Seyren.System.Units;
// using Seyren.Universe;
// using UnityEngine;
// using UnityEngine.InputSystem.Interactions;

// namespace Seyren.Unity.Abilities {

//     public class Shoot : AbilityV2 {
//         static string abilityID = "Shoot";
//         public override bool IsDone => isDone;
//         string instanceID;
//         Vector3 location;
//         Vector3 direction;
//         int availableTick;
//         float speed;
//         bool isDone;
//         string path;

//         public Shoot(string projectilePath, Transform transform, Vector3 direction, float speed = 1.0f) {
//             // bullet = GameObject.Instantiate()
//             Guid uniqueId = Guid.NewGuid();
//             instanceID = uniqueId.ToString();

//             this.location = transform.position;
//             this.direction = direction;
//             this.availableTick = 80;
//             this.isDone = false;
//             this.speed = speed;
//             this.path = projectilePath;

//             // GameObject proj = Resources.Load<GameObject>(projectilePath);
//             // bullet = GameObject.Instantiate(proj);
//             // Debug.Log(bullet);

//             // pooling
//             // bullet = ObjectPool.GetObject(projectilePath);
//             // bullet.gameObject.transform.position = location;
//             // bullet.gameObject.transform.rotation = transform.rotation;
//         }

//         public override void End()
//         {
//             // Debug.Log("end");
//             // GameObject.Destroy(bullet);
//             // bullet.SetActive(false);
//             isDone = true;
//         }

//         public override string GetID()
//         {
//             return abilityID;
//         }

//         public override string GetInstanceID()
//         {
//             return instanceID;
//         }

//         public override void Process(IUniverse universe) {
//             // Debug.Log("Shoot");
//             location += direction * speed * universe.Time.DeltaTime;
//             // universe.CallAPI

//             List<IUnit> units = universe.Space.GetUnits(location, 0.5f);
//             if (units.Count > 0) {
//                 End();
//                 Debug.Log("Hit");
//             }

//             availableTick--;
//             if (availableTick <= 0) {
//                 End();
//             }
            
//         }
//     }
// }