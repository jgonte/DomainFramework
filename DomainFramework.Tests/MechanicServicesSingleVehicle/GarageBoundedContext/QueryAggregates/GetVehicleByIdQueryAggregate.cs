using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class GetVehicleByIdQueryAggregate : GetByIdQueryAggregate<Vehicle, int?, VehicleOutputDto>
    {
        public GetCollectionLinkedValueObjectQueryOperation<Vehicle, Cylinder, Vehicle_Cylinders_QueryRepository.RepositoryKey> GetCylindersOperation { get; private set; }

        public GetVehicleByIdQueryAggregate() : this(null)
        {
        }

        public GetVehicleByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            VehicleQueryRepository.Register(context);

            Vehicle_Cylinders_QueryRepository.Register(context);

            GetCylindersOperation = new GetCollectionLinkedValueObjectQueryOperation<Vehicle, Cylinder, Vehicle_Cylinders_QueryRepository.RepositoryKey>
            {
                GetLinkedValueObjects = (repository, entity, user) => ((Vehicle_Cylinders_QueryRepository)repository).GetAll(RootEntity.Id).ToList(),
                GetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((Vehicle_Cylinders_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    return items.ToList();
                }
            };

            QueryOperations.Enqueue(GetCylindersOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.VehicleId = RootEntity.Id.Value;

            OutputDto.Model = RootEntity.Model;

            OutputDto.MechanicId = RootEntity.MechanicId;

            OutputDto.Cylinders = GetCylindersDtos();
        }

        public List<CylinderOutputDto> GetCylindersDtos()
        {
            return GetCylindersOperation
                .LinkedValueObjects
                .Select(vo => new CylinderOutputDto
                {
                    Diameter = vo.Diameter
                })
                .ToList();
        }

    }
}