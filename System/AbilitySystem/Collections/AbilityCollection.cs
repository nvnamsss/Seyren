using Base2D.System.AbilitySystem;
using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.AbilitySystem
{
    /// <summary>
    /// Represents set of abilities
    /// </summary>
    /// 
    [Serializable]
    public class AbilityCollection : IEnumerable
    {
        public Unit owner;
        public int Count => abilities.Count;
        private Dictionary<int, Ability> abilities;
        public AbilityCollection()
        {
            abilities = new Dictionary<int, Ability>();
        }
#if UNITY_EDITOR
        /// <summary>
        /// editor only, using to indicate count of ability is added
        /// </summary>
        [SerializeField]
        public int count;
#endif
        /// <summary>
        /// Initialize with a collection, all abilites in param will be copied
        /// </summary>
        /// <param name="collection"></param>
        public AbilityCollection(AbilityCollection collection)
        {
            Dictionary<int, Ability> sample = collection.GetEnumerator() as Dictionary<int, Ability>;
            abilities = new Dictionary<int, Ability>(sample);
        }
        public IEnumerator GetEnumerator()
        {
            return abilities.GetEnumerator();
        }

        /// <summary>
        /// Get ability in collection with id return
        /// <see cref="Ability.DoNothing"/>
        /// if ability is not contained
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ability if contained otherwise return <see cref="Ability.DoNothing"></see></returns>
        public Ability this[int id]
        {
            get
            {
                if (abilities.ContainsKey(id))
                {
                    return abilities[id];
                }

                return Ability.DoNothing;
            }
        }

        /// <summary>
        /// Add an ability to collection
        /// </summary>
        /// <param name="ability"></param>
        /// <returns> true if ability is added either false</returns>
        public bool Add(Ability ability)
        {
            var props = ability.GetType().GetFields();
            int id = -1;
            for (int loop = 0; loop < props.Length; loop++)
            {
                if (props[loop].Name == "Id" && props[loop].FieldType == typeof(int))
                {
                    id = (int)props[loop].GetValue(null);
                }
            }
#if UNITY_EDITOR
            if (id == -1)
            {
                Debug.LogError(ability.GetType().Name + ": cannot found Id. Ensure that Id of ability is declare as static int field");
            }
#endif
            if (abilities.ContainsKey(id))
            {
                return false;
            }

            abilities.Add(id, ability);
            abilities[id].UnlockAbility();
            return true;
        }

        /// <summary>
        /// Remove ability out of collection
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool Remove(Ability ability)
        {
            var props = ability.GetType().GetFields();
            int id = -1;
            for (int loop = 0; loop < props.Length; loop++)
            {
                if (props[loop].Name == "Id" && props[loop].FieldType == typeof(int))
                {
                    id = (int)props[loop].GetValue(null);
                }
            }

            bool removed = abilities.Remove(id);
            if (!removed)
            {
#if UNITY_EDITOR
                Debug.LogWarning(ability.GetType().Name + " is try to remove but it not exist");
#endif
                return false;
            }
            
            abilities[id].UnlockAbility();
            abilities[id].Active = false;
            return true;
        }

    }
}
