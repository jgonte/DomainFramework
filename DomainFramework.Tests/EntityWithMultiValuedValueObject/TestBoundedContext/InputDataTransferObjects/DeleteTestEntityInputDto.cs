using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class DeleteTestEntityInputDto : IInputDataTransferObject
    {
        public int TestEntityId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            TestEntityId.ValidateRequired(result, nameof(TestEntityId));
        }

    }
}