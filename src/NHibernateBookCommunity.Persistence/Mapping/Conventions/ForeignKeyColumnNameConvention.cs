using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace NHibernateBookCommunity.Persistence.Mapping.Conventions
{
    public class ForeignKeyColumnNameConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            return type.Name + "Id";
        }
    }
}