namespace Necromancy.Server.Database
{
    public interface IDatabase
    {
        void Execute(string sql);
    }
}