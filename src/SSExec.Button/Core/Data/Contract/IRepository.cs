using System;
using System.Collections.Generic;

namespace SSExec.Button.Core.Data.Contract
{
    public interface IRepository<T, in TKey> : IDisposable where T : IEntity<TKey>
    {
        int Count();
        int Count(Func<T, bool> filter);
        int Count<TK>(int page, int pagesize);

        IEnumerable<T> All();
        IEnumerable<T> All(Func<T, bool> filter);
        IEnumerable<T> All<TK>(int page, int pagesize, Func<T, TK> orderBy, bool asc = true);
        IEnumerable<T> All<TK>(int page, int pagesize, Func<T, TK> orderBy, Func<T, bool> filter, bool asc = true);

        T Get(Func<T, bool> filter);
        T Get(TKey key);

        void Delete(T item);
        T Update(T item);
        T Add(T item);
    }
}