using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class AddTypeValues1InputDto : IInputDataTransferObject
    {
        public int TestEntityId { get; set; }

        public List<TypeValueInputDto> TypeValues1 { get; set; } = new List<TypeValueInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            TestEntityId.ValidateNotZero(result, nameof(TestEntityId));

            foreach (var typeValue in TypeValues1)
            {
                typeValue.Validate(result);
            }
        }

    }
}