using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public interface IHandler
    {
        ushort Id { get; }
        int ExpectedSize { get; }
        void Handle(NecClient client, NecPacket packet);
    }
}