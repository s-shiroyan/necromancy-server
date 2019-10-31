using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_select_map_and_channel_r : ClientHandler
    {
        public send_event_select_map_and_channel_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_select_map_and_channel_r;
        
        public override void Handle(NecClient client, NecPacket packet)
        {
            int selectedMap = packet.Data.ReadInt32();
            int selectedChannel = packet.Data.ReadInt32();
            Logger.Debug($"Packet Contents being sent are {selectedMap} and {selectedChannel}");
            if (selectedMap == -2147483648)
            {
                SendEventEnd(client);
            }
            else
            {
                SendMapChangeForce(client, selectedMap);
                SendMapEntry(client, selectedMap);

                SendEventEnd(client);
            }
        }

        private void SendMapChangeForce(NecClient client, int MapID)
        {
            IBuffer res = BufferProvider.Provide();
            client.Character.MapId = MapID;
            Map map = Server.Map.Get(client.Character.MapId);

            client.Character.X = map.X;
            client.Character.Y = map.Y;
            client.Character.Z = map.Z;

            //sub_4E4210_2341  // impacts map spawn ID
            res.WriteInt32(MapID); //MapSerialID
            res.WriteInt32(MapID); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            //sub_484420   //  does not impact map spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.viewOffset); //View offset

            Router.Send(client, (ushort)AreaPacketId.recv_map_change_force, res, ServerType.Area);

        }
        private void SendMapEntry(NecClient client, int myMapId)
        {
            int mapId = myMapId;


            Map map = Server.Map.Get(mapId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_map_entry_r, res, ServerType.Area);

            SendEventEnd(client);  //Forgot to add this.  its slightly important.
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);

        }



    }
}
