using System;

namespace Seyren.System.Actions.DelayAction
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
