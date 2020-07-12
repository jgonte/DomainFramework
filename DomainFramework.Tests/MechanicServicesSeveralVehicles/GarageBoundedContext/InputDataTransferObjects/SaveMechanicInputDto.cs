using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class SaveMechanicInputDto : IInputDataTransferObject
    {
        public int MechanicId { get; set; }

        public string Name { get; set; }

        public List<VehicleInputDto> Vehicles { get; set; } = new List<VehicleInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            foreach (var vehicle in Vehicles)
            {
                vehicle.Validate(result);
            }
        }

    }
}