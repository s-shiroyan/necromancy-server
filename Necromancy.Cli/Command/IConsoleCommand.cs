namespace Necromancy.Cli.Command
{
    public interface IConsoleCommand
    {
        void Handle(string line);
        string Key { get; }
    }
}
