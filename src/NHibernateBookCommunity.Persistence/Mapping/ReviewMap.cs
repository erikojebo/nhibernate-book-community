using FluentNHibernate.Mapping;
using NHibernateBookCommunity.Domain.Entities;

namespace NHibernateBookCommunity.Persistence.Mapping
{
    public class ReviewMap : ClassMap<Review>
    {
        public ReviewMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.Body).Length(4001);
            Map(x => x.DateTime);
            Map(x => x.Rating);
        }
    }
}