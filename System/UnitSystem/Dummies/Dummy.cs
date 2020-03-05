using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem;
using Base2D.System.UnitSystem.EventData;
using Base2D.System.UnitSystem.Projectiles;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem.Dummies
{
    /// <summary>
    /// Represents a fake unit they can help to make many unit based features
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public partial class Dummy : MonoBehaviour, IAttribute, IObject
    {
        Dummy()
        {
            AffectedUnits = new List<Unit>();
            AffectedDummies = new List<Dummy>();
            AffectedProjectiles= new List<Projectile >();
            Modification = new ModificationInfos();
            UnitCondition = (s, u) => true;
            ProjectileCondition = (s, p) => true;
            DummyCondition = (s, d) => true;
        }

        protected virtual void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
        }

        protected virtual void Start()
        {

        }
            
        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            Dummy dummy = collision.gameObject.GetComponent<Dummy>();

            if (unit != null)
            {
                bool match = UnitCondition(this, unit);
                
                if (match)
                {
                    AffectedUnits.Add(unit);
                    UnitIn?.Invoke(this, unit);
                }
                
            }

            if (projectile != null)
            {
                ConditionEventArgs<Projectile> e = new ConditionEventArgs<Projectile>(projectile, true);
                bool match = ProjectileCondition(this, projectile);

                if (match)
                {
                    AffectedProjectiles.Add(projectile);
                    ProjectileIn?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                ConditionEventArgs<Dummy> e = new ConditionEventArgs<Dummy>(dummy, true);
                bool? match = DummyCondition?.Invoke(this, dummy);

                if (match.Value) 
                {
                    AffectedDummies.Add(dummy);
                    DummyIn?.Invoke(this, dummy);
                }
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            Dummy dummy = collision.gameObject.GetComponent<Dummy>();

            if (unit != null)
            {
                bool match = UnitCondition(this, unit);
                
                if (match)
                {
                    AffectedUnits.Add(unit);
                    UnitIn?.Invoke(this, unit);
                }
            }

            if (projectile != null)
            {
                bool match = ProjectileCondition(this, projectile);
                if (match)
                {
                    AffectedProjectiles.Add(projectile);
                    ProjectileIn?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                bool match = DummyCondition(this, dummy);
                if (match)
                {
                    AffectedDummies.Add(dummy);
                    DummyIn?.Invoke(this, dummy);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            Dummy dummy = collision.gameObject.GetComponent<Dummy>();

            if (unit != null)
            {
                bool match = UnitCondition(this, unit);

                if (match)
                {
                    AffectedUnits.Remove(unit);
                    UnitOut?.Invoke(this, unit);
                }
            }

            if (projectile != null)
            {
                bool match = ProjectileCondition(this, projectile);

                if (match)
                {
                    AffectedProjectiles.Remove(projectile);
                    ProjectileOut?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                bool match = DummyCondition(this, dummy);

                if (match)
                {
                    AffectedDummies.Remove(dummy);
                    DummyOut?.Invoke(this, dummy);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Unit unit = collision.GetComponent<Unit>();
            Projectile projectile = collision.GetComponent<Projectile>();
            Dummy dummy = collision.GetComponent<Dummy>();

            if (unit != null)
            {
                bool match = UnitCondition(this, unit);

                if (match)
                {
                    AffectedUnits.Remove(unit);
                    UnitOut?.Invoke(this, unit);
                }
            }

            if (projectile != null)
            {
                bool match = ProjectileCondition(this, projectile);

                if (match)
                {
                    AffectedProjectiles.Remove(projectile);
                    ProjectileOut?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                bool match = DummyCondition(this, dummy);

                if (match)
                {
                    AffectedDummies.Remove(dummy);
                    DummyOut?.Invoke(this, dummy);
                }
            }
        }

        public void Damage(Unit target, DamageType type)
        {

        }

        public void Damage(Unit target, float damage, DamageType type)
        {

        }

        public void Look(Vector2 direction)
        {
            Look(Forward, direction);
        }

        public void Look(Vector2 forward, Vector2 direction)
        {
            if (!Active)
            {
                return;
            }

            float forwardDot = Vector2.Dot(forward, direction);
            Vector2 f = forward * forwardDot;
            Quaternion q1 = Quaternion.FromToRotation(forward, f);
            Quaternion q2 = Quaternion.FromToRotation(f, direction);
            transform.rotation = q2 * q1;
        }
    }
}
