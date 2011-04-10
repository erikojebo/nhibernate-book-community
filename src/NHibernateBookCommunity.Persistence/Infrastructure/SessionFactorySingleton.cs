using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Mapping.Conventions;
using NHibernateBookCommunity.Persistence.Mapping.Overrides;

namespace NHibernateBookCommunity.Persistence.Infrastructure
{
    public class SessionFactorySingleton
    {
        private static ISessionFactory _factory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = Fluently.Configure()
                        .Database(
                            MsSqlConfiguration.MsSql2005.ConnectionString(
                                x => x.Server(@".\SQLEXPRESS")
                                    .Database("nhibernate_book_community")
                                         .TrustedConnection())
                                .ShowSql())
                        .Mappings(m => m.AutoMappings.Add(
                            AutoMap.Assemblies(
                                new AutoMappingConfiguration(),
                                typeof(User).Assembly)
                                           .UseOverridesFromAssemblyOf<ReviewMappingOverride>()
                                           .Conventions.AddFromAssemblyOf<ForeignKeyColumnNameConvention>()
                                           )
                                           .ExportTo(@"c:\temp\mappings"))
                        .ExposeConfiguration(cfg =>
                            {
                                var export = new SchemaExport(cfg);
                                export.Drop(true, true);
                                export.Create(true, true);
                            })
                        .BuildSessionFactory();
                }

                return _factory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}