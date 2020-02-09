using Base2D.System.AbilitySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Base2D.System.AbilitySystem
{
    public class AbilityCollection : IEnumerable
    {
        private Dictionary<int, Ability> abilities;
        public AbilityCollection()
        {
            abilities = new Dictionary<int, Ability>();
        }
        public IEnumerator GetEnumerator()
        {
            return abilities.GetEnumerator();
        }
        public Ability this[int id]
        {
            get
            {
                if (abilities.ContainsKey(id))
                {
                    return abilities[id];
                }

                return null;
            }
        }

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

    }
}
