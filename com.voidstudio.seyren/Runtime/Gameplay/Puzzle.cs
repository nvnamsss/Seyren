namespace Seyren.Gameplay
{
    public class Puzzle
    {
        public bool IsSolved { get; set; }

        public void Solve()
        {
            IsSolved = true;
            // Puzzle solve logic
        }

        public void Reset()
        {
            IsSolved = false;
            // Puzzle reset logic
        }
    }
}
