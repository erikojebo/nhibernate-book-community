using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using NHibernateBookCommunity.Domain.Entities;

namespace NHibernateBookCommunity.Persistence.Mapping.Overrides
{
    public class ReviewMappingOverride : IAutoMappingOverride<Review>
    {
        public void Override(AutoMapping<Review> mapping)
        {
            mapping.Map(x => x.Body).Length(4001);
        }
    }
}