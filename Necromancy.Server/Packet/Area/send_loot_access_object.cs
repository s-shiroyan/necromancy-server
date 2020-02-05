using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using System;
using System.Threading.Tasks;

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

        public override ushort Id => (ushort)AreaPacketId.send_loot_access_object;
        private short i = 0;
        public override void Handle(NecClient client, NecPacket packet)
        {
            int instanceID = packet.Data.ReadInt32();
            _logger.Debug($"{client.Character.Name} is {client.Character.Alignmentid}");


            IBuffer res = null;
            //res2.WriteInt32(instanceID);

            //Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res2, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(instanceID);

            Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);

            MonsterSpawn monster = client.Map.GetMonsterByInstanceId((uint)instanceID);

            DropTables dropTable = new DropTables(_server);
            DropItem dropItem = dropTable.GetLoot(monster.MonsterId);
            if (dropItem == null)
                return;
            InventoryItem invItem = client.Character.GetNextInventoryItem(_server, (byte)dropItem.NumItems, dropItem.Item);
            if (invItem == null)
            {
                res = BufferProvider.Provide();
                res.WriteInt32(-207);
                Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);

                RecvNormalSystemMessage noSpace = new RecvNormalSystemMessage("Inventory is full!!!!");
                _server.Router.Send(noSpace, client);
                return;
            }

            string lootMsg = $"Looted {dropItem.NumItems} {dropItem.Item.Name} from {monster.Name}.";
            _logger.Debug($"Loot is {dropItem.NumItems} of {dropItem.Item.Id}");

            if (dropItem.Item.Id == 1)
            {
                client.Character.AdventureBagGold += dropItem.NumItems;
                RecvSelfMoneyNotify addMoney = new RecvSelfMoneyNotify(client.Character.AdventureBagGold);
                Router.Send(addMoney, client);
            }
            else
            {
                res = null;
                res = BufferProvider.Provide();

                //res.WriteInt64(dropItem.Item.Id); //Item Object Instance ID 
                res.WriteInt64(dropItem.Item.InstanceId); //Item Object Instance ID 

                res.WriteCString(dropItem.Item.Name); //Name

                //res.WriteInt32(dropItem.Item.IconType); 
                res.WriteInt32(dropItem.Item.IconType); //item type

                res.WriteInt32(0);

                res.WriteByte((byte)dropItem.NumItems); //Number of items

                res.WriteInt32(0); //Item status 0 = identified  

                res.WriteInt32(dropItem.Item.Id); //Item icon 50100301 = camp
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(1); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(invItem.StorageType); // 0 = adventure bag. 1 = character equipment
                res.WriteByte(invItem.StorageId); // 0~2
                res.WriteInt16(invItem.StorageSlot); // bag index
                res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

                res.WriteInt64(0);

                res.WriteInt32(0);

                Router.Send(client, (ushort)AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);


            }

            RecvMonsterStateUpdateNotify monsterState = new RecvMonsterStateUpdateNotify((uint)instanceID, 1);
            Router.Send(client.Map, monsterState);

            if (dropItem.Item.Id != 1)
            {
                //RecvPartyNotifyGetItem itemMsg = new RecvPartyNotifyGetItem(dropItem.Item.Id, dropItem.Item.Name, (byte)dropItem.NumItems);

                RecvPartyNotifyAddDrawItem itemMsg = new RecvPartyNotifyAddDrawItem(dropItem.Item.InstanceId, 30.0F, 0);
                _server.Router.Send(itemMsg, client);
            }
        }
    }
}
