using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.QuestSystem
{
    public class QuestCondition
    {
        public delegate void QuestConditionHandler(QuestCondition sender);
        public event QuestConditionHandler Completed;
        public bool IsCompleted => CurrentProgress == MaxProgress;
        public int MaxProgress { get; }
        public int CurrentProgress { get; }

        public QuestCondition()
        {

        }

        public void Register<TEvent>(TEvent e) where TEvent : MulticastDelegate
        {
            /*We can using MulticastDelegate to handler add and remove*/
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
