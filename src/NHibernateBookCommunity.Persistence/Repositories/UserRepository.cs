using System.Collections.Generic;
using NHibernate.Transform;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;

namespace NHibernateBookCommunity.Persistence.Repositories
{
    public class UserRepository
    {
        public User Get(int id)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.Get<User>(id);
            }
        }

        public IList<User> GetAll()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.CreateCriteria<T>().List<User>();
            }
        }

        public void Delete(User entity)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete((object)entity);

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

        public void Save(User entity)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SaveOrUpdate(entity);

                transaction.Commit();
            }
        }

        public void Save(IEnumerable<User> entities)
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

        public IList<User> GetUsersWithReviewsWithRating(int rating)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.CreateQuery(
                    "select u from User u join u.Reviews r where r.Rating = :rating")
                    .SetInt32("rating", rating)
                    .SetResultTransformer(Transformers.DistinctRootEntity)
                    .List<User>();
            }
        }

        public IList<User> GetUsersWithReviewsWithRating_QueryOver(int rating)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.QueryOver<User>()
                    .JoinQueryOver(u => u.Reviews)
                    .Where(r => r.Rating == rating)
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .List<User>();
            }
        }

        public long GetTotalReviewCount()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.CreateQuery("select count(r.Id) from Review r")
                    .UniqueResult<long>();
            }
        }

        public long GetReviewCountForUser(int userId)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.CreateQuery(
                    "select count(review.Id) from User user left join user.Reviews review " +
                    "where user.Id = :userId")
                    .SetInt32("userId", userId)
                    .UniqueResult<long>();
            }
        }
    }
}