using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class SaveCarCommandAggregate : CommandAggregate<Car>
    {
        public SaveCarCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
        }

        public SaveCarCommandAggregate(CarInputDto car, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
            Initialize(car, dependencies);
        }

        public override void Initialize(IInputDataTransferObject car, EntityDependency[] dependencies)
        {
            Initialize((CarInputDto)car, dependencies);
        }

        private void Initialize(CarInputDto car, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Car>(() => new CarCommandRepository());

            RegisterCommandRepositoryFactory<Vehicle>(() => new VehicleCommandRepository());

            RegisterCommandRepositoryFactory<Vehicle_Cylinders_CommandRepository.RepositoryKey>(() => new Vehicle_Cylinders_CommandRepository());

            RegisterCommandRepositoryFactory<Car_Doors_CommandRepository.RepositoryKey>(() => new Car_Doors_CommandRepository());

            var mechanicDependency = (Mechanic)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Car
            {
                Id = car.CarId,
                Passengers = car.Passengers,
                Model = car.Model,
                MechanicId = (mechanicDependency != null) ? mechanicDependency.Id : car.MechanicId
            };

            Enqueue(new SaveEntityCommandOperation<Car>(RootEntity, dependencies));

            Enqueue(new DeleteLinkedValueObjectCommandOperation<Car, Cylinder, Vehicle_Cylinders_CommandRepository.RepositoryKey>(RootEntity));

            foreach (var dto in car.Cylinders)
            {
                var cylinderValueObject = new Cylinder
                {
                    Diameter = dto.Diameter
                };

                Enqueue(new AddLinkedValueObjectCommandOperation<Car, Cylinder, Vehicle_Cylinders_CommandRepository.RepositoryKey>(RootEntity, cylinderValueObject));
            }

            Enqueue(new DeleteLinkedValueObjectCommandOperation<Car, Door, Car_Doors_CommandRepository.RepositoryKey>(RootEntity));

            foreach (var dto in car.Doors)
            {
                var doorValueObject = new Door
                {
                    Number = dto.Number
                };

                Enqueue(new AddLinkedValueObjectCommandOperation<Car, Door, Car_Doors_CommandRepository.RepositoryKey>(RootEntity, doorValueObject));
            }
        }

    }
}