using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_unequip : Handler
    {
        public send_item_unequip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_unequip;
        int x;
        public override void Handle(NecClient client, NecPacket packet)
        {
            int slotNum = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //error check. 0 to work

            /*
            ERR_UNEQUIP 1
            ERR_UNEQUIP - 203
            ERR_UNEQUIP - 201
            ERR_UNEQUIP - 208
            ERR_UNEQUIP GENERIC
            */

            Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res);
            SendCharaUnequipped(client);
        }

        private void SendCharaUnequipped(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();


            x = 0;

            res.WriteInt32(EquipBitMask[x]);

            res.WriteInt32(EquipBitMask[x]);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_dbg_chara_unequipped, res, client);

        }

        int[] itemIDs = new int[] {10800405/*Weapon*/,15100901/*Shield* */,20000101/*Arrow*/,110301/*head*/,210701/*Torso*/,360103/*Pants*/,401201/*Hands*/,560103/*Feet*/,690101/*Cape*/
                    ,30300101/*Necklace*/,30200107/*Earring*/,30400105/*Belt*/,30100106/*Ring*/,70000101/*Talk Ring*/,160801/*Avatar Head */,260801/*Avatar Torso*/,360801/*Avatar Pants*/,460801/*Avatar Hands*/,560801/*Avatar Feet*/ };
        int[] NPCModelID = new int[] { 1911105, 1112101, 1122401, 1122101, 1311102, 1111301, 1121401, 1131401, 2073002, 1421101 };
        int[] NPCSerialID = new int[] { 10000101, 10000102, 10000103, 10000104, 10000105, 10000106, 10000107, 10000108, 80000009, 10000101 };
        //int[] EquipBitMask = new int[] { 0b1, 0b10, 0b100, 0b1000, 0b10000, 0b100000, 0b1000000, 0b10000000, 0b100000000, 0b1000000000, 0b10000000000, 0b100000000000, 0b1000000000000, 0b10000000000000, 0b100000000000000, 0b10000000000000000, 0b10000000000000000, 0b1000000000000000000, 0b10000000000000000000 };
        int[] EquipBitMask = new int[] { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152 };
        int[] EquipItemType = new int[] { 9, 21, 23, 28, 31, 32, 36, 40, 41, 44, 43, 45, 42, 54, 62, 62, 62, 62, 62, 62, 62, 62 };
        int[] EquipStatus = new int[] { 0, 1, 2, 4, 8, 16 };

    }
}