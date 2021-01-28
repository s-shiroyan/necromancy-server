using System.Collections.Generic;
using System.Threading;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Moves character x units upward.
    /// </summary>
    public class NoStringTestCommand : ServerChatCommand
    {
        public NoStringTestCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //if (command[0] == "")
            //{
            //    responses.Add(ChatResponse.CommandError(client, $"Invalid opcode: {command[0]}"));
            //    return;
            //}

            IBuffer res = BufferProvider.Provide();

            //no int32 section
            Router.Send(client, (ushort)AreaPacketId.recv_0x166B, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x8066, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x825D, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x9899, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xBF0D, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xD04A, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xEDE7, res, ServerType.Area); Thread.Sleep(2000);

            //one int32 section
            res.WriteInt32(0);

            Router.Send(client, (ushort)MsgPacketId.recv_0x6C94, res, ServerType.Msg); Thread.Sleep(2000);
            Router.Send(client, (ushort)MsgPacketId.recv_0x831C, res, ServerType.Msg); Thread.Sleep(2000);

            Router.Send(client, (ushort)AreaPacketId.recv_0x218A, res, ServerType.Area); Thread.Sleep(2000); //007B4D90 <wizp | C2 0400          | ret 4
            Router.Send(client, (ushort)AreaPacketId.recv_0x2B7A, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x3A0E, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x3C1F, res, ServerType.Area); Thread.Sleep(2000);
            //Router.Send(client, (ushort)AreaPacketId.recv_0x3F2F, res, ServerType.Area); Thread.Sleep(2000); //Not a JP Op code

            Router.Send(client, (ushort)AreaPacketId.recv_chara_status_set_look_switch_r, res, ServerType.Area);Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x50D1, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x735E, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x8364, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x8487, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x8549, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x916, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_chara_status_set_comment_r, res, ServerType.Area); Thread.Sleep(2000);

            Router.Send(client, (ushort)AreaPacketId.recv_trade_offer_r, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x8549, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xD909, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xDA4A, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_chara_status_access_end_r, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xF024, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xFA0B, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xFB79, res, ServerType.Area); Thread.Sleep(2000);

            //double int32 section
            res = BufferProvider.Provide(); Thread.Sleep(2000);
            res.WriteInt32(0); Thread.Sleep(2000);
            res.WriteInt32(0); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x4D12, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x1489, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x692A, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x755C, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0x7697, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_noact_notify_heal_ap, res, ServerType.Area); Thread.Sleep(2000);
            Router.Send(client, (ushort)AreaPacketId.recv_0xCF29, res, ServerType.Area); Thread.Sleep(2000);




            //int32 int 16 sectons
            res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteInt16(0); 
            Router.Send(client, (ushort)AreaPacketId.recv_0x4ABB, res, ServerType.Area); Thread.Sleep(2000);

            //int32 byte section
            res = BufferProvider.Provide(); 
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_0x7B86, res, ServerType.Area); Thread.Sleep(2000);

            Router.Send(client, (ushort)AreaPacketId.recv_0xEEB7, res, ServerType.Area); Thread.Sleep(2000);


            //cstring section
            res = BufferProvider.Provide(); Thread.Sleep(2000);
            res.WriteCString("This is not the greatest test in the world! ohh no, this is just a Tribuuuute"); Thread.Sleep(2000);//find max size
            Router.Send(client, (ushort)AreaPacketId.recv_0xE983, res, ServerType.Area); 


        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test all non string protocols.";
    }
    //res.WriteInt32(numEntries); //less than 0x1E 
    //res.WriteInt32(0);
    //res.WriteInt64(0); 
    //res.WriteInt16(0); 
    //res.WriteByte(0);
    //res.WriteFixedString("Xeno", 0x10);
    //res.WriteCString("What");
    //res.WriteFloat(0);
}
