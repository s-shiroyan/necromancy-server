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
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //less than 0xA
            for (int k = 0; k < numEntries; k++)
            {
                //SUB 496800
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteFixedString("Xeno", 0x61);
                res.WriteFixedString("Xeno", 0x61);
                res.WriteFixedString("Xeno", 0x301);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0xA; k++) //must be 0xA
                {
                    res.WriteInt32(0);
                }
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                //End Sub 496800
            }
            for (int k = 0; k < 0x2; k++)
            {
                //sub 496AB0
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteFixedString("Xeno", 0x5B);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0xA; j++)
                {
                    res.WriteInt32(0);
                }
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);//suspect

            }

            Router.Send(client, (ushort)0x8433, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test a non string protocol.";
    }
    //res.WriteInt32(numEntries); //less than 0x1E 
    //res.WriteInt32(0);
    //res.WriteInt16(0); 
    //res.WriteByte(0);
    //res.WriteFixedString("Xeno", 0x10);
    //res.WriteCString("What");
    //res.WriteFloat(0);
}
