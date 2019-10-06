using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    [TypeConverter(typeof(AssetTypeConverter))]
    public class Asset : ValueObject<Asset>
    {
        public int? Number { get; set; }

        public override bool IsEmpty() => 
            Number == default(int?);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                Number
            };

        public static bool TryParse(string s, out Asset result)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                result = new Asset
                {
                    Number = null
                };
            }

            result = new Asset
            {
                Number = int.Parse(s)
            };

            return true;
        }

    }
}