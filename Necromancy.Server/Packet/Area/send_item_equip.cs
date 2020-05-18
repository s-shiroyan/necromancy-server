using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

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
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);

            //InventoryItem invItem = client.Character.GetInventoryItem(storageType, bagId, backpackSlot);
            //RecvItemUpdateEqMask eqMask = new RecvItemUpdateEqMask(invItem.StorageItem.InstanceId);
            //Router.Send(eqMask, client);

            EQMask(client, 0);
        }

        void EQMask(NecClient client, int x)
        {
            IBuffer res13 = BufferProvider.Provide();
            //95 torso ?
            //55 full armor too ?
            //93 full armor ?
            // 27 full armor ?
            //11 under ?
            // 38 = boots and cape
            //byte y = unchecked((byte)110111);
            //byte y = unchecked ((byte)Util.GetRandomNumber(0, 100)); // for the moment i only get the armor on this way :/

            res13.WriteInt64(10200101);
            res13.WriteInt32(8); // Permit to get the armor on the chara

            res13.WriteInt32(180202); // List of items that gonna be equip on the chara
            res13.WriteByte(0); // ?? when you change this the armor dissapear, apparently
            res13.WriteByte(0);
            res13.WriteByte(0); //need to find the right number, permit to get the armor on the chara

            res13.WriteInt32(1);
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
