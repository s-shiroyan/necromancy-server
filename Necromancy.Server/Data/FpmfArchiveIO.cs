using System;
using System.IO;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Data
{
    public class FpmfArchiveIO
    {
        private static byte[] MagicBytes = {0x46, 0x50, 0x4D, 0x46};

        private static byte[] RotKey =
        {
            0x21, 0xB0, 0x18, 0x7A, 0xE5, 0x02, 0x3E, 0x5E, 0xDA, 0xA7, 0x48, 0x62, 0xDA, 0xF7, 0x5F, 0x04,
            0x66, 0xE8, 0x1B, 0xFB, 0x55, 0xA0, 0xF9, 0xBA, 0x14, 0x7E, 0x92, 0xE0, 0x02, 0xA6, 0x65, 0x7F,
            0xB4, 0xC3, 0xA7, 0x8C, 0xE9, 0x1F, 0x8C, 0x22, 0xBD, 0x52, 0xC7, 0x81, 0x93, 0xA1, 0xB9, 0x97,
            0x17, 0x05, 0x32, 0xCC, 0x43, 0xFF, 0xAA, 0xCD, 0x60, 0x5E, 0x79, 0x0E, 0x64, 0x0D, 0xD1, 0x0D,
            0x56, 0x2D, 0x08, 0xAE, 0xDC, 0x62, 0xDE, 0x5F, 0xB2, 0x78, 0xA2, 0xB0, 0x1E, 0xCF, 0xB1, 0x69,
            0x2C, 0x36, 0x16, 0x8B, 0xD1, 0x08, 0x86, 0x10, 0x07, 0x37, 0xB4, 0x8A, 0x41, 0x6B, 0x97, 0x59,
            0xA0, 0xAA, 0x2B, 0x64, 0x23, 0x9D, 0xC9, 0xEC, 0xBF, 0xB3, 0xD5, 0x17, 0x2F, 0xBB, 0x94, 0xB5,
            0xB0, 0x21, 0x15, 0xD4, 0xF0, 0xFA, 0xA2, 0x31, 0xA5, 0x50, 0x9C, 0x80, 0x14, 0x01, 0x41, 0xA0,
            0xE9, 0x8C, 0x8B, 0x47, 0x90, 0x58, 0x96, 0x3D, 0xDB, 0x5F, 0xD0, 0xFD, 0xEA, 0x3E, 0x75, 0x5E,
            0xB6, 0xED, 0x2E, 0x08, 0xBD, 0x17, 0x4F, 0xB0, 0x9E, 0xBB, 0x7E, 0x75, 0x37, 0x20, 0x90, 0xE1,
            0xC7, 0x94, 0x4D, 0x67, 0x64, 0x66, 0xD0, 0x1A, 0x1F, 0x42, 0x78, 0x10, 0x77, 0xAF, 0x14, 0xE9,
            0x9B, 0xD6, 0x3C, 0x5A, 0x1C, 0x1D, 0x55, 0x6C, 0x03, 0xFA, 0x57, 0x68, 0x07, 0x3F, 0xF6, 0x4F,
            0xA0, 0x33, 0x11, 0xEA, 0xC5, 0xD0, 0x4A, 0xC0, 0xEC, 0xAC, 0xDD, 0xF2, 0xBA, 0x8D, 0xAB, 0x73,
            0x5E, 0xE1, 0xCD, 0x1B, 0xB7, 0xD6, 0x53, 0x1A, 0x78, 0xED, 0x9A, 0x14, 0x32, 0xE1, 0x49, 0x62,
            0xE0, 0x15, 0x27, 0xE5, 0xF8, 0x42, 0x1D, 0x61, 0xBE, 0x42, 0x11, 0x43, 0x64, 0xCF, 0x31, 0x63,
            0x0E, 0x5D, 0xFC, 0x85, 0x2D, 0x4B, 0x65, 0x86, 0xD2, 0xA5, 0x0A, 0x2D, 0x1B, 0xDD, 0x55, 0xF4,
            0x81
        };


        private ILogger _logger;


        public FpmfArchiveIO()
        {
            _logger = LogProvider.Logger(this);
        }

        public FpmfArchive Open(string hedFilePath)
        {
            FileInfo file = new FileInfo(hedFilePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException($"File: {hedFilePath} not found.");
            }

            IBuffer buffer = new StreamBuffer(hedFilePath);

            if (buffer.Size < 12)
            {
                throw new Exception("File to small");
            }

            buffer.SetPositionStart();
            byte[] magicBytes = buffer.ReadBytes(4);
            for (int i = 0; i < 4; i++)
            {
                if (magicBytes[i] != MagicBytes[i])
                {
                    throw new Exception("Invalid File");
                }
            }

            FpmfArchive archive = new FpmfArchive();
            archive.Size = buffer.ReadUInt32();
            uint unknown0 = buffer.ReadUInt32();

            buffer = DecryptHed(buffer);
            buffer.SetPositionStart();

            uint unknown1 = buffer.ReadUInt32();
            uint unknown2 = buffer.ReadUInt32();
            byte unknown3 = buffer.ReadByte();
            byte unknown4 = buffer.ReadByte();
            uint unknown5 = buffer.ReadUInt32();
            uint unknown6 = buffer.ReadUInt32();
            int strLen = (int) unknown6 - 8;
            string unknown7 = buffer.ReadString(strLen);
            uint unknown8 = buffer.ReadUInt32();
            uint unknown44 = buffer.ReadUInt32();
            uint unknown13 = buffer.ReadUInt32();
            uint unknown14 = buffer.ReadUInt32();
            uint keyLen = buffer.ReadUInt32();
            byte[] key = buffer.ReadBytes((int) keyLen);
            uint unknown15 = buffer.ReadUInt32();
            uint unknown16 = buffer.ReadUInt32();
            uint unknown17 = buffer.ReadUInt32();

            // index

            
            strLen = buffer.ReadByte();
            string directoryPath = buffer.ReadString(strLen);
            strLen = buffer.ReadByte();
            string filePath = buffer.ReadString(strLen);
            uint unknown18 = buffer.ReadUInt32();
            uint unknown19 = buffer.ReadUInt32();
            uint fileSize = buffer.ReadUInt32();
            uint fileOffset = buffer.ReadUInt32();
            uint unknown22 = buffer.ReadUInt32();
            
            return archive;
        }

        public void Decrypt(string hedFilePath, string outDirectoryPath)
        {
            FileInfo file = new FileInfo(hedFilePath);
            if (!file.Exists)
            {
                throw new FileNotFoundException($"File: {hedFilePath} not found.");
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(outDirectoryPath);
            if (!directoryInfo.Exists)
            {
                throw new FileNotFoundException($"Directory: {hedFilePath} not found.");
            }

            string outPath = directoryInfo.FullName + file.Name;

            IBuffer buffer = new StreamBuffer(hedFilePath);
            buffer = DecryptHed(buffer);
            File.WriteAllBytes(outPath, buffer.GetAllBytes());
        }

        /// <summary>
        /// 0xA7E480
        /// </summary>
        private IBuffer DecryptHed(IBuffer buffer)
        {
            byte dl = 0xA6;
            byte bl = 0;
            byte al = 0;
            byte sub = 0x21;
            buffer.Position = 12;
            IBuffer outBuffer = new StreamBuffer();
            while (buffer.Position < buffer.Size)
            {
                byte cl = buffer.ReadByte();
                bl = al;
                bl = (byte) (bl + dl);
                bl = (byte) (bl ^ cl);
                bl = (byte) (bl - sub);
                al = (byte) (al + 1);
                dl = cl;
                outBuffer.WriteByte(bl);
            }

            return outBuffer;
        }

        /// <summary>
        /// 0xA7E3F0
        /// </summary>
        private IBuffer DecryptDat(IBuffer buffer, int fileOffset, int fileLength)
        {
            if (fileOffset + fileLength > buffer.Size)
            {
                throw new Exception("Buffer to small");
            }

            int rotKeyIndex = 0;
            buffer.Position = fileOffset;
            IBuffer outBuffer = new StreamBuffer();
            while (buffer.Position < fileLength)
            {
                byte al = buffer.ReadByte();
                al = (byte) (al - rotKeyIndex);
                outBuffer.WriteByte(al);
                rotKeyIndex++;
                if (rotKeyIndex > RotKey.Length)
                {
                    rotKeyIndex = 0;
                }
            }

            return outBuffer;
        }
    }
}