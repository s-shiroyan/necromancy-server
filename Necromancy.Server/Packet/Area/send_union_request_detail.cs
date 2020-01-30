using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_detail : ClientHandler
    {
        public send_union_request_detail(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_union_request_detail;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //Acknowledge send
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.InstanceId); //probably error check. 0 for success
           
            Router.Send(client, (ushort) MsgPacketId.recv_union_request_detail_r, res2, ServerType.Msg);

            int currentDay = System.DateTime.Today.Day;

            //Notify client if msg server found Union settings in database(memory) for client character Unique Persistant ID.
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(8888); //Union Instance ID
            res.WriteFixedString("Trade_Union", 0x31); //size is 0x31
            res.WriteInt32(client.Character.InstanceId); //leader
            res.WriteInt32(client.Character.InstanceId); //subleader.  We need to assign character Instance IDs at server start instead of login...
            res.WriteInt32(client.Character.InstanceId); //subleader2
            res.WriteInt32(1111);
            res.WriteInt32(2222);
            res.WriteInt32(3333);
            res.WriteInt32(4444);
            res.WriteByte(6); //Union Level
            res.WriteInt32(800123 /*myUnion.currentExp*/); //Union EXP Current
            res.WriteInt32(1000000 /*UnionLevels.Level2EXP*/); //Union EXP next level Target
            res.WriteByte(30); //Increase Union Member Limit above default 50 (See Union Bonuses
            res.WriteByte(10);
            res.WriteInt32(client.Character.InstanceId);
            res.WriteInt16(55555);
            res.WriteFixedString("You are all members of Trade Union now.  Welcome!", 0x196); //size is 0x196
            for (int i = 0; i < 8; i++)
                res.WriteInt32(currentDay);
            res.WriteByte(15);

            Router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail, res, ServerType.Msg);

            //for each unionMember in myUnion.UnionMembers {  }
            //Notify client of each union member in above union, queried by charaname and InstanceId (for menu based interactions)
            for (int i = 0; i < 4; i++)
            {
                IBuffer res3 = BufferProvider.Provide();
                res3.WriteInt32(1001007); //not sure what this is.  union_Notify ID?
                res3.WriteInt32(client.Character.InstanceId);
                res3.WriteFixedString($"{client.Soul.Name}", 0x31); //size is 0x31
                res3.WriteFixedString($"{client.Character.Name}", 0x5B); //size is 0x5B
                res3.WriteInt32(client.Character.ClassId);
                res3.WriteByte(client.Character.Level);
                res3.WriteInt32(client.Character.MapId); // Location of your Union Member
                res3.WriteInt32(0); //Area of Map, somehow.
                res3.WriteFixedString($"Channel {client.Character.Channel}", 0x61); // Channel location
                res3.WriteInt32(99999);
                res3.WriteInt32(888888);
                res3.WriteInt32(77777777);
                res3.WriteInt32(666666666);
                res3.WriteInt32(5555555);
                res3.WriteInt32(44444);

                Router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail_member, res3, ServerType.Msg);
            }


        }
    }
}
