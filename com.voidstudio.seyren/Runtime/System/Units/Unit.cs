// using Seyren.System.Damages;
// using Seyren.System.States;
// using UnityEngine;
// using Seyren.System.Common;
// using System.Threading;
// using Seyren.System.Abilities;
// using System.Collections.Generic;

// namespace Seyren.System.Units
// {
//     public partial class Unit : IUnit
//     {
//         private static long id;

//         public string ReferenceID => throw new global::System.NotImplementedException();

//         string IUnit.UnitID => throw new global::System.NotImplementedException();

//         public Unit() : this(Interlocked.Increment(ref id))
//         {
//         }

//         Unit(long uid)
//         {
//             UnitID = uid;
//             Attribute = new Attribute();
//             _actions = new Actions.ActionCollection();
//             _state = new State();
//         }

//         public Error DamageTarget(Damage damageInfo)
//         {
//             Error err = null;
//             State.CurrentHp -= damageInfo.FinalDamage();
//             OnDamaged?.Invoke(this, new TakeDamageEventArgs(damageInfo));
//             if (State.CurrentHp < 0) err = Kill(this);

//             return err;
//         }

//         /// <summary>
//         /// Kill this unit
//         /// </summary>
//         /// <param name="killer"></param>
//         public Error Kill(IUnit by)
//         {
//             status = 0;
//             GameEventHandler<Unit, UnitDyingEventArgs> dying = Dying;
//             UnitDyingEventArgs udinge = new UnitDyingEventArgs();
//             if (dying != null)
//             {
//                 dying.Invoke(this, udinge);
//             }

//             if (udinge.Cancel)
//             {
//                 return new Error($"kill unit is cancel due to {udinge.CancelReason}");
//             }

//             GameEventHandler<Unit, UnitDiedEventArgs> died = Died;
//             UnitDiedEventArgs udede = new UnitDiedEventArgs(by);
//             if (died != null)
//             {
//                 died.Invoke(this, udede);
//             }

//             return null;
//         }

//         public Error Move(Vector3 location)
//         {
//             Vector3 old = _position;
//             this._position = location;
//             OnMoved?.Invoke(this, new MovedEventArgs(old, _position));
//             return null;
//         }

//         public Error Look(Quaternion quaternion)
//         {
//             Quaternion old = rotation;
//             rotation = quaternion;
//             Rotated?.Invoke(this, new UnitRotatedEventArgs(old, rotation));
//             return null;
//         }

//         public Error Cast(Ability ability)
//         {
//             return ability.Cast(this);
//         }

//         public AbilityV2 GetAbility(string abilityID)
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public List<IModifier> GetModifiers()
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public List<IResistance> GetResistances()
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public bool IsImmune()
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public List<IOnHitEffect> GetOnHitEffects()
//         {
//             throw new global::System.NotImplementedException();
//         }

//         public void InflictDamage(Damage damage)
//         {
//             throw new global::System.NotImplementedException();
//         }
//     }

// }
