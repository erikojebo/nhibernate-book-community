using System.Data;
using System.Data.SqlClient;
using NHibernateBookCommunity.Persistence.Repositories;

namespace NHibernateBookCommunity.Persistence.Infrastructure
{
    public class DataDeleter
    {
        public static void ClearDatabase()
        {
            using (IDbConnection connection = new SqlConnection(AdoUserRepository.ConnectionString))
            using (IDbCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "delete from Review";
                command.ExecuteNonQuery();

                command.CommandText = "delete from [User]";
                command.ExecuteNonQuery();
            }
        }
    }
}