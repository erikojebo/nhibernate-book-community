using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using NHibernateBookCommunity.Domain.Entities;

namespace NHibernateBookCommunity.Persistence.Infrastructure
{
    public class AutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.IsSubclassOf(typeof(Entity));
        }

        public override Access GetAccessStrategyForReadOnlyProperty(Member member)
        {
            return Access.CamelCaseField(CamelCasePrefix.Underscore);
        }
    }
}