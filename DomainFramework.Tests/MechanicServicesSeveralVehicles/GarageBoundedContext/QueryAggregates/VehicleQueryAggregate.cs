using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    public class VehicleQueryAggregate : GetByIdQueryAggregate<Vehicle, int?, VehicleOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<Mechanic> GetMechanicQueryOperation { get; }

        public Mechanic Mechanic => GetMechanicQueryOperation.LinkedEntity;

        public SetCollectionLinkedValueObjectQueryOperation<Vehicle, Vehicle_Cylinders_QueryRepository.RepositoryKey> Cylinders { get; private set; }

        public VehicleQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(MechanicServicesSeveralVehiclesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            VehicleQueryRepository.Register(context);

            MechanicQueryRepository.Register(context);

            Vehicle_Cylinders_QueryRepository.Register(context);

            GetMechanicQueryOperation = new GetSingleLinkedEntityQueryOperation<Mechanic>
            {
                GetLinkedEntity = (repository, entity, user) => ((MechanicQueryRepository)repository).GetMechanicForVehicle(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((MechanicQueryRepository)repository).GetMechanicForVehicleAsync(RootEntity.Id)
            };

            QueryOperations.Enqueue(GetMechanicQueryOperation);

            Cylinders = new SetCollectionLinkedValueObjectQueryOperation<Vehicle, Vehicle_Cylinders_QueryRepository.RepositoryKey>
            {
                SetLinkedValueObjects = (repository, entity, user) => entity.Cylinders = ((Vehicle_Cylinders_QueryRepository)repository)
                    .GetAll(RootEntity.Id)
                    .ToList(),
                SetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((Vehicle_Cylinders_QueryRepository)repository).GetAllAsync(RootEntity.Id);

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

        public List<InspectionOutputDto> GetInspectionsDtos(Truck truck)
        {
            return truck
                .Inspections
                .Select(vo => new InspectionOutputDto
                {
                    Date = vo.Date
                })
                .ToList();
        }

        public List<DoorOutputDto> GetDoorsDtos(Car car)
        {
            return car
                .Doors
                .Select(vo => new DoorOutputDto
                {
                    Number = vo.Number
                })
                .ToList();
        }

        public override void PopulateDto(Vehicle entity)
        {
            if (entity is Car)
            {
                var car = (Car)entity;

                var carDto = new CarOutputDto();

                carDto.Id = car.Id.Value;

                carDto.Passengers = car.Passengers;

                carDto.Model = car.Model;

                carDto.MechanicId = car.MechanicId;

                carDto.Doors = GetDoorsDtos(car);

                carDto.Cylinders = GetCylindersDtos(car);

                OutputDto = carDto;
            }
            else if (entity is Truck)
            {
                var truck = (Truck)entity;

                var truckDto = new TruckOutputDto();

                truckDto.Id = truck.Id.Value;

                truckDto.Weight = truck.Weight;

                truckDto.Model = truck.Model;

                truckDto.MechanicId = truck.MechanicId;

                truckDto.Inspections = GetInspectionsDtos(truck);

                truckDto.Cylinders = GetCylindersDtos(truck);

                OutputDto = truckDto;
            }
            else
            {
                var vehicleDto = new VehicleOutputDto();

                vehicleDto.Id = entity.Id.Value;

                vehicleDto.Model = entity.Model;

                vehicleDto.MechanicId = entity.MechanicId;

                vehicleDto.Cylinders = GetCylindersDtos(entity);

                OutputDto = vehicleDto;
            }

            OutputDto.Mechanic = GetMechanicDto();
        }

    }
}