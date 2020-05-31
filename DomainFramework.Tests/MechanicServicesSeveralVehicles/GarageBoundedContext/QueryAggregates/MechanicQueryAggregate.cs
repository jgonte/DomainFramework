using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class MechanicQueryAggregate : GetByIdQueryAggregate<Mechanic, int?, MechanicOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Vehicle, VehicleOutputDto> GetAllVehiclesLinkedAggregateQueryOperation { get; set; }

        public MechanicQueryAggregate() : this(null)
        {
        }

        public MechanicQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            MechanicQueryRepository.Register(context);

            VehicleQueryRepository.Register(context);

            GetAllVehiclesLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Vehicle, VehicleOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((VehicleQueryRepository)repository).GetAllVehiclesForMechanic(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((VehicleQueryRepository)repository).GetAllVehiclesForMechanicAsync(RootEntity.Id);

                    return entities.ToList();
                },
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

            QueryOperations.Enqueue(GetAllVehiclesLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Vehicles = GetAllVehiclesLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}