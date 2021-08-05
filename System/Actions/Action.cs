using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Generics;

namespace Seyren.System.Actions
{
    public class Action : IAction
    {
        IThing thing;
        public Action() {

        }

        public ActionConditionHandler RunCondition => throw new NotImplementedException();

        public event GameEventHandler<IAction> ActionStart;
        public event GameEventHandler<IAction> ActionEnd;

        public bool Break()
        {
            throw new NotImplementedException();
        }

        public Error Constraint(IAction action)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IThing> Do(params object[] obj)
        {
            yield return thing;
        }

        public void Invoke()
        {
            throw new NotImplementedException();
        }

        
    }
}
