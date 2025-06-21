using System.Collections;
using System.Collections.Generic;
using Seyren.System.Actions;
using Seyren.System.Damages;
using Seyren.System.Common;
using UnityEngine;

namespace Seyren.System.Units
{
    public interface IAnimable
    {
        float AnimationSpeed { get; set; }
        float TurnSpeed { get; set; }
    }
    public interface ICoordinate
    {
        Vector3 Location { get; }
        Vector3 Size { get; set; }
        Quaternion Rotation { get; }
        Vector3 Forward { get; }
    }
    
    public interface IAttachable
    {
        IAttachable AttactedObject { get; }
        List<IAttachable> Attachments { get; set; }
        Quaternion Rotation { get; set; }
        float ZIndex { get; set; }

        void Update();
    }


    public interface IObject : ICoordinate
    {
        string ID { get; }
        ObjectStatus ObjectStatus { get; set; }
        bool IsActive { get; }
    }
}

