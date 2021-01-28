using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventMessageNoObject : PacketResponse
    {
        private readonly string _name;
        private readonly string _title;
        private readonly string _text;

        public RecvEventMessageNoObject(string name, string title, string text)
            : base((ushort) AreaPacketId.recv_event_message_no_object, ServerType.Area)
        {
            _name = name;
            _title = title;
            _text = text;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(_name.ToString());//Name (need to find max size)
            res.WriteCString(_title.ToString());//Title (need to find max size)
            res.WriteCString(_text.ToString());//Text block (need to find max size)
            return res;
        }
    }
}
