using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class MechanicOutputDto : IOutputDataTransferObject
    {
        public int MechanicId { get; set; }

        public string Name { get; set; }

        public IEnumerable<VehicleOutputDto> Vehicles { get; set; }

    }
}