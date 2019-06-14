using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet
{
    public abstract class Handler : IHandler
    {
        protected Handler(NecServer server)
        {
            Logger = LogProvider.Logger<NecLogger>(this);
            Server = server;
            Router = server.Router;
        }

        public abstract ushort Id { get; }
        public virtual int ExpectedSize => NecQueueConsumer.NoExpectedSize;
        protected NecServer Server { get; }
        protected NecSetting Settings { get; }
        protected NecLogger Logger { get; }
        protected PacketRouter Router { get; }
        public abstract void Handle(NecClient client, NecPacket packet);
    }
}