using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens your shop
    public class SendShopNotifyOpen : ServerChatCommand
    {
        public SendShopNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res0 = BufferProvider.Provide();
            res0.WriteInt16(2); //shop mode
            /* Shop ID, 0 it's forge, 1 it's cursed, 2 Purchase shop, 3 purchase and curse, 4 it's sell, 
                        5 sell and curse. 6 purchase and sell. 7 Purchase, Sell, Curse.
                        
                        8 Identify. 9 identify & curse. 10 Purchase & Identify.
                        
                        11 Purchase, Identify & Curse. 12 Sell And Identify
                        
                        13 Sell, Identify & Curse, 14 Purchase, Sell & Identify
                        
                        15 All of what i say before except the forge.
                        
                        16 Repair ! 17 repair and curse. 18 Repair and purchase;
                        
                        19 repair, purchase, cursed. 20 Repair and sell
           */
            res0.WriteInt32(1); // item number
            res0.WriteInt32(10200101); // don't know too
            res0.WriteByte(0); // 0 = shop open, 1 = shop not open ?
            Router.Send(client, (ushort) AreaPacketId.recv_shop_notify_open, res0, ServerType.Area);

            IBuffer res = BufferProvider.Provide();
            res.WriteByte(1); //idx
            res.WriteUInt32(10200101); // item Serial id
            res.WriteInt64(10200101); // item price
            res.WriteFixedString("Dagger", 0x10); // ?
            //Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item, res, ServerType.Area);

            IBuffer res1 = BufferProvider.Provide();
            res1.WriteCString("GnomeBoobs");
            Router.Send(client, (ushort)AreaPacketId.recv_shop_title_push, res1, ServerType.Area);


            res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteUInt32(client.Character.InstanceId);

            int numEntries = 0xA;
            res.WriteInt32(0xA);//<a
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt16((short)i);
                int numEntries2 = 0xC1;
                res.WriteInt64(numEntries2);
                for (int j = 0; j < numEntries2; j++)
                {
                    res.WriteByte(1);
                }
            }
            Router.Send(client, (ushort)0x4978, res, ServerType.Area);

            res = BufferProvider.Provide();

            int numEntries3 = 0xA;
            res.WriteInt32(numEntries3);
            for (int i = 0; i < numEntries3; i++)
            {
                res.WriteInt16((short)i);
                res.WriteInt64(10500501);
                res.WriteFixedString("UNKNOWN", 0xC1);
            }

            Router.Send(client, (ushort)0xBA61, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "shop";
    }
}
