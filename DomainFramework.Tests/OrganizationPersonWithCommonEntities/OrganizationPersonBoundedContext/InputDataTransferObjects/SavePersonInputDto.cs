using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class SavePersonInputDto : IInputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public List<SavePhoneInputDto> Phones { get; set; } = new List<SavePhoneInputDto>();

        public SaveAddressInputDto Address { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            var phonesCount = (uint)Phones.Where(item => item != null).Count();

            phonesCount.ValidateCountIsEqualOrGreaterThan(result, 1, nameof(Phones));

            foreach (var phone in Phones)
            {
                phone.Validate(result);
            }

            Address.Validate(result);
        }

    }
}