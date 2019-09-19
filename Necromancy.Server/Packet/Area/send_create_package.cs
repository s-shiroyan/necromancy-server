using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_create_package : Handler
    {
        public send_create_package(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_create_package;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string recipient = packet.Data.ReadCString();
            string title = packet.Data.ReadCString();
            string content = packet.Data.ReadCString();
            byte unknownByte = packet.Data.ReadByte();
            int unknownInt = packet.Data.ReadInt32();
            long money = packet.Data.ReadInt64();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);//Failed to send message error if not 0

            Router.Send(client, (ushort) AreaPacketId.recv_create_package_r, res);

            SendPackageNotifyAdd(client, recipient, title, content, unknownByte, unknownInt, money);
        }

        private void SendPackageNotifyAdd(NecClient client, string recipient, string title, string content,
                                         byte unknownByte, int unknownInt, long money)
        {
            IBuffer res = BufferProvider.Provide();
            //recv_package_notify_add = 0x556E,

            res.WriteInt32(0);//Failed to send message error if not 0
            res.WriteInt32(client.Character.Id);
            res.WriteFixedString("unknown", 0x31);//Soul name
            res.WriteFixedString("master", 0x5B);//Character name sender?
            res.WriteFixedString($"{title}", 0x5B);//Title
            res.WriteFixedString($"{content}", 0x259);//Content
            res.WriteInt32(0);//Causes loop problem? (Number of "mail" to respond to?)
            res.WriteInt16(0);
            res.WriteInt64(0x1111111111111111);
            res.WriteInt32(121002);//Responsible for icon
            res.WriteFixedString("help", 0x49);//
            res.WriteFixedString("me", 0x49);//Item Title
            res.WriteInt32(121002);
            res.WriteInt32(121002);
            res.WriteInt32(121002);
            res.WriteInt32(121002);
            res.WriteFixedString("pls", 0x10);
            res.WriteByte(1);
            res.WriteInt32(121002);
            res.WriteInt32(121002);

            res.WriteByte(1);//bool
            res.WriteInt32(121002);
            res.WriteInt32(121002);
            res.WriteInt32(121002);

            res.WriteByte(0);//bool
            res.WriteInt32(121002);
            res.WriteInt32(121002);
            res.WriteInt32(121002);

            res.WriteByte(0);//bool
            res.WriteInt32(121002);
            res.WriteInt32(121002);
            res.WriteInt32(121002);

            res.WriteInt64(money);//Transfered money

            Router.Send(client, (ushort)AreaPacketId.recv_package_notify_add, res);
        }
    }
}