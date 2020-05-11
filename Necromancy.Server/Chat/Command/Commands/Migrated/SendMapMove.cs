using System;
using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendMapMove : ServerChatCommand
    {
        public SendMapMove(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            string entry = command[0];
            Server.Maps.TryGet(client.Character.MapId, out Map map);
            if (entry.Contains(":"))
            {
                string[] keyValue = entry.Split(":");
                if (keyValue.Length != 4)
                {
                    return;
                }

                int X = Convert.ToInt32(keyValue[0]);
                int Y = Convert.ToInt32(keyValue[1]);
                int Z = Convert.ToInt32(keyValue[2]);
                int Orietation = Convert.ToInt32(keyValue[3]);
                MapPosition mapPos = new MapPosition(X, Y, Z, (byte) Orietation);
                map.EnterForce(client, mapPos);
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "MapMove";
    }
}
