using System;
using System.Collections.Generic;

namespace NHibernateBookCommunity.Domain.Entities
{
    public class User : Entity
    {
        private readonly IList<Review> _reviews = new List<Review>();

        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime LastLoginDate { get; set; }

        public virtual IEnumerable<Review> Reviews
        {
            get { return _reviews; }
        }

        public virtual void AddReview(Review review)
        {
            _reviews.Add(review);
        }

        public override string ToString()
        {
            return "Username: " + Username;
        }
    }
}