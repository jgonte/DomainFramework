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

            Enqueue(new SaveEntityCommandOperation<Car>(RootEntity, dependencies));

            //.OnAfterCommandExecuted(cmd =>
            // {
            //     var query = (CollectionQuery<TestEntity>)cmd;

            //     foreach (var entity in query.Data)
            //     {
            //         var repository = new TestEntity_TypeValues1_QueryRepository();

            //         entity.TypeValues1 = repository.GetAll(entity.Id).ToList();
            //     }
            // })

            //.OnAfterCommandExecutedAsync(async cmd =>
            // {
            //     var query = (SingleQuery<TestEntity>)cmd;

            //     var entity = query.Data;

            //     if (entity == null)
            //     {
            //         return;
            //     }

            //     var repository = new TestEntity_TypeValues1_QueryRepository();

            //     var valueObjects = await repository.GetAllAsync(entity.Id);

            //     entity.TypeValues1 = valueObjects.ToList();
            // })
        }

    }
}