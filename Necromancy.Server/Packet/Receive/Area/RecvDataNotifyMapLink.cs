using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyMapLink : PacketResponse
    {
        private readonly int _mapId;
        private readonly MapPosition _mapPos;
        private readonly int _offset;
        private readonly int _width;
        private readonly int _color;

        public RecvDataNotifyMapLink(NecClient client, int mapId, MapPosition mapPos, int offset, int width,
            int color = 2)
            : base((ushort) AreaPacketId.recv_data_notify_maplink, ServerType.Area)
        {
            _mapId = mapId;
            _mapPos = mapPos;
            _offset = offset;
            _width = width;
            _color = color;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide(); // it's the aura portal for map
            res.WriteInt32(_mapId); // Unique ID

            res.WriteFloat(_mapPos.X); //x
            res.WriteFloat(_mapPos.Y); //y
            res.WriteFloat(_mapPos.Z + 2); //z
            res.WriteByte(_mapPos.Heading); // offset

            res.WriteFloat(_width); // Width
            res.WriteFloat(_offset); // Offset from map coords

            res.WriteInt32(_color); // Aura color 0=blue 1=gold 2=white 3=red 4=purple 5=black  0 to 5, crash above 5
            return res;
        }
    }
}
