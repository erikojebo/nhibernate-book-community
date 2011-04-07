using System;

namespace NHibernateBookCommunity.Domain.Entities
{
    public class StatusUpdate : Entity
    {
        public virtual string Message { get; set; }
        public virtual DateTime DateTime { get; set; }
    }
}