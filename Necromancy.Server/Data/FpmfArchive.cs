using System.Collections.Generic;

namespace Necromancy.Server.Data
{
    public class FpmfArchive
    {
        private uint _size;
        private uint _numFiles;
        private List<FpmfArchiveFile> _files;
        private int _datPathLen;
        private string _datPath;
        private int _keyLen;
        private byte[] _key;
        private byte[] _header;
        private uint _unknown0;
        private uint _unknown1;
        private uint _unknown2;
        private byte _unknown3;
        private byte _unknown4;
        private uint _unknown5;
        private uint _unknown6;
        private uint _unknown7;
        private uint _unknown8;
        private uint _unknown9;
        private uint _unknown10;
        private uint _unknown11;
        private uint _unknown12;
        private uint _unknown13;

        public uint Size
        {
            get => _size;
            set => _size = value;
        }
        public uint NumFiles
        {
            get => _numFiles;
            set => _numFiles = value;
        }

        public int DatPathLen
        {
            get => _datPathLen;
            set => _datPathLen = value;
        }
        public string DatPath
        {
            get => _datPath;
            set => _datPath = value;
        }

        public int KeyLen
        {
            get => _keyLen;
            set => _keyLen = value;
        }
        public byte[] Key
        {
            get => _key;
            set => _key = value;
        }

        public byte[] Header
        {
            get => _header;
            set => _header = value;
        }
        public FpmfArchive()
        {
            _files = new List<FpmfArchiveFile>();
        }

        public List<FpmfArchiveFile> GetFiles()
        {
            return new List<FpmfArchiveFile>(_files);
        }

        public void AddFile(FpmfArchiveFile file)
        {
            _files.Add(file);
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
        public uint Unknown2
        {
            get => _unknown2;
            set => _unknown2 = value;
        }
        public byte Unknown3
        {
            get => _unknown3;
            set => _unknown3 = value;
        }
        public byte Unknown4
        {
            get => _unknown4;
            set => _unknown4 = value;
        }
        public uint Unknown5
        {
            get => _unknown5;
            set => _unknown5 = value;
        }
        public uint Unknown6
        {
            get => _unknown6;
            set => _unknown6 = value;
        }
        public uint Unknown7
        {
            get => _unknown7;
            set => _unknown7 = value;
        }
        public uint Unknown8
        {
            get => _unknown8;
            set => _unknown8 = value;
        }
        public uint Unknown9
        {
            get => _unknown9;
            set => _unknown9 = value;
        }
        public uint Unknown10
        {
            get => _unknown10;
            set => _unknown10 = value;
        }
        public uint Unknown11
        {
            get => _unknown11;
            set => _unknown11 = value;
        }
        public uint Unknown12
        {
            get => _unknown12;
            set => _unknown12 = value;
        }
        public uint Unknown13
        {
            get => _unknown13;
            set => _unknown13 = value;
        }
    }
}
