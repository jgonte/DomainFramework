using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class MechanicOutputDto : IOutputDataTransferObject
    {
        public int MechanicId { get; set; }

        public string Name { get; set; }

        public VehicleOutputDto Vehicle { get; set; }

    }
}