using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //recv loot access object?  doesnt do anything
    public class SendLootAccessObject : ServerChatCommand
    {
        public SendLootAccessObject(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(999999999);
            res.WriteByte(0);//bool
            //Router.Send(client, (ushort)AreaPacketId.recv_self_exp_notify, res, ServerType.Area);

           

             res = BufferProvider.Provide();
            res.WriteInt32(1);
            //Router.Send(client, (ushort)AreaPacketId.recv_self_soul_point_notify, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(100101);
            res.WriteCString("Derp");
            res.WriteByte(1);
            //Router.Send(client, (ushort)MsgPacketId.recv_party_notify_get_money, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(5);
            res.WriteInt64(500);
            //Router.Send(client, (ushort)MsgPacketId.recv_party_notify_get_money, res, ServerType.Area);

             res = BufferProvider.Provide();
            res.WriteInt16(1);  //item row id
            res.WriteInt64(10001); //item instance ID
            res.WriteInt64(10001); //item sale price
            //Router.Send(client, (ushort)AreaPacketId.recv_0x94B9, res, ServerType.Area);

            res = BufferProvider.Provide();
            int numEntries = 0xA;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt16(1);
                res.WriteInt64(10001);
                res.WriteFixedString("test", 0xC1);
            }
            //Router.Send(client, (ushort)AreaPacketId.recv_0xBA61, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(100101);
            res.WriteCString("ToBeFound");
            res.WriteByte(1);
            //Router.Send(client, (ushort)AreaPacketId.recv_party_notify_get_item, res, ServerType.Area);
            //IBuffer res = BufferProvider.Provide();
            //res.WriteInt32(-10);
            /*
                LOOT	-1	It is carrying nothing
                LOOT	-10	No one can be looted nearby
                LOOT	-207	No space available in inventory
                LOOT	-1500	No permission to loot
            */

            res = BufferProvider.Provide();
            res.WriteInt64(10001);
            res.WriteInt64(10002);
            res.WriteInt64(10003);
            res.WriteInt64(10004);
            res.WriteInt64(10005);
            //Router.Send(client, (ushort)AreaPacketId.recv_0xFDCB, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(100101);
            res.WriteByte(1);
            //Router.Send(client, (ushort)AreaPacketId.recv_0xEEB7, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(100101);
            res.WriteCString("helo"); //0x31
            res.WriteCString("derp"); //0x5B
            res.WriteByte(2);
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            //Router.Send(client, (ushort)AreaPacketId.recv_0x9CA1, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(100101);
            res.WriteByte(1);
            //Router.Send(client, (ushort)AreaPacketId.recv_0x7B86, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            //Router.Send(client, (ushort)AreaPacketId.recv_0x7697, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(100101);
            res.WriteInt32(100101);
            //Router.Send(client, (ushort)AreaPacketId.recv_0x755C, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(2);
            res.WriteFloat(5);
            res.WriteCString("halp");//0x31
            res.WriteCString("derp");//0x5B	
            //Router.Send(client, (ushort)AreaPacketId.recv_0x531B, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt16(1);
            res.WriteUInt32(5);
            res.WriteInt64(10001);
            res.WriteInt64(10001);
            res.WriteInt64(10001);
            res.WriteByte(1);
            res.WriteFixedString("Test", 0x10);
            res.WriteInt32(23);
            res.WriteInt16(1);
            //Router.Send(client, (ushort)AreaPacketId.recv_0x5418, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(2);
            Router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);
            res = BufferProvider.Provide();
            res.WriteInt64(90000000);
            Router.Send(client, (ushort)AreaPacketId.recv_self_bonus_money_notify, res, ServerType.Area);
            res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "accs";
    }
}
