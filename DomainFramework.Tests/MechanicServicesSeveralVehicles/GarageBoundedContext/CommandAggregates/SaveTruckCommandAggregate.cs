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
            RegisterCommandRepositoryFactory<Truck>(() => new TruckCommandRepository());

            RegisterCommandRepositoryFactory<Vehicle>(() => new VehicleCommandRepository());

            RegisterCommandRepositoryFactory<Vehicle_Cylinders_CommandRepository.RepositoryKey>(() => new Vehicle_Cylinders_CommandRepository());

            RegisterCommandRepositoryFactory<Truck_Inspections_CommandRepository.RepositoryKey>(() => new Truck_Inspections_CommandRepository());

            var mechanicDependency = (Mechanic)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Truck
            {
                Id = truck.TruckId,
                Weight = truck.Weight,
                Model = truck.Model,
                MechanicId = (mechanicDependency != null) ? mechanicDependency.Id : truck.MechanicId
            };

            Enqueue(new SaveEntityCommandOperation<Truck>(RootEntity, dependencies));

            Enqueue(new DeleteLinkedValueObjectCommandOperation<Truck, Cylinder, Vehicle_Cylinders_CommandRepository.RepositoryKey>(RootEntity));

            foreach (var dto in truck.Cylinders)
            {
                var cylinderValueObject = new Cylinder
                {
                    Diameter = dto.Diameter
                };

                Enqueue(new AddLinkedValueObjectCommandOperation<Truck, Cylinder, Vehicle_Cylinders_CommandRepository.RepositoryKey>(RootEntity, cylinderValueObject));
            }

            Enqueue(new DeleteLinkedValueObjectCommandOperation<Truck, Inspection, Truck_Inspections_CommandRepository.RepositoryKey>(RootEntity));

            foreach (var dto in truck.Inspections)
            {
                var inspectionValueObject = new Inspection
                {
                    Date = dto.Date
                };

                Enqueue(new AddLinkedValueObjectCommandOperation<Truck, Inspection, Truck_Inspections_CommandRepository.RepositoryKey>(RootEntity, inspectionValueObject));
            }
        }

    }
}