using System;
using System.Data;
using System.Data.SqlClient;
using NHibernateBookCommunity.Domain.Entities;

namespace NHibernateBookCommunity.Persistence.Repositories
{
    public class AdoUserRepository
    {
        public const string ConnectionString = "data source=.\\SQLEXPRESS;initial catalog=nhibernate_book_community;Integrated Security=SSPI;";

        public void Insert(User user)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "insert into [User] (Username, Password, LastLoginDate) " +
                        "values (@username, @password, @lastLoginDate);" +
                        "select SCOPE_IDENTITY()";

                    var usernameParameter = command.CreateParameter();
                    usernameParameter.DbType = DbType.String;
                    usernameParameter.ParameterName = "username";
                    usernameParameter.Value = user.Username;

                    var passwordParameter = command.CreateParameter();
                    passwordParameter.DbType = DbType.String;
                    passwordParameter.ParameterName = "password";
                    passwordParameter.Value = user.Password;

                    var loginDateParameter = command.CreateParameter();
                    loginDateParameter.DbType = DbType.DateTime;
                    loginDateParameter.ParameterName = "lastLoginDate";
                    loginDateParameter.Value = user.LastLoginDate;

                    command.Parameters.Add(usernameParameter);
                    command.Parameters.Add(passwordParameter);
                    command.Parameters.Add(loginDateParameter);

                    var id = Convert.ToInt32(command.ExecuteScalar());

                    user.Id = id;
                }

                foreach (var review in user.Reviews)
                {
                    InsertReview(connection, user, review);
                }
            }
        }

        private void InsertReview(IDbConnection connection, User user, Review review)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    "insert into [Review] (UserId, Title, Body, Rating, DateTime) " +
                    "values (@userId, @title, @body, @rating, @dateTime);" +
                    "select SCOPE_IDENTITY()";

                var title = command.CreateParameter();
                title.DbType = DbType.String;
                title.ParameterName = "title";
                title.Value = review.Title;

                var body = command.CreateParameter();
                body.DbType = DbType.String;
                body.ParameterName = "body";
                body.Value = review.Body;

                var dateTime = command.CreateParameter();
                dateTime.DbType = DbType.DateTime;
                dateTime.ParameterName = "dateTime";
                dateTime.Value = review.DateTime;
                        
                var rating = command.CreateParameter();
                rating.DbType = DbType.Int32;
                rating.ParameterName = "rating";
                rating.Value = review.Rating;
                        
                var userId = command.CreateParameter();
                userId.DbType = DbType.Int32;
                userId.ParameterName = "userId";
                userId.Value = user.Id;

                command.Parameters.Add(title);
                command.Parameters.Add(body);
                command.Parameters.Add(dateTime);
                command.Parameters.Add(rating);
                command.Parameters.Add(userId);

                var id = Convert.ToInt32(command.ExecuteScalar());
                review.Id = id;
            }
        }

        public void Update(User user)
        {
            // Orka ...
        }

        public void Delete(User user)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            using (IDbCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = 
                    "delete from Review where UserId = @id;"+
                    "delete from [User] where [User].Id = @id";

                var parameter = command.CreateParameter();
                parameter.DbType = DbType.Int32;
                parameter.ParameterName = "id";
                parameter.Value = user.Id;

                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
            }
        }

        public User Get(int id)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            using (IDbCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText =
                    "select [User].Id as UserId, [User].Username, [User].Password, [User].LastLoginDate from [User]";

                using (var reader = command.ExecuteReader())
                {
                    User user = null;

                    if (reader.Read())
                    {
                        user = ReadUser(reader);
                    }

                    return user;
                }
            }
        }

        public User GetWithReviews(int userId)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            using (IDbCommand command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText =
                    "select [User].Id as UserId, [User].Username, [User].Password, [User].LastLoginDate, " +
                    "Review.Id as ReviewId, Review.DateTime, Review.Title, Review.Body, Review.Rating " +
                    "from [User] left outer join Review on [User].Id = Review.UserId " +
                    "where [User].Id = @userId";

                var userIdParameter = command.CreateParameter();
                userIdParameter.DbType = DbType.Int32;
                userIdParameter.ParameterName = "userId";
                userIdParameter.Value = userId;

                command.Parameters.Add(userIdParameter);

                using (var reader = command.ExecuteReader())
                {
                    User user = null;

                    while (reader.Read())
                    {
                        if (user == null)
                        {
                            user = ReadUser(reader);
                        }

                        var review = new Review
                            {
                                Id = (int)reader["ReviewId"],
                                Title = (string)reader["Title"],
                                Body = (string)reader["Body"],
                                DateTime = (DateTime)reader["DateTime"],
                                Rating = (int)reader["Rating"]
                            };

                        user.AddReview(review);
                    }

                    return user;
                }
            }
        }

        private User ReadUser(IDataReader reader)
        {
            return new User
                {
                    Id = (int)reader["UserId"],
                    Username = (string)reader["Username"],
                    Password = (string)reader["Password"],
                    LastLoginDate = (DateTime)reader["LastLoginDate"],
                };
        }
    }
}