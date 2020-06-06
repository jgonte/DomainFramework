using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class GetTruckByIdQueryAggregate : GetByIdQueryAggregate<Truck, int?, TruckOutputDto>
    {
        public GetCollectionLinkedValueObjectQueryOperation<Truck, Cylinder, Vehicle_Cylinders_QueryRepository.RepositoryKey> GetCylindersOperation { get; private set; }

        public GetCollectionLinkedValueObjectQueryOperation<Truck, Inspection, Truck_Inspections_QueryRepository.RepositoryKey> GetInspectionsOperation { get; private set; }

        public GetTruckByIdQueryAggregate() : this(null)
        {
        }

        public GetTruckByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            TruckQueryRepository.Register(context);

            Vehicle_Cylinders_QueryRepository.Register(context);

            Truck_Inspections_QueryRepository.Register(context);

            GetCylindersOperation = new GetCollectionLinkedValueObjectQueryOperation<Truck, Cylinder, Vehicle_Cylinders_QueryRepository.RepositoryKey>
            {
                GetLinkedValueObjects = (repository, entity, user) => ((Vehicle_Cylinders_QueryRepository)repository).GetAll(RootEntity.Id).ToList(),
                GetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((Vehicle_Cylinders_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    return items.ToList();
                }
            };

            QueryOperations.Enqueue(GetCylindersOperation);

            GetInspectionsOperation = new GetCollectionLinkedValueObjectQueryOperation<Truck, Inspection, Truck_Inspections_QueryRepository.RepositoryKey>
            {
                GetLinkedValueObjects = (repository, entity, user) => ((Truck_Inspections_QueryRepository)repository).GetAll(RootEntity.Id).ToList(),
                GetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((Truck_Inspections_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    return items.ToList();
                }
            };

            QueryOperations.Enqueue(GetInspectionsOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Weight = RootEntity.Weight;

            OutputDto.Model = RootEntity.Model;

            OutputDto.MechanicId = RootEntity.MechanicId;

            OutputDto.Cylinders = GetCylindersDtos();

            OutputDto.Inspections = GetInspectionsDtos();
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

        public List<InspectionOutputDto> GetInspectionsDtos()
        {
            return GetInspectionsOperation
                .LinkedValueObjects
                .Select(vo => new InspectionOutputDto
                {
                    Date = vo.Date
                })
                .ToList();
        }

    }
}