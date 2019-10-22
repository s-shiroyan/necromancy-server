using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_create : ClientHandler
    {
        public send_chara_create(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_chara_create;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte character_slot_id = packet.Data.ReadByte();
            string character_name = packet.Data.ReadCString();
            uint race_id = packet.Data.ReadUInt32();
            uint sex_id = packet.Data.ReadUInt32();

            byte hair_id = packet.Data.ReadByte();
            byte hair_color_id = packet.Data.ReadByte();
            byte face_id = packet.Data.ReadByte();

            uint alignment_id = packet.Data.ReadUInt32();

            ushort strength = packet.Data.ReadUInt16();
            ushort vitality = packet.Data.ReadUInt16();
            ushort dexterity = packet.Data.ReadUInt16();
            ushort agility = packet.Data.ReadUInt16();
            ushort intelligence = packet.Data.ReadUInt16();
            ushort piety = packet.Data.ReadUInt16();
            ushort luck = packet.Data.ReadUInt16();

            uint class_id = packet.Data.ReadUInt32();
            uint unknown_a = packet.Data.ReadUInt32();


            //-------------------------------------
            // Send Character Creation packets to Database for laster use.

            Character character = new Character();
            character.Characterslotid = character_slot_id;
            character.Name = character_name;
            character.Raceid = race_id;
            character.Sexid = sex_id;
            character.HairId = hair_id;
            character.HairColorId = hair_color_id;
            character.FaceId = face_id;
            character.Alignmentid = alignment_id;
            character.Strength = strength;
            character.vitality = vitality;
            character.dexterity = dexterity;
            character.agility = agility;
            character.intelligence = intelligence;
            character.piety = piety;
            character.luck = luck;
            character.ClassId = class_id;
            character.AccountId = client.Account.Id;
            character.SoulId = client.Soul.Id;

            character.NewCharaProtocol = true;

            //----------------------------------------------------------
            // Character Slot ID

            if (!Database.InsertCharacter(character))
            {
                Logger.Error(client, $"Failed to create CharacterSlot: {character_slot_id}");
                client.Close();
                return;
            }

            client.Character = character;
            Logger.Info($"Created CharacterSlot: {character_slot_id}");
            Logger.Info($"Created CharacterName: {character_name}");
            Logger.Info($"Created race: {race_id}");
            Logger.Info($"Created sex: {sex_id}");
            Logger.Info($"Created hair: {hair_id}");
            Logger.Info($"Created hair color: {hair_color_id}");
            Logger.Info($"Created faceid: {face_id}");
            Logger.Info($"Created alignment_id: {alignment_id}");
            Logger.Info($"Created strength: {strength}");
            Logger.Info($"Created vitality: {vitality}");
            Logger.Info($"Created dexterity: {dexterity}");
            Logger.Info($"Created agility: {agility}");
            Logger.Info($"Created intelligence: {intelligence}");
            Logger.Info($"Created piety: {piety}");
            Logger.Info($"Created luck: {luck}");
            Logger.Info($"Created class_id: {class_id}");

            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------






            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(1);

            Router.Send(client, (ushort)MsgPacketId.recv_chara_create_r, res, ServerType.Msg);
        }
    }
}
