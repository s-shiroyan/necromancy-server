using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_access_object : Handler
    {
        public send_event_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_event_access_object;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectID = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);
            Console.WriteLine($"Packet int32 Contents in Decimal: {objectID} ");
            SendEventMessage(client, objectID);
            //Task.Delay(TimeSpan.FromMilliseconds((int)(2 * 1000))).ContinueWith(t1 =>
            {
                Router.Send(client, (ushort)AreaPacketId.recv_event_access_object_r, res);
            }
            //);

            //Task.Delay(TimeSpan.FromMilliseconds((int)(3 * 1000))).ContinueWith(t1 => { SendEventMessage(client, objectID); });
            //Task.Delay(TimeSpan.FromMilliseconds((int)(4 * 1000))).ContinueWith(t1 => { SendEventBlockMessage(client, objectID); });
            //SendEventMessage(client, objectID);
            //SendEventBlockMessage(client,objectID);
        }

        private void SendEventMessage(NecClient client,int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_message, res);
        }

        private void SendEventBlockMessage(NecClient client, int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);
            res.WriteCString("Hello world.");
            Router.Send(client, (ushort)AreaPacketId.recv_event_block_message, res);
        }
    }
}