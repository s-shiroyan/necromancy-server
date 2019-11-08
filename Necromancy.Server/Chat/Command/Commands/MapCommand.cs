using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Changes the map
    /// </summary>
    public class SendMapChangeForce : ServerChatCommand
    {
        public SendMapChangeForce(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!int.TryParse(command[0], out int mapId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!Server.Maps.TryGet(mapId, out Map map))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid MapId: {mapId}"));
                return;
            }

            IBuffer res = BufferProvider.Provide();
            client.Character.X = map.X;
            client.Character.Y = map.Y;
            client.Character.Z = map.Z;

            //sub_4E4210_2341  // impacts map spawn ID
            res.WriteInt32(map.Id); //MapSerialID
            res.WriteInt32(map.Id); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            //sub_484420   //  does not impact map spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(1); //View offset

            Router.Send(client, (ushort) AreaPacketId.recv_map_change_force, res, ServerType.Area);

            //SendMapChangeSyncOk(client);
            SendMapEntry(client, map);
        }

        private void SendMapEntry(NecClient client, Map map)
        {
            //map.Enter(client);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_map_entry_r, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "map";
    }
}
