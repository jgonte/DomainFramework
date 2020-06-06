using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TypeValueInputDto : IInputDataTransferObject
    {
        public TypeValue.DataTypes DataType { get; set; }

        public string Data { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            ((int)DataType).ValidateNotZero(result, nameof(DataType));

            Data.ValidateNotEmpty(result, nameof(Data));

            Data.ValidateMaxLength(result, nameof(Data), 200);
        }

    }
}