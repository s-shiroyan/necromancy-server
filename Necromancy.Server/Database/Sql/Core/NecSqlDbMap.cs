using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model.MapModel;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateMap =
            "INSERT INTO `nec_map` (`id`, `country`, `area`, `place`, `x`, `y`, `z`, `orientation`) VALUES (@id, @country, @area, @place, @x, @y, @z, @orientation);";

        private const string SqlSelectMapById =
            "SELECT `id`, `country`, `area`, `place`, `x`, `y`, `z`, `orientation` FROM `nec_map` WHERE `id`=@id; ";

        private const string SqlSelectMaps =
            "SELECT `id`, `country`, `area`, `place`, `x`, `y`, `z`, `orientation` FROM `nec_map`; ";

        private const string SqlUpdateMap =
            "UPDATE `nec_map` SET `country`=@country, `area`=@area, `place`=@place, `x`=@x, `y`=@y, `z`=@z `orientation`=@orientation WHERE `id`=@id;";

        private const string SqlDeleteMap =
            "DELETE FROM `nec_map` WHERE `id`=@id;";

        public bool InsertMap(MapData map)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateMap, command =>
            {
                AddParameter(command, "@id", map.Id);
                AddParameter(command, "@country", map.Country);
                AddParameter(command, "@area", map.Area);
                AddParameter(command, "@place", map.Place);
                AddParameter(command, "@x", map.X);
                AddParameter(command, "@y", map.Y);
                AddParameter(command, "@z", map.Z);
                AddParameter(command, "@orientation", map.Orientation);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected)
            {
                return false;
            }

            return true;
        }

        public MapData SelectItemMapId(int mapId)
        {
            MapData map = null;
            ExecuteReader(SqlSelectMapById,
                command => { AddParameter(command, "@id", mapId); }, reader =>
                {
                    if (reader.Read())
                    {
                        map = ReadMap(reader);
                    }
                });
            return map;
        }

        public List<MapData> SelectMaps()
        {
            List<MapData> maps = new List<MapData>();
            ExecuteReader(SqlSelectMaps, reader =>
            {
                while (reader.Read())
                {
                    MapData map = ReadMap(reader);
                    maps.Add(map);
                }
            });
            return maps;
        }

        public bool UpdateMap(MapData map)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateMap, command =>
            {
                AddParameter(command, "@id", map.Id);
                AddParameter(command, "@country", map.Country);
                AddParameter(command, "@area", map.Area);
                AddParameter(command, "@place", map.Place);
                AddParameter(command, "@x", map.X);
                AddParameter(command, "@y", map.Y);
                AddParameter(command, "@z", map.Z);
                AddParameter(command, "@orientation", map.Orientation);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteMap(int mapId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteMap,
                command => { AddParameter(command, "@id", mapId); });
            return rowsAffected > NoRowsAffected;
        }

        private MapData ReadMap(DbDataReader reader)
        {
            MapData map = new MapData();
            map.Id = GetInt32(reader, "id");
            map.Country = GetString(reader, "country");
            map.Area = GetString(reader, "area");
            map.Place = GetString(reader, "place");
            map.X = GetInt32(reader, "x");
            map.Y = GetInt32(reader, "y");
            map.Z = GetInt32(reader, "z");
            map.Orientation = GetInt32(reader, "orientation");
            return map;
        }
    }
}
