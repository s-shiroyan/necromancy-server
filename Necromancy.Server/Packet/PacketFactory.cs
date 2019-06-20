using System;
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
        public const int PacketIdSize = 2;
        public const int PacketLengthTypeSize = 1;

        private bool _readHeader;
        private bool _readPacketLengthType;
        private uint _dataSize;
        private ushort _id;
        private int _position;
        private IBuffer _buffer;
        private readonly NecLogger _logger;
        private readonly NecSetting _setting;
        private byte[] _header;
        private PacketLengthType _packetLengthType;
        private byte _headerSize;

        public PacketFactory(NecSetting setting)
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            _setting = setting;
            Reset();
        }

        public byte[] Write(NecPacket packet, NecClient client)
        {
            // TODO update arrowgene service to write uint*

            byte[] data = packet.Data.GetAllBytes();
            IBuffer buffer = BufferProvider.Provide();
            ulong dataSize = (ushort) (data.Length + PacketIdSize);

            PacketLengthType packetLengthType;

            if (dataSize < byte.MaxValue)
            {
                packetLengthType = PacketLengthType.Byte;
                buffer.WriteByte((byte) packetLengthType);
                buffer.WriteByte((byte) dataSize);
            }
            else if (dataSize < ushort.MaxValue)
            {
                packetLengthType = PacketLengthType.UInt16;
                buffer.WriteByte((byte) packetLengthType);
                buffer.WriteInt16((ushort) dataSize);
            }
            else if (dataSize < uint.MaxValue)
            {
                packetLengthType = PacketLengthType.UInt32;
                buffer.WriteByte((byte) packetLengthType);
                buffer.WriteInt32((uint) dataSize);
            }
            else
            {
                _logger.Error($"{dataSize} to big");
                return null;
            }

            buffer.WriteInt16(packet.Id);
            buffer.WriteBytes(data);

            byte headerSize = CalculateHeaderSize(packetLengthType);
            packet.Header = buffer.GetBytes(0, headerSize);

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

                if (!_readPacketLengthType
                    && _buffer.Size - _buffer.Position > PacketLengthTypeSize)
                {
                    byte lengthType = _buffer.ReadByte();
                    if (!Enum.IsDefined(typeof(PacketLengthType), lengthType))
                    {
                        _logger.Error($"PacketLengthType: '{lengthType}' not found");
                        byte[] dataDump = _buffer.GetBytes(_buffer.Position - 1, _buffer.Size);
                        _logger.LogErrorPacket(client, dataDump, null);
                        Reset();
                        return packets;
                    }

                    _readPacketLengthType = true;
                    _packetLengthType = (PacketLengthType) lengthType;
                    _headerSize = CalculateHeaderSize(_packetLengthType);
                }

                if (_readPacketLengthType
                    && !_readHeader
                    && _buffer.Size - _buffer.Position >= _headerSize - PacketLengthTypeSize)
                {
                    // TODO aquire 1st byte differently incase -1 doesnt work
                    _header = _buffer.GetBytes(_buffer.Position - PacketLengthTypeSize, _headerSize);

                    switch (_packetLengthType)
                    {
                        case PacketLengthType.Byte:
                        {
                            _dataSize = _buffer.ReadByte();
                            break;
                        }
                        case PacketLengthType.UInt16:
                        {
                            _dataSize = _buffer.ReadUInt16();
                            break;
                        }
                        case PacketLengthType.UInt32:
                        {
                            _dataSize = _buffer.ReadUInt32();
                            break;
                        }
                        default:
                        {
                            // TODO update arrowgene service to read uint24 && int24
                            _logger.Error($"PacketLengthType: '{_packetLengthType}' not supported");
                            Reset();
                            return packets;
                        }
                    }

                    _dataSize -= PacketIdSize;
                    _id = _buffer.ReadUInt16();
                    _readHeader = true;
                }

                if (_readPacketLengthType
                    && _readHeader
                    && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    // TODO update arrowgene service to read uint
                    byte[] packetData = _buffer.ReadBytes((int) _dataSize);
                    IBuffer buffer = BufferProvider.Provide(packetData);
                    NecPacket packet = new NecPacket(_id, buffer);
                    packet.Header = _header;
                    packets.Add(packet);
                    _readPacketLengthType = false;
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
            _readPacketLengthType = false;
            _readHeader = false;
            _dataSize = 0;
            _id = 0;
            _position = 0;
            _buffer = null;
            _header = null;
        }

        private byte CalculateHeaderSize(PacketLengthType packetLengthType)
        {
            return (byte) (PacketLengthTypeSize + (packetLengthType + 1) + PacketIdSize);
        }

        private int CalculatePadding(int size)
        {
            return (4 - (size & 3)) & 3;
        }
    }
}