using System.Collections.Generic;
using Necromancy.Server.Model;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Gimmick related commands.
    /// </summary>
    public class GimmickCommand : ServerChatCommand
    {
        public GimmickCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }

            if(!int.TryParse(command[1], out int x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Please provide a value to test"));
                return;
            }
            if (!int.TryParse(command[2], out int y))
            {
                responses.Add(ChatResponse.CommandError(client, $"No 2nd parameter provided.  that's ok"));
            }

            switch (command[0])
            {
                case "spawn":
                    Gimmick myGimmick = Server.Instances.CreateInstance<Gimmick>();
                    myGimmick.ModelId = x;
                    IBuffer resI = BufferProvider.Provide();
                    resI.WriteInt32(myGimmick.InstanceId);
                    resI.WriteFloat(client.Character.X);
                    resI.WriteFloat(client.Character.Y);
                    resI.WriteFloat(client.Character.Z);
                    resI.WriteByte(client.Character.Heading);
                    resI.WriteInt32(x); //Gimmick number (from gimmick.csv)
                    resI.WriteInt32(0); //Gimmick State
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_gimmick_data, resI, ServerType.Area);
                    Logger.Debug($"You just created a gimmick with instance ID {myGimmick.InstanceId}");
                    break;
                case "move":
                    Gimmick myGimmick2 = Server.Instances.GetInstance((uint)x) as Gimmick;
                    myGimmick2.Z += y;
                    IBuffer res5 = BufferProvider.Provide();
                    res5.WriteInt32(myGimmick2.InstanceId);
                    res5.WriteFloat(myGimmick2.X);
                    res5.WriteFloat(myGimmick2.Y);
                    res5.WriteFloat(myGimmick2.Z);
                    res5.WriteByte(myGimmick2.Heading);
                    res5.WriteByte(0xA);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res5, ServerType.Area);
                    break;
                case "add":
                    Gimmick myGimmick3 = Server.Instances.GetInstance((uint)x) as Gimmick;
                    myGimmick3.MapId = client.Character.MapId;
                    myGimmick3.X = client.Character.X;
                    myGimmick3.Y = client.Character.Y;
                    myGimmick3.Z = client.Character.Z;
                    myGimmick3.Heading = client.Character.Heading;
                    myGimmick3.State = 0;
                    myGimmick3.Updated = DateTime.Now;
                    if (!Server.Database.InsertGimmick(myGimmick3))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick3 could not be saved to database"));
                        return;
                    }
                    break;
                case "remove":
                    Gimmick myGimmick4 = Server.Instances.GetInstance((uint)x) as Gimmick;
                    if (!Server.Database.DeleteGimmick(myGimmick4.Id))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick3 could not be deleted from database"));
                        return;
                    }
                    break;
                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "gimmick";
        public override string HelpText => "usage: `/gimmick [argument] [number]` - does something gimmick related";


    }

}
