using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class TruckInputDto : VehicleInputDto
    {
        public int? TruckId { get; set; }

        public int Weight { get; set; }

        public List<InspectionInputDto> Inspections { get; set; } = new List<InspectionInputDto>();

        public override void Validate(ValidationResult result)
        {
            base.Validate(result);

            Weight.ValidateNotZero(result, nameof(Weight));

            foreach (var inspection in Inspections)
            {
                inspection.Validate(result);
            }
        }

    }
}