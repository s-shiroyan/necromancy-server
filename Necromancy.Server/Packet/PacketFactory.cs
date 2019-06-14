using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet
{
    public class PacketFactory
    {
        public const int HeaderSize = 4;
        public const int MaxPacketSize = 1048576;
        public const int HeaderSizeOffset = 2; // 2bytes for size are not included in header value for size.

        private bool _readHeader;
        private ushort _size;
        private ushort _id;
        private int _position;
        private IBuffer _buffer;
        private readonly NecLogger _logger;
        private readonly NecSetting _setting;
        private byte[] _header;

        public PacketFactory(NecSetting setting)
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            _setting = setting;
            Reset();
        }

        public byte[] Write(NecPacket packet, NecClient client)
        {
            byte[] data = packet.Data.GetAllBytes();
            IBuffer buffer = BufferProvider.Provide();
            ushort size = (ushort) (data.Length + HeaderSizeOffset);
            buffer.WriteInt16(size, Endianness.Big);
            buffer.WriteInt16(packet.Id);
            buffer.WriteBytes(data);
            packet.Header = buffer.GetBytes(0, HeaderSize);
            return buffer.GetAllBytes();
        }

        public List<NecPacket> Read(byte[] data, NecClient client)
        {
            List<NecPacket> packets = new List<NecPacket>();
            if (_buffer == null)
            {
                _buffer = BufferProvider.Provide(data);
            }
            else
            {
                _buffer.SetPositionEnd();
                _buffer.WriteBytes(data);
            }

            _buffer.Position = _position;

            bool read = true;
            while (read)
            {
                read = false;
                if (!_readHeader && _buffer.Size - _buffer.Position >= HeaderSize)
                {
                    _header = _buffer.GetBytes(0, HeaderSize);
                    _size = _buffer.ReadUInt16(Endianness.Big);
                    _size -= HeaderSizeOffset;
                    _id = _buffer.ReadUInt16();
                    if (_size > MaxPacketSize)
                    {
                        _logger.Error(client,
                            $"Packet Id: {_id} - Size: {_size} is bigger than maximum allowed Size: {MaxPacketSize}");
                        Reset();
                        return packets;
                    }

                    _readHeader = true;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _size)
                {
                    byte[] packetData = _buffer.ReadBytes(_size);
                    IBuffer buffer = BufferProvider.Provide(packetData);
                    NecPacket packet = new NecPacket(_id, buffer);
                    packet.Header = _header;
                    packets.Add(packet);
                    _readHeader = false;
                    read = _buffer.Position != _buffer.Size;
                }
            }

            if (_buffer.Position == _buffer.Size)
            {
                Reset();
            }
            else
            {
                _position = _buffer.Position;
            }

            return packets;
        }

        private void Reset()
        {
            _readHeader = false;
            _size = 0;
            _id = 0;
            _position = 0;
            _buffer = null;
            _header = null;
        }
    }
}