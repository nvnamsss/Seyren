using Seyren.System.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Seyren.System.Abilities
{
    public abstract class ToggleAbility : Ability
    {
        public delegate void ToggleHandler(ToggleAbility sender);
        public event ToggleHandler On;
        public event ToggleHandler Off;
        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;
                if (_isOn)
                {
                    On?.Invoke(this);
                }
                else
                {
                    Off?.Invoke(this);
                }
            }
        }

        protected bool _isOn;
        protected abstract void DoCastAbility();

        public ToggleAbility(int level) : base(level)
        {
            _isOn = true;
        }
    }
}