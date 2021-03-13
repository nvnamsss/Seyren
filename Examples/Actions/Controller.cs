using System;
using System.Collections;
using System.Collections.Generic;
using Seyren.System.Abilities;
using Seyren.System.Terrains;
using Seyren.System.Units;
using UnityEngine;
namespace Seyren.Examples.Actions
{
    public class Controller : MonoBehaviour
    {
        public Rigidbody2D Body;
        public Collider2D Collider;
        public Action action;
        public Unit unit;
        public List<Work> works;
        public Queue<int> pending;

        private long lastAttack;
        private void Awake()
        {
            unit = new Unit();
            unit.Attribute.MovementSpeed = new BaseFloat(1, 0);
            unit.Moved += (u, e) => {
                transform.position = e.NewPosition;
            };
        }

        public void Cast(Ability ability)
        {
            if (ability.CooldownRemaining > 0) return;
            action.DoAction(() => {
                ability.Cast(unit);
                Debug.Log("Cast");
            });
        }

        public void Attack()
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (lastAttack < now) return;

            lastAttack = now;
            action.DoAction(() => {
                // cast attack ability
                Debug.Log("Attack");
            });
        }

        private void FixedUpdate()
        {
            StartCoroutine(MoveTo(Vector3.right, 10, Time.deltaTime));
        }

        private void Update()
        {

        }

        /// <summary>
        /// Move unit by a direction for a time
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="time"></param>
        private IEnumerator MoveTo(Vector3 direction, int tick, float delay)
        {
            WaitForSeconds wait = new WaitForSeconds(delay);
            while (tick > 0)
            {
                yield return wait;
                tick -= 1;
                Vector3 to = unit.position + direction * unit.Attribute.MovementSpeed.Total;
                unit.Move(to);
            }
        }
        
        private IEnumerator Rotate(Quaternion quaternion, int tick, float delay) {
            WaitForSeconds wait = new WaitForSeconds(delay);
            while (tick > 0)
            {
                yield return wait;
                tick -= 1;
            }
        }
        
        public void cancelWork()
        {
            for (int loop = 0; loop < works.Count; loop++)
            {
                works[loop].Cancel();
            }
        }

        private void UpdateGrounding()
        {
            if (Collider.IsTouchingLayers(Ground.Grass))
            {
                unit.StandOn = GroundType.Grass;
            }
            else if (Collider.IsTouchingLayers(Ground.Hard))
            {
                unit.StandOn = GroundType.Ground;
            }
            else
                unit.StandOn = GroundType.Unknown;
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            switch (other.transform.tag)
            {
                case "GrassGround":
                    break;
                case "HardGround":
                    break;
                case "Enemy":
                    break;
            }
        }

        public void OnCollisionExit2D(Collision2D other)
        {
            unit.StandOn = GroundType.Unknown;
        }
    }

}