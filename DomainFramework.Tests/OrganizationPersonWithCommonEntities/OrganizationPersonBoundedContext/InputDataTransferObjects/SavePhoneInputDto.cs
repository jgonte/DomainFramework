using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class SavePhoneInputDto : IInputDataTransferObject
    {
        public int? PhoneId { get; set; }

        public string Number { get; set; }

        public int? OrganizationId { get; set; }

        public int? PersonId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Number.ValidateNotEmpty(result, nameof(Number));

            Number.ValidateMaxLength(result, nameof(Number), 15);
        }

    }
}