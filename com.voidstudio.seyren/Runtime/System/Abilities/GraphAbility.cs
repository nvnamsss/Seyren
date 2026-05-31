using System.Collections.Generic;
using Seyren.System.Common;
using Seyren.System.Units;
using Seyren.Universe;
using UnityEngine;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Base class for abilities defined as runtime execution graphs.
    /// Subclasses implement BuildGraph() to describe the ability as an IAbilityNode tree.
    /// Cast() wraps AbilityContext into AbilityGraphContext, applies modifiers, and
    /// creates an AbilityGraphInstance that is driven by TickEffect() each frame.
    /// </summary>
    public abstract class GraphAbility : Ability
    {
        private readonly List<AbilityGraphInstance> _instances = new List<AbilityGraphInstance>();
        private readonly List<IAbilityGraphModifier> _modifiers = new List<IAbilityGraphModifier>();
        private readonly List<IGraphAbilityVFX> _vfxChain = new List<IGraphAbilityVFX>();

        protected GraphAbility(Universe.Universe universe) : base(universe)
        {
        }

        /// <summary>
        /// Adds a modifier that is applied at cast time before the graph instance is created.
        /// If the modifier also implements <see cref="IGraphAbilityVFXProvider"/>, its companion
        /// VFX handler is appended to the VFX chain immediately (registration time, not per-cast).
        /// </summary>
        public void AddModifier(IAbilityGraphModifier modifier)
        {
            _modifiers.Add(modifier);
            if (modifier is IGraphAbilityVFXProvider vfxProvider)
                AddVFX(vfxProvider.GetVFX());
        }

        /// <summary>
        /// Appends a VFX handler to the chain. Handlers are consulted in registration order;
        /// the first handler that returns a non-null / true result wins for that event.
        /// </summary>
        public void AddVFX(IGraphAbilityVFX vfx)
        {
            _vfxChain.Add(vfx);
        }

        /// <summary>
        /// Returns the first non-null GameObject produced by the VFX chain for the given entity key,
        /// or null if no handler claims it.
        /// </summary>
        protected GameObject ResolveVisual(string key, AbilityGraphContext ctx)
        {
            for (int i = 0; i < _vfxChain.Count; i++)
            {
                GameObject go = _vfxChain[i].CreateVisual(key, ctx);
                if (go != null) return go;
            }
            return null;
        }

        /// <summary>
        /// Walks the VFX chain for OnEntitySpawned; stops at the first handler that returns true.
        /// </summary>
        protected void NotifySpawned(string key, object entity, AbilityGraphContext ctx)
        {
            for (int i = 0; i < _vfxChain.Count; i++)
                if (_vfxChain[i].OnEntitySpawned(key, entity, ctx)) return;
        }

        /// <summary>
        /// Walks the VFX chain for OnEntityCompleted; stops at the first handler that returns true.
        /// </summary>
        protected void NotifyCompleted(string key, object entity, AbilityGraphContext ctx)
        {
            for (int i = 0; i < _vfxChain.Count; i++)
                if (_vfxChain[i].OnEntityCompleted(key, entity, ctx)) return;
        }

        /// <summary>
        /// Constructs the static graph definition for this cast.
        /// Called once per cast; the result may be transformed by modifiers before instantiation.
        /// </summary>
        protected abstract AbilityGraph BuildGraph(AbilityContext context);

        public override (IAbilityInstance instance, Error error) Cast(AbilityContext data)
        {
            Error conditionError = ValidateCondition(data);
            if (conditionError != null)
                return (null, conditionError);

            AbilityGraph graph = BuildGraph(data);

            for (int i = 0; i < _modifiers.Count; i++)
            {
                graph = _modifiers[i].Apply(graph, data);
            }

            var graphContext = new AbilityGraphContext(data, universe);
            graphContext.OnEntitySpawnedCallback  = (key, entity) => NotifySpawned(key, entity, graphContext);
            graphContext.OnEntityCompletedCallback = (key, entity) => NotifyCompleted(key, entity, graphContext);
            var instance = new AbilityGraphInstance(graph, graphContext);
            _instances.Add(instance);

            onCast();

            return (instance, null);
        }

        private Error ValidateCondition(AbilityContext data)
        {
            if (data.target != null)
                return Condition(data.caster, data.target);
            if (data.location.HasValue)
                return Condition(data.caster, data.location.Value);
            return Condition(data.caster);
        }

        protected override void TickEffect(ITime time)
        {
            float dt = time.DeltaTime;
            for (int i = _instances.Count - 1; i >= 0; i--)
            {
                _instances[i].Tick(dt);
                if (!_instances[i].IsActive)
                    _instances.RemoveAt(i);
            }
        }
    }
}
