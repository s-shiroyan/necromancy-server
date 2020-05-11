using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Custom
{
    public class SendDisconnect : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SendDisconnect));

        public SendDisconnect(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) CustomPacketId.SendDisconnect;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int data = packet.Data.ReadInt32();
            Logger.Error(client,
                $"{client.Soul.Name} {client.Character.Name} has sent a disconnect packet to the server.  Wave GoodBye! ");

            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt32(data);

            NecPacket response = new NecPacket(
                (ushort) CustomPacketId.RecvDisconnect,
                buffer,
                packet.ServerType,
                PacketType.Disconnect
            );

            Router.Send(client, response);
        }
    }
}
