using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_search_recruited_party : ClientHandler
    {
        public send_party_search_recruited_party(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_search_recruited_party;

       public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.InstanceId + 1000);

            res.WriteInt32(0x14); // cmp to 0x14

            int numEntries = 0x14;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(5009);
                res.WriteInt32(5010);
                res.WriteInt32(5110);
                res.WriteInt32(5120);

                res.WriteInt32(0);

                res.WriteInt32(0);

                int numEntries2 = 0x4;
                for (int j = 0; j < numEntries2; j++)
                {
                    res.WriteInt32(1999); //Party ID?
                    res.WriteInt32(client.Character.InstanceId);
                    res.WriteFixedString($"{client.Soul.Name}{j}", 0x31);
                    res.WriteFixedString($"{client.Character.Name}{j}", 0x5B);
                    res.WriteInt32(client.Character.ClassId);
                    res.WriteByte(client.Character.Level);
                    res.WriteByte(0);
                    res.WriteByte(0); // bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                }

                res.WriteByte(0);

                res.WriteInt32(24);
                res.WriteInt32(2);
                res.WriteInt32(3);
                res.WriteByte(0);
                res.WriteInt32(0);

                res.WriteInt32(0);

                res.WriteFixedString($"PartyLoop{i}", 0xB5);
            }

            Router.Send(client, (ushort)AreaPacketId.recv_party_search_recruited_party_r, res, ServerType.Area);
        }
    }
}
