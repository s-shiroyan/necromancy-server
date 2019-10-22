﻿using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_identify : ClientHandler
    {
        public send_shop_identify(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_shop_identify;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_shop_identify_r, res, ServerType.Area);
        }

    }
}