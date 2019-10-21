using System.Collections.Generic;
using System.IO;
using Arrowgene.Services.Logging;
using Necromancy.Server.Data;
using Necromancy.Server.Data.Settings;

namespace Necromancy.Server.Model
{
    public class Repository
    {
        private DirectoryInfo _directory;
        private readonly ILogger _logger;

        public Repository(string folder)
        {
            _logger = LogProvider.Logger(this);
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                _logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            Items = new Dictionary<int, Item>();
        }

        public Dictionary<int, Item> Items { get; }


        public Repository Initialize()
        {
            Items.Clear();
            Load(Items, "iteminfo.csv", new ItemInfoCsvReader());
            return this;
        }

        private void Load<T>(Dictionary<int, T> dictionary, string fileName, CsvReader<T> reader)
        {
            string path = Path.Combine(_directory.FullName, fileName);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                _logger.Error($"Could not load '{fileName}', file does not exist");
            }

            List<T> items = reader.Read(file.FullName);
            foreach (T item in items)
            {
                if (item is IRepositoryItem repositoryItem)
                {
                    dictionary.Add(repositoryItem.Id, item);
                }
            }
        }
    }
}
