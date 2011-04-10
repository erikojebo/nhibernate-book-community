using System;
using System.Collections.Generic;

namespace NHibernateBookCommunity.Domain.Entities
{
    public class User : Entity
    {
        private IList<Review> _reviews = new List<Review>();
        private IList<StatusUpdate> _statusUpdates = new List<StatusUpdate>();

        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime LastLoginDate { get; set; }

        public virtual IList<Review> Reviews
        {
            get { return _reviews; }
        }

        public virtual IList<StatusUpdate> StatusUpdates
        {
            get { return _statusUpdates; }
        }

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