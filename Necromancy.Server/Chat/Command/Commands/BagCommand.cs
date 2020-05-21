using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick item test commands.
    /// </summary>
    public class BagCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(BagCommand));
        
        private readonly NecServer _server;

        public BagCommand(NecServer server) : base(server)
        {
            _server = server;
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            int.TryParse(command[0], out int x);
            int.TryParse(command[1], out int y);


            SendItemInstance(client);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "bag";
        public override string HelpText => "usage: `/bag [int] [int]`";

        public Item SendItemInstanceUnidentified(NecClient client)
        {
            IBuffer res = null;
            InventoryItem invItem = client.Character.GetNextInventoryItem(_server);
            if (invItem == null)
            {
                res = BufferProvider.Provide();
                res.WriteInt32(-207);
                Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
                RecvNormalSystemMessage noSpace = new RecvNormalSystemMessage("Inventory is full!!!!");
                _server.Router.Send(noSpace, client);
                return null;
            }

            Item item = invItem.StorageItem = _server.Instances.CreateInstance<Item>();
            Logger.Debug($"invItem.StorageId [{invItem.StorageId}] invItem.StorageSlot [{invItem.StorageSlot}]");
            item.Id = 50100501;
            item.IconType = 47;
            item.Name = "Bag (S)";
            invItem.StorageType = 0;
            res = null;
            res = BufferProvider.Provide();

            //res.WriteInt64(dropItem.Item.Id); //Item Object Instance ID 
            res.WriteUInt64(item.InstanceId); //Item Object Instance ID 

            res.WriteCString(item.Name); //Name

            //res.WriteInt32(dropItem.Item.IconType); 
            res.WriteInt32(item.IconType); //item type

            res.WriteInt32(0);

            res.WriteByte((byte) 1); //Number of items

            res.WriteInt32(0); //Item status 0 = identified  

            res.WriteUInt32(item.Id); //Item icon 50100301 = camp
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

            Router.Send(client, (ushort) AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);

            //client.Character.inventoryItems.Add(invItem);
            return item;
        }

        public Item SendItemInstance(NecClient client)
        {
            IBuffer res = null;
            InventoryItem invItem = client.Character.GetNextInventoryItem(_server);
            if (invItem == null)
            {
                res = BufferProvider.Provide();
                res.WriteInt32(-207);
                Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
                RecvNormalSystemMessage noSpace = new RecvNormalSystemMessage("Inventory is full!!!!");
                _server.Router.Send(noSpace, client);
                return null;
            }

            Item item = invItem.StorageItem = _server.Instances.CreateInstance<Item>();
            item.Id = 50100501;
            item.IconType = 47;
            item.Name = "bag";
            invItem.StorageType = 0;
            Logger.Debug($"instanceId [{invItem.InstanceId}]");
            res = BufferProvider.Provide();
            res.WriteUInt64(item.InstanceId); //ItemID
            res.WriteInt32(2); // 0 does not display icon
            res.WriteByte((byte) 1); //Number of "items"
            res.WriteInt32(0); //Item status, in multiples of numbers, 8 = blessed/cursed/both 
            res.WriteFixedString("Wolfzen", 0x10);
            res.WriteByte(invItem.StorageType); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(invItem.StorageId); // 0~2 // maybe.. more bag index?
            res.WriteInt16(invItem.StorageSlot); // bag index
            res.WriteInt32(0); //Equip bitmask
            res.WriteUInt32(item.Id); //Percentage stat, 9 max i think
            res.WriteByte(0);
            res.WriteByte(0); // Dest slot
            res.WriteCString(item.Name); // find max size 
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt32(item.IconType); //Divides max % by this number
            res.WriteByte(0);
            res.WriteInt32(item.IconType);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2

            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(0);
            //res.WriteInt32(0);

            numEntries = 3;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0); //Guard protection toggle, 1 = on, everything else is off
            res.WriteInt16(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);

            return item;
        }
    }
}
