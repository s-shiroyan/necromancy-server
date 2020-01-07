using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvEventMessage : PacketResponse
    {
        private readonly int _unknown;
        private readonly string _message;
        public RecvEventMessage(int unknown, string message)
            : base((ushort) AreaPacketId.recv_event_message, ServerType.Area)
        {
            _unknown = unknown;
            _message = message;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_unknown);
            res.WriteCString(_message);

            return res;
        }
    }
}
