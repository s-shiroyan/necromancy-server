using System.Collections.Generic;

namespace Necromancy.Server.Data
{
    public class FpmfArchive
    {
        private uint _size;
        private List<FpmfArchiveFile> _files;
        private string _datPath;
        private byte[] _key;

        public uint Size
        {
            get => _size;
            set => _size = value;
        }

        public string DatPath
        {
            get => _datPath;
            set => _datPath = value;
        }

        public byte[] Key
        {
            get => _key;
            set => _key = value;
        }

        public string GetDatDirectory()
        {
            return DatPath.Replace("%08x.dat", "");
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
    }
}