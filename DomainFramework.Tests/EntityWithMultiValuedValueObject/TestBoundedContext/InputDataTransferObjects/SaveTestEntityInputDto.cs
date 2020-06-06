using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class SaveTestEntityInputDto : IInputDataTransferObject
    {
        public int? TestEntityId { get; set; }

        public string Text { get; set; }

        public List<TypeValueInputDto> TypeValues1 { get; set; } = new List<TypeValueInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            Text.ValidateNotEmpty(result, nameof(Text));

            Text.ValidateMaxLength(result, nameof(Text), 50);

            foreach (var typeValue in TypeValues1)
            {
                typeValue.Validate(result);
            }
        }

    }
}