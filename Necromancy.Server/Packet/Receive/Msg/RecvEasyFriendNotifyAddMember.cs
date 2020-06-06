using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvEasyFriendNotifyAddMember : PacketResponse
    {
        public RecvEasyFriendNotifyAddMember()
            : base((ushort) MsgPacketId.recv_easy_friend_notify_add_member, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x31); //soul name?
            res.WriteFixedString("", 0x5B); //character name?
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteByte(0);//bool
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
