using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Generics;

namespace Seyren.System.Actions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current">current action is doing</param>
    /// <returns></returns>
    public delegate bool ActionConditionHandler(IAction current);
    public interface IAction
    {
        event GameEventHandler<IAction> ActionStart;
        event GameEventHandler<IAction> ActionEnd;
        /// <summary>
        /// Condition to play action
        /// </summary>
        ActionConditionHandler RunCondition { get; }
        ActionType ActionType { get; }
        /// <summary>
        /// Start action
        /// </summary>
        void Invoke();
        /// <summary>
        /// End action
        /// </summary>
        bool Break();
        void Constraint(IAction action);
    }
}
