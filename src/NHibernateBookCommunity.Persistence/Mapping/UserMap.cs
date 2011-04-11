using FluentNHibernate.Mapping;
using NHibernateBookCommunity.Domain.Entities;

namespace NHibernateBookCommunity.Persistence.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);

            Map(x => x.Username);
            Map(x => x.Password);
            Map(x => x.LastLoginDate);

            HasMany(x => x.Reviews);
        }
    }
}