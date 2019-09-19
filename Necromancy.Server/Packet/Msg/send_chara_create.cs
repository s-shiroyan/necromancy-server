using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_create : Handler
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


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check 0 is good
            res.WriteInt32(15100901);

            Router.Send(client, (ushort)MsgPacketId.recv_chara_create_r, res);
        }
    }
}
