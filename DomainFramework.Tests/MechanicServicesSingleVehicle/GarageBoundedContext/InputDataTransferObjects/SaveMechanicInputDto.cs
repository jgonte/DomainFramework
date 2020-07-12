using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class SaveMechanicInputDto : IInputDataTransferObject
    {
        public int MechanicId { get; set; }

        public string Name { get; set; }

        public VehicleInputDto Vehicle { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Vehicle?.Validate(result);
        }

    }
}