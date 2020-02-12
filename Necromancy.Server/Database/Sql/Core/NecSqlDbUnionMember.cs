using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model.Union;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertUnionMember=
            "INSERT INTO `nec_union_member` (`union_id`,`character_id`,`member_priviledge_bitmask`,`joined`)VALUES(@union_id,@character_id,@member_priviledge_bitmask,@joined);";
        
        private const string SqlSelectUnionMemberByCharacterId =
            "SELECT `id`,`union_id`,`character_id`,`member_priviledge_bitmask`,`joined` FROM `nec_union_member` WHERE `character_id`=@character_id;";

        private const string SqlSelectUnionMembersByUnionId =
            "SELECT `id`,`union_id`,`character_id`,`member_priviledge_bitmask`,`joined` FROM `nec_union_member` WHERE `union_id`=@union_id;";

        private const string SqlUpdateUnionMember =
            "UPDATE `nec_union_member` SET `id`=@id,`union_id`=@union_id,`character_id`=@character_id,`member_priviledge_bitmask`=@member_priviledge_bitmask,`joined`=@joined  WHERE `character_id`=@character_id;";

        private const string SqlDeleteUnionMember =
            "DELETE FROM `nec_union_member` WHERE `character_id`=@character_id;";

        private const string SqlDeleteAllUnionMember =
            "DELETE FROM `nec_union_member` WHERE `union_id`=@union_id;";

        public bool InsertUnionMember(UnionMember unionMember)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertUnionMember, command =>
            {
                //AddParameter(command, "@id", unionMember.Id);
                AddParameter(command, "@union_id", unionMember.UnionId);
                AddParameter(command, "@character_id", unionMember.CharacterDatabaseId);
                AddParameter(command, "@member_priviledge_bitmask", unionMember.MemberPriviledgeBitMask);
                AddParameter(command, "@joined", unionMember.Joined);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            unionMember.Id = (int)autoIncrement;
            return true;
        }
        
        public UnionMember SelectUnionMemberByCharacterId(int CharacterDatabaseId)
        {
            UnionMember unionMember = null;
            ExecuteReader(SqlSelectUnionMemberByCharacterId,
                command => { AddParameter(command, "@character_id", CharacterDatabaseId); }, reader =>
                {
                    if (reader.Read())
                    {
                        unionMember = ReadUnionMember(reader);
                    }
                });
            return unionMember;
        }

        public List<UnionMember> SelectUnionMembersByUnionId(int unionId)
        {
            List<UnionMember> unionMembers = new List<UnionMember>();
            ExecuteReader(SqlSelectUnionMembersByUnionId,
                command => { AddParameter(command, "@union_id", unionId); }, reader =>
                {
                    while (reader.Read())
                    {
                        UnionMember unionMember = ReadUnionMember(reader);
                        unionMembers.Add(unionMember);
                    }
                });
            return unionMembers;
        }

        public bool UpdateUnionMember(UnionMember unionMember)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateUnionMember, command =>
            {
                //AddParameter(command, "@id", unionMember.Id);
                AddParameter(command, "@union_id", unionMember.UnionId);
                AddParameter(command, "@character_id", unionMember.CharacterDatabaseId);
                AddParameter(command, "@member_proviledge_bitmask", unionMember.MemberPriviledgeBitMask);
                AddParameter(command, "@joined", unionMember.Joined);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteUnionMember(int characterDatabaseId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteUnionMember, command => { AddParameter(command, "@character_id", characterDatabaseId); });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteAllUnionMembers(int unionId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteAllUnionMember, command => { AddParameter(command, "@union_id", unionId); });
            return rowsAffected > NoRowsAffected;
        }

        private UnionMember ReadUnionMember(DbDataReader reader)
        {
            {
                UnionMember unionMember = new UnionMember();
                unionMember.Id = GetInt32(reader, "id");
                unionMember.UnionId = GetInt32(reader, "union_id");
                unionMember.CharacterDatabaseId = GetInt32(reader, "character_id");
                unionMember.MemberPriviledgeBitMask = (uint)GetInt32(reader, "member_priviledge_bitmask");
                unionMember.Joined = GetDateTime(reader, "joined");
                return unionMember;
            }
        }
    }
}
