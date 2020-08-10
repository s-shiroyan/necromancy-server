using Necromancy.Server.Model;
using Necromancy.Server.Systems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;


namespace Necromancy.Server.Inventory.Database
{
    public class DbInventory : DatabaseAccessor
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

        public void UpdateInventoryGoldAdd(Character character, int amount)
        {
            ExecuteNonQuery(SqlUpdateGoldAdd, command =>
            {
                AddParameter(command, "@amount", amount);
                AddParameter(command, "@character_id", character.Id);

            });
        }

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
