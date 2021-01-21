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
    public class send_item_move : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_move));
        public send_item_move(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_item_move;
        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType) packet.Data.ReadByte();
            byte fromContainer = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            ItemZoneType toZone = (ItemZoneType) packet.Data.ReadByte();
            byte toContainer = packet.Data.ReadByte();
            short toSlot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();

            Logger.Debug($"fromStoreType byte [{fromZone}] toStoreType byte [{toZone}]");
            Logger.Debug($"fromContainerId byte [{fromContainer}] toBagId byte [{toContainer}]");
            Logger.Debug($"fromSlot byte [{fromSlot}] toSlot[{ toSlot}]");
            Logger.Debug($"itemCount [{quantity}]");

            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot);
            ItemLocation toLoc = new ItemLocation(toZone, toContainer, toSlot);
            ItemService itemService = new ItemService(client.Character);
            int error = 0;

            try
            {
                List<ItemInstance> movedItems = itemService.Move(fromLoc, toLoc, quantity, out ItemService.MoveType moveType);
                PacketResponse pResp;
                if (movedItems.Count == 1) pResp = new RecvItemUpdatePlace(client, movedItems[0]);
                else pResp = new RecvItemUpdatePlaceChange(client, movedItems[0], movedItems[1]);
                Router.Send(pResp);
            }
            catch (ItemException e) { error = (int) e.ExceptionType; }

            RecvItemMove recvItemMove = new RecvItemMove(client, error);
            Router.Send(recvItemMove);
        }        
    }
}
