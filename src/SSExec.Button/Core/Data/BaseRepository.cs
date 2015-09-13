using System;
using System.Collections.Generic;
using System.Linq;
using SSExec.Button.Core.Data.Contract;

namespace SSExec.Button.Core.Data
{
    public abstract class BaseRepository<T, TKey> : DisposeObject, IRepository<T, TKey> where T : IEntity<TKey>, new()
    {
        public abstract IEnumerable<T> All();
        public abstract void Delete(T item);
        public abstract T Update(T item);
        public abstract T Add(T item);

        public int Count()
        {
            return All().Count();
        }

        public int Count(Func<T, bool> filter)
        {
            return All(filter).Count();
        }

        public int Count<TK>(int page, int pagesize)
        {
            return All(page, pagesize, x => x.Id).Count();
        }

        public IEnumerable<T> All(Func<T, bool> filter)
        {
            return All().Where(filter);
        }

        public IEnumerable<T> All<TK>(int page, int pagesize, Func<T, TK> orderBy, bool asc = true)
        {
            var skip = page * pagesize;
            if (asc)
                return All().OrderBy(orderBy).Skip(skip).Take(pagesize);
            return All().OrderByDescending(orderBy).Skip(skip).Take(pagesize);
        }

        public IEnumerable<T> All<TK>(int page, int pagesize, Func<T, TK> orderBy, Func<T, bool> filter, bool asc = true)
        {
            var skip = page * pagesize;
            if (asc)
                return All(filter).OrderBy(orderBy).Skip(skip).Take(pagesize);
            return All(filter).OrderByDescending(orderBy).Skip(skip).Take(pagesize);
        }

        public T Get(Func<T, bool> filter)
        {
            return All(filter).FirstOrDefault();
        }

        public T Get(TKey key)
        {
            return All(x => Equals(x.Id, key)).FirstOrDefault();
        }
    }
}