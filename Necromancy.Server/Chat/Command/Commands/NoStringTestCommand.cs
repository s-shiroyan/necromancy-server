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

            res.WriteInt32(0);
            res.WriteInt32(numEntries); //less than 0x1E

            if (numEntries == 0)
            {
                res.WriteInt32(0);
            }
            else
            {
                //sub_4936D0
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteFixedString("Test", 0x61);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("SecondTest", 0x61);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                for (int i = 0; i < 0xA; i++)
                {
                    //sub 493640
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }

                res.WriteByte(0);

                for (int i = 0; i < 0xC; i++)
                {
                    //sub 493640
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }

                res.WriteByte(0);

                res.WriteInt32(0);
                //Ret

                res.WriteFixedString("", 0x181);
                res.WriteFixedString("", 0x181);
                for (int i = 0; i < 0x5; i++)
                {
                    res.WriteByte(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                }
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt32(0);
                //Ret
                res.WriteInt32(0);
            }
            Router.Send(client, (ushort)0x1ACA, res, ServerType.Area);


        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test a non string protocol.";
    }
}
