using Arrowgene.Services.Logging;
using System.Collections.Generic;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick item test commands.
    /// </summary>
    public class ItemCommand : ServerChatCommand
    {
        private readonly NecLogger _logger;
        private readonly NecServer _server;
        public ItemCommand(NecServer server) : base(server)
        {
            _server = server;
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            _logger.Debug($"Entering");
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
            }
            _logger.Debug($"command [0]");

            if (!int.TryParse(command[1], out int x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Please provide a value to test"));
            }
            if (!int.TryParse(command[2], out int y))
            {
                responses.Add(ChatResponse.CommandError(client, $"Please provide a value to test"));
            }

            
            switch (command[0])
            {
                case "dagger":
                    Item item = null;
                    if (y == 0)
                    {
                        item = SendItemInstanceUnidentified(client, 10200101, x);
                    }
                    else
                    {
                        item = SendItemInstance(client, "3130323030313031");
                    }
                    if (item == null)
                        return;
                    _logger.Debug($"dagger instanceId [{item.InstanceId}]");
                    break;
                case "healpot":
                    Item healItem = null;
                    if (y == 0)
                    {
                        healItem = SendItemInstanceUnidentified(client, 50100101, x);
                    }
                    else
                    {
                        healItem = SendItemInstance(client, "Test");
                    }
                    _logger.Debug($"dagger instanceId [{healItem.InstanceId}]");
                    break;
                case "create":
                    Item createItem = null;
                    if (y == 0)
                    {
                        createItem = SendItemInstanceUnidentified(client, x, y);
                    }
                    else
                    {
                        createItem = SendItemInstance(client, "Test");
                    }
                    _logger.Debug($"dagger instanceId [{createItem.InstanceId}]");
                    break;
                case "draw":
                    RecvPartyNotifyAddDrawItem itemMsg = new RecvPartyNotifyAddDrawItem((ulong)x, 30.0F, 0);
                    _server.Router.Send(itemMsg, client);
                    break;
                case "getitema":
                    IBuffer resa = BufferProvider.Provide();
                    //recv_normal_system_message = 0xAE2B,
                    resa.WriteInt32(x);
                    resa.WriteCString(command[3]);
                    resa.WriteByte((byte)y);
                    Router.Send(client, (ushort)AreaPacketId.recv_party_notify_get_item, resa, ServerType.Area);
                    break;
                case "getitemm":
                    IBuffer resm = BufferProvider.Provide();
                    //recv_normal_system_message = 0xAE2B,
                    resm.WriteInt32(x);
                    resm.WriteCString(command[3]);
                    resm.WriteByte((byte)y);
                    Router.Send(client, (ushort)MsgPacketId.recv_party_notify_get_item, resm, ServerType.Msg);
                    break;
                case "soulitem":
                    IBuffer res19 = BufferProvider.Provide();
                    res19.WriteInt32(Util.GetRandomNumber(62000001, 62000015)); //soul_dispitem.csv
                    Router.Send(client, (ushort)AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);
                    break;
                case "soulmat":
                    IBuffer res20 = BufferProvider.Provide();        
                    res20.WriteInt32(x);

                    res20.WriteFloat(client.Character.X);//X
                    res20.WriteFloat(client.Character.Y);//Y
                    res20.WriteFloat(client.Character.Z);//Z

                    res20.WriteFloat(client.Character.X+10);//X
                    res20.WriteFloat(client.Character.Y+10);//Y
                    res20.WriteFloat(client.Character.Z+10);//Z
                    res20.WriteByte(client.Character.Heading);

                    res20.WriteInt32(14);

                    res20.WriteInt32(10);
                    res20.WriteInt32(10);
                    res20.WriteInt32(1); // movement speed per tick?
                    if (y==1) Router.Send(client, (ushort)AreaPacketId.recv_data_notify_goldobject_data, res20, ServerType.Area);

                    res20.WriteInt32(11);
                    if (y == 2) Router.Send(client, (ushort)AreaPacketId.recv_data_notify_soulmaterialobject_data, res20, ServerType.Area);
                    if (y == 3) Router.Send(client, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res20, ServerType.Area);

                    break;


                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "iitem";
        public override string HelpText => "usage: `/iitem [command] [int]`";

        public Item SendItemInstanceUnidentified(NecClient client, int itemId, int count)
        {
            IBuffer res = null;
            InventoryItem invItem = client.Character.GetNextInventoryItem(_server);
            if (invItem == null)
            {
                res = BufferProvider.Provide();
                res.WriteInt32(-207);
                Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
                RecvNormalSystemMessage noSpace = new RecvNormalSystemMessage("Inventory is full!!!!");
                _server.Router.Send(noSpace, client);
                return null;
            }
            Item item = invItem.StorageItem = _server.Instances64.CreateInstance<Item>();
            Logger.Debug($"invItem.StorageId [{invItem.StorageId}] invItem.StorageSlot [{invItem.StorageSlot}]");
            item.Id = itemId;
            item.IconType = 2;
            item.Name = "dagger";
            invItem.StorageType = 0;
            invItem.StorageCount = (byte)count;
            res = null;
            res = BufferProvider.Provide();

            //res.WriteInt64(dropItem.Item.Id); //Item Object Instance ID 
            res.WriteInt64(invItem.InstanceId); //Item Object Instance ID 

            res.WriteCString("dagger"); //Name

            //res.WriteInt32(dropItem.Item.IconType); 
            res.WriteInt32(item.IconType); //item type

            res.WriteInt32(0);

            res.WriteByte(invItem.StorageCount); //Number of items

            res.WriteInt32(0); //Item status 0 = identified  

            res.WriteInt32(item.Id); //Item icon 50100301 = camp
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
            ConfigureItem(client, invItem.InstanceId);

            client.Character.inventoryItems.Add(invItem);
            return item;
        }

        public Item SendItemInstance(NecClient client, string key)
        {
            IBuffer res = BufferProvider.Provide();
            //Item item = _server.Instances64.CreateInstance<Item>();
            InventoryItem invItem = client.Character.GetNextInventoryItem(_server);
            if (invItem == null)
            {
                res = BufferProvider.Provide();
                res.WriteInt32(-207);
                Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
                RecvNormalSystemMessage noSpace = new RecvNormalSystemMessage("Inventory is full!!!!");
                _server.Router.Send(noSpace, client);
                return null;
            }
            Item item = invItem.StorageItem = _server.Instances64.CreateInstance<Item>();
            Logger.Debug($"invItem.StorageId [{invItem.StorageId}] invItem.StorageSlot [{invItem.StorageSlot}]");
            item.Id = 10200101;
            item.IconType = 2;
            item.Name = "dagger";
            invItem.StorageType = 0;
            invItem.StorageCount = (byte)1;

            uint instanceId = _server.Instances.CreateInstance<Model.Object>().InstanceId;
            Logger.Debug($"instanceId [{instanceId}]");
            //res.WriteInt32(instanceId); //InstanceId
            // res.WriteInt32(10200101); //ItemID
            res.WriteInt64(instanceId); //ItemID
            res.WriteInt32(item.IconType); // 0 does not display icon
            res.WriteByte((byte)1); //Number of "items"
            res.WriteInt32(0); //Item status, in multiples of numbers, 8 = blessed/cursed/both 
            res.WriteFixedString(key, 0x10);
            res.WriteByte(invItem.StorageType); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(invItem.StorageId); // 0~2 // maybe.. more bag index?
            res.WriteInt16(invItem.StorageSlot); // bag index
            res.WriteInt32(0); //Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)
            res.WriteInt32(0); //Percentage stat, 9 max i think
            res.WriteByte(0);
            res.WriteByte(0);  // Dest slot
            res.WriteCString("Dagger"); // find max size 
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt32(0); //Divides max % by this number
            res.WriteByte(0);
            res.WriteInt32(0);
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

            Router.Send(client, (ushort)AreaPacketId.recv_item_instance, res, ServerType.Area);

            ConfigureItem(client, item.InstanceId);
            return item;
        }

        public void ConfigureItem(NecClient client, ulong instanceId)
        {
            IBuffer res = BufferProvider.Provide();

            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_level, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt32(900);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_weight, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt16((short)8); // Defense and attack points
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_physics, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt32(10);
            //Router.Send(client, (ushort)AreaPacketId.recv_item_update_enchantid, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteByte(2);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_hardness, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt32(35);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_maxdur, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt32(35);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt16(0);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_magic, res, ServerType.Area);


            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt16((short)10000);
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res, ServerType.Area);
            
            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteInt16((short)10000); // Shwo GP on certain items
                                          //Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            //res.WriteInt32(instanceId);
            //res.WriteInt32(10800405);
            res.WriteInt64(instanceId); //Item Object ID 
            res.WriteByte(0);
            //Router.Send(client, (ushort)AreaPacketId.recv_item_update_sp_level, res, ServerType.Area);
        }
    }

}
