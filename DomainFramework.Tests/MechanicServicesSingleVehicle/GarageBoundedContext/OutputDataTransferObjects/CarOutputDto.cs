using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CarOutputDto : VehicleOutputDto
    {
        public int Passengers { get; set; }

        public IEnumerable<DoorOutputDto> Doors { get; set; }

    }
}