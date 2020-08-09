using System.Collections.Generic;
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
            if (!int.TryParse(command[0], out int x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            IBuffer res = BufferProvider.Provide();

            for (int i = 0; i < 5; i++)
            {
                res.WriteFixedString("aaaa", 0x31);
                res.WriteFixedString("bbbb", 0x25);
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteInt64(1111111111111);
                res.WriteUInt32(client.Character.InstanceId);
            }
            res.WriteFixedString("dddd", 0x31);
            res.WriteFixedString("  cccc", 0x25);
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteInt64(111111111111111);
            res.WriteUInt32(client.Character.InstanceId);
            Router.Send(client, (ushort)0x9201, res, ServerType.Area);

        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test a non string protocol.";
    }
}
