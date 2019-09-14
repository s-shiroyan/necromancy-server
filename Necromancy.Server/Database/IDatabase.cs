using Necromancy.Server.Model;

namespace Necromancy.Server.Database
{
    public interface IDatabase
    {
        void Execute(string sql);
        Account CreateAccount(string name, string mail, string hash);
        Account SelectAccount(string accountName);
        Account SelectAccount(int accountId);
    }
}