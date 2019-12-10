using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Crom.System.UnitSystem
{
    class UnitAttaction : IAttachable
    {
        public IAttachable AttactedObject { get; set; }

        public List<IAttachable> Attachments { get; set; }
        public AttachStatus Status { get; set; }
        public Vector2 RelatedV2 { get; set; }
        public Quaternion Rotation { get; set; }
        public float ZIndex { get; set; }
        public Action Action { get; set; }

        public void Update()
        {
        }
    }
}
