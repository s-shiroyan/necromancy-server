using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyAddMember : PacketResponse
    {
        private NecClient _client;
        public RecvPartyNotifyAddMember(NecClient client)
            : base((ushort) MsgPacketId.recv_party_notify_add_member, ServerType.Msg)
        {
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); //Party Add Reason?

            res.WriteUInt32(_client.Character.InstanceId); //Object ID?
            res.WriteUInt32(_client.Character.InstanceId);
            res.WriteFixedString($"{_client.Soul.Name}", 0x31); //Soul name
            res.WriteFixedString($"{_client.Character.Name}", 0x5B); //Character name
            res.WriteUInt32(_client.Character.ClassId); //Class
            res.WriteByte(_client.Soul.Level); //Soul rank
            res.WriteByte(_client.Character.Level); //Character level
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(Util.GetRandomNumber(1, 10));
            res.WriteInt32(0); //Might make the character selectable?
            res.WriteInt32(_client.Character.MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString($"Channel {_client.Character.Channel}",0x61); //Location of player if not in same zone
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteFloat(_client.Character.X);
            res.WriteFloat(_client.Character.Y);
            res.WriteFloat(_client.Character.Z);
            res.WriteByte(_client.Character.Heading);
            res.WriteByte(_client.Character.criminalState); //?
            res.WriteByte(1); //Beginner Protection (bool) ???
            res.WriteByte(1); //Membership Status???
            //res.WriteByte(1);
            //res.WriteByte(1);
            return res;
        }
    }
}
