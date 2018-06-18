using System.Collections.Generic;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    public class Phone : ValueObject<Phone>
    {
        public string Number { get; set; }

        public enum Types
        {
            Unknown,
            Cell,
            Work,
            Home
        }

        public Types PhoneType { get; set; }

        public Phone()
        {
        }

        public Phone(string number, Types type = Types.Cell)
        {
            Number = number;

            PhoneType = type;
        }

        protected override IEnumerable<object> GetFieldsToCheckForEquality()
        {
            return new List<object>() { Number, PhoneType };
        }
    }
}