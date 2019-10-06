using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class InspectionOutputDto : IOutputDataTransferObject
    {
        public DateTime? Date { get; set; }

    }
}