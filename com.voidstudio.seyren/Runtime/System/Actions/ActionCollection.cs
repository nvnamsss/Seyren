using System.Collections.Generic;
using System.Collections.Concurrent;
using Seyren.System.Generics;
using UnityEngine;

namespace Seyren.System.Actions
{
    /*
    manage a list of action, allow efficiently add and remove actions
    actions in a list may affect each other.
    */
    public class ActionCollection
    {
        HashSet<IAction> actions;
        // List<IAction> actions;
        public ActionCollection()
        {
            actions = new HashSet<IAction>();
            // actions = new List<IAction>();
        }

        public Error Run(IAction action)
        {
            int actionType = action.ActionType;
            List<IAction> affect = new List<IAction>();
            Error err = action.RunCondition();
            if (err != null) {
                return err;
            }
            foreach (var item in actions)
            {
                err = action.Constraint(item);
                if (err != null) {
                    return err;
                }

                if (item.IsAffectedBy(actionType)) {
                    Debug.Log("Break");
                    affect.Add(item);
                }

            }

            for (int i = 0; i < affect.Count; i++)
            {
                affect[i].Break();
                actions.Remove(affect[i]);
            }

            action.Run();
            actions.Add(action);
            return null;
        }

    }
}