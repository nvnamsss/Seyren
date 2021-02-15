using System;
using System.Collections.Generic;
using UnityEngine;

namespace Seyren.System.Forces
{
    [Serializable]
    public class ForceList
    {
        [SerializeField]
        private List<Force> forces;
        public ForceList()
        {
            forces = new List<Force>();
        }

        public void Add(Force force)
        {

        }
    }
}
