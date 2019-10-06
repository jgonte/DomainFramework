using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class CylinderOutputDto : IOutputDataTransferObject
    {
        public int? Diameter { get; set; }

    }
}