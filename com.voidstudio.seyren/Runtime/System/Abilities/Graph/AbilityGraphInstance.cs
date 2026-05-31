using Seyren.System.Units;

namespace Seyren.System.Abilities
{
    /// <summary>
    /// Runtime execution of a single AbilityGraph cast.
    /// Created by GraphAbility.Cast(); ticked every frame by GraphAbility.TickEffect().
    /// </summary>
    public class AbilityGraphInstance : IAbilityInstance
    {
        private readonly AbilityGraph _graph;
        private readonly AbilityGraphContext _context;
        private float _aliveTime;
        private bool _isActive;

        public IUnit Caster => _context.caster;
        public float AliveTime => _aliveTime;
        public bool IsActive => _isActive;

        public AbilityGraphInstance(AbilityGraph graph, AbilityGraphContext context)
        {
            _graph = graph;
            _context = context;
            _isActive = true;
        }

        public void Tick(float deltaTime)
        {
            if (!_isActive) return;

            _aliveTime += deltaTime;
            NodeState result = _graph.EntryNode.Tick(_context, deltaTime);
            if (result != NodeState.Running)
            {
                _isActive = false;
            }
        }

        public void Cancel()
        {
            _isActive = false;
            _context.FireEvent("cancel");
        }
    }
}
