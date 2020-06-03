using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model.Union;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertUnion =
            "INSERT INTO `nec_union` (`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created`)VALUES(@name,@leader_character_id,@subleader1_character_id,@subleader2_character_id,@level,@current_exp,@next_level_exp,@member_limit_increase,@cape_design_id,@union_news,@created);";
        
        private const string SqlSelectUnionById =
            "SELECT `id`,`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `id`=@id;";

        private const string SqlSelectUnionByLeaderId =
            "SELECT `id`,`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `leader_character_id`=@leader_character_id;";
        
        private const string SqlSelectUnionByName =
            "SELECT `id`,`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `name`=@name;";


        private const string SqlUpdateUnion =
            "UPDATE `nec_union` SET `id`=@id,`name`=@name,`leader_character_id`=@leader_character_id,`subleader1_character_id`=@subleader1_character_id,`subleader2_character_id`=@subleader2_character_id,`level`=@level,`current_exp`=@current_exp,`next_level_exp`=@next_level_exp,`member_limit_increase`=@member_limit_increase,`cape_design_id`=@cape_design_id,`union_news`=@union_news,`created`=@created WHERE `id`=@id;";

        private const string SqlDeleteUnion =
            "DELETE FROM `nec_union` WHERE `id`=@id;";

        public bool InsertUnion(Union union)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertUnion, command =>
            {
                //AddParameter(command, "@id", union.Id);
                AddParameter(command, "@name", union.Name);
                AddParameter(command, "@leader_character_id", union.LeaderId);
                AddParameter(command, "@subleader1_character_id", union.SubLeader1Id);
                AddParameter(command, "@subleader2_character_id", union.SubLeader2Id);
                AddParameter(command, "@level", union.Level);
                AddParameter(command, "@current_exp", union.CurrentExp);
                AddParameter(command, "@next_level_exp", union.NextLevelExp);
                AddParameter(command, "@member_limit_increase", union.MemberLimitIncrease);
                AddParameter(command, "@cape_design_id", union.CapeDesignID);
                AddParameter(command, "@union_news", union.UnionNews);
                AddParameter(command, "@created", union.Created);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            union.Id = (int) autoIncrement;
            return true;
        }
        
        public Union SelectUnionById(int unionId)
        {
            Union union = null;
            ExecuteReader(SqlSelectUnionById,
                command => { AddParameter(command, "@id", unionId); }, reader =>
                {
                    if (reader.Read())
                    {
                        union = ReadUnion(reader);
                    }
                });
            return union;
        }
        public Union SelectUnionByLeaderId(int leaderId)
        {
            Union union = null;
            ExecuteReader(SqlSelectUnionByLeaderId,
                command => { AddParameter(command, "@leader_character_id", leaderId); }, reader =>
                {
                    if (reader.Read())
                    {
                        union = ReadUnion(reader);
                    }
                });
            return union;
        }

        public Union SelectUnionByName(string unionName)
        {
            Union union = null;
            ExecuteReader(SqlSelectUnionByName,
                command => { AddParameter(command, "@name", unionName); }, reader =>
                {
                    if (reader.Read())
                    {
                        union = ReadUnion(reader);
                    }
                });
            return union;
        }

        public bool UpdateUnion(Union union)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateUnion, command =>
            {
                AddParameter(command, "@id", union.Id);
                AddParameter(command, "@name", union.Name);
                AddParameter(command, "@leader_character_id", union.LeaderId);
                AddParameter(command, "@subleader1_character_id", union.SubLeader1Id);
                AddParameter(command, "@subleader2_character_id", union.SubLeader2Id);
                AddParameter(command, "@level", union.Level);
                AddParameter(command, "@current_exp", union.CurrentExp);
                AddParameter(command, "@next_level_exp", union.NextLevelExp);
                AddParameter(command, "@member_limit_increase", union.MemberLimitIncrease);
                AddParameter(command, "@cape_design_id", union.CapeDesignID);
                AddParameter(command, "@union_news", union.UnionNews);
                AddParameter(command, "@created", union.Created);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteUnion(int unionId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteUnion, command => { AddParameter(command, "@id", unionId); });
            return rowsAffected > NoRowsAffected;
        }

        private Union ReadUnion(DbDataReader reader)
        {
            {
                Union union = new Union();
                union.Id = GetInt32(reader, "id");
                union.Name = GetStringNullable(reader, "name");
                union.LeaderId = GetInt32(reader, "leader_character_id");
                union.SubLeader1Id = GetInt32(reader, "subleader1_character_id");
                union.SubLeader2Id = GetInt32(reader, "subleader1_character_id");
                union.Level = (uint)GetInt32(reader, "level");
                union.CurrentExp = (uint)GetInt32(reader, "current_exp");
                union.NextLevelExp = (uint)GetInt32(reader, "next_level_exp");
                union.MemberLimitIncrease = (byte)GetInt32(reader, "member_limit_increase");
                union.CapeDesignID = GetInt16(reader, "cape_design_id");
                union.UnionNews = GetStringNullable(reader, "union_news");
                union.Created = GetDateTime(reader, "created");
                return union;
            }
        }
    }
}
