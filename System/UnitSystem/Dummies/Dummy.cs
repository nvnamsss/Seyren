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
                ConditionEventArgs<Unit> e = new ConditionEventArgs<Unit>(unit, true);
                UnitCondition?.Invoke(this, e);
                
                if (e.Match)
                {
                    AffectedUnits.Add(unit);
                    UnitIn?.Invoke(this, unit);
                }
                
            }

            if (projectile != null)
            {
                ConditionEventArgs<Projectile> e = new ConditionEventArgs<Projectile>(projectile, true);
                ProjectileCondition?.Invoke(this, e);

                if (e.Match)
                {
                    AffectedProjectiles.Add(projectile);
                    ProjectileIn?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                ConditionEventArgs<Dummy> e = new ConditionEventArgs<Dummy>(dummy, true);
                DummyCondition?.Invoke(this, e);

                if (e.Match)
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
                ConditionEventArgs<Unit> e = new ConditionEventArgs<Unit>(unit, true);
                UnitCondition?.Invoke(this, e);
                
                if (e.Match)
                {
                    AffectedUnits.Add(unit);
                    UnitIn?.Invoke(this, unit);
                }
            }

            if (projectile != null)
            {
                ConditionEventArgs<Projectile> e = new ConditionEventArgs<Projectile>(projectile, true);
                ProjectileCondition?.Invoke(this, e);
                if (e.Match)
                {
                    AffectedProjectiles.Add(projectile);
                    ProjectileIn?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                ConditionEventArgs<Dummy> e = new ConditionEventArgs<Dummy>(dummy, true);
                DummyCondition?.Invoke(this, e);
                if (e.Match)
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
                ConditionEventArgs<Unit> e = new ConditionEventArgs<Unit>(unit, true);
                UnitCondition?.Invoke(this, e);

                if (e.Match)
                {
                    AffectedUnits.Remove(unit);
                    UnitOut?.Invoke(this, unit);
                }
            }

            if (projectile != null)
            {
                ConditionEventArgs<Projectile> e = new ConditionEventArgs<Projectile>(projectile, true);
                ProjectileCondition?.Invoke(this, e);

                if (e.Match)
                {
                    AffectedProjectiles.Remove(projectile);
                    ProjectileOut?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                ConditionEventArgs<Dummy> e = new ConditionEventArgs<Dummy>(dummy, true);
                DummyCondition?.Invoke(this, e);

                if (e.Match)
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
                ConditionEventArgs<Unit> e = new ConditionEventArgs<Unit>(unit, true);
                UnitCondition?.Invoke(this, e);

                if (e.Match)
                {
                    AffectedUnits.Remove(unit);
                    UnitOut?.Invoke(this, unit);
                }
            }

            if (projectile != null)
            {
                ConditionEventArgs<Projectile> e = new ConditionEventArgs<Projectile>(projectile, true);
                ProjectileCondition?.Invoke(this, e);

                if (e.Match)
                {
                    AffectedProjectiles.Remove(projectile);
                    ProjectileOut?.Invoke(this, projectile);
                }
            }

            if (dummy != null)
            {
                ConditionEventArgs<Dummy> e = new ConditionEventArgs<Dummy>(dummy, true);
                DummyCondition?.Invoke(this, e);

                if (e.Match)
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
    }
}
