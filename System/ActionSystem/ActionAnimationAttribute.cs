using Base2D.System.UnitSystem.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.ActionSystem
{
    public class ActionAnimationAttribute : Attribute
    {
        public ActionAnimationAttribute(string animation)
        {
            Console.WriteLine("a");
            //u.Action.Animator.SetTrigger(animation);
        }
    }
}
