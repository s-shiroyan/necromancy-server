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
            Gimmick myGimmick = new Gimmick();
            IBuffer res = BufferProvider.Provide();

            switch (command[0])
            {
                case "spawn":
                    myGimmick = Server.Instances.CreateInstance<Gimmick>();
                    myGimmick.ModelId = x;
                    myGimmick.X = client.Character.X;
                    myGimmick.Y = client.Character.Y;
                    myGimmick.Z = client.Character.Z;
                    myGimmick.Heading = client.Character.Heading;
                    myGimmick.State = 0xA;
                    res.WriteInt32(myGimmick.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteInt32(x); //Gimmick number (from gimmick.csv)
                    res.WriteInt32(0); //Gimmick State
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_gimmick_data, res, ServerType.Area);
                    Logger.Debug($"You just created a gimmick with instance ID {myGimmick.InstanceId}");

                    //Add a sign above them so you know their ID.
                    GGateSpawn gGateSpawn = Server.Instances.CreateInstance<GGateSpawn>();
                    res = BufferProvider.Provide();
                    res.WriteInt32(gGateSpawn.InstanceId); // Unique Object ID.  Crash if already in use (dont use your character ID)
                    res.WriteInt32(gGateSpawn.SerialId); // Serial ID for Interaction? from npc.csv????
                    res.WriteByte(0); // 0 = Text, 1 = F to examine  , 2 or above nothing
                    res.WriteCString($"You spawned a Gimmick here : {client.Character.Name}"); //"0x5B" //Name
                    res.WriteCString($"The Instance ID of your Gimmick is: {myGimmick.InstanceId}"); //"0x5B" //Title
                    res.WriteFloat(client.Character.X ); //X Pos
                    res.WriteFloat(client.Character.Y ); //Y Pos
                    res.WriteFloat(client.Character.Z+200); //Z Pos
                    res.WriteByte(client.Character.Heading); //view offset
                    res.WriteInt32(gGateSpawn.ModelId); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv
                    res.WriteInt16(gGateSpawn.Size); //  size of the object
                    res.WriteInt32(gGateSpawn.Active); // 0 = collision, 1 = no collision  (active/Inactive?)
                    res.WriteInt32(gGateSpawn.Glow); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_ggate_stone_data, res, ServerType.Area);

                    //Jump over your gimmick
                    res = BufferProvider.Provide();
                    res.WriteInt32(client.Character.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z+500);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(client.Character.movementAnim);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    break;
                case "move": //move a gimmick to your current position and heading
                    Gimmick myGimmick2 = Server.Instances.GetInstance((uint)x) as Gimmick;
                    res.WriteInt32(myGimmick2.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "heading": //only update the heading to your current heading
                    myGimmick = Server.Instances.GetInstance((uint)x) as Gimmick;
                    res.WriteInt32(myGimmick.InstanceId);
                    res.WriteFloat(myGimmick.X);
                    res.WriteFloat(myGimmick.Y);
                    res.WriteFloat(myGimmick.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "add":
                    myGimmick = Server.Instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.MapId = client.Character.MapId;
                    //myGimmick.X = myGimmick.X;
                    //myGimmick.Y = myGimmick.Y;
                    //myGimmick.Z = myGimmick.Z;
                    //myGimmick.Heading = myGimmick.Heading;
                    myGimmick.State = 0xA;
                    myGimmick.Updated = DateTime.Now;
                    if (!Server.Database.InsertGimmick(myGimmick))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick3 could not be saved to database"));
                        return;
                    }
                    break;
                case "update":
                    myGimmick = Server.Instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.MapId = client.Character.MapId;
                    myGimmick.X = client.Character.X;
                    myGimmick.Y = client.Character.Y;
                    myGimmick.Z = client.Character.Z;
                    myGimmick.Heading = client.Character.Heading;
                    myGimmick.State = 0xA;
                    myGimmick.Updated = DateTime.Now;
                    if (!Server.Database.UpdateGimmick(myGimmick))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick3 could not be saved to database"));
                        return;
                    }
                    break;
                case "remove":
                    myGimmick = Server.Instances.GetInstance((uint)x) as Gimmick;
                    if (!Server.Database.DeleteGimmick(myGimmick.Id))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick3 could not be deleted from database"));
                        return;
                    }
                    break;
                    case "rotate":
                    myGimmick = Server.Instances.GetInstance((uint)x) as Gimmick;
                    //myGimmick5.Z += y;
                    res.WriteInt32(myGimmick.InstanceId);
                    res.WriteFloat(myGimmick.X);
                    res.WriteFloat(myGimmick.Y);
                    res.WriteFloat(myGimmick.Z);
                    res.WriteByte((byte)y);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    Logger.Debug($"Gimmick {myGimmick.InstanceId} has been rotated to {y*2} degrees.");
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
