﻿using Base2D.System.UnitSystem.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.AbilitySystem
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

        public ToggleAbility(Unit caster) : base(caster, 0, 0, 1)
        {
            _isOn = true;
        }

        public override bool Cast()
        {
            if (!Condition())
            {
                return false;
            }

            IsOn = !IsOn;
            Caster.StartCoroutine(Casted(TimeDelay, BaseCoolDown));
            return true;
        }

        protected virtual IEnumerator Casted(float timeDelay, float cooldown)
        {
            DoCastAbility();
            TimeCoolDownLeft = cooldown - timeDelay;

            while (TimeCoolDownLeft >= 0)
            {
                yield return new WaitForSeconds(timeDelay);
                TimeCoolDownLeft -= timeDelay;
            }
            yield break;
        }
    }
}