using Necromancy.Server.Model;
using Necromancy.Server.Systems;
using Necromancy.Server.Systems.Inventory.Data_Access;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;


namespace Necromancy.Server.Inventory.Database
{
    public class InventoryDao : DatabaseAccessObject, IInventoryDao
    {
        private const string SqlSelectGold = @"
            SELECT
                gold
            FROM
                nec_character
            WHERE
                id = @character_id";        

        private const string SqlUpdateGoldAdd = @"
            UPDATE
                nec_character
                (
                    gold
                )
            VALUES
                (
                    gold + @amount
                )
            WHERE
                id = @character_id";

        private const string SqlUpdateGoldSubtract = @"
            UPDATE
                nec_character
                (
                    gold
                )
            VALUES
                (
                    gold - @amount
                )
            WHERE
                id = @character_id";

        private const string SqlSelectItemBySlot = @"
            SELECT
                id
            FROM
                nec_item_spawn
            WHERE
                character_id = @character_id
            AND
                bag = @bag_number
            AND
                slot = @bag_slot";
        public int SelectInventoryGold(Character character)
        {
            int gold = 0;
            ExecuteReader(SqlSelectGold,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                }, reader =>
                {
                    reader.Read();
                    gold = reader.GetInt32("gold");
                });
            return gold;
        }

        public long SelectSpawnIdBySlot(Character character, int bagNumber, int bagSlot)
        {
            long spawnId = 0;
            ExecuteReader(SqlSelectGold,
                command =>
                {
                    AddParameter(command, "@character_id", character.Id);
                    AddParameter(command, "@bag_number", bagNumber);
                    AddParameter(command, "@bag_slot", bagSlot);
                }, reader =>
                {
                    spawnId = reader.GetInt64("id");
                });
            return spawnId;
        }

        public void UpdateInventoryGoldAdd(Character character, int amount)
        {
            ExecuteNonQuery(SqlUpdateGoldAdd, command =>
            {
                AddParameter(command, "@amount", amount);
                AddParameter(command, "@character_id", character.Id);

            });
        }

        //TODO add check for 0 somewhere...
        public void UpdateInventoryGoldSubtract(Character character, int amount)
        {
            ExecuteNonQuery(SqlUpdateGoldSubtract, command =>
            {
                AddParameter(command, "@amount", amount);
                AddParameter(command, "@character_id", character.Id);

            });
        }
    }
}
