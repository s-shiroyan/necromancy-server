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
            "INSERT INTO `QuestRequest` (`quest_id `, `soul_level_mission `, `quest_name `, `quest_level `, `time_limit `, `quest_giver_name `, `reward_exp `, `reward_gold `, `numbers_of_items `, `items_type`) VALUES (@quest_id, @soul_level_mission, @quest_name, @quest_level, @time_limit, @quest_giver_name, @reward_exp, @reward_gold, @numbers_of_items, @items_type);";

        private const string SqlSelectQuestById =
            "SELECT `quest_id `, `soul_level_mission `, `quest_name `, `quest_level `, `time_limit `, `quest_giver_name `, `reward_exp `, `reward_gold `, `numbers_of_items `, `items_type` FROM `QuestRequest` WHERE `quest_id`=@quest_id; ";

        private const string SqlUpdateQuest =
            "UPDATE `QuestRequest` SET `id`=@id, `item_name`=@item_name, `items_type`=@items_type,  `physics`=@physics, `magic`=@magic, `enchant_id`=@enchant_id, `durab`=@durab, `hardness`=@hardness, `max_dur`=@max_dur, `numbers`=@numbers, `level`=@level, `sp_level`=@sp_level, `weight`=@weight, `state`=@state WHERE `id`=@id;";

        private const string SqlDeleteQuest =
            "DELETE FROM `QuestRequest` WHERE `quest_id`=@quest_id;";

        public bool InsertQuest(Quest quest)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateQuest, command =>
            {
                AddParameter(command, "@quest_id", quest.QuestID);
                AddParameter(command, "@soul_level_mission", quest.SoulLevelMission);
                AddParameter(command, "@quest_name", quest.QuestName);
                AddParameter(command, "@quest_level", quest.QuestLevel);
                AddParameter(command, "@time_limit", quest.TimeLimit);
                AddParameter(command, "@quest_giver_name", quest.QuestGiverName);
                AddParameter(command, "@reward_exp", quest.RewardEXP);
                AddParameter(command, "@reward_gold", quest.RewardGold);
                AddParameter(command, "@numbers_of_items", quest.NumbersOfItems);
                AddParameter(command, "@items_type", quest.ItemsType);
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
                command => { AddParameter(command, "@quest_id", questId); }, reader =>
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
                AddParameter(command, "@quest_id", quest.QuestID);
                AddParameter(command, "@soul_level_mission", quest.SoulLevelMission);
                AddParameter(command, "@quest_name", quest.QuestName);
                AddParameter(command, "@quest_level", quest.QuestLevel);
                AddParameter(command, "@time_limit", quest.TimeLimit);
                AddParameter(command, "@quest_giver_name", quest.QuestGiverName);
                AddParameter(command, "@reward_exp", quest.RewardEXP);
                AddParameter(command, "@reward_gold", quest.RewardGold);
                AddParameter(command, "@numbers_of_items", quest.NumbersOfItems);
                AddParameter(command, "@items_type", quest.ItemsType);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteQuest(int questId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteQuest,
                command => { AddParameter(command, "@quest_id", questId); });
            return rowsAffected > NoRowsAffected;
        }

        private Quest ReadQuest(DbDataReader reader)
        {
            Quest quest = new Quest();
            quest.QuestID = GetInt32(reader, "quest_id");
            quest.SoulLevelMission = GetByte(reader, "soul_level_mission");
            quest.QuestName = GetString(reader, "quest_name");
            quest.QuestLevel = GetInt32(reader, "quest_level");
            quest.TimeLimit = GetInt32(reader, "time_limit");
            quest.QuestGiverName = GetString(reader, "quest_giver_name");
            quest.RewardEXP = GetInt32(reader, "reward_exp");
            quest.RewardGold = GetInt32(reader, "reward_gold");
            quest.NumbersOfItems = (short)GetInt32(reader, "numbers_of_items");
            quest.ItemsType = GetInt32(reader, "items_type");
            return quest;
        }
    }
}
