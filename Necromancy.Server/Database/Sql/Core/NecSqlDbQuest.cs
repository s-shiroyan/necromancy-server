using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateQuest =
            "INSERT INTO `QuestRequest` (`QuestID `, `SoulLevelMission `, `QuestName `, `QuestLevel `, `TimeLimit `, `QuestGiverName `, `RewardEXP `, `RewardGold `, `NumbersOfItems `, `ItemsType`) VALUES (@QuestID, @SoulLevelMission, @QuestName, @QuestLevel, @TimeLimit, @QuestGiverName, @RewardEXP, @RewardGold, @NumbersOfItems, @ItemsType);";

        private const string SqlSelectQuestById =
            "SELECT `QuestID `, `SoulLevelMission `, `QuestName `, `QuestLevel `, `TimeLimit `, `QuestGiverName `, `RewardEXP `, `RewardGold `, `NumbersOfItems `, `ItemsType` FROM `QuestRequest` WHERE `QuestID`=@QuestID; ";

        private const string SqlUpdateQuest =
            "UPDATE `QuestRequest` SET `id`=@id, `ItemName`=@ItemName, `ItemType`=@ItemType,  `Physics`=@Physics, `Magic`=@Magic, `EnchantID`=@EnchantID, `Durab`=@Durab, `Hardness`=@Hardness, `MaxDur`=@MaxDur, `Numbers`=@Numbers, `Level`=@Level, `Splevel`=@Splevel, `Weight`=@Weight, `State`=@State WHERE `id`=@id;";

        private const string SqlDeleteQuest =
            "DELETE FROM `QuestRequest` WHERE `QuestID`=@QuestID;";

        public bool InsertQuest(Quest quest)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateQuest, command =>
            {
                AddParameter(command, "@QuestID", quest.QuestID);
                AddParameter(command, "@SoulLevelMission", quest.SoulLevelMission);
                AddParameter(command, "@QuestName", quest.QuestName);
                AddParameter(command, "@QuestLevel", quest.QuestLevel);
                AddParameter(command, "@TimeLimit", quest.TimeLimit);
                AddParameter(command, "@QuestGiverName", quest.QuestGiverName);
                AddParameter(command, "@RewardEXP", quest.RewardEXP);
                AddParameter(command, "@RewardGold", quest.RewardGold);
                AddParameter(command, "@NumbersOfItems", quest.NumbersOfItems);
                AddParameter(command, "@ItemsType", quest.ItemsType);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            quest.QuestID = (int)autoIncrement;
            return true;
        }


        public Quest SelectQuestById(int questId)
        {
            Quest quest = null;
            ExecuteReader(SqlSelectQuestById,
                command => { AddParameter(command, "@QuestID", questId); }, reader =>
                {
                    if (reader.Read())
                    {
                        quest = ReadQuest(reader);
                    }
                });
            return quest;
        }

        public bool UpdateQuest(Quest quest)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateQuest, command =>
            {
                AddParameter(command, "@QuestID", quest.QuestID);
                AddParameter(command, "@SoulLevelMission", quest.SoulLevelMission);
                AddParameter(command, "@QuestName", quest.QuestName);
                AddParameter(command, "@QuestLevel", quest.QuestLevel);
                AddParameter(command, "@TimeLimit", quest.TimeLimit);
                AddParameter(command, "@QuestGiverName", quest.QuestGiverName);
                AddParameter(command, "@RewardEXP", quest.RewardEXP);
                AddParameter(command, "@RewardGold", quest.RewardGold);
                AddParameter(command, "@NumbersOfItems", quest.NumbersOfItems);
                AddParameter(command, "@ItemsType", quest.ItemsType);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteQuest(int questId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteQuest,
                command => { AddParameter(command, "@QuestID", questId); });
            return rowsAffected > NoRowsAffected;
        }

        private Quest ReadQuest(DbDataReader reader)
        {
            Quest quest = new Quest();
            quest.QuestID = GetInt32(reader, "QuestID");
            quest.SoulLevelMission = GetByte(reader, "SoulLevelMission");
            quest.QuestName = GetString(reader, "QuestName");
            quest.QuestLevel = GetInt32(reader, "QuestLevel");
            quest.TimeLimit = GetInt32(reader, "TimeLimit");
            quest.QuestGiverName = GetString(reader, "QuestGiverName");
            quest.RewardEXP = GetInt32(reader, "RewardEXP");
            quest.RewardGold = GetInt32(reader, "RewardGold");
            quest.NumbersOfItems = (short)GetInt32(reader, "NumbersOfItems");
            quest.ItemsType = GetInt32(reader, "ItemsType");
            return quest;
        }
    }
}