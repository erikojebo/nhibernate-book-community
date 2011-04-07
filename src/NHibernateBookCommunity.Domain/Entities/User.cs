using System;
using System.Collections.Generic;

namespace NHibernateBookCommunity.Domain.Entities
{
    public class User : Entity
    {
        public User()
        {
            Reviews = new List<Review>();
            StatusUpdates = new List<StatusUpdate>();
        }

        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastLoginDate { get; set; }

        public virtual IList<Review> Reviews { get; private set; }
        public virtual IList<StatusUpdate> StatusUpdates { get; private set; }

        public virtual void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        public virtual void AddStatusUpdate(StatusUpdate statusUpdate)
        {
            StatusUpdates.Add(statusUpdate);
        }
    }
}