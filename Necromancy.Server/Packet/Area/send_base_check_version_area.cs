using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_base_check_version_area : Handler
    {
        public send_base_check_version_area(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_base_check_version;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint unknown = packet.Data.ReadUInt32();
            uint major = packet.Data.ReadUInt32();
            uint minor = packet.Data.ReadUInt32();
            Logger.Info($"{major} - {minor}");

            IBuffer res = BufferProvider.Provide();

            int numEntries = 0x1E;
            res.WriteInt32(numEntries); //less than or equal to 0x1E
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteFixedString("", 0x61);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x61);
                res.WriteByte(0);//bool
                res.WriteByte(0);//bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                for (int j = 0; j < 0xA; j++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                }
                res.WriteByte(0);
                for (int k = 0; k < 0xC; k++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                }
                res.WriteByte(0);

                res.WriteFixedString("", 0x181);
                res.WriteFixedString("", 0x181);
                for (int l = 0; l < 0x5; l++)
                {
                    res.WriteByte(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                }
                res.WriteByte(0);
            }
           
            Router.Send(client, (ushort) AreaPacketId.recv_base_check_version_r, res);
        }
    }
}