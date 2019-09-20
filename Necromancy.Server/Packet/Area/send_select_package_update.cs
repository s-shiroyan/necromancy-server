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
            bool loot = false;
            int objectID = packet.Data.ReadInt32();
            int operation = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            if (operation == 0x03)//delete mail
                res.WriteInt32(0);
            else if (operation == 0x04)//unprotect message
                res.WriteInt32(0);
            else if (operation == 0x01 || operation == 0x0)//receive message
            {
                res.WriteInt32(0);
                loot = true;
            }
            else if (operation == 0x2)//reply to message
                res.WriteInt32(0);

            //res.WriteInt32(0);//Not sure on this,  0 is constant updates, 1 is "you have unopened mail", 2 is "no mail to delete"
            //3 is "you have %d unreceived mails. pleas check your inbox.", 4+ is "failed to send mail",

            Router.Send(client, (ushort) AreaPacketId.recv_select_package_update_r, res);

            if(loot)
            {
                SendLootAccessObject(client, objectID);
                loot = false;
            }
        }

        private void SendLootAccessObject(NecClient client, int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);

            Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res);
        }
    }
}