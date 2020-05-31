using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class CylinderInputDto : IInputDataTransferObject
    {
        public int? Diameter { get; set; }

        public virtual void Validate(ValidationResult result)
        {
        }

    }
}