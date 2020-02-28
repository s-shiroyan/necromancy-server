using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertGGateSpawn =
            "INSERT INTO `nec_gGate_spawn` (`serial_id`,`interaction`,`name`,`title`,`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `size`,`active`,`glow`,`created`, `updated`) VALUES (@serial_id,@interaction,@name,@title,@map_id, @x, @y, @z, @heading, @model_id, @size, @active, @glow, @created, @updated);";

        private const string SqlSelectGGateSpawns =
            "SELECT `id`, `serial_id`,`interaction`,`name`,`title`,`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `size`,`active`,`glow`,`created`, `updated` FROM `nec_ggate_spawn`;";

        private const string SqlSelectGGateSpawnsByMapId =
            "SELECT `id`, `serial_id`,`interaction`,`name`,`title`,`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `size`,`active`,`glow`,`created`, `updated` FROM `nec_ggate_spawn` WHERE `map_id`=@map_id;";

        private const string SqlUpdateGGateSpawn =
            "UPDATE `nec_ggate_spawn` SET `id`=@id,`serial_id`=@serial_id,`interaction`=@interaction,`name`=@name,`title`=@title,`map_id`=@map_id, `x`=@x,  `y`=@y, `z`=@z, `heading`=@heading, `model_id`=@model_id, `size`=@size,`active`=@active,`glow`=@glow,`created`=@created, `updated`=@updated WHERE `id`=@id;";

        private const string SqlDeleteGGateSpawn =
            "DELETE FROM `nec_ggate_spawn` WHERE `id`=@id;";

        public bool InsertGGateSpawn(GGateSpawn gGateSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertGGateSpawn, command =>
            {
                //AddParameter(command, "@id", gGateSpawn.Id);
                AddParameter(command, "@serial_id", gGateSpawn.SerialId);
                AddParameter(command, "@interaction", gGateSpawn.Interaction);
                AddParameter(command, "@name", gGateSpawn.Name);
                AddParameter(command, "@title", gGateSpawn.Title);
                AddParameter(command, "@map_id", gGateSpawn.MapId);
                AddParameter(command, "@x", gGateSpawn.X);
                AddParameter(command, "@y", gGateSpawn.Y);
                AddParameter(command, "@z", gGateSpawn.Z);
                AddParameter(command, "@heading", gGateSpawn.Heading);
                AddParameter(command, "@model_id", gGateSpawn.ModelId);
                AddParameter(command, "@size", gGateSpawn.Size);
                AddParameter(command, "@active", gGateSpawn.Active);
                AddParameter(command, "@glow", gGateSpawn.Glow);
                AddParameter(command, "@created", gGateSpawn.Created);
                AddParameter(command, "@updated", gGateSpawn.Updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            gGateSpawn.Id = (int) autoIncrement;
            return true;
        }

        public List<GGateSpawn> SelectGGateSpawns()
        {
            List<GGateSpawn> gGateSpawns = new List<GGateSpawn>();
            ExecuteReader(SqlSelectGGateSpawns, reader =>
            {
                while (reader.Read())
                {
                    GGateSpawn gGateSpawn = ReadGGateSpawn(reader);
                    gGateSpawns.Add(gGateSpawn);
                }
            });
            return gGateSpawns;
        }

        public List<GGateSpawn> SelectGGateSpawnsByMapId(int mapId)
        {
            List<GGateSpawn> gGateSpawns = new List<GGateSpawn>();
            ExecuteReader(SqlSelectGGateSpawnsByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        GGateSpawn gGateSpawn = ReadGGateSpawn(reader);
                        gGateSpawns.Add(gGateSpawn);
                    }
                });
            return gGateSpawns;
        }

        public bool UpdateGGateSpawn(GGateSpawn gGateSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateGGateSpawn, command =>
            {
                AddParameter(command, "@id", gGateSpawn.Id);
                AddParameter(command, "@serial_id", gGateSpawn.SerialId);
                AddParameter(command, "@interaction", gGateSpawn.Interaction);
                AddParameter(command, "@name", gGateSpawn.Name);
                AddParameter(command, "@title", gGateSpawn.Title);
                AddParameter(command, "@map_id", gGateSpawn.MapId);
                AddParameter(command, "@x", gGateSpawn.X);
                AddParameter(command, "@y", gGateSpawn.Y);
                AddParameter(command, "@z", gGateSpawn.Z);
                AddParameter(command, "@heading", gGateSpawn.Heading);
                AddParameter(command, "@model_id", gGateSpawn.ModelId);
                AddParameter(command, "@size", gGateSpawn.Size);
                AddParameter(command, "@active", gGateSpawn.Active);
                AddParameter(command, "@glow", gGateSpawn.Glow);
                AddParameter(command, "@created", gGateSpawn.Created);
                AddParameter(command, "@updated", gGateSpawn.Updated);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteGGateSpawn(int gGateSpawnId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteGGateSpawn,
                command => { AddParameter(command, "@id", gGateSpawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private GGateSpawn ReadGGateSpawn(DbDataReader reader)
        {
            GGateSpawn gGateSpawn = new GGateSpawn();
            gGateSpawn.Id = GetInt32(reader, "id");
            gGateSpawn.SerialId = GetInt32(reader, "serial_id");
            gGateSpawn.Interaction = GetByte(reader, "interaction");
            gGateSpawn.Name = GetString(reader, "name");
            gGateSpawn.Title = GetString(reader, "title");
            gGateSpawn.MapId = GetInt32(reader, "map_id");
            gGateSpawn.X = GetFloat(reader, "x");
            gGateSpawn.Y = GetFloat(reader, "y");
            gGateSpawn.Z = GetFloat(reader, "z");
            gGateSpawn.Heading = (byte)GetInt32(reader, "heading");
            gGateSpawn.ModelId = GetInt32(reader, "model_id");
            gGateSpawn.Size = GetInt16(reader, "size");
            gGateSpawn.Active = GetInt32(reader, "active");
            gGateSpawn.Glow = GetInt32(reader, "glow");
            gGateSpawn.Created = GetDateTime(reader, "created");
            gGateSpawn.Updated = GetDateTime(reader, "updated");
            return gGateSpawn;
        }
    }
}
