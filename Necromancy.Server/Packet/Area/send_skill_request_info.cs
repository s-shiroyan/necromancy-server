using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_request_info : ClientHandler
    {
        public send_skill_request_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_request_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int[] skillTreeIds = null;
            if (client.Character.ClassId == 0)
            {
                skillTreeIds = fighterSkills;
            }
            else if (client.Character.ClassId == 1)
            {
                skillTreeIds = thiefSkills;
            }
            else if (client.Character.ClassId == 2)
            {
                skillTreeIds = mageSkills;
            }
            else if (client.Character.ClassId == 3)
            {
                skillTreeIds = priestSkills;
            }
            
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(skillTreeIds[0]);      // base skill id
            res.WriteInt32(1);          // skill level
            res.WriteByte(0);
            res.WriteByte(0);
            // No Response OP code
            Router.Send(client, (ushort)AreaPacketId.recv_skill_tree_notify, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            res.WriteInt32(skillTreeIds[1]);      // base skill id
            res.WriteInt32(1);          // skill level
            res.WriteByte(0);
            res.WriteByte(0);
            // No Response OP code
            Router.Send(client, (ushort)AreaPacketId.recv_skill_tree_notify, res, ServerType.Area);
            // Router.Send(client, (ushort) 0x0000, res, ServerType.Area);   

            if (client.Character.ClassId == 1)
            {
                res = null;
                res = BufferProvider.Provide();
                res.WriteInt32(skillTreeIds[2]);      // base skill id
                res.WriteInt32(1);          // skill level
                res.WriteByte(0);
                res.WriteByte(0);
                // No Response OP code
                Router.Send(client, (ushort)AreaPacketId.recv_skill_tree_notify, res, ServerType.Area);
            }
        }

        int[] thiefSkills = new int[] { 14101, 14302, 14803 };
        int[] fighterSkills = new int[] { 11101, 11201 };
        int[] mageSkills = new int[] { 13101, 13404 };
        int[] priestSkills = new int[] { 12501, 12601 };

    }
}
