using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SSExec.Button.Core.Data.Contract;

namespace SSExec.Button.Core.Data
{
    public class SingleFileRepository<T, TKey> : BaseRepository<T, TKey> where T : IEntity<TKey>, new()
    {
        private readonly string _fileName;
        private List<T> _cache;

        public SingleFileRepository(string fileName)
        {
            _fileName = fileName;
            _cache = ReadCache(_fileName);
        }

        private List<T> ReadCache(string filename)
        {
            if (!File.Exists(filename)) return new List<T>();

            using (var sr = new StreamReader(filename))
            {
                var ser = new XmlSerializer(typeof (List<T>));
                return (List<T>) ser.Deserialize(sr);
            }
        }

        private void WriteCache(string filename, List<T> cache)
        {
            if (File.Exists(filename)) File.Create(filename);

            using (var sw = new StreamWriter(filename, false))
            {
                var ser = new XmlSerializer(typeof (List<T>));
                ser.Serialize(sw, cache);
            }
        }


        public override IEnumerable<T> All()
        {
            return _cache;
        }

        public override void Delete(T item)
        {
            _cache = _cache.Where(x => !x.Id.Equals(item.Id)).ToList();
            WriteCache(_fileName, _cache);
        }

        public override T Update(T item)
        {
            var el = _cache.SingleOrDefault(x => x.Id.Equals(item.Id));
            if (el == null) return default(T);
            _cache.Remove(el);
            _cache.Add(item);
            WriteCache(_fileName, _cache);
            return item;
        }

        public override T Add(T item)
        {
            _cache = _cache.Where(x => !x.Id.Equals(item.Id)).ToList();
            _cache.Add(item);
            WriteCache(_fileName, _cache);
            return item;
        }
    }
}