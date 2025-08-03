using System.Collections.Generic;
using Seyren.System.Units;
using Seyren.Universe;

namespace Seyren.Gameplay
{
    /// <summary>
    /// Shared state for a single pass of the core gameplay loop.
    /// </summary>
    public class GameContext
    {
        public IUnit Player { get; set; }
        public List<IUnit> Enemies { get; set; } = new List<IUnit>();
        public List<Puzzle> Puzzles { get; set; } = new List<Puzzle>();
        // public List<GameEvent> Events { get; set; } = new List<GameEvent>();
        // public List<ICommand> Input { get; set; } = new List<ICommand>();
        // public ITime Time { get; }
        //     public ISpace Space { get; }
        public Universe.Universe Universe { get; }
        public GameContext(Universe.Universe universe)
        {
            // Time = time;
            // Space = space;
            Universe = universe;
        }
    }
}
