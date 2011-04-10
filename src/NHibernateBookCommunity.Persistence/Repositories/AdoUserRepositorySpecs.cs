using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;
using NUnit.Framework;

namespace NHibernateBookCommunity.Persistence.Repositories
{
    [TestFixture]
    public class AdoUserRepositorySpecs
    {
        private User _user1;
        private AdoUserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            ClearDatabase();

            _user1 = new User
            {
                Username = "User 1",
                Password = "Password 1",
                LastLoginDate = new DateTime(2011, 1, 2, 3, 4, 5),
            };

            _user1.AddReview(new Review
            {
                Title = "Review 1 title",
                Body = "Review 1 body",
                DateTime = new DateTime(2011, 2, 3, 4, 5, 6),
                Rating = 5
            });

            _user1.AddReview(new Review
            {
                Title = "Review 2 title",
                Body = "Review 2 body",
                DateTime = new DateTime(2011, 2, 3, 6, 7, 8),
                Rating = 4
            });

            _repository = new AdoUserRepository();
        }

        [Test]
        public void User_can_be_written_to_database_and_read_back_again()
        {
            _repository.Insert(_user1);

            var actual = _repository.Get(_user1.Id);

            Assert.AreEqual(_user1.Id, actual.Id);
            Assert.AreEqual("User 1", actual.Username);
            Assert.AreEqual("Password 1", actual.Password);
            Assert.AreEqual(new DateTime(2011, 1, 2, 3, 4, 5), actual.LastLoginDate);
        }

        [Test]
        public void User_with_reviews_can_be_written_to_database_and_read_back_again()
        {
            _repository.Insert(_user1);

            var actual = _repository.GetWithReviews(_user1.Id);

            Assert.AreEqual(_user1.Id, actual.Id);
            Assert.AreEqual("User 1", actual.Username);
            Assert.AreEqual("Password 1", actual.Password);
            Assert.AreEqual(new DateTime(2011, 1, 2, 3, 4, 5), actual.LastLoginDate);

            var actualReviews = actual.Reviews.OrderBy(x => x.Title);
            var firstReview = actualReviews.FirstOrDefault();
            var lastReview = actualReviews.LastOrDefault();
            
            Assert.AreEqual(2, actualReviews.Count());
            
            Assert.AreEqual("Review 1 title", firstReview.Title);
            Assert.AreEqual("Review 1 body", firstReview.Body);
            Assert.AreEqual(new DateTime(2011, 2, 3, 4, 5, 6), firstReview.DateTime);
            Assert.AreEqual(5, firstReview.Rating);

            Assert.AreEqual("Review 2 title", lastReview.Title);
            Assert.AreEqual("Review 2 body", lastReview.Body);
            Assert.AreEqual(new DateTime(2011, 2, 3, 6, 7, 8), lastReview.DateTime);
            Assert.AreEqual(4, lastReview.Rating);
        }

        [Test]
        public void Can_delete_user()
        {
            _repository.Insert(_user1);
            _repository.Delete(_user1);
            Assert.IsNull(_repository.Get(_user1.Id));
        }

        private void ClearDatabase()
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