using Base2D.System.QuestSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base2D.Example.Quests
{
    /// <summary>
    /// Example for a Quest that lead beginner familiarize with the game
    /// This quest is a list of seven step to the moon
    /// </summary>
    public class BeginnerPath
    {
        public Quest WhoYouAre;
        private string WhoYouAreName = "Who You Are?";
        private string WhoYouAreContent = "All hero come from zero, now you just a child but you will growing up" +
            "You can start slow but you will finish fast, just do it";
        public BeginnerPath()
        {
            WhoYouAre = new Quest(WhoYouAreName, WhoYouAreContent);
        }
    }
}
