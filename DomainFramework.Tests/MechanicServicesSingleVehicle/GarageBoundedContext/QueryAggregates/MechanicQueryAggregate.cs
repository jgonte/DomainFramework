using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class MechanicQueryAggregate : GetByIdQueryAggregate<Mechanic, int?, MechanicOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<Vehicle> GetVehicleQueryOperation { get; }

        public Vehicle Vehicle => GetVehicleQueryOperation.LinkedEntity;

        public MechanicQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName());

            MechanicQueryRepository.Register(context);

            VehicleQueryRepository.Register(context);

            RepositoryContext = context;

            GetVehicleQueryOperation = new GetSingleLinkedEntityQueryOperation<Vehicle>
            {
                GetLinkedEntity = (repository, entity, user) => ((VehicleQueryRepository)repository).GetVehicleForMechanic(RootEntity.Id, user),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((VehicleQueryRepository)repository).GetVehicleForMechanicAsync(RootEntity.Id, user)
            };

            QueryOperations.Enqueue(GetVehicleQueryOperation);
        }

        public VehicleOutputDto GetVehicleDto()
        {
            if (Vehicle != null)
            {
                var dto = new VehicleOutputDto
                {
                    Id = Vehicle.Id.Value,
                    Model = Vehicle.Model,
                    MechanicId = Vehicle.MechanicId
                };

                return dto;
            }

            return null;
        }

        public override void PopulateDto(Mechanic entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Name = entity.Name;

            OutputDto.Vehicle = GetVehicleDto();
        }

    }
}