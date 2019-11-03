using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Data.Setting
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

        /// <summary>
        /// Defines the minimum number of items to expect.
        /// If the entry has less items it will be discarded without processing.
        /// </summary>
        protected abstract int NumExpectedItems { get; }

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

            if (properties.Length < NumExpectedItems)
            {
                return;
            }

            T item = default;
            try
            {
                item = CreateInstance(properties);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }

            if (item == null)
            {
                return;
            }

            items.Add(item);
        }
    }
}
