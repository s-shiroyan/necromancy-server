using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model.ItemModel;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick item test commands.
    /// </summary>
    public class ItemCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemCommand));

        public ItemCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "iitem";
        public override string HelpText => "usage: `/iitem [itemId]`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, $"To few arguments"));
                return;
            }

            //100101
            if (!int.TryParse(command[0], out int itemId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            Item item = Server.Database.SelectItemById(itemId);
            if (item == null)
            {
                // Require Item to be in Database because we have a constraint.
                // Create Item
                // TODO use this code to initialize `nec_item` table

                if (!Server.SettingRepository.ItemNecromancy.TryGetValue(itemId, out ItemNecromancySetting necItem))
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"ItemId: {itemId} - not found in `SettingRepository.ItemNecromancy`"));
                    return;
                }

                if (!Server.SettingRepository.ItemInfo.TryGetValue(itemId, out ItemInfoSetting itemInfo))
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"ItemId: {itemId} - not found in `SettingRepository.ItemInfo`"));
                    return;
                }

                item = new Item();
                item.Id = itemInfo.Id;
                item.Name = itemInfo.Name;
                item.Durability = necItem.Durability;
                item.Physical = necItem.Physical;
                item.Magical = necItem.Magical;
                item.EquipmentSlotType = Item.EquipmentSlotTypeByItemId(itemInfo.Id);
                // TODO remove this when ItemType is defined in SettingRepository.ItemNecromancy
                item.ItemType = Item.ItemTypeByEquipmentSlotType(item.EquipmentSlotType);

                if (!Server.Database.InsertItem(item))
                {
                    responses.Add(ChatResponse.CommandError(client, "Could not save Item to Database"));
                    return;
                }
            }

            Character character = client.Character;
            if (character == null)
            {
                responses.Add(ChatResponse.CommandError(client, "Character is null"));
                return;
            }

            // Create InventoryItem
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Item = item;
            inventoryItem.ItemId = item.Id;
            inventoryItem.Quantity = 1;
            inventoryItem.CurrentDurability = item.Durability;
            inventoryItem.CharacterId = character.Id;

            if (!Server.Database.InsertInventoryItem(inventoryItem))
            {
                responses.Add(ChatResponse.CommandError(client, "Could not save InventoryItem to Database"));
                return;
            }

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt64((ulong) inventoryItem.Id); //ItemID
            res.WriteInt32(47); // 0 does not display icon
            res.WriteByte((byte) inventoryItem.Quantity); //Number of "items"
            res.WriteInt32(0); //Item status, in multiples of numbers, 8 = blessed/cursed/both 
            res.WriteFixedString(inventoryItem.Item.Name + "          ", 0x10);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2 // maybe.. more bag index?
            res.WriteInt16(0); // bag index
            res.WriteInt32(0); //Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)  /13
            res.WriteUInt32(9); //Percentage stat, 9 max i think       /12
            res.WriteByte(36); //1
            res.WriteByte(37); // Dest slot
            res.WriteCString(inventoryItem.Item.Name); // find max size     //11    10 byte fixed string
            res.WriteInt16(38); //10
            res.WriteInt16(39); //9
            res.WriteInt32(32); //Divides max % by this number     //8
            res.WriteByte(40); //7
            res.WriteInt32(33); //6

            int numEntries = 0;
            res.WriteInt32(numEntries); // less than or equal to 2
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(34 + i);
            }

            numEntries = 0;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(34); //4
            res.WriteInt32(35); //5
            res.WriteInt16(41);
            res.WriteInt32(43); //Guard protection toggle, 1 = on, everything else is off //3
            res.WriteInt16(42); //2

            Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);

            //ConfigureItem(client, item.InstanceId);
            //client.Character.inventoryItems.Add(invItem);


            //  Logger.Debug($"command [0]");
//
            //  int.TryParse(command[1], out int x);
            //  int.TryParse(command[2], out int y);
            //  int.TryParse(command[3], out int z);
//

            //   switch (command[0])
            //    {
            //    case "dagger":
            //        Item item = null;
            //        if (y == 0)
            //        {
            //            RecvCharaUpdateAlignment charAlign = new RecvCharaUpdateAlignment(1);
            //            _server.Router.Send(charAlign, client);
            //            item = SendItemInstanceUnidentified(client, 10200101, x, (int) ItemType.DAGGER, "Dagger");
            //        }
            //        else
            //        {
            //            item = SendItemInstance(client, 10200101, (int) ItemType.DAGGER, "Dagger");
            //        }

            //        if (item == null)
            //            return;
            //        Logger.Debug($"dagger instanceId [{item.InstanceId}]");
            //        break;
            //    case "healpot":
            //        Item healItem = null;
            //        if (y == 0)
            //        {
            //            healItem = SendItemInstanceUnidentified(client, 50100101, x, (int) ItemType.DRUG, "Heal Pot");
            //        }
            //        else
            //        {
            //            //healItem = SendItemInstance(client, "Test");
            //        }

            //        Logger.Debug($"dagger instanceId [{healItem.InstanceId}]");
            //        break;
            //    case "create":
            //        Item createItem = null;
            //        createItem = SendItemInstanceUnidentified(client, x, 1, y, "createItem");

            //        if (!Server.Database.InsertItem(createItem))
            //        {
            //            responses.Add(ChatResponse.CommandError(client,
            //                $"Item could not be added to the database"));
            //        }
            //        else
            //        {
            //            responses.Add(ChatResponse.CommandError(client,
            //                $"Item added to the database"));
            //        }

            //        Logger.Debug($"weapon instanceId [{createItem.InstanceId}]");
            //        break;
            //    case "draw":
            //        RecvPartyNotifyAddDrawItem itemMsg = new RecvPartyNotifyAddDrawItem((ulong) x, 30.0F, 0);
            //        _server.Router.Send(itemMsg, client);
            //        break;
            //    case "getitema":
            //        IBuffer resa = BufferProvider.Provide();
            //        //recv_normal_system_message = 0xAE2B,
            //        resa.WriteInt32(x);
            //        resa.WriteCString(command[3]);
            //        resa.WriteByte((byte) y);
            //        Router.Send(client, (ushort) AreaPacketId.recv_party_notify_get_item, resa, ServerType.Area);
            //        break;
            //    case "getitemm":
            //        IBuffer resm = BufferProvider.Provide();
            //        //recv_normal_system_message = 0xAE2B,
            //        resm.WriteInt32(x);
            //        resm.WriteCString(command[3]);
            //        resm.WriteByte((byte) y);
            //        Router.Send(client, (ushort) MsgPacketId.recv_party_notify_get_item, resm, ServerType.Msg);
            //        break;
            //    case "soulitem":
            //        IBuffer res19 = BufferProvider.Provide();
            //        res19.WriteInt32(Util.GetRandomNumber(62000001, 62000015)); //soul_dispitem.csv
            //        Router.Send(client, (ushort) AreaPacketId.recv_soul_dispitem_notify_data, res19, ServerType.Area);
            //        break;
            //    case "soulmat":
            //        for (int i = 0; i < x; i++)
            //        {
            //            IBuffer res20 = BufferProvider.Provide();
            //            res20.WriteInt32(Util.GetRandomNumber(998000, 1000000));

            //            res20.WriteFloat(client.Character.X); //X
            //            res20.WriteFloat(client.Character.Y); //Y
            //            res20.WriteFloat(client.Character.Z); //Z

            //            res20.WriteFloat(client.Character.X + Util.GetRandomNumber(-300, 300)); //X
            //            res20.WriteFloat(client.Character.Y + Util.GetRandomNumber(-300, 200)); //Y
            //            res20.WriteFloat(client.Character.Z + 10); //Z
            //            res20.WriteByte((byte) Util.GetRandomNumber(0, 255));

            //            res20.WriteInt32(Util.GetRandomNumber(0, 199999));

            //            res20.WriteInt32(Util.GetRandomNumber(0, 199999));
            //            res20.WriteInt32(Util.GetRandomNumber(0, 199999));
            //            res20.WriteInt32(Util.GetRandomNumber(0, 1000)); // bitmask  0bxxxxx1 = arch  0bxxxxx0 = no arch
            //            y = Util.GetRandomNumber(1, 4);
            //            if (y == 1)
            //                Router.Send(client, (ushort) AreaPacketId.recv_data_notify_goldobject_data, res20,
            //                    ServerType.Area);

            //            res20.WriteInt32(Util.GetRandomNumber(0, 199999));
            //            if (y == 2)
            //                Router.Send(client, (ushort) AreaPacketId.recv_data_notify_soulmaterialobject_data, res20,
            //                    ServerType.Area);
            //            if (y == 3)
            //                Router.Send(client, (ushort) AreaPacketId.recv_data_notify_itemobject_data, res20,
            //                    ServerType.Area);
            //        }

            //        break;
            //    case "physics":
            //        IBuffer res21 = BufferProvider.Provide();
            //        res21.WriteInt64(x); //item instance id
            //        res21.WriteInt16((short) y); //item's attack stat
            //        Router.Send(client, (ushort) AreaPacketId.recv_item_update_physics, res21, ServerType.Area);
            //        break;
            //    case "leatherguard":
            //    case "lg":
            //        if (y == 0)
            //        {
            //            item = SendItemInstanceUnidentified(client, 100101, x, (int) ItemType.HELMET, "Leather Guard");
            //        }
            //        else
            //        {
            //            item = SendItemInstance(client, 100101, (int) ItemType.HELMET, "Leather Guard");
            //        }

            //        if (item == null)
            //            return;
            //        Logger.Debug($"dagger instanceId [{item.InstanceId}]");
            //        break;
            //    case "rottenleathermail":
            //    case "rlm":
            //        if (x == 0)
            //        {
            //            item = SendItemInstanceUnidentified(client, 200110, 1, y, "Rotten Leather Mail");
            //        }
            //        else
            //        {
            //            item = SendItemInstance(client, 200110, (int) ItemType.COAT, "Rotten Leather Mail");
            //        }

            //        if (item == null)
            //            return;
            //        Logger.Debug($"dagger instanceId [{item.InstanceId}]");
            //        break;
            //    case "buff":
            //        Buff[] selfBuffs = new Buff[1];
            //        Buff testBuff = new Buff();
            //        testBuff.buffId = x;
            //        testBuff.unknown1 = y;
            //        testBuff.unknown2 = z;
            //        selfBuffs[0] = testBuff;
            //        RecvSelfBuffNotify selfBuff = new RecvSelfBuffNotify(selfBuffs);
            //        _server.Router.Send(selfBuff, client);
            //        break;
            //    case "itemt":
            //        IBuffer res = BufferProvider.Provide();
            //        res.WriteUInt64(invItem.InstanceId);
            //        res.WriteInt16((short) x);
            //        Router.Send(client, (ushort) AreaPacketId.recv_0x746F, res, ServerType.Area);
            //        break;
            //    case "testitem":
            //        Item item1 = SendItemInstanceUnidentified(client, 10200101, 1, (int) ItemType.DAGGER, "Dagger");
            //        RecvItemTest recvTest = new RecvItemTest((ulong) item1.InstanceId, (ushort) x, (uint) y, (uint) z);
            //        Router.Send(recvTest, client);
            //        break;
            //       default:
            //           Logger.Error($"There is no recv of type : {command[0]} ");
            //         break;
        }
    }


    //   public Item SendItemInstanceUnidentified(NecClient client, int itemId, int count, int itemType, string name)
    //   {
    //       IBuffer res = null;
    //       invItem = client.Character.GetNextInventoryItem(_server);
    //       if (invItem == null)
    //       {
    //           res = BufferProvider.Provide();
    //           res.WriteInt32(-207);
    //           Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
    //           RecvNormalSystemMessage noSpace = new RecvNormalSystemMessage("Inventory is full!!!!");
    //           _server.Router.Send(noSpace, client);
    //           return null;
    //       }

    //       Item item = invItem.StorageItem = _server.Instances.CreateInstance<Item>();
    //       Logger.Debug($"invItem.StorageId [{invItem.StorageId}] invItem.StorageSlot [{invItem.StorageSlot}]");

    //       if (itemId > 10100100 && itemId < 11700302)
    //           item.type = (byte) (itemId / 100000 % 100);
    //       else
    //           item.type = itemId;

    //       item.Id = (uint) itemId;
    //       item.icon = itemId;
    //       item.IconType = itemType;
    //       item.Name = name;
    //       item.name = name;
    //       invItem.StorageType = 0;
    //       invItem.StorageCount = (byte) count;
    //       item.count = invItem.StorageCount;
    //       item.bitmask = 1; //Fix the calculation for this 

    //       res = null;
    //       res = BufferProvider.Provide();

    //       res.WriteUInt64(invItem.InstanceId); //Item Object Instance ID 

    //       res.WriteCString(name); //Name

    //       res.WriteInt32(item.type); //item type

    //       res.WriteInt32(item.bitmask); //Bit mask designation

    //       res.WriteByte(invItem.StorageCount); //Number of items

    //       res.WriteInt32(0); //Item status 0 = identified  

    //       res.WriteInt32(item.icon); //Item icon 50100301 = camp
    //       res.WriteByte(0);
    //       res.WriteByte(0);
    //       res.WriteByte(0);
    //       res.WriteInt32(item.icon);
    //       res.WriteByte(0);
    //       res.WriteByte(0);
    //       res.WriteByte(0);

    //       res.WriteByte(0);
    //       res.WriteByte(0);
    //       res.WriteByte(0); // bool
    //       res.WriteByte(0);
    //       res.WriteByte(0);
    //       res.WriteByte(0);
    //       res.WriteByte(0);
    //       res.WriteByte(0);

    //       res.WriteByte(invItem.StorageType); // 0 = adventure bag. 1 = character equipment
    //       res.WriteByte(invItem.StorageId); // 0~2
    //       res.WriteInt16(invItem.StorageSlot); // bag index

    //       res.WriteInt32(0); //bit mask. This indicates where to put items.   e.g. 01 head 010 arm 0100 feet etc (0 for not equipped)

    //       res.WriteInt64(69);

    //       res.WriteInt32(59);

    //       Router.Send(client, (ushort) AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);
    //       ConfigureItem(client, invItem.InstanceId, item);

    //       //client.Character.inventoryItems.Add(invItem);
    //       //client.Character.EquipId[0] = 10200101;
    //       //RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name,false);
    //       //Router.Send(myCharacterData, client);

    //       //UpdateEqMask(client);
    //       return item;
    //   }

    //  public Item SendItemInstance(NecClient client, int itemId, int itemType, string name)
    //  {
    //      IBuffer res = BufferProvider.Provide();
    //      //Item item = _server.Instances64.CreateInstance<Item>();
    //      invItem = client.Character.GetNextInventoryItem(_server);
    //      if (invItem == null)
    //      {
    //          res = BufferProvider.Provide();
    //          res.WriteInt32(-207);
    //          Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
    //          RecvNormalSystemMessage noSpace = new RecvNormalSystemMessage("Inventory is full!!!!");
    //          _server.Router.Send(noSpace, client);
    //          return null;
    //      }

    //      Item item = invItem.StorageItem = _server.Instances.CreateInstance<Item>();
    //      Logger.Debug($"invItem.StorageId [{invItem.StorageId}] invItem.StorageSlot [{invItem.StorageSlot}]");
    //      item.Id = (uint) itemId;
    //      item.IconType = itemType;
    //      item.Name = name;
    //      invItem.StorageType = 0;
    //      invItem.StorageCount = (byte) 1;

    //      //ulong instanceId = invItem.InstanceId << 32 | 0xffffffff;
    //      Logger.Debug($"instanceId [{invItem.InstanceId}]");
    //      //res.WriteInt32(instanceId); //InstanceId
    //      // res.WriteInt32(10200101); //ItemID
    //      res.WriteUInt64(invItem.InstanceId); //ItemID
    //      res.WriteInt32(invItem.StorageItem.IconType); // 0 does not display icon
    //      res.WriteByte((byte) 1); //Number of "items"
    //      res.WriteInt32(0); //Item status, in multiples of numbers, 8 = blessed/cursed/both 
    //      res.WriteFixedString(name + "          ", 0x10);
    //      res.WriteByte(invItem.StorageType); // 0 = adventure bag. 1 = character equipment
    //      res.WriteByte(invItem.StorageId); // 0~2 // maybe.. more bag index?
    //      res.WriteInt16(invItem.StorageSlot); // bag index
    //      res.WriteInt32(0); //Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)                          /13
    //      res.WriteUInt32(invItem.StorageItem
    //          .Id); //Percentage stat, 9 max i think                                                                        /12
    //      res.WriteByte(36); //1
    //      res.WriteByte(37); // Dest slot
    //      res.WriteCString(
    //          name); // find max size                                                                                                        //11    10 byte fixed string
    //      res.WriteInt16(38); //10
    //      res.WriteInt16(39); //9
    //      res.WriteInt32(
    //          32); //Divides max % by this number                                                                                              //8
    //      res.WriteByte(40); //7
    //      res.WriteInt32(33); //6
    //      int numEntries = 0;
    //      res.WriteInt32(numEntries); // less than or equal to 2

    //      for (int i = 0; i < numEntries; i++)
    //          res.WriteInt32(34 + i);
    //      //res.WriteInt32(0);

    //      numEntries = 0;
    //      res.WriteInt32(numEntries); // less than or equal to 3
    //      for (int i = 0; i < numEntries; i++)
    //      {
    //          res.WriteByte(0); //bool
    //          res.WriteInt32(0);
    //          res.WriteInt32(0);
    //          res.WriteInt32(0);
    //      }

    //      res.WriteInt32(34); //4
    //      res.WriteInt32(35); //5
    //      res.WriteInt16(41);
    //      res.WriteInt32(
    //          43); //Guard protection toggle, 1 = on, everything else is off                                                                   //3
    //      res.WriteInt16(42); //2

    //      Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);

    //      //ConfigureItem(client, item.InstanceId);
    //      //client.Character.inventoryItems.Add(invItem);
    //      return invItem.StorageItem;
    //  }

    //     public void UpdateEqMask(NecClient client, InventoryItem invItem)
    //     {
    //         //Item item = invItem.StorageItem;
    //         RecvItemUpdateEqMask eqMask = new RecvItemUpdateEqMask(invItem, invItem.StorageItem.bitmask);
    //         Router.Send(eqMask, client);
    //     }
//
    //     public void UpdateState(NecClient client, InventoryItem invItem, uint state)
    //     {
    //         IBuffer res = BufferProvider.Provide();
//
    //         res = BufferProvider.Provide();
    //         res.WriteUInt64(invItem
    //             .InstanceId); //client.Character.EquipId[x]   put stuff unidentified and get the status equipped  , 0 put stuff identified
    //         res.WriteUInt32(state);
    //         Router.Send(client, (ushort) AreaPacketId.recv_item_update_state, res, ServerType.Area);
    //     }

    //  public void ConfigureItem(NecClient client, ulong instanceId, Item item)
    //  {
    //      item.ac = (short) Util.GetRandomNumber(0, 100000);
    //      item.durability = Util.GetRandomNumber(1, 200);
    //      item.maxDurability = Util.GetRandomNumber(199, 200);
    //      item.level = (byte) Util.GetRandomNumber(0, 5);
    //      item.weight = Util.GetRandomNumber(800, 10000);
    //      item.physics = (short) Util.GetRandomNumber(5, 500);
    //      item.magic = (short) Util.GetRandomNumber(5, 500);
    //      item.enchatId = Util.GetRandomNumber(1, 10);
    //      item.dateEndProtect = Util.GetRandomNumber(1, 50);
    //      item.hardness = (byte) Util.GetRandomNumber(0, 100);
    //      item.state = 0;
//
    //      IBuffer res = BufferProvider.Provide();
//
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteByte(item.level);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_level, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt32(item.weight);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_weight, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt16(item.physics); // Defense and attack points
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_physics, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt32(item.enchatId);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_enchantid, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteByte(item.hardness);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_hardness, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt32(item.maxDurability);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_maxdur, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt32(item.durability);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_durability, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt16(item.magic);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_magic, res, ServerType.Area);
//
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt16(item.ac);
    //      Router.Send(client, (ushort) AreaPacketId.recv_item_update_ac, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteInt16((short) 10000); // Shwo GP on certain items
    //      //Router.Send(client, (ushort)AreaPacketId.recv_item_update_ac, res, ServerType.Area);
//
    //      res = null;
    //      res = BufferProvider.Provide();
    //      //res.WriteInt32(instanceId);
    //      //res.WriteInt32(10800405);
    //      res.WriteUInt64(instanceId); //Item Object ID 
    //      res.WriteByte(0);
    //      //Router.Send(client, (ushort)AreaPacketId.recv_item_update_sp_level, res, ServerType.Area);
    //  }
}
