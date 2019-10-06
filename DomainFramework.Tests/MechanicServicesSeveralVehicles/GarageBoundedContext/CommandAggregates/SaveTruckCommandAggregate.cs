using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class SaveTruckCommandAggregate : CommandAggregate<Truck>
    {
        public SaveTruckCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
        }

        public SaveTruckCommandAggregate(TruckInputDto truck, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
            Initialize(truck, dependencies);
        }

        public override void Initialize(IInputDataTransferObject truck, EntityDependency[] dependencies)
        {
            Initialize((TruckInputDto)truck, dependencies);
        }

        private void Initialize(TruckInputDto truck, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Vehicle>(() => new VehicleCommandRepository());

            RegisterCommandRepositoryFactory<Truck>(() => new TruckCommandRepository());

            RootEntity = new Truck
            {
                Id = truck.Id,
                Weight = truck.Weight,
                Model = truck.Model,
                MechanicId = truck.MechanicId,
                Inspections = truck.Inspections.Select(dto => new Inspection
                {
                    Date = dto.Date
                }).ToList(),
                Cylinders = truck.Cylinders.Select(dto => new Cylinder
                {
                    Diameter = dto.Diameter
                }).ToList()
            };

            Enqueue(new SaveEntityCommandOperation<Truck>(RootEntity));
        }

    }
}