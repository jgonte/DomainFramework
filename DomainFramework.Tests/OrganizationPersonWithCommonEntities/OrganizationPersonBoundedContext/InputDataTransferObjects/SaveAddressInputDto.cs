using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class SaveAddressInputDto : IInputDataTransferObject
    {
        public int AddressId { get; set; }

        public string Street { get; set; }

        public int? OrganizationId { get; set; }

        public int? PersonId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Street.ValidateRequired(result, nameof(Street));

            Street.ValidateMaxLength(result, nameof(Street), 25);
        }

    }
}