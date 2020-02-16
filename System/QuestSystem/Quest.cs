using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.QuestSystem
{
    /// <summary>
    /// Represents a quest in game
    /// </summary>
    public class Quest
    {
        public delegate void QuestStateHandler(Quest sender);
        public event QuestStateHandler Completed;
        public event QuestStateHandler Failed;
        /// <summary>
        /// Status of Quest
        /// </summary>
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                Condition.Active = _active;
            }
        }
        /// <summary>
        /// Quest's name
        /// </summary>
        public string Name;
        /// <summary>
        /// Quest's content
        /// </summary>
        public string Content;
        /// <summary>
        /// Related Quest need to unlock first to unlock this quest
        /// </summary>
        public IQuestCondition Condition { get; }
        private HierarchyCondition<Quest> QuestRequire;
        private bool _active;
        public Quest(string questName, string content, IQuestCondition condition)
        {
            QuestRequire = new HierarchyCondition<Quest>(this);
            Name = questName;
            Content = content;

            if (condition != null)
            {
                Condition = condition;
                Condition.Completed += ConditionCompleted;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("Quest: " + questName + " is assigned by a null condition");
            }
#endif
            Completed += OnQuestCompleted;
            Active = false;
        }

        /// <summary>
        /// this quest only unlocked when target is unlocked
        /// </summary>
        /// <param name="quest"></param>
        public void AssignToQuest(Quest target)
        {
            QuestRequire.AddCondition(target.QuestRequire);
            QuestRequire.Unlock();
            Active = QuestRequire.Unlocked;
        }

        public void Cancel()
        {
            Failed?.Invoke(this);
            Condition.Completed -= ConditionCompleted;
        }

        private void ConditionCompleted(IQuestCondition s)
        {
            Completed?.Invoke(this);
            Condition.Completed -= ConditionCompleted;
        }

        private void OnQuestCompleted(Quest s)
        {
            QuestRequire.Unlock();
            Completed -= OnQuestCompleted;
        }
    }
}
