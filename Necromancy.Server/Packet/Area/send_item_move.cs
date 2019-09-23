using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_move : Handler
    {
        public send_item_move(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_move;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int slotNum = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(00); //error check. 0 to work

            /*
                ITEMUSE	GENERIC	Unable to use this item right now
                ITEMUSE	-201	Store location is incorrect
                ITEMUSE	-204	Item amount is incorrect
                ITEMUSE	-205	The target to use this item is incorrect
                ITEMUSE	-206	Unable to use due to delay time
                ITEMUSE	-207	No space available in inventory
                ITEMUSE	-208	Unable to use this item since it is cursed
                ITEMUSE	-209	Unable to use this item since it is broken
                ITEMUSE	-210	You do not meet the requirements to equip this item
                ITEMUSE	-211	Unable to use this item
                ITEMUSE	-212	You are not in the right status to use this item
                ITEMUSE	-230	Unable to use since it is on cool down.
                ITEMUSE	-2601	You've already received this scrap
                ITEMUSE	-2708	Cannot be used outside of town
                ITEMUSE	-3001	Unable to move items when you have a shop open

            */

            Router.Send(client, (ushort) AreaPacketId.recv_item_move_r, res);
        }
    }
}