using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_exec : ClientHandler
    {
        public send_skill_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_skill_exec;

        int i = 0;

        public override void Handle(NecClient client, NecPacket packet)
        {

            int myTargetID = packet.Data.ReadInt32();

            float X = packet.Data.ReadFloat();
            float Y = packet.Data.ReadFloat();
            float Z = packet.Data.ReadFloat();
         
            int errcode = packet.Data.ReadInt32();


            Console.WriteLine($"myTargetID : {myTargetID}");
            Console.WriteLine($"Target location : X-{X}Y-{Y}Z-{Z}");
            Console.WriteLine($"ErrorCode : {errcode}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errcode);//see sys_msg.csv
            /*
                -1      Unable to use skill
                -1322   Incorrect target
                -1325   Insufficient usage count for Power Level
                1       Not enough distance
                GENERIC Unable to use skill: < errcode >
            */
            res.WriteFloat(1);//Cool time      ./Skill_base.csv   Column J 
            res.WriteFloat(1);//Rigidity time  ./Skill_base.csv   Column L  

            Router.Send(client, (ushort)AreaPacketId.recv_skill_exec_r, res, ServerType.Area);

            SendDataNotifyEOData(client, X, Y, Z, i);
            //SendDataNotifyEOData2(client);
            //SendEOBaseNotifySphere(client);
            //SendEOUpdateState(client);
            i++;
        }

        private void SendDataNotifyEOData(NecClient client, float X, float Y, float Z, int i)
        {
            //recv_data_notify_eo_data = 0x8075, // Parent = 0x8066 // Range ID = 02
            IBuffer res = BufferProvider.Provide();
            
            res.WriteInt32(1+i);//Effect ID(has a unique ID like characters and so on.)
            res.WriteFloat(client.Character.X);//x      Point of origin.X
            res.WriteFloat(client.Character.Y);//y      Point of origin.Y
            res.WriteFloat(client.Character.Z);//z      Point of origin.Z

            res.WriteFloat(5);//x
            res.WriteFloat(5);//y
            res.WriteFloat(5);//z

            res.WriteInt32(6);//ID for skills effect from eo_base.csv
            res.WriteInt32(200);
            res.WriteInt32(0);
            res.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_eo_data, res, ServerType.Area);

            //recv_eo_update_end_trapid = 0xE8BF,
            IBuffer res2 = BufferProvider.Provide();
			res2.WriteInt32(1+i);
            res2.WriteInt32(4);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_eo_update_end_trapid, res2, ServerType.Area);

            //recv_eo_update_second_trapid = 0x96EA, = 0xE8BF,
            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(1+i);
            res3.WriteInt32(4);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_eo_update_second_trapid, res3, ServerType.Area);

            SendEOBaseNotifySphere(client);
        }

        private void SendDataNotifyEOData2(NecClient client)
        {
            //recv_data_notify_eo_data2 = 0xEDB3,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(1+i);//Effect ID(has a unique ID like characters and so on.)
            res.WriteInt32(client.Character.Id);

            res.WriteFloat(client.Character.X);
            res.WriteFloat(client.Character.Y + 50);
            res.WriteFloat(client.Character.Z + 120);

            res.WriteFloat(client.Character.X);
            res.WriteFloat(client.Character.Y + 50);
            res.WriteFloat(client.Character.Z + 120);

            res.WriteInt32(1430211);

            res.WriteInt32(1430211);

            res.WriteInt32(1430211);

            res.WriteInt32(1430211);

            res.WriteInt32(1430211);

            res.WriteInt32(1430211);

            res.WriteInt32(1430211);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_eo_data2, res, ServerType.Area);

            //SendEOUpdateState(client);
        }

        private void SendEOBaseNotifySphere(NecClient client)
        {
            //recv_eo_base_notify_sphere = 0xAF6D,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(i+1);

            res.WriteFloat(300);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_eo_base_notify_sphere, res, ServerType.Area);

            //SendDataNotifyEOData2(client);
        }

        private void SendEOUpdateState(NecClient client)
        {
            //recv_eo_update_state = 0x28FD, // Parent = 0x28E7 // Range ID = 01
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_eo_update_state, res, ServerType.Area);
        }
    }
}
