using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateItem =
            "INSERT INTO `nec_item` (`name`,`type`,`bitmask`,`count`,`state`,`icon`,`hair_override`,`face_override`,`durability`,`max_durability`,`weight`,`physics`,`magic`,`enchant_id`,`ac`,`date_end_protect`,`hardness`,`level`) VALUES (@name,@type,@bitmask,@count,@state,@icon,@hair_override,@face_override,@durability,@max_durability,@weight,@physics,@magic,@enchant_id,@ac,@date_end_protect,@hardness,@level);";

        private const string SqlSelectItemById =
            "SELECT `row_id`, `name`, `type`, `bitmask`, `count`, `state`, `icon`, `hair_override`, `face_override`, `durability`, `max_durability`, `weight`, `physics`, `magic`, `enchant_id`, `ac`, `date_end_protect`, `hardness`, `level` FROM `Items` WHERE `row_id`=@row_id; ";

        private const string SqlUpdateItem =
            "UPDATE `Items` SET `row_id`=@row_id, `name`=@name, `type`=@type, `bitmask`=@bitmask, `count`=@count, `state`=@state, `icon`=@icon, `hair_override`=@hair_override, `face_override`=@face_override, `durability`=@durability, `max_durability`=@max_durability, `weight`=@weight, `physics`=@physics, `magic`=@magic, `enchant_id`=@enchant_id, `ac`=@ac, `date_end_protect`=@date_end_protect, `hardness`=@hardness, `level`=@level WHERE `row_id`=@row_id;";

        private const string SqlDeleteItem =
            "DELETE FROM `Items` WHERE `row_id`=@row_id;";

        public bool InsertItem(Item item)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateItem, command =>
            {
                //AddParameter(command, "@rowId", item.instanceId);
                AddParameter(command, "@name", item.name);
                AddParameter(command, "@type", item.type);
                AddParameter(command, "@bitmask", item.bitmask);
                AddParameter(command, "@count", item.count);
                AddParameter(command, "@state", item.state);
                AddParameter(command, "@icon", item.icon);
                AddParameter(command, "@hair_override", item.hairOverride);
                AddParameter(command, "@face_override", item.faceOverride);
                AddParameter(command, "@durability", item.durability);
                AddParameter(command, "@max_durability", item.maxDurability);
                AddParameter(command, "@weight", item.weight);
                AddParameter(command, "@physics", item.physics);
                AddParameter(command, "@magic", item.magic);
                AddParameter(command, "@enchant_id", item.enchatId);
                AddParameter(command, "@ac", item.ac);
                AddParameter(command, "@date_end_protect", item.dateEndProtect);
                AddParameter(command, "@hardness", item.hardness);
                AddParameter(command, "@level", item.level);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            item.rowId = autoIncrement;
            return true;
        }


        public Item SelectItemById(int itemId)
        {
            Item item = null;
            ExecuteReader(SqlSelectItemById,
                command => { AddParameter(command, "@row_id", itemId); }, reader =>
                {
                    if (reader.Read())
                    {
                        item = ReadItem(reader);
                    }
                });
            return item;
        }

        public bool UpdateItem(Item item)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateItem, command =>
            {
                AddParameter(command, "@name", item.name);
                AddParameter(command, "@type", item.type);
                AddParameter(command, "@bitmask", item.bitmask);
                AddParameter(command, "@count", item.count);
                AddParameter(command, "@state", item.state);
                AddParameter(command, "@icon", item.icon);
                AddParameter(command, "@hair_override", item.hairOverride);
                AddParameter(command, "@face_override", item.faceOverride);
                AddParameter(command, "@durability", item.durability);
                AddParameter(command, "@max_durability", item.maxDurability);
                AddParameter(command, "@weight", item.weight);
                AddParameter(command, "@physics", item.physics);
                AddParameter(command, "@magic", item.magic);
                AddParameter(command, "@enchant_id", item.enchatId);
                AddParameter(command, "@ac", item.ac);
                AddParameter(command, "@date_end_protect", item.dateEndProtect);
                AddParameter(command, "@hardness", item.hardness);
                AddParameter(command, "@level", item.level);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteItem(int itemId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteItem,
                command => { AddParameter(command, "@row_id", itemId); });
            return rowsAffected > NoRowsAffected;
        }

        private Item ReadItem(DbDataReader reader)
        {
            Item item = new Item();
            item.rowId = GetInt32(reader, "row_id");
            item.name = GetString(reader, "name");
            item.type = GetInt32(reader, "type");
            item.bitmask = (byte)GetInt32(reader, "bitmask");
            item.count = (byte)GetInt32(reader, "count");
            item.state = GetInt32(reader, "state");
            item.icon = (byte)GetInt32(reader, "icon");
            item.hairOverride = (byte)GetInt32(reader, "hair_override");
            item.faceOverride = (byte)GetInt32(reader, "face_override");
            item.durability = (byte)GetInt32(reader, "durability");
            item.maxDurability = (byte)GetInt32(reader, "max_durability");
            item.weight = (byte)GetInt32(reader, "weight");
            item.physics = (byte)GetInt32(reader, "physics");
            item.magic = (byte)GetInt32(reader, "magic");
            item.enchatId = (byte)GetInt32(reader, "enchant_id");
            item.ac = (byte)GetInt32(reader, "ac");
            item.dateEndProtect = (byte)GetInt32(reader, "date_end_protect");
            item.hardness = (byte)GetInt32(reader, "hardness");
            item.level = (byte)GetInt32(reader, "level");

            return item;
        }
    }
}
