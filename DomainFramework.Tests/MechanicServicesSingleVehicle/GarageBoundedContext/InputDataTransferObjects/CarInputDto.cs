using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CarInputDto : VehicleInputDto
    {
        public int CarId { get; set; }

        public int Passengers { get; set; }

        public List<DoorInputDto> Doors { get; set; } = new List<DoorInputDto>();

        public override void Validate(ValidationResult result)
        {
            base.Validate(result);

            Passengers.ValidateRequired(result, nameof(Passengers));

            var doorsCount = (uint)Doors.Where(item => item != null).Count();

            doorsCount.ValidateCountIsBetween(result, 2, 5, nameof(Doors));

            foreach (var door in Doors)
            {
                door.Validate(result);
            }
        }

    }
}