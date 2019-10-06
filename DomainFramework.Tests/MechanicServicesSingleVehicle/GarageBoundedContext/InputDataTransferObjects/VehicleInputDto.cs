using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class VehicleInputDto : IInputDataTransferObject
    {
        public string Model { get; set; }

        public int MechanicId { get; set; }

        public List<CylinderInputDto> Cylinders { get; set; } = new List<CylinderInputDto>();

        public void Validate(ValidationResult result)
        {
            Model.ValidateNotEmpty(result, nameof(Model));

            Model.ValidateMaxLength(result, nameof(Model), 50);

            MechanicId.ValidateNotZero(result, nameof(MechanicId));

            var cylindersCount = (uint)Cylinders.Where(item => item != null).Count();

            cylindersCount.ValidateCountIsBetween(result, 2, 12, nameof(Cylinders));

            foreach (var cylinder in Cylinders)
            {
                cylinder.Validate(result);
            }
        }

    }
}