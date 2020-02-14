using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Quest's name
        /// </summary>
        public string Name;
        /// <summary>
        /// Quest's content
        /// </summary>
        public string Content;
        public HierarchyCondition<Quest> PreQuestRequire;
        public QuestCondition Condition;
        public QuestConditionCollection Conditions;
        public Quest(string questName, string content)
        {
            PreQuestRequire = new HierarchyCondition<Quest>(this);
            Name = questName;
            Content = content;
            Conditions = new QuestConditionCollection();
        }

        public void Cancel()
        {
            Failed?.Invoke(this);
        }
    }
}
