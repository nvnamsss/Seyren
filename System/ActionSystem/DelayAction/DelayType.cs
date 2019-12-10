using System;

namespace Base2D.System.ActionSystem.DelayAction
{
    [Flags]
    public enum DelayType
    {
        SampleType1 = 1,
        SampleType2 = 2,
        SampleType3 = 4,
        All = ~0,
    }
}
