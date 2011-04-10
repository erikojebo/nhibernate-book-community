using System;
using System.Collections.Generic;
using FluentNHibernate.Testing;
using NHibernateBookCommunity.Domain.Entities;
using NHibernateBookCommunity.Persistence.Infrastructure;
using NUnit.Framework;

namespace NHibernateBookCommunity.Persistence.Mapping
{
    [TestFixture]
    public class UserMappingSpecs
    {
        private List<Review> _reviews;

        [SetUp]
        public void SetUp()
        {
            _reviews = new List<Review>
                {
                    new Review
                        {
                            Title = "Review 1 title",
                            Body = "Review 1 body",
                            DateTime = new DateTime(2011, 1, 2, 3, 3, 3),
                            Rating = 3
                        },
                    new Review
                        {
                            Title = "Review 1 title",
                            Body = "Review 1 body",
                            DateTime = new DateTime(2011, 1, 2, 3, 3, 3),
                            Rating = 3
                        },
                };
        }

        [Test]
        public void Can_write_and_read_user()
        {
            using (var session = SessionFactorySingleton.OpenSession())
            {
                new PersistenceSpecification<User>(session)
                    .CheckProperty(x => x.Username, "username")
                    .CheckProperty(x => x.Password, "password")
                    .CheckProperty(x => x.LastLoginDate, new DateTime(2011, 1, 2, 3, 4, 5))
                    .CheckList(x => x.Reviews, _reviews, new EntityIdComparer(),
                        (u, r) => u.AddReview(r));
            }
        }
    }
}