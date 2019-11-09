using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database
{
    public interface IDatabase
    {
        void Execute(string sql);

        /// <summary>
        /// Return true if database was created, or false if not.
        /// </summary>
        bool CreateDatabase();

        // Account
        Account CreateAccount(string name, string mail, string hash);
        Account SelectAccountById(int accountId);
        Account SelectAccountByName(string accountName);
        bool UpdateAccount(Account account);
        bool DeleteAccount(int accountId);

        // Soul
        bool InsertSoul(Soul soul);
        Soul SelectSoulById(int soulId);
        List<Soul> SelectSoulsByAccountId(int accountId);
        bool UpdateSoul(Soul soul);
        bool DeleteSoul(int soulId);

        // Character
        bool InsertCharacter(Character character);
        Character SelectCharacterById(int characterId);
        Character SelectCharacterBySlot(int soulId, int slot);
        List<Character> SelectCharactersByAccountId(int accountId);
        List<Character> SelectCharactersBySoulId(int soulId);
        bool UpdateCharacter(Character character);
        bool DeleteCharacter(int characterId);

        // NpcSpawn
        bool InsertNpcSpawn(NpcSpawn npcSpawnSpawn);
        List<NpcSpawn> SelectNpcSpawns();
        List<NpcSpawn> SelectNpcSpawnsByMapId(int mapId);
        bool UpdateNpcSpawn(NpcSpawn npcSpawnSpawn);
        bool DeleteNpcSpawn(int npcSpawnId);

        // Monster Spawn
        bool InsertMonsterSpawn(MonsterSpawn monsterSpawn);
        List<MonsterSpawn> SelectMonsterSpawns();
        List<MonsterSpawn> SelectMonsterSpawnsByMapId(int mapId);
        bool UpdateMonsterSpawn(MonsterSpawn monsterSpawn);
        bool DeleteMonsterSpawn(int monsterSpawnId);
    }
}
