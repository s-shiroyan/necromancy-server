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
            "INSERT INTO `nec_inventory_item` (`character_id`, `item_id`, `quantity`, `current_durability`) VALUES (@item_id, @quantity, @current_durability);";

        private const string SqlSelectInventoryItemById =
            "SELECT `id`, `character_id`, `item_id`, `quantity`, `current_durability` FROM `nec_inventory_item` WHERE `id`=@id; ";

        private const string SqlSelectInventoryItemsByCharacterId =
            "SELECT `id`, `character_id`, `item_id`, `quantity`, `current_durability` FROM `nec_inventory_item` WHERE `character_id`=@character_id; ";

        private const string SqlUpdateInventoryItem =
            "UPDATE `nec_inventory_item` SET `character_id`=@character_id, `item_id`=@item_id, `quantity`=@quantity, `current_durability`=@current_durability WHERE `id`=@id;";

        private const string SqlDeleteInventoryItem =
            "DELETE FROM `nec_inventory_item` WHERE `id`=@id;";


        public bool InsertInventoryItem(InventoryItem inventoryItem)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateInventoryItem, command =>
            {
                AddParameter(command, "@character_id", inventoryItem.CharacterId);
                AddParameter(command, "@item_id", inventoryItem.ItemId);
                AddParameter(command, "@quantity", inventoryItem.Quantity);
                AddParameter(command, "@current_durability", inventoryItem.CurrentDurability);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

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
            inventoryItem.ItemId = GetUInt32(reader, "item_id");
            inventoryItem.Quantity = GetInt32(reader, "quantity");
            inventoryItem.CurrentDurability = GetInt32(reader, "current_durability");
            return inventoryItem;
        }
    }
}
