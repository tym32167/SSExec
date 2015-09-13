using System;
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
        private static List<T> _cache;

        private static DateTime CacheLoadTime = DateTime.MinValue;

        private List<T> Cache
        {
            get
            {
                if (_cache == null || (DateTime.UtcNow - CacheLoadTime).Seconds > 0)
                {
                    _cache = ReadCache(_fileName);
                    CacheLoadTime = DateTime.UtcNow;
                }
                return _cache;
            }
            set { _cache = value; }
        }

        public SingleFileRepository(string fileName)
        {
            _fileName = fileName;
            
        }

        private List<T> ReadCache(string filename)
        {
            if (!File.Exists(filename)) return new List<T>();

            using (var sr = new StreamReader(filename))
            {
                var ser = new XmlSerializer(typeof(List<T>));
                return (List<T>)ser.Deserialize(sr);
            }
        }

        private void WriteCache(string filename, List<T> cache)
        {
            if (File.Exists(filename)) File.Create(filename);

            using (var sw = new StreamWriter(filename, false))
            {
                var ser = new XmlSerializer(typeof(List<T>));
                ser.Serialize(sw, cache);
            }
        }


        public override IEnumerable<T> All()
        {
            return Cache;
        }

        public override void Delete(T item)
        {
            Cache = Cache.Where(x => !x.Id.Equals(item.Id)).ToList();
            WriteCache(_fileName, Cache);
        }

        public override T Update(T item)
        {
            var el = Cache.SingleOrDefault(x => x.Id.Equals(item.Id));
            if (el == null) return default(T);
            Cache.Remove(el);
            Cache.Add(item);
            WriteCache(_fileName, Cache);
            return item;
        }

        public override T Add(T item)
        {
            Cache = Cache.Where(x => !x.Id.Equals(item.Id)).ToList();
            Cache.Add(item);
            WriteCache(_fileName, Cache);
            return item;
        }
    }
}