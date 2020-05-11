using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyUnionData : PacketResponse
    {
        private readonly Character _character;
        private readonly string _unionName;

        public RecvDataNotifyUnionData(Character character, string unionName)
            : base((ushort) AreaPacketId.recv_chara_notify_union_data, ServerType.Area)
        {
            _character = character;
            _unionName = unionName;
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 19; //Max of 19 Equipment Slots for Character Player
            int numStatusEffects = 0x80; //Statuses effects. Max 128

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_character.InstanceId);
            res.WriteInt32(_character.unionId);
            res.WriteCString(_unionName);
            return res;
        }
    }
}
