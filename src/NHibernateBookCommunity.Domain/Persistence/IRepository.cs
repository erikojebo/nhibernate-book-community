using System.Collections.Generic;

namespace NHibernateBookCommunity.Domain.Persistence
{
    public interface IRepository<T>
    {
        T Get(int id);
        IList<T> GetAll();
        void Delete(T entity);
        void Save(T entity);
        void Save(IEnumerable<T> entities);
    }
}