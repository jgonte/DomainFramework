using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class DoorInputDto : IInputDataTransferObject
    {
        public int? Number { get; set; }

        public virtual void Validate(ValidationResult result)
        {
        }

    }
}