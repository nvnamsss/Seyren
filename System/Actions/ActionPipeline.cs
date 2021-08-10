using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Generics;

namespace Seyren.System.Actions
{
    public class ActionPipeline : IAction
    {
        public ActionConditionHandler RunCondition => throw new NotImplementedException();

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;

        IEnumerable<IThing> things;
        bool broke;
        public ActionPipeline(IEnumerable<IThing> things) {
            this.things = things;
        }

        public bool Break()
        {
            broke = true;
            return broke;
        }

        public Error Constraint(IAction action)
        {
            return null;
        }

        public IEnumerable<IThing> Do(params object[] obj)
        {
            foreach (IThing thing in things)
            {
                if (!broke) break;
                yield return thing;
            }
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }
    }
}
