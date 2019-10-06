using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class DoorOutputDto : IOutputDataTransferObject
    {
        public int? Number { get; set; }

    }
}