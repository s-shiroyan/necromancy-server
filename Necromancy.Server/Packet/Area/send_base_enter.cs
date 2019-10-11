using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;


namespace Necromancy.Server.Packet.Area
{
    public class send_base_enter : Handler
    {
        public send_base_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_base_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int accountId = packet.Data.ReadInt32();
            int unknown = packet.Data.ReadInt32();
            byte[] unknown1 = packet.Data.ReadBytes(20); // Suspect SessionId


            // TODO replace with sessionId
            Session session = Server.Sessions.GetSession(accountId.ToString());
            if (session == null)
            {
                Logger.Error(client, $"AccountId: {accountId} has no active session");
                client.Socket.Close();
                return;
            }

            client.Session = session;
            session.areaSocket = client.Socket;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //  Error
            Router.Send(client, (ushort) AreaPacketId.recv_base_enter_r, res);
        }
    }
}