using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvDataNotifyObjectData : PacketResponse
    {
        private readonly Object _objectData;
        public RecvDataNotifyObjectData(Object objectData)
            : base((ushort) AreaPacketId.recv_data_notify_itemobject_data, ServerType.Area)
        {
            _objectData = objectData;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_objectData.InstanceId);
            res.WriteFloat(_objectData.ObjectCoord.X);
            res.WriteFloat(_objectData.ObjectCoord.Y);
            res.WriteFloat(_objectData.ObjectCoord.Z);

            res.WriteFloat(_objectData.TriggerCoord.X);
            res.WriteFloat(_objectData.TriggerCoord.Y);
            res.WriteFloat(_objectData.TriggerCoord.Z);
            res.WriteByte(_objectData.Heading);

            res.WriteInt32(_objectData.Bitmap1);
            res.WriteInt32(_objectData.Unknown1);
            res.WriteInt32(_objectData.Unknown2);

            res.WriteInt32(_objectData.Bitmap2);

            res.WriteInt32(_objectData.Unknown3);
            return res;
        }
    }
}
