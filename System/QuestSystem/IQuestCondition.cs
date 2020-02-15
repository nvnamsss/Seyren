using Base2D.System.Generic;

namespace Base2D.System.QuestSystem
{
    public interface IQuestCondition
    {
        event GameEventHandler<IQuestCondition> Completed;
        int CurrentProgress { get; }
        int MaxProgress { get; }
    }
}
