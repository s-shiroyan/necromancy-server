using System.IO;
using Arrowgene.Services.Buffers;

namespace Necromancy.Server.Data
{
    public class FpmfArchive
    {
        public FpmfArchive()
        {
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

        private void DecryptDat()
        {
        }
    }
}