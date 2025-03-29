// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Seyren.System.Damages;
// using Seyren.System.Units;
// using UnityEngine;

// namespace Seyren.System.Units
// {
//     public class MovedEventArgs
//     {
//         public Vector3 OldPosition { get; }
//         public Vector3 NewPosition { get; }

//         public MovedEventArgs(Vector3 oldPosition, Vector3 newPosition)
//         {
//             OldPosition = oldPosition;
//             NewPosition = newPosition;
//         }
//     }

//     public class UnitRotatedEventArgs {
//         public Quaternion OldRotation {get;}
//         public Quaternion NewRotation {get;}
//         public UnitRotatedEventArgs(Quaternion oldRotation, Quaternion newRotation) {
//             OldRotation = oldRotation;
//             NewRotation = newRotation;
//         }
//     }


//     public class StatusChangedEventArgs : EventArgs
//     {
//         public UnitStatus Status { get; }
//         public StatusChangedEventArgs(UnitStatus status)
//         {
//             Status = status;
//         }
//     }

//     public class TakeDamageEventArgs
//     {
//         public Damage Info { get; }
//         public TakeDamageEventArgs(Damage info)
//         {
//             Info = info;
//         }
//     }

//     public class UnitDiedEventArgs
//     {
//         public IUnit Killer { get; }

//         public UnitDiedEventArgs(IUnit killer)
//         {
//             Killer = killer;
//         }
//     }

//     public class UnitDyingEventArgs
//     {
//         public bool Cancel { get; set; }
//         public string CancelReason {get;set;}
//         public UnitDyingEventArgs()
//         {
//             Cancel = false;
//         }
//     }

//     public class ConditionEventArgs<T> : EventArgs
//     {
//         public T Object { get; }
//         public bool Match { get; set; }
//         public ConditionEventArgs(T obj, bool match)
//         {
//             Object = obj;
//             Match = match;
//         }
//     }
// }
