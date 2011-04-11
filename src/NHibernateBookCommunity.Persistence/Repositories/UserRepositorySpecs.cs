using System;
using System.Linq;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;
using NUnit.Framework;

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
            DataDeleter.ClearDatabase();

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
                    LastLoginDate = new DateTime(2011, 2, 2, 3, 4, 5),
                };

            _user3 = new User
                {
                    Username = "User 3",
                    Password = "Password 3",
                    LastLoginDate = new DateTime(2011, 3, 3, 3, 4, 5),
                };

            _user4 = new User
                {
                    Username = "User 4",
                    Password = "Password 4",
                    LastLoginDate = new DateTime(2011, 4, 4, 4, 4, 5),
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
        public void Get_returns_the_user_with_the_given_id()
        {
            var actual = _repository.Get(_user1.Id);

            Assert.AreEqual(_user1.Id, actual.Id);
            Assert.AreEqual("User 1", actual.Username);
        }

        [Test]
        public void Get_returns_null_for_non_existing_id()
        {
            var actual = _repository.Get(-1);
            Assert.IsNull(actual);
        }

        [Test]
        public void GetAll_returns_all_saved_users()
        {
            var actual = _repository.GetAll()
                .OrderBy(x => x.Username);

            Assert.AreEqual(4, actual.Count());
            Assert.AreEqual("User 1", actual.ElementAt(0).Username);
            Assert.AreEqual("User 2", actual.ElementAt(1).Username);
            Assert.AreEqual("User 3", actual.ElementAt(2).Username);
            Assert.AreEqual("User 4", actual.ElementAt(3).Username);
        }

        [Test]
        public void Save_writes_the_user_to_the_database()
        {
            var user = new User
                {
                    Username = "new user", 
                    LastLoginDate = new DateTime(2011, 5, 5, 5, 5, 5)
                };

            _repository.Save(user);

            var actual = _repository.Get(user.Id);

            Assert.AreEqual("new user", actual.Username);
        }

        [Test]
        public void Save_updates_an_existing_user()
        {
            _user1.Username = "new username";

            _repository.Save(_user1);

            var actual = _repository.Get(_user1.Id);

            Assert.AreEqual("new username", actual.Username);
        }

        [Test]
        public void Save_updates_all_given_users()
        {
            _user1.Username = "new username";
            _user2.Username = "new username 2";

            var entities = new [] { _user1, _user2 };

            _repository.Save(entities);

            var actual1 = _repository.Get(_user1.Id);
            var actual2 = _repository.Get(_user2.Id);

            Assert.AreEqual("new username", actual1.Username);
            Assert.AreEqual("new username 2", actual2.Username);
        }

        [Test]
        public void Delete_by_user_removes_user_from_the_database()
        {
            _repository.Delete(_user1);

            var actual = _repository.Get(_user1.Id);
            
            Assert.IsNull(actual);
        }
        
        [Test]
        public void Delete_by_user_id_removes_user_from_the_database()
        {
            _repository.Delete(_user1.Id);

            var actual = _repository.Get(_user1.Id);

            Assert.IsNull(actual);
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
    }
}