using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class InspectionOutputDto : IOutputDataTransferObject
    {
        public DateTime? Date { get; set; }

    }
}