using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class PhoneNumber : ValueObject<PhoneNumber>
    {
        /// <summary>
        /// The area code
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// The exchange
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// The number
        /// </summary>
        public string Number { get; set; }

        public override bool IsEmpty() => 
            AreaCode == default(string) && Exchange == default(string) && Number == default(string);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                AreaCode,
                Exchange,
                Number
            };

    }
}