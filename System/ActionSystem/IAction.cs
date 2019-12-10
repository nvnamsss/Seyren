using Base2D.System.ActionSystem.DelayAction;
using Base2D.System.ActionSystem.BreakAtion;
using UnityEngine;

namespace Base2D.System.ActionSystem
{
    public abstract class IAction : MonoBehaviour
    {
        public ActionType Type { get; set; }


        abstract public bool BreakAction(BreakType breakType);
        abstract public bool DelayAction(DelayInfo delayInfo);
    }
}
