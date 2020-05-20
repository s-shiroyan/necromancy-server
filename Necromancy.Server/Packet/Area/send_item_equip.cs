using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_equip : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_equip));

        public send_item_equip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_equip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte storageType = packet.Data.ReadByte();
            byte bagId = packet.Data.ReadByte(); //Equip slot maybe?
            short backpackSlot = packet.Data.ReadInt16(); //Slot from backpack the item is in
            int equipBit = packet.Data.ReadInt32();
            Logger.Debug(
                $"storageType: [{storageType}] bagId: [{bagId}]  backpackSlot: [{backpackSlot}] equipBit: [{equipBit}]");

            InventoryItem invItem = client.Character.GetInventoryItem(storageType, bagId, backpackSlot);

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);

            int slotNum = 0;

            for (int i = 0; i < 19; i++)
            {
                if (EquipBitMask[i] == invItem.StorageItem.bitmask)
                {
                    slotNum = i;
                    break;
                }
            }

            if (client.Character.equipSlots[slotNum] != null)
            {
                RecvItemUpdateEqMask eqMask = new RecvItemUpdateEqMask(client.Character.equipSlots[slotNum], 0); //used to unequip item if it is equpped before a switch
                Router.Send(eqMask, client);
            }

            client.Character.equipSlots[slotNum] = invItem;

            EQMask(client, invItem);
        }
        
        int[] EquipBitMask = new int[]
            {
                1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152
            };

        void EQMask(NecClient client, InventoryItem invItem)
        {
            IBuffer res13 = BufferProvider.Provide();
            res13.WriteUInt64(invItem.InstanceId);
            res13.WriteInt32(1); // Bitmask for location

            res13.WriteInt32(invItem.StorageItem.icon); // List of items that gonna be equip on the chara
            res13.WriteByte(0); // ?? when you change this the armor dissapear, apparently
            res13.WriteByte(0);
            res13.WriteByte(0); //need to find the right number, permit to get the armor on the chara

            res13.WriteInt32(invItem.StorageItem.icon);
            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0);

            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0); //bool
            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0);
            res13.WriteByte(0); // 1 = body pink texture
            res13.WriteByte(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_item_update_eqmask, res13, ServerType.Area);
        }
    }
}
