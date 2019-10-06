using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class VehicleQueryAggregate : GetByIdQueryAggregate<Vehicle, int?, VehicleOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<Mechanic> GetMechanicQueryOperation { get; }

        public Mechanic Mechanic => GetMechanicQueryOperation.LinkedEntity;

        public SetCollectionLinkedValueObjectQueryOperation<Vehicle, Vehicle_Cylinders_QueryRepository.RepositoryKey> Cylinders { get; private set; }

        public VehicleQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName());

            VehicleQueryRepository.Register(context);

            MechanicQueryRepository.Register(context);

            Vehicle_Cylinders_QueryRepository.Register(context);

            RepositoryContext = context;

            GetMechanicQueryOperation = new GetSingleLinkedEntityQueryOperation<Mechanic>
            {
                GetLinkedEntity = (repository, entity, user) => ((MechanicQueryRepository)repository).GetMechanicForVehicle(RootEntity.Id, user),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((MechanicQueryRepository)repository).GetMechanicForVehicleAsync(RootEntity.Id, user)
            };

            QueryOperations.Enqueue(GetMechanicQueryOperation);

            Cylinders = new SetCollectionLinkedValueObjectQueryOperation<Vehicle, Vehicle_Cylinders_QueryRepository.RepositoryKey>
            {
                SetLinkedValueObjects = (repository, entity, user) => entity.Cylinders = ((Vehicle_Cylinders_QueryRepository)repository)
                    .Get(RootEntity.Id, null)
                    .ToList(),
                SetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((Vehicle_Cylinders_QueryRepository)repository).GetAsync(RootEntity.Id, null);

                    entity.Cylinders = items.ToList();
                }
            };

            QueryOperations.Enqueue(Cylinders);
        }

        public MechanicOutputDto GetMechanicDto()
        {
            if (Mechanic != null)
            {
                var dto = new MechanicOutputDto
                {
                    Id = Mechanic.Id.Value,
                    Name = Mechanic.Name
                };

                return dto;
            }

            return null;
        }

        public List<CylinderOutputDto> GetCylindersDtos(Vehicle vehicle)
        {
            return vehicle
                .Cylinders
                .Select(vo => new CylinderOutputDto
                {
                    Diameter = vo.Diameter
                })
                .ToList();
        }

        public override void PopulateDto(Vehicle entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Model = entity.Model;

            OutputDto.MechanicId = entity.MechanicId;

            OutputDto.Cylinders = GetCylindersDtos(entity);

            OutputDto.Mechanic = GetMechanicDto();
        }

    }
}