using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class MechanicQueryAggregate : GetByIdQueryAggregate<Mechanic, int?, MechanicOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<int?, Vehicle, VehicleOutputDto> GetVehicleLinkedAggregateQueryOperation { get; set; }

        public MechanicQueryAggregate() : this(null)
        {
        }

        public MechanicQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            MechanicQueryRepository.Register(context);

            VehicleQueryRepository.Register(context);

            GetVehicleLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<int?, Vehicle, VehicleOutputDto>
            {
                OnBeforeExecute = entity =>
                {
                    if (ProcessedEntities.Contains(("Vehicle", entity)))
                    {
                        return false;
                    }

                    ProcessedEntities.Add(("Vehicle", entity));

                    return true;
                },
                GetLinkedEntity = (repository, entity, user) => ((VehicleQueryRepository)repository).GetVehicleForMechanic(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((VehicleQueryRepository)repository).GetVehicleForMechanicAsync(RootEntity.Id),
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is Car)
                    {
                        return new GetCarByIdQueryAggregate();
                    }
                    else if (entity is Truck)
                    {
                        return new GetTruckByIdQueryAggregate();
                    }
                    else if (entity is Vehicle)
                    {
                        return new GetVehicleByIdQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetVehicleLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.MechanicId = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Vehicle = GetVehicleLinkedAggregateQueryOperation.OutputDto;
        }

    }
}