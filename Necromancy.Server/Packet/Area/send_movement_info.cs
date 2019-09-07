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
                client.Character.a = packet.Data.ReadByte();

                client.Character.b = packet.Data.ReadByte();
                client.Character.b1 = packet.Data.ReadByte();

                client.Character.c = packet.Data.ReadByte();

                client.Character.d = packet.Data.ReadByte();

                client.Character.e = packet.Data.ReadByte();
                client.Character.e1 = packet.Data.ReadByte();

                client.Character.xAnim = packet.Data.ReadByte();

                client.Character.g1 = packet.Data.ReadByte();
                client.Character.g2 = packet.Data.ReadByte();
                client.Character.g3 = packet.Data.ReadByte();
                client.Character.g4 = packet.Data.ReadByte();

                client.Character.h = packet.Data.ReadByte();

                client.Character.i = packet.Data.ReadByte();

                client.Character.j = packet.Data.ReadByte();

                client.Character.k = packet.Data.ReadByte();
                client.Character.k1 = packet.Data.ReadByte();
                client.Character.k2 = packet.Data.ReadByte();
                client.Character.k3 = packet.Data.ReadByte();

                client.Character.l = packet.Data.ReadByte();

                client.Character.movementAnim = packet.Data.ReadByte();

                client.Character.animJumpFall = packet.Data.ReadByte();

                client.Character.H2 = client.Character.xAnim;

                client.Character.H = 126; //HANDLES LEFT AND RIGHT ANIMS WITH WEAPONS/SIDEWALKING   
                client.Character.H2 = 0; // HANDLES FRONT AND BACK ANIMS WITH WEAPONS/SIDEWALKING

                // the game divides the normal 360 radius by 2. giving view direction only 1-180

                if (client.Character.viewOffset <= 180) // NORTH
                {
                    client.Character.H = 0;
                    client.Character.H2 = 126;

                    if (client.Character.viewOffset <= 168.75) // SOUTH-EAST
                    {
                        client.Character.H = 126;
                        client.Character.H2 = 126;

                        if (client.Character.viewOffset <= 146.25) // EAST
                        {
                            client.Character.H = 126;
                            client.Character.H2 = 0;

                            if (client.Character.viewOffset <= 123.75) // SOUTH-EAST
                            {
                                client.Character.H = 126;
                                client.Character.H2 = 129;

                                if (client.Character.viewOffset <= 101.25) // SOUTH
                                {
                                    client.Character.H = 0;
                                    client.Character.H2 = 129;

                                    if (client.Character.viewOffset <= 78.75) // SOUTH-WEST
                                    {
                                        client.Character.H = 129;
                                        client.Character.H2 = 129;

                                        if (client.Character.viewOffset <= 56.25) // WEST
                                        {
                                            client.Character.H = 129;
                                            client.Character.H2 = 0;

                                            if (client.Character.viewOffset <= 33.75) // NORTH-WEST
                                            {
                                                client.Character.H = 129;
                                                client.Character.H2 = 126;

                                                if (client.Character.viewOffset <= 11.25) // NORTH
                                                {
                                                    client.Character.H = 0;
                                                    client.Character.H2 = 126;
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
                    client.Character.H = 0;
                    client.Character.H2 = 0;
                }

                {
                    // for (byte xd = 0; xd < 255; xd++)
                    {

                        IBuffer res2 = BufferProvider.Provide();

                        res2.WriteInt32(client.Character.Id);//Character ID
                        res2.WriteFloat(client.Character.X);
                        res2.WriteFloat(client.Character.Y);
                        res2.WriteFloat(client.Character.Z);

                        res2.WriteByte(client.Character.H);//MOVEMENT ANIMS WITH WEAPON EQUIPPED
                        res2.WriteByte(client.Character.H2);// ALLOWS DIAGONAL WALKING WITH WEAPON EQUIPPED
                        res2.WriteByte(0);

                        //res2.WriteInt16(0);
                        res2.WriteByte(0);
                        res2.WriteByte(0);

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