using System;

namespace Necromancy.Server.Model
{
    public class Quest
    {
        public int QuestID { get; set; }
        public Byte SoulLevelMission { get; set; }
        public string QuestName { get; set; }
        public int QuestLevel { get; set; }
        public int TimeLimit { get; set; }
        public string QuestGiverName { get; set; }
        public int RewardEXP { get; set; }
        public int RewardGold { get; set; }
        public Int16 NumbersOfItems { get; set; }
        public int ItemsType { get; set; }
        public Quest()
        {

        }
    }

}