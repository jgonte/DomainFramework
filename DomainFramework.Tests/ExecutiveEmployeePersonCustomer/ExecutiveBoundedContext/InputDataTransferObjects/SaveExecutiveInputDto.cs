using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class SaveExecutiveInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public decimal Bonus { get; set; }

        public DateTime HireDate { get; set; }

        public string Name { get; set; }

        public AssetInputDto Asset { get; set; }

        public void Validate(ValidationResult result)
        {
            HireDate.ValidateNotEmpty(result, nameof(HireDate));

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Asset.ValidateNotNull(result, nameof(Asset));

            Asset?.Validate(result);
        }

    }
}