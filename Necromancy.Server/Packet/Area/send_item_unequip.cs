using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_unequip : ClientHandler
    {
        public send_item_unequip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_unequip;
        int x;
        int y;
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

            Router.Send(client, (ushort) AreaPacketId.recv_item_unequip_r, res, ServerType.Area);
            SendCharaUnequipped(client);
            EQMask(client, y);
            y++;
        }

        private void SendCharaUnequipped(NecClient client)
        {
            x = 1;
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(-1);//itemIDs[x]);

            res.WriteInt32(-1);// itemIDs[x]);
            Router.Send(client, (ushort)AreaPacketId.recv_dbg_chara_unequipped, res, ServerType.Area);
        }

        int[] itemIDs = new int[]
       {
            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/, 210701 /*Torso*/,
            360103 /*Pants*/, 401201 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 160801 /*Avatar Head */,
            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 560801 /*Avatar Feet*/, 1, 2, 3
       };

        int[] EquipBitMask = new int[]
   {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
            1048576, 2097152
   };

        int[] EquipItemType = new int[]
            {9, 20, 23, 28, 31, 32, 36, 40, 41, 44, 43, 45, 42, 54, 61, 61, 61, 61, 61, 61, 0, 0};

        int[] EquipStatus = new int[] { 0, 1, 2, 4, 8, 16 };
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
            res13.WriteInt32(0); // Bitmask for location (0 to unequip)

            res13.WriteInt32(0); // List of items that gonna be equip on the chara
            res13.WriteByte(0); // ?? when you change this the armor dissapear, apparently
            res13.WriteByte(0);
            res13.WriteByte(0); //need to find the right number, permit to get the armor on the chara

            res13.WriteInt32(0); //previously 1
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
