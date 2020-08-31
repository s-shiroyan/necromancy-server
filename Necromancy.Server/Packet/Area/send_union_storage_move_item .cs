using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_union_storage_move_item : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_union_storage_move_item));
        public send_union_storage_move_item(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_union_storage_move_item;
        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZone fromZone = (ItemZone) packet.Data.ReadByte();
            byte fromBag = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            ItemZone toZone = (ItemZone) packet.Data.ReadByte();
            byte toBag = packet.Data.ReadByte();
            short toSlot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();

            Logger.Debug($"fromStoreType byte [{fromZone}] toStoreType byte [{toZone}]");
            Logger.Debug($"fromBagId byte [{fromBag}] toBagId byte [{toBag}]");
            Logger.Debug($"fromSlot byte [{fromSlot}] toSlot [{toSlot}]");
            Logger.Debug($"itemCount [{quantity}]");

            ItemLocation fromLoc = new ItemLocation(fromZone, fromBag, fromSlot);
            ItemLocation toLoc = new ItemLocation(toZone, toBag, toSlot);
            ItemService itemService = new ItemService(client.Character);
            int error = 0;

            try
            {
                List<SpawnedItem> movedItems = itemService.Move(fromLoc, toLoc, quantity);
                PacketResponse pResp;
                if (movedItems.Count == 1) pResp = new RecvItemUpdatePlace(client, movedItems[0]);
                else pResp = new RecvItemUpdatePlaceChange(client, movedItems[0], movedItems[1]);
                Router.Send(pResp);
            }
            catch (ItemException e) { error = (int) e.ExceptionType; }

            RecvUnionStorageMoveItem recvUnionStorageMoveItem = new RecvUnionStorageMoveItem(client, error);
            Router.Send(recvUnionStorageMoveItem);
        }
    }
}
