using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SSExec.Button.Core.Data.Contract;

namespace SSExec.Button.Core.Data
{
    public abstract class FileBasedRepository<T, TKey> : BaseRepository<T, TKey> where T : IEntity<TKey>, new()
    {
        protected readonly string FolderName;
        protected readonly ILog Log;

        protected abstract string GetFilename(TKey key);
        protected abstract string[] GetFilenames();

        protected abstract void EnsureDirectoryExists(string filename);


        private static Dictionary<TKey, T> _cache;

        private Dictionary<TKey, T> Cache
        {
            get
            {
                if (_cache == null)
                {
                    var files = GetFilenames();

                    // TODO: Don't forget to retrieve cache
                    //_cache = files.Select(GetFromFile).ToDictionary(x => x.Id);
                    return files.Select(GetFromFile).ToDictionary(x => x.Id);

                }

                return _cache;
            }
        }

        private void ResetCache()
        {
            _cache = null;
        }

        protected FileBasedRepository(string folderName, ILog log)
        {
            FolderName = folderName;
            Log = log;
            if (!Directory.Exists(FolderName)) Directory.CreateDirectory(folderName);
        }

        protected T GetFromFile(TKey id)
        {
            var filename = GetFilename(id);
            return GetFromFile(filename);
        }

        protected T GetFromFile(string filename)
        {
            if (!File.Exists(filename)) return new T();

            using (var sr = new StreamReader(filename))
            {
                var ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(sr);
            }
        }

        protected T UpdateFile(T item, bool ensureDirectoryExists = false)
        {
            var filename = GetFilename(item.Id);

            if (ensureDirectoryExists)
                EnsureDirectoryExists(filename);

            using (var sw = new StreamWriter(filename, false))
            {
                var ser = new XmlSerializer(typeof(T));
                ser.Serialize(sw, item);
            }
            return GetFromFile(item.Id);
        }

        public override IEnumerable<T> All()
        {
            return Cache.Values;
        }

        public override void Delete(T item)
        {
            var filename = GetFilename(item.Id);
            File.Delete(filename);
            Cache.Remove(item.Id);
        }

        public override T Update(T item)
        {
            UpdateFile(item);
            item = GetFromFile(item.Id);
            Cache[item.Id] = item;
            return item;
        }

        public override T Add(T item)
        {
            UpdateFile(item, true);
            item = GetFromFile(item.Id);
            Cache[item.Id] = item;
            return item;
        }

        protected override void DisposeManagedResources()
        {
            ResetCache();
        }
    }
}
