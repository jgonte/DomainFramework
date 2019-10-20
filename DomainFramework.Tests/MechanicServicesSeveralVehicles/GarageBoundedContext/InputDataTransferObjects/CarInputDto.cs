using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class CarInputDto : VehicleInputDto
    {
        public int? Id { get; set; }

        public int Passengers { get; set; }

        public string Model { get; set; }

        public int MechanicId { get; set; }

        public List<DoorInputDto> Doors { get; set; } = new List<DoorInputDto>();

        public List<CylinderInputDto> Cylinders { get; set; } = new List<CylinderInputDto>();

        public void Validate(ValidationResult result)
        {
            Passengers.ValidateNotZero(result, nameof(Passengers));

            Model.ValidateNotEmpty(result, nameof(Model));

            Model.ValidateMaxLength(result, nameof(Model), 50);

            MechanicId.ValidateNotZero(result, nameof(MechanicId));

            var doorsCount = (uint)Doors.Where(item => item != null).Count();

            doorsCount.ValidateCountIsBetween(result, 2, 5, nameof(Doors));

            foreach (var door in Doors)
            {
                door.Validate(result);
            }

            var cylindersCount = (uint)Cylinders.Where(item => item != null).Count();

            cylindersCount.ValidateCountIsBetween(result, 2, 12, nameof(Cylinders));

            foreach (var cylinder in Cylinders)
            {
                cylinder.Validate(result);
            }
        }

    }
}