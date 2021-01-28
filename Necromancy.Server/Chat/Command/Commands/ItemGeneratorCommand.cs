using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Chat.Command.Commands
{
    class ItemGeneratorCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemCommand));

        public ItemGeneratorCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "genitems";
        public override string HelpText => "usage: `/genitems [package]`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, "To few arguments"));
                return;
            }

            switch (command[0])
            {
                case "bags":
                    spawnBags(client);
                    break;
                case "a_darkmerchant":
                    spawnAvatarDarkMerchant(client);
                    break;
                case "charmed":
                    SpawnCharmedGear(client);
                    break;
                default:
                    responses.Add(ChatResponse.CommandError(client, $"Invalid Package: {command[0]}"));
                    return;
            }
            
        }

        private void SpawnCharmedGear(NecClient client)
        {
            int[] itemIds = new int[5];
            itemIds[0] = 120308;
            itemIds[1] = 220308;
            itemIds[2] = 320308;
            itemIds[3] = 420308;
            itemIds[4] = 520308;
            SendItems(client, itemIds);
        }

        private void spawnAvatarDarkMerchant(NecClient client)
        {
            int[] itemIds = new int[5];
            itemIds[0] = 162502;
            itemIds[1] = 262502;
            itemIds[2] = 362502;
            itemIds[3] = 462502;
            itemIds[4] = 562502;
            SendItems(client, itemIds);
        }

        private void spawnBags(NecClient client)
        {
            int[] itemIds = new int[10];
            itemIds[0] = 50100501;
            itemIds[1] = 50100502;
            itemIds[2] = 50100503;
            itemIds[3] = 50100504;
            itemIds[4] = 50100505;
            itemIds[5] = 50100506;
            itemIds[6] = 50100507;
            itemIds[7] = 50100511;
            itemIds[8] = 90012001; //royal bag 16 slots
            itemIds[9] = 90012002; //deluxe royal bag - 24 slots??
            
            SendItems(client, itemIds);
        }

        private void SendItems(NecClient client, int[] itemIds)
        {
            ItemSpawnParams[] spawmParams = new ItemSpawnParams[itemIds.Length];
            for (int i = 0; i < itemIds.Length; i++)
            {
                spawmParams[i] = new ItemSpawnParams();
                spawmParams[i].ItemStatuses = ItemStatuses.Identified;
            }
            ItemService itemService = new ItemService(client.Character);
            List<ItemInstance> items = itemService.SpawnItemInstances(ItemZoneType.AdventureBag, itemIds, spawmParams);
            RecvSituationStart recvSituationStart = new RecvSituationStart();
            RecvSituationEnd recvSituationEnd = new RecvSituationEnd();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2);
            Router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);
            foreach (ItemInstance itemInstance in items)
            {
                Logger.Debug(itemInstance.Type.ToString());
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                Router.Send(client, recvItemInstance.ToPacket());
            }
            res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
        }
    }
}

