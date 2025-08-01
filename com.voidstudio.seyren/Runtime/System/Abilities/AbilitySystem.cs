using System.Collections.Generic;
using Seyren.System.Common;

namespace Seyren.System.Abilities
{
    public interface IAbilitySystem
    {
        void AddAbility(Ability ability);
        void RemoveAbility(Ability ability);
        Ability GetAbility(string name);
        Ability GetAbility(int id);
        List<Ability> GetAbilities();
        Error CastAbility(string id, AbilityData data);
    }

    public class AbilitySystem : IAbilitySystem
    {
        public static Error AbilityNotFound = new Error("ability not found.");
        Dictionary<string, Ability> abilities;
        List<Ability> hotAbilities;
        
        public AbilitySystem()
        {
            abilities = new Dictionary<string, Ability>();
            hotAbilities = new List<Ability>();
        }
        
        public void AddAbility(Ability ability)
        {

            abilities.Add(ability.id, ability);
            hotAbilities.Add(ability);
        }

        public Error CastAbility(string id, AbilityData data)
        {
            if (!abilities.ContainsKey(id.ToString()))
            {
                return AbilityNotFound;
            }

            Ability ability = abilities[id.ToString()];
            return ability.Cast(data);
        }

        public List<Ability> GetAbilities()
        {
            return hotAbilities;
        }

        public Ability GetAbility(string name)
        {
            if (abilities.ContainsKey(name))
            {
                return abilities[name];
            }
            return null;
        }

        public Ability GetAbility(int id)
        {
            return hotAbilities[id];
        }

        public void RemoveAbility(Ability ability)
        {
            abilities.Remove(ability.id);
            hotAbilities.Remove(ability);
        }

    }
}