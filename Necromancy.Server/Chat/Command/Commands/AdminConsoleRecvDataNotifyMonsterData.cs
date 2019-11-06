using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //Spawns a monster near you
    public class AdminConsoleRecvDataNotifyMonsterData : ServerChatCommand
    {
        public AdminConsoleRecvDataNotifyMonsterData(NecServer server) : base(server)
        {
        }

        int[] itemIDs = new int[]
        {
            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/, 210701 /*Torso*/,
            360103 /*Pants*/, 401201 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 160801 /*Avatar Head */,
            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 560801 /*Avatar Feet*/, 1, 2, 3
        };

        private int x = 0;

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            int MonsterUniqueId = Util.GetRandomNumber(55566, 55888);
            res.WriteInt32(MonsterUniqueId);

            res.WriteCString($"Demon Bardock{MonsterUniqueId}"); //Name while spawning

            res.WriteCString($"Titan"); //Title

            res.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150)); //X Pos
            res.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150)); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.viewOffset); //view offset

            res.WriteInt32(
                900102); // Monster serial ID.  70101 for Lesser Demon.  If this is invalid, you can't "loot" the monster or see it's first CString

            res.WriteInt32(2016001); // Model from model_common.csv  2070001 for Lesser Demon

            res.WriteInt16(100); //model size

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries = 0x10;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(16);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries2 = 0x10;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(16); // this was an x2 loop (i broke it down)
            }

            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteInt32(0);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);

                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1); // bool
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);

                res.WriteByte(1);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16

            int numEntries3 = 0x10; //equipment slots to display?
            for (int i = 0; i < numEntries3; i++)

            {
                res.WriteInt64(100);
            }

            res.WriteInt32(900102); //1000 0000 here makes it stand up and not be dead.   or 0 = alive, 1 = dead

            res.WriteInt64(itemIDs[x]);

            res.WriteInt64(itemIDs[x]);

            res.WriteInt64(itemIDs[x]);

            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteInt32(900); //Current HP

            res.WriteInt32(1000); //Max HP

            res.WriteInt32(0x80); // cmp to 0x80 = 128

            int numEntries4 = 0x80; //Statuses?
            for (int i = 0; i < numEntries4; i++)

            {
                res.WriteInt32(900102); // ID ?
                res.WriteInt32(1);
                res.WriteInt32(0);
            }

            Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_monster_data, res, ServerType.Area);


            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(11);
            res5.WriteInt32(MonsterUniqueId);
            Router.Send(client, (ushort) AreaPacketId.recv_monster_hate_on, res5, ServerType.Area);

            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(11);
            res6.WriteInt32(MonsterUniqueId);
            Router.Send(client, (ushort) AreaPacketId.recv_battle_report_notify_damage_hp, res6, ServerType.Area);


            IBuffer res12 = BufferProvider.Provide();
            res12.WriteInt32(0);

            res12.WriteInt32(MonsterUniqueId);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_monster_state_update_notify, res12, ServerType.Area);

            /*IBuffer res81 = BufferProvider.Provide();
            res81.WriteInt32(11);
    
            res81.WriteFloat(45);
            res81.WriteFloat(0);
            res81.WriteFloat(0);
            res81.WriteByte(0);
    
            res81.WriteFloat(0);
            res81.WriteFloat(0);
            res81.WriteInt32(3);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_maplink, res81);
    
            /*
                        IBuffer res5 = BufferProvider.Provide();
                        res5.WriteInt32(8);
                        res5.WriteInt32(0);
                        res5.WriteFloat(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, res5);
    
    
                        IBuffer res6 = BufferProvider.Provide();
                        res6.WriteInt32(0);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_exec, res6);
    
                        IBuffer res8 = BufferProvider.Provide();
                        res8.WriteInt32(client.Character.Id);
                        res8.WriteInt32(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_notify_damage_hp, res8);
    
                        IBuffer res4 = BufferProvider.Provide();
                        res4.WriteInt32(1);
                        Router.Send(client, (ushort)AreaPacketId.recv_battle_report_notify_hit_effect, res4);
    
                        IBuffer res10 = BufferProvider.Provide();
                        res10.WriteByte(1);
                        res10.WriteInt16(0);
                        Router.Send(client, (ushort)AreaPacketId.recv_chara_target_move_side_speed_per, res10);
    
                        IBuffer res9 = BufferProvider.Provide();
                        res9.WriteInt32(8);
    
                        res9.WriteInt32(8); // 1 = no reactive ?
                        Router.Send(client, (ushort)AreaPacketId.recv_monster_state_update_notify, res9); */


            //3100102 attack monster, where to put it ?
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "Monster";
    }
}
