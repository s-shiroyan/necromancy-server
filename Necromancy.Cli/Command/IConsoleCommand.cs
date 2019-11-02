namespace Necromancy.Cli.Command
{
    public interface IConsoleCommand
    {
        void Handle(string[] args);        
        void Shutdown();
        string Key { get; }
        string Description { get; }
        bool RequireArgs { get; }
    }
}
