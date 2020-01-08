using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Chat.Command.Commands
{
    //displays message that you died
    public class Died : ServerChatCommand
    {
        public Died(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (client.Character.hadDied == true)
            {
                IBuffer res4 = BufferProvider.Provide();
                Router.Send(client.Map, (ushort)AreaPacketId.recv_self_lost_notify, res4, ServerType.Area);
            }

            if (client.Character.hadDied == false)
            {
                client.Character.hadDied = true; // setting before the Sleep so other monsters can't "kill you" while you're dieing
                client.Character.currentHp = 0;
                List<PacketResponse> brList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(client.Character.InstanceId);
                RecvBattleReportNoactDead cDead1 = new RecvBattleReportNoactDead(client.Character.InstanceId, 1);
                RecvBattleReportNoactDead cDead2 = new RecvBattleReportNoactDead(client.Character.InstanceId, 2);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();

                brList.Add(brStart);
                brList.Add(cDead1); //animate the death of your living body
                brList.Add(brEnd);
                Server.Router.Send(client.Map, brList, client); // send death animation to other players


                brList[1] = cDead2;
                Router.Send(client, brList); // send death animaton to player 1

                DeadBody deadBody = Server.Instances.GetInstance((uint)client.Character.DeadBodyInstanceId) as DeadBody;

                deadBody.X = client.Character.X;
                deadBody.Y = client.Character.Y;
                deadBody.Z = client.Character.Z;
                deadBody.Heading = client.Character.Heading;
                client.Character.movementId = client.Character.DeadBodyInstanceId;

                Task.Delay(TimeSpan.FromMilliseconds((int)(5 * 1000))).ContinueWith
                (t1 =>
                    {
                        client.Character.hadDied = false; // quick switch to living state so your dead body loads with your gear
                                                          //load your dead body on to the map for you to see in soul form. 
                        RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody, client);
                        Server.Router.Send(client, cBodyData.ToPacket());

                        client.Character.hadDied = true; // back to dead so your soul appears with-out gear.
                    }
                );

                Task.Delay(TimeSpan.FromMilliseconds((int)(15 * 1000))).ContinueWith
                (t1 =>
                    {
                        //reload your living body with no gear
                        RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
                        Server.Router.Send(client, cData.ToPacket());
                    }
                );


            }


        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "Died";
    }
}
