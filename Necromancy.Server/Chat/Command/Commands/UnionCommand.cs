using System.Collections.Generic;
using Necromancy.Server.Model;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Does Union stuff
    /// </summary>
    public class UnionCommand : ServerChatCommand
    {
        public UnionCommand(NecServer server) : base(server)
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
            IBuffer res36 = BufferProvider.Provide();

            switch (command[0])
            {
                case "union":
                    res36.WriteInt32(client.Character.InstanceId);
                    res36.WriteInt32(client.Character.unionId);
                    res36.WriteCString("Trade_Union");
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
                    break;

                case "xunion":
                    res36.WriteInt32(client.Character.InstanceId);
                    res36.WriteInt32(0 /*client.Character.UnionId*/);
                    res36.WriteCString("");
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
                    break;

                case "disband":
                    Router.Send(client.Map, (ushort)MsgPacketId.recv_union_notify_disband, res36, ServerType.Msg);

                    res36.WriteInt32(0); //error check
                    Router.Send(client, (ushort)AreaPacketId.recv_union_request_disband_result, res36, ServerType.Area);

                    IBuffer res37 = BufferProvider.Provide();
                    res37.WriteInt32(client.Character.unionId);
                    Router.Send(client, (ushort)MsgPacketId.recv_union_request_disband_r, res37, ServerType.Msg);

                    IBuffer res40 = BufferProvider.Provide();
                    res40.WriteInt32(client.Character.InstanceId);
                    res40.WriteInt32(0 /*client.Character.UnionId*/);
                    res40.WriteCString("");
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_union_data, res40, ServerType.Area);

                    client.Character.unionId = 0;

                    break;

                case "open":
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_union_open_window, res36, ServerType.Area);
                    break;

                case "storage":
                    res36.WriteInt64(1);
                    res36.WriteInt64(1);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_event_union_storage_open, res36, ServerType.Area);
                    break;

                case "establish":
                    res36.WriteInt32(0); //error check
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_union_request_establish_r, res36, ServerType.Area);
                    break;

                case "rename":
                    res36.WriteCString("YouDidIt"); 
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_union_rename_open, res36, ServerType.Area);

                    IBuffer res38 = BufferProvider.Provide();
                    res38.WriteInt32(0); //error check
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_union_request_rename_r, res38, ServerType.Area);
                    break;
                case "resetid":
                    client.Character.unionId = 0;
                    break;

                case "growth":
                    res36.WriteInt32(0); //error check
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_union_request_growth_result, res36, ServerType.Area);
                    IBuffer res39 = BufferProvider.Provide();
                    res39.WriteInt16(3); //sets union current exp
                    Router.Send(client.Map, (ushort)MsgPacketId.recv_union_notify_growth, res39, ServerType.Msg);
                    break;



                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    break;

            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "union";
        public override string HelpText => "usage: `/union [command]` - Does something Union related.";
    }
}
