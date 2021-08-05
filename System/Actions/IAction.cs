﻿using System;
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
        /// <summary>
        /// Start action
        /// </summary>
        IEnumerable<IThing> Do(params object[] obj);
        bool Break();
        Error Constraint(IAction action);
    }

    public interface IThing {
        void Do(params object[] obj);
    }
}