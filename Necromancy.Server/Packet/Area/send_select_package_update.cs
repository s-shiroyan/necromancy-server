using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_select_package_update : Handler
    {
        public send_select_package_update(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_select_package_update;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int characterID = packet.Data.ReadInt32();
            int operation = packet.Data.ReadInt32();
            int errcode = 0;

            IBuffer res = BufferProvider.Provide();

            /*if (operation == 0x03)//delete mail
                res.WriteInt32(3);
            else if (operation == 0x04)//unprotect message
                res.WriteInt32(4);
            else if (operation == 0x01 || operation == 0x0)//receive message
                res.WriteInt32(0);*/

            res.WriteInt32(errcode);//Error message Call. 0 for success. see additional options in Sys_msg.csv
                /*
                1	You have unopened mails	SYSTEM_WARNING
                2	No mails to delete	SYSTEM_WARNING
                3	You have %d unreceived mails. Please check your inbox.	SYSTEM_WARNING
                -2414	Mail title includes banned words	SYSTEM_WARNING

                */

            Router.Send(client, (ushort) AreaPacketId.recv_select_package_update_r, res);
        }
    }
}