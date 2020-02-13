using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.System.QuestSystem
{
    /// <summary>
    /// Represents a quest in game
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// Quest's name
        /// </summary>
        public string Name;
        /// <summary>
        /// Quest's content
        /// </summary>
        public string Content;
        /// <summary>
        /// Quest's reward
        /// </summary>
        public IQuestReward Reward;
    }
}
