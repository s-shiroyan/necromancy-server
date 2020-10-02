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

            //res.WriteInt32(0);

            Router.Send(client, (ushort)0x1B22, res, ServerType.Area);


        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test a non string protocol.";
    }
}
