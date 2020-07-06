using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model.ItemModel;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateInventoryItem =
            "INSERT INTO `nec_item_spawn` (`character_id`, `item_id`, `quantity`, `current_durability`, `storage_type`, `bag`, `slot`, `state`) VALUES (@character_id, @item_id, @quantity, @current_durability, @storage_type, @bag, @slot, @state);";

        private const string SqlSelectInventoryItemById =
            "SELECT `id`, `character_id`, `item_id`, `quantity`, `current_durability`, `storage_type`, `bag`, `slot`, `state` FROM `nec_item_spawn` WHERE `id`=@id; ";

        private const string SqlSelectInventoryItemsByCharacterId =
            "SELECT `id`, `character_id`, `item_id`, `quantity`, `current_durability`, `storage_type`, `bag`, `slot`, `state` FROM `nec_item_spawn` WHERE `character_id`=@character_id; ";

        private const string SqlUpdateInventoryItem =
            "UPDATE `nec_item_spawn` SET `character_id`=@character_id, `item_id`=@item_id, `quantity`=@quantity, `current_durability`=@current_durability, `storage_type`=@storage_type, `bag`=@bag, `slot`=@slot, `state`=@state WHERE `id`=@id;";

        private const string SqlDeleteInventoryItem =
            "DELETE FROM `nec_item_spawn` WHERE `id`=@id;";


        public bool InsertInventoryItem(InventoryItem inventoryItem)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateInventoryItem, command =>
            {
                AddParameter(command, "@character_id", inventoryItem.CharacterId);
                AddParameter(command, "@item_id", inventoryItem.ItemId);
                AddParameter(command, "@quantity", inventoryItem.Quantity);
                AddParameter(command, "@current_durability", inventoryItem.CurrentDurability);
                AddParameter(command, "@storage_type", inventoryItem.StorageType);
                AddParameter(command, "@bag", inventoryItem.BagId);
                AddParameter(command, "@slot", inventoryItem.BagSlotIndex);
                AddParameter(command, "@state", inventoryItem.State);

            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            inventoryItem.Id = (int)autoIncrement;
            return true;
        }


        public InventoryItem SelectInventoryItemById(int inventoryItemId)
        {
            InventoryItem item = null;
            ExecuteReader(SqlSelectInventoryItemById,
                command => { AddParameter(command, "@id", inventoryItemId); }, reader =>
                {
                    if (reader.Read())
                    {
                        item = ReadInventoryItem(reader);
                    }
                });
            return item;
        }

        public List<InventoryItem> SelectInventoryItemsByCharacterId(int characterId)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            ExecuteReader(SqlSelectInventoryItemsByCharacterId,
                command => { AddParameter(command, "@character_id", characterId); }, reader =>
                {
                    while (reader.Read())
                    {
                        InventoryItem item = ReadInventoryItem(reader);
                        inventoryItems.Add(item);
                    }
                });
            return inventoryItems;
        }

        public bool UpdateInventoryItem(InventoryItem inventoryItem)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateInventoryItem, command =>
            {
                AddParameter(command, "@id", inventoryItem.Id);
                AddParameter(command, "@character_id", inventoryItem.CharacterId);
                AddParameter(command, "@item_id", inventoryItem.ItemId);
                AddParameter(command, "@quantity", inventoryItem.Quantity);
                AddParameter(command, "@current_durability", inventoryItem.CurrentDurability);
                AddParameter(command, "@storage_type", inventoryItem.StorageType);
                AddParameter(command, "@bag", inventoryItem.BagId);
                AddParameter(command, "@slot", inventoryItem.BagSlotIndex);
                AddParameter(command, "@state", inventoryItem.State);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteInventoryItem(int inventoryItemId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteInventoryItem,
                command => { AddParameter(command, "@id", inventoryItemId); });
            return rowsAffected > NoRowsAffected;
        }

        private InventoryItem ReadInventoryItem(DbDataReader reader)
        {
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Id = GetInt32(reader, "id");
            inventoryItem.CharacterId = GetInt32(reader, "character_id");
            inventoryItem.ItemId = GetInt32(reader, "item_id");
            inventoryItem.Quantity = GetByte(reader, "quantity");
            inventoryItem.CurrentDurability = GetInt32(reader, "current_durability");
            inventoryItem.StorageType = GetByte(reader, "storage_type");
            inventoryItem.BagId = GetByte(reader, "bag");
            inventoryItem.BagSlotIndex = GetInt16(reader, "slot");
            inventoryItem.State = GetInt32(reader, "state");
            return inventoryItem;
        }
    }
}
