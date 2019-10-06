using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class TruckOutputDto : VehicleOutputDto
    {
        public int Weight { get; set; }

        public List<InspectionOutputDto> Inspections { get; set; } = new List<InspectionOutputDto>();

    }
}