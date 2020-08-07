using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    { 
        private const string SqlInsertOrReplaceShortcutItem =
            "INSERT OR REPLACE INTO nec_shortcut_bar (character_id, bar_num, slot_num, shortcut_type, shortcut_id) VALUES (@character_id, @bar_num, @slot_num, @shortcut_type, @shortcut_id);";

        private const string SqlSelectShortcutBar =
            "SELECT slot_num, shortcut_type, shortcut_id FROM nec_shortcut_bar WHERE character_id = @character_id AND bar_num = @bar_num";

        public void InsertOrReplaceShortcutItem(Character character, int barNumber, int slotNumber, ShortcutItem shortcutItem)
        {
            ExecuteNonQuery(SqlInsertOrReplaceShortcutItem, command =>
            {
                AddParameter(command, "@character_id", character.Id);               
                AddParameter(command, "@bar_num", barNumber);
                AddParameter(command, "@slot_num", slotNumber);
                AddParameter(command, "@shortcut_type", (int) shortcutItem.Type);
                AddParameter(command, "@shortcut_id", shortcutItem.Id);
            });
        }

        public ShortcutBar GetShortcutBar(Character character, int barNumber)
        {
            ShortcutBar shortcutBar = new ShortcutBar();
            ExecuteReader(SqlSelectShortcutBar,
                command => 
                { 
                    AddParameter(command, "@character_id", character.Id);
                    AddParameter(command, "@bar_num", barNumber);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        int i = GetInt32(reader, "slot_num");
                        if (i < 0 || i > ShortcutBar.COUNT) continue;                        
                        ShortcutItem shortcutItem = new ShortcutItem(GetInt32(reader, "shortcut_id"), (ShortcutItem.ShortcutType)GetInt32(reader, "shortcut_type"));                        
                        shortcutBar.Item[i] = shortcutItem;
                    }
                });
            return shortcutBar;
        }
    }
}
