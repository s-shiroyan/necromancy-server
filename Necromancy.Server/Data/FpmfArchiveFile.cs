namespace Necromancy.Server.Data
{
    public class FpmfArchiveFile
    {
        private uint _size;
        private uint _offset;
        private uint _datNumber;
        private string _filePath;
        private uint _filePathSize;
        private string _directoryPath;
        private uint _directoryPathSize;
        private byte[] _data;
        private uint _unknown0;
        private uint _unknown1;

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

        public uint FilePathSize
        {
            get => _filePathSize;
            set => _filePathSize = value;
        }
        public string DirectoryPath
        {
            get => _directoryPath;
            set => _directoryPath = value;
        }

        public uint DirectoryPathSize
        {
            get => _directoryPathSize;
            set => _directoryPathSize = value;
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
        public uint Unknown0
        {
            get => _unknown0;
            set => _unknown0 = value;
        }
        public uint Unknown1
        {
            get => _unknown1;
            set => _unknown1 = value;
        }

        public FpmfArchiveFile()
        {
        }
    }
}
