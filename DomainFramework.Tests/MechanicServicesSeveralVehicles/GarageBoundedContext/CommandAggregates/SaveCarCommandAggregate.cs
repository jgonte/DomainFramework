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
            RegisterCommandRepositoryFactory<Vehicle>(() => new VehicleCommandRepository());

            RegisterCommandRepositoryFactory<Car>(() => new CarCommandRepository());

            RootEntity = new Car
            {
                Id = car.Id,
                Passengers = car.Passengers,
                Model = car.Model,
                MechanicId = car.MechanicId,
                Doors = car.Doors.Select(dto => new Door
                {
                    Number = dto.Number
                }).ToList(),
                Cylinders = car.Cylinders.Select(dto => new Cylinder
                {
                    Diameter = dto.Diameter
                }).ToList()
            };

            Enqueue(new SaveEntityCommandOperation<Car>(RootEntity));
        }

    }
}