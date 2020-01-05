using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvEventStart : PacketResponse
    {
        private readonly int _type;
        private readonly byte _unknown;
        public RecvEventStart(int type, byte unknown)
            : base((ushort) AreaPacketId.recv_event_start, ServerType.Area)
        {
            _type = type;
            _unknown = unknown;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_type); // 0 = normal 1 = cinematic
            res.WriteByte(_unknown);

            return res;
        }
    }
}
