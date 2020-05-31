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

        public SaveMechanicCommandAggregate(SaveMechanicInputDto mechanic, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
            Initialize(mechanic, dependencies);
        }

        public override void Initialize(IInputDataTransferObject mechanic, EntityDependency[] dependencies)
        {
            Initialize((SaveMechanicInputDto)mechanic, dependencies);
        }

        private void Initialize(SaveMechanicInputDto mechanic, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Mechanic>(() => new MechanicCommandRepository());

            RootEntity = new Mechanic
            {
                Id = mechanic.MechanicId,
                Name = mechanic.Name
            };

            Enqueue(new SaveEntityCommandOperation<Mechanic>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Mechanic>(RootEntity, "UnlinkVehiclesFromMechanic"));

            if (mechanic.Vehicles?.Any() == true)
            {
                foreach (var dto in mechanic.Vehicles)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is CarInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Mechanic, SaveCarCommandAggregate, CarInputDto>(
                            RootEntity,
                            (CarInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Vehicles"
                                }
                            }
                        );

                        Enqueue(operation);
                    }
                    else if (dto is TruckInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Mechanic, SaveTruckCommandAggregate, TruckInputDto>(
                            RootEntity,
                            (TruckInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Vehicles"
                                }
                            }
                        );

                        Enqueue(operation);
                    }
                    else if (dto is VehicleInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Mechanic, SaveVehicleCommandAggregate, VehicleInputDto>(
                            RootEntity,
                            (VehicleInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Vehicles"
                                }
                            }
                        );

                        Enqueue(operation);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

    }
}