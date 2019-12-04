using Crom.System.ActionSystem.DelayAction;
using Crom.System.ActionSystem.BreakAtion;
using UnityEngine;

namespace Crom.System.ActionSystem
{
    public abstract class IAction : MonoBehaviour
    {
        public ActionType Type { get; set; }


        abstract public bool BreakAction(BreakType breakType);
        abstract public bool DelayAction(DelayInfo delayInfo);
    }
}
