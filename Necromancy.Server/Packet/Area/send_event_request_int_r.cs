using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_request_int_r : ClientHandler
    {
        private readonly NecServer _server;

        public send_event_request_int_r(NecServer server) : base(server)
        {
            _server = server;
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_request_int_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            if (client.Character.currentEvent == null)
            {
                Logger.Error($"Recevied AreaPacketId.send_event_request_int_r with no current event saved.");
                return;
            }
            switch (client.Character.currentEvent)
            {
                case MoveItem moveItem:
                    IBuffer res = BufferProvider.Provide();
                    int count = packet.Data.ReadInt32();
                    Logger.Debug($"Returned [{count}]");
                    SendEventEnd(client);
                    MoveItem(client, moveItem, count);
                    client.Character.currentEvent = null;
                    break;
                default:
                    Logger.Error($"Recevied AreaPacketId.send_event_request_int_r with undefined event type.");
                    break;
            }
        }

        private void MoveItem(NecClient client, MoveItem moveItem, int count)
        {
            if (count <= 0)
            {
                client.Character.currentEvent = null;
                return;
            }
            moveItem.itemCount = (byte)count;
            moveItem.Move(_server, client);
        }
        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);

        }

    }
}
