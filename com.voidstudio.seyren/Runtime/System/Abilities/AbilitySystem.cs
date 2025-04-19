using System.Collections.Generic;
using Seyren.System.Units;
using Seyren.Universe;

namespace Seyren.System.Abilities
{
    public interface IAbilitySystem
    {
        void AddAbility(Ability ability);
        void RemoveAbility(Ability ability);
        Ability GetAbility(string name);
        Ability GetAbility(int id);
        List<Ability> GetAbilities();
        bool CastAbility(string id, AbilityData data);
    }

    public class AbilitySystem : IAbilitySystem
    {
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

        public bool CastAbility(string id, AbilityData data)
        {
            if (!abilities.ContainsKey(id.ToString()))
            {
                return false;
            }

            Ability ability = abilities[id.ToString()];
            ability.Cast(data);

            return true;
        }

        public List<Ability> GetAbilities()
        {
            return hotAbilities;
        }

        public Ability GetAbility(string name)
        {
            throw new global::System.NotImplementedException();
        }

        public Ability GetAbility(int id)
        {
            throw new global::System.NotImplementedException();
        }

        public void RemoveAbility(Ability ability)
        {
            throw new global::System.NotImplementedException();
        }

    }
}