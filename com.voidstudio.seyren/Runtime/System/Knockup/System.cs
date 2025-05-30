using System.Collections.Generic;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Knockup
{
    public interface IKnockupMotion
    {
        void Apply(ITime time, KnockupData data);
    }

    public class KnockupData
    {
        public IUnit target;
        public float currentHeight;
        public float prevHeight;
        public float maxHeight;
        public float duration;
        public float elapsed;
        public bool active = true;

        public KnockupData(IUnit target, float maxHeight, float duration)
        {
            this.target = target;
            this.maxHeight = maxHeight;
            this.duration = duration;
            this.elapsed = 0f;
        }
    }

    class KnockupInstance
    {
        public KnockupData data;
        public IKnockupMotion knockupMotion;
        public KnockupInstance(KnockupData data, IKnockupMotion knockupMotion)
        {
            this.data = data;
            this.knockupMotion = knockupMotion;
        }
    }

    public class KnockupSystem : ILoop
    {
        private Dictionary<string, KnockupInstance> targets = new Dictionary<string, KnockupInstance>();
        private IKnockupMotion defaultKnockupMotion;

        public KnockupSystem() : this(new SinusoidalKnockupMotion())
        {
        }

        public KnockupSystem(IKnockupMotion knockupMotion)
        {
            defaultKnockupMotion = knockupMotion;
        }


        public void ApplyKnockup(IUnit target, float height, float duration)
        {
            if (target == null || height <= 0 || duration <= 0)
            {
                Debug.LogWarning("Invalid parameters for knockup");
                return;
            }

            if (targets.ContainsKey(target.ID))
            {
                KnockupData data = new KnockupData(target, height, duration);
                KnockupInstance instance = new KnockupInstance(data, defaultKnockupMotion);
                targets[target.ID] = instance;
            }
            else
            {
                targets.Add(target.ID, new KnockupInstance(new KnockupData(target, height, duration), defaultKnockupMotion));
            }
        }

        public void RemoveKnockup(IUnit target)
        {
            if (targets.ContainsKey(target.ID))
            {
                KnockupInstance instance = targets[target.ID];
                Reset(instance.data);
                targets.Remove(target.ID);
            }
        }

        private void Reset(KnockupData data)
        {
            // move the target back to its original position
            Vector3 currentPosition = data.target.Location;
            data.target.Move(new Vector3(currentPosition.x, currentPosition.y - data.currentHeight, currentPosition.z));
        }

        public void Loop(ITime time)
        {
            if (targets.Count == 0)
            {
                return;
            }

            List<string> toRemove = new List<string>();

            foreach (KnockupInstance instance in targets.Values)
            {
                // Apply the knockup motion to the data
                instance.knockupMotion.Apply(time, instance.data);
                // If the knockup is complete, add to removal list
                if (!instance.data.active)
                {
                    toRemove.Add(instance.data.target.ID);
                }

                // TODO: restrict the target during the knockup
                // TODO: apply visual effects
            }

            foreach (string id in toRemove)
            {
                targets.Remove(id);
            }
        }
    }
}