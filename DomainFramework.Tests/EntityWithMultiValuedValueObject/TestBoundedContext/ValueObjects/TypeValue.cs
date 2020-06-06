using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TypeValue : ValueObject<TypeValue>
    {
        public enum DataTypes
        {
            Integer,
            String
        }

        /// <summary>
        /// The type of the data
        /// </summary>
        public DataTypes DataType { get; set; }

        /// <summary>
        /// The actual data
        /// </summary>
        public string Data { get; set; }

        public override bool IsEmpty() => 
            DataType == default(int) && Data == default(string);

        protected override IEnumerable<object> GetFieldsToCheckForEquality() => 
            new List<object>
            {
                DataType,
                Data
            };

    }
}