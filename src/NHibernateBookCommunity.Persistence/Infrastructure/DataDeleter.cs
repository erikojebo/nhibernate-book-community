namespace NHibernateBookCommunity.Persistence.Infrastructure
{
    public class DataDeleter
    {
        public static void ClearDatabase()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                session.CreateQuery("delete Review").ExecuteUpdate();
                session.CreateQuery("delete User").ExecuteUpdate();
            }
        }
    }
}