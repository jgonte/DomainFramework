using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class TruckOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public int Weight { get; set; }

        public string Model { get; set; }

        public int MechanicId { get; set; }

        public List<InspectionOutputDto> Inspections { get; set; } = new List<InspectionOutputDto>();

        public List<CylinderOutputDto> Cylinders { get; set; } = new List<CylinderOutputDto>();

    }
}