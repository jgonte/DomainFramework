using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class AssetInputDto : IInputDataTransferObject
    {
        public int? Number { get; set; }

        public virtual void Validate(ValidationResult result)
        {
        }

    }
}