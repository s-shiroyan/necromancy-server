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
            "INSERT INTO `nec_union` (`id`,`name`,`union_leader_id`,`union_sub_leader1_id`,`union_sub_leader2_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created`)VALUES(@id,@name,@union_leader_id,@union_sub_leader1_id,@union_sub_leader2_id,@level,@current_exp,@next_level_exp,@member_limit_increase,@cape_design_id,@union_news,@created);";
        
        private const string SqlSelectUnionById =
            "SELECT `id`,`name`,`union_leader_id`,`union_sub_leader1_id`,`union_sub_leader2_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `id`=@id;";

        private const string SqlSelectUnionByUnionLeaderId =
            "SELECT `id`,`name`,`union_leader_id`,`union_sub_leader1_id`,`union_sub_leader2_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `union_leader_id`=@union_leader_id;";
        
        private const string SqlSelectUnionByName =
            "SELECT `id`,`name`,`union_leader_id`,`union_sub_leader1_id`,`union_sub_leader2_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `name`=@name;";


        private const string SqlUpdateUnion =
            "UPDATE `nec_union` SET `id`=@id,`name`=@name,`union_leader_id`=@union_leader_id,`union_sub_leader1_id`=@union_sub_leader1_id,`union_sub_leader2_id`=@union_sub_leader2_id,`level`=@level,`current_exp`=@current_exp,`next_level_exp`=@next_level_exp,`member_limit_increase`=@member_limit_increase,`cape_design_id`=@cape_design_id,`union_news`=@union_news,`created`=@created WHERE `id`=@id;";

        private const string SqlDeleteUnion =
            "DELETE FROM `nec_union` WHERE `id`=@id;";

        public bool InsertUnion(Union union)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertUnion, command =>
            {
                AddParameter(command, "@id", union.Id);
                AddParameter(command, "@name", union.Name);
                AddParameter(command, "@union_leader_id", union.UnionLeaderId);
                AddParameter(command, "@union_sub_leader1_id", union.UnionSubLeader1Id);
                AddParameter(command, "@union_sub_leader2_id", union.UnionSubLeader2Id);
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

            union.Id = (int)autoIncrement;
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
        public Union SelectUnionByUnionLeaderId(int leaderId)
        {
            Union union = null;
            ExecuteReader(SqlSelectUnionById,
                command => { AddParameter(command, "@id", leaderId); }, reader =>
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
                AddParameter(command, "@union_leader_id", union.UnionLeaderId);
                AddParameter(command, "@union_sub_leader1_id", union.UnionSubLeader1Id);
                AddParameter(command, "@union_sub_leader2_id", union.UnionSubLeader2Id);
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
                union.UnionLeaderId = GetInt32(reader, "union_leader_id");
                union.UnionSubLeader1Id = (uint)GetInt32(reader, "union_sub_leader1_id");
                union.UnionSubLeader2Id = (uint)GetInt32(reader, "union_sub_leader1_id");
                union.Level = (uint)GetInt32(reader, "level");
                union.CurrentExp = (uint)GetInt32(reader, "current_exp");
                union.NextLevelExp = (uint)GetInt32(reader, "next_level_exp");
                union.MemberLimitIncrease = GetByte(reader, "memver_limit_increase");
                union.CapeDesignID = GetInt16(reader, "cape_design_id");
                union.UnionNews = GetStringNullable(reader, "union_news");
                union.Created = GetDateTime(reader, "created");
                return union;
            }
        }
    }
}
