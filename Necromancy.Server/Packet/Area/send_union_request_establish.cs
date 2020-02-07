using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_union_request_establish : ClientHandler
    {
        public send_union_request_establish(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_union_request_establish;

        public override void Handle(NecClient client, NecPacket packet)
        {
            string unionName = packet.Data.ReadCString(); //It's the Name of your new Union
            int sys_msg = 0;

            if (unionName.Length >= 9) { sys_msg = -1; }
            else if (client.Character.AdventureBagGold < 30000) { sys_msg = -2; }
            else if (client.Soul.Level < 3) { sys_msg = -3; }
            else if (client.Character.unionId != 0) { sys_msg = -4; }
            /*
            else if (client.Soul.Level < 3) { sys_msg = -1711; }
            else if (unionNameRegExpressionCheck(unionName)== fail) { sys_msg = -1701; }
            else if (client.Character.unionId != 0) { sys_msg = -1702; }
            else if (Server.Database.SelectUnionByName(unionName) != null) { sys_msg = -1704; }
            else if (Server.Characters.GetAll().Count <= 5) { sys_msg = -1705; }
            else if (client.Character.UnionLastLeave <= Date.Time.Now() - 24) { sys_msg = -1719; }
             */

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(sys_msg);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_union_request_establish_r, res2, ServerType.Area);

            /*
            UNION_CREATE	0	Union created
            UNION_CREATE	-1	A union name must be 2 to 8 characters in length
            UNION_CREATE	-2	Not enough gold
            UNION_CREATE	-3	Insufficient Soul Rank
            UNION_CREATE	-4	You already belong to a union
            UNION_CREATE	-1711	Insufficient Soul Rank
            UNION_CREATE	-1701	The union name you have entered includes unavailable characters
            UNION_CREATE	-1702	You already belong to a union
            UNION_CREATE	-1704	This union name is already in use
            UNION_CREATE	-1705	Not enough people to create a union
            UNION_CREATE	-1719	You may not create a Union for 24 hours after leaving your last Union.
             */

            if (sys_msg != 0) { return; }

            //Server.Instances.CreateInstance<Union>();
            client.Character.unionId = (uint)Util.GetRandomNumber(8890, 8900); // crash if you get a pre-established number

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(client.Character.unionId); //Union Member ID
            res3.WriteInt32(client.Character.InstanceId);
            res3.WriteFixedString($"{client.Soul.Name}", 0x31); //size is 0x31
            res3.WriteFixedString($"{client.Character.Name}", 0x5B); //size is 0x5B
            res3.WriteInt32(client.Character.ClassId);
            res3.WriteByte(client.Character.Level);
            res3.WriteInt32(client.Character.MapId); // Location of your Union Member
            res3.WriteInt32(1); //1 = offline, 0 = online?
            res3.WriteFixedString($"Channel {client.Character.Channel}", 0x61); // Channel location
            res3.WriteInt32(0b111111); //leader?
            res3.WriteInt32(0b111111); //Subleader?
            res3.WriteInt32(0b111111); //Invite?
            res3.WriteInt32(0b111111); //Kick?
            res3.WriteInt32(0b111111); //News?
            res3.WriteInt32(0b111111); //Storage?

            Router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail_member, res3, ServerType.Msg);

            //Notify client if msg server found Union settings in database(memory) for client character Unique Persistant ID.
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.unionId); //Union Instance ID
            res.WriteFixedString(unionName, 0x31); //size is 0x31
            res.WriteInt32(client.Character.InstanceId); //leader
            res.WriteInt32(0); //subleader. 
            res.WriteInt32(0); //subleader2
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(1); //Union Level
            res.WriteInt32(1 /*myUnion.currentExp*/); //Union EXP Current
            res.WriteInt32(100 /*UnionLevels.Level2EXP*/); //Union EXP next level Target
            res.WriteByte(5); //Increase Union Member Limit above default 50 (See Union Bonuses
            res.WriteByte(10);
            res.WriteInt32(0);
            res.WriteInt16(0x0B); //Mantle/Cape design
            res.WriteFixedString($"You are all members of {unionName} now.  Welcome!", 0x196); //size is 0x196
            for (int i = 0; i < 8; i++)
                res.WriteInt32(i);
            res.WriteByte(15);

            Router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail, res, ServerType.Msg);

            IBuffer res36 = BufferProvider.Provide();
            res36.WriteInt32(client.Character.InstanceId);
            res36.WriteInt32(client.Character.unionId /*client.Character.UnionId*/);
            res36.WriteCString(unionName);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);

        }
    }
}
