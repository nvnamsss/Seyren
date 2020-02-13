using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.QuestSystem
{
    public class QuestCondition
    {
        public int MaxProgress { get; }
        public int CurrentProgress { get; }

        public QuestCondition()
        {

        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
