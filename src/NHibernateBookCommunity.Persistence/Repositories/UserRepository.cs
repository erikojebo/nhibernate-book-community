using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;

namespace NHibernateBookCommunity.Persistence.Repositories
{
    public class UserRepository : Repository<User>
    {
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

        public User GetByLogin(string username, string password)
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                return session.QueryOver<User>()
                    .Where(x => x.Username == username && x.Password == password)
                    .FutureValue<User>().Value;

            }
        }
    }
}