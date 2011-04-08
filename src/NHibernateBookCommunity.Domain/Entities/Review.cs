using System;
using System.Collections.Generic;

namespace NHibernateBookCommunity.Domain.Entities
{
    public class Review : Entity
    {
        public Review()
        {
            Comments = new List<ReviewComment>();
        }

        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual int Rating { get; set; }
        public virtual DateTime DateTime { get; set; }

        public virtual IList<ReviewComment> Comments { get; set; }

        public virtual void AddComment(ReviewComment comment)
        {
            Comments.Add(comment);
        }
    }
}