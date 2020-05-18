using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateItems =
            "INSERT INTO `nec_item` (`name`,`type`,`bitmask`,`count`,`status`,`icon1`,`icon2`,`hairOverride`,`faceOverride`,`durability`,`maxDurability`,`weight`,`physics`,`magic`,`enchatId`,`ac`,`dateEndProtect`,`hardness`,`level`) VALUES (@name,@type,@bitmask,@count,@status,@icon1,@icon2,@hairOverride,@faceOverride,@durability,@maxDurability,@weight,@physics,@magic,@enchatId,@ac,@dateEndProtect,@hardness,@level);";

        private const string SqlSelectItemsById =
            "SELECT `rowId`, `name`, `type`, `bitmask`, `count`, `status`, `icon1`, `icon2`, `hairOverride`, `faceOverride`, `durability`, `maxDurability`, `weight`, `physics`, `magic`, `enchatId`, `ac`, `dateEndProtect`, `hardness`, `level` FROM `Items` WHERE `rowId`=@rowId; ";

        private const string SqlUpdateItems =
            "UPDATE `Items` SET `rowId`=@rowId, `name`=@name, `type`=@type, `bitmask`=@bitmask, `count`=@count, `status`=@status, `icon1`=@icon1, `icon2`=@icon2, `hairOverride`=@hairOverride, `faceOverride`=@faceOverride, `durability`=@durability, `maxDurability`=@maxDurability, `weight`=@weight, `physics`=@physics, `magic`=@magic, `enchantId`=@enchatId, `ac`=@ac, `dateEndProtect`=@dateEndProtect, `hardness`=@hardness, `level`=@level WHERE `rowId`=@rowId;";

        private const string SqlDeleteItems =
            "DELETE FROM `Items` WHERE `rowId`=@rowId;";

        public bool InsertItems(Item item)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateItems, command =>
            {
                //AddParameter(command, "@rowId", item.instanceId);
                AddParameter(command, "@name", item.name);
                AddParameter(command, "@type", item.type);
                AddParameter(command, "@bitmask", item.bitmask);
                AddParameter(command, "@count", item.count);
                AddParameter(command, "@state", item.state);
                AddParameter(command, "@icon1", item.icon1);
                AddParameter(command, "@icon2", item.icon2);
                AddParameter(command, "@hairOverride", item.hairOverride);
                AddParameter(command, "@faceOverride", item.faceOverride);
                AddParameter(command, "@durability", item.durability);
                AddParameter(command, "@maxDurability", item.maxDurability);
                AddParameter(command, "@weight", item.weight);
                AddParameter(command, "@physics", item.physics);
                AddParameter(command, "@magic", item.magic);
                AddParameter(command, "@enchatId", item.enchatId);
                AddParameter(command, "@ac", item.ac);
                AddParameter(command, "@dateEndProtect", item.dateEndProtect);
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


        public Item SelectitemsById(int itemsId)
        {
            Item item = null;
            ExecuteReader(SqlSelectItemsById,
                command => { AddParameter(command, "@rowId", itemsId); }, reader =>
                {
                    if (reader.Read())
                    {
                        item = ReadItems(reader);
                    }
                });
            return item;
        }

        public bool UpdateItems(Item item)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateItems, command =>
            {
                AddParameter(command, "@name", item.name);
                AddParameter(command, "@type", item.type);
                AddParameter(command, "@bitmask", item.bitmask);
                AddParameter(command, "@count", item.count);
                AddParameter(command, "@state", item.state);
                AddParameter(command, "@icon1", item.icon1);
                AddParameter(command, "@icon2", item.icon2);
                AddParameter(command, "@hairOverride", item.hairOverride);
                AddParameter(command, "@faceOverride", item.faceOverride);
                AddParameter(command, "@durability", item.durability);
                AddParameter(command, "@maxDurability", item.maxDurability);
                AddParameter(command, "@weight", item.weight);
                AddParameter(command, "@physics", item.physics);
                AddParameter(command, "@magic", item.magic);
                AddParameter(command, "@enchatId", item.enchatId);
                AddParameter(command, "@ac", item.ac);
                AddParameter(command, "@dateEndProtect", item.dateEndProtect);
                AddParameter(command, "@hardness", item.hardness);
                AddParameter(command, "@level", item.level);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteItems(int itemsId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteItems,
                command => { AddParameter(command, "@rowId", itemsId); });
            return rowsAffected > NoRowsAffected;
        }

        private Item ReadItems(DbDataReader reader)
        {
            Item item = new Item();
            item.rowId = GetInt32(reader, "rowId");
            item.name = GetString(reader, "name");
            item.type = GetInt32(reader, "type");
            item.bitmask = (byte)GetInt32(reader, "bitmask");
            item.count = (byte)GetInt32(reader, "count");
            item.state = GetInt32(reader, "state");
            item.icon1 = (byte)GetInt32(reader, "icon1");
            item.icon2 = (byte)GetInt32(reader, "icon2");
            item.hairOverride = (byte)GetInt32(reader, "hairOverride");
            item.faceOverride = (byte)GetInt32(reader, "faceOverride");
            item.durability = (byte)GetInt32(reader, "durability");
            item.maxDurability = (byte)GetInt32(reader, "maxDurability");
            item.weight = (byte)GetInt32(reader, "weight");
            item.physics = (byte)GetInt32(reader, "physics");
            item.magic = (byte)GetInt32(reader, "magic");
            item.enchatId = (byte)GetInt32(reader, "enchatId");
            item.ac = (byte)GetInt32(reader, "ac");
            item.dateEndProtect = (byte)GetInt32(reader, "dateEndProtect");
            item.hardness = (byte)GetInt32(reader, "hardness");
            item.level = (byte)GetInt32(reader, "level");

            return item;
        }
    }
}
