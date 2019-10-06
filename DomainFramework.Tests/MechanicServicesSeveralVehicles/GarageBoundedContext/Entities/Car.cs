using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class Car : Vehicle
    {
        public int Passengers { get; set; }

        public List<Door> Doors { get; set; }

    }
}