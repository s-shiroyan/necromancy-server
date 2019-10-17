﻿using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_removetrap_end : ClientHandler
    {
        public send_event_removetrap_end(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_removetrap_end;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
            SendEventRemoveTrapClose(client);
        }
        private void SendEventRemoveTrapClose(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_removetrap_close, res, ServerType.Area, client);

        }

    }
}