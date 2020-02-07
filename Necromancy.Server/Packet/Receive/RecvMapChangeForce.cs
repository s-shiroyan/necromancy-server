using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvMapChangeForce : PacketResponse
    {
        private readonly Map _map;
        private readonly MapPosition _mapPosition;

        public RecvMapChangeForce(Map map, MapPosition mapPosition)
            : base((ushort) AreaPacketId.recv_map_change_force, ServerType.Area)
        {
            _map = map;
            _mapPosition = mapPosition;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_map.Id); //MapSerialID
            res.WriteInt32(_map.Id); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port
            if (_mapPosition == null)
            {
                res.WriteFloat(_map.X); //X Pos
                res.WriteFloat(_map.Y); //Y Pos
                res.WriteFloat(_map.Z); //Z Pos
                res.WriteByte((byte)(_map.Orientation)); //View offset
            }
            else
            {
                res.WriteFloat(_mapPosition.X); //X Pos
                res.WriteFloat(_mapPosition.Y); //Y Pos
                res.WriteFloat(_mapPosition.Z); //Z Pos
                res.WriteByte((byte)_mapPosition.Heading); //View offset
            }
            return res;
        }
    }
}
