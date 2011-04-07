using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Domain.Persistence;

namespace NHibernateBookCommunity.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository {}
}