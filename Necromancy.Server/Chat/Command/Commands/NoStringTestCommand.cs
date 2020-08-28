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

           // IBuffer res = BufferProvider.Provide();


            //foreach (InventoryItem inventoryItem in client.Character.Inventory._inventory[0])
            //{
            //    res = BufferProvider.Provide();
            //    res.WriteInt64(inventoryItem.Id);
            //    res.WriteInt32(inventoryItem.Item.Durability+1); // Current durability points
            //    Router.Send(client, (ushort)AreaPacketId.recv_item_update_durability, res, ServerType.Area);
            //    inventoryItem.Item.Durability += 1;
            //    //When repairing this should be item's max durability.
            //}

        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test a non string protocol.";
    }
}
