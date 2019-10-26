using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.IO;
using System.Globalization;

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


            if (client.Character != null)
            {
                client.Character.X = packet.Data.ReadFloat();
                client.Character.Y = packet.Data.ReadFloat();
                client.Character.Z = packet.Data.ReadFloat();

                float percentMovementIsX = packet.Data.ReadFloat();
                float percentMovementIsY = packet.Data.ReadFloat();

                /*for testing individual bytes
                byte d = packet.Data.ReadByte();
                byte e = packet.Data.ReadByte();
                byte e1 = packet.Data.ReadByte(); //Character Y Alignment with the Map Y Axis (128 for perfect, 0 for 90Degree offset)
                byte f = packet.Data.ReadByte(); //Character facing Direction Relive to Map Y Axis. 63 for moving forward along the axis. 191 for moving backward along the axis.
                float percentMovementIsY = BitConverter.ToSingle(new[] { d, e, e1, f }, 0);
                */

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


                //This is all Animation related.
                        IBuffer res2 = BufferProvider.Provide();

                        res2.WriteInt32(client.Character.Id);//Character ID
                        res2.WriteFloat(verticalMovementSpeedMultiplier);
                        res2.WriteFloat(movementSpeed);
                        res2.WriteFloat(horizontalMovementSpeedMultiplier);

                       //Body Rotation relative to movement direction (client.Character.viewOffset). must have a value in a byte for movement animation in battle pose. Body rotation off by 90* with these settings
                       if (client.Character.weaponEquipped == true)
                        {
                            res2.WriteByte((byte)(percentMovementIsX * 256)); //HANDLES EAST AND WEST ANIMS WITH WEAPON unsheathed
                            res2.WriteByte((byte)(percentMovementIsY * 255));// // HANDLES NORTH AND SOUTH ANIMS WITH WEAPONS unsheathed
                            res2.WriteByte((byte)(horizontalMovementSpeedMultiplier * 255)); //
                        }
                        else
                        {
                            res2.WriteByte(0); //HANDLES EAST AND WEST ANIMS WITH WEAPON unsheathed
                            res2.WriteByte(0);// // HANDLES NORTH AND SOUTH ANIMS WITH WEAPONS unsheathed
                            res2.WriteByte(0); //
                        }
                        res2.WriteInt16(0xFFFF); //FIXES MOVEMENT LAG???

                        res2.WriteByte(0);// DONT TOUCH >.> CAUSES VISUAL TELEPORTING??? Character re-draw byte? 255 shrinks to nothing then redraws.
                        res2.WriteByte(client.Character.movementAnim); //MOVEMENT ANIM
                        res2.WriteByte(client.Character.animJumpFall);//JUMP & FALLING ANIM




                        Router.Send(client.Map, (ushort)AreaPacketId.recv_0xE8B9, res2, ServerType.Area, client);

                
                //This is all Position and Orientation Related.
                    IBuffer res = BufferProvider.Provide();

                    res.WriteInt32(client.Character.Id);//Character ID
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.viewOffset);//View offset / Head Rotation
                    res.WriteByte(0);//Character state?

                    Router.Send(client.Map, (ushort)AreaPacketId.recv_0x6B6A, res, ServerType.Area, client);

                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area, client);

            }
        }
    }
}

/*
  SAMPLE -  [REMOVED CORDS, AND THE LAST 7 BYTES AS THEY DID NOT CHANGE OVER MY TEST DATA]

-a -b -b1-c -d -e -e1-f -[incline]-h -i -j -    Varibles to position of packet

-AE-4A-A3-BD-5A-2F-7F-BF-[incline]-86-01-75-
-76-59-A3-BD-49-2F-7F-BF-[incline]-61-F8-06-	Running Sideways
-53-C9-A2-BD-A4-30-7F-BF-[incline]-C6-0A-B1-

-0C-4D-67-BD-6D-97-7F-3F-[incline]-88-A1-91-
-F0-92-5D-BE-63-EF-79-3F-[incline]-87-00-E1-	Forward running
-7D-8A-5D-BE-DB-EF-79-3F-[incline]-6B-00-E1-
-F0-92-5D-BE-63-EF-79-3F-[incline]-87-00-E1-
                                                running in said directions below
-8C-93-7D-3F-7C-92-0C-BE-[incline]-D9-C2-F0-  e

-94-E6-90-3D-C2-5B-7F-BF-[incline]-E3-EB-C0-  s 

-04-F8-7F-BF-B4-BD-7F-BC-[incline]-E6-DE-C4-  w

-4A-D1-1F-BE-E6-DC-7C-3F-[incline]-32-E7-CB-  n

-07-97-EF-3E-CA-3D-59-3F-[incline]-37-DB-B2-  ne

-CE-E1-59-3F-59-94-12-BF-[incline]-16-65-20-  se

-53-6A-35-BF-59-9F-34-BF-[incline]-14-8B-CE-  sw

-6E-D9-28-BF-D1-6B-40-3F-[incline]-17-6C-BB-  nw


*/
