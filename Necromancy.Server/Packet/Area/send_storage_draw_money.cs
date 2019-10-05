using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{

    public class send_storage_draw_money : Handler
    {

        public int DrawnMoney { get; set; }


        public send_storage_draw_money(NecServer server) : base(server)
        {

        }
        public override ushort Id => (ushort)AreaPacketId.send_storage_draw_money;

        public override void Handle(NecClient client, NecPacket packet)
        {


            int Balance, DrawMoney;

            Balance = 500;
            DrawMoney = 100;
           /* int Balance = packet.Data.ReadInt32();
            int DrawMoney = packet.Data.ReadInt32();
            Storage Money = new Storage();
            Money.Withdraw = DrawMoney;
            Money.Balance = Balance;
            */

            if (Balance != DrawMoney)
            {
                Console.WriteLine("you don't have enough money in your storage");
            }
            else
            { 
                Balance += DrawMoney;

            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); 
            Router.Send(client.Map, (ushort)AreaPacketId.recv_storage_drawmoney, res);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt64(Balance + DrawnMoney);  // Get The money you withdraw
            Router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res2);
        }

    }
}