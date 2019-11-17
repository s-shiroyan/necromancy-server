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
            byte skillBar = packet.Data.ReadByte(),
                 skillSlot = packet.Data.ReadByte();
            int unknown1 = packet.Data.ReadInt32();
            long skillID = packet.Data.ReadInt64();
            Logger.Debug(client, $"skillBar {skillBar} skillSlot {skillSlot} unknown1 {unknown1} skillID {skillID}");
            res.WriteByte(skillBar);
            res.WriteByte(skillSlot);
            res.WriteInt32(unknown1);
            res.WriteInt64(skillID);
            res.WriteFixedString("SkillName", 16);//size is 0x10

            Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res, ServerType.Area);
        }
    }
}
