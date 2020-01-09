using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Numerics;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvCharaNotifyStateflag : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly int _state;
        public RecvCharaNotifyStateflag(uint instanceId, int state)
            : base((ushort) AreaPacketId.recv_chara_notify_stateflag, ServerType.Area)
        {
            _instanceId = instanceId;
            _state = state;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId);
            res.WriteInt32(_state);
            return res;
        }
    }
}
