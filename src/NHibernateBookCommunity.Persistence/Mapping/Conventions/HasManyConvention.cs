using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace NHibernateBookCommunity.Persistence.Mapping.Conventions
{
    public class HasManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.All();
        }
    }
}