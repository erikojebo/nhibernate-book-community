using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateBookCommunity.Domain.Entities;

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
                    var cfg = new Configuration();

                    //cfg.AddClass(typeof(User));
                    //cfg.AddClass(typeof(Review));

                    cfg.Configure();
                 
                    cfg.AddAssembly(Assembly.GetExecutingAssembly());
                    cfg.AddAssembly(typeof(User).Assembly);

                    var export = new SchemaExport(cfg);
                    export.Drop(true, true);
                    export.Create(true, true);

                    _factory = cfg.BuildSessionFactory();
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