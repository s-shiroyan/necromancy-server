using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Data
{
    public abstract class CsvReader<T>
    {
        private const int BufferSize = 128;
        protected readonly ILogger Logger;

        public CsvReader()
        {
            Logger = LogProvider.Logger(this);
        }

        public List<T> Read(string path)
        {
            Logger.Debug($"Reading {path}");
            List<T> items = new List<T>();
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                using (var fileStream = File.OpenRead(file.FullName))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            ProcessLine(line, items);
                        }
                    }
                }
            }

            return items;
        }

        protected abstract T CreateInstance(string[] properties);

        private void ProcessLine(string line, ICollection<T> items)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            if (line.StartsWith('#'))
            {
                // Ignoring Comment
                return;
            }

            string[] properties = line.Split(",");
            if (properties.Length <= 0)
            {
                return;
            }

            try
            {
                T item = CreateInstance(properties);
                items.Add(item);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
        }
    }
}
