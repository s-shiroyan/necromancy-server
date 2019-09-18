using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_exec : Handler
    {
        public send_skill_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_skill_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //byte coolTimeID = packet.Data.ReadByte();
            //byte coolTimeID2 = packet.Data.ReadByte();
            //byte notSure = packet.Data.ReadByte();
            //int distance = packet.Data.ReadByte();
            int errorCode = packet.Data.ReadInt32();

            float X = packet.Data.ReadFloat();
            float Y = packet.Data.ReadFloat();
            float Z = packet.Data.ReadFloat();
         
            int myCharacterID = packet.Data.ReadInt32();

            Console.WriteLine($"myCharacterID : {myCharacterID}");
            Console.WriteLine($"Target location : X-{X}Y-{Y}Z-{Z}");
            Console.WriteLine($"ErrorCode : {errorCode}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Error check     | 1 - not enough distance, 2 or above - unable to use skill: 2 error, 0 - success
            res.WriteFloat(1);//Cool time      ./Skill_base.csv   Column J 
            res.WriteFloat(1);//Rigidity time  ./Skill_base.csv   Column L  
            Router.Send(client, (ushort)AreaPacketId.recv_skill_exec_r, res);
        }
    }
}