using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertNpcSpawn =
            "INSERT INTO `nec_npc_spawn` (`npc_id`, `model_id`, `level`,  `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `visibility`, `created`, `updated`) VALUES (@npc_id, @model_id, @level, @name, @title, @map_id, @x, @y, @z, @active, @heading, @size, @visibility, @created, @updated);";

        private const string SqlSelectNpcSpawns =
            "SELECT `id`, `npc_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `visibility`, `created`, `updated` FROM `nec_npc_spawn`;";

        private const string SqlSelectNpcSpawnsByMapId =
            "SELECT `id`, `npc_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `visibility`, `created`, `updated` FROM `nec_npc_spawn` WHERE `map_id`=@map_id;";

        private const string SqlUpdateNpcSpawn =
            "UPDATE `nec_npc_spawn` SET `npc_id`=@npc_id, `model_id`=@model_id, `level`=@level,  `name`=@name, `title`=@title, `map_id`=@map_id, `x`=@x, `y`=@y, `z`=@z, `active`=@active, `heading`=@heading, `size`=@size, `visibility`=@visibility, `created`=@created, `updated`=@updated WHERE `id`=@id;";

        private const string SqlDeleteNpcSpawn =
            "DELETE FROM `nec_npc_spawn` WHERE `id`=@id;";

        public bool InsertNpcSpawn(NpcSpawn npcSpawnSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertNpcSpawn, command =>
            {
                AddParameter(command, "@npc_id", npcSpawnSpawn.NpcId);
                AddParameter(command, "@model_id", npcSpawnSpawn.ModelId);
                AddParameter(command, "@level", npcSpawnSpawn.Level);
                AddParameter(command, "@name", npcSpawnSpawn.Name);
                AddParameter(command, "@title", npcSpawnSpawn.Title);
                AddParameter(command, "@map_id", npcSpawnSpawn.MapId);
                AddParameter(command, "@x", npcSpawnSpawn.X);
                AddParameter(command, "@y", npcSpawnSpawn.Y);
                AddParameter(command, "@z", npcSpawnSpawn.Z);
                AddParameter(command, "@active", npcSpawnSpawn.Active);
                AddParameter(command, "@heading", npcSpawnSpawn.Heading);
                AddParameter(command, "@size", npcSpawnSpawn.Size);
                AddParameter(command, "@visibility", npcSpawnSpawn.Visibility);
                AddParameter(command, "@created", npcSpawnSpawn.Created);
                AddParameter(command, "@updated", npcSpawnSpawn.Updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            npcSpawnSpawn.Id = (int) autoIncrement;
            return true;
        }

        public List<NpcSpawn> SelectNpcSpawns()
        {
            List<NpcSpawn> npcSpawns = new List<NpcSpawn>();
            ExecuteReader(SqlSelectNpcSpawns, reader =>
            {
                while (reader.Read())
                {
                    NpcSpawn npcSpawnSpawn = ReadNpcSpawn(reader);
                    npcSpawns.Add(npcSpawnSpawn);
                }
            });
            return npcSpawns;
        }

        public List<NpcSpawn> SelectNpcSpawnsByMapId(int mapId)
        {
            List<NpcSpawn> npcSpawns = new List<NpcSpawn>();
            ExecuteReader(SqlSelectNpcSpawnsByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        NpcSpawn npcSpawnSpawn = ReadNpcSpawn(reader);
                        npcSpawns.Add(npcSpawnSpawn);
                    }
                });
            return npcSpawns;
        }

        public bool UpdateNpcSpawn(NpcSpawn npcSpawnSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateNpcSpawn, command =>
            {
                AddParameter(command, "@npc_id", npcSpawnSpawn.NpcId);
                AddParameter(command, "@model_id", npcSpawnSpawn.ModelId);
                AddParameter(command, "@level", npcSpawnSpawn.Level);
                AddParameter(command, "@name", npcSpawnSpawn.Name);
                AddParameter(command, "@title", npcSpawnSpawn.Title);
                AddParameter(command, "@map_id", npcSpawnSpawn.MapId);
                AddParameter(command, "@x", npcSpawnSpawn.X);
                AddParameter(command, "@y", npcSpawnSpawn.Y);
                AddParameter(command, "@z", npcSpawnSpawn.Z);
                AddParameter(command, "@active", npcSpawnSpawn.Active);
                AddParameter(command, "@heading", npcSpawnSpawn.Heading);
                AddParameter(command, "@size", npcSpawnSpawn.Size);
                AddParameter(command, "@visibility", npcSpawnSpawn.Visibility);
                AddParameter(command, "@created", npcSpawnSpawn.Created);
                AddParameter(command, "@updated", npcSpawnSpawn.Updated);
                AddParameter(command, "@id", npcSpawnSpawn.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteNpcSpawn(int npcSpawnId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteNpcSpawn,
                command => { AddParameter(command, "@id", npcSpawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private NpcSpawn ReadNpcSpawn(DbDataReader reader)
        {
            NpcSpawn npcSpawnSpawn = new NpcSpawn();
            npcSpawnSpawn.Id = GetInt32(reader, "id");
            npcSpawnSpawn.ModelId = GetInt32(reader, "model_id");
            npcSpawnSpawn.NpcId = GetInt32(reader, "npc_id");
            npcSpawnSpawn.Level = GetByte(reader, "level");
            npcSpawnSpawn.Name = GetString(reader, "name");
            npcSpawnSpawn.Title = GetString(reader, "title");
            npcSpawnSpawn.MapId = GetInt32(reader, "map_id");
            npcSpawnSpawn.X = GetFloat(reader, "x");
            npcSpawnSpawn.Y = GetFloat(reader, "y");
            npcSpawnSpawn.Z = GetFloat(reader, "z");
            npcSpawnSpawn.Active = GetBoolean(reader, "active");
            npcSpawnSpawn.Heading = GetByte(reader, "heading");
            npcSpawnSpawn.Size = GetInt16(reader, "size");
            npcSpawnSpawn.Visibility = GetInt32(reader, "visibility");
            npcSpawnSpawn.Created = GetDateTime(reader, "created");
            npcSpawnSpawn.Updated = GetDateTime(reader, "updated");
            return npcSpawnSpawn;
        }
    }
}
