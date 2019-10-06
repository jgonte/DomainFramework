using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class SaveMechanicCommandAggregate : CommandAggregate<Mechanic>
    {
        public SaveMechanicCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
        }

        public SaveMechanicCommandAggregate(SaveMechanicInputDto mechanic) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
            Initialize(mechanic);
        }

        public override void Initialize(IInputDataTransferObject mechanic, EntityDependency[] dependencies)
        {
            Initialize((SaveMechanicInputDto)mechanic);
        }

        private void Initialize(SaveMechanicInputDto mechanic)
        {
            RegisterCommandRepositoryFactory<Mechanic>(() => new MechanicCommandRepository());

            RegisterCommandRepositoryFactory<Vehicle>(() => new VehicleCommandRepository());

            RootEntity = new Mechanic
            {
                Id = mechanic.Id,
                Name = mechanic.Name
            };

            Enqueue(new SaveEntityCommandOperation<Mechanic>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<Mechanic>(RootEntity, "Vehicles"));

            if (mechanic.Vehicles?.Any() == true)
            {
                foreach (var vehicle in mechanic.Vehicles)
                {
                    if (vehicle is CarInputDto)
                    {
                        Enqueue(
                            new AddLinkedAggregateCommandOperation<Mechanic, SaveCarCommandAggregate, CarInputDto>(
                                RootEntity, 
                                (CarInputDto)vehicle, 
                                new EntityDependency[] 
                                {
                                    new EntityDependency
                                    {
                                        Entity = RootEntity
                                    }
                                })
                        );
                    }
                    //Enqueue(new AddLinkedEntityCommandOperation<Mechanic, Vehicle>(RootEntity, () => new Vehicle
                    //{
                    //    Model = vehicle.Model,
                    //    Cylinders = vehicle.Cylinders.Select(dto => new Cylinder
                    //    {
                    //        Diameter = dto.Diameter
                    //    }).ToList()
                    //}, "Vehicles"));
                }
            }
        }

    }
}