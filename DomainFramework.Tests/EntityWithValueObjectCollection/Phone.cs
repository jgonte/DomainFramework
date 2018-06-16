using System.Collections.Generic;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class Phone : ValueObject<Phone>
    {
        public string Number { get; private set; }

        public enum Types
        {
            Unknown,
            Cell,
            Work,
            Home
        }

        public Types Type { get; private set; }

        public Phone(string number, Types type = Types.Cell)
        {
            Number = number;

            Type = type;
        }

        protected override IEnumerable<object> GetFieldsToCheckForEquality()
        {
            return new List<object>() { Number, Type };
        }
    }
}