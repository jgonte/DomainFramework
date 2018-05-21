using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class Money : ValueObject<Money>
    {
        protected readonly decimal Value;

        public Money() : this(0m)
        {
        }

        public Money(decimal value)
        {
            Value = value;
        }

        public Money Add(Money money)
        {
            return new Money(Value + money.Value);
        }

        public Money Subtract(Money money)
        {
            return new Money(Value - money.Value);
        }

        protected override IEnumerable<object> GetFieldsToCheckForEquality()
        {
            return new List<Object>() { Value };
        }
    }
}
