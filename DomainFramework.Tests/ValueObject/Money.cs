using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class Money : ValueObject<Money>
    {
        public decimal Value { get; private set; }

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
            return new List<object>() { Value };
        }
    }
}
