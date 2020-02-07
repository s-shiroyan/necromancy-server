using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvPartyNotifyAddDrawItem : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly float _timeout;
        private readonly int _iconId;
        public RecvPartyNotifyAddDrawItem(ulong instanceId, float timeout, int iconId)
            : base((ushort) AreaPacketId.recv_party_notify_add_draw_item, ServerType.Area)
        {
            _instanceId = instanceId;
            _timeout = timeout;
            _iconId = iconId;
        }
        // Item instance must be sent first
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_instanceId);    // InstanceId
            res.WriteFloat(_timeout);       // Timer in seconds
            res.WriteInt32(_iconId);        // A 0 here causes Join/Pass buttons

            return res;
        }
    }
}
