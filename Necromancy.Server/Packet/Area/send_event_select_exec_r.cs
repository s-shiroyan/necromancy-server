using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_select_exec_r : ClientHandler
    {
        public send_event_select_exec_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_select_exec_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int selectExecCode = packet.Data.ReadInt32();
            Logger.Debug($" THe packet contents are :{selectExecCode}");
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("Message at End of Event"); // find max size
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_select_exec, res, ServerType.Area);
            if (selectExecCode == -1)
            {
                RecvEventEnd(client);
            }
        }
        private void RecvEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);

        }

    }
}
