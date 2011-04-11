using System.Collections.Generic;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;

namespace NHibernateBookCommunity.Persistence.Repositories
{
    public class Repository<T> where T : class
    {
        public T Get(int id)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public IList<T> GetAll()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.CreateCriteria<T>().List<T>();
            }
        }

        public void Delete(T entity)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(entity);

                transaction.Commit();
            }
        }
        
        public void Delete(int id)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var proxy = session.Load<User>(id);
                session.Delete(proxy);

                transaction.Commit();
            }
        }

        public void Save(T entity)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(entity);

                transaction.Commit();
            }
        }

        public void Save(IEnumerable<T> entities)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                foreach (var entity in entities)
                {
                    session.SaveOrUpdate(entity);
                }

                transaction.Commit();
            }
        }
    }
}