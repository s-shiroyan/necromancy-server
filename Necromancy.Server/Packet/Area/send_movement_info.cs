using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;


namespace Necromancy.Server.Packet.Area
{
    public class send_movement_info : ClientHandler
    {
        public send_movement_info(NecServer server) : base(server)
        {

        }

        public override ushort Id => (ushort)AreaPacketId.send_movement_info;



        public override void Handle(NecClient client, NecPacket packet)
        {

                client.Character.X = packet.Data.ReadFloat();
                client.Character.Y = packet.Data.ReadFloat();
                client.Character.Z = packet.Data.ReadFloat();

                float percentMovementIsX = packet.Data.ReadFloat();
                float percentMovementIsY = packet.Data.ReadFloat();
                float verticalMovementSpeedMultiplier = packet.Data.ReadFloat(); //  Confirm by climbing ladder at 1 up or -1 down. or Jumping

                float movementSpeed = packet.Data.ReadFloat();

                float horizontalMovementSpeedMultiplier = packet.Data.ReadFloat(); //always 1 when moving.  Confirm by Coliding with an  object and watching it Dip.

                client.Character.movementAnim = packet.Data.ReadByte(); //Character Movement Type: Type 8 Falling / Jumping. Type 3 normal:  Type 9  climbing

                client.Character.animJumpFall = packet.Data.ReadByte(); //Action Modifier Byte
                                                                        //146 :ladder left Foot Up.      //147 Ladder right Foot Up. 
                                                                        //151 Left Foot Down,            //150 Right Root Down .. //155 falling off ladder
                                                                        //81  jumping up,                //84  jumping down       //85 landing

                /* Uncomment for debugging movement. causes heavy console output. recommend commenting out "Packet" method in NecLogger.CS when debugging movement
                if (movementSpeed != 0)
                {
                    Logger.Debug($"Character {client.Character.Name} is in map {client.Character.MapId} @ : X[{client.Character.X}]Y[{client.Character.Y}]Z[{client.Character.Z}]");
                    Logger.Debug($"X Axis Aligned : {percentMovementIsX.ToString("P", CultureInfo.InvariantCulture)} | Y Axis Aligned  : {percentMovementIsY.ToString("P", CultureInfo.InvariantCulture)}");
                    Logger.Debug($"vertical Speed multi : {verticalMovementSpeedMultiplier}| Move Speed {movementSpeed} | Horizontal Speed Multi {horizontalMovementSpeedMultiplier}");
                    Logger.Debug($"Movement Type[{client.Character.movementAnim}]  Type Anim [{client.Character.animJumpFall}] View Offset:{client.Character.viewOffset}");
                    Logger.Debug($"---------------------------------------------------------------");

                    //Logger.Debug($"Var 1 {(byte)(percentMovementIsX*255)} |Var 2 {(byte)(percentMovementIsY*255)}  ");
                    //Logger.Debug($" Y to Map Y[{d}][{e}][{e1}][{f} |  and {percentMovementIsY}");
                }
                else
                {
                    Logger.Debug($"Movement Stop Reset");
                    Logger.Debug($"---------------------------------------------------------------");
                }
                */


                IBuffer res2 = BufferProvider.Provide();

                res2.WriteInt32(client.Character.InstanceId);//Character ID
                res2.WriteFloat(client.Character.X);
                res2.WriteFloat(client.Character.Y);
                res2.WriteFloat(client.Character.Z);
                                               
                res2.WriteFloat(percentMovementIsX);
                res2.WriteFloat(percentMovementIsY);
                res2.WriteFloat(verticalMovementSpeedMultiplier);
               
                res2.WriteFloat(movementSpeed);

                res2.WriteFloat(horizontalMovementSpeedMultiplier);

                res2.WriteByte(client.Character.movementAnim); //MOVEMENT ANIM
                res2.WriteByte(client.Character.animJumpFall);//JUMP & FALLING ANIM

                Router.Send(client.Map, (ushort)AreaPacketId.recv_0x8D92, res2, ServerType.Area, client);
            
            if (client.Character.takeover == true)
            {
                Logger.Debug($"Moving object ID {client.Character.eventSelectReadyCode}");
                IBuffer res = BufferProvider.Provide();
                IBuffer res3 = BufferProvider.Provide();


                res.WriteInt32(client.Character.eventSelectReadyCode);
                res.WriteFloat(client.Character.X);
                res.WriteFloat(client.Character.Y);
                res.WriteFloat(client.Character.Z);
                res.WriteByte(client.Character.movementAnim); //MOVEMENT ANIM
                res.WriteByte(client.Character.animJumpFall);//JUMP & FALLING ANIM
                
                Router.Send(client, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_r, res3, ServerType.Area, client);


            }




        }
    }
}
