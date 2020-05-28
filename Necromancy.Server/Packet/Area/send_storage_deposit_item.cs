using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_storage_deposit_item : ClientHandler
    {
        public send_storage_deposit_item(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_storage_deposit_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte fromStorageType = packet.Data.ReadByte();
            byte fromBagId = packet.Data.ReadByte();
            ushort fromBagSlot = packet.Data.ReadUInt16();
            byte toStorageType = packet.Data.ReadByte();
            byte toBagId = packet.Data.ReadByte();
            ushort toStorageSlot = packet.Data.ReadUInt16();
            byte itemStack = packet.Data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_storage_deposit_item2_r, res, ServerType.Area);

            SendItemPlace(client, toStorageType, toBagId, toStorageSlot);
        }

        private void SendItemPlace(NecClient client, byte toStoreType, byte toBagId, ushort toStorageSlot)
        {
       //     ulong instanceId = client.Character.GetInventoryItem(toStoreType, toBagId, (short)toStorageSlot).InstanceId;
       //     IBuffer res = BufferProvider.Provide();
       //     res.WriteUInt64(instanceId); // item id
       //     res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag, 3 = warehouse
       //     res.WriteByte(toBagId); // position 2	cause crash if you change the 0	]	} im assumming these are x/y row, and page
       //     res.WriteUInt16(toStorageSlot); // bag index 0 to 24
       //     Router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);
        }
    }
}
