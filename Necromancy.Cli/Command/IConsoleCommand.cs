namespace Necromancy.Cli.Command
{
    public interface IConsoleCommand
    {
        void Handle(string line);        
        void Shutdown();
        string Key { get; }
        string Description { get; }
        bool RequireArgs { get; }
    }
}
