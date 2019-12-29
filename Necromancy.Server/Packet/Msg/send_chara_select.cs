using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

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
            int characterId = packet.Data.ReadInt32();
            Character character = Database.SelectCharacterById(characterId);
            if (character == null)
            {
                Logger.Error(client, $"No character for CharacterId: {characterId}");
                client.Close();
                return;
            }

            Server.Instances.AssignInstance(character);

            client.Character = character;
            client.UpdateIdentity();

            Logger.Debug(client, $"Selected Character: {character.Name}");

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); // error check
            res2.WriteInt32(0); // error check
            //sub_494c50
            res2.WriteInt32(character.MapId);
            res2.WriteInt32(character.MapId);
            res2.WriteInt32(20);
            res2.WriteInt16(4);
            //sub_4834C0
            res2.WriteByte(69);
            //sub_494B90 - for loop
            for (int i = 0; i < 0x80; i++)
            {
                res2.WriteInt32(character.MapId);
                res2.WriteFixedString($"Channel {i}", 97);
                res2.WriteByte(1); //bool 1 | 0
                res2.WriteInt16(0xFFFF); //Max players
                res2.WriteInt16(0xFF); //Current players
                res2.WriteByte(0);
                res2.WriteByte(0);
                //
            }

            res2.WriteByte(10); //# of channels
            Router.Send(client, (ushort) MsgPacketId.recv_chara_select_channel_r, res2, ServerType.Msg);

            //Logic to support your dead body
            DeadBody deadBody = new DeadBody();
            Server.Instances.AssignInstance(deadBody);
            character.DeadBodyInstanceId = deadBody.InstanceId;
            deadBody.CharacterInstanceId = character.InstanceId;
            character.movementId = character.InstanceId;
            Logger.Debug($"Dead Body Instance ID {deadBody.InstanceId}   |  Character Instance ID {character.InstanceId}");
            deadBody.CharaName = character.Name;
            deadBody.MapId = character.MapId;
            deadBody.X = character.X;
            deadBody.Y = character.Y;
            deadBody.Z = character.Z;
            deadBody.Heading = character.Heading;
            deadBody.RaceId = character.Raceid;
            deadBody.SexId = character.Sexid;
            deadBody.HairStyle = character.HairId;
            deadBody.HairColor = character.HairColorId;
            deadBody.FaceId = character.FaceId;
        }
    }
}
