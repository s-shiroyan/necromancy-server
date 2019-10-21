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
            Console.WriteLine($"Character ID In Selected Slot = {CharacterIdInSelectedSlot}");

            foreach (Character myCharacter in Database.SelectCharacterBySoulId(client.Character.SoulId))
            {
                Console.WriteLine($"CharacterSlotId: {myCharacter.Id} Comparing to CharacterIdInSelectedSlot: {CharacterIdInSelectedSlot}");
                if (myCharacter.Id == CharacterIdInSelectedSlot)
                {
                    int[] MyWeaponType = new int[] { 14, 8, 15, 8, 10, 10 };
                    client.Character = myCharacter;
                    client.Character.WeaponType = MyWeaponType[client.Character.Characterslotid];
                    Console.WriteLine($"Found a Match! myCharacter.Id: {myCharacter.Id} is equal to CharacterIdInSelectedSlot: {CharacterIdInSelectedSlot}");

                    //Settings for Map Entry until settings are databased.
                    client.Character.MapId = 1001002;//Set your map here. See reference table in "MapSettings.CS"
                    string[] theMapSettings = MapSetting.MapLoadInfo(client.Character.MapId);

                    client.Character.X = float.Parse(theMapSettings[6]);
                    client.Character.Y = float.Parse(theMapSettings[7]);
                    client.Character.Z = float.Parse(theMapSettings[8]);
                    //client.Character.viewOffset = byte.Parse(theMapSettings[9]);
                    break;
                }
            }
            if (CharacterIdInSelectedSlot < 6)
            {

                IBuffer res = BufferProvider.Provide();

                res.WriteInt32(0);//Error

                //sub_4E4210_2341  // impacts map spawn ID
                res.WriteInt32(1001002);//MapSerialID
                res.WriteInt32(1001002);//MapID
                res.WriteFixedString("127.0.0.1", 65);//IP
                res.WriteInt16(60002);//Port

                //sub_484420   //  does not impact map spawn coord
                res.WriteFloat(0);//X Pos
                res.WriteFloat(-0);//Y Pos
                res.WriteFloat(0);//Z Pos
                res.WriteByte(0);//View offset
                                 //

                Router.Send(client, (ushort)MsgPacketId.recv_channel_select_r, res, ServerType.Msg);

                client.Character.NewCharaProtocol = true;

            }

            if (CharacterIdInSelectedSlot > 6)
            {
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
}