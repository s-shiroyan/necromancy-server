using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemTest : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly uint _value;
        private readonly ushort _opCode;
        private readonly uint _type;
        public RecvItemTest(ulong instanceId, ushort opCode, uint value,  uint type)
            : base((ushort)opCode, ServerType.Area)
        {
            _instanceId = instanceId;
            _value = value;
            _opCode = opCode;
            _type = type;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_instanceId);
            switch (_type)
            {
                case 0:
                    res.WriteByte((byte)_value);
                    break;
                case 1:
                    res.WriteInt16((ushort)_value);
                    break;
                case 2:
                    res.WriteInt32((uint)_value);
                    break;
                default:
                    break;
            }
            return res;
        }
    }
}
