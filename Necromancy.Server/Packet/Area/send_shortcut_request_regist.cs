using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_shortcut_request_regist : ClientHandler
    {
        public send_shortcut_request_regist(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_shortcut_request_regist;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            byte unknown1 = packet.Data.ReadByte(),
                 unknown2 = packet.Data.ReadByte();
            int skillSlot = packet.Data.ReadInt32();
            long skillID = packet.Data.ReadInt64();

            res.WriteByte(unknown1);
            res.WriteByte(unknown2);
            res.WriteInt32(skillSlot);
            res.WriteInt64(skillID);
            res.WriteFixedString("SkillName", 16);//size is 0x10
            Router.Send(client, (ushort) AreaPacketId.recv_shortcut_notify_regist, res);            
        }
    }
}