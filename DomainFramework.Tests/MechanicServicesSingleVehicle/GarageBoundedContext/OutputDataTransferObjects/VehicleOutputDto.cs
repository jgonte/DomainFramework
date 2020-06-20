using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class VehicleOutputDto : IOutputDataTransferObject
    {
        public int VehicleId { get; set; }

        public string Model { get; set; }

        public int? MechanicId { get; set; }

        public IEnumerable<CylinderOutputDto> Cylinders { get; set; }

    }
}