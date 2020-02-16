using Base2D.System.Generic;

namespace Base2D.System.QuestSystem
{
    public interface IQuestCondition
    {
        event GameEventHandler<IQuestCondition> Completed;
        bool Active { get; set; }
        int CurrentProgress { get; }
        int MaxProgress { get; }
    }
}
