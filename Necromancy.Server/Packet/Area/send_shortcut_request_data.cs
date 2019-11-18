using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_shortcut_request_data : ClientHandler
    {
        public send_shortcut_request_data(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_shortcut_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_shortcut_request_data_r, res, ServerType.Area);

            // Need to find definitions for different action types as Ids do overlap,  0 ?, 1 ?, 2 ? , 3 for skill_tree item, 4 for system , 5 for emote
            int[] shortcutBar0 = Database.SelectShortcutBarById(client.Character.shortcutBar0Id).getArray();
            int[] shortcutBar1 = Database.SelectShortcutBarById(client.Character.shortcutBar1Id).getArray();
            int[] shortcutBar2 = Database.SelectShortcutBarById(client.Character.shortcutBar2Id).getArray();
            int[] shortcutBar3 = Database.SelectShortcutBarById(client.Character.shortcutBar3Id).getArray();
            for (int i = 0; i < shortcutBar0.Length; i++)
            {
                if (shortcutBar0[i] > 0)
                {
                    IBuffer res0 = BufferProvider.Provide();
                    res0.WriteByte(0);
                    res0.WriteByte((byte)i);
                    if (shortcutBar0[i] > 100)   // Action type?    3 for skill_tree item, 4 for system , 5 for emote
                    {
                        res0.WriteInt32(3);
                    }
                    else
                    {
                        res0.WriteInt32(4);
                    }
                    res0.WriteInt64(shortcutBar0[i]);       // SkillId from skill_tree.csv for class skills
                    res0.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res0, ServerType.Area);
                }
            }
            for (int i = 0; i < shortcutBar1.Length; i++)
            {
                if (shortcutBar1[i] > 0)
                {
                    IBuffer res1 = BufferProvider.Provide();
                    res1.WriteByte(1);
                    res1.WriteByte((byte)i);
                    if (shortcutBar1[i] > 100)  // Action type?    3 for skill_tree item, 4 for system , 5 for emote
                    {
                        res1.WriteInt32(3);
                    }
                    else
                    {
                        res1.WriteInt32(5);
                    }
                    res1.WriteInt64(shortcutBar1[i]);
                    res1.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res1, ServerType.Area);
                }
            }
            // The following 2 bars are not complete, placeholder
            for (int i = 0; i < shortcutBar2.Length; i++)
            {
                if (shortcutBar2[i] > 0)
                {
                    IBuffer res1 = BufferProvider.Provide();
                    res1.WriteByte(1);
                    res1.WriteByte((byte)i);
                    res1.WriteInt32(3);
                    res1.WriteInt64(shortcutBar2[i]);
                    res1.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res1, ServerType.Area);
                }
            }
            for (int i = 0; i < shortcutBar3.Length; i++)
            {
                if (shortcutBar3[i] > 0)
                {
                    IBuffer res1 = BufferProvider.Provide();
                    res1.WriteByte(1);
                    res1.WriteByte((byte)i);
                    res1.WriteInt32(3);
                    res1.WriteInt64(shortcutBar3[i]);
                    res1.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res1, ServerType.Area);
                }
            }

        }
    }
}
