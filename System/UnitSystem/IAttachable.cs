using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Base2D.System.UnitSystem
{
    public interface IAttachable
    {
        IAttachable AttactedObject { get; }
        List<IAttachable> Attachments { get; set; }
        AttachStatus Status { get; set; }
        Vector2 RelatedV2 { get; set; }
        Quaternion Rotation { get; set; }
        float ZIndex { get; set; }

        Action Action { get; set; }

        void Update();
    }
}
