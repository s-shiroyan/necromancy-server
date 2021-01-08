using System;
using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Anything related commands.  This is a sandbox
    /// </summary>
    public class RTestCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(RTestCommand));

        public RTestCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }
            int x = 1;
            if (!int.TryParse(command[1], out  x))
            {
                try
                {
                    string binaryString = command[1];
                    binaryString = binaryString.Replace("0b", "");
                    Logger.Debug(binaryString);
                    x = Convert.ToInt32(binaryString, 2);
                }
                catch
                {
                    responses.Add(ChatResponse.CommandError(client, $"no value specified. setting x to 1"));
                    return;
                }

            }

            if (!int.TryParse(command[2], out int y))
            {
                responses.Add(ChatResponse.CommandError(client, $"Good Job!"));
            }

            switch (command[0])
            {

                case "itemobject":
                    recv_data_notify_local_itemobject_data itemObject = new recv_data_notify_local_itemobject_data(client.Character, x);
                    Router.Send(client.Map, itemObject);
                    break;

                case "itemcharapost":
                    recv_item_chara_post_notify itemCharaPost = new recv_item_chara_post_notify(x, (byte)y);
                    Router.Send(client.Map, itemCharaPost);
                    break;

                case "itemuse":
                    RecvItemUseNotify itemUseNotify = new RecvItemUseNotify((long)x, (float)y);
                    Router.Send(client.Map, itemUseNotify);
                    RecvItemUse itemUse = new RecvItemUse(0 /*success*/, (float)y);
                    Router.Send(client.Map, itemUse);
                    break;

                case "dispitem":
                    RecvSoulDispItemNotifyData dispItem = new RecvSoulDispItemNotifyData(x);
                    Router.Send(client.Map, dispItem);
                    break;

                case "templeopen":
                    RecvTempleNotifyOpen templeOpen = new RecvTempleNotifyOpen((byte)x);
                    Router.Send(client.Map, templeOpen);
                    break;

                case "wantedstate":
                    RecvWantedUpdateState wantedState = new RecvWantedUpdateState(x);
                    Router.Send(client.Map, wantedState);
                    RecvWantedUpdateStateNotify wantedStateNotify = new RecvWantedUpdateStateNotify((int)client.Character.InstanceId, x);
                    Router.Send(client.Map, wantedStateNotify);
                    break;
                    



                default: //you don't know what you're doing do you?
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"{command[0]} is not a valid command."));
                    }
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "rtest";

        public override string HelpText =>
            "usage: `/rtest [argument] [number] [parameter]` - this is free-form for testing new recvs";
    }
}
