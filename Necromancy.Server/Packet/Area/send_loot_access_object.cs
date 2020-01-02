using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_loot_access_object : ClientHandler
    {
        private readonly NecLogger _logger;
        private readonly NecServer _server;

        public send_loot_access_object(NecServer server) : base(server)
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_loot_access_object;
        private short i = 0;
        public override void Handle(NecClient client, NecPacket packet)
        {
            int instanceID = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(instanceID);

            Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);

            MonsterSpawn monster = client.Map.GetMonsterByInstanceId((uint)instanceID);

            DropTables dropTable = new DropTables(_server);
            DropItem dropitem = dropTable.GetLoot(monster.MonsterId);
            _logger.Debug($"Loot is {dropitem.NumItems} of {dropitem.Item.Id}");
            if (dropitem.Item.Id == 1)
            {
                client.Character.AdventureBagGold += dropitem.NumItems;
                RecvSelfMoneyNotify addMoney = new RecvSelfMoneyNotify(client.Character.AdventureBagGold);
                Router.Send(addMoney, client);
            }
            else
            {
                res = null;
                res = BufferProvider.Provide();

                res.WriteInt64(dropitem.Item.Id); //Item Object Instance ID 

                res.WriteCString(""); //Name

                res.WriteInt32(dropitem.Item.IconType); //item type from itemType.csv

                res.WriteInt32(dropitem.Item.ItemType);

                res.WriteByte((byte)dropitem.NumItems); //Number of items

                res.WriteInt32(0); //Item status 0 = identified  

                res.WriteInt32(dropitem.Item.Id); //Item icon 50100301 = camp
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
                res.WriteByte(0); // 0~2
                res.WriteInt16(i); // bag index

                res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

                res.WriteInt64(i);

                res.WriteInt32(1);

                Router.Send(client, (ushort)AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);
            }

            RecvMonsterStateUpdateNotify monsterState = new RecvMonsterStateUpdateNotify((uint)instanceID, 1);
            Router.Send(client.Map, monsterState);

        }
    }
}
