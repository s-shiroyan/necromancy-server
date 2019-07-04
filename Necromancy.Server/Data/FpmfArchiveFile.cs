namespace Necromancy.Server.Data
{
    public class FpmfArchiveFile
    {
        private uint _size;
        private uint _offset;
        private uint _datNumber;
        private string _filePath;
        private string _directoryPath;
        private byte[] _data;

        public uint Size
        {
            get => _size;
            set => _size = value;
        }

        public uint Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public string FilePath
        {
            get => _filePath;
            set => _filePath = value;
        }

        public string DirectoryPath
        {
            get => _directoryPath;
            set => _directoryPath = value;
        }

        public uint DatNumber
        {
            get => _datNumber;
            set => _datNumber = value;
        }

        public byte[] Data
        {
            get => _data;
            set => _data = value;
        }
        
        public FpmfArchiveFile()
        {
        }
    }
}