using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    public class GetCarByIdQueryAggregate : GetByIdQueryAggregate<Car, int?, CarOutputDto>
    {
        public GetCollectionLinkedValueObjectQueryOperation<Car, Cylinder, Vehicle_Cylinders_QueryRepository.RepositoryKey> GetCylindersOperation { get; private set; }

        public GetCollectionLinkedValueObjectQueryOperation<Car, Door, Car_Doors_QueryRepository.RepositoryKey> GetDoorsOperation { get; private set; }

        public GetCarByIdQueryAggregate() : this(null)
        {
        }

        public GetCarByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSingleVehicleConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            CarQueryRepository.Register(context);

            Vehicle_Cylinders_QueryRepository.Register(context);

            Car_Doors_QueryRepository.Register(context);

            GetCylindersOperation = new GetCollectionLinkedValueObjectQueryOperation<Car, Cylinder, Vehicle_Cylinders_QueryRepository.RepositoryKey>
            {
                GetLinkedValueObjects = (repository, entity, user) => ((Vehicle_Cylinders_QueryRepository)repository).GetAll(RootEntity.Id).ToList(),
                GetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((Vehicle_Cylinders_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    return items.ToList();
                }
            };

            QueryOperations.Enqueue(GetCylindersOperation);

            GetDoorsOperation = new GetCollectionLinkedValueObjectQueryOperation<Car, Door, Car_Doors_QueryRepository.RepositoryKey>
            {
                GetLinkedValueObjects = (repository, entity, user) => ((Car_Doors_QueryRepository)repository).GetAll(RootEntity.Id).ToList(),
                GetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((Car_Doors_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    return items.ToList();
                }
            };

            QueryOperations.Enqueue(GetDoorsOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Passengers = RootEntity.Passengers;

            OutputDto.Model = RootEntity.Model;

            OutputDto.MechanicId = RootEntity.MechanicId;

            OutputDto.Cylinders = GetCylindersDtos();

            OutputDto.Doors = GetDoorsDtos();
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

        public List<DoorOutputDto> GetDoorsDtos()
        {
            return GetDoorsOperation
                .LinkedValueObjects
                .Select(vo => new DoorOutputDto
                {
                    Number = vo.Number
                })
                .ToList();
        }

    }
}