using System;

namespace NHibernateBookCommunity.Domain.Entities
{
    public class Review : Entity
    {
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual int Rating { get; set; }
        public DateTime DateTime { get; set; }
    }
}