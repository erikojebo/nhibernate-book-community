using System;

namespace NHibernateBookCommunity.Domain.Entities
{
    public class ReviewComment : Entity
    {
        public virtual string Body { get; set; }
        public virtual DateTime DateTime { get; set; }
    }
}