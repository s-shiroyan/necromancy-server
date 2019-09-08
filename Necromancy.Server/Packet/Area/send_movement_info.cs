using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.IO;

namespace Necromancy.Server.Packet.Area
{
    public class send_movement_info : Handler
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


                //these bytes below with a varible as a name have not been confirmed this is for testing
                byte a = packet.Data.ReadByte();

                byte b = packet.Data.ReadByte();
                byte b1 = packet.Data.ReadByte();

                byte c = packet.Data.ReadByte();

                byte d = packet.Data.ReadByte();

                byte e = packet.Data.ReadByte();
                byte e1 = packet.Data.ReadByte();

                byte f = packet.Data.ReadByte();

                byte g1 = packet.Data.ReadByte();
                byte g2 = packet.Data.ReadByte();
                byte g3 = packet.Data.ReadByte();
                byte g4 = packet.Data.ReadByte();

                byte h = packet.Data.ReadByte();

                byte i = packet.Data.ReadByte();

                byte j = packet.Data.ReadByte();

                byte k = packet.Data.ReadByte();
                byte k1 = packet.Data.ReadByte();
                byte k2 = packet.Data.ReadByte();
                byte k3 = packet.Data.ReadByte();

                byte l = packet.Data.ReadByte();

                client.Character.movementAnim = packet.Data.ReadByte();

                client.Character.animJumpFall = packet.Data.ReadByte();

                // the game divides the normal 360 radius by 2. giving view direction only 1-180

                if (client.Character.viewOffset <= 180) // NORTH
                {
                    client.Character.wepEastWestAnim = 0;
                    client.Character.wepNorthSouthAnim = 127;

                    if (client.Character.viewOffset <= 168.75) // SOUTH-EAST
                    {
                        client.Character.wepEastWestAnim = 127;
                        client.Character.wepNorthSouthAnim = 127;

                        if (client.Character.viewOffset <= 146.25) // EAST
                        {
                            client.Character.wepEastWestAnim = 127;
                            client.Character.wepNorthSouthAnim = 0;

                            if (client.Character.viewOffset <= 123.75) // SOUTH-EAST
                            {
                                client.Character.wepEastWestAnim = 127;
                                client.Character.wepNorthSouthAnim = 128;

                                if (client.Character.viewOffset <= 101.25) // SOUTH
                                {
                                    client.Character.wepEastWestAnim = 0;
                                    client.Character.wepNorthSouthAnim = 128;

                                    if (client.Character.viewOffset <= 78.75) // SOUTH-WEST
                                    {
                                        client.Character.wepEastWestAnim = 128;
                                        client.Character.wepNorthSouthAnim = 128;

                                        if (client.Character.viewOffset <= 56.25) // WEST
                                        {
                                            client.Character.wepEastWestAnim = 128;
                                            client.Character.wepNorthSouthAnim = 0;

                                            if (client.Character.viewOffset <= 33.75) // NORTH-WEST
                                            {
                                                client.Character.wepEastWestAnim = 128;
                                                client.Character.wepNorthSouthAnim = 127;

                                                if (client.Character.viewOffset <= 11.25) // NORTH
                                                {
                                                    client.Character.wepEastWestAnim = 0;
                                                    client.Character.wepNorthSouthAnim = 127;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (client.Character.weaponEquipped == false)
                {
                    client.Character.wepEastWestAnim = 0;
                    client.Character.wepNorthSouthAnim = 0;
                }

                {
                    // for (byte xd = 0; xd < 255; xd++)
                    {

                        IBuffer res2 = BufferProvider.Provide();

                        res2.WriteInt32(client.Character.Id);//Character ID
                        res2.WriteFloat(client.Character.X);
                        res2.WriteFloat(client.Character.Y);
                        res2.WriteFloat(client.Character.Z);

                        res2.WriteByte(client.Character.wepEastWestAnim); //HANDLES EAST AND WEST ANIMS WITH WEAPONS
                        res2.WriteByte(client.Character.wepNorthSouthAnim);// // HANDLES NORTH AND SOUTH ANIMS WITH WEAPONS
                        res2.WriteByte(0);

                        res2.WriteInt16(0);

                        res2.WriteByte(0);
                        res2.WriteByte(client.Character.movementAnim); //MOVEMENT ANIM
                        res2.WriteByte(client.Character.animJumpFall);//JUMP & FALLING ANIM




                        Router.Send(client.Map, (ushort)AreaPacketId.recv_0xE8B9, res2, client);


                        //System.Threading.Thread.Sleep(1000);
                    }

                    IBuffer res = BufferProvider.Provide();

                    res.WriteInt32(client.Character.Id);//Character ID
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.viewOffset);//View offset
                    res.WriteByte(0);//Character state?

                    Router.Send(client.Map, (ushort)AreaPacketId.recv_0x6B6A, res, client);

                   // Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, client);


                }
            }
        }
    }
}