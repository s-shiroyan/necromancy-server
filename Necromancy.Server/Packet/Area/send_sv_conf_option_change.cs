using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_sv_conf_option_change : Handler
    {
        public send_sv_conf_option_change(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_sv_conf_option_change;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Success?
            Router.Send(client, (ushort)AreaPacketId.recv_sv_conf_option_change, res);

            //SendMapChangeForce(client);
            //SendDataNotifyCharabodyData(client);
        }

        private void SendMapChangeForce(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(1001001);
            res.WriteInt32(1001001);
            res.WriteFixedString("127.0.0.1", 65);//IP
            res.WriteInt16(60002);//Port

            res.WriteFloat(100);//x coord
            res.WriteFloat(100);//y coord
            res.WriteFloat(100);//z coord
            res.WriteByte(1);//view offset maybe?

            Router.Send(client, (ushort)AreaPacketId.recv_map_change_force, res);
        }

        private void SendDataNotifyCharabodyData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(1);
            res.WriteInt32(1);

            res.WriteCString("charaname");
            res.WriteCString("soulname");

            res.WriteFloat(1);
            res.WriteFloat(2);
            res.WriteFloat(3);
            res.WriteByte(1);

            res.WriteInt32(1);

            int numEntries = 19;
            res.WriteInt32(numEntries);

            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(1);

            numEntries = 19;
            res.WriteInt32(numEntries);

            for(int i = 0; i < numEntries; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    res.WriteInt32(1);
                    res.WriteByte(1);
                    res.WriteByte(1);
                    res.WriteByte(1);
                }
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);//Bool
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
            }

            numEntries = 19;
            res.WriteInt32(numEntries);

            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(1);

            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteByte(1);
            res.WriteByte(1);
            res.WriteByte(1);

            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteByte(1);
            res.WriteByte(1);//Bool
            res.WriteInt32(1);

            Router.Send(client, (ushort)AreaPacketId.recv_data_notify_charabody_data, res);
        }
    }
}