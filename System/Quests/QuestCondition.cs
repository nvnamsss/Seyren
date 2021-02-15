using System;
using System.Reflection;
using Seyren.System.Generics;
using UnityEngine;

namespace Seyren.System.Quests
{
    /// <summary>
    /// Represents a condition for a quest, manage progress
    /// </summary>
    public class QuestCondition : IQuestCondition
    {
        public delegate bool MatchHandler<TSender>(TSender e);
        public delegate bool MatchHandler<TSender, TEvent>(TSender s, TEvent e);
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
            Active = true;
#if UNITY_EDITOR
            if (maxProgress < 0) Debug.LogWarning("QuestCondition: progress is negative");
#endif
            maxProgress = maxProgress < 1 ? 1 : maxProgress;
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

        /// <summary>
        /// Register an event of an object.
        /// <see cref="QuestCondition.ProcessIncrease"/>
        /// will triggering if attach event
        /// <see cref="GameEventHandler{TSender, TEvent}"/>
        /// which is matched by
        /// <see cref="MatchHandler{TSender, TEvent}"/>
        /// match is trigger <br></br>
        /// </summary>
        /// <typeparam name="TSender">Sender type</typeparam>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="o">sender instace</param>
        /// <param name="eventName">event name</param>
        /// <param name="match">condition for event</param>
        public void Register<TSender, TEvent>(TSender o, string eventName, MatchHandler<TSender, TEvent> match)
        {
            GameEventHandler<TSender, TEvent> gameEvent = (s, e) =>
            {
                if (match(s, e))
                {
                    ProcessIncrease(s, e);
                }
            };

            Register(o, eventName, gameEvent);
        }

        /// <summary>
        /// Register an event of an object.
        /// <see cref="QuestCondition.ProcessIncrease"/>
        /// will triggering if attach event
        /// <see cref="GameEventHandler{TSender}"/>
        /// which is matched by
        /// <see cref="MatchHandler{TSender}"/>
        /// match is trigger <br></br>
        /// </summary>
        /// <typeparam name="TSender">Sender type</typeparam>
        /// <param name="o">sender instace</param>
        /// <param name="eventName">event name</param>
        /// <param name="match">condition for event</param>
        public void Register<TSender>(TSender o, string eventName, MatchHandler<TSender> match)
        {
            GameEventHandler<TSender> gameEvent = (s) =>
            {
                if (match(s))
                {
                    ProcessIncrease(s);
                }
            };

            Register(o, eventName, gameEvent);
        }

        private void Register(object o, string eventName, Delegate handler)
        {
            EventInfo eventInfo = o.GetType().GetEvent(eventName);
            if (registerEvent != null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("QuestCondition: " + "event " + eventName + " of " + o + " is existed.");
#endif
                return;
            }

            if (eventInfo == null)
            {
#if UNITY_EDITOR
                Debug.LogError("QuestCondition: " + "event " + eventName + " of " + o + " is invalid. Must be an existed event which is implemented from GameEventHandler");
#endif
                return;
            }

            registerObject = o;
            registerEvent = eventName;
            registerHandler = handler;
            eventInfo.AddEventHandler(o, handler);
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
