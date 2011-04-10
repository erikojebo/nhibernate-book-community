using System;
using NHibernate;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;
using NUnit.Framework;
using System.Linq;

namespace NHibernateBookCommunity.Persistence.Repositories
{
    [TestFixture]
    public class UserRepositorySpecs
    {
        private User _user1;
        private User _user2;
        private User _user3;
        private UserRepository _repository;
        private User _user4;

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

            _user2 = new User
                {
                    Username = "User 2",
                    Password = "Password 2",
                    LastLoginDate = new DateTime(2022, 2, 2, 3, 4, 5),
                };

            _user3 = new User
                {
                    Username = "User 3",
                    Password = "Password 3",
                    LastLoginDate = new DateTime(3033, 3, 3, 3, 4, 5),
                };

            _user4 = new User
                {
                    Username = "User 3",
                    Password = "Password 3",
                    LastLoginDate = new DateTime(3033, 3, 3, 3, 4, 5),
                };

            _user1.AddReview(new Review
                {
                    Title = "User 1's first review",
                    Body = "This is the first review written by User 1",
                    DateTime = new DateTime(2011, 2, 3, 4, 5, 6),
                    Rating = 5
                });

            _user1.AddReview(new Review
                {
                    Title = "User 1's second review",
                    Body = "This is the second review written by User 1",
                    DateTime = new DateTime(2011, 2, 3, 6, 7, 8),
                    Rating = 4
                });

            _user2.AddReview(new Review
                {
                    Title = "User 2's first review",
                    Body = "This is the first review written by User 2",
                    DateTime = new DateTime(2011, 2, 4, 5, 6, 7),
                    Rating = 3
                });

            _user3.AddReview(new Review
                {
                    Title = "User 3's first review",
                    Body = "This is the first review written by User 3",
                    DateTime = new DateTime(2011, 2, 5, 6, 7, 8),
                    Rating = 5
                });

            _user3.AddReview(new Review
                {
                    Title = "User 3's second review",
                    Body = "This is the second review written by User 3",
                    DateTime = new DateTime(2011, 2, 5, 7, 8, 9),
                    Rating = 5
                });

            SaveAllUsers();

            _repository = new UserRepository();
        }

        [Test]
        public void GetTotalReviewCount_returns_total_number_of_reviews_for_all_users()
        {
            var actualReviewCount = _repository.GetTotalReviewCount();

            Assert.AreEqual(5, actualReviewCount);
        }

        [Test]
        public void GetReviewCountForUser_returns_zero_for_user_without_reviews()
        {
            var actualReviewCount = _repository.GetReviewCountForUser(_user4.Id);

            Assert.AreEqual(0, actualReviewCount);
        }

        [Test]
        public void GetReviewCountForUser_returns_number_of_reviews_for_user_with_given_id()
        {
            var actualReviewCount = _repository.GetReviewCountForUser(_user1.Id);

            Assert.AreEqual(2, actualReviewCount);
        }

        [Test]
        public void GetUsersWithReviewsWithRating_returns_all_users_with_atleast_one_review_with_the_given_rating()
        {
            var actualUsers = _repository.GetUsersWithReviewsWithRating(5);

            Assert.AreEqual(2, actualUsers.Count);
            Assert.AreEqual(1, actualUsers.Count(x => x.Id == _user1.Id));
            Assert.AreEqual(1, actualUsers.Count(x => x.Id == _user3.Id));
        }
        
        [Test]
        public void GetUsersWithReviewsWithRating_QueryOver_returns_all_users_with_atleast_one_review_with_the_given_rating()
        {
            var actualUsers = _repository.GetUsersWithReviewsWithRating(5);

            Assert.AreEqual(2, actualUsers.Count);
            Assert.AreEqual(1, actualUsers.Count(x => x.Id == _user1.Id));
            Assert.AreEqual(1, actualUsers.Count(x => x.Id == _user3.Id));
        }

        private void SaveAllUsers()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(_user1);
                session.Save(_user2);
                session.Save(_user3);
                session.Save(_user4);

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
    }
}