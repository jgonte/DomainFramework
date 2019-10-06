using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class InspectionInputDto : IInputDataTransferObject
    {
        public DateTime? Date { get; set; }

        public void Validate(ValidationResult result)
        {
        }

    }
}