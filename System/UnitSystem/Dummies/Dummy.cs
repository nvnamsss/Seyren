using Base2D.System.DamageSystem;
using Base2D.System.UnitSystem;
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

        }

        protected virtual void Awake()
        {

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
        }

        protected virtual void OnCollisionStay2D(Collision2D collision)
        {
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
        }
        public void Damage(Unit target, DamageType type)
        {

        }

        public void Damage(Unit target, float damage, DamageType type)
        {

        }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Hit()
        {
            throw new NotImplementedException();
        }
    }
}
