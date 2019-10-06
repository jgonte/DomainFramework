using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CarOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public int Passengers { get; set; }

        public string Model { get; set; }

        public int MechanicId { get; set; }

        public List<DoorOutputDto> Doors { get; set; } = new List<DoorOutputDto>();

        public List<CylinderOutputDto> Cylinders { get; set; } = new List<CylinderOutputDto>();

    }
}