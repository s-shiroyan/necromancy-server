using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertGimmick =
            "INSERT INTO `nec_gimmick_spawn` (`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `state`, `created`, `updated`) VALUES (@map_id, @x, @y, @z, @heading, @model_id, @state, @created, @updated);";

        private const string SqlSelectGimmicks =
            "SELECT `id`, `map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `state`, `created`, `updated` FROM `nec_gimmick_spawn`;";

        private const string SqlSelectGimmicksByMapId =
            "SELECT `id`, `map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `state`, `created`, `updated` FROM `nec_gimmick_spawn` WHERE `map_id`=@map_id;";

        private const string SqlUpdateGimmick =
            "UPDATE `nec_gimmick_spawn` SET `id`=@id, `map_id`=@map_id, `x`=@x,  `y`=@y, `z`=@z, `heading`=@heading, `model_id`=@model_id, `state`=@state, `created`=@created, `updated`=@updated WHERE `id`=@id;";

        private const string SqlDeleteGimmick =
            "DELETE FROM `nec_gimmick_spawn` WHERE `id`=@id;";

        public bool InsertGimmick(Gimmick gimmick)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertGimmick, command =>
            {
                //AddParameter(command, "@id", gimmick.Id);
                AddParameter(command, "@map_id", gimmick.MapId);
                AddParameter(command, "@x", gimmick.X);
                AddParameter(command, "@y", gimmick.Y);
                AddParameter(command, "@z", gimmick.Z);
                AddParameter(command, "@heading", gimmick.Heading);
                AddParameter(command, "@model_id", gimmick.ModelId);
                AddParameter(command, "@state", gimmick.State);
                AddParameter(command, "@created", gimmick.Created);
                AddParameter(command, "@updated", gimmick.Updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            gimmick.Id = (int) autoIncrement;
            return true;
        }

        public List<Gimmick> SelectGimmicks()
        {
            List<Gimmick> gimmicks = new List<Gimmick>();
            ExecuteReader(SqlSelectGimmicks, reader =>
            {
                while (reader.Read())
                {
                    Gimmick gimmick = ReadGimmick(reader);
                    gimmicks.Add(gimmick);
                }
            });
            return gimmicks;
        }

        public List<Gimmick> SelectGimmicksByMapId(int mapId)
        {
            List<Gimmick> gimmicks = new List<Gimmick>();
            ExecuteReader(SqlSelectGimmicksByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        Gimmick gimmick = ReadGimmick(reader);
                        gimmicks.Add(gimmick);
                    }
                });
            return gimmicks;
        }

        public bool UpdateGimmick(Gimmick gimmick)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateGimmick, command =>
            {
                AddParameter(command, "@id", gimmick.Id);
                AddParameter(command, "@map_id", gimmick.MapId);
                AddParameter(command, "@x", gimmick.X);
                AddParameter(command, "@y", gimmick.Y);
                AddParameter(command, "@z", gimmick.Z);
                AddParameter(command, "@heading", gimmick.Heading);
                AddParameter(command, "@model_id", gimmick.ModelId);
                AddParameter(command, "@state", gimmick.State);
                AddParameter(command, "@created", gimmick.Created);
                AddParameter(command, "@updated", gimmick.Updated);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteGimmick(int gimmickId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteGimmick,
                command => { AddParameter(command, "@id", gimmickId); });
            return rowsAffected > NoRowsAffected;
        }

        private Gimmick ReadGimmick(DbDataReader reader)
        {
            Gimmick gimmick = new Gimmick();
            gimmick.Id = GetInt32(reader, "id");
            gimmick.MapId = GetInt32(reader, "map_id");
            gimmick.X = GetFloat(reader, "x");
            gimmick.Y = GetFloat(reader, "y");
            gimmick.Z = GetFloat(reader, "z");
            gimmick.Heading = (byte)GetInt32(reader, "heading");
            gimmick.ModelId = GetInt32(reader, "model_id");
            gimmick.State = GetInt32(reader, "state");
            gimmick.Created = GetDateTime(reader, "created");
            gimmick.Updated = GetDateTime(reader, "updated");
            return gimmick;
        }
    }
}
