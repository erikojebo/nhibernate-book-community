using System;
using System.Linq;
using NHibernate.Transform;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;

namespace NHibernateBookCommunity.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ClearDatabase();
            CreateTestData();

            PrintReviewCountsWithLazyLoading();
            PrintReviewCountsWithEagerLoading();
        }

        private static void PrintReviewCountsWithLazyLoading()
        {
            WriteConsoleSeparator();

            using (var session = SessionFactorySingleton.OpenSession())
            {
                var users = session.CreateQuery("from User").List<User>();

                foreach (var user in users)
                {
                    System.Console.WriteLine(user.Username + ": " + user.Reviews.Count());
                }
            }
        }

        private static void PrintReviewCountsWithEagerLoading()
        {
            WriteConsoleSeparator();

            using (var session = SessionFactorySingleton.OpenSession())
            {
                var users = session.CreateQuery("from User user join fetch user.Reviews")
                    .SetResultTransformer(Transformers.DistinctRootEntity)
                    .List<User>();

                foreach (var user in users)
                {
                    System.Console.WriteLine(user.Username + ": " + user.Reviews.Count());
                }
            }
        }

        private static void CreateTestData()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user1 = new User
                    {
                        Username = "User 1",
                        Password = "Password 1",
                        LastLoginDate = new DateTime(2011, 1, 2, 3, 4, 5),
                    };

                var user2 = new User
                    {
                        Username = "User 2",
                        Password = "Password 2",
                        LastLoginDate = new DateTime(2022, 2, 2, 3, 4, 5),
                    };

                var user3 = new User
                    {
                        Username = "User 3",
                        Password = "Password 3",
                        LastLoginDate = new DateTime(3033, 3, 3, 3, 4, 5),
                    };

                user1.AddReview(new Review
                    {
                        Title = "User 1's first review",
                        Body = "This is the first review written by User 1",
                        DateTime = new DateTime(2011, 2, 3, 4, 5, 6),
                        Rating = 5
                    });

                user1.AddReview(new Review
                    {
                        Title = "User 1's second review",
                        Body = "This is the second review written by User 1",
                        DateTime = new DateTime(2011, 2, 3, 6, 7, 8),
                        Rating = 4
                    });

                user2.AddReview(new Review
                    {
                        Title = "User 2's first review",
                        Body = "This is the first review written by User 2",
                        DateTime = new DateTime(2011, 2, 4, 5, 6, 7),
                        Rating = 5
                    });

                user2.AddReview(new Review
                    {
                        Title = "User 2's second review",
                        Body = "This is the second review written by User 2",
                        DateTime = new DateTime(2022, 2, 4, 6, 7, 8),
                        Rating = 4
                    });
                
                user2.AddReview(new Review
                    {
                        Title = "User 2's third review",
                        Body = "This is the third review written by User 2",
                        DateTime = new DateTime(2022, 2, 4, 7, 8, 9),
                        Rating = 3
                    });

                user3.AddReview(new Review
                    {
                        Title = "User 3's first review",
                        Body = "This is the first review written by User 3",
                        DateTime = new DateTime(3011, 3, 4, 5, 6, 7),
                        Rating = 1
                    });

                session.Save(user1);
                session.Save(user2);
                session.Save(user3);

                transaction.Commit();
            }
        }

        private static void ClearDatabase()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                session.CreateQuery("delete Review").ExecuteUpdate();
                session.CreateQuery("delete User").ExecuteUpdate();
            }
        }

        private static void WriteConsoleSeparator()
        {
            System.Console.WriteLine("\n\n\n");
            System.Console.WriteLine("-----------------------------");
            System.Console.WriteLine("\n\n\n");
        }
    }
}