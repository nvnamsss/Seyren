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
            ConditionEventArgs<GameObject> e = new ConditionEventArgs<GameObject>(collision.gameObject, true);
            Condition?.Invoke(this, e);

            if (e.Match)
            {
                Unit unit = collision.gameObject.GetComponent<Unit>();
                Projectile projectile = collision.gameObject.GetComponent<Projectile>();
                Dummy dummy = collision.gameObject.GetComponent<Dummy>();

                if (unit != null)
                {
                    AffectedUnits.Remove(unit);
                    UnitOut?.Invoke(this, unit);
                }

                if (projectile != null)
                {
                    AffectedProjectiles.Remove(projectile);
                    ProjectileOut?.Invoke(this, projectile);
                }

                if (dummy != null)
                {
                    AffectedDummies.Remove(dummy);
                    DummyOut?.Invoke(this, dummy);
                }
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            ConditionEventArgs<GameObject> e = new ConditionEventArgs<GameObject>(collision.gameObject, true);
            Condition?.Invoke(this, e);

            if (e.Match)
            {
                Unit unit = collision.gameObject.GetComponent<Unit>();
                Projectile projectile = collision.gameObject.GetComponent<Projectile>();
                Dummy dummy = collision.gameObject.GetComponent<Dummy>();

                if (unit != null)
                {
                    AffectedUnits.Remove(unit);
                    UnitOut?.Invoke(this, unit);
                }

                if (projectile != null)
                {
                    AffectedProjectiles.Remove(projectile);
                    ProjectileOut?.Invoke(this, projectile);
                }

                if (dummy != null)
                {
                    AffectedDummies.Remove(dummy);
                    DummyOut?.Invoke(this, dummy);
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
                AffectedUnits.Remove(unit);
                UnitOut?.Invoke(this, unit);
            }

            if (projectile != null)
            {
                AffectedProjectiles.Remove(projectile);
                ProjectileOut?.Invoke(this, projectile);
            }

            if (dummy != null)
            {
                AffectedDummies.Remove(dummy);
                DummyOut?.Invoke(this, dummy);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Unit unit = collision.GetComponent<Unit>();
            Projectile projectile = collision.GetComponent<Projectile>();
            Dummy dummy = collision.GetComponent<Dummy>();

            if (unit != null)
            {
                AffectedUnits.Remove(unit);
                UnitOut?.Invoke(this, unit);
            }

            if (projectile != null)
            {
                AffectedProjectiles.Remove(projectile);
                ProjectileOut?.Invoke(this, projectile);
            }

            if (dummy != null)
            {
                AffectedDummies.Remove(dummy);
                DummyOut?.Invoke(this, dummy);
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
