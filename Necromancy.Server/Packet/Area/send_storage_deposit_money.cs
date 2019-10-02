using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{

    public class send_storage_deposit_money : Handler
    {
        public send_storage_deposit_money(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_storage_deposit_money;

        public override void Handle(NecClient client, NecPacket packet)
        {


            int Balance, DepositMoney;

            Balance = 500;
            DepositMoney = 0;

            /* int Balance = packet.Data.ReadInt32();
            int DepositMoney = packet.Data.ReadInt32();
            Storage Money = new Storage();
             Money.Deposit = DepositMoney;
             Money.Balance = Balance; */


            if (Balance > DepositMoney)
            {
                Balance -= DepositMoney;
            }
            else
            {
                Console.WriteLine("you don't have enough money to deposit");

            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  // 0 to work
            Router.Send(client.Map, (ushort)AreaPacketId.recv_storage_deposit_money_r, res);
       


        IBuffer res2 = BufferProvider.Provide();
        res2.WriteInt64(Balance); // Get The money you withdraw in the storage
            Router.Send(client, (ushort) AreaPacketId.recv_self_money_notify, res2);

        }
    }
}