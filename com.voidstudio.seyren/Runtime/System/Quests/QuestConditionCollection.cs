using Seyren.System.Common;
using System.Collections;
using System.Collections.Generic;

namespace Seyren.System.Quests
{
    public class QuestConditionCollection : IQuestCondition, IEnumerable
    {
        public event GameEventHandler<IQuestCondition> Completed;
        public bool Active { get; set; }
        public int CurrentProgress => _currentProgress;
        public int MaxProgress => Conditions.Count;

        private List<QuestCondition> Conditions;
        private int _currentProgress;
        public QuestConditionCollection()
        {
            Active = true;
            _currentProgress = 0;
            Conditions = new List<QuestCondition>();
        }

        public void Add(QuestCondition condition)
        {
            Conditions.Add(condition);
            condition.Completed += ProgressComplete;
        }

        protected void ProgressComplete(IQuestCondition s)
        {
            if (!Active)
            {
                return;
            }

            _currentProgress += 1;
            if (_currentProgress == MaxProgress)
            {
                Completed?.Invoke(this);
                s.Completed -= ProgressComplete;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return Conditions.GetEnumerator();
        }
    }
}
