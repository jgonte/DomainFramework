using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class TruckOutputDto : VehicleOutputDto
    {
        public int Weight { get; set; }

        public IEnumerable<InspectionOutputDto> Inspections { get; set; }

    }
}