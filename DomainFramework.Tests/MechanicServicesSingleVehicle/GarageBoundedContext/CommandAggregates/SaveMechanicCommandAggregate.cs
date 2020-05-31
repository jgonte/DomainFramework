using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class SaveMechanicCommandAggregate : CommandAggregate<Mechanic>
    {
        public SaveMechanicCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName()))
        {
        }

        public SaveMechanicCommandAggregate(SaveMechanicInputDto mechanic, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName()))
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

            Enqueue(new DeleteLinksCommandOperation<Mechanic>(RootEntity, "UnlinkVehicleFromMechanic"));

            if (mechanic.Vehicle != null)
            {
                ILinkedAggregateCommandOperation operation;

                var vehicle = mechanic.Vehicle;

                if (vehicle is CarInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Mechanic, SaveCarCommandAggregate, CarInputDto>(
                        RootEntity,
                        (CarInputDto)vehicle,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "Vehicle"
                            }
                        }
                    );

                    Enqueue(operation);
                }
                else if (vehicle is TruckInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Mechanic, SaveTruckCommandAggregate, TruckInputDto>(
                        RootEntity,
                        (TruckInputDto)vehicle,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "Vehicle"
                            }
                        }
                    );

                    Enqueue(operation);
                }
                else if (vehicle is VehicleInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Mechanic, SaveVehicleCommandAggregate, VehicleInputDto>(
                        RootEntity,
                        (VehicleInputDto)vehicle,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "Vehicle"
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