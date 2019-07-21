using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Auth
{
    public class send_base_authenticate : Handler
    {
        public send_base_authenticate(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AuthPacketId.send_base_authenticate;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string accountName = packet.Data.ReadCString();
            string password = packet.Data.ReadCString();
            string macAddress = packet.Data.ReadCString();
            int unknown = packet.Data.ReadInt16();
            Logger.Info($"Account:{accountName} Password:{password} Unknown:{unknown}");

            // TODO authenticate from db
            Account account = new Account();
            account.Name = accountName;
            account.Id = Util.GetRandomNumber(10, 100000);
            Character character = new Character();
            character.Id = Util.GetRandomNumber(10, 100000);
            character.Name = accountName;
            //

            client.Account = account;
            client.Character = character;
            Server.ClientLookup.Add(client);


            IBuffer res = BufferProvider.Provide();
            //  0 = OK
            // 1 = ID or Pw to long
            // X unknown auth error, X
            res.WriteInt32(0);
            res.WriteInt32(1);

            Router.Send(client, (ushort) AuthPacketId.recv_base_authenticate_r, res);
        }
    }
}