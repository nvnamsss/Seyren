using System;
using System.Reflection;
using Base2D.System.Generic;
using UnityEngine;

namespace Base2D.System.QuestSystem
{
    /// <summary>
    /// Represents a condition for a quest, manage progress
    /// </summary>
    public class QuestCondition : IQuestCondition
    {
        public event GameEventHandler<IQuestCondition> Completed;
        public bool Active { get; set; }
        public bool IsCompleted => CurrentProgress == MaxProgress;
        public int MaxProgress { get; }
        public int CurrentProgress => _currentProgress;
        protected int _currentProgress;
        private object registerObject;
        private string registerEvent;
        private Delegate registerHandler;

        /// <summary>
        /// Construct
        /// <see cref="QuestCondition"/>
        /// with a progress must to be reached.
        /// </summary>
        /// <param name="maxProgress">must positive</param>
        public QuestCondition(int maxProgress)
        {
#if UNITY_EDITOR
            if (maxProgress < 0) Debug.LogWarning("QuestCondition: progress is negative");
#endif
            maxProgress = maxProgress < 0 ? 1 : maxProgress;
            MaxProgress = maxProgress;
            _currentProgress = 0;
        }

        /// <summary>
        /// Register an event of an object.
        /// <see cref="QuestCondition.ProcessIncrease"/>
        /// will triggering if attach event is trigger <br></br>
        /// Register only work with event implemented from 
        /// <see cref="GameEventHandler{TSender}"/>
        /// and
        /// <see cref="GameEventHandler{TSender, TEvent}"/>
        /// event
        /// </summary>
        /// <param name="o">event's owner</param>
        /// <param name="eventName">attach event</param>
        public void Register(object o, string eventName)
        {
            EventInfo e = o.GetType().GetEvent(eventName);
            if (registerEvent != null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("QuestCondition: " + "event " + eventName + " of " + o + " is existed.");
#endif
                return;
            }

            if (e == null)
            {
#if UNITY_EDITOR
                Debug.LogError("QuestCondition: " + "event " + eventName + " of " + o + " is invalid. Must be an existed event which is implemented from GameEventHandler");
#endif
                return;
            }

            ParameterInfo[] parameters = e.EventHandlerType.GetMethod("Invoke").GetParameters();
            Type[] tParams = new Type[parameters.Length];

            for (int loop = 0; loop < parameters.Length; loop++)
            {
                tParams[loop] = parameters[loop].ParameterType;
            }

            Type actionT = typeof(GameEventHandler<>).MakeGenericType(tParams);
            if (e.EventHandlerType != actionT)
            {
#if UNITY_EDITOR
                Debug.LogError("QuestCondition: " + "event " + eventName + " of " + o + " is not suitable to register." + eventName +
                    " must be implemented from GameEventHandler");
#endif
                return;
            }

            MethodInfo[] methodInfos = this.GetType().GetMethods();
            MethodInfo methodInfo = null;
            for (int loop = 0; loop < methodInfos.Length; loop++)
            {
                if (methodInfos[loop].Name == "ProcessIncrease" && methodInfos[loop].GetGenericArguments().Length == tParams.Length)
                {
                    methodInfo = methodInfos[loop].MakeGenericMethod(tParams);
                }
            }
#if UNITY_EDITOR
            if (methodInfo == null)
            {
                Debug.LogError("Missing handler when registering for QuestCondition");
            }
#endif
            registerObject = o;
            registerEvent = eventName;
            registerHandler = Delegate.CreateDelegate(actionT, this, methodInfo);
            e.AddEventHandler(o, registerHandler);
        }


        protected void Unregister()
        {
            EventInfo e = registerObject.GetType().GetEvent(registerEvent);
            e.RemoveEventHandler(registerObject, registerHandler);
        }

        /// <summary>
        /// Determine method to increase progress. Suitable for
        /// <see cref="GameEventHandler{TSender, TEvent}"/>
        /// event
        /// </summary>
        /// <typeparam name="TSender"></typeparam>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public void ProcessIncrease<TSender, TEvent>(TSender s, TEvent e)
        {
            ProcessIncrease();
        }

        /// <summary>
        /// Determine method to increase progress. Suitable for
        /// <see cref="GameEventHandler{TSender}"/>
        /// event
        /// </summary>
        /// <typeparam name="TSender"></typeparam>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public void ProcessIncrease<TSender>(TSender s)
        {
            ProcessIncrease();
        }

        private void ProcessIncrease()
        {
            if (!Active)
            {
                return;
            }

            _currentProgress += 1;
            if (_currentProgress == MaxProgress)
            {
                Unregister();
                Completed?.Invoke(this);
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
