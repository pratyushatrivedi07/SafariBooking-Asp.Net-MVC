using System;
using System.Collections.Generic;

namespace Jungle.Repos
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(object id);
        bool Add(T entity);
        bool Update(T entity);
        bool Remove(T entity);
    }
}
