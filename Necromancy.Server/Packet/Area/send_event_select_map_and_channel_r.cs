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

                IBuffer res7 = BufferProvider.Provide();

                int numEntries = 0x2; //Max of 0x20 : cmp ebx,20 
                res7.WriteInt32(numEntries);
                for (int i = 0; i < numEntries; i++)
                {
                    //sub_494c50
                    res7.WriteInt32(client.Character
                        .MapId); //Unknown.  trying for clues from 'send_chara_select'  It uses the same Subs, and structure
                    res7.WriteInt32(client.Character.MapId);
                    res7.WriteInt32(0128);
                    res7.WriteInt16(60001);
                    //sub_4834C0
                    res7.WriteByte(128);
                    for (int j = 0; j < 0x80; j++) //j max 0x80
                    {
                        res7.WriteInt32(client.Character.MapId);
                        res7.WriteFixedString($"Loop{i}:{j}",
                            0x61); //Channel Names.  Variables let you know what Loop Iteration you're on
                        res7.WriteByte(1); //bool 1 | 0
                        res7.WriteInt16(0xFFFF); //Max players  -  Comment from other recv
                        res7.WriteInt16(0xFF); //Current players  - Comment from other recv
                        res7.WriteByte(10);
                        res7.WriteByte(10);
                    }

                    res7.WriteByte(5); //Number or Channels  - comment from other recv
                }

                Router.Send(client.Map, (ushort) AreaPacketId.recv_event_select_map_and_channel, res7, ServerType.Area);
            }
        }
        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area, client);

        }

    }
}
