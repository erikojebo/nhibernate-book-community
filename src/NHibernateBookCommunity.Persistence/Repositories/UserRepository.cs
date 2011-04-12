using System;
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
            throw new NotImplementedException();
        }

        public IList<User> GetAll()
        {
            throw new NotImplementedException();

        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();

        }

        public void Delete(int id)
        {
            throw new NotImplementedException();

        }

        public void Save(User entity)
        {
            throw new NotImplementedException();

        }

        public void Save(IEnumerable<User> entities)
        {
            throw new NotImplementedException();

        }

        public IList<User> GetUsersWithReviewsWithRating(int rating)
        {
            throw new NotImplementedException();

        }

        public IList<User> GetUsersWithReviewsWithRating_QueryOver(int rating)
        {
            throw new NotImplementedException();
        }

        public long GetTotalReviewCount()
        {
            throw new NotImplementedException();
        }

        public User GetByLogin(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}