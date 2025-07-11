namespace Seyren.System.Damages.Resistances
{
    
    public class Evasion :  IResistance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Chance { get; set; }
        public StackType StackType { get; set; }

        public Evasion(float chance, StackType stackType = StackType.None)
        {
            Chance = chance;
            StackType = stackType;
        }

        public void Apply(Damage damage)
        {
            float chance = UnityEngine.Random.Range(0, 100);
            if (chance < Chance)
            {
                damage.Evaded = true;
            }
        }
    }
}
