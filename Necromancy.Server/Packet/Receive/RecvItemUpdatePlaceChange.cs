using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemUpdatePlaceChange : PacketResponse
    {
        private readonly int _instanceId;
        private readonly byte _level;
        public RecvItemUpdatePlaceChange(int instanceId, byte level)
            : base((ushort) AreaPacketId.recv_item_update_place_change, ServerType.Area)
        {
            _instanceId = instanceId;
            _level = level;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId); // 0 = normal 1 = cinematic
            res.WriteByte(_level);

            return res;
        }
    }
}
