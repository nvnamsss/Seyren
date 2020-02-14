using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.QuestSystem
{
    public class QuestConditionCollection : IEnumerable
    {
        public int Count => Conditions.Count;
        private List<QuestCondition> Conditions;

        public QuestConditionCollection()
        {
            Conditions = new List<QuestCondition>();
        }

        public void Add(QuestCondition condition)
        {
            Conditions.Add(condition);
        }

        public IEnumerator GetEnumerator()
        {
            return Conditions.GetEnumerator();
        }
    }
}
