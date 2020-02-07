using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_union_close_window : ClientHandler
    {
        public send_union_close_window(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_union_close_window;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_union_close_window_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            uint cid = client.Character.InstanceId;
            res2.WriteInt32(cid);
            res2.WriteFixedString("unk", 0x31); //size is 0x31
            res2.WriteInt32(cid);
            res2.WriteInt32(cid);
            res2.WriteInt32(cid);
            res2.WriteInt32(cid);
            res2.WriteInt32(cid);
            res2.WriteInt32(cid);
            res2.WriteInt32(cid);
            res2.WriteByte(1);
            res2.WriteInt32(cid);
            res2.WriteInt32(cid);
            res2.WriteByte(2);
            res2.WriteByte(3);
            res2.WriteInt32(cid);
            res2.WriteInt16(69);
            res2.WriteFixedString("union", 0x196); //size is 0x196
            for (int i = 0; i < 8; i++)
                res2.WriteInt32(cid);
            res2.WriteByte(4);
            Router.Send(client.Map, (ushort)MsgPacketId.recv_union_notify_detail, res2, ServerType.Msg);

            IBuffer res3 = BufferProvider.Provide();
            //res3.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res3, ServerType.Area);
        }

    }
}
