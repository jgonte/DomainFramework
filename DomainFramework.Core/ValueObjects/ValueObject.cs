using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueObject<T> : IValueObject, IEquatable<T>
        where T : ValueObject<T>
    {
        public override bool Equals(object other)
        {
            return Equals(other as T);
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }

            return GetFieldsToCheckForEquality().SequenceEqual(other.GetFieldsToCheckForEquality());
        }

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            int hash = 17;

            foreach (var obj in GetFieldsToCheckForEquality())
            {
                hash = hash * 59 + (obj == null ? 0 : obj.GetHashCode());
            }

            return hash;
        }

        protected abstract IEnumerable<object> GetFieldsToCheckForEquality();

    }
}
