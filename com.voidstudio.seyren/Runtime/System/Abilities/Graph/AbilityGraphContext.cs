using System;
using System.Collections.Generic;
using Seyren.Universe;
using Seyren.Visual;
using UnityEngine;
using Seyren.System.Units;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Runtime data shared across all nodes in a single ability graph instance.
    /// Extends AbilityContext with per-instance state: variables, spawned entities,
    /// and an event bus scoped to this cast.
    /// </summary>
    public class AbilityGraphContext : AbilityContext
    {
        /// <summary>
        /// The universe that owns this ability instance.
        /// Use to access DamageEngine, Space, AbilitySystem, etc.
        /// </summary>
        public Universe.Universe Universe { get; }

        /// <summary>
        /// Arbitrary key/value store for inter-node communication within this instance.
        /// </summary>
        public Dictionary<string, object> Variables { get; } = new Dictionary<string, object>();

        /// <summary>
        /// All entities (projectiles, VFX, units) spawned by nodes in this instance.
        /// </summary>
        public List<object> SpawnedEntities { get; } = new List<object>();

        private readonly Dictionary<string, List<Action>> _eventHandlers
            = new Dictionary<string, List<Action>>();

        /// <summary>
        /// Invoked by <see cref="Seyren.System.Abilities.SpawnEntityNode"/> after an entity is
        /// spawned and stored. Arguments are (entityKey, entity). Populated by GraphAbility.
        /// </summary>
        public Action<string, object> OnEntitySpawnedCallback;

        /// <summary>
        /// Invoked when a spawned IProjectile fires its OnCompleted event.
        /// Arguments are (entityKey, entity). Populated by GraphAbility.
        /// </summary>
        public Action<string, object> OnEntityCompletedCallback;

        /// <summary>
        /// Creates a graph context from an existing AbilityContext, copying all base fields.
        /// </summary>
        public AbilityGraphContext(AbilityContext src, Universe.Universe universe)
        {
            Universe = universe;
            level = src.level;
            caster = src.caster;
            target = src.target;
            location = src.location;
            start = src.start;
            end = src.end;
            if (src.visualEffects != null)
            {
                foreach (var kv in src.visualEffects)
                    visualEffects[kv.Key] = kv.Value;
            }
        }

        /// <summary>
        /// Subscribes <paramref name="handler"/> to be called when <paramref name="eventName"/> is fired.
        /// </summary>
        public void SubscribeEvent(string eventName, Action handler)
        {
            if (!_eventHandlers.TryGetValue(eventName, out var list))
            {
                list = new List<Action>();
                _eventHandlers[eventName] = list;
            }
            list.Add(handler);
        }

        /// <summary>
        /// Fires all handlers registered for <paramref name="eventName"/>.
        /// </summary>
        public void FireEvent(string eventName)
        {
            if (!_eventHandlers.TryGetValue(eventName, out var list)) return;
            for (int i = 0; i < list.Count; i++)
                list[i]?.Invoke();
        }

        /// <summary>
        /// Creates a child context that shares all base ability fields
        /// (caster, target, level, etc.) and the same Universe reference,
        /// but owns an empty <see cref="Variables"/> dictionary so the
        /// branch can write keys without polluting sibling branches.
        /// </summary>
        public AbilityGraphContext Fork()
        {
            var fork = new AbilityGraphContext(this, Universe);
            fork.OnEntitySpawnedCallback   = OnEntitySpawnedCallback;
            fork.OnEntityCompletedCallback = OnEntityCompletedCallback;
            return fork;
        }
    }
}
