using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvBaseSelectWorld : PacketResponse
    {
        private readonly NecSetting _necSetting;
        public RecvBaseSelectWorld(NecSetting necSetting)
            : base((ushort) AuthPacketId.recv_base_select_world_r, ServerType.Auth)
        {
            _necSetting = necSetting;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(_necSetting.DataMsgIpAddress);
            res.WriteInt32(_necSetting.MsgPort);
            return res;
        }
    }
}
