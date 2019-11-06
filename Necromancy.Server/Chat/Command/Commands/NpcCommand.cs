using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class NpcCommand : ServerChatCommand
    {
        public NpcCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!int.TryParse(command[0], out int npcId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!Server.SettingRepository.Npc.TryGetValue(npcId, out NpcSetting npcSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid NpcId: {npcId}"));
                return;
            }

            if (!int.TryParse(command[1], out int modelId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                return;
            }

            if (!Server.SettingRepository.ModelCommon.TryGetValue(modelId, out ModelCommonSetting modelSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid ModelId: {modelId}"));
                return;
            }

            int numEntries = 0; // 1 to 19 equipment.  Setting to 0 because NPCS don't wear gear.
            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(Server.IdGenerator.GetId()); // NPC ID (object id)

            res3.WriteInt32(npcSetting.Id); // NPC Serial ID from "npc.csv"

            res3.WriteByte(0); // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

            res3.WriteCString(npcSetting.Name); //Name

            res3.WriteCString(npcSetting.Title); //Title

            res3.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150)); //X Pos
            res3.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150)); //Y Pos
            res3.WriteFloat(client.Character.Z); //Z Pos
            res3.WriteByte(client.Character.viewOffset); //view offset

            res3.WriteInt32(numEntries); // # Items to Equip

            for (int i = 0; i < numEntries; i++)

            {
                res3.WriteInt32(24);
            }

            res3.WriteInt32(numEntries); // # Items to Equip

            for (int i = 0; i < numEntries; i++)

            {
                // loop start
                res3.WriteInt32(210901); // this is a loop within a loop i went ahead and broke it up
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(3);

                res3.WriteInt32(10310503);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(3);

                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(1); // bool
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
                res3.WriteByte(0);
            }

            res3.WriteInt32(numEntries); // # Items to Equip

            for (int i = 0; i < numEntries; i++) // Item type bitmask per slot

            {
                res3.WriteInt32(1);
            }

            res3.WriteInt32(modelSetting.Id); //NPC Model from file "model_common.csv"

            res3.WriteInt16(100); //NPC Model Size

            res3.WriteByte(2);

            res3.WriteByte(5);

            res3.WriteByte(6);

            res3.WriteInt32(
                0); //Hp Related Bitmask?  This setting makes the NPC "alive"    11111110 = npc flickering, 0 = npc alive

            res3.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull

            res3.WriteInt32(8); // add strange light on certain npc
            res3.WriteFloat(0); //x for icons
            res3.WriteFloat(0); //y for icons
            res3.WriteFloat(50); //z for icons

            res3.WriteInt32(128);

            int numEntries2 = 128;


            for (int i = 0; i < numEntries2; i++)

            {
                res3.WriteInt32(0);
                res3.WriteInt32(0);
                res3.WriteInt32(0);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_npc_data, res3, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "npc";
    }
}
