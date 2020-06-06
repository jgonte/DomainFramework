using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class SaveVehicleCommandAggregate : CommandAggregate<Vehicle>
    {
        public SaveVehicleCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
        }

        public SaveVehicleCommandAggregate(VehicleInputDto vehicle, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
            Initialize(vehicle, dependencies);
        }

        public override void Initialize(IInputDataTransferObject vehicle, EntityDependency[] dependencies)
        {
            Initialize((VehicleInputDto)vehicle, dependencies);
        }

        private void Initialize(VehicleInputDto vehicle, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Vehicle>(() => new VehicleCommandRepository());

            RegisterCommandRepositoryFactory<Vehicle_Cylinders_CommandRepository.RepositoryKey>(() => new Vehicle_Cylinders_CommandRepository());

            var mechanicDependency = (Mechanic)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Vehicle
            {
                Id = vehicle.VehicleId,
                Model = vehicle.Model,
                MechanicId = (mechanicDependency != null) ? mechanicDependency.Id : vehicle.MechanicId
            };

            Enqueue(new SaveEntityCommandOperation<Vehicle>(RootEntity, dependencies));

            Enqueue(new DeleteLinkedValueObjectCommandOperation<Vehicle, Cylinder, Vehicle_Cylinders_CommandRepository.RepositoryKey>(RootEntity));

            foreach (var dto in vehicle.Cylinders)
            {
                var cylinderValueObject = new Cylinder
                {
                    Diameter = dto.Diameter
                };

                Enqueue(new AddLinkedValueObjectCommandOperation<Vehicle, Cylinder, Vehicle_Cylinders_CommandRepository.RepositoryKey>(RootEntity, cylinderValueObject));
            }
        }

    }
}