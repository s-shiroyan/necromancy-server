using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database
{
    public interface IDatabase
    {
        void Execute(string sql);

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
        List<Character> SelectCharacterByAccountId(int accountId);
        List<Character> SelectCharacterBySoulId(int soulId);
        bool UpdateCharacter(Character character);
        bool DeleteCharacter(int characterId);
    }
}