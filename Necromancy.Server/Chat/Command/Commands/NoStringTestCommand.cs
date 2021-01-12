using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.ItemModel;
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
            if (command[0] == "")
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid opcode: {command[0]}"));
                return;
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_0x2B7A, res, ServerType.Area);
        }
        //strings tested:0xFB79,recv_0xFA0B,recv_0xF024,recv_0xEEB7,recv_0xE7CF,recv_0xDA4A,recv_0xD909,recv_0xCF29,recv_0xC055,
        //recv_0xB684(disconnect),recv_0xB586,recv_0x97D9(battleRep), recv_0x916,recv_0x8549,recv_0x8487,recv_0x8364,recv_0x7B86,
        //recv_0x7697,recv_0x755C,recv_0x735E,recv_0x692A,recv_0x50D1,recv_0x4CF3,recv_0x4ABB,recv_0x3C1F,recv_0x3A0E,recv_0x218A,
        //recv_0x1489(disconnect),recv_0x4D12,recv_0x2B7A
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test a non string protocol.";
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
