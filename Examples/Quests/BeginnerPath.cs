using Seyren.Example.Abilities;
using Seyren.System.Quests;
using Seyren.System.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Seyren.Example.Quests
{
    /// <summary>
    /// Example for a Quest that lead beginner familiarize with the game
    /// This quest is a list of seven step to the moon
    /// </summary>
    public class BeginnerPath
    {
        public Unit receiver;
        public Quest WhoYouAre;
        private string WhoYouAreName = "Who You Are?";
        private string WhoYouAreContent = "All hero come from zero, now you just a child but you will growing up" +
            "You can start slow but you will finish fast, just do it";

        public Quest CantTouchMe;
        private QuestCondition CantTouchMeCondition = new QuestCondition(3);
        private string CantTouchMeName = "Can't touch me";
        private string CantTouchMeContent = "Do not need to face opponent's attack or you have your plan, try to get rid of them";
        public Quest FirstStepToTheWorld;
        private QuestCondition FirstStepToTheWorldCondition = new QuestCondition(10);
        private string FirstStepToTheWorldName = "First step to the world";
        private string FirstStepToTheWorldContent = "Skeletons are around there, do you fear to face with them?" +
            "If not, go and prove!";
        public BeginnerPath()
        {
            receiver.Abilites.Add(Dash.Id, new Dash(receiver));
            receiver.Abilites[Dash.Id].UnlockAbility();
            CantTouchMeCondition.Register(receiver.Abilites[Dash.Id], "Casted");
            CantTouchMe = new Quest(CantTouchMeName, CantTouchMeContent, CantTouchMeCondition);
            CantTouchMe.Completed += (s) =>
            {
                Debug.Log(s.Name + " completed");
            };
        
            FirstStepToTheWorldCondition.Register<Unit, Unit>(receiver, "Killing", (killing, killed) =>
            {
                if (killed.name.Contains("Skeleton"))
                {
                    return true;
                }

                return false;
            });
            FirstStepToTheWorld = new Quest(FirstStepToTheWorldName, FirstStepToTheWorldContent, FirstStepToTheWorldCondition);
            FirstStepToTheWorld.AssignToQuest(CantTouchMe); //First step to the world can only do when Cant touch me have done

        }
    }
}
