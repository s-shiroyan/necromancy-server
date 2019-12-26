using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvSkillStartCastR : PacketResponse
    {
        private readonly float _castingTime;

        public RecvSkillStartCastR(float castingTime)
            : base((ushort) AreaPacketId.recv_skill_start_cast_r, ServerType.Area)
        {
            _castingTime = castingTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 0 - success  
            res.WriteFloat(_castingTime);//Casting time (countdown before auto-cast)    ./Skill_base.csv   Column I             
            return res;
        }
    }
}
