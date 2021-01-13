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
            res.WriteInt64(100000000000);
            res.WriteByte(0);//bool
            Router.Send(client, (ushort)AreaPacketId.recv_self_exp_notify, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(10000);
            //Router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res, ServerType.Area);

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


        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "accs";
    }
}
