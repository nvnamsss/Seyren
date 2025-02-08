﻿using Seyren.System.Generics;

namespace Seyren.System.Quests
{
    public interface IQuestCondition
    {
        event GameEventHandler<IQuestCondition> Completed;
        bool Active { get; set; }
        int CurrentProgress { get; }
        int MaxProgress { get; }
    }
}
