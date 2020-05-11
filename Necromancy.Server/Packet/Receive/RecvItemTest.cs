using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvItemTest : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly uint _value;
        private readonly ushort _opCode;
        private readonly uint _type;

        public RecvItemTest(ulong instanceId, ushort opCode, uint value, uint type)
            : base((ushort) opCode, ServerType.Area)
        {
            _instanceId = instanceId;
            _value = value;
            _opCode = opCode;
            _type = type;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_instanceId);
            switch (_type)
            {
                case 0:
                    res.WriteByte((byte) _value);
                    break;
                case 1:
                    res.WriteUInt16((ushort) _value);
                    break;
                case 2:
                    res.WriteUInt32((uint) _value);
                    break;
                default:
                    break;
            }

            return res;
        }
    }
}
