using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_select : ClientHandler
    {
        public send_chara_select(NecServer server) : base(server)
        {

        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte CharacterIdInSelectedSlot = packet.Data.ReadByte();
            Logger.Debug(client, $"Character ID In Selected Slot = {CharacterIdInSelectedSlot}");
            Logger.Debug(client, $"soul ID  = {client.Character.SoulId}");

            if (client.Character.NewCharaProtocol == false)
            {
                foreach (Character myCharacter in Database.SelectCharacterBySoulId(client.Character.SoulId))
                {
                    Logger.Debug(client, $"CharacterSlotId: {myCharacter.Id} Comparing to CharacterIdInSelectedSlot: {CharacterIdInSelectedSlot}");
                    if (myCharacter.Id == CharacterIdInSelectedSlot)
                    {
                        client.Character = myCharacter;
                        Logger.Debug(client, $"Found a Match! myCharacter.Id: {myCharacter.Id} is equal to CharacterIdInSelectedSlot: {CharacterIdInSelectedSlot}");
                        break;
                    }
                }
            }

                //Settings for Map Entry until settings are databased.
                client.Character.MapId = 1001007;//Set your map here. See reference table in "MapSettings.CS"

                if (client.Character.NewCharaProtocol == true) { client.Character.MapId = 1001902; }

                string[] theMapSettings = MapSetting.MapLoadInfo(client.Character.MapId);

                client.Character.X = float.Parse(theMapSettings[6]);
                client.Character.Y = float.Parse(theMapSettings[7]);
                client.Character.Z = float.Parse(theMapSettings[8]);
                //client.Character.viewOffset = byte.Parse(theMapSettings[9]);

            IBuffer res2 = BufferProvider.Provide();

                res2.WriteInt32(0); // error check
                res2.WriteInt32(0); // error check

                //sub_494c50
                res2.WriteInt32(128);
                res2.WriteInt32(2);
                res2.WriteInt32(88888888);
                res2.WriteInt16(4);
                //sub_4834C0
                res2.WriteByte(69);

                //sub_494B90 - for loop
                for (int i = 0; i < 0x80; i++)
                {
                    res2.WriteInt32(i);
                    res2.WriteFixedString($"Channel {i}", 97);
                    res2.WriteByte(1);   //bool 1 | 0
                    res2.WriteInt16(0xFFFF);  //Max players
                    res2.WriteInt16(0xFF);  //Current players
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    //
                }


                res2.WriteByte(10); //# of channels

                Router.Send(client, (ushort)MsgPacketId.recv_chara_select_channel_r, res2, ServerType.Msg);
            
        }
    }
}