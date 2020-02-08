using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ActionSystem
{
    public delegate void ActionHandler(IAction sender);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current">current action is doing</param>
    /// <returns></returns>
    public delegate bool ActionConditionHandler(IAction current);
    public interface IAction
    {
        event ActionHandler ActionStart;
        event ActionHandler ActionEnd;
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
        void Revoke();  
    }
}
