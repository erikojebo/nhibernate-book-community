using System;
using System.Collections;
using NHibernateBookCommunity.Domain.Entities;

namespace NHibernateBookCommunity.Persistence.Mapping
{
    public class EntityIdComparer : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            var left = x as Entity;
            var right = y as Entity;

            if (left != null && right != null)
            {
                return left.Id == right.Id;
            }

            return object.Equals(x, y);
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}