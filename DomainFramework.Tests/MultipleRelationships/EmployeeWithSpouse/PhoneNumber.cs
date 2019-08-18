using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests.EmployeeWithSpouse
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

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                AreaCode,
                Exchange,
                Number
            };

        public override bool IsEmpty() => 
            AreaCode == default(string) && Exchange == default(string) && Number == default(string);
    }

}