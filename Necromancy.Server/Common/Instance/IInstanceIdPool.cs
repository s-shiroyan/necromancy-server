namespace Necromancy.Server.Common.Instance
{
    public interface IInstanceIdPool
    {
        uint Used { get; }
        uint LowerBound { get; }
        uint UpperBound { get; }
        uint Size { get; }
        string Name { get; }
    }
}
