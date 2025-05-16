using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seyren.System.Common;
using Seyren.Universe;

namespace Seyren.System.Actions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current">current action is doing</param>
    /// <returns></returns>
    public delegate bool ActionConditionHandler(IAction current);
    /*
    Action has constraint and do an action in time.
    during run time, if action is broken or violate the constraint then nothing happen.
    action may contains running time indicate the time an action needed to complete,
    */
    public interface IAction : ILoop
    {
        event GameEventHandler<IAction> OnStarted;
        event GameEventHandler<IAction> OnStopped;
        event GameEventHandler<IAction> OnCompleted;
        /// <summary>
        /// Condition to play action
        /// </summary>
        Error RunCondition();
        /// <summary>
        /// Start action
        /// </summary>
        // IEnumerable<IThing> Do(params object[] obj);
        string ID { get; }
        int ActionType { get; }
        bool IsCompleted { get; }
        bool IsStarted { get; }
        void Start();
        void Stop();
    }

}
