using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class CarOutputDto : VehicleOutputDto
    {
        public int Passengers { get; set; }

        public List<DoorOutputDto> Doors { get; set; } = new List<DoorOutputDto>();

    }
}