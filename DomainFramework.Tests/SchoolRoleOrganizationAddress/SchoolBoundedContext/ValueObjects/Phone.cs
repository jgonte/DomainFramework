using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    [TypeConverter(typeof(PhoneTypeConverter))]
    public class Phone : ValueObject<Phone>
    {
        /// <summary>
        /// The phone number
        /// </summary>
        public string Number { get; set; }

        public override bool IsEmpty() => 
            Number == default(string);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Number
            };

        public static bool TryParse(string s, out Phone result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = new Phone
                {
                    Number = null
                };
            }

            result = new Phone
            {
                Number = s
            };

            return true;
        }

    }
}