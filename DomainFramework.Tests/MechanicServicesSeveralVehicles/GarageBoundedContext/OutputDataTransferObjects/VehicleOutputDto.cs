using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class VehicleOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public int MechanicId { get; set; }

        public MechanicOutputDto Mechanic { get; set; }

        public List<CylinderOutputDto> Cylinders { get; set; } = new List<CylinderOutputDto>();

    }
}