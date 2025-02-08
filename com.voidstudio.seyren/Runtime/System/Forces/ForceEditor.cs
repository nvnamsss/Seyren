using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Seyren.System.Forces
{
    [ExecuteAlways]
    [CreateAssetMenu(fileName = "New Force", menuName = "Force")]
    public class ForceEditor : ScriptableObject, ISerializationCallbackReceiver
    {
        public string forceName = string.Empty;
        public List<ForceEditor> Alliance = new List<ForceEditor>();
        public List<ForceEditor> Enemy = new List<ForceEditor>();
        private Force force;
        private bool triggerSync = true;
        private bool triggerValidate = true;
        public void OnAfterDeserialize()
        {
            if (!triggerSync) return;
            //sync data

            Dictionary<string, ForceEditor> da = new Dictionary<string, ForceEditor>();
            for (int loop = 0; loop < Alliance.Count; loop++)
            {
                if (Alliance[loop] == null) continue;

                if (!da.ContainsKey(Alliance[loop].forceName))
                {
                    da.Add(Alliance[loop].forceName, Alliance[loop]);
                    Alliance[loop].AddAlliance(this, false, true);
                }
            }

            Dictionary<string, ForceEditor> de = new Dictionary<string, ForceEditor>();

            for (int loop = 0; loop < Enemy.Count; loop++)
            {
                if (Enemy[loop] == null) continue;

                if (!de.ContainsKey(Enemy[loop].forceName))
                {
                    de.Add(Enemy[loop].forceName, Enemy[loop]);
                    Enemy[loop].AddEnemy(this, false, true);
                }
            }

            force = Force.CreateForce(forceName);
            //for (int loop = 0; loop < Alliance.Count; loop++)
            //{
            //    force.MakeAlliance(Alliance[loop].force);
            //}

            //for (int loop = 0; loop < Enemy.Count; loop++)
            //{
            //    force.MakeEnemy(Enemy[loop].force);
            //}
        }

        public void OnBeforeSerialize()
        {
            forceName = GetFileName();
            if (!triggerValidate) return;

            //validate data

            if (Alliance.Contains(this))
            {
                RemoveAlliance(this, false, false);
            }

            if (Enemy.Contains(this))
            {
                RemoveEnemy(this, false, false);
            }

            for (int loop = 0; loop < Alliance.Count; loop++)
            {
                if (Alliance[loop] == null) continue;

                if (Enemy.Contains(Alliance[loop]))
                {
                    RemoveEnemy(Alliance[loop], false, false);
                    loop--;
                }
            }

            Dictionary<string, ForceEditor> da = new Dictionary<string, ForceEditor>();
            for (int loop = 0; loop < Alliance.Count; loop++)
            {
                if (Alliance[loop] == null) continue;
                if (!da.ContainsKey(Alliance[loop].forceName))
                {
                    da.Add(Alliance[loop].forceName, Alliance[loop]);
                }
                else
                {
                    RemoveAlliance(Alliance[loop], false, false);
                }
            }
        }

        private string GetFileName()
        {
            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());             

            return Path.GetFileNameWithoutExtension(assetPath);
        }
        private void Awake()
        {
            Debug.Log("AAA");
            
        }

        public void AddAlliance(ForceEditor force, bool trigger, bool validate)
        {
            this.triggerSync = trigger;
            triggerValidate = validate;

            Alliance.Add(force);

            triggerValidate = true;
            this.triggerSync = true;
        }

        public void RemoveAlliance(ForceEditor force, bool sync, bool validate)
        {
            this.triggerSync = sync;
            triggerValidate = validate;

            Alliance.Remove(force);

            triggerValidate = true;
            this.triggerSync = true;
        }
        public void AddEnemy(ForceEditor force, bool trigger, bool validate)
        {
            this.triggerSync = trigger;
            triggerValidate = validate;

            Enemy.Add(force);

            triggerValidate = true;
            this.triggerSync = true;
        }

        public void RemoveEnemy(ForceEditor force, bool trigger, bool validate)
        {
            this.triggerSync = trigger;
            triggerValidate = validate;

            Enemy.Remove(force);

            triggerValidate = true;
            this.triggerSync = true;
        }
    }
}
