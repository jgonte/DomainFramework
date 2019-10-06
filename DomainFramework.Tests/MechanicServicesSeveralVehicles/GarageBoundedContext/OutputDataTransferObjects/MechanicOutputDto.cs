using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class MechanicOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<VehicleOutputDto> Vehicles { get; set; } = new List<VehicleOutputDto>();

    }
}